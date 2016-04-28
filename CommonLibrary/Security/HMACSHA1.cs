using System;
using System.Text;

namespace CommonLibrary.Security
{
    public class HMACSHA1
    {
        //字节数组转字符串
        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static string sign(String data, String appSecret)
        {
            System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(appSecret);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return byteToHexStr(hashBytes);
        }
    }
}
