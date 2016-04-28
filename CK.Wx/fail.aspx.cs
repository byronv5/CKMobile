using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CK.Wx
{
    public partial class fail : System.Web.UI.Page
    {
        protected string Msg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Msg = Request.QueryString["msg"];
        }
    }
}