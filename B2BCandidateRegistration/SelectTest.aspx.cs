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
    public partial class SelectTest : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();
        TMS_Lib.BAL.ClientSite client = new TMS_Lib.BAL.ClientSite();
        public Dictionary<int, UserTestOption> lstTestTypes;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["EnrollClientId"] == null)
                    {
                        Response.Redirect("Enroll.aspx", false);
                    }
                    else
                    {
                        DataTable dtEnrollTestOptions = myDBClass.EnrollCode_TestOptions(Convert.ToInt32(Session["EnrollId"].ToString()));
                        lstTestTypes = new Dictionary<int, UserTestOption>();
                        int clientid = Convert.ToInt32(Session["EnrollClientId"].ToString());
                        var lst2 = client.getClientTestOptions(clientid);

                        if (lst2.Count() > 0)
                        {
                            foreach (usp_client_getClientTestOptionsResult opt in lst2)
                            {
                                try
                                {
                                    UserTestOption ut = new UserTestOption();
                                    ut.testOptionId = opt.testoptionid;
                                    ut.testOptionName = opt.testoptionname;
                                    ut.producttype = (int)opt.producttype;
                                    ut.subproducttype = (int)opt.subproducttype;
                                    ut.testdelivery = (int)opt.testdelivery;
                                    ut.isbundle = (int)opt.isbundle;
                                    ut.testid = (int)opt.testid;
                                    ut.isslot = (bool)opt.openslot;
                                    ut.testingmode = (int)opt.TestingMode;
                                    ut.whomakecalls = (int)opt.WhoMakeCalls;
                                    ut.hasOPI = (int)opt.HasOPI;
                                    ut.hasOPIProctorless = (int)opt.HasProctorlessOPI;
                                    ut.OPIAfter = (int)opt.OPIAfter;
                                    ut.ProctorType = (int)opt.ProctorType;
                                    ut.B2BMultipleCoupon = -1;
                                    
                                    // Exceptions are Open Slot, ALIRA, SOBL, Bundle tests and AAPPL tests
                                    if (opt.openslot == false && !(opt.subproducttype == 45 || opt.subproducttype == 46 || opt.subproducttype == 26))
                                    {
                                        if (dtEnrollTestOptions.Select("TestOptionId = " + opt.testoptionid).Length > 0)
                                        {
                                            ListItem li = new ListItem(opt.testoptionname, opt.testoptionid.ToString());
                                            rbgSelectTest.Items.Add(li);
                                            lstTestTypes.Add(opt.testoptionid, ut);
                                        }
                                    }
                                    else
                                    {
                                        int i = 0;
                                        int j = 1;
                                        int c = 0;
                                        c = i + j;
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }                            

                            if (lstTestTypes.Count <= 0)
                            {
                                lblError.Text = "There are no Test available for enrollment Code. Please contact LTI.";
                            }
                        }
                        else
                        {
                            lblError.Text = "There are no Test available for enrollment Code. Please contact LTI.";
                        }                        
                        Session["lstTestTypes"] = lstTestTypes;
                    }
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Please retry, your request could not be processed.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "Page_Load", "");
                }
                catch { }
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {                
                lstTestTypes = (Dictionary<int, UserTestOption>)Session["lstTestTypes"];

                int pType = -1;
                int testdelivery = -1;
                int isbundle = 0;
                int testid = -1;
                bool isslot = false;
                int testingmode = -1;
                int whomakecalls = -1;
                int hasOPIProctorless = 0;
                int OPIAfter = 0;
                int spType = -1;
                int proctortype = 0;
                int b2bMultipleCoupon = 0;

                //find the properties for the selected test
                if (lstTestTypes.ContainsKey(Convert.ToInt32(rbgSelectTest.SelectedValue)))
                {
                    UserTestOption ut = lstTestTypes[Convert.ToInt32(rbgSelectTest.SelectedValue)];
                    pType = ut.producttype;
                    testdelivery = ut.testdelivery;
                    isbundle = ut.isbundle;
                    testid = ut.testid;
                    isslot = ut.isslot;
                    testingmode = ut.testingmode;
                    whomakecalls = ut.whomakecalls;
                    hasOPIProctorless = ut.hasOPIProctorless;
                    OPIAfter = ut.OPIAfter;
                    spType = ut.subproducttype;
                    proctortype = ut.ProctorType;
                    b2bMultipleCoupon = ut.B2BMultipleCoupon;
                }
                else
                {
                    lblError.Text = "Test properties not found. Please contact LTI.";
                }

                //store test properties 
                Session["ProductType"] = pType;
                Session["SubProductType"] = spType;
                Session["TestDelivery"] = testdelivery;
                if (isbundle == 1)
                    Session["IsBundle"] = true;
                else
                    Session["IsBundle"] = false;
                Session["testoptionid"] = Convert.ToInt32(rbgSelectTest.SelectedValue);
                Session["TestName"] = rbgSelectTest.SelectedItem.ToString();
                Session["testid"] = testid;
                Session["IsSlot"] = isslot;
                Session["TestingMode"] = testingmode;
                Session["WhoMakeCalls"] = whomakecalls;
                Session["hasOPIProctorless"] = hasOPIProctorless;
                string strProctorTitle = myDBClass.GetProctorTitle(testid);
                Session["ProctorTitle"] = strProctorTitle;
                Session["ProctorType"] = proctortype;
                Session["B2BMultipleCoupon"] = b2bMultipleCoupon;
                Session["OPIAfter"] = OPIAfter;

                Response.Redirect("TestRequest.aspx", false);

            }
            catch (Exception ex)
            {
                lblError.Text = "Please retry, your request could not be processed.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", ex.Message + ex.StackTrace, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnContinue_Click()", "");
                }
                catch
                {
                }
            }
        }

        public class UserTestOption
        {
            public int testOptionId { get; set; }
            public string testOptionName { get; set; }
            public int testdelivery { get; set; }
            public int producttype { get; set; }
            public int subproducttype { get; set; }
            public int isbundle { get; set; }
            public int testid { get; set; }
            public bool isslot { get; set; }
            public int testingmode { get; set; }
            public int whomakecalls { get; set; }
            public int hasOPI { get; set; }
            public int hasOPIProctorless { get; set; }
            public int OPIAfter { get; set; }
            public int CustomExpiryDt { get; set; }
            public int ExpiryNoofDays { get; set; }
            public int ProctorType { get; set; }
            public int B2BMultipleCoupon { get; set; }
        }

    }
}