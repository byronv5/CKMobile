using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace CommonLibrary.Assist
{
    public class XmlHelper
    {
        /// <summary>
        /// 获取xml中的值
        /// </summary>
        public static string GetXmlNodeText(XmlDocument xmlDoc, string node)
        {
            node = node.Replace("\\", "/");
            var selectSingleNode = xmlDoc.SelectSingleNode(node);
            return selectSingleNode == null ? null : selectSingleNode.InnerText;
        }
        #region 获取省份或地市
        /// <summary>
        /// 根据省编号获取省名称
        /// </summary>
        /// <param name="strProvinceId"></param>
        /// <returns></returns>
        public static string GetProvinceName(string strProvinceId)
        {
            string sRet = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/ChinaArea.xml"));
            XmlNodeList xnlist = xmlDoc.SelectNodes("area/province[@provinceID]");


            if (xnlist != null)
                foreach (XmlNode myNode in xnlist)
                {
                    if (myNode.Attributes != null && myNode.Attributes["provinceID"].Value == strProvinceId)
                    {
                        sRet = myNode.Attributes["province"].Value;
                    }

                }
            return sRet;
        }

        /// <summary>
        /// 根据省名称获取省编号
        /// </summary>
        /// <param name="strProvinceName"></param>
        /// <returns></returns>
        public static string GetProvinceId(string strProvinceName)
        {
            string sRet = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/ChinaArea.xml"));
            XmlNodeList xnlist = xmlDoc.SelectNodes("area/province[@province]");


            if (xnlist != null)
                foreach (XmlNode myNode in xnlist)
                {
                    if (myNode.Attributes != null && myNode.Attributes["province"].Value == strProvinceName)
                    {
                        sRet = myNode.Attributes["provinceID"].Value;
                    }

                }
            return sRet;
        }

        /// <summary>
        /// 根据cityname获取cityid
        /// </summary>
        /// <param name="provinceid"></param>
        /// <param name="cityname"></param>
        /// <returns></returns>
        public static string GetCityIdByName(string provinceid, string cityname)
        {
            string cityid = "0";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/ChinaArea.xml"));
                XmlNode xn = xmlDoc.SelectSingleNode(string.Format("area/province[@provinceID='{0}']", provinceid));

                if (xn != null)
                {
                    XmlNodeList nodelist = xn.SelectNodes("City");
                    if (nodelist != null)
                        foreach (XmlNode myNode in nodelist)
                        {
                            if (myNode.Attributes != null && myNode.Attributes["City"].Value == cityname)
                            {
                                cityid = myNode.Attributes["CityID"].Value; ;
                                break;
                            }
                        }
                }
            }
            catch (Exception)
            {
                cityid = "0";
            }
            return cityid;
        }

        #endregion
    }
}
