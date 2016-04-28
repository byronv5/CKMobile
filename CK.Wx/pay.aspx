<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="CK.Wx.pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>微信安全支付</title>
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js"></script>
    <script type="text/javascript">
        var appId = "<%=AppId %>";
        var timeStamp = "<%=TimeStamp %>";
        var nonceStr = "<%=NonceStr %>";
        var prepay_id = "<%=PrepayId %>";
        var paySign = "<%=PaySign %>";
        var OrderID = "<%=OrderId %>";

        function onBridgeReady() {
            WeixinJSBridge.invoke(
               'getBrandWCPayRequest', {
                   "appId": appId,     //公众号名称，由商户传入     
                   "timeStamp": timeStamp,         //时间戳，自1970年以来的秒数     
                   "nonceStr": nonceStr, //随机串     
                   "package": "prepay_id=" + prepay_id,
                   "signType": "MD5",         //微信签名方式:     
                   "paySign": paySign //微信签名 
               },
               function (res) {
                   if (res.err_msg == "get_brand_wcpay_request:ok") {//支付成功
                       // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回ok，但并不保证它绝对可靠。

                       //更改数据库中支付状态
                       $.ajax({
                           type: "post",
                           dataType: "json",
                           data: { "orderId": OrderID, "action": 1 },
                           url: "ajax/PayOrderHandle.ashx",
                           success: function (info) {
                               if (info.ErrCode == '0000')
                                   window.location.href = info.WxPayUrl;
                           }
                       });
                   }
                   else {//取消支付||支付失败                  
                       $.ajax({
                           type: "post",
                           dataType: "json",
                           data: { "orderId": OrderID, "action": 2, "ErrMsg": res.err_msg },
                           url: "ajax/PayOrderHandle.ashx",
                           success: function (info) {
                               if (info.ErrCode == '0000')
                                   window.location.href = info.WxPayUrl;
                           }
                       });
                   }
               }
            );
        }
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
            } else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
                document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
            }
        } else {
            onBridgeReady();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
