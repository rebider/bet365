using System.Collections.Generic;
using System;
using System.Text;

namespace Bet365.PushTechnology.Diffusion
{
    class MessageFactory
    {
         string sessionID { get; set; }

        byte[] byte_sessionID { get { return System.Text.Encoding.ASCII.GetBytes(sessionID); } }


        public void GetSessionId()
        {
            throw  new KeyNotFoundException();
            //Console.WriteLine(Bet365.ReadIt.Net.StandardProtocolConstants.FIELD_DELIM);
            //return;
            //Tool.HttpHelper hh = new Tool.HttpHelper();
            //string tem_sessionId = hh.WebClientGET(hh.getHeader(), System.Text.Encoding.UTF8, "https://www.bet365.com/home/FlashGen4/WebConsoleApp.asp?&cb=105812123906",false);
            ////sessionID = tem_sessionId;
            ////Console.WriteLine(tem_sessionId);
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"pstk=(?<pstk>[\S]+);", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.Singleline);
            //System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(hh.HeadCookie);
            //foreach (System.Text.RegularExpressions.Match match in matchCollection)
            //{
            //    sessionID = match.Groups["pstk"].Value;
            //    System.Console.WriteLine(sessionID);
            //    break;
            //}

            return;

        }

        public byte[] getByte_Time()
        {
        //    byte[] bytes = new byte[] { 35, 1, 23, 7, 95, 95, 116, 105, 109, 101, 0 };

            List<byte> tem_nb = new List<byte>();
            //     byte[] tem_time = new byte[] {0x23, 0x01, 0x17, 0x07, 0x5f, 0x5f, 0x74, 0x69, 0x6d, 0x65, 0x2c, 0x53, 0x5f, 0x31, 0x43, 0x30, 0x31, 0x34, 0x45, 0x45, 0x41, 0x42, 0x31, 0x44, 0x39, 0x41, 0x45, 0x32, 0x36, 0x39, 0x32, 0x31, 0x32, 0x36, 0x36, 0x30, 0x34, 0x46, 0x32, 0x38, 0x44, 0x44, 0x44, 0x32, 0x43, 0x30, 0x30, 0x30, 0x30, 0x30, 0x33, 0x00  };
            byte[] tem_time = new byte[] { 0x23, 0x01, 0x17, 0x07, 0x5f, 0x5f, 0x74, 0x69, 0x6d, 0x65, 0x2c, 0x53, 0x5f, 0x00 };
            tem_nb.AddRange(tem_time);
            tem_nb.InsertRange(13, byte_sessionID);
            return tem_nb.ToArray();
           // return bytes;
        }

        public byte[] getInplay_ByteArray()
        {
            byte[] bytes = new byte[] { 
                0x00, 0x00, 0x00, 0x14, 0x16, 
0x00, 0x49, 0x6e, 0x50, 0x6c, 0x61, 0x79, 0x5f, 
0x31, 0x30, 0x5f, 0x30, 0x2f, 0x2f, 0x01
            };
            //这里可能有错
            return bytes;
        }


        public byte[] getRelease(string it)
        {
            byte[] bit = getID_ByteArray(it);
            bit[4] = 0x17;
            return bit;
        }

