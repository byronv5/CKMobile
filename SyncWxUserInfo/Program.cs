using System;
using System.Collections.Generic;
using CK.Bll;
using CK.Model;
using CommonLibrary.Assist;
using Newtonsoft.Json;
using tenpay;

namespace SyncWxUserInfo
{
    static class Program
    {
        /// <summary>
        /// 同步关注公众号的用户到数据库
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LogHelper.WriteInfoLog("开始同步微信用户信息...");
            TenpayUtil tp = new TenpayUtil();
            WxUserBll userBll = new WxUserBll();
            #region 获取关注订阅号的用户openid列表
            string token = tp.GetAccessToken();
            string rsp1 = HttpHelper.GetResponse(
                            "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + token
                            );
            if (rsp1.Contains("errcode"))
                return;

            UserListJsonResult jsob = JsonConvert.DeserializeObject<UserListJsonResult>(rsp1);
            List<string> openidList = jsob.data.openid;
            #endregion
            foreach (var openid in openidList)
            {
                #region 获取用户基本信息
                string rsp2 =
                    HttpHelper.GetResponse("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token +
                                           "&openid=" + openid + "&lang=zh_CN ");//限制500w次/每天
                if (rsp1.Contains("errcode"))
                    continue;

                WxUserInfo user = JsonConvert.DeserializeObject<WxUserInfo>(rsp2);
                #endregion
                #region 同步用户信息
                //upsert
                var u = userBll.SearchUserByOpenid(user);
                if (u == null)
                {
                    userBll.UpdateUserInfo(user);
                }
                else
                {
                    userBll.InsertUser(user);
                }
                #endregion
            }
            if (jsob.total > 10000)
            {
                LogHelper.WriteInfoLog("卧槽，粉丝过万了！程序该升级了...");
            }
        }
    }
}
