using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SocketEvent.Dto
{
    class SocketEventRequestDto
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("event")]
        public string EventName { get; set; }

        [JsonProperty("args")]
        public dynamic Args { get; set; }
    }
}
