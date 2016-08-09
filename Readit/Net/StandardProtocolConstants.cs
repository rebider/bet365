using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Readit.Net
{

    /// <summary>
    /// 标识符
    /// </summary>
    public class StandardProtocolConstants
    {
       public  const int  BASE64_ENCODING= 19;
        public const  int ACK_DELTA= 31;
        public  const   int ENCRYPTED_ENCODING = 17;
        public  const   int COMPRESSED_ENCODING = 18;
        public  const   int ACK_RESPONSE = 32;
        public  const  int  INITIAL_TOPIC_LOAD = 20;
        public  const   int  DELTA = 21;
        /// <summary>
        /// 分段标识符1
        /// </summary>
        public   const  String RECORD_DELIM = "\x01";
        public  const   int CLIENT_CLOSE = 29;
        /// <summary>
        /// 分段标识符2
        /// </summary>
        public   const  String FIELD_DELIM= "\x02";
        public  const   int CLIENT_ABORT = 28;
        public  const  int  CLIENT_PING = 25;
        public  const   int ACK_ITL = 30;
        public  const   int CLIENT_POLL = 1;
        public  const   int SERVER_PING = 24;
        public   const   int CLIENT_SUBSCRIBE = 22;
        public  const  String MESSAGE_DELIM= "\b";
        public  const   int NONE_ENCODING = 0;
        public  const   int CLIENT_SEND = 2;
        public   const   int  CLIENT_UNSUBSCRIBE = 23;

    }
}
