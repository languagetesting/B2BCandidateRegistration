using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace B2BCandidateRegistration
{
    public partial class B2BCandDashboard : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_FirstName"] == null)
            {
                Response.Redirect("Login.aspx?timeout=1");
            }
        }
    }
}