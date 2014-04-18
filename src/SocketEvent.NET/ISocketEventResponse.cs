using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    /// <summary>
    /// This class representing a response from SocketEvent server.
    /// </summary>
    public interface ISocketEventResponse
    {
        /// <summary>
        /// Get request ID.
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// Get response status.
        /// </summary>
        RequestResult Status { get; }

        /// <summary>
        /// Get error sent from server. Available only if Status == RequestResult.Fail
        /// </summary>
        IServerError Error { get; }
    }
}
