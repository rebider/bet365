using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bet365.Util
{
    public class DataProcress
    {

        public string RegexSet(string IntoRegex, string stopRegex, string EndRegex)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Match(IntoRegex, String.Format(@"{0}[\w|\W]*?{1}", stopRegex, EndRegex), System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Replace(stopRegex, "").Replace(EndRegex, "");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string SESSION { get { return _session; } }
        static string _session;
        static object LockGetSession = new object();

       public static string getSession()
        {

            try
            {
                var sessionID = "";
                Util.HttpHelper hh = new Util.HttpHelper();
                string tem_sessionId = hh.WebClientGET(hh.getHeader(), Encoding.UTF8, "https://www.bet365.com/home/FlashGen4/WebConsoleApp.asp?&cb=105812123906");
                //sessionID = tem_sessionId;
                // Console.WriteLine(tem_sessionId);
                Regex regex = new Regex(@"pstk=(?<pstk>[\S]+);", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                if (hh.HeadCookie == "" || hh.HeadCookie == null)
                {
                    return "";
                }
                MatchCollection matchCollection = regex.Matches(hh.HeadCookie);
                foreach (Match match in matchCollection)
                {
                    sessionID = match.Groups["pstk"].Value;
                    Console.WriteLine(sessionID);
                    break;
                }
                _session = sessionID;
                return sessionID;
            }
            catch
            {
                _session = "";
                return "";
            }
        }



        public static string GetSessionId()
        {
            if (_session != "" )//&& count_session++ < 20)
            {
                return _session;
            }
            else
            {
                //count_session = 0;
               

                return getSession();
            }
        }

        public static string GetNewSessionID(string oldID)
        {
            lock (LockGetSession)
            {
                if (_session != oldID)
                {
                    return _session;
                }

                var newid = GetSessionId();
                return newid;

            }

        }



        private static List<string> LSessionID = new List<string>();

        private static System.Threading.Timer _timerSession;

        public static void BeginRefreshSessionID()
        {
            _timerSession = new System.Threading.Timer(new System.Threading.TimerCallback(delegate(object s)
            {
                string tem_ID = getSession();

                if ((tem_ID.IsNullOrEmpty() || tem_ID == ""))
                {
                    _session = tem_ID;
                    LSessionID.Add(tem_ID);
                }
            }), null, 2 * 60 * 1000, 5 * 1000 * 60);


        }
    }
}
