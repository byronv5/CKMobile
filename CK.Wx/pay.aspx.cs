using System;
using CommonLibrary.Assist;

namespace CK.Wx
{
    public partial class pay : System.Web.UI.Page
    {
        #region 全局变量
        /// <summary>
        /// 订单id
        /// </summary>
        protected string OrderId = "";
        /// <summary>
        /// 公众号名称
        /// </summary>
        protected string AppId = "";
        /// <summary>
        /// 时间戳
        /// </summary>
        protected string TimeStamp = "";
        /// <summary>
        /// 随机串
        /// </summary>
        protected string NonceStr = "";
        /// <summary>
        /// 预支付id
        /// </summary>
        protected string PrepayId = "";
        /// <summary>
        /// 签名
        /// </summary>
        protected string PaySign = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            OrderId = Context.Request.QueryString["OrderID"];
            AppId = Context.Request.QueryString["appId"];
            TimeStamp = Context.Request.QueryString["timeStamp"];
            NonceStr = Context.Request.QueryString["nonceStr"];
            PrepayId = Context.Request.QueryString["prepay_id"];
            PaySign = Context.Request.QueryString["paySign"];
            LogHelper.WriteInfoLog("appId:" + AppId + ",timeStamp:" + TimeStamp + ",nonceStr:" + NonceStr +
                                   ",prepay_id:" + PrepayId + ",paySign:" + PaySign);
        }
    }
}