using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data
{
    public class DataManager
    {
        Tree.TreeLookup _treeLookup = new Tree.TreeLookup();

      
        /// <summary>
        /// 这里用来装所有Inplay和所有赛事，分一个inplay子stem和多个赛事子stem
        /// </summary>
        Tree.Stem _root = new Tree.Stem();

        List<Tree.Stem> testStem = new List<Tree.Stem>();

      

        /// <summary>
        /// 这里用来保存要输出的附加的大小盘和附加的上半大小盘，还有附加的让球和上半让球 MA Stem
        /// </summary>
        List<Tree.Stem> l_MatchStem_MA_ForSubmit = new List<Tree.Stem>();

        

   

        /// <summary>
        /// 处理更新等信息，信息更新添加删除等，初始化先不在这里搞
        /// </summary>
        /// <param name="data"></param>
        void Update(string data)
        {
            //这是处理更新等信息
            //this.lb_Inplay.Items.Add("bbb");
            Tree.StemEvent e = new Tree.StemEvent(data);
            //switch (e.EventFlag)
            //{
            //    case Tree.StemEvent.DELETE:
            //        break;
            //    case Tree.StemEvent.FULL:
            //        break;
            //    case Tree.StemEvent.INSERT:
            //        break;
            //    case Tree.StemEvent.UPDATE:
            //        break;
            //}

            if (e.EventFlag == Tree.StemEvent.DELETE)
            {
                if (e.Trace.Count > 1)
                {
                    if (e.Trace[1] == "CL_1_10_0")
                    {
                        //Console.WriteLine(e.Trace[1]);
                    }
                }
            }

            Bet365.Data.Tree.Stem target_Stem = GetTargetStem(e.Trace);
            if (e.EventFlag == Tree.StemEvent.INSERT)
            {
                target_Stem = GetTargetStem(e.Trace.GetRange(0, e.Trace.Count - 1));
            }

            if (null == target_Stem && e.EventFlag != Tree.StemEvent.INSERT)
            {
                return;
            }

            if (e.EventFlag == Tree.StemEvent.DELETE)
            {
                Tree.Stem parentstem = target_Stem.Parent;
                if (null == target_Stem.Name || "" == target_Stem.Name)
                {
                    return;
                }
                parentstem.RemoveChildStem(target_Stem.Properties["IT"]);
            }
            else if (e.EventFlag == Tree.StemEvent.INITIAL)
            {
                //TODO:Inplay 初始化数据
            }
            else if (e.EventFlag == Tree.StemEvent.INSERT)
            {
                e.STEM.Parent = target_Stem;
                if (target_Stem != null)
                {
                    if (target_Stem.Properties["NA"] == "足球")
                    {
                        //TODO:添加一场赛事获取
                        Console.WriteLine("添加一场赛事获取");
                    }
                }
            }
            else if (e.EventFlag == Tree.StemEvent.UPDATE)
            {
            //    Console.WriteLine("修改  :  {0}", target_Stem.Name);
                target_Stem.Properties.Update(e.PROPERTY);

            }
        }

        /// <summary>
        /// 获取更新数据指定路径Stem
        /// </summary>
        /// <param name="trace"></param>
        /// <returns></returns>
        Tree.Stem GetTargetStem(List<string> trace)
        {
            Bet365.Data.Tree.Stem target_Stem = _root;
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
