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

        public void Subscribe(string eventName, Action<IServerResponse> callback = null)
        {
            var dto = new SubscribeDto()
            {
                Event = eventName,
                SenderId = this.ClientId
            };
            this.socket.Emit(SUBSCRIBE, dto, string.Empty, (data) =>
            {
                var json = data as JsonEncodedEventMessage;
                var result = JsonConvert.DeserializeObject<ServerResponseDto>(json.Args[0]);
                var response = Mapper.Map<ServerResponseDto, ServerResponse>(result);

                if (callback != null)
                {
                    callback(response);
                }
            });
        }

        public void Unsubscribe(string eventName, Action<IServerResponse> callback)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(string eventName, int retryLimit = 0, int timeout = 60, dynamic args = null, Action<IServerResponse> callback = null)
        {
            var dto = new EnqueueDto()
            {
                Event = eventName,
                SenderId = this.ClientId,
                RetryLimit = retryLimit,
                Timeout = timeout,
                Args = args
            };

            this.socket.Emit(ENQUEUE, dto, string.Empty, (data) =>
                {
                    var json = data as JsonEncodedEventMessage;
                    var result = JsonConvert.DeserializeObject<ServerResponseDto>(json.Args[0]);
                    var response = Mapper.Map<ServerResponseDto, ServerResponse>(result);

                    if (callback != null)
                    {
                        callback(response);
                    }
                });
        }

        public void Enqueue(string eventName, Action<IServerResponse> callback = null)
        {
            this.Enqueue(eventName, 0, 60, null, callback);
        }

        public void RegisterEventListener<TResult>(string eventName, Action<TResult> eventHandler)
        {
            throw new NotImplementedException();
        }

        private void ChangeState()
        {
            // TODO: OnStateChange event.
        }
    }
}
