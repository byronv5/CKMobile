<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fail.aspx.cs" Inherits="CK.Wx.fail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>支付失败</title>
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
    <div class="weui_icon_area"><i class="weui_icon_msg weui_icon_warn"></i></div>
    <div class="weui_text_area">
        <h2 class="weui_msg_title">报名失败</h2>
        <p class="weui_msg_desc"><%=Msg %></p>
    </div>
    <div class="weui_opr_area">
        <p class="weui_btn_area">
            <a href="#" class="weui_btn weui_btn_primary" onclick="closeWindow()">返回公众号</a>
        </p>
    </div>
</div>
</body>
</html>
