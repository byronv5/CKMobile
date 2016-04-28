using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary.Security;
using tenpay;

namespace CK.Wx
{
    public partial class WxSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_setKf_Click(object sender, EventArgs e)
        {
            string token = new TenpayUtil().GetAccessToken();
            string account = txt_account.Text.Trim();
            string name = txt_Name.Text.Trim();
            string psd = Md5Helper.SpMd5(txt_psd.Text.Trim());

            string data = "{";
            data += "\"kf_account\" : \"" + account + "\",";
            data += "\"nickname\" : \"" + name + "\",";
            data += "\"password\" : \"" + psd + "\"";
            data+="}";
            string rst =
                TenpayUtil.PostDataToUrl("https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + token,
                    data);

            Response.Write("添加客服信息返回：" + rst);
        }
    }
}