        /// <summary>
        /// 得到发送ID数据请求的byte[]数据
        /// ******数据包里面的长度标识要动态改变******
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] getID_ByteArray(string id)
        {
            /*
              0x00, 0x00, 0x00, 0x17, 0x16, 0x00, 0x32, 0x35, 
0x34, 0x34, 0x32, 0x30, 0x35, 0x30, 0x41, 0x5f, 
0x31, 0x30, 0x5f, 0x30, 0x2f, 0x2f, 0x01
             */
            byte[] b_id = System.Text.Encoding.UTF8.GetBytes(id);
            List<byte> l_bs = new List<byte>();

            l_bs.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x17, 0x16, 0x00 });
            l_bs.AddRange(b_id);
            l_bs.AddRange(new byte[] { 0x2f, 0x2f, 0x01 });

            //这里检验byte长度并修改前四位长度标识
            int lenght = l_bs.Count;
            byte[] index = intToByte(lenght);
            for (int i = 0; i < 4; i++)
            {
                l_bs[i] = index[3 - i];
            }
            return l_bs.ToArray();

        }

        public string getAsiaQuaterID(string orginalStr,string type)
        {
            /*
                |EV;ID=25466646A_10_0;ET=3;IT=L25466646A_10_0;IP=CL_1_10_0;NA=上半场亚洲滚球盘;OR=8;EL=0;SS=;AU=0;VI=0;MS=0;SD=;VS=;HP=0;DO=0;HO=0;RO=0;SE=0;C1=1;C2=10170;C3=0;T1=5;T2=12;T3=0;IM=;BC=;CT=全部比赛;IF=;EA=;CB=;MO=;SY=;
|EV;ID=23768652A_10_0;ET=2;IT=L23768652A_10_0;IP=CL_1_10_0;NA=亚洲滚球;OR=9;EL=0;SS=;AU=0;VI=0;MS=0;SD=;VS=;HP=0;DO=0;HO=0;RO=0;SE=0;C1=1;C2=10147;C3=0;T1=5;T2=14;T3=0;IM=;BC=;CT=全部比赛;IF=;EA=;CB=;MO=;SY=;
|EV;ID=23768650A_10_0;ET=1;IT=L23768650A_10_0;IP=CL_1_10_0;NA=现场投注选项;OR=10;EL=0;SS=;AU=0;VI=0;MS=0;SD=;VS=;HP=0;DO=0;HO=0;RO=0;SE=0;C1=1;C2=737;C3=0;T1=5;T2=198;T3=0;IM=;BC=;CT=全部比赛;IF=;EA=;CB=;MO=;SY=;
                 */
            string[] lines = orginalStr.Split(new char[] { '|' });
            string id = "";
            foreach (string str in lines)
            {
               // if (str.Contains("NA=亚洲滚球"))
                if (str.Contains(type))
                {

                    Bet365.Util.DataProcress hh = new Bet365.Util.DataProcress();
                    //TODO: !!!!这里要改一下，获取id的正则
                    //id = hh.RegexSet(str, "EV;ID=", ";ET");
                    if (str.StartsWith("EV;"))
                    {
                        id = hh.RegexSet(str, "ID=", ";");
                        System.Console.WriteLine(id);
                        return id;
                    }
                }
            }
            return id;
        }

        public byte[] getMoreID_ByteArray(string orginalStr)
        {
            /* demo
             0x00, 0x00, 0x00, 0x18, 0x16, 0x00, 0x32, 0x36, 
0x33, 0x34, 0x37, 0x35, 0x38, 0x38, 0x53, 0x41, 
0x5f, 0x31, 0x30, 0x5f, 0x30, 0x2f, 0x2f, 0x01,  
             */
            List<byte> l_ids = new List<byte>();
            Bet365.Util.DataProcress hh = new Bet365.Util.DataProcress();
            string[] lines = orginalStr.Split(new char[] { '|' });
            int num = 0;

            byte[] head = new byte[] { 0x00, 0x00, 0x00, 0x18, 0x16, 0x00 };
            byte[] food = new byte[] { 0x2f, 0x2f, 0x01 };

            foreach (string lstr in lines)
            {
                if (lstr.StartsWith("LG"))
                {
                    num++;
                    string tl = hh.RegexSet(lstr, "TL=", ";");
                    List<byte> temp = new List<byte>();
                    byte[] content = System.Text.Encoding.UTF8.GetBytes(tl);
                    temp.AddRange(head);
                    temp.AddRange(content);
                    temp.AddRange(food);

                    int length = temp.Count;
                    byte[] lentem = intToByte(length);

                    for (int i = 0; i < 4; i++)
                    {
                        temp[i] = lentem[3 - i];
                    }

                    l_ids.AddRange(temp.ToArray());

                }
            }
            //这里是一个模块,不用控件统一显示所以不用这个
         //   gameNumber = num;
            return l_ids.ToArray();
        }

        public byte[] intToByte(int i)
        {

            byte[] abyte0 = new byte[4];

            abyte0[0] = (byte)(0xff & i);

            abyte0[1] = (byte)((0xff00 & i) >> 8);

            abyte0[2] = (byte)((0xff0000 & i) >> 16);

            abyte0[3] = (byte)((0xff000000 & i) >> 24);

            return abyte0;

        }


        public byte[] getMedia_L10_Z0_BygeArray()
        {
            byte[] peer0_4 = {
0x00, 0x00, 0x00, 0x19, 0x16, 0x00, 0x36, 0x56, 
0x35, 0x32, 0x33, 0x30, 0x32, 0x39, 0x35, 0x30, 
0x43, 0x31, 0x41, 0x5f, 0x31, 0x30, 0x5f, 0x30, 
0x01 };
            return peer0_4;
        }


        /// <summary>
        /// 获取ID转化BYTE[] 值
        /// </summary>
        /// <param name="legacyid">eg. 123564654M1_10_0</param>
        /// <returns></returns>
        public byte[] GetLegacyID(string legacyid)
        {
            byte[] b_id = System.Text.Encoding.UTF8.GetBytes(legacyid);
            List<byte> l_bs = new List<byte>();

       //     l_bs.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x17, 0x16, 0x00 });
            l_bs.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x19, 0x16, 0x00 });
            l_bs.AddRange(b_id);
            l_bs.AddRange(new byte[] { 0x01 });

            //这里检验byte长度并修改前四位长度标识
            int lenght = l_bs.Count;
            byte[] index = intToByte(lenght);
            for (int i = 0; i < 4; i++)
            {
                l_bs[i] = index[3 - i];
            }
            return l_bs.ToArray();
        }

        public byte[] GetlegacyIDByte(string fullStr,ref string strlid)
        {
            string ld = GetLegacyIDString(fullStr) + "M1_10_0";
            strlid = ld;
            byte[] b_id = GetLegacyID(ld);
            return b_id;

        }

         string GetLegacyIDString(string fullStr)
        {
            string c1 = GetProperty(fullStr, "C1");
            string c2 = GetProperty(fullStr, "C2");
            string t1 = GetProperty(fullStr, "T1");
            string t2 = GetProperty(fullStr, "T2");
            string lid = c1 + t1 + c2 + t2;
        //    return "15276846782";  
            return lid;
        }

        /// <summary>
        /// 通过一行信息获取其中单个属性
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        string GetProperty(string fullStr, string pn)
        {
            string reg = @"C1=(?<PN>.*?);";
            reg = reg.Replace("C1",pn);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reg, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.Singleline);
            System.Text.RegularExpressions.Match match = regex.Match( fullStr );
            if (match.Groups.Count > 0)
            {
                return match.Groups["PN"].Value;
            }
            return "";

        }


        /// <summary>
        /// 这个用于计算分数转小数,自动给结果+1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string caculate(string str)
        {
            try
            {
                string result = new System.Data.DataTable().Compute(str, "0.000").ToString();
                if (result.Length <= 5)
                {
                   
                    //return result;
                }
                else
                {
                    result = result.Substring(0, 5);
                    //return result;
                }
                if (result != "")
                {
               //     double db_result = Double.Parse(result) + 1;
                    double db_result = System.Double.Parse(result);
                    result = db_result + "";
                }
                return result;
                
            }
            catch (System.Exception ex)
            {
                return str;
            }


            if (str == "") { return ""; }
            string num = new System.Data.DataTable().Compute(str, "").ToString();
            double n = double.Parse(num) + 1.00;
            num = n.ToString();
            string[] ns = num.Split(new char[] { '.' });
            if (ns.Length > 1)
            {
                if (ns[1].Length >= 2)
                {
                    ns[1] = ns[1].Substring(0, 2);
                    return string.Format("{0}.{1}", ns[0], ns[1]);
                }
            }
            return num;


        }






        #region 克隆一个对象
        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object CloneObject(object o)
        {
            System.Type t = o.GetType();
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            System.Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (System.Reflection.PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }







        #endregion

    }
}
