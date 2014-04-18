using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SocketEvent.Dto
{
    class EnqueueDto : BaseCommunicateDto
    {
        /// <summary>
        /// Event to subscribe.
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("tryTimes")]
        public int TryTimes { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        [JsonProperty("args")]
        public dynamic Args { get; set; }
    }
}
