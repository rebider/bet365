using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Media
{
   public class Snapshot
    {
       public string _fullData;
       public string FullData { get { return _fullData; } }

       public Snapshot(string fd)
       {
           _fullData = fd;
       }

       //public 

    }
}
