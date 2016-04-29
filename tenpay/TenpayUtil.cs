using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using CommonLibrary.Assist;
using Newtonsoft.Json.Linq;

namespace tenpay
{
    public class TenpayUtil
    {
        //private string _paySignKey = ConfigurationManager.AppSettings["PaySignKey"];
        private readonly string _appSecret = ConfigurationManager.AppSettings["AppSecret"];
        //private string _mchId = ConfigurationManager.AppSettings["mch_id"];
        private readonly string _appId = ConfigurationManager.AppSettings["AppId"];

        /// <summary>
        /// 获取调用接口凭证的接口
        /// </summary>
        private const string AccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token";
        /// <summary>
        /// 统一支付接口
        /// </summary>
        const string UnifiedPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 获取网页授权access_token
        /// </summary>
        const string OauthAccessTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        const string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 随机串
        /// </summary>
        public static string GetNoncestr()
        {
            Random random = new Random();
            return MD5Util.GetMd5(random.Next(1000).ToString(CultureInfo.InvariantCulture), "GBK").ToLower().Replace("s", "S");
        }

        /// <summary>
        /// 时间截，自1970年以来的秒数
        /// </summary>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取网页授权access_token接口
        /// </summary>
        public static string getOauth2Access_tokenUrl()
        {
            return OauthAccessTokenUrl;
        }
        /// <summary>
        /// 获取接口调用凭证access_token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            var token = HttpRuntime.Cache["AccessToken"];
            if (token == null)
            {
                string data = "grant_type=client_credential&appid=" + _appId + "&secret=" + _appSecret + "";
                string returnmsg = HttpHelper.GetHttpResponse(AccessTokenUrl, data);                
                var jb = JObject.Parse(returnmsg);
                token = jb["access_token"];
                if (token != null)
                {
                    HttpRuntime.Cache.Add("AccessToken", token, null, DateTime.Now.AddMinutes(100),
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Default, null); //虽然token两小时失效，但为了避免网络延迟造成的时间差，所以设置100min
                }
                else
                {
                    LogHelper.WriteInfoLog("token为null");
                    return "";
                }

            }
            return token.ToString();
        }

        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CreateSign(SortedDictionary<string, string> sParams, string key)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (string.IsNullOrEmpty(temp.Value) || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string signkey = sb.ToString();
            string sign = MD5Util.GetMd5(signkey, "utf-8");
            return sign;
        }

        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string PostDataToUrl(string url, string postData)
        {
            string returnmsg;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                LogHelper.WriteInfoLog("WebClient方式请求url:" + url + "。data:" + postData);
                returnmsg = wc.UploadString(url, "POST", postData);
                LogHelper.WriteInfoLog("WebClient方式请求返回:" + returnmsg);
            }
            return returnmsg;
        }

        /// <summary>
        /// 获取prepay_id
        /// </summary>
        public string getPrepay_id(UnifiedOrder order, string key)
        {
            string prepayId = "";
            string postData = getUnifiedOrderXml(order, key);
            LogHelper.WriteInfoLog("微信支付统一接口post请求data：" + postData);
            string requestData = PostDataToUrl(UnifiedPayUrl, postData);
            LogHelper.WriteInfoLog("微信支付统一接口post请求返回：" + requestData);
            SortedDictionary<string, string> requestXML = GetInfoFromXml(requestData);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "prepay_id")
                {
                    prepayId = k.Value;
                    break;
                }
            }
            return prepayId;
        }

        /// <summary>
        /// 获取微信订单明细
        /// </summary>
        public OrderDetail GetOrderDetail(QueryOrder queryorder, string key)
        {
            string postData = getQueryOrderXml(queryorder, key);
            string requestData = PostDataToUrl(OrderQueryUrl, postData);
            OrderDetail orderdetail = new OrderDetail();
            SortedDictionary<string, string> requestXml = GetInfoFromXml(requestData);
            foreach (KeyValuePair<string, string> k in requestXml)
            {
                switch (k.Key)
                {
                    case "retuen_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "return_msg":
                        orderdetail.return_msg = k.Value;
                        break;
                    case "appid":
                        orderdetail.appid = k.Value;
                        break;
                    case "mch_id":
                        orderdetail.mch_id = k.Value;
                        break;
                    case "nonce_str":
                        orderdetail.nonce_str = k.Value;
                        break;
                    case "sign":
                        orderdetail.sign = k.Value;
                        break;
                    case "result_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "err_code":
                        orderdetail.err_code = k.Value;
                        break;
                    case "err_code_des":
                        orderdetail.err_code_des = k.Value;
                        break;
                    case "trade_state":
                        orderdetail.trade_state = k.Value;
                        break;
                    case "device_info":
                        orderdetail.device_info = k.Value;
                        break;
                    case "openid":
                        orderdetail.openid = k.Value;
                        break;
                    case "is_subscribe":
                        orderdetail.is_subscribe = k.Value;
                        break;
                    case "trade_type":
                        orderdetail.trade_type = k.Value;
                        break;
                    case "bank_type":
                        orderdetail.bank_type = k.Value;
                        break;
                    case "total_fee":
                        orderdetail.total_fee = k.Value;
                        break;
                    case "coupon_fee":
                        orderdetail.coupon_fee = k.Value;
                        break;
                    case "fee_type":
                        orderdetail.fee_type = k.Value;
                        break;
                    case "transaction_id":
                        orderdetail.transaction_id = k.Value;
                        break;
                    case "out_trade_no":
                        orderdetail.out_trade_no = k.Value;
                        break;
                    case "attach":
                        orderdetail.attach = k.Value;
                        break;
                    case "time_end":
                        orderdetail.time_end = k.Value;
                        break;
                    default:
                        break;
                }
            }
            return orderdetail;
        }

        /// <summary>
        /// 把XML数据转换为SortedDictionary集合
        /// </summary>
        /// <param name="xmlstring"></param>
        /// <returns></returns>
        private SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlstring);
            XmlElement root = doc.DocumentElement;
            if (root != null)
            {
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            return sParams;
        }

        /// <summary>
        /// 微信统一下单接口xml参数整理
        /// </summary>
        /// <param name="order">微信支付参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        private string getUnifiedOrderXml(UnifiedOrder order, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", order.appid);
            sParams.Add("attach", order.attach);
            sParams.Add("body", order.body);
            sParams.Add("device_info", order.device_info);
            sParams.Add("mch_id", order.mch_id);
            sParams.Add("nonce_str", order.nonce_str);
            sParams.Add("notify_url", order.notify_url);
            sParams.Add("openid", order.openid);
            sParams.Add("out_trade_no", order.out_trade_no);
            sParams.Add("spbill_create_ip", order.spbill_create_ip);
            sParams.Add("total_fee", order.total_fee.ToString());
            sParams.Add("trade_type", order.trade_type);
            order.sign = CreateSign(sParams, key);
            sParams.Add("sign", order.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            return return_string;

        }

        /// <summary>
        /// 微信订单查询接口XML参数整理
        /// </summary>
        /// <param name="queryorder">微信订单查询参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        private string getQueryOrderXml(QueryOrder queryorder, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", queryorder.appid);
            sParams.Add("mch_id", queryorder.mch_id);
            sParams.Add("transaction_id", queryorder.transaction_id);
            sParams.Add("out_trade_no", queryorder.out_trade_no);
            sParams.Add("nonce_str", queryorder.nonce_str);
            queryorder.sign = CreateSign(sParams, key);
            sParams.Add("sign", queryorder.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
            return return_string;
        }
    }
}