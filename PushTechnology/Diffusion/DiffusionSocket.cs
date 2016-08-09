using System;
using System.Collections.Generic;
using System.Text;
using NewLife.Net;

namespace Bet365.PushTechnology.Diffusion
{
    /// <summary>
    ///  建立链接步骤
    ///  先发送 time  ＋sessionID
    ///  接收 Remote => time , 发送Topic ID
    ///  此后放可监听SocketData 
    /// </summary>
   internal class DiffusionSocket
  {
      #region extend var
       string _sessionID;
       public string sessionID { get { return Bet365.Util.DataProcress.GetSessionId(); } set { _sessionID = value; } }
      byte[] byte_sessionID { get { return System.Text.Encoding.ASCII.GetBytes(sessionID); } }
      /// <summary>
      /// 每次receive整理后的buffer都加在这里，然后从这里取出每一组数据
      /// </summary>
      List<byte> Ltemp_buffer = new List<byte>();
      private int buffer_size = 0;

      private string legacID;
      /// <summary>
      ///  计数接收次数，如果大于10还未转码成String进行处理，
      ///  则数据可能产生混乱，重新进行连接，
      ///  每成功处理一条String数据 ，改变量清零，
      ///  每次接受一次Byte[]数据 +1 
      /// </summary>
      int UnProcessRecCount = 0;
      private System.Threading.ManualResetEvent timeoutObject;

      #endregion


      public event EventHandler<EventArgs> onConnectSuccess;
      public event EventHandler<EventArgs> onMessage;

      public event EventHandler<EventArgs> onInplayMessage;
      public event EventHandler<EventArgs> onMatchMessage;

      /// <summary>
      /// 改事件仅做断连通知用途，不传递任何信息
      /// 用来传递给SocketClient
      /// </summary>
      public event EventHandler<EventArgs> onSocketError;


      void CallSocketError()
      {
          if (onSocketError != null)
          {
              onSocketError(this, new EventArgs());
          }
      }

      void CallConnectSuccess()
      {
          UnProcessRecCount = 0;
          if (onConnectSuccess != null)
          {
              onConnectSuccess(this, new EventArgs());
          }
      }

      void CallonMessage(string msg)
      {
          UnProcessRecCount = 0;
          if (onMessage != null)
          {
              onMessage(this, new Events.DiffusionMessageEvent(msg));
          }
      }

      void ProcessInplay(string data)
      {
          UnProcessRecCount = 0;
          if (onInplayMessage != null)
          {
              //onProcessInplay(sender, events);
              onInplayMessage(this ,new Events.DiffusionMessageEvent(data));
          }
      }

      //void ProcessMatch(object sender, Bet365.Data.Tree.StemEvent events)
      void ProcessMatch(string data)
      {
          UnProcessRecCount = 0;
          if (onMatchMessage != null)
          {
              onMatchMessage(this, new Events.DiffusionMessageEvent(data));
          }
      }


      #region 内部变量
      private bool isConnectedFlag = false;
        private Util.FastCrypt theCrypto;
        private  int thePort;
        private  int theConnectionTimeout = 5000;
        private  string theClientID;
        private  string theHost;
        private  string theTopic;
         private  NewLife.Net.Tcp.TcpClientX theSocket;
        private NewLife.Net.Sockets.ISocketSession theSocketSession;

      /// <summary>
      /// 证书
      /// </summary>
        private DiffusionClientCredentials  theCredentials = null;
        private System.Threading.Timer theConnectionTimer= null;
        private  bool isTimeout = false;
        private ProtocolDecoder theProtocolDecoder;
        private  int theMessageSize = 4;
      #endregion


        System.Threading.Timer _timeTicket;
        bool ConnectIng { get; set; }

     
       
       

