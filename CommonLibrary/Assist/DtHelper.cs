using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;

namespace CommonLibrary.Assist
{
    public sealed class DtHelper
    {
        /// <summary>
        /// 将DataTable转换成泛类型List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            DataColumnCollection columns = dt.Columns;
            int iColumnCount = columns.Count;
            var entity = new T();
            Type elementType = entity.GetType();
            System.Reflection.PropertyInfo[] publicProperties = elementType.GetProperties();
            var result = new List<T>();
            //if (publicProperties.Length == iColumnCount)
            //{
            foreach (DataRow currentRow in dt.Rows)
            {
                int i;
                for (i = 0; i < iColumnCount; i++)
                {
                    int j;
                    for (j = 0; j < publicProperties.Length; j++)
                    {
                        if (columns[i].ColumnName.ToLower() == publicProperties[j].Name.ToLower())
                        {
                            publicProperties[j].SetValue(entity,
                                string.IsNullOrEmpty(currentRow[i].ToString()) ? null : currentRow[i], null);
                        }
                    }
                }
                result.Add(entity);
                entity = new T();
            }
            //}
            //else
            //{
            //    result = null;
            //}
            return result;
        }

        /// <summary>
        /// 获取随机四位数
        /// </summary>
        /// <param name="rLength"></param>
        /// <returns></returns>
        public static string RandCode(int rLength)
        {
            var rd = new Random();
            string str = "abcdefghijklmnopqrstuvwxyz0123456789_";
            string result = "";
            for (int i = 0; i < rLength; i++)
                result += str[rd.Next(str.Length)];
            return result;
        }

        /// <summary>
        /// 获取随机数
        /// min到max之间任意数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNum(int min, int max)
        {
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(data);
            Random random = new Random(BitConverter.ToInt32(data, 0));
            return random.Next(min, max);
        }

        /// <summary>
        /// datetime转换成unixtime
        /// </summary>
        /// <returns></returns>
        public static string GetTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //DateTime time = DateTime.Now;
            return Math.Floor((time - startTime).TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}
