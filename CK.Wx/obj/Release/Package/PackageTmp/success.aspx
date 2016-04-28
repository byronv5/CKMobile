<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="success.aspx.cs" Inherits="CK.Wx.success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报名成功</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <link href="common/css/weui.min.css" rel="stylesheet" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="common/js/WxConfig.js"></script>
    <script>
        $(function () {
            $.ajax({
                type: "post",
                dataType: "json",
                data: { "Pagepath": "success.aspx" },
                url: "ajax/GetWxJsApiConfig.ashx",
                success: function (data) {
                    wxconfig(data);
                }
            });
        });

        function closeWindow() {
            wx.closeWindow();
        }
    </script>
</head>
<body>
    <div class="weui_msg">
        <div class="weui_icon_area"><i class="weui_icon_success weui_icon_msg"></i></div>
        <div class="weui_text_area">
            <h2 class="weui_msg_title">报名成功</h2>
            <table style="margin: 0 auto;">
                <tr>
                    <td colspan="2">
                        <p>订单信息</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="weui_msg_desc">姓名</p>
                    </td>
                    <td>
                        <p class="weui_msg_desc"><%=Name %></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="weui_msg_desc">支付金额</p>
                    </td>
                    <td>
                        <p class="weui_msg_desc"><%=PayMoney %>元</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="weui_msg_desc">订单号</p>
                    </td>
                    <td>
                        <p class="weui_msg_desc"><%=Order %></p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="weui_opr_area">
            <p class="weui_btn_area">
                <a href="#" class="weui_btn weui_btn_primary" onclick="closeWindow()">返回公众号</a>
            </p>
        </div>
    </div>
</body>
</html>
