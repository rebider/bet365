using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    public interface ILookupClient
    {
       // public ILookupClient();

        Dictionary<string,object> data
        {
            get;
            set;
        }

        bool hasLookupReferenct
        {
            get;
            set;
        }


        //bool hasLookupRefrence();

        /*
         * function get data() : Object;
         * 
         * function set data(param1:Object):void;
         * 
         * function set hasLookupReference(param1:Boolean):void;
         * 
         * function get hasLookupReference():Boolean;
         */ 

    }
}
