using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.PushTechnology.Diffusion
{
    internal class ProtocolDecoder
    {
        private byte[] theBuffer;
        private Util.FastCrypt theCrypto;
        private bool hasAborted;
        private int theMessageSize;



        public ProtocolDecoder(int msgsize)
        {
            theMessageSize = msgsize;
            theCrypto = new Util.FastCrypt();
            theBuffer = new byte[]{};
            hasAborted=false;
        }




        private void compact(byte[] tembyte)
        {

        }





    }
}