        public DiffusionSocket(string host,int port,string topic)
        {
            theCrypto= new Util.FastCrypt();
            theHost=host;
            thePort=port;
            if (topic == "null" || topic == null || string.IsNullOrEmpty(topic))
            {
            }
            else
            {
                theTopic = topic;
            }

            timeoutObject = new System.Threading.ManualResetEvent(false);

        //    theSocket = new NewLife.Net.Tcp.TcpClientX();
     //       theSocket.BufferSize = 2000;

        }

        protected NewLife.Net.Sockets.ISocketSession _Session;
       /// <summary>
       /// Setp 1  发送 Time + sessionID
       /// </summary>
        public void Connect()
        {
            //theConnectionTimer = new System.Threading.Timer(,,,
   rc:         Console.WriteLine( "开始连接");
                
                Ltemp_buffer.Clear();
                UnProcessRecCount = 0;
            ConnectIng = true;
            //if (theSocket == null || theSocket.Disposed == true)
            //{
            //if (theSocket != null)
            //{
            //    theSocket.Dispose();
            //}
                NetUri uri = new NetUri(System.Net.Sockets.ProtocolType.Tcp, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("5.226.180.15"), 843));

            if (theSocket!=null&&!theSocket.Disposed)
            {
                return;
                theSocket = new NewLife.Net.Tcp.TcpClientX();
            }
                theSocket = new NewLife.Net.Tcp.TcpClientX();
                theSocket.Received += new EventHandler<NewLife.Net.Sockets.NetEventArgs>(theSocket_sendTopic);
                theSocket.Error += new EventHandler<NewLife.Net.ExceptionEventArgs>(theSocket_Error);
            //}
            Console.WriteLine("thesocket.connect");
            //theSocket.Connect(theHost, thePort);
            if (!theSocket.BeginConnect(new System.Net.IPEndPoint(NetHelper.ParseAddress(this.theHost), this.thePort), 10000))
            {
                Close();
                goto rc;
            }
            
            CallConnectSuccess();
            var tem_times = getTime();
            
                theSocket.Send(tem_times, 0, tem_times.Length);// Send time
                theSocket.ReceiveAsync();


      //      theSocket.Client.SendTimeout = 10 * 1000;
                timeoutObject.Close();
                timeoutObject = new System.Threading.ManualResetEvent(false);

                if (!timeoutObject.WaitOne(5000, false))
                {
                    Console.WriteLine("Canot Receive");
                    Close();
                    goto rc;
                }
                _Session = theSocket.CreateSession();
         //_timeTicket = new System.Threading.Timer(new System.Threading.TimerCallback(SendTime),theSocket, 10000, 1 * 3 * 1000);
        }

 

        void theSocket_sendTopic(object sender, NewLife.Net.Sockets.NetEventArgs e)
        {
            ConnectIng = false;
         //   theSocket.Client.SetTcpKeepAlive(true);
            //lock (this)
            //{
                if (!theTopic.Contains("InPlay"))
                {
                    Console.WriteLine(theTopic);
                }

                NewLife.Net.Tcp.TcpClientX tcx = sender as NewLife.Net.Tcp.TcpClientX;
                NetHelper.SetTcpKeepAlive(tcx.Client, true, 1000, 1000);
            
            
                tcx.Received -= new EventHandler<NewLife.Net.Sockets.NetEventArgs>(theSocket_sendTopic);
                tcx.Received += new EventHandler<NewLife.Net.Sockets.NetEventArgs>(theSocket_handshakeHandler);
                #region edit
                List<byte> _loc_3 = new List<byte>();
                int _loc_5 = 0;
                System.IO.Stream st = e.GetStream();
                if (st.ReadByte() != 35)
                {
                    Console.WriteLine("Invalid Protocol");

                }
                var b2 = st.ReadByte();
                var b3 = st.ReadByte();
                theMessageSize = st.ReadByte();
                int _loc_9 = 0;
                switch (b3)
                {
                    case 111:
                        Console.WriteLine("Connection Rejected");
                        break;
                    case 100:
                        do
                        {
                            _loc_3.Add((byte)_loc_5);
                            _loc_9 = st.ReadByte();
                            _loc_5 = st.ReadByte();
                        } while (_loc_9 != 0);
                        var clid = Encoding.UTF8.GetString(_loc_3.ToArray());
                        Console.WriteLine("ClientID: {0}", clid);

                        if (_loc_5 == -1)//读完
                        {

                        }
                        else
                        {
                            string _time = Encoding.UTF8.GetString(st.ReadBytes());
                            Console.WriteLine("__time : {0}", _time);
                        }
                        //decode(st);

                        break;

                }



                #endregion
            
                MessageFactory tem_mf = new MessageFactory();
          
              //  byte[] bt_Topic = tem_mf.getID_ByteArray(theTopic);
                byte[] bt_Topic = tem_mf.GetLegacyID(theTopic);
                //    string ddd="";
             //       byte[] bt_Topic = tem_mf.GetlegacyIDByte("sss", ref ddd);


                tcx.Send(bt_Topic, 0, bt_Topic.Length);
      //          tcx.Send(bt_media, 0, bt_media.Length);
                timeoutObject.Set();
            //}
                
        }


