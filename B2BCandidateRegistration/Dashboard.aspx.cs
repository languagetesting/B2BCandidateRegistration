using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TMS_Lib.DAL.ClientSite;

namespace B2BCandidateRegistration
{
    public partial class Dashboard : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();
        TMS_Lib.BAL.ClientSite client = new TMS_Lib.BAL.ClientSite();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["User_EmailId"] == null)
                    {
                        Response.Redirect("Login.aspx?timeout=1");
                    }
                    else
                    {
                        lblError.Text = "";
                        GetData();
                    }
                }
            }
            catch (Exception ex)
            {                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", ex.Message + ex.StackTrace, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "Page_Load()", "");
                }
                catch
                {
                }
            }
        }

        void GetData()
        {
            DataTable dt = myDBClass.B2B_DashBoard(Convert.ToInt32(Session["User_B2BRowId"].ToString()), Session["User_EmailId"].ToString());                        
            if (dt.Rows.Count > 0)
            {
                lnkTesttoSchedule.Text = "( " + dt.Rows[0]["TestsToSchedule"].ToString() + " ) ";
                lnkTestScheduled.Text = "( " + dt.Rows[0]["TestScheduled"].ToString() + " ) ";
            }
            else
            {
                lnkTesttoSchedule.Text = "( 0 ) ";
                lnkTestScheduled.Text = "( 0 ) ";
            }
        }

        protected void lnkEnrollment_Click(object sender, EventArgs e)
        {
            try
            {                
                Response.Redirect("Enroll.aspx", false);
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "lnkEnrollment_Click", "");
                }
                catch { }
            }
        }

        protected void lnkTesttoSchedule_Click(object sender, EventArgs e)
        {
            try
            {             
                Response.Redirect("TestsToSchedule.aspx", false);
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "lnkTesttoSchedule_Click", "");
                }
                catch { }
            }
        }

        protected void lnkTestScheduled_Click(object sender, EventArgs e)
        {
            try
            {             
                Response.Redirect("ScheduledTests.aspx", false);
            }
            catch (Exception Ex)
            {
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "lnkTestScheduled_Click", "");
                }
                catch { }
            }
        }

    }
}