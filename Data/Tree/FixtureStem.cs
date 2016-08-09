using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    public class FixtureStem : Stem
    {
        private Array _additionalScores;
        private Array _teamGroups;
        private Array _statGroups;

        private Util.DataProcress httpHelper = new Util.DataProcress();

        private Dictionary<string, string> C2P = new Dictionary<string, string>();
            

        public FixtureStem()
        {
            //C2P.Add("ES", "EV");
            //C2P.Add("SG", "EV");
            


            _statGroups = new object[] { };
            _teamGroups = new object[] { };
            _statGroups = new object[] { };
        }

        private DateTime tem_time;

        public Stem Package(string data, DateTime time)
        {
            tem_time = time;
            Stem tem_stem = Package(data);
            tem_stem.Time = time;
            
            return tem_stem;
        }

        /// <summary>
        /// 这个那个封装字符信息成stem变量
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Stem Package(string data)
        {
            
            Stem root = new Stem();
            Stem currentStem = null;
            if (data.StartsWith("F|"))
            {
                data = data.Remove(0, 2);
            }
            string[] lines = data.Split(new char[] { '|' });
            currentStem = root;
            root.Properties = GetProperties(lines[0]);
            root.Name = lines[0].Substring(0, 2);//断定名字长度是2
            root.Line = lines[0];

            for (int i = 1; i < lines.Length; i++)
            {
                string l = lines[i];
                if (l.Length < 2)
                {
                    break;
                }
                Stem ts = new Stem();
                ts.Line = l;
                ts.Name = l.Substring(0, 2);
                //ts.Properties = GetProperties(l);
                ts.Properties = GetProperties(l);
                ts.Time = tem_time;//TODO: 未测试过该行代码
                if(currentStem.Name==ts.Name)//同根同源
                {
                    ts.Parent=currentStem.Parent;//自动添加Childs
                    //ts.Parent.Children.Add(ts.Properties["IT"],ts);
                }
                else if (C2P.ContainsKey(ts.Name))
                {

                    if (C2P[ts.Name] == currentStem.Name)
                    {
                        ts.Parent = currentStem;
                    }
                    else
                    {
                        if (ts.Name == "ES" || ts.Name == "SG" || ts.Name == "MA" || ts.Name == "TG")
                        {
                            ts.Parent = root;
                        }
                        else
                        {
                            ts.Parent = GetParent(ts, currentStem);
                        }
                        //ts.Parent = temp.Name == "PA" ? root : temp;//TODO:可能有错误
                    }
                }
                else
                {
                    if (ts.Name == "ES" || ts.Name == "SG" || ts.Name == "MA" || ts.Name == "TG")
                    {
                        ts.Parent = root;
                    }
                    else
                    {
                        ts.Parent = currentStem;
                    }
                  //  C2P.Add(ts.Name, currentStem.Name);
                    C2P.Add(ts.Name, ts.Parent.Name);
                }
                currentStem = ts;
            }
            return root;
        }


        public Stem PackageForInplay(string data)
        {

            Stem root = new Stem();
            Stem currentStem = null;
            if (data.StartsWith("F|"))
            {
                data = data.Remove(0, 2);
            }
            string[] lines = data.Split(new char[] { '|' });
            currentStem = root;
            root.Properties = GetProperties(lines[0]);
            //root.Name = lines[0].Substring(0, 2);//断定名字长度是2
            root.Name = data.Split(new char[] { '|' })[0];
            root.Line = lines[0];

            for (int i = 1; i < lines.Length; i++)
            {
                string l = lines[i];
                if (l.Length < 2)
                {
                    break;
                }
                Stem ts = new Stem();
                ts.Line = l;
                ts.Name = l.Substring(0, 2);
                ts.Properties = GetProperties(l);
                if (currentStem.Name == ts.Name)//同根同源
                {
                    ts.Parent = currentStem.Parent;//自动添加Childs
                    //ts.Parent.Children.Add(ts.Properties["IT"],ts);
                }
                else if (C2P.ContainsKey(ts.Name))
                {

                    if (C2P[ts.Name] == currentStem.Name)
                    {
                        ts.Parent = currentStem;
                    }
                    else
                    {
                        if (ts.Name == "CL" || ts.Name == "SG" || ts.Name == "TG")//||ts.Name == "MA"  )
                        {
                            ts.Parent = root;
                        }
                        else
                        {
                            ts.Parent = GetParent(ts, currentStem);
                        }
                        //ts.Parent = temp.Name == "PA" ? root : temp;//TODO:可能有错误
                    }
                }
                else
                {
                    if (ts.Name == "ES" || ts.Name == "SG" || ts.Name == "CL" || ts.Name == "TG")
                    {
                        ts.Parent = root;
                    }
                    else
                    {
                        ts.Parent = currentStem;
                    }
                    //  C2P.Add(ts.Name, currentStem.Name);
                    C2P.Add(ts.Name, ts.Parent.Name);
                }
                currentStem = ts;
            }
            return root;
        }

        /// <summary>
        /// 获取每一行的所有属性
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //public Dictionary<string, string> GetProperties(string data)
        public StemProperty GetProperties(string data)
        {
            StemProperty ps = new StemProperty();
            //TODO: getProperties
            //ps["IT"] = "";
            string[] pairs = data.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
            foreach (string para in pairs)
            {
               // string[] kv = para.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                string[] kv = para.Split(new char[] { '=' },StringSplitOptions.None);
                if (kv.Length == 2)
                {
                   // ps.Add(kv[0].Trim(), kv[1].Trim());
                    ps[kv[0].Trim(),tem_time]= kv[1].Trim();
                }
            }

            return ps;
        }

        public Stem GetSingleStem(string data)
        {
            Stem s = new Stem();
            //TODO: GetSingleStem
            return s;
        }

        public Stem GetParent( Stem child, Stem current)
        {
            if (current.Parent != null)
            {
                if (current.Parent.l_Childrens[0].Name == child.Name)
                {
                    return current.Parent;
                }
                else
                {
                    return GetParent(child, current.Parent);
                }
            }
            else
            {
                return current;
            }
    //        return s;
        }




        //public override void remove()
        //{
        //    base.remove();
        //}

        public Array StatGroups
        {
            get { return _statGroups; }
        }
        public string LegacyID
        {
            get;
            set;
            //  get{return _data["C1"].ToString()+_data["T1"].ToString()+_data["C2"].ToString()+_data["T2"].ToString()+"-"+(parent==null?parent.Data["ID"]==null):(_data["CL"]);
            //}
        }

        Array AdditionalScores
        {
            get { return _additionalScores; }
        }

        Array TeamGroups
        {
            get { return _teamGroups; }
        }

        //public override void insert(object p1, Type p2 = null)
        //{
        //    //TODO: overwrite insert
        //    base.insert(p1, p2);
        //}
        
           
        

        
    }
}
