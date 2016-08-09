using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.PushTechnology.Diffusion.Events
{
    public class DiffusionMessageEvent:EventArgs
    {
        public string Message { get { return _msg; } }
        private string _msg;
       const  string DATA_DELETE = "D|";
       const   string DATA_INSERT = "I|";
       const   string DATA_UPDATE = "U|";
       const   string DATA_FULL = "F|";

        public DiffusionMessageFlag MessageFlag;
        private Bet365.Data.Tree.FixtureStem fixtureStem = new Bet365.Data.Tree.FixtureStem();

        public Bet365.Data.Tree.StemProperty PROPERTY
        {
            get;
            set;
        }

        public Bet365.Data.Tree.Stem STEM
        {
            get;
            set;
        }


        public DiffusionMessageEvent(string msg)
        {
            this._msg = msg;
            Analyze(msg);
        }

        public List<string> Trace = new List<string>();

        void Analyze(string fulldata)
        {
          //  FullData = fulldata;
            if (fulldata.StartsWith("F|"))
            {
                //fulldata = fulldata.Remove(0, 2);
                MessageFlag = DiffusionMessageFlag.INITIAL;
                //TODO:再初始化以下stem
                STEM = fixtureStem.Package(fulldata);
            }
            else
            {
                if (fulldata.Contains(DATA_INSERT))
                {
                    MessageFlag = DiffusionMessageFlag.INSERT;
                    //STEM = fixtureStem.Package(fulldata);
                    string[] ss = fulldata.Split(new string[] { DATA_INSERT }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length > 1)
                    {
                        //TODO:
                        STEM = fixtureStem.Package(ss[1]);
                    }
                }
                else if (fulldata.Contains(DATA_UPDATE))
                {
                    //处理更新数据
                    MessageFlag = DiffusionMessageFlag.UPDATE;
                    string[] ps = fulldata.Split(new string[] { DATA_UPDATE }, StringSplitOptions.RemoveEmptyEntries);
                    Bet365.Data.Tree.StemProperty stemProperty = fixtureStem.GetProperties(ps.Length > 1 ? ps[1] : "");
                    PROPERTY = stemProperty;
                }
                else if (fulldata.Contains(DATA_DELETE))
                {
                    MessageFlag = DiffusionMessageFlag.DELETE;
                }
                Trace = GetTraces(fulldata);
            }
        }

        #region 获取更新标识 例如 41983335A_10_0/160030-41983335A_10_0/41983453A_10_0/41984783A_10_0U|SU=0;|
        /// <summary>
        /// 获取更新标识 例如 41983335A_10_0/160030-41983335A_10_0/41983453A_10_0/41984783A_10_0U|SU=0;|
        /// 分析成数组
        /// </summary>
        /// <param name="fulldata"></param>
        /// <returns></returns>
        public List<string> GetTraces(string fulldata)
        {
            if (fulldata.Contains("|PA"))
            {
                Console.WriteLine(fulldata);
            }
            List<string> tls = new List<string>();
            //([\d\w\-]+/)+[\d\w\-]+
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"([\d\w\-]+/)+[\d\w\-]+");
            System.Text.RegularExpressions.Match m = r.Match(fulldata);
            string[] its = m.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            tls.AddRange(its);
            return tls;
        }
        #endregion

    }
}
