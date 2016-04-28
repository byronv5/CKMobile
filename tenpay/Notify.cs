using System;
using System.Text;
using System.Web.UI;
using CommonLibrary.Assist;

namespace tenpay
{
    public class Notify
    {
        private Page Page { get; set; }
        public Notify(Page page)
        {
            this.Page = page;
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Page.Request.InputStream;
            int count;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            LogHelper.WriteInfoLog(GetType().ToString(), "微信异步通知结果 : " + builder);

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (Exception ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                LogHelper.WriteInfoLog(GetType().ToString(), "签名验证失败 : " + res.ToXml());
                Page.Response.Write(res.ToXml());
                Page.Response.End();
            }

            LogHelper.WriteInfoLog(this.GetType().ToString(), "签名验证成功");
            return data;
        }

    }
}
