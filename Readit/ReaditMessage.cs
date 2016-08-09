using System;
using System.Collections.Generic;
using System.Text;
using Bet365.Readit.Net;

namespace Bet365.Readit
{
    public class ReaditMessage
    {
        public int MessageType { get; set; }

        private byte[] _message;
        private string _topic;
        private string[] _userHeaders;
        private string[] _rows;

        public ReaditMessage(int msgType,string topic,byte[] msg,string[] userhead) 
        {
            this.MessageType = msgType;
            this._topic = topic;
            this._message = msg;
            this._userHeaders = userhead;

        }

        public Byte[] MessageBytes { get { return _message; } }

        public string[] UserHeaders { get { return _userHeaders; } }

        public string Message { get { return Encoding.UTF8.GetString(_message); } }

        public int getNumberOfRecords()
        {
            return getRecords().Length;
        }

        public string Topic { get { return _topic; } }


        public string[] getFields( int param1 )
        {
            var _loc_2 = getRecords()[param1].Split(new string[]{StandardProtocolConstants.FIELD_DELIM},StringSplitOptions.RemoveEmptyEntries);
            return _loc_2;
        }


        private  string[] getRecords()
        {
            if (_rows == null)
            {
                _rows = Message.Split(new string[] { StandardProtocolConstants.RECORD_DELIM }, StringSplitOptions.RemoveEmptyEntries);
            }
            return _rows;
        }

        public string BaseTopic { get { return _topic.Substring(_topic.IndexOf("/") + 1, _topic.Length); } }
        

    }
}
