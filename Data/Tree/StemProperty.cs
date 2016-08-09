using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    public class StemProperty:Dictionary<string,string>
    {
        #region old
        private System.Windows.Forms.DataGridView _dgv;// = new System.Windows.Forms.DataGridView();

        public System.Windows.Forms.DataGridView DataGridView
        {
            get { return _dgv; }
            set { _dgv = value; }
        }


        public delegate void HandlePaint(string x, string y, string data);
        public event HandlePaint RepaintCell;

        public delegate void HandleUpdateDataGridView(object sender, object valueContent, object value);

        public event HandleUpdateDataGridView RepaintValue;

        void Draw(string x, string y, string data)
        {
            if (RepaintCell != null)
            {
                RepaintCell(x, y, data);
            }
        }

        //void Reflesh(object sender, object valueContent, object value)
        //{
        //    if (RepaintValue != null)
        //    {
        //        RepaintValue(sender, valueContent, value);
        //    }
        //}

        delegate void Handle_DataGridView(object sender);

        event Handle_DataGridView e_Reflesh;
        void Reflesh(object sender)
        {
            System.Windows.Forms.DataGridView td = sender as System.Windows.Forms.DataGridView;
            td.Refresh();
        }
        #endregion


        public Dictionary<string, List<PropertyTimeStamp>> ValueLog = new Dictionary<string, List<PropertyTimeStamp>>();

        public StemProperty()
        {
            e_Reflesh = Reflesh;
        }

      


        public void Update(StemProperty sp)
        {
            foreach (string k in sp.Keys)
            {
                if (this.ContainsKey(k))
                {
                    //this[k] = sp[k]+"********";
                    this[k] = sp[k];
                    //Console.WriteLine("修改成功");
                }
                else
                {
                    //    this.Add(k, sp[k]); 这里不用ADD set属性里面会自动add
                    this[k] = sp[k];
                   // Console.WriteLine("添加新属性        {0}={1}", k, sp[k]);
                }
            }
        }

        public void Update(StemProperty sp, DateTime time)
        {
            foreach (string k in sp.Keys)
            {
                if (this.ContainsKey(k))
                {
                    this[k,time] = sp[k];
                    //Console.WriteLine("修改成功");
                }
                else
                {
                //    this.Add(k, sp[k]); 这里不用ADD set属性里面会自动add
                    this[k, time] = sp[k];
                    Console.WriteLine("添加新属性        {0}={1}", k, sp[k]);
                }
            }
        }

        public string this[string key, DateTime time]
        {
            get
            {
                return this[key];
            }
            set
            {
                this[key] = value;
                if (ValueLog.ContainsKey(key))
                {
                    ValueLog[key].Add(new PropertyTimeStamp(key,value,time));
                }
                else
                {
                    ValueLog.Add(key, new List<PropertyTimeStamp>() { new PropertyTimeStamp(key, value, time) });
                }
            }
        }


        /// <summary>
        /// 这里的key，value添加更改都在这里，如果有key则直接修改,没有则添加
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new  string this[string key]
     //   public string this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return base[key];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                //TODO: 个人认为，属性更改事件要在这里写！！！！！！不在Stem里面写，Stem里面写的是增加或者删除事件！！！

                if (this.ContainsKey(key))
                {
                    base[key] = value;
                    Console.WriteLine(value);
                }
                else
                {
                    base.Add(key, value);//
                }
            }
        }


        public override string ToString()
        {
            //return "test";
           // return base.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append((this["NA"] + "  ").Trim() + (this["HA"] + "  ").Trim() + (this["OD"] + "  ").Trim());
            return sb.ToString();
        }

        public void RefleshAll()
        {
            if (_dgv != null)
            {
                this._dgv.Invoke(e_Reflesh, new object[] { this._dgv });
            }
        }
    }


    public class PropertyTimeStamp
    {
       public string PropertyKEY { get; set; }
        public string PropertyValue { get; set; }
        public DateTime TimeStamp { get; set; }

        public PropertyTimeStamp(string key, string v, DateTime time)
        {
            this.PropertyKEY = key;
            this.PropertyValue = v;
            this.TimeStamp = time;
        }

        public override string ToString()
        {
            //]return base.ToString();
            if (PropertyKEY == "VC")
            {
                return VCDCT.Translate(PropertyValue) + " - " + TimeStamp.ToString() ;
            }
            return base.ToString();

        }
    }

   public struct VCDCT
    {
       public static string Translate(string vc)
       {
           if (vc.Length==4)
           {
               if (ACT.ContainsKey(vc))
               {
                 return  ACT[vc];
               }
               return vc;
           }
           else if (vc.Length == 5)
           {
               string temPlayr = vc.Substring(0, 1);
               string temAct = vc.Substring(1, 4);

               switch (temPlayr)
               {
                   case "1":
                       temPlayr = "主队";
                       break;
                   case "2":
                       temPlayr = "客队";
                       break;
                   default:
                       break;
               }

               if (ACT.ContainsKey(temAct))
               {
                   temAct = ACT[temAct];
               }

               return temPlayr + "  -  " + temAct;
           }
           else
           {
               return vc;
           }
       }

       static Dictionary<string, string> ACT = new Dictionary<string, string>()
       {
           {"1000","危险进攻 - danger"},
           {"1001","进攻 - attack"},
           {"1002","控球 - safe"},
           {"1003","goal"},//进球？
           {"1004","角球 - corner"},
           {"1007","球门球 - goalkick"},
           {"1008","penalty"},
           {"1009","dfreekick"},
           {"1010","sfreekick"},
           {"1024","throw"},
           {"0008","penaltyScore"},
           {"0009","penaltyMiss"},
           {"0021","penaltyToTake"},
           {"1005","黄牌 - yellowCard"},
           {"1006","红牌  - redCard"},
           {"1011","shotongoal"},
           {"1012","shotoffgoal"},
           {"1013","替补 - substitution"},
           {"1014","kickoff"},
           {"1015","halftime"},
           {"1016","secondhalf"},
           {"1017","全场结束 - fulltime"},
           {"1018","startExtraTime1"},
           {"1019","extraTimeHT"},
           {"1020","startExtraTime2"},
           {"1021","endOfExtraTime"},
           {"1022","penaltyshootout"},
           {"1023","penaltyMiss"},
           {"1025","injury"},
           {"1026","injuryTime"},
           {"1236","ZonedFreekick"},
           {"1237","掷界外球 - ZoneThrow"},
           {"1233","MatchInfo"},
           {"1234","offside"},
           {"1238","Substitution"},
           {"1239","ZonedCorner"},
           
           
          // {"236","越位"},
           
           
           


    

       };

    }
}
