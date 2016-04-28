using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CommonLibrary.Assist;
using Newtonsoft.Json;

namespace CK.Wx.ajax
{
    public abstract class BaseAjax : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            context.Response.Expires = -1;
            context.Response.ContentType = "application/json";
            string result;
            try
            {
                result = GetAjaxResult(context);
            }
            catch (Exception ex)
            {
                var errorMsg = new { ErrCode = "1111", ErrMsg = ex.Message };
                LogHelper.WriteErrorLog("跳转支付异常", ex);
                result = JsonConvert.SerializeObject(errorMsg);
            }            
            context.Response.Write(result);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected abstract string GetAjaxResult(HttpContext context);

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}