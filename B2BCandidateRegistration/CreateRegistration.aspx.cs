using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace B2BCandidateRegistration
{
    public partial class CreateRegistration : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception Ex)
            {
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "Page_Load", "");
                }
                catch { }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                DataTable dtRet = myDBClass.CreateB2BRegistation(txtLoginID.Text, txtFirstName.Text, txtLastName.Text, txtPassword.Text, GetClientIp(), Request.UserAgent);
                if (dtRet.Rows.Count > 0)
                {
                    string strRetMessage = dtRet.Rows[0]["RetMessage"].ToString();
                    string strRetVal = dtRet.Rows[0]["RetVal"].ToString();

                    if (strRetVal == "1")
                    {
                        pnlLogin.Visible = false;
                        pnlOTP.Visible = true;
                        lblError_OTP.Text = strRetMessage;
                    }
                    else
                    {
                        lblError.Text = strRetMessage;
                    }

                    if (strRetMessage != "" && strRetVal != "1")
                        strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";
                    else
                    {
                        strMessage = "<script>HideProgress();</script>";
                    }

                    if (strMessage != "")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnCreate_Click", "");
                }
                catch { }
            }
        }

        protected void btnValidateOTP_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                DataTable dtRet = myDBClass.OTP_Validate(txtLoginID.Text, Convert.ToInt32(txtOTP.Text), "VALIDATE", GetClientIp(), Request.UserAgent);
                if (dtRet.Rows.Count > 0)
                {
                    string strRetMessage = dtRet.Rows[0]["RetMsg"].ToString();
                    if (strRetMessage != "Success")
                    {
                        lblError_OTP.Text = strRetMessage;
                        strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";
                    }
                    else
                    {
                        Session["User_EmailId"] = txtLoginID.Text;
                        Session["User_FirstName"] = dtRet.Rows[0]["User_FirstName"].ToString();
                        Session["User_LastName"] = dtRet.Rows[0]["User_LastName"].ToString();
                        Response.Redirect("Enroll.aspx", false);
                    }
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnValidateOTP_Click", "");
                }
                catch { }
            }
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                DataTable dtRet = myDBClass.OTP_Validate(txtLoginID.Text, 0, "GENERATE", GetClientIp(), Request.UserAgent);
                if (dtRet.Rows.Count > 0)
                {
                    string strRetMessage = dtRet.Rows[0]["RetMsg"].ToString();
                    pnlOTP.Visible = true;
                    pnlLogin.Visible = false;
                    lblError_OTP.Text = strRetMessage;
                    strMessage = "<script>HideProgress();showMessage('" + strRetMessage + "');</script>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnLogin_Click", "");
                }
                catch { }
            }
        }

        protected string GetClientIp()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ip))
            {
                // Sometimes X_FORWARDED_FOR has multiple IPs (client, proxy, etc.)
                string[] addresses = ip.Split(',');
                if (addresses.Length > 0)
                {
                    return addresses[0];
                }
            }

            string ip_addr = Request.ServerVariables["REMOTE_ADDR"];
            if (!string.IsNullOrEmpty(ip_addr))
            {
                return ip_addr;
            }

            return Request.UserHostAddress;
        }
        
    }
}