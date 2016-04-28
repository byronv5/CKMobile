using System;
using System.Text;
using System.Security.Cryptography;

namespace tenpay
{
    public class MD5Util
    {
        public MD5Util()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /** 获取大写的MD5签名结果 */
        public static string GetMd5(string encypStr, string charset)
        {
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            byte[] outputBye = m5.ComputeHash(inputBye);

            string retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}