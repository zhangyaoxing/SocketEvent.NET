using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    public interface ISocketEventClient
    {
        /// <summary>
        /// Get unique ID of this client.
        /// </summary>
        string ClientId { get; }

        ClientState State { get; }

        /// <summary>
        /// Subscribe a event by its event name.
        /// Callback will be called when server responds.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="callback">callback</param>
        void Subscribe(string eventName, Action<IServerResponse> callback);

        void Unsubscribe(string eventName, Action<IServerResponse> callback);

        void Enqueue(string eventName, int retryLimit = 0, int timeout = 60, dynamic args = null, Action<IServerResponse> callback = null);

        void Enqueue(string eventName, Action<IServerResponse> callback = null);

        void RegisterEventListener(string eventName, Action<dynamic> eventHandler);

        void RegisterEventListener<TResult>(string eventName, Action<TResult> eventHandler);
    }
}
