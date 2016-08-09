using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Readit
{
    public class ReaditLog
    {
        public static List<string> log;


        public static void Log(string msg)
        {
            var _loc_2 = new LogEntry(msg);
            Console.WriteLine("Readit:" + _loc_2);
            log.Add(_loc_2.ToString());
        }





    }
    class LogEntry
    {

        public DateTime timestamp;
        public string message;

        public LogEntry(string msg)
        {
            this.message = msg;
            this.timestamp = new DateTime();
        }

        public override string ToString()
        {
            return "[" + timestamp + "] - " + message;
        }

    }


 



}
