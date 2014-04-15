using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    public interface IServerError
    {
        string Name { get; }

        string Message { get; }

        string Stack { get; set; }
    }
}
