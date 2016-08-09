using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Readit.Event
{
    public class TransportConnectionEvent : System.EventArgs
    {
        public   const string CONNECTED = "connected";
        public  const string DISCONNECTED = "disconnected";
        public   const string CONNECTION_FAILED = "connectionFailed";


        public  string EventMsg { get; set; }
        public TransportConnectionEvent(string param1,bool param2 = false,bool param3 = false)
        {
            //TODO
            EventMsg = param1;

        }

    }
}
