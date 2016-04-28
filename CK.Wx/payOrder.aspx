<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payOrder.aspx.cs" Inherits="CK.Wx.PayOrder" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>填写报名信息</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <link href="common/css/weui.min.css" rel="stylesheet" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="common/js/payOrder.js"></script> 
</head>
<body>
    <div class="weui_cells_title" style="text-align: center;font-size: 17px;">请完善报名信息</div>
    <div class="weui_cells weui_cells_form">
        <div class="weui_cell">
            <div class="weui_cell_hd">
                <label class="weui_label">学校</label>
            </div>
            <div class="weui_cell_bd weui_cell_primary">
                <input class="weui_input" id="school" type="text" maxlength="20" />
            </div>
        </div>
        <div class="weui_cell">
            <div class="weui_cell_hd">
                <label class="weui_label">姓名</label>
            </div>
            <div class="weui_cell_bd weui_cell_primary">
                <input class="weui_input" id="name" type="text" maxlength="10" />
            </div>
        </div>
        <div class="weui_cell">
            <div class="weui_cell_hd">
                <label class="weui_label">性别</label>
            </div>
            <div class="weui_cell_bd weui_cell_primary">
                <select class="weui_select" id="sex">
                    <option selected="selected" value="2">女</option>
                    <option value="1">男</option>
                </select>
            </div>
        </div>
        <div class="weui_cell">
            <div class="weui_cell_hd">
                <label class="weui_label">身份证</label>
            </div>
            <div class="weui_cell_bd weui_cell_primary">
                <input class="weui_input" id="identity" type="text" maxlength="18" />
            </div>
        </div>
        <div class="weui_cell">
            <div class="weui_cell_hd">
                <label class="weui_label">手机</label>
            </div>
            <div class="weui_cell_bd weui_cell_primary">
                <input class="weui_input" id="telNo" type="tel" maxlength="11" />
            </div>
        </div>
    </div>
    <div class="weui_cells_tips">温馨提示:我们将严格保密您的个人信息，身份证号码仅用作购买旅行保险</div>
    <div class="weui_cells_tips" style="text-align: center"><input id="ipt_rule" type="checkbox" checked="checked"/>同意《<a href="rule.html">休行服务条款</a>》</div>
    <div class="weui_btn_area">
        <a id="btn_pay" href="#" class="weui_btn weui_btn_primary">确认报名</a>
    </div>
    <div id="loadingToast" class="weui_loading_toast" style="display: none;">
        <div class="weui_mask_transparent"></div>
        <div class="weui_toast">
            <div class="weui_loading">
                <div class="weui_loading_leaf weui_loading_leaf_0"></div>
                <div class="weui_loading_leaf weui_loading_leaf_1"></div>
                <div class="weui_loading_leaf weui_loading_leaf_2"></div>
                <div class="weui_loading_leaf weui_loading_leaf_3"></div>
                <div class="weui_loading_leaf weui_loading_leaf_4"></div>
                <div class="weui_loading_leaf weui_loading_leaf_5"></div>
                <div class="weui_loading_leaf weui_loading_leaf_6"></div>
                <div class="weui_loading_leaf weui_loading_leaf_7"></div>
                <div class="weui_loading_leaf weui_loading_leaf_8"></div>
                <div class="weui_loading_leaf weui_loading_leaf_9"></div>
                <div class="weui_loading_leaf weui_loading_leaf_10"></div>
                <div class="weui_loading_leaf weui_loading_leaf_11"></div>
            </div>
            <p class="weui_toast_content">正在跳转支付</p>
        </div>
    </div>
    <input type="hidden" id="wxCode" value="<%= Code%>" />
    <input type="hidden" id="wxState" value="<%= State%>" />
    <div id="err_alert"></div>
</body>
</html>
