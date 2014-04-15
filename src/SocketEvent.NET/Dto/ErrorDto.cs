using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent.Dto
{
    public class ErrorDto
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public string Stack { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}", this.Name, this.Message, this.Stack);
        }
    }
}
