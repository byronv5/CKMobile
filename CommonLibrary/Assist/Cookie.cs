using System;
using System.Globalization;
using System.Web;

namespace CommonLibrary.Assist
{
    public class Cookie
    {
        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>

        public static string GetCookie(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                return cookie.Value.ToString(CultureInfo.InvariantCulture);
            }
            return null;
        }

        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>
        public static bool DelCookie(string strName)
        {
            try
            {
                var cookie = new HttpCookie(strName) { Expires = DateTime.Now.AddDays(-1) };
                //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="strName">主键</param>
        /// <param name="strValue">键值</param>
        /// <param name="days">天数</param>
        /// <returns></returns>
        public static bool SetCookie(string strName, string strValue, int days)
        {
            try
            {
                //LogHelper.WriteInfoLog("设置TokenName：" + strName + ",Value：" + strValue);
                var cookie = new HttpCookie(strName)
                {
                    Expires = DateTime.Now.AddDays(days),
                    Value = strValue
                };
                //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                //HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.AppendCookie(cookie);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfoLog("出错了：" + ex.Message);
                return false;
            }
        }
    }
}
