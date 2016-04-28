using System.Linq;
using System.Xml;

namespace CommonLibrary.Assist
{
    public sealed class ConversionType
    {
        /// <summary>
        /// XML转换为Json
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XmlToJson(string xml)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            //doc.LoadXml(doc.ChildNodes[1].OuterXml);
            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        }

        public static string PicUrlForImg(object picBind)
        {
            if (picBind == null)
                return "";
            string pic = picBind.ToString();
            if (!string.IsNullOrEmpty(pic))
                return pic.Split('|').First();
            return "";
        }
    }
}
