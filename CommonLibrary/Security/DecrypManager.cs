using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace CommonLibrary.Security
{
    public class DecrypManager
    {
        //默认密钥向量     
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private const string EncryptKey = "jf189189";

        /// <summary>
        ///  DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDes(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(EncryptKey);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, Keys), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptDes(string decryptString)
        {

            byte[] rgbKey = Encoding.UTF8.GetBytes(EncryptKey);
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            var dcsp = new DESCryptoServiceProvider();
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, Keys), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());

        }

        /// <summary>
        /// 获取验证的签名
        /// </summary>
        /// <param name="infoArray"></param>
        /// <returns></returns>
        public static string VerificatySign(string[] infoArray)
        {
            var arrTemp = new string[infoArray.Length];
            for (int i = 0; i < infoArray.Length - 1; i++)
            {
                if (!Regex.IsMatch(infoArray[i], @"[\u4e00-\u9fa5]+$"))
                {
                    arrTemp[i] = infoArray[i];
                }
            }
            Array.Sort(arrTemp);
            string tmpStr = string.Join("", arrTemp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr;
        }


        /// <summary>
        /// 获取Token的签名校验
        /// </summary>
        /// <param name="infoArray"></param>
        /// <returns></returns>
        public static string GenerateSign(string[] infoArray)
        {
            var arrTemp = new string[infoArray.Length];
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (!Regex.IsMatch(infoArray[i], @"[\u4e00-\u9fa5]+$"))
                {
                    arrTemp[i] = infoArray[i];
                }
            }
            Array.Sort(arrTemp);
            string tmpStr = string.Join("", arrTemp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr;
        }
    }
}
