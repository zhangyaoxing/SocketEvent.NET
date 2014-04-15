using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SocketEvent.Dto
{
    /// <summary>
    /// DTO used when subscribing an event.
    /// </summary>
    class SubscribeDto
    {
        public SubscribeDto()
        {
            this.RequestId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Request ID. Auto-generated. Don't have to set it unless necessary.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// ID of subscriber.
        /// </summary>
        [JsonProperty("senderId")]
        public string SenderId { get; set; }

        /// <summary>
        /// Event to subscribe.
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }
    }
}
