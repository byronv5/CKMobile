<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxSet.aspx.cs" Inherits="CK.Wx.WxSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        kf_account：<asp:TextBox ID="txt_account" runat="server"></asp:TextBox>
        nickname：<asp:TextBox ID="txt_Name" runat="server"></asp:TextBox>
        password：<asp:TextBox ID="txt_psd" runat="server"></asp:TextBox>
        <asp:Button ID="btn_setKf" runat="server" Text="添加客服账号" OnClick="btn_setKf_Click" />
    </div>
    </form>
</body>
</html>
