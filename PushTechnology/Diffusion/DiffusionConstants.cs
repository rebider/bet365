using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.PushTechnology.Diffusion
{
    public class DiffusionConstants
    {
        const int BASE64_ENCODING = 19;
        const int ACK_DELTA = 31;
        const int ENCRYPTED_ENCODING = 17;
        const int COMPRESSED_ENCODING = 18;
        const int ACK_RESPONSE = 32;
        const int INITIAL_TOPIC_LOAD = 20;
        const int DELTA = 21;
        public const string RECORD_DELIM = "\x01";
        const int CLIENT_CLOSE = 29;
        const int CLIENT_PING = 25;
        public const string FIELD_DELIM = "\x02";
        const int CLIENT_ABORT = 28;
        const int ACK_ITL = 30;
        const int SERVER_PING = 24;
        const string MESSAGE_DELIM = "\b";
        const int NONE_ENCODING = 0;

    }
}
