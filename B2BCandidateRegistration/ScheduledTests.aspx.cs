using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TMS_Lib.DAL.ClientSite;
using JWT.Algorithms;
using JWT;
using JWT.Serializers;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace B2BCandidateRegistration
{
    public partial class ScheduledTests : System.Web.UI.Page
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
                lblError.Text = "Please retry, your request could not be processed.";
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

        protected void gvTestsSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTestsSchedule.DataSource = Session["gvData"];
            gvTestsSchedule.PageIndex = e.NewPageIndex;
            gvTestsSchedule.DataBind();
        }

        protected void gvTestsSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow gvr;
                    gvr = e.Row;
                    HiddenField hdnProductType = (HiddenField)e.Row.FindControl("hdnProductType");
                    HiddenField hdnProctorType = (HiddenField)e.Row.FindControl("hdnProctorType");

                    Button btnSchedule = (Button)e.Row.FindControl("btnSchedule");
                    Button btnInstructions = (Button)e.Row.FindControl("btnInstructions");
                    
                    if (hdnProctorType.Value != "5" && hdnProctorType.Value != "6")
                    {
                        btnSchedule.Visible = false;
                        btnInstructions.Visible = true;
                    }
                    else
                    {
                        btnInstructions.Visible = false;
                    }
                }

            }
            catch (Exception Ex)
            {
                lblError.Text = "Please retry, error occured while retreiving data";
                try
                {
                    myDBClass.ErrorLogWrite("TMS", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "gvTestsSchedule_RowDataBound", Session["User_EmailId"].ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        void GetData()
        {
            DataTable dt = myDBClass.B2B_TestsScheduled(Convert.ToInt32(Session["User_B2BRowId"].ToString()), Session["User_EmailId"].ToString());
            Session["gvData"] = dt;
            gvTestsSchedule.DataSource = dt;
            gvTestsSchedule.DataBind();
            if (dt.Rows.Count <= 0)
            {
                lblError.Text = "No Tests found.";
            }
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            GridViewRow gvr;
            Button btn;
            btn = (Button)sender;
            gvr = (GridViewRow)btn.NamingContainer;

            int aid = Convert.ToInt32(gvTestsSchedule.DataKeys[gvr.RowIndex].Values[0].ToString());
            HiddenField hdnCand_HashValue = (HiddenField)gvr.FindControl("hdnCand_HashValue");
            HiddenField hdnProctorType = (HiddenField)gvr.FindControl("hdnProctorType");
            try
            {
                var payload = new Dictionary<string, object>
                        {
                            { "Apptid", aid.ToString() },
                            { "Cand_HashValue", hdnCand_HashValue.Value },
                            { "DateTime", DateTime.UtcNow.ToString() },
                            { "MyToken", "AWS@2026" }
                        };
                string secret = "";
                string PageURL = "";
                secret = System.Configuration.ConfigurationManager.AppSettings["WPT_LTISecretKey"].ToString();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                var token = encoder.Encode(payload, secret);
                if (token == "")
                {
                    lblError.Text = "There seems to be an issue connecting to LTI Scheduling system, please logoff and retry again.";
                }
                else
                {
                    if (hdnProctorType.Value == "5" || hdnProctorType.Value == "6")
                    {
                        PageURL = System.Configuration.ConfigurationManager.AppSettings["LTIRemoteProctorSchedulingSystem"].ToString() + "?gId=" + token;
                        Response.Redirect(PageURL, false);
                    }
                    else
                    {
                        lblError.Text = "You cannot Schedule this Test. Contact LTI.";
                    }
                }
                string strMessage = "<script>HideProgress();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
            }
            catch (Exception Ex)
            {
                string strMessage = "<script>HideProgress();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                try
                {
                    myDBClass.ErrorLogWrite("TMS", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnSchedule_Click", Session["User_EmailId"].ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        protected void btnInstructions_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            GridViewRow gvr;
            Button btn;
            btn = (Button)sender;
            gvr = (GridViewRow)btn.NamingContainer;

            int aid = Convert.ToInt32(gvTestsSchedule.DataKeys[gvr.RowIndex].Values[0].ToString());            
            HiddenField hdnProctorType = (HiddenField)gvr.FindControl("hdnProctorType");
            try
            {                
                DataTable dtInstruction = myDBClass.B2B_GetInstructions(aid, Session["User_EmailId"].ToString());
                if (dtInstruction.Rows.Count > 0)
                {
                    lblInstruction.Text = dtInstruction.Rows[0]["Instruction"].ToString();
                }
                else
                {
                    lblInstruction.Text = "Please retry, unable to get Test Instructions.";
                }
                ModalPopupExtender1.Show();
            }
            catch (Exception Ex)
            {
                string strMessage = "<script>HideProgress();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", strMessage, false);
                try
                {
                    myDBClass.ErrorLogWrite("TMS", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnSchedule_Click", Session["User_EmailId"].ToString());
                }
                catch (Exception)
                {
                }
            }
        }

    }
}