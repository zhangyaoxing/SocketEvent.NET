using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SocketEvent.Dto
{
    public class ErrorDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("stack")]
        public string Stack { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}", this.Name, this.Message, this.Stack);
        }
    }
}
