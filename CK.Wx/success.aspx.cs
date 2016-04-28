using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CK.Wx
{
    public partial class success : System.Web.UI.Page
    {
        protected string Name = "";
        protected string Order = "";
        protected string PayMoney = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Name = Request.QueryString["n"];
            Order = Request.QueryString["o"];
            PayMoney = Request.QueryString["m"];
        }
    }
}