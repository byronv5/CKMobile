using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using CommonLibrary.Assist;
using Newtonsoft.Json;
using tenpay;

namespace CK.Wx.ajax
{
    /// <summary>
    /// 获取JsApi权限配置的数组/四个参数
    /// </summary>
    public class GetWxJsApiConfig : BaseAjax
    {

        protected override string GetAjaxResult(HttpContext context)
        {
            string timestamp = TenpayUtil.GetTimestamp();//生成签名的时间戳
            string nonceStr = TenpayUtil.GetNoncestr();//生成签名的随机串
            string page = context.Request["Pagepath"];
            string url = "http://ck-sh.com/ck/" + page;//当前的地址
            string accsToken = new TenpayUtil().GetAccessToken();
            string appid = ConfigurationManager.AppSettings["AppId"];

            string jsapiTicke;
            //ticket 缓存7200秒
            if (context.Session["jsapi_ticket"] == null)
            {
                jsapiTicke =
                HttpHelper.WxApiPost(
                    "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accsToken + "&type=jsapi", "");
                context.Session["jsapi_ticket"] = jsapiTicke;
                context.Session.Timeout = 7200;
            }
            else
            {
                jsapiTicke = context.Session["jsapi_ticket"].ToString();
            }

            Dictionary<string, object> respDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsapiTicke);
            jsapiTicke = respDic["ticket"].ToString();//获取ticket
            string[] arrayList = { "jsapi_ticket=" + jsapiTicke, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + url };
            Array.Sort(arrayList);
            string signature = string.Join("&", arrayList);
            LogHelper.WriteInfoLog("加密前的signature：" + signature);
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1").ToLower();
            string rst = "{\"appId\":\"" + appid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr +
                         "\",\"signature\":\"" + signature + "\"}";
            LogHelper.WriteInfoLog("获取JsApi权限配置的参数--" + rst);
            return rst;
        }
    }
}