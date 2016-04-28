using System;
using CommonLibrary.Assist;
using tenpay;

namespace CK.Wx
{
    public partial class WxCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Notify n = new Notify(Page);
                WxPayData data = n.GetNotifyData();
                LogHelper.WriteInfoLog("微信异步通知支付结果：" + data.GetValue("return_code"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("微信异步通知支付结果异常！", ex);
            }       
        }
    }
}