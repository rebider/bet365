using System;
using System.Collections.Generic;
using System.Text;


namespace Bet365.Media
{
    public class RecordPlayer
    {
        
        public delegate void SendValue(string  v);
        public SendValue send;

        private string _path;
        private List<string> allLine;
       // protected System.Threading.Timer timeTicket;
        protected System.Windows.Forms.Timer timeTicket;
        private int currentLineIndex = 0;
        private DateTime currentTimeStamp;
        int period = 20;

        public DateTime EndTime;

        public event EventHandler<PlayerEvent> onCurrentMsg;

        public RecordPlayer(string dbpath)
        {
            _path = dbpath;
            allLine=new List<string>();
           // allLine = new List<string>(System.IO.File.ReadAllLines(dbpath));
          //  allLine.AddRange(System.IO.File.ReadAllLines(dbpath));
        }

        void HandleMsg(string msg)
        {
            if (onCurrentMsg != null)
            {
                onCurrentMsg(this,new PlayerEvent(msg));
            }

        }

        public void BeginPlay()
        {
            timeTicket = new System.Windows.Forms.Timer();
            timeTicket.Interval = 20;
            timeTicket.Enabled = true;
            timeTicket.Tick += new EventHandler(timeCallback);
         //   timeTicket = new System.Threading.Timer(new System.Threading.TimerCallback(timeCallback), 0, 1000, period);
            //timeCallback(null);
            //timeCallback(null);
            //timeCallback(null);
            

        }

        #region test
        public void Initial()
        {
            Bet365.Data.Tree.Stem stem_ES = Root.l_Childrens.Find(c => c.Line.Trim().StartsWith("ES"));
            Console.WriteLine(stem_ES);
            var stem_SC_IGoal = stem_ES.l_Childrens.Find(e => e.Line.Contains("NA=IGoal"));
            var stem_SC_IGoal_SL_1 = stem_SC_IGoal.l_Childrens[0].Properties.ValueLog;
            var stem_SC_IGoal_SL_2 = stem_SC_IGoal.l_Childrens[1].Properties.ValueLog;
        }
        #endregion

        //  void timeCallback(object sender)
        void timeCallback(object sender,EventArgs e)
        {
            string tem_line = allLine[currentLineIndex];
            string[] arr_str = tem_line.Split(new string[]{">>"},StringSplitOptions.RemoveEmptyEntries);
            //string[] arr_str = tem_line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
           // string[] arr_str = tem_line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_str.Length >= 2)
            {
                string timeStamp = arr_str[0].Trim();
                string content = arr_str[1].Trim();

                DateTime dt_timeStamp = DateTime.Parse(timeStamp);

                if (currentLineIndex==0)//第一帧
                {
                    currentTimeStamp = dt_timeStamp;
                }
                currentTimeStamp = currentTimeStamp.AddMilliseconds(period);
                TimeSpan ts = (TimeSpan)(dt_timeStamp - currentTimeStamp);

                if ( ts.Milliseconds<= 0|| currentLineIndex==0)//到时间
                {
                    currentTimeStamp = dt_timeStamp;
                    currentLineIndex++;
                    HandleMsg(arr_str[1]);

                    if (send != null)
                    {
                        send(content);
                    }
                }
            }
            
        }

        void timeCallback2(object sender, EventArgs e)
        {
        }

        public Bet365.Data.Tree.Stem Root;

       public void Load(string path)
        {
            allLine = new List<string>(System.IO.File.ReadAllLines(path));

            foreach (var tem_line in allLine)
            {
                string[] arr_str = tem_line.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime tem_time = DateTime.Parse(arr_str[0].Trim());
                string msg = arr_str[1].Trim();
                if (msg.StartsWith("F|"))//全局快照
                {

                    Bet365.Data.Tree.FixtureStem fs = new Bet365.Data.Tree.FixtureStem();
                    //Bet365.Data.Tree.Stem root = fs.Package(msg);
                    Bet365.Data.Tree.Stem root = fs.Package(msg,tem_time);
                    Console.WriteLine(root);
                    if (root.Properties["IT"].Contains("M1_10_0")&&Root==null)
                    {
                        Root = root;
                    }
                }
                else
                {
                    if (msg.Contains("15276991102M1_10_0/ML52394443C1ES_10_0/ML52394443C1ES2_10_0/ML52394443C1ES2-1_10_0"))
                    {
                        Console.WriteLine(msg);
                    }

                    if (Root != null)
                    {
                        Update(msg, tem_time);
                    }
                }
            }
          //  Initial();
            string[] tem_arr = allLine[allLine.Count - 1].Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries);
            EndTime = DateTime.Parse(tem_arr[0].Trim());
            Console.WriteLine("导入赛事数据 ,共 {0} 条 ",allLine.Count);
        }

