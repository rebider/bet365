using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace Bet365.Util
{
    internal class HttpHelper
    {

        public string HeadCookie { get; set; }


        public  CookieContainer cookieContainer { get; set; }

        public HttpHelper()
        {
            this.cookieContainer = new CookieContainer();
        }


        #region 同步方法
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public System.Drawing.Image GetImage_proxy(string url, string cookie,string proxy_address,string proxy_port)
        {
            try
            {
                using (XWebClient client = new XWebClient() { Cookies=this.cookieContainer})
                {
                    Uri uri = new Uri(url);
                    client.Headers.Add("Accept: */*");
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    // client.Headers.Add("Accept-Encoding: gzip, deflate");
                    client.Headers.Add("Cookie", cookie);
                    client.Proxy = null;
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    //调用代理地址请求
               //     client.Proxy = new WebProxy(string.Format("http://{0}:{1}",proxy_address,proxy_port), true);
               //     client.Proxy = new WebProxy(string.Format("http://{0}:{1}", "58.242.42.91", "3128"), true);//59.39.145.178 3128
                    var d = client.DownloadData(uri);
                    HeadCookie = client.ResponseHeaders["Set-Cookie"];
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(d))
                    {
                        return System.Drawing.Image.FromStream(ms);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                if (e.ToString().Contains("403"))
                {
                    //TODO 验证码 403错误
                    //throw e;
                }
                return null;
            }
        }


        /// <summary>
        /// GET模拟 (代理)
        /// </summary>
        /// <param name="Headers"></param>
        /// <param name="Url"></param>
        public string WebClientGET_proxy(Dictionary<String, String> Headers, Encoding _Encoding, string Url, string proxy_address, string proxy_port)
        {
            try
            {
                using (XWebClient myWebClient = new XWebClient() { Cookies=this.cookieContainer})
                {
                    if (Headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    //调用代理地址请求
                    myWebClient.Proxy = new WebProxy(string.Format("http://{0}:{1}", proxy_address, proxy_port), true);
                    var d = myWebClient.DownloadData(Url);
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];


                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// POST模拟 (代理)
        /// </summary>
        /// <param name="Headers"></param>
        /// <param name="Url"></param>
        /// <param name="postStr"></param>
        public string WebClientPOST_proxy(Dictionary<String, String> Headers, Encoding _Encoding, string Url, string postStr, string proxy_address, string proxy_port)
        {
            try
            {
                using (XWebClient myWebClient = new XWebClient() { Cookies=this.cookieContainer})
                {
                    if (Headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    //调用代理地址请求
                    myWebClient.Proxy = new WebProxy(string.Format("http://{0}:{1}", proxy_address, proxy_port), true);
                    var d = myWebClient.UploadData(Url, "POST", System.Text.Encoding.Default.GetBytes(postStr));
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];
                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



        #endregion



        #region 备份
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public System.Drawing.Image GetImage(string url, string cookie)
        {
            try
            {
                using (XWebClient client = new XWebClient() { Cookies=this.cookieContainer})
                {
                    Uri uri = new Uri(url);
                    client.Headers.Add("Accept: */*");
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    // client.Headers.Add("Accept-Encoding: gzip, deflate");
                    client.Headers.Add("Cookie", cookie);
                    client.Proxy = null;
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    var d = client.DownloadData(uri);
                    HeadCookie = client.ResponseHeaders["Set-Cookie"];
                     
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(d))
                    {
                        return System.Drawing.Image.FromStream(ms);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }


        /// <summary>
        /// GET模拟
        /// </summary>
        /// <param name="Headers"></param>
        /// <param name="Url"></param>
        public string WebClientGET(Dictionary<String, String> Headers, Encoding _Encoding, string Url)
        {
            try
            {
                using (XWebClient myWebClient = new XWebClient() { Cookies=this.cookieContainer})
                {
                    if (Headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                      //      if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length"))
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length") ||!pair.Key.Contains("Cookie"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    var d = myWebClient.DownloadData(Url);
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];


                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string WebClientGET2(Dictionary<String, String> Headers, Encoding _Encoding, string Url)
        {
            try
            {
                using (WebClient myWebClient = new WebClient() )
                {
                    if (Headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length") || !pair.Key.Contains("Cookie"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    var d = myWebClient.DownloadData(Url);
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];


                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// POST模拟
        /// </summary>
        /// <param name="Headers"></param>
        /// <param name="Url"></param>
        /// <param name="postStr"></param>
        public string WebClientPOST(Dictionary<String, String> Headers, Encoding _Encoding, string Url, string postStr)
        {
            try
            {
                //using (XWebClient myWebClient = new XWebClient() { Cookies=this.cookieContainer})
                using (XWebClient myWebClient = new XWebClient() { Cookies = this.cookieContainer })
                {
                    if (Headers.Count > 0 )
                   // if (false)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length") || !pair.Key.Contains("Cookie"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    var d = myWebClient.UploadData(Url, "POST", System.Text.Encoding.Default.GetBytes(postStr));
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];
                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string WebClientPOST2(Dictionary<String, String> Headers, Encoding _Encoding, string Url, string postStr)
        {
            try
            {
                using (WebClient myWebClient = new WebClient())
                {
                    if (Headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> pair in Headers)
                        {
                            if (!pair.Key.Contains("Host") || !pair.Key.Contains("Content-Length"))
                            {
                                myWebClient.Headers.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;//验证服务器证书回调自动验证
                    var d = myWebClient.UploadData(Url, "POST", System.Text.Encoding.Default.GetBytes(postStr));
                    HeadCookie = myWebClient.ResponseHeaders["Set-Cookie"];
                    return _Encoding.GetString(d);
                }
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion


        public Dictionary<string, string> getHeader()
        {
            Dictionary<string, string> dce = new Dictionary<string, string>();
            dce.Add("Accept", "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            dce.Add("User-Agent", "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.47 Safari/536.11");
            dce.Add("Content-Type", "application/x-www-form-urlencoded");

            return dce;
        }


        public static Dictionary<string, string> GetHeader()
        {
            Dictionary<string, string> dce = new Dictionary<string, string>();
            dce.Add("Accept", "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            dce.Add("User-Agent", "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.47 Safari/536.11");
            dce.Add("Content-Type", "application/x-www-form-urlencoded");

            return dce;
        }

        //for 2.0
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {   //   Always   accept   
            return true;
        }



        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public string RegexSet(string IntoRegex, string stopRegex, string EndRegex)
        {
            try
            {
                return Regex.Match(IntoRegex, String.Format(@"{0}[\w|\W]*?{1}", stopRegex, EndRegex), RegexOptions.IgnoreCase).Value.Replace(stopRegex, "").Replace(EndRegex, "");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




        #region 获取匹配项
        /// <summary>
        /// 获取匹配项
        /// </summary>
        /// <param name="content"></param>
        /// <param name="regStr"></param>
        /// <returns></returns>
        public string regextest(string content, string regStr)
        {
            string regular_str = regStr;
            //<div class="tb-detail-hd">
            //@"(?=<form[^>]*id=[""']?bid-form[\s\S]*>)[\s\S]*(?<=</form>)";
            Regex r = new Regex(regular_str);
            Match m = r.Match(content);

            if (m.Success)
            {
                //product_Name.Text = m.Value;
                return m.Value;
            }
            return "";

        }
        #endregion


        sealed class XWebClient : WebClient
        {
            #region 属性 / 变量
            // Cookie 容器
            private CookieContainer cookieContainer;

            /// <summary>
            /// 创建一个新的 WebClient 实例。
            /// </summary>
            public XWebClient()
            {
                this.cookieContainer = new CookieContainer();
            }
            /// <summary>
            /// 创建一个新的 WebClient 实例。
            /// </summary>
            /// <param name="cookie">Cookie 容器</param>
            public XWebClient(CookieContainer cookies)
            {
                this.cookieContainer = cookies;
            }
            /// <summary>
            /// Cookie 容器
            /// </summary>
            public CookieContainer Cookies
            {
                get { return this.cookieContainer; }
                set { this.cookieContainer = value; }
            }


            #endregion

            #region 方法

            /*
            /// <summary>
            /// 返回带有 Cookie 的 HttpWebRequest。
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    HttpWebRequest httpRequest = request as HttpWebRequest;
                    BugFix_CookieDomain(Cookies);

                    httpRequest.CookieContainer = cookieContainer;
                }
                return request;
            }

            protected override WebResponse GetWebResponse(WebRequest request)
            {
                var rp =  base.GetWebResponse(request);

                foreach (Cookie tem_cookie in ((HttpWebResponse)rp).Cookies)
                {
                    BugFix_CookieDomain(Cookies);
                    if (tem_cookie.Domain.StartsWith("."))
                    {
                        Cookies.Add(new Uri("http://" + tem_cookie.Domain.Substring(1) + "/"), tem_cookie);

                    }
                    else
                    {
                        Cookies.Add(new Uri("http://" + tem_cookie.Domain + "/"), tem_cookie);
                    }
                    BugFix_CookieDomain(Cookies);

                }

               
                var lc = GetAllCookies(Cookies);

                Console.WriteLine(lc.Count);

                return rp;
            }
            */
            public static List<Cookie> GetAllCookies(CookieContainer cc)
            {
                List<Cookie> lstCookies = new List<Cookie>();

                Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

                foreach (object pathList in table.Values)
                {
                    SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                    foreach (CookieCollection colCookies in lstCookieCol.Values)
                        foreach (Cookie c in colCookies) lstCookies.Add(c);
                }

                return lstCookies;
            }

            private void BugFix_CookieDomain(CookieContainer cookieContainer)
            {
                System.Type _ContainerType = typeof(CookieContainer);
                Hashtable table = (Hashtable)_ContainerType.InvokeMember("m_domainTable",
                                           System.Reflection.BindingFlags.NonPublic |
                                           System.Reflection.BindingFlags.GetField |
                                           System.Reflection.BindingFlags.Instance,
                                           null,
                                           cookieContainer,
                                           new object[] { });
                ArrayList keys = new ArrayList(table.Keys);
                foreach (string keyObj in keys)
                {
                    string key = (keyObj as string);
                    if (key[0] == '.')
                    {
                        string newKey = key.Remove(0, 1);
                        table[newKey] = table[keyObj];
                    }
                }
            }

            #endregion

            
        }
    }
}
