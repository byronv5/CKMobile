$(function () {
    var btnPay = $('#btn_pay');
    var s = $('#wxState').val();
    //s形如4000_2016-04-23,报名费用_活动截止时间
    var endDate = new Date(s.split('_')[1].replace(/-/g,'/') + ' 00:00:00');//活动终止时间
    var nowDate = new Date();
    if (endDate > nowDate) {
        btnPay.click(function () {
            if ($('input:checkbox')[0].checked == true) {
                var c = $('#wxCode').val();

                var sc = $('#school').val().trim();
                var nm = $('#name').val().trim();
                var sx = $('#sex').val();
                var id = $('#identity').val();
                var tel = $('#telNo').val();
                if (!sc) {
                    Alert("请填写学校名称！");
                    return;
                }
                if (!nm) {
                    Alert("请填写您的姓名！");
                    return;
                }
                if (!isCardNo(id)) {
                    Alert("身份证号码格式填写有误！");
                    return;
                }
                if (TelValidate(tel)) {
                    $('#loadingToast').show();
                    $.ajax({
                        type: "post",
                        dataType: "json",
                        data: { "code": c, "state": s, "schoolName": sc, "name": nm, "sx": sx, "id": id, "tel": tel, "action": 0 },
                        url: "ajax/PayOrderHandle.ashx",
                        success: function (info) {
                            if (info.ErrCode == '0000') {
                                $(this).unbind('click'); //只点击一次
                                location.href = info.WxPayUrl;
                            } else {
                                $('#loadingToast').hide();
                                Alert(info.ErrMsg);
                            }
                        }
                    });
                }
            }
        });
        //同意服务条款checkbox事件
        $("#ipt_rule").change(function () {
            if ($('input:checkbox')[0].checked == true) {

                //启用报名按钮
                btnPay.removeClass('weui_btn_disabled weui_btn_default').addClass('weui_btn_primary');
                btnPay.attr('disable', false);
            } else {
                //禁用报名按钮
                btnPay.removeClass('weui_btn_primary').addClass('weui_btn_disabled weui_btn_default');
                btnPay.attr('disable', true);
            }
        });
    } else {
        //禁用报名按钮
        btnPay.removeClass('weui_btn_primary').addClass('weui_btn_disabled weui_btn_default');
        btnPay.attr('disable', true);
        Alert("亲，本次活动报名已经结束啦，请您继续关注休行最新活动！");
    }
});

//#region 身份证号码简单验证
function isCardNo(card) {
    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X  
    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (reg.test(card)) {
        return true;
    }
    return false;
}
//#endregion

//#region 手机号有效性校验
function TelValidate(tel) {
    if (tel) {
        var reg = /^[0-9]*$/;
        if ((tel.length == 11 && reg.test(tel)) && tel.substring(0, 1) == 1) {
            return true;
        }
        Alert("手机号码填写有误！");
        return false;
    } else {
        Alert("请输入手机号码！");
        return false;
    }
}
//#endregion

//#region 优美的弹出框
//function Alert(cont) {
//    trook.dialog.alert(cont);
//}
function Alert(str) {
    var dialog = "<div class=\"weui_dialog_alert\" id=\"dialog_alert\" style=\"display: none;\">";
    dialog += "<div class=\"weui_mask\"></div>";
    dialog += "<div class=\"weui_dialog\">";
    dialog += "<div class=\"weui_dialog_hd\"><strong class=\"weui_dialog_title\">温馨提示</strong></div>";
    dialog += "<div class=\"weui_dialog_bd\">" + str + "</div>";
    dialog += "<div class=\"weui_dialog_ft\">";
    dialog += "<a href=\"javascript:;\" class=\"weui_btn_dialog primary\" id=\"dialog_close\">确定</a>";
    dialog += "</div>";
    dialog += "</div>";
    dialog += "</div>";

    $("#err_alert")[0].innerHTML = dialog;
    $("#dialog_alert").show();

    $("#dialog_close").click(function () {
        $("#dialog_alert").hide();
    });
}
//#endregion
