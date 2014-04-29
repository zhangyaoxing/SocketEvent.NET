using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    public enum ClientState
    {
        Connecting,
        Connected,
        ConnectFailed,
        Reconnecting,
        Reconnect,
        ReconnectFailed,
        Disconnect
    }
}
