using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent.Dto
{
    class RequestResultDto
    {
        public RequestResultDto()
        {
        }

        public string RequestId { get; set; }

        public string Status { get; set; }

        public ErrorDto Error { get; set; }
    }
}
