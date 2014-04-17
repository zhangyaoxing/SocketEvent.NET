using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent.Impl
{
    class ServerResponse: IServerResponse
    {
        public string RequestId { get; set; }

        public RequestResult Status { get; set; }

        public IServerError Error { get; set; }
    }
}
