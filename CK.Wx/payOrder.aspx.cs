using System;
using System.Web.UI;
using CommonLibrary.Assist;

namespace CK.Wx
{
    public partial class PayOrder : Page
    {
        protected string Code = "";
        protected string State = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            State = Context.Request.QueryString["state"];
            Code = Context.Request.QueryString["code"];
            LogHelper.WriteInfoLog("授权认证第一步获取到code：" + Code);
        }       
    }
}
