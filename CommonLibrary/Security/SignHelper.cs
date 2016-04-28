using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.Security
{
    public static class SignHelper
    {
        /// <summary>
        /// 生成验证签名sign
        /// </summary>
        /// <param name="appcode"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetSign(string appcode, string time)
        {
            string key = GetSignKeyByAppcode(appcode);
            string waitSign = "AppCode=" + appcode + "&SingKey=" + key + "&Date=" + time;
            string ret = string.Empty;
            byte[] b = Encoding.UTF8.GetBytes(waitSign);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            return b.Aggregate(ret, (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }
        /// <summary>
        /// 根据appcode获取key
        /// </summary>
        /// <param name="appcode"></param>
        /// <returns></returns>
        private static string GetSignKeyByAppcode(string appcode)
        {
            string[] appcodes = ConfigurationManager.AppSettings["AppCodes"] != "" ? ConfigurationManager.AppSettings["AppCodes"].Split(',') : new string[] { };
            string key = "";
            for (int i = 0, j = appcodes.Length; i < j; i++)
            {
                if (appcodes[i].Contains(appcode))
                {
                    string[] ck = appcodes[i].Split('|');
                    key = ck[1];
                    break;
                }
            }
            return key;
        }
    }
}
