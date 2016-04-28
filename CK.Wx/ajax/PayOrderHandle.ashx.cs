using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using CK.Bll;
using CK.Model;
using CommonLibrary.Assist;
using Newtonsoft.Json;
using tenpay;

namespace CK.Wx.ajax
{
    /// <summary>
    /// PayOrderHandle 的摘要说明
    /// </summary>
    public class PayOrderHandle : BaseAjax
    {
        protected override string GetAjaxResult(HttpContext context)
        {
            PayLogBll payBll = new PayLogBll();
            var rsp = new object();

            string action = context.Request["action"];
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            //三段逻辑：发起支付、支付成功、支付失败
            switch (action)
            {
                case "0": //发起支付：由payOrder.aspx调用

                    #region 准备微信支付所需参数

                    #region 相关配置参数

                    TenpayUtil tenpay = new TenpayUtil();
                    string paySignKey = ConfigurationManager.AppSettings["paySignKey"];
                    string appSecret = ConfigurationManager.AppSettings["AppSecret"];
                    string mchId = ConfigurationManager.AppSettings["mch_id"];
                    string appId = ConfigurationManager.AppSettings["AppId"];

                    #endregion

                    #region 获取前台传值

                    string code = context.Request["code"];
                    string state = context.Request["state"]; //支付金额_报名开始时间_报名截止时间，例如:1_2016-01-01
                    string[] strState = state.Split('_');
                    string school = context.Request["schoolName"];
                    string nm = context.Request["name"];
                    string sex = context.Request["sx"];
                    string identity = context.Request["id"];
                    string telNo = context.Request["tel"];

                    #endregion

                    #region 活动时间过期判断

                    DateTime endTime = Convert.ToDateTime(strState[1]);
                    if (DateTime.Now > endTime)
                        throw new Exception("亲，本次活动报名已经结束啦，请您继续关注休行最新活动！");

                    #endregion

                    #region 用户报名重复验证

                    PayLogInfo p0 = new PayLogInfo { IdentityNo = identity };
                    var payList = payBll.SearchLogById(p0);
                    if (payList.Count > 0)
                    {
                        DateTime dt = DateTime.Now; //当前时间        
                        DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).Date;
                        //本周周一  
                        var lst1 = payList.Where(i => i.CreateTime > startWeek).ToList();
                        var lst2 = lst1.Where(i => i.PayStatus == 1).ToList();
                        if (lst2.Count > 0)
                        {
                            throw new Exception("亲，您已经报过名啦");
                        }
                    }

                    #endregion

                    #region 根据code获取用户openid

                    string postData = "appid=" + appId + "&secret=" + appSecret + "&code=" + code +
                                      "&grant_type=authorization_code";
                    string requestData = TenpayUtil.PostDataToUrl(TenpayUtil.getOauth2Access_tokenUrl(), postData);
                    LogHelper.WriteInfoLog("获取用户openid返回：" + requestData);
                    Authorization auth = JsonConvert.DeserializeObject<Authorization>(requestData);
                    //将json数据转化为对象类型并赋值给auth

                    #endregion

                    string timeStamp = TenpayUtil.GetTimestamp();
                    string nonceStr = TenpayUtil.GetNoncestr();

                    #region 通过统一支付接口获取JSAPI支付所需的prepay_id

                    UnifiedOrder order = new UnifiedOrder();
                    order.appid = appId;
                    order.attach = "vinson";
                    order.body = "报名费用";
                    order.device_info = telNo;
                    order.mch_id = mchId;
                    order.nonce_str = TenpayUtil.GetNoncestr();
                    order.notify_url = baseUrl + "WxCallback.aspx";
                    order.openid = auth.openid;
                    order.out_trade_no = "ck" + timeStamp;
                    order.trade_type = "JSAPI";
                    order.spbill_create_ip = context.Request.UserHostAddress;
                    order.total_fee = int.Parse(strState[0]);
                    //order.total_fee = 600;
                    string prepayId = tenpay.getPrepay_id(order, paySignKey);

                    #endregion

                    LogHelper.WriteInfoLog("prepay_id:" + prepayId);

                    #region 获取微信sign

                    SortedDictionary<string, string> sParams = new SortedDictionary<string, string>
                        {
                            {"appId", appId},
                            {"timeStamp", timeStamp},
                            {"nonceStr", nonceStr},
                            {"package", "prepay_id=" + prepayId},
                            {"signType", "MD5"}
                        };
                    var paySign = TenpayUtil.CreateSign(sParams, paySignKey);

                    #endregion

                    #endregion

                    #region 支付记录入库

                    Task tk = new Task(() =>
                    {
                        PayLogInfo paylog = new PayLogInfo
                        {
                            CustName = nm,
                            IdentityNo = identity,
                            OpenId = auth.openid,
                            PayMoney = order.total_fee / 100.0,
                            SchoolName = school,
                            Sex = int.Parse(sex),
                            TelNo = telNo,
                            TradeNo = order.out_trade_no,
                            PayStatus = 0,
                            CreateTime = DateTime.Now, //切记：mongogdb中显示的是GMT时间
                            Remark = "初始"
                        };
                        payBll.InsertLog(paylog);
                    });
                    tk.Start();

                    #endregion

                    LogHelper.WriteInfoLog("paySign:" + paySign);
                    //由pay.aspx页面发起微信支付请求
                    string url = baseUrl + "pay.aspx?showwxpaytitle=1&appId=" + appId + "&timeStamp=" + timeStamp +
                                 "&nonceStr=" + nonceStr + "&prepay_id=" + prepayId + "&signType=MD5&paySign=" +
                                 paySign +
                                 "&OrderID=" + order.out_trade_no;
                    rsp = new { ErrCode = "0000", WxPayUrl = url };
                    break;
                case "1": //支付成功：由pay.aspx调用
                    string orderId1 = context.Request["orderId"];
                    PayLogInfo p1 = new PayLogInfo { TradeNo = orderId1 };
                    var rst1 = payBll.SearchLogByOrderId(p1); //获取支付记录
                    Task t = new Task(() =>
                    {
                        rst1.PayStatus = 1;
                        rst1.Remark = "支付成功";
                        payBll.UpdaPayLogInfo(rst1); //更新支付状态

                        #region 发送报名成功消息

                        string openid = rst1.OpenId;
                        string content = string.Format(ConfigurationManager.AppSettings["SMsg"], rst1.CustName, rst1.SchoolName, rst1.IdentityNo, rst1.TelNo);//短信模板
                        string token = new TenpayUtil().GetAccessToken();
                        string data = "{";
                        data += "\"touser\" : \"" + openid + "\",";
                        data += "\"msgtype\" : \"text\",";
                        data += "\"text\" : {\"content\":\"" + content + "\"}";
                        data += "}";


                        HttpHelper.PostData(
                            "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token,
                            data);

                        #endregion
                    });
                    t.Start();
                    rsp =
                        new
                        {
                            ErrCode = "0000",
                            WxPayUrl =
                                baseUrl + "success.aspx?n=" + rst1.CustName + "&&o=" + rst1.TradeNo + "&&m=" +
                                rst1.PayMoney
                        };
                    break;
                case "2": //支付失败：由pay.aspx调用
                    string orderId2 = context.Request["orderId"];
                    string errMsg = context.Request["ErrMsg"];
                    PayLogInfo p2 = new PayLogInfo { TradeNo = orderId2 };
                    var rst2 = payBll.SearchLogByOrderId(p2);
                    if (rst2.PayStatus == 1)
                    {
                        //用户支付成功后点击返回依然跳成功页面
                        rsp = new
                        {
                            ErrCode = "0000",
                            WxPayUrl =
                                baseUrl + "success.aspx?n=" + rst2.CustName + "&&o=" + rst2.TradeNo + "&&m=" +
                                rst2.PayMoney
                        };
                    }
                    else
                    {
                        rst2.PayStatus = -1;
                        rst2.Remark = "支付失败:" + errMsg;//数据库中保存详细错误信息
                        string showMsg = "支付失败";//默认显示
                        if (errMsg == "get_brand_wcpay_request:cancel")
                            showMsg = "取消支付";
                        payBll.UpdaPayLogInfo(rst2);
                        rsp = new { ErrCode = "0000", WxPayUrl = baseUrl + "fail.aspx?msg=" + showMsg };
                    }
                    break;
            }

            return JsonConvert.SerializeObject(rsp);
        }

        /// <summary>
        /// 微信授权认证获取openid
        /// </summary>
        private struct Authorization
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }
    }
}