        public void Close()
        {
            try
            {
                theSocket.Client.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                theSocket.Client.Disconnect(false);
              //  theSocket.Client.Close();
                Ltemp_buffer.Clear();
                NewLife.DisposeHelper.TryDispose(theSocket);

           //     _Session.Stream.Close();
                theSocket.Stream.Close();
                theSocket.Close();
                
                //theSocket.Dispose();
                ////theSocket.Client.Close();
                //_Session.Dispose();
                
                theSocket.Client = null;
                theSocket = null;
                
                
          //    theSocket.Client.p
                // _timeTicket.Dispose();
              //  theSocket.Dispose();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Ltemp_buffer.Clear();
            }
        }

        void theSocket_Error(object sender, NewLife.Net.ExceptionEventArgs e)
        {
            //throw new NotImplementedException();
            //NewLife.Net.Sockets.NetEventArgs se = sender as NewLife.Net.Sockets.NetEventArgs;
            //se.Dispose();
            Console.WriteLine("网络异常 : "+ this.theTopic);//+e.Exception);
            
           Close();
          //  Connect();

         //   Ltemp_buffer.Clear();
            CallSocketError();
        }

        void theSocket_handshakeHandler(object sender, NewLife.Net.Sockets.NetEventArgs e)
        {
            
            //if(theTopic.Contains("InPlay")){
            //    Console.WriteLine(theTopic);
            //}
            //lock (e.UserToken)
            //{
                if (++this.UnProcessRecCount > 50)
                {
                    Close();
                    Console.WriteLine("if (++this.UnProcessRecCount > 50)");
                    theSocket_Error(this, new NewLife.Net.ExceptionEventArgs());
                    return;
                }
                //lock (this)
                //{
                //if (!theTopic.Contains("InPlay"))
                //{
                //    Console.WriteLine(theTopic);
                //}




                NewLife.Net.Tcp.TcpClientX client = sender as NewLife.Net.Tcp.TcpClientX;
                Console.WriteLine("Topic:  {0}  ThreadID: {1}  ClientID:{2}  theSocket_handshakeHandler", theTopic, System.Threading.Thread.CurrentThread.ManagedThreadId, e.ID);
                //处理信息 赛事/Inplay 信息 之前已经进行完基本通讯，现在是开始处理数据
                // int num = e.BytesTransferred;
                //Console.WriteLine(num);


                /*
                 *   byte[] buffer = e.Buffer;
                int size = e.BytesTransferred; //s.Receive(buffer, buffer.Length, SocketFlags.None);
                buffer_size = size;

                List<byte> l_buffer = new List<byte>();
                for (int n = 0; n < size; n++)
                {
                    l_buffer.Add(buffer[n]);
                }
                buffer = l_buffer.ToArray();
                if (buffer[0] != 0)
                {
                    Console.WriteLine(buffer[0]);
                }
                */

                byte[] buffer = e.GetStream().ReadBytes();
                Ltemp_buffer.AddRange(buffer);
                //可能会剪错
                //   compactByteArray(0);
                int length = Ltemp_buffer.Count;
                // buffer.Length;

                #region test

                //     return;

                #endregion



                bool flag = false;

                while (!flag)
                {
                    if (Ltemp_buffer.Count >= 4)
                    {


                        byte[] len = new byte[4];
                        len[0] = Ltemp_buffer[0];
                        len[1] = Ltemp_buffer[1];
                        len[2] = Ltemp_buffer[2];
                        len[3] = Ltemp_buffer[3];
                        //UInt32 l1  = Ltemp_buffer[0];
                        //UInt32 l2  = Ltemp_buffer[1];
                        //  UInt32 l3= Ltemp_buffer[2];
                        //  UInt32 l4 = Ltemp_buffer[3];

                        int intn = len[0] * 256 * 256 * 256 + len[1] * 256 * 256 + len[2] * 256 + len[3];
                        //  UInt32 intn = l1 * 256 * 256 * 256 + l2 * 256 * 256 + l3 * 256 +l4;

                        //      if (buffer.Length >= intn) 
                        if (Ltemp_buffer.Count >= intn && intn > 0)
                        {
                            List<byte> l_singleMessage = new List<byte>();
                            for (int i = 0; i < intn; i++)
                            {
                                //   l_singleMessage.Add(buffer[i]);
                                l_singleMessage.Add(Ltemp_buffer[i]);
                            }
                            if (l_singleMessage.Count > 4)
                            {
                                processMessage(l_singleMessage.ToArray());
                            }

                            // if (intn != buffer.Length)
                            if (intn != Ltemp_buffer.Count)
                            {
                                try
                                {
                                    //compactByteArray(intn);
                                    compactByteArray((int)intn);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    //这里应该换个方法,直接抛出错误重新连接
                                    throw ex;
                                    //  buffer = new byte[3];
                                }
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                        continue;
                    }
                    flag = true;
                }
                //}
            
        }

        public void Send(byte[] btMsg)
        {
            if (theSocket.Client.Connected)
            {
                theSocket.Send(btMsg, 0, btMsg.Length);
            }
            //theSocket.Send(btMsg, 0, btMsg.Length);
            
        }




        #region exetndMethod

        byte[] getTime()
        {
            List<byte> tem_nb = new List<byte>();
            byte[] tem_time = new byte[] { 0x23, 0x01, 0x17, 0x07, 0x5f, 0x5f, 0x74, 0x69, 0x6d, 0x65, 0x2c, 0x53, 0x5f, 0x00 };
            tem_nb.AddRange(tem_time);
            tem_nb.InsertRange(13, byte_sessionID);
            return tem_nb.ToArray();
        }


        #region  剪裁数组，len为要剪掉前面的长度
        /// <summary>
        /// 剪裁数组，len为要剪掉前面的长度
        /// </summary>
        /// <param name="len"></param>
        private void compactByteArray(int len)
        //       private void compactByteArray(UInt64 len)
        {

            List<byte> l_bytes = new List<byte>();

            int compactIndex = len;

            if (Ltemp_buffer.Count > 0)
            {
                for (int i = 0; i < (Ltemp_buffer.Count - len); i++)
                {
                    l_bytes.Add(Ltemp_buffer[compactIndex + i]);
                    //compactIndex++;
                }
            }
            Ltemp_buffer.Clear();
            Ltemp_buffer.AddRange(l_bytes.ToArray());
        }
        #endregion


        #region 处理每一次根据长度剪裁出来的信息
        /// <summary>
        /// 处理每一次根据长度剪裁出来的信息
        /// </summary>
        /// <param name="message"></param>
         void processMessage(byte[] message)
        {
            lock (this)
            {
                byte[] len = new byte[4];
                len[0] = message[0];
                len[1] = message[1];
                len[2] = message[2];
                len[3] = message[3];

                int intn = len[0] * 256 * 256 * 256 + len[1] * 256 * 256 + len[2] * 256 + len[3];


                List<byte> l_topic = new List<byte>();
                List<byte> l_content = new List<byte>();
                int content_index = 0;

                //这里要先判断数据包的编码方式看第六位

                int coding = message[5];
                //buffer[5];

                if (0 == coding)
                {
                    for (int i = 6; i < message.Length; i++)
                    {
                        l_content.Add(message[i]);
                    }

                    string conts = Encoding.UTF8.GetString(l_content.ToArray());
             //       Console.WriteLine(conts);

                    // 这里开始处理信息
                    if (!conts.Contains("InPlay_10_0") && !conts.Contains("time"))
                    {
                        //   string[] arry_conts = conts.Split(new char[] { '|' });
                        string cont = conts; ;
                        //   SoccerInplay.Tool.OddLog.WritlLine(cont);

                        ProcessMatch(conts);

                       
                    }
                    else if (conts.Contains("time"))
                    {

                    }
                    else if (conts.Contains("InPlay_10_0"))//这里是更新比赛时间和比分的，
                    {
                        ProcessInplay(conts);
                    }

                }
                else if (18 == coding)
                {
                    for (int i = 6; i < message.Length; i++)
                    {
                        if (message[i] != 1)
                        {
                            l_topic.Add(message[i]);
                        }
                        else
                        {
                            content_index = i + 1;
                            break;
                        }
                    }

                    int contLength = intn - content_index;
                    for (int n = 0; n < contLength; n++)
                    {
                        l_content.Add(message[content_index]);
                        content_index++;
                    }

                    byte[] debyte = Util.ZlibCompress.DecompressBytes(l_content.ToArray());//该静态方法不存在线程安全问题，没有调用别的静态变量
                    if (debyte != null)
                    {
                        string content = Encoding.UTF8.GetString(debyte);
                        //写LOG
                        //  SoccerInplay.Tool.OddLog.WritlLine(content);
                        #region 测试
                        //  if ((t++)<2)//测试
                        if (false)
                        {
                            ////设置table数据

                            //string[] strs = content.Split(new char[] { '|' });
                            //Console.WriteLine(content);
                            //byte[] bbbb = new byte[] { };
                            //string id = httpHelper.RegexSet(strs[2], "ID=", ";");
                            //bbbb = mf.getID_ByteArray(id);
                            //tcx.Send(bbbb, 0, bbbb.Length);
                        }
                        #endregion

                        #region 初始化数据处理

                        if (content.StartsWith("F|EV"))//全局数据
                        {
                            ProcessMatch(content);
                            //if (false)
                            #region 不在这里发送 赛事请求 LegacID 会造成重复
                            //{
                                //if (this.legacID == "" || string.IsNullOrEmpty(this.legacID))
                                //{
                                //    MessageFactory tem_mf = new MessageFactory();
                                //    string lid = "";

                                //    byte[] lgid = tem_mf.GetlegacyIDByte(content, ref lid);
                                //    Console.WriteLine("LegacyID : {0}", lid);

                                    //if (lid.Length > 10)
                                    //{
                                    //    this.legacID = lid;
                                    //    this.theSocket.Send(lgid, 0, lgid.Length);
                                    //}
                                    //}
                            //}
                            #endregion

                        }
                        else if (content.StartsWith("F|CL"))//左边赛事菜单 Inplay
                        {
                            ProcessInplay(content);
                        }
                        else
                        {
                            ProcessMatch(content);
                        }
                        #endregion

                    }
                }
            }
        }
        #endregion



       void decode(System.IO.Stream st)
       {
           
           byte[] theBuffer = st.ReadBytes();
           bool _loc_2 = false;
           while (!_loc_2)
           {
               
           }
       }

        #endregion



  }
}
