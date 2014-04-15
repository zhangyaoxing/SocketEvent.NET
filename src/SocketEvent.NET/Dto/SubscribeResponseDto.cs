using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent.Dto
{
    class SubscribeResponseDto
    {
        public string RequestId { get; set; }

        public string Status { get; set; }

        public ErrorDto Error { get; set; }
    }
}
