using System;
using System.Collections.Generic;
using System.Text;
using Bet365.Readit.Event;

namespace Bet365.Readit
{
   public class ReaditConnectionDetails
    {
       public int port;
       public ReaditConnectionDetails next;
       public string host;
       //        public var transportMethod:Class;
       //public object transportMethod;
       public IReaditTransportMethod transportMethod;
       public string defaultTopic;

       public ReaditConnectionDetails()
       {

       }

       public override string ToString()
       {
           //return base.ToString();
           return "[host:" + host + ", port:" + port + ", topic:" + defaultTopic + ", transport:" + transportMethod + "]";
       }


    }
}
