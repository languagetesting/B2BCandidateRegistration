using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace B2BCandidateRegistration
{
    public partial class Login : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Abandon();                
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

                if (Request.QueryString["timeout"] == "1")
                {
                    lblError.Text = "Session timed out, please re-login.";
                }
                else if (Request.QueryString["timeout"] == "2")
                {                
                    lblError.Text = "You are logged out of the system successfully.";
                }
            }
        }

        protected void lnkCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateRegistration.aspx", false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblError_OTP.Text = "";
                string strMessage = "";
                DataTable dtRet = myDBClass.Login_Validate(txtLoginID.Text, txtPassword.Text, GetClientIp(), Request.UserAgent);
                if (dtRet.Rows.Count > 0)
                {
                    string strRetMessage = dtRet.Rows[0]["RetMsg"].ToString();
                    string strRetVal = dtRet.Rows[0]["RetVal"].ToString();

                    if (strRetVal == "2")
                    {
                        pnlOTP.Visible = true;
                        pnlLogin.Visible = false;
                        lblError_OTP.Text = strRetMessage;
                        strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";
                    }
                    else if (strRetVal == "3")
                    {
                        lblError.Text = strRetMessage;
                        strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";                        
                    }
                    else
                    {
                        Session["User_EmailId"] = txtLoginID.Text;
                        Session["User_FirstName"] = dtRet.Rows[0]["User_FirstName"].ToString();
                        Session["User_LastName"] = dtRet.Rows[0]["User_LastName"].ToString();
                        Session["User_B2BRowId"] = dtRet.Rows[0]["B2BRowId"].ToString();
                        Response.Redirect("Enroll.aspx", false);
                        strMessage = "<script>HideProgress();</script>";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Failed to Login, please retry again.";
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnLogin_Click", "");
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
                    pnlOTP.Visible = true;
                    pnlLogin.Visible = false;
                    lblError_OTP.Text = strRetMessage;
                    strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";
                    if (strRetMessage == "Success")
                    {
                        Session["User_EmailId"] = txtLoginID.Text;
                        Session["User_FirstName"] = dtRet.Rows[0]["User_FirstName"].ToString();
                        Session["User_LastName"] = dtRet.Rows[0]["User_LastName"].ToString();
                        Session["User_B2BRowId"] = dtRet.Rows[0]["B2BRowId"].ToString();
                        Response.Redirect("Enroll.aspx", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                    }
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
                    strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";

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

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                string strEmail = txtLoginID.Text;
                if (strEmail == "")
                {
                    string strRetMessage = "Please enter your email Id and click on Forgot password link.";
                    //lblError.Text = strRetMessage;
                    strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                }
                else
                {                    
                    DataTable dtRet = myDBClass.Forgot_Password(txtLoginID.Text);
                    if (dtRet.Rows.Count > 0)
                    {
                        string strRetMessage = dtRet.Rows[0]["RetMsg"].ToString();
                        //lblError.Text = strRetMessage;
                        strMessage = "<script>showMessage('" + strRetMessage + "');HideProgress();</script>";                        
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                    }
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "lnkForgotPassword_Click", "");
                }
                catch { }
            }
        }
    }
}