using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Data.Tree
{
    public interface IStemDisplay
    {
        //public function IStemDisplay();

        Stem stem
        {
            get;
            set;
        }

        void detachStem();

        string topic
        {
            get;
            set;
        }
    
    }
}
