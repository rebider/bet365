using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    public class StemEvent : EventArgs
    {
        private object _data;

        private string str_data;

        public static string UPDATE = "UPDATE";
        public static string INSERT = "INSTER";
        public static string DELETE = "DELETE";
        public static string INITIAL = "INITIAL";

        public static string DATA_DELETE="D|";
        public static string DATA_INSERT = "I|";
        public static string DATA_UPDATE = "U|";
        public static string DATA_FULL = "F|";

        private FixtureStem fixtureStem = new FixtureStem();

        public string EventFlag
        {
            get;
            set;
        }

        public StemEvent(string p1, object p2 = null, bool p3 = false, bool p4 = false) :base()
        {
            _data = p2;
        }

        public StemEvent(string eventFlag, string data):base()
        {
            this.EventFlag = eventFlag;
            this.str_data = data;
        }

        public StemProperty PROPERTY
        {
            get;
            set;
        }

        public Stem STEM
        {
            get;
            set;
        }

        public List<string> Trace = new List<string>();

        public string FullData
        {
            get;
            set;
        }

        public StemEvent(string fulldata,DateTime time)
        {
            FullData = fulldata;
            if (fulldata.StartsWith("F|"))
            {
                //fulldata = fulldata.Remove(0, 2);
                EventFlag = INITIAL;
                //TODO:再初始化以下stem
             //   STEM = fixtureStem.Package(fulldata);
                STEM = fixtureStem.Package(fulldata,time);
            }
            else
            {
                if (fulldata.Contains(DATA_INSERT))
                {
                    EventFlag = INSERT;
                    //STEM = fixtureStem.Package(fulldata);
                    string[] ss = fulldata.Split(new string[] { DATA_INSERT }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length > 1)
                    {
                        //TODO:
                     //   STEM = fixtureStem.Package(ss[1]);
                        STEM = fixtureStem.Package(ss[1],time);
                    }
                }
                else if (fulldata.Contains(DATA_UPDATE))
                {
                    //处理更新数据
                    EventFlag = UPDATE;
                    string[] ps = fulldata.Split(new string[] { DATA_UPDATE }, StringSplitOptions.RemoveEmptyEntries);
                    StemProperty stemProperty = fixtureStem.GetProperties(ps.Length > 1 ? ps[1] : "");
                    PROPERTY = stemProperty;
                }
                else if (fulldata.Contains(DATA_DELETE))
                {
                    EventFlag = DELETE;
                }
                Trace = GetTraces(fulldata);
            }
        }


        public StemEvent(string fulldata)
        {
            FullData = fulldata;
            if (fulldata.StartsWith("F|"))
            {
                //fulldata = fulldata.Remove(0, 2);
                EventFlag = INITIAL;
                //TODO:再初始化以下stem
                STEM = fixtureStem.Package(fulldata);
            }
            else
            {
                if (fulldata.Contains(DATA_INSERT))
                {
                    EventFlag = INSERT;
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
                    EventFlag = UPDATE;
                    string[] ps = fulldata.Split(new string[] { DATA_UPDATE }, StringSplitOptions.RemoveEmptyEntries);
                    StemProperty stemProperty = fixtureStem.GetProperties(ps.Length > 1 ? ps[1] : "");
                    PROPERTY = stemProperty;
                }
                else if (fulldata.Contains(DATA_DELETE))
                {
                    EventFlag = DELETE;
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
            if(fulldata.Contains("|PA"))
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

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /*
         * Event () 构造函数 

public function Event(type:String, bubbles:Boolean = false, cancelable:Boolean = false) 

语言版本 :  ActionScript 3.0 
Player 版本 :  Flash Player 9 


创建一个作为参数传递给事件侦听器的 Event 对象。 

参数  type:String — 事件的类型，可以作为 Event.type 访问。  
  
 bubbles:Boolean (default = false) — 确定 Event 对象是否参与事件流的冒泡阶段。 默认值为 false。  
  
 cancelable:Boolean (default = false) — 确定是否可以取消 Event 对象。 默认值为 false。  

         */
    }
}
