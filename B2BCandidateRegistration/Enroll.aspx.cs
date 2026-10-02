using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace B2BCandidateRegistration
{
    public partial class Enroll : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                string strRetMessage = "";
                DataTable dtRet = myDBClass.EnrollCode_Validate(txtCode.Text);
                if (dtRet.Rows.Count <= 0)
                {
                    strRetMessage = "Invalid Enrollment Code.";
                    strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
                else
                {
                    DateTime dtExpiryDate = Convert.ToDateTime(dtRet.Rows[0]["ExpirationDate"].ToString());
                    if (dtExpiryDate < DateTime.Now.AddDays(-1))
                    {
                        strRetMessage = "Enrollment Code has Expired.";
                        strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                    }
                    else
                    {
                        Session["EnrollClientId"] = dtRet.Rows[0]["ClientId"].ToString();
                        Session["EnrollClientUserId"] = dtRet.Rows[0]["ClientUserId"].ToString();
                        Session["EnrollId"] = dtRet.Rows[0]["EnrollId"].ToString();
                        Response.Redirect("SelectTest.aspx",false);
                    }
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Please retry, failed to validate your Enrollment Code.";
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnLogin_Click", "");
                }
                catch { }
            }
        }
    }
}