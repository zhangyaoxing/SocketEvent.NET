using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    public interface ISocketEventRequest
    {
        string EventName { get; }

        string RequestId { get; }

        dynamic Args { get; }
    }
}
