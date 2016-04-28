using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CommonLibrary.Assist;

namespace CommonLibrary.Security
{
    public class New3Des
    {
        /// <summary>
        ///     DES3 CBC模式解密
        /// </summary>
        /// <param name="text">解密内容</param>
        /// <returns></returns>
        public static string DecodeCbc(string text)
        {
            try
            {
                string key = "surfingclientkefu@lx#5#*";
                string iv = "87654321";
                byte[] keys = Encoding.GetEncoding("gb2312").GetBytes(key);
                byte[] ivs = Encoding.GetEncoding("gb2312").GetBytes(iv);
                byte[] data = Convert.FromBase64String(text);
                return Encoding.GetEncoding("gb2312").GetString(Des3DecodeCbc(keys, ivs, data)).Replace("\0", "");
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("CBC解密出错：", ex);
                //UnionLog.WriteLog(LogType.Info, "CBC加密转换" + ex);
                return "";
            }
        }

        /// <summary>
        /// DES3 CBC模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public static byte[] Des3DecodeCbc(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                var msDecrypt = new MemoryStream(data);
                var tdsp = new TripleDESCryptoServiceProvider {Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7};
                var csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                var fromEncrypt = new byte[data.Length];
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                return fromEncrypt;
            }
            catch (CryptographicException ex)
            {
                LogHelper.WriteErrorLog("CBC解密出错：", ex);
                return null;
            }
        }

        /// <summary>
        ///     DES3 CBC模式加密
        /// </summary>
        /// <param name="text">加密内容</param>
        /// <returns></returns>
        public static string EncodeCbc(string text)
        {
            try
            {
                string key = "surfingclientkefu@lx#5#*";
                string iv = "87654321";
                Encoding utf8 = Encoding.GetEncoding("gb2312");
                byte[] keys = Encoding.GetEncoding("gb2312").GetBytes(key);
                byte[] ivs = Encoding.GetEncoding("gb2312").GetBytes(iv);
                byte[] data = utf8.GetBytes(text);
                return Convert.ToBase64String(Des3EncodeCbc(keys, ivs, data));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("CBC加密出错：", ex);
                return "";
            }
        }

        /// <summary>
        ///     DES3 CBC模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public static byte[] Des3EncodeCbc(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                var mStream = new MemoryStream();
                var tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC; //默认值
                tdsp.Padding = PaddingMode.PKCS7; //默认值
                var cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                byte[] ret = mStream.ToArray();
                cStream.Close();
                mStream.Close();
                return ret;
            }
            catch (CryptographicException ex)
            {
                LogHelper.WriteErrorLog("CBC解密出错：", ex);
                return null;
            }
        }
    }


    public class Des3
    {
        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] Data = Convert.FromBase64String(data);
            byte[] Key = Encoding.Default.GetBytes(key);
            byte[] IV = Encoding.Default.GetBytes(iv);
            try
            {
                MemoryStream msDecrypt = new MemoryStream(Data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Key = Key;
                tdsp.IV = IV;
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(),
                    CryptoStreamMode.Read);
                byte[] fromEncrypt = new byte[Data.Length];
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                return System.Text.Encoding.UTF8.GetString(fromEncrypt);
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog("Des3解密出错：", e);
                return null;
            }
        }
    }

}