        /// <summary>
        /// 处理更新等信息，信息更新添加删除等，初始化先不在这里搞
        /// </summary>
        /// <param name="data"></param>
        void Update(string data,DateTime time)
        {
            //这是处理更新等信息
            //this.lb_Inplay.Items.Add("bbb");
            //Bet365.Data.Tree.StemEvent e = new Bet365.Data.Tree.StemEvent(data);
            Bet365.Data.Tree.StemEvent e = new Bet365.Data.Tree.StemEvent(data,time);
           
            if (e.EventFlag == Bet365.Data.Tree.StemEvent.DELETE)
            {
                if (e.Trace.Count > 1)
                {
                    if (e.Trace[1] == "CL_1_10_0")
                    {
                //        Console.WriteLine(e.Trace[1]);
                    }
                }
            }

      //      Bet365.Data.DataManager dm = new Data.DataManager();
       
            Bet365.Data.Tree.Stem target_Stem =  GetTargetStem(e.Trace);
            if (e.EventFlag == Bet365.Data.Tree.StemEvent.INSERT)
            {
                target_Stem = GetTargetStem(e.Trace.GetRange(0, e.Trace.Count - 1));
            }
            if (target_Stem == null)
            {
                target_Stem = GetTargetStem(e.Trace.GetRange(0, e.Trace.Count - 2));
            }


            if (null == target_Stem && e.EventFlag != Bet365.Data.Tree.StemEvent.INSERT)
            {
                return;
            }

            if (e.EventFlag == Bet365.Data.Tree.StemEvent.DELETE)
            {
                Bet365.Data.Tree.Stem parentstem = target_Stem.Parent;
                if (null == target_Stem.Name || "" == target_Stem.Name)
                {
                    return;
                }
                try
                {
                    parentstem.RemoveChildStem(target_Stem.Properties["IT"]);
                }catch{}
            }
            else if (e.EventFlag == Bet365.Data.Tree.StemEvent.INITIAL)
            {
                //TODO:Inplay 初始化数据
            }
            else if (e.EventFlag == Bet365.Data.Tree.StemEvent.INSERT)
            {
                e.STEM.Parent = target_Stem;
               
            }
            else if (e.EventFlag == Bet365.Data.Tree.StemEvent.UPDATE)
            {
               // Console.WriteLine("修改  :  {0}", target_Stem.Name);
                //target_Stem.Properties.Update(e.PROPERTY);
                target_Stem.Properties.Update(e.PROPERTY,time);
            }
        }



        /// <summary>
        /// 获取更新数据指定路径Stem
        /// </summary>
        /// <param name="trace"></param>
        /// <returns></returns>
        Bet365.Data.Tree.Stem GetTargetStem(List<string> trace)
        {
            Bet365.Data.Tree.Stem target_Stem = Root;

            if (trace == null || trace.Count == 0)
            {
                return Root;
            }

            return Root.FindChildByProperty("IT", trace[trace.Count - 1]);

            foreach (string it in trace)
            {
                // if (_root.Childrens.ContainsKey(it))
                if (target_Stem != null)
                {
                    if (target_Stem.Childrens.ContainsKey(it))
                    {
                        target_Stem = target_Stem.Childrens[it];//TODO: 这里要改
                    }
                    else
                    {
                        // Console.WriteLine("t");
                        target_Stem = null;
                        break;
                    }
                }
            }
            return target_Stem;
        }

    }


    
}
