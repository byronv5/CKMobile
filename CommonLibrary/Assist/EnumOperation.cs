using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace CommonLibrary.Assist
{
    public static class EnumOperation
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            try
            {
                var customAttributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((customAttributes.Length > 0))
                {
                    return customAttributes[0].Description;
                }
                return value.ToString();
            }
            catch
            {
                return "其它";
            }
        }

        /// <summary>
        /// 枚举转化成DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable ToDataTable<T>()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Columns.Add("Description");
            var source = typeof(T);
            foreach (string name in Enum.GetNames(source))
            {
                var dr = dt.Rows.Add();
                dr[0] = name;
                dr[1] = (int)Enum.Parse(source, name);
                dr[2] = (Enum.Parse(source, name) as Enum).GetDescription();
            }
            return dt;
        }

        /// <summary>
        /// 获取枚举对象集合
        /// </summary>
        /// <returns></returns>
        public static IList<EnumInfo> ToList<T>()
        {
            var list = new List<EnumInfo>();
            var source = typeof(T);
            foreach (string name in Enum.GetNames(source))
            {
                var obj = Enum.Parse(source, name);
                var info = new EnumInfo
                    {
                        Name = name,
                        EnumValue = obj as Enum,
                        Value = (int)obj
                    };
                info.Description = info.EnumValue.GetDescription();
                list.Add(info);
            }
            return list;
        }

        public static T GetEnumValue<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Text(this Enum value)
        {
            return value.GetDescription();
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Value(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }

    /// <summary>
    /// 枚举实体对象
    /// </summary>
    public class EnumInfo
    {
        /// <summary>
        /// 枚举对象
        /// </summary>
        public Enum EnumValue { get; set; }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description { get; set; }
    }
}

