using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Media
{
    public class PlayerEvent:EventArgs
    {
       

        private string _msg;
        public string Message { get { return _msg; } }
        public PlayerEvent(string msg)
        {
            this._msg = msg;
        }
    }
}
