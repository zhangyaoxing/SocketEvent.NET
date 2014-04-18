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

        private Client socket;

        public SocketEventClient(string url): this(Guid.NewGuid().ToString(), url)
        {
        }

        public SocketEventClient(string id, string url)
        {
            this.ClientId = id;
            this.State = ClientState.Disconnected;
            this.socket = new Client(url);
            this.socket.Connect();
            this.State = ClientState.Connected;
        }

        public string ClientId { get; set; }

        public ClientState State { get; set; }

        public void Subscribe(string eventName, Func<ISocketEventRequest, RequestResult> eventCallback, Action<ISocketEventResponse> subscribeReadyCallback = null)
        {
            this.socket.On(eventName, (msg) =>
            {
                var dto = JsonConvert.DeserializeObject<SocketEventRequestDto>(msg.Json.Args[0].ToString());
                var request = Mapper.Map<SocketEventRequestDto, SocketEventRequest>(dto);
                var result = eventCallback(request);
                
                // Simulate a ack callback because SocketIO4Net doesn't provide one by default.
                var msgText = JsonConvert.SerializeObject(new SocketEventResponseDto() {
                    RequestId = request.RequestId,
                    Status = result.ToString()
                });
                var ack = new AckMessage()
                {
                    AckId = msg.AckId,
                    Endpoint = msg.Endpoint,
                    MessageText = msgText
                };
                this.socket.Send(ack);
            });
            var subscribeDto = new SubscribeDto()
            {
                Event = eventName,
                SenderId = this.ClientId
            };
            this.socket.Emit(SUBSCRIBE, subscribeDto, string.Empty, (data) =>
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

            this.socket.Emit(ENQUEUE, dto, string.Empty, (data) =>
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

        private void ChangeState()
        {
            // TODO: OnStateChange event.
        }
    }
}
