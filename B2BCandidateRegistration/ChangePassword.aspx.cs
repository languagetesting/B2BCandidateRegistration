using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace B2BCandidateRegistration
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_EmailId"] == null)
            {
                Response.Redirect("Login.aspx?timeout=1");
            }
        }

        protected void btnChangepw_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                DataTable dtRet = myDBClass.ChangePassword(Session["User_EmailId"].ToString(), txtNewPassword.Text, Convert.ToInt32(Session["User_B2BRowId"].ToString()), txtPassword.Text);
                if (dtRet.Rows.Count > 0)
                {
                    string strRetMessage = dtRet.Rows[0]["RetMessage"].ToString();
                    string strRetVal = dtRet.Rows[0]["RetVal"].ToString();
                    
                    strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";                    

                    if (strMessage != "")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnChangepw_Click", "");
                }
                catch { }
            }
        }

    }
}