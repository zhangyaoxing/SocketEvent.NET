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

        /// <summary>
        /// State of the client.
        /// </summary>
        ClientState State { get; }

        /// <summary>
        /// Subscribe a event by its event name.
        /// Callback will be called when server responds.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="eventCallback">Callback when someone fired the event.</param>
        /// <param name="subscribeReadyCallback">Callback when subscription is accepted by server.</param>
        void Subscribe(string eventName, Func<ISocketEventRequest, RequestResult> eventCallback, Action<ISocketEventResponse> subscribeReadyCallback);

        /// <summary>
        /// Unsubscribe from an event.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="callback">Callback when unsubscription is accpeted by server.</param>
        void Unsubscribe(string eventName, Action<ISocketEventResponse> callback);

        /// <summary>
        /// Enqueue some event to server. Server will fire this event for all subscribers.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="tryTimes">How many times should we try before failing. Defaults to 1. -1 stands for always retry.</param>
        /// <param name="timeout">In seconds. When firing this event, 
        /// how long should the server be waiting for the subscribers before treating it as a failure.
        /// Defaults to 60s</param>
        /// <param name="args">Arguments that needed to pass to subscribers.</param>
        /// <param name="enqueueReadyCallback">Callback when enqueue is accepted by server.</param>
        void Enqueue(string eventName, int tryTimes = 1, int timeout = 60, dynamic args = null, Action<ISocketEventResponse> enqueueReadyCallback = null);

        /// <summary>
        /// Enqueue some event to server. Server will fire this event for all subscribers.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="enqueueReadyCallback">Callback when enqueue is accepted by server.</param>
        void Enqueue(string eventName, Action<ISocketEventResponse> enqueueReadyCallback);
    }
}
