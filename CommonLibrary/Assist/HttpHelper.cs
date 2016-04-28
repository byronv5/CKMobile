using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommonLibrary.Assist
{
    public class HttpHelper
    {
        public static CookieContainer CookieContainers = new CookieContainer();
        public static string Ie7 = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";

        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string hg = reader.ReadToEnd();
                return hg;
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="url"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string GetResponse(string url, Encoding codeType)
        {
            LogHelper.WriteInfoLog("HTPP的GET请求url:" + url);
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.CookieContainer = CookieContainers;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = Ie7;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,**;q=0.8";
                req.Timeout = 50000;

                var res = (HttpWebResponse)req.GetResponse();
                using (Stream s = res.GetResponseStream())
                {
                    var reader = new StreamReader(s, codeType);
                    string hg = reader.ReadToEnd();
                    LogHelper.WriteInfoLog("HTPP的GET请求返回:" + hg);
                    return hg;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("HTPP的GET请求返回异常:" + ex.Message);
                return null;
            }
        }


        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string GetResponse(string url, int? timeOut)
        {
            LogHelper.WriteInfoLog("HTPP的GET请求url:" + url);
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.CookieContainer = CookieContainers;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = Ie7;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,**;q=0.8";
                req.Timeout = timeOut != null ? (int)timeOut : 50000;
                var res = (HttpWebResponse)req.GetResponse();
                using (Stream s = res.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    string hg = reader.ReadToEnd();
                    LogHelper.WriteInfoLog("HTPP的GET请求返回:" + hg);
                    return hg;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("HTPP的GET请求返回异常:" + ex.Message);
                return null;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            LogHelper.WriteInfoLog("HTPP的GET请求url:" + url);
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.CookieContainer = CookieContainers;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = Ie7;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,**;q=0.8";
                req.Timeout = 50000;

                var res = (HttpWebResponse)req.GetResponse();
                using (Stream s = res.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    string hg = reader.ReadToEnd();
                    LogHelper.WriteInfoLog("HTPP的GET请求返回:" + hg);
                    return hg;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("HTPP的GET请求返回异常:" + ex.Message);
                return null;
            }
        }

        public static string PostData(string postUrl, string data)
        {
            return PostData(postUrl, data, Encoding.UTF8);
        }

        public static string PostData(string postUrl, string data, Encoding encoding)
        {
            LogHelper.WriteInfoLog("HTPP的POST请求url:" + postUrl + ",data:" + data);
            try
            {
                string pageStr = string.Empty;
                var url = new Uri(postUrl);//配置在web.config中
                byte[] reqbytes = encoding.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "post";
                req.ContentType = "text/xml";
                req.ContentLength = reqbytes.Length;
                req.Timeout = 5000;
                Stream stm = req.GetRequestStream();
                stm.Write(reqbytes, 0, reqbytes.Length);
                stm.Close();
                var wr = (HttpWebResponse)req.GetResponse();
                using (Stream s = wr.GetResponseStream())
                {
                    var srd = new StreamReader(s, encoding);
                    pageStr += srd.ReadToEnd();
                    LogHelper.WriteInfoLog("HTPP的POST请求返回:" + pageStr);
                }
                return pageStr;
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("HTPP的POST请求返回异常:" + ex.Message);
                return null;
            }
        }

        public static string WxApiPost(string postUrl, string data)
        {
            LogHelper.WriteInfoLog("HTPP的POST请求url:" + postUrl + ",data:" + data);
            try
            {
                string pageStr = string.Empty;
                var url = new Uri(postUrl);//配置在web.config中
                byte[] reqbytes = Encoding.UTF8.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "post";
                req.ContentType = "application/json";
                req.ContentLength = reqbytes.Length;
                req.Timeout = 60000;
                Stream stm = req.GetRequestStream();
                stm.Write(reqbytes, 0, reqbytes.Length);
                stm.Close();
                Stopwatch watch = new Stopwatch();//计时器
                watch.Start();
                var wr = (HttpWebResponse)req.GetResponse();
                watch.Stop();
                using (Stream s = wr.GetResponseStream())
                {
                    var srd = new StreamReader(s, Encoding.GetEncoding("utf-8"));
                    pageStr += srd.ReadToEnd();
                }
                LogHelper.WriteInfoLog("HTTP的POST请求返回:" + pageStr + " 耗时" + watch.ElapsedMilliseconds + "毫秒");
                return pageStr;
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("HTPP的POST请求返回异常:" + ex.Message);
                throw;
                //return null;
            }
        }


        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            const string formID = "PostForm";
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }

            strForm.Append("</form>");
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            return strForm.ToString() + strScript;
        }


        public static string PostHttpResponse(string posturl, string postDataStr, CookieCollection cookies = null)
        {
            //postDataStr = HttpUtility.UrlEncode(postDataStr, Encoding.GetEncoding("GBK"));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(posturl);
            request.CookieContainer = new CookieContainer();
            //var cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链  
            //request.Referer = "http://localhost/index.php";
            //request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
            //request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            //request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
            //request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            Encoding encoding = Encoding.GetEncoding("GB2312");//根据网站的编码自定义  
            byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            string retString = null;
            if (responseStream != null)
            {
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                retString = streamReader.ReadToEnd();

                streamReader.Close();
                responseStream.Close();

            }
            return retString;
        }

        public static string GetHttpResponse(string geturl, string getDataStr, CookieCollection cookies = null)
        {
            if (!string.IsNullOrEmpty(getDataStr))
                geturl = geturl + "?" + getDataStr;
            LogHelper.WriteInfoLog("HTPP的Get请求url:" + geturl);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(geturl);
                request.Method = "GET";
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                Stopwatch watch = new Stopwatch();//计时器
                watch.Start();
                HttpWebResponse httpWebResponse = request.GetResponse() as HttpWebResponse;
                watch.Stop();
                if (httpWebResponse == null)
                    return "";
                using (Stream s = httpWebResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    string hg = reader.ReadToEnd();
                    LogHelper.WriteInfoLog("HTTP的Get请求返回:" + hg + " 耗时" + watch.ElapsedMilliseconds + "毫秒");
                    return hg;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("HTTP的Get请求异常", ex);
                throw;
            }
        }

    }
}
