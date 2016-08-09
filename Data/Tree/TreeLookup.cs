using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    /// <summary>
    /// 这里是用来存放和查找数据的，相当于缓存
    /// 所有初始化和更新删改的数据都提交到这里
    /// 这里有空要搞个单例
    /// </summary>
    public class TreeLookup
    {
        public Stem Root
        {
            get;
            set;
        }

        public Stem[] Stem
        {
            get;
            set;
        }
        
      //  private object table;
        //这里可能需要自定义一个Dictionary重写一个ToString()方法
        private Dictionary<string, object> table;

        public TreeLookup()
        {
           //table={};
        }

        public void removeAllReferences()
        {
            table = null;
            table = new Dictionary<string,object>();
        }

        public object getReference(string p1)
        {
            return table[p1];
        }

        public void removeReference(string p1)
        {
            object o = table[p1];
            if (o!=null)
            {
                table.Remove(p1);
                //table.hasLookupRefencnce=false;
            }
        }

        public void addReferenct(ILookupClient p1,string p2="IT")
        {
            //TODO: 这个方法都不知道什么意思
           var o = p1.data[p2];
           if (o != null)
           {
               table[o.ToString()] = p1;
               p1.hasLookupReferenct = true;
           }
        }


        /*
         * final public function addReference(param1:ILookupClient,param2:String = "IT"):void
         * {
         *  var _loc_3:* = param1.data[param2];
         *  if(_loc_3)
         *  {
         *      table[_loc_3]=param1;
         *      param1.hasLookupReference = true;
         *  }
         *  return;
         * }
         */ 
    }
}
