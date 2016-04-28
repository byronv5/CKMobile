using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace CommonLibrary.Security
{
    public class EncryptHelper
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
                if (string.IsNullOrEmpty(infoArray[i]) || !Regex.IsMatch(infoArray[i], @"[\u4e00-\u9fa5]+$"))
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
        ///  Base64 加密
        /// </summary>
        public static string Base64Encrypt(string result)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(result);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///  Base64 解密
        /// </summary>
        public static string Base64Decrypt(string result)
        {
            byte[] outputb = Convert.FromBase64String(result);
            return Encoding.UTF8.GetString(outputb);
        }

        public static string SHA1(string str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            //ASCIIEncoding enc = new ASCIIEncoding();
            UTF8Encoding enc = new UTF8Encoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash.ToLower();
        }
    }
}
