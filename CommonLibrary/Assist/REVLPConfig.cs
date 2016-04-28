using System;
using System.Collections.Generic;
using System.Configuration;

namespace CommonLibrary.Assist
{
    public class REVLPConfig
    {
        /// <summary>
        /// Cookie超时时间
        /// </summary>
        public static int CookieDays = Convert.ToInt32(ConfigurationManager.AppSettings["CookieExpiresDay"]);

        /// <summary>
        /// Cache超时时间
        /// </summary>
        public static int CacheHours = Convert.ToInt32(ConfigurationManager.AppSettings["CacheExpiresHours"]);

        /// <summary>
        /// 第三方调用业务编号
        /// </summary>
        public static string SPId = ConfigurationManager.AppSettings["RedEvlpSPId"];

        /// <summary>
        /// 获取渠道:[0]客户端；[1]wap
        /// </summary>
        public static string[] AppCodes
        {
            get
            {
                string[] codes = ConfigurationManager.AppSettings["AppCodes"] != null ? ConfigurationManager.AppSettings["AppCodes"].Split(',') : new string[] { };
                for (int i = 0; i < codes.Length; i++)
                {
                    codes[i] = codes[i].Split('|')[0];
                }
                return codes;
            }
        }

        /// <summary>
        /// 星级对应积分范围,Instance:4|1-1000,7|500-10000
        /// </summary>
        public static IDictionary<string, string> YjfRange
        {
            get
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                string[] ranges = ConfigurationManager.AppSettings["YJFRange"] != null ? ConfigurationManager.AppSettings["YJFRange"].Split(',') : new string[] { };
                foreach (var range in ranges)
                {
                    var rg = range.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                    dic.Add(rg[0], rg[1]);
                }
                return dic;
            }
        }

        /// <summary>
        /// 天翼客服接口地址
        /// </summary>
        public static string TykfBaseUrl = ConfigurationManager.AppSettings["TYKFBaseAddrs"];
    }
    /// <summary>
    /// 能力接口地址
    /// </summary>
    public static class AbilityController
    {
        /// <summary>
        /// 用户信息查询
        /// </summary>
        public const string GetAccountInfo = "UserManage/GetLoadAuth";

        /// <summary>
        /// 积分翼积分查询
        /// </summary>
        public const string IntegralVoucher = "UserManage/GetIntegralVoucher";

        /// <summary>
        /// 翼积分使用获取明细
        /// </summary>
        public const string YjfGetAndUsedInfo = "UserManage/QueryUsedVoucher";

        /// <summary>
        /// 生成sp订单编号
        /// </summary>
        public const string CreateExternalOrder = "SpOrderManage/CreateExternalOrder";

        /// <summary>
        /// 积分发行
        /// </summary>
        public const string PublishSpIntegral = "SpOrderManage/PublishSpIntegral";

        /// <summary>
        /// 自定义短信发送
        /// </summary>
        public const string SendMsg = "SMSCodeManage/SendCustomMsg";
        /// <summary>
        /// 随机码下发
        /// </summary>
        public const string SendRandomCode = "SMSCodeManage/Send";
        /// <summary>
        /// 随机码验证
        /// </summary>
        public const string CheckRandomCode = "SMSCodeManage/SmsCodeCheck";
    }
}
