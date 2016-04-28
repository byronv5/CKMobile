using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.Security
{
    public sealed class Md5Helper
    {

        private static readonly string[] HexDigits = {"0", "1", "2", "3", "4", "5", "6", "7", 
													  "8", "9", "A", "B", "C", "D", "E", "F"};

        public static string Md5Digest(string str)
        {
            //将字符串转换位字节数组
            IEnumerable<byte> send = Md5Digest(Encoding.GetEncoding("gb2312").GetBytes(str));

            string result = ByteArrayToHexString(send);

            return result;
        }

        private static IEnumerable<byte> Md5Digest(Byte[] src)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(src);
            return t;
        }
        /** 
        * 转换字节数组为16进制字串 
        * @param b 字节数组 
        * @return 16进制字串 
        */

        private static string ByteArrayToHexString(IEnumerable<byte> b)
        {
            StringBuilder resultSb = new StringBuilder();
            foreach (byte t in b)
                resultSb.Append(ByteToHexString(t));
            return resultSb.ToString();
        }
        private static string ByteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
                n = 256 + n;
            int d1 = n / 16;
            int d2 = n % 16;
            return HexDigits[d1] + HexDigits[d2];
        }


        /// <summary> MD5标准加密 </summary>
        public static string SpMd5(string strVal)
        {
            string ret = string.Empty;
            byte[] b = Encoding.UTF8.GetBytes(strVal);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            return b.Aggregate(ret, (current, t) => current + t.ToString("x").PadLeft(2, '0'));

        }
        /// <summary>
        /// str转换为16进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodingSMS(string s)
        {
            string result = string.Empty;

            byte[] arrByte = System.Text.Encoding.GetEncoding("GB2312").GetBytes(s);
            for (int i = 0; i < arrByte.Length; i++)
            {
                result += System.Convert.ToString(arrByte[i], 16); 
            }

            return result;
        }
    }
}
