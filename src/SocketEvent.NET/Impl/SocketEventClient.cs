using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SocketIOClient;
using SocketEvent.Dto;
using Newtonsoft.Json;
using SocketIOClient.Messages;
using AutoMapper;

namespace SocketEvent.Impl
{
    class SocketEventClient : ISocketEventClient
    {
        public const string SUBSCRIBE = "subscribe";

        public const string UNSUBSCRIBE = "unsubscribe";

        public const string ENQUEUE = "enqueue";

        public event EventHandler<StateChangedEventArgs> StateChanged;

        public event EventHandler Disconnected;

        public event EventHandler Retried;

        public event EventHandler Connected;

        private Client socket;

        public SocketEventClient(string url)
            : this(Guid.NewGuid().ToString(), url)
        {
        }

        public SocketEventClient(string id, string url)
        {
            this.ClientId = id;
            this.State = ClientState.Disconnected;
            this.Url = url;
        }

        public string ClientId { get; set; }

        public ClientState State { get; set; }

        public string Url { get; set; }

        private Client Socket
        {
            get
            {
                if (this.socket == null)
                {
                    this.socket = new Client(this.Url);
                    this.socket.Connect();
                    this.socket.SocketConnectionClosed += new EventHandler(SocketConnectionClosed);
                    this.socket.ConnectionRetryAttempt += new EventHandler(ConnectionRetryAttempt);
                    this.State = ClientState.Connected;
                }

                return this.socket;
            }
        }

        public void Subscribe(string eventName, Func<ISocketEventRequest, RequestResult> eventCallback, Action<ISocketEventResponse> subscribeReadyCallback = null)
        {
            // TODO: remember these parameters for then use for auto-reconnecting.
            this.Socket.On(eventName, (msg) =>
            {
                var dto = JsonConvert.DeserializeObject<SocketEventRequestDto>(msg.Json.Args[0].ToString());
                var request = Mapper.Map<SocketEventRequestDto, SocketEventRequest>(dto);
                var result = eventCallback(request);

                // Simulate a ack callback because SocketIO4Net doesn't provide one by default.
                var msgText = JsonConvert.SerializeObject(new object[] {
                    new SocketEventResponseDto() {
                        RequestId = request.RequestId,
                        Status = result.ToString().ToUpper()
                    }
                });
                var ack = new AckMessage()
                {
                    AckId = msg.AckId,
                    MessageText = msgText
                };
                this.Socket.Send(ack);
            });
            var subscribeDto = new SubscribeDto()
            {
                Event = eventName,
                SenderId = this.ClientId
            };
            this.Socket.Emit(SUBSCRIBE, subscribeDto, string.Empty, (data) =>
            {
                var json = data as JsonEncodedEventMessage;
                var result = JsonConvert.DeserializeObject<SocketEventResponseDto>(json.Args[0]);
                var response = Mapper.Map<SocketEventResponseDto, SocketEventResponse>(result);

                if (subscribeReadyCallback != null)
                {
                    subscribeReadyCallback(response);
                }
            });
        }

        public void Unsubscribe(string eventName, Action<ISocketEventResponse> callback)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(string eventName, int tryTimes = 1, int timeout = 60, dynamic args = null, Action<ISocketEventResponse> callback = null)
        {
            var dto = new EnqueueDto()
            {
                Event = eventName,
                SenderId = this.ClientId,
                TryTimes = tryTimes == 0 ? 1 : tryTimes,
                Timeout = timeout,
                Args = args
            };

            this.Socket.Emit(ENQUEUE, dto, string.Empty, (data) =>
                {
                    var json = data as JsonEncodedEventMessage;
                    var result = JsonConvert.DeserializeObject<SocketEventResponseDto>(json.Args[0]);
                    var response = Mapper.Map<SocketEventResponseDto, SocketEventResponse>(result);

                    if (callback != null)
                    {
                        callback(response);
                    }
                });
        }

        public void Enqueue(string eventName, Action<ISocketEventResponse> callback)
        {
            this.Enqueue(eventName, 0, 60, null, callback);
        }

        private void SocketConnectionClosed(object sender, EventArgs e)
        {
            this.ChangeState(ClientState.Disconnected);
        }

        private void ConnectionRetryAttempt(object sender, EventArgs e)
        {
            this.ChangeState(ClientState.Reconnecting);
        }

        protected void ChangeState(ClientState state)
        {
            var originalState = this.State;
            this.State = state;
            this.OnStateChanged(originalState, state);
        }

        protected void OnStateChanged(ClientState from, ClientState to)
        {
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new StateChangedEventArgs(from, to));
            }
            switch (to)
            {
                case ClientState.Connected:
                    if (this.Connected != null)
                    {
                        this.Connected(this, new EventArgs());
                    }
                    break;
                case ClientState.Disconnected:
                    if (this.Disconnected != null)
                    {
                        this.Disconnected(this, new EventArgs());
                    }
                    break;
                case ClientState.Reconnecting:
                    if (this.Retried != null)
                    {
                        this.Retried(this, new EventArgs());
                    }
                    break;
            }
        }
    }
}
