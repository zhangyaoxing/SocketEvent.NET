using System;
using System.Collections.Generic;
using System.Text;
using SocketIOClient;
using SocketEvent.Dto;
using SocketEvent.Exceptions;
using Newtonsoft.Json;

namespace SocketEvent.Impl
{
    public class SocketEventClient
    {

        private Client socket;

        /// <summary>
        /// Connect to a socket event URL
        /// </summary>
        /// <param name="url">Target SocketIO URL</param>
        /// <returns>SocketEventClient</returns>
        public static SocketEventClient Connect(string url)
        {
            return new SocketEventClient(url);
        }

        private SocketEventClient(string url)
        {
            this.State = ClientState.Disconnected;
            this.socket = new Client(url);
            this.socket.Connect();
            this.State = ClientState.Connected;
            this.ClientId = Guid.NewGuid().ToString();
        }

        public event EventHandler StateChanged;

        public string ClientId { get; set; }

        public ClientState State { get; protected set; }

        public void Subscribe(string eventName)
        {
            var dto = new SubscribeDto()
            {
                Event = eventName,
                SenderId = this.ClientId
            };
            this.socket.Emit("subscribe", dto, string.Empty, (data) =>
            {
                RequestResultDto result = data.Json.GetFirstArgAs<RequestResultDto>();
                if (result.Status != RequestResult.SUCCESS)
                {
                    var up = new ServerException(result.Error.Name, result.Error.Message, result.Error.Stack);
                    throw up;
                }
            });
        }

        private void ChangeState()
        {
            // TODO: OnStateChange event.
        }
    }
}
