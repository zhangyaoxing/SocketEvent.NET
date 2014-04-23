using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketEvent
{
    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(ClientState from, ClientState to)
        {
            this.FromState = from;
            this.ToState = to;
        }

        public ClientState FromState { get; protected set; }

        public ClientState ToState { get; protected set; }
    }
}
