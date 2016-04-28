using System;
using System.Text;

namespace CommonLibrary.Security
{
    public class XXTEA
    {
        private static Byte[] Encrypt(Byte[] Data, Byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(Data, true), ToUInt32Array(Key, false)), false);
        }
        private static Byte[] Decrypt(Byte[] Data, Byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Decrypt(ToUInt32Array(Data, false), ToUInt32Array(Key, false)), true);
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            return v;
        }
        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            sum = unchecked((UInt32)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return v;
        }

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

        /// <summary>
        /// 16进制字符串转换为byte[]
        /// </summary>
        private static byte[] HexStringToByteArray(string s)
        {
            Byte[] buf = new byte[s.Length / 2];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(chr2hex(s.Substring(i * 2, 1)) * 0x10 + chr2hex(s.Substring(i * 2 + 1, 1)));
            }
            return buf;
        }
        private static byte chr2hex(string s)
        {
            switch (s)
            {
                case "0":
                    return 0x00;
                case "1":
                    return 0x01;
                case "2":
                    return 0x02;
                case "3":
                    return 0x03;
                case "4":
                    return 0x04;
                case "5":
                    return 0x05;
                case "6":
                    return 0x06;
                case "7":
                    return 0x07;
                case "8":
                    return 0x08;
                case "9":
                    return 0x09;
                case "A":
                    return 0x0a;
                case "B":
                    return 0x0b;
                case "C":
                    return 0x0c;
                case "D":
                    return 0x0d;
                case "E":
                    return 0x0e;
                case "F":
                    return 0x0f;
            }
            return 0x00;
        }



        private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
        {
            Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            UInt32[] Result;
            if (IncludeLength)
            {
                Result = new UInt32[n + 1];
                Result[n] = (UInt32)Data.Length;
            }
            else
            {
                Result = new UInt32[n];
            }
            n = Data.Length;
            for (Int32 i = 0; i < n; i++)
            {
                Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }
        private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
        {
            Int32 n;
            if (IncludeLength)
            {
                n = (Int32)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            Byte[] Result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        private static string Encrypt(string Target)
        {
            return Encrypt(Target, string.Empty);
        }


        private static string Encrypt(string Target, string Key)
        {
            if (0 == Key.Length)
                Key = "07bHrYkaFYBD4Bxrhdbm9KhnT5jmvFho";
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            Byte[] data = Encrypt(encoder.GetBytes(Target), encoder.GetBytes(Key));
            return System.Convert.ToBase64String(data);

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        private static string Decrypt(string Target)
        {
            return Decrypt(Target, string.Empty);
        }
        private static string Decrypt(string Target, string Key)
        {
            if (0 == Key.Length)
                Key = "07bHrYkaFYBD4Bxrhdbm9KhnT5jmvFho";
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            return encoder.GetString(Decrypt(System.Convert.FromBase64String(Target), encoder.GetBytes(Key)));

        }


        public static string XXteaEncrypt(String plainText, String appSecret)
        {
            byte[] msg = Encoding.UTF8.GetBytes(plainText);
            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            return byteToHexStr(Encrypt(msg, key));
        }


        public static string XXteaDecrypt(String cipherText, String appSecret)
        {
            byte[] encryptText = HexStringToByteArray(cipherText);
            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            return Encoding.UTF8.GetString(Decrypt(encryptText, key));
        }

    }
}
