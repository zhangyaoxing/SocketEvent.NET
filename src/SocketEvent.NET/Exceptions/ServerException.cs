using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent.Exceptions
{
    public class ServerException: ApplicationException
    {
        public string ServerErrorName { get; protected set; }

        public string ServerMessage { get; protected set; }

        public string ServerStackTrace { get; protected set; }

        public ServerException(string name, string message, string serverStackTrace)
        {
            this.ServerErrorName = name;
            this.ServerMessage = message;
            this.ServerStackTrace = serverStackTrace;
        }
    }
}
