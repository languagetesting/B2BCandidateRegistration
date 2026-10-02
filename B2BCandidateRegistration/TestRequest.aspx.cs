using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TMS_Lib.DAL.ClientSite;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;

namespace B2BCandidateRegistration
{
    public partial class TestRequest : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();
        TMS_Lib.BAL.ClientSite client = new TMS_Lib.BAL.ClientSite();
        List<usp_client_getTestLanguagesResult> testLanguagesLst;
        List<usp_client_getTestLanguagesResult> testOriginalLanguagesLst;
        List<usp_client_getTestLanguagesByTestIDResult> testLanguagesByTestIDLst;
        List<usp_client_getTestLevelsResult> testLevelsRPTLst;
        List<usp_client_getTestLevelsResult> testLevelsLPTLst;
        List<TMS_Lib.DAL.TMSDropdown.usp_getTimeZonesResult> timeZoneLst;
        List<usp_client_getTestsInBundleResult> testsInBundleLst;
        DataTable dtTestLevelOPIc;
        public int testoptid;
        public string testname;
        public string cfTitle1, cfTitle2, cfTitle3, cfTitle4, cfTitle5, cfTitle6, cfTitle7, cfTitle8, cfTitle9, cfTitle10;
        public List<usp_client_getCandidateCustomFieldsResult> candidateCFLst;
        public int productType;
        public string message = "";
        public bool errorOccurred = false;
        public bool isBundle;
        public bool isStateFarm = false;
        public string CandidateNumLabel;
        public string CandidateNameMessage;
        public bool isUnofficial = false;
        List<usp_client_getTestInstructionLanguagesResult> instrLanguagesLst;
        List<usp_client_getClientCustomFieldsResult> clientCFLst;

        // Below is from user control        
        protected string ddlOpicLevelsName;
        protected string ddlRPTLevelsName;
        protected string ddlLPTLevelsName;
        protected string ddlLNRInstructionLangName;        
        protected string childFieldControlName;
        protected int controlId = 1;
        protected string controlName = "Candidate_1";

        public int ControlId
        {
            get { return controlId; }
            set { controlId = value; }
        }        

        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; }
        }

        public bool IsBundle
        {
            get { return isBundle; }
            set { isBundle = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["TestName"] == null)
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                    else
                    {
                        txtName.Text = Session["User_FirstName"].ToString() + " " + Session["User_LastName"].ToString();
                        testname = (string)Session["TestName"];
                        productType = (int)Session["ProductType"];

                        testoptid = (int)Session["testoptionid"];
                        int clientid = Convert.ToInt32(Session["EnrollClientId"].ToString());
                        lblTestName.Text = Session["TestName"].ToString();

                        Session["NewClientID"] = "-1";
                        Session["IsNewClient"] = "0";
                        Session["NewTestOptionID"] = "-1";
                        Session["NewTestID"] = "-1";

                        Session["NewSubformID"] = "-1";
                        Session["NewIsFormRandom"] = "-1";

                        Session["ProductTypeID"] = "";
                        Session["4Skills"] = "0";
                                                
                        var lst = myDBClass.getClientDetailedInfo(clientid, Convert.ToInt32(Session["EnrollClientUserId"].ToString())).ToList();

                        try
                        {
                            DataTable dt4Skills = myDBClass.Check4Skills(clientid, testoptid);
                            if (dt4Skills.Rows.Count > 0)
                            {
                                if (dt4Skills.Rows[0]["RetVal"].ToString() == "YES")
                                    Session["4Skills"] = "1";
                            }
                        }
                        catch { }

                        string mappedCustomFieldName = myDBClass.GetClientsCustomFieldMapping(clientid);
                        if (mappedCustomFieldName != "")
                            Session["MappedCustomFieldName"] = mappedCustomFieldName;
                        else
                            Session["MappedCustomFieldName"] = "";

                        if (lst.Count() <= 0)
                        {
                            lblError.Text = "No data found for client.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                        }
                        else
                        {
                            try
                            {                                
                                timeZoneLst = client.getTimeZones().ToList();

                                if (timeZoneLst.Count() <= 0)
                                {
                                    Session["ErrorMessage"] = "No time zone found for client.";                             
                                }

                                //default 
                                var itemTZ = new TMS_Lib.DAL.TMSDropdown.usp_getTimeZonesResult();
                                itemTZ.TimeZoneID = -1;
                                itemTZ.TimeZoneCode = "Select Time Zone";
                                itemTZ.TimeZoneDesc = "Select Time Zone";
                                timeZoneLst.Insert(0, itemTZ);
                                
                                Session["TimeZone"] = timeZoneLst;

                                Session["IsPrePostFieldsRequired"] = lst[0].IsPrePostFieldsRequired;

                                //set up client custom fields 
                                clientCFLst = client.getClientCustomFields(clientid).ToList();

                                //candidate custom field columns (will be set up in user control)
                                candidateCFLst = client.getCandidateCustomFields(clientid, testoptid).ToList();

                                if ((candidateCFLst != null) && (candidateCFLst.Count > 0))
                                    Session["candidateCFLst"] = candidateCFLst[0];
                                else
                                    Session["candidateCFLst"] = null;

                                //check and store custom field relations
                                StoreCustomFieldRelations();

                                if ((clientCFLst != null) && (clientCFLst.Count > 0))
                                {
                                    int intEnabled = 0;
                                    for (int j = 0; j < clientCFLst.Count; j++)
                                    {
                                        if ((clientCFLst[j].F1_Enabled == true && clientCFLst[j].F1_Visible == true) || (clientCFLst[j].F2_Enabled == true && clientCFLst[j].F2_Visible == true)
                                            || (clientCFLst[j].F3_Enabled == true && clientCFLst[j].F3_Visible == true) || (clientCFLst[j].F4_Enabled == true && clientCFLst[j].F4_Visible == true)
                                            || (clientCFLst[j].F5_Enabled == true && clientCFLst[j].F5_Visible == true) || (clientCFLst[j].F6_Enabled == true && clientCFLst[j].F6_Visible == true)
                                            || (clientCFLst[j].F7_Enabled == true && clientCFLst[j].F7_Visible == true) || (clientCFLst[j].F8_Enabled == true && clientCFLst[j].F8_Visible == true)
                                            || (clientCFLst[j].F9_Enabled == true && clientCFLst[j].F9_Visible == true) || (clientCFLst[j].F10_Enabled == true && clientCFLst[j].F10_Visible == true)
                                           )
                                        {
                                            intEnabled = 1;
                                        }
                                    }

                                    if (intEnabled == 1)
                                    {
                                        pnlClientCustomFields.Visible = true;
                                        SetupClientCustomFields(clientCFLst);
                                        Session["clientCFLst"] = clientCFLst;
                                    }
                                }

                                //*** For candidate section ***

                                //get test languages 
                                testLanguagesLst = client.getTestLanguages(clientid, testoptid).ToList();                                

                                //default language
                                var item = new usp_client_getTestLanguagesResult();
                                item.languageid = -1;
                                item.name = "Select";
                                testLanguagesLst.Insert(0, item);

                                Session["testLanguagesList"] = testLanguagesLst;

                                List<usp_client_getTestLanguagesResult> testOriginalLanguagesLst = new List<usp_client_getTestLanguagesResult>();
                                for (int i = 0; i < testLanguagesLst.Count; i++)
                                {
                                    var item2 = new usp_client_getTestLanguagesResult();
                                    item2.languageid = testLanguagesLst[i].languageid;
                                    item2.name = testLanguagesLst[i].name;
                                    testOriginalLanguagesLst.Insert(i, item2);
                                }

                                Session["OriginaltestLanguagesList"] = testOriginalLanguagesLst;

                                //get tests in bundle
                                testsInBundleLst = client.getTestsInBundle(clientid, testoptid).ToList();
                                Session["bundleTestsList"] = testsInBundleLst;

                                //store for later use if needed
                                Session["OriginalTestID"] = testsInBundleLst[0].clienttestid;

                                if (testsInBundleLst.Count() <= 0)
                                {
                                    lblError.Text = "No individual tests found for selected bundle test.";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                                }

                                //get RPT/LPT test levels  (a bundle won't have two of the same tests)
                                int intsetpnlOPINote = 0;
                                for (int i = 0; i < testsInBundleLst.Count; i++)
                                {
                                    if (testsInBundleLst[i].producttype == 1)
                                    {
                                        intsetpnlOPINote = 1;
                                    }

                                    if (testsInBundleLst[i].producttype == 3)
                                    {
                                        testLevelsRPTLst = client.getTestLevels(clientid, 3, (int)testsInBundleLst[i].clienttestid).ToList();
                                        Session["ProductTypeID"] = "3";
                                    }
                                    else if (testsInBundleLst[i].producttype == 5)
                                    {
                                        testLevelsLPTLst = client.getTestLevels(clientid, 5, (int)testsInBundleLst[i].clienttestid).ToList();
                                        Session["ProductTypeID"] = "5";
                                    }
                                    else if (testsInBundleLst[i].producttype == 2)
                                    {
                                        if (testsInBundleLst[i].IsSelectionbyClient == true)
                                        {                                            
                                            dtTestLevelOPIc = myDBClass.GetClientFormSelection((int)testsInBundleLst[i].clienttestid);
                                        }
                                        Session["ProductTypeID"] = "2";
                                    }
                                    else if (testsInBundleLst[i].producttype == 9)
                                    {
                                        //get test instruction languages                                        
                                        instrLanguagesLst = myDBClass.getTestInstructionLanguages(clientid, (int)testsInBundleLst[i].clienttestid).ToList();

                                        //default language
                                        var itemInstr = new usp_client_getTestInstructionLanguagesResult();
                                        itemInstr.languageid = -1;
                                        itemInstr.name = "Select";
                                        instrLanguagesLst.Insert(0, itemInstr);

                                        Session["testInstrLanguagesList"] = instrLanguagesLst;

                                        Session["ProductTypeID"] = "9";
                                    }
                                    else if (testsInBundleLst[i].producttype == 4)
                                    {
                                        Session["ProductTypeID"] = "4";
                                    }

                                    if (testsInBundleLst[i].RatingCertificationName.ToUpper().StartsWith("UNOFFICIAL"))
                                        isUnofficial = true;

                                    if (((bool)Session["IsBundle"]) == true)
                                    {
                                        pnlTestFields.Visible = true;
                                        testLanguagesByTestIDLst = myDBClass.getTestLanguagesByTestID(clientid, (int)testsInBundleLst[i].clienttestid).ToList();

                                        //default language
                                        var itemLangByTestID = new usp_client_getTestLanguagesByTestIDResult();
                                        itemLangByTestID.languageid = -1;
                                        itemLangByTestID.name = "Select";
                                        testLanguagesByTestIDLst.Insert(0, itemLangByTestID);
                                        Session["testLanguagesList_" + testsInBundleLst[i].producttype] = testLanguagesByTestIDLst;
                                    }
                                }

                                //default level for RPT/LPT (ok to add this even if test does not have RPT or LPT 
                                var level = new usp_client_getTestLevelsResult();
                                level.formid = -1;
                                level.formname = "Select Level";
                                if (testLevelsRPTLst != null)
                                    testLevelsRPTLst.Insert(0, level);
                                if (testLevelsLPTLst != null)
                                    testLevelsLPTLst.Insert(0, level);

                                Session["testLevelsRPTList"] = testLevelsRPTLst;
                                Session["testLevelsLPTList"] = testLevelsLPTLst;
                                Session["testLevelsOPIcList"] = dtTestLevelOPIc;

                                //Setup Custom fields for Candidate on Top
                                SetupCandidateTopCustomFields();

                                if (Session["IsBundle"] != null)
                                {
                                    IsBundle = (bool)Session["IsBundle"];
                                    if (IsBundle)
                                    {
                                        lblLanguage.Visible = false;
                                        ddlLanguage.Visible = false;
                                    }
                                    else
                                    {
                                        lblLanguage.Visible = true;
                                        ddlLanguage.Visible = true;
                                    }
                                }
                                else
                                {
                                    IsBundle = false;
                                    lblLanguage.Visible = true;
                                    ddlLanguage.Visible = true;
                                }

                                // User control code
                                ddlLanguage.DataSource = Session["testLanguagesList"];
                                ddlLanguage.DataTextField = "name";
                                ddlLanguage.DataValueField = "languageid";
                                ddlLanguage.DataBind();
                                cvLanguage.ErrorMessage = "Please select a Language.";

                                //set up candidate custom fields
                                usp_client_getCandidateCustomFieldsResult cfCheckObj = (usp_client_getCandidateCustomFieldsResult)Session["candidateCFLst"];
                                if (cfCheckObj != null)
                                {
                                    pnlCustomSection.Visible = true;
                                }
                                else
                                {
                                    pnlCustomSection.Visible = false;
                                }

                                SetupCandidateCustomFields();

                                //set up tests
                                SetupEachTest();

                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Error when loading data for Test request, Pleast retry again.";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                            }
                        }
                    }
                }
                else
                {
                    //recreate client custom fields
                    if (Session["clientCFLst"] != null)
                    {
                        SetupClientCustomFields((List<usp_client_getClientCustomFieldsResult>)Session["clientCFLst"]);
                    }
                    //Setup Custom fields for Candidate on Top
                    SetupCandidateTopCustomFields();

                    SetupCandidateCustomFields();
                    //set up tests
                    SetupEachTest();
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

        public void StoreCustomFieldRelations()
        {
            string cfMasterField;
            string cfChildField;
            int cfRelationId;
            bool hasClientCFMasterField = false;
            bool hasClientCFChildField = false;
            bool hasCandidateCFMasterField = false;
            bool hasCandidateCFChildField = false;
            
            DataTable dtCustomFieldRelation = myDBClass.getCustomFieldRelationInfoForClient(Convert.ToInt32(Session["EnrollClientId"].ToString()));

            if (dtCustomFieldRelation != null)
            {
                if (dtCustomFieldRelation.Rows.Count >= 1)
                {
                    cfRelationId = Convert.ToInt32(dtCustomFieldRelation.Rows[0]["RelationshipID"]);
                    cfMasterField = dtCustomFieldRelation.Rows[0]["MasterField"].ToString().ToUpper();
                    cfChildField = dtCustomFieldRelation.Rows[0]["ChildField"].ToString().ToUpper();


                    //check and store parent custom field relation
                    if ((clientCFLst != null) && (clientCFLst.Count > 0))
                    {
                        usp_client_getClientCustomFieldsResult cfParentObj = clientCFLst[0];

                        if ((bool)cfParentObj.F1_Enabled)
                        {
                            // must match and must be drop down
                            if ((cfParentObj.F1_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F1_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F1_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F1_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F2_Enabled)
                        {
                            if ((cfParentObj.F2_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F2_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F2_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F2_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F3_Enabled)
                        {
                            if ((cfParentObj.F3_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F3_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F3_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F3_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F4_Enabled)
                        {
                            if ((cfParentObj.F4_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F4_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F4_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F4_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F5_Enabled)
                        {
                            if ((cfParentObj.F5_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F5_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F5_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F5_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F6_Enabled)
                        {
                            if ((cfParentObj.F6_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F6_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F6_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F6_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F7_Enabled)
                        {
                            if ((cfParentObj.F7_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F7_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F7_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F7_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F8_Enabled)
                        {
                            if ((cfParentObj.F8_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F8_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F8_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F8_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F9_Enabled)
                        {
                            if ((cfParentObj.F9_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F9_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F9_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F9_Type == 2))
                                hasClientCFChildField = true;
                        }
                        if ((bool)cfParentObj.F10_Enabled)
                        {
                            if ((cfParentObj.F10_Title.ToUpper() == cfMasterField) && ((int)cfParentObj.F10_Type == 2))
                                hasClientCFMasterField = true;
                            if ((cfParentObj.F10_Title.ToUpper() == cfChildField) && ((int)cfParentObj.F10_Type == 2))
                                hasClientCFChildField = true;
                        }

                        if ((hasClientCFMasterField) && (hasClientCFChildField))
                        {
                            Session["CF_hasClientRelationship"] = true;
                            Session["CF_ClientRelation_ID"] = cfRelationId;
                            Session["CF_ClientRelation_MasterField"] = cfMasterField;
                            Session["CF_ClientRelation_ChildField"] = cfChildField;
                        }
                        else
                            Session["CF_hasClientRelationship"] = false;
                    }
                    else
                        Session["CF_hasClientRelationship"] = false;

                    //store child custom field relation
                    if ((candidateCFLst != null) && (candidateCFLst.Count > 0))
                    {
                        usp_client_getCandidateCustomFieldsResult cfObj = candidateCFLst[0];

                        if ((bool)cfObj.F1_Enabled)
                        {
                            // must match and must be drop down
                            if ((cfObj.F1_Title.ToUpper() == cfMasterField) && ((int)cfObj.F1_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F1_Title.ToUpper() == cfChildField) && ((int)cfObj.F1_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F2_Enabled)
                        {
                            if ((cfObj.F2_Title.ToUpper() == cfMasterField) && ((int)cfObj.F2_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F2_Title.ToUpper() == cfChildField) && ((int)cfObj.F2_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F3_Enabled)
                        {
                            if ((cfObj.F3_Title.ToUpper() == cfMasterField) && ((int)cfObj.F3_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F3_Title.ToUpper() == cfChildField) && ((int)cfObj.F3_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F4_Enabled)
                        {
                            if ((cfObj.F4_Title.ToUpper() == cfMasterField) && ((int)cfObj.F4_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F4_Title.ToUpper() == cfChildField) && ((int)cfObj.F4_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F5_Enabled)
                        {
                            if ((cfObj.F5_Title.ToUpper() == cfMasterField) && ((int)cfObj.F5_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F5_Title.ToUpper() == cfChildField) && ((int)cfObj.F5_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F6_Enabled)
                        {
                            if ((cfObj.F6_Title.ToUpper() == cfMasterField) && ((int)cfObj.F6_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F6_Title.ToUpper() == cfChildField) && ((int)cfObj.F6_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F7_Enabled)
                        {
                            if ((cfObj.F7_Title.ToUpper() == cfMasterField) && ((int)cfObj.F7_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F7_Title.ToUpper() == cfChildField) && ((int)cfObj.F7_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F8_Enabled)
                        {
                            if ((cfObj.F8_Title.ToUpper() == cfMasterField) && ((int)cfObj.F8_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F8_Title.ToUpper() == cfChildField) && ((int)cfObj.F8_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F9_Enabled)
                        {
                            if ((cfObj.F9_Title.ToUpper() == cfMasterField) && ((int)cfObj.F9_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F9_Title.ToUpper() == cfChildField) && ((int)cfObj.F9_Type == 2))
                                hasCandidateCFChildField = true;
                        }
                        if ((bool)cfObj.F10_Enabled)
                        {
                            if ((cfObj.F10_Title.ToUpper() == cfMasterField) && ((int)cfObj.F10_Type == 2))
                                hasCandidateCFMasterField = true;
                            if ((cfObj.F10_Title.ToUpper() == cfChildField) && ((int)cfObj.F10_Type == 2))
                                hasCandidateCFChildField = true;
                        }

                        if ((hasCandidateCFMasterField) && (hasCandidateCFChildField))
                        {
                            Session["CF_HasCandidateRelationship"] = true;
                            Session["CF_CandidateRelation_ID"] = cfRelationId;
                            Session["CF_CandidateRelation_MasterField"] = cfMasterField;
                            Session["CF_CandidateRelation_ChildField"] = cfChildField;
                        }
                        else
                            Session["CF_HasCandidateRelationship"] = false;
                    }
                    else
                        Session["CF_HasCandidateRelationship"] = false;
                }
                else
                {
                    Session["CF_HasClientRelationship"] = false;
                    Session["CF_HasCandidateRelationship"] = false;
                }
            }
            else
            {
                Session["CF_HasClientRelationship"] = false;
                Session["CF_HasCandidateRelationship"] = false;
            }

        }

        private void SetupClientCustomFields(List<usp_client_getClientCustomFieldsResult> cfList)
        {
            try
            {
                usp_client_getClientCustomFieldsResult cfObj = cfList[0];

                Session["ClientCustomFields"] = cfObj;
                if (cfObj.F1_Title != null)
                    SetupLabelAndControl(lblCFClient1, cfObj.F1_Title, ctrlCFClientArea1, (int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Required, (bool)cfObj.F1_Visible, (bool)cfObj.F1_Encrypted, cfObj.F1_XML, 1);
                if (cfObj.F2_Title != null)
                    SetupLabelAndControl(lblCFClient2, cfObj.F2_Title, ctrlCFClientArea2, (int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Required, (bool)cfObj.F2_Visible, (bool)cfObj.F2_Encrypted, cfObj.F2_XML, 2);
                if (cfObj.F3_Title != null)
                    SetupLabelAndControl(lblCFClient3, cfObj.F3_Title, ctrlCFClientArea3, (int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Required, (bool)cfObj.F3_Visible, (bool)cfObj.F3_Encrypted, cfObj.F3_XML, 3);
                if (cfObj.F4_Title != null)
                    SetupLabelAndControl(lblCFClient4, cfObj.F4_Title, ctrlCFClientArea4, (int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Required, (bool)cfObj.F4_Visible, (bool)cfObj.F4_Encrypted, cfObj.F4_XML, 4);
                if (cfObj.F5_Title != null)
                    SetupLabelAndControl(lblCFClient5, cfObj.F5_Title, ctrlCFClientArea5, (int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Required, (bool)cfObj.F5_Visible, (bool)cfObj.F5_Encrypted, cfObj.F5_XML, 5);
                if (cfObj.F6_Title != null)
                    SetupLabelAndControl(lblCFClient6, cfObj.F6_Title, ctrlCFClientArea6, (int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Required, (bool)cfObj.F6_Visible, (bool)cfObj.F6_Encrypted, cfObj.F6_XML, 6);
                if (cfObj.F7_Title != null)
                    SetupLabelAndControl(lblCFClient7, cfObj.F7_Title, ctrlCFClientArea7, (int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Required, (bool)cfObj.F7_Visible, (bool)cfObj.F7_Encrypted, cfObj.F7_XML, 7);
                if (cfObj.F8_Title != null)
                    SetupLabelAndControl(lblCFClient8, cfObj.F8_Title, ctrlCFClientArea8, (int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Required, (bool)cfObj.F8_Visible, (bool)cfObj.F8_Encrypted, cfObj.F8_XML, 8);
                if (cfObj.F9_Title != null)
                    SetupLabelAndControl(lblCFClient9, cfObj.F9_Title, ctrlCFClientArea9, (int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Required, (bool)cfObj.F9_Visible, (bool)cfObj.F9_Encrypted, cfObj.F9_XML, 9);
                if (cfObj.F10_Title != null)
                    SetupLabelAndControl(lblCFClient10, cfObj.F10_Title, ctrlCFClientArea10, (int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Required, (bool)cfObj.F10_Visible, (bool)cfObj.F10_Encrypted, cfObj.F10_XML, 10);
            }
            catch (Exception Ex)
            {         
                myDBClass.ErrorLogWrite("ClientSite", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "SetupClientCustomFields", "SetupClientCustomFields");
            }
        }

        private void SetupLabelAndControl(Label cfLabel, string title, Panel controlArea, int controltype, bool enabled, bool required, bool visible, bool encrypted, string xmlString, int index)
        {
            if (enabled)
            {
                cfLabel.Text = title + ":";
                cfLabel.CssClass = "LTIformlabel";
                if (controltype == 1)    //textbox
                {
                    TextBox tf = new TextBox();
                    tf.ID = "control" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-175px";
                    controlArea.Controls.Add(tf);

                    if (required)  //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);
                    }

                    //if (xmlString != "")    //default value
                    //{
                    //    tf.Text = xmlString;
                    //}
                }
                else if (controltype == 2)   //drop-down list
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "control" + index;
                    ddl.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "-1";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvDDL);
                    }

                    if (xmlString != "" && xmlString != "<ROOT />")     //default value
                    {
                        StringReader sReader = new StringReader(xmlString);
                        DataSet dSet = new DataSet();
                        dSet.ReadXml(sReader);
                        ddl.DataSource = dSet;
                        ddl.DataValueField = "ItemValue";
                        ddl.DataTextField = "ItemText";
                        ddl.DataBind();
                    }

                    ddl.Items.Insert(0, new ListItem("Select a value", "-1"));

                    if ((Session["CF_HasClientRelationship"] != null) && ((bool)Session["CF_HasClientRelationship"] == true))
                    {
                        //if parent relationship dropdown, add onchange method
                        if ((Session["CF_ClientRelation_MasterField"].ToString() == title.ToUpper()))
                        {
                            ddl.SelectedIndexChanged += new System.EventHandler(this.ddlParentCFMasterField_SelectedIndexChanged);
                            ddl.AutoPostBack = true;
                        }

                        //if child relationship dropdown, store name of control for future use 
                        if ((Session["CF_ClientRelation_ChildField"].ToString() == title.ToUpper()))
                        {
                            //clear any default entries (entries will be populated when parent value selected
                            ddl.Items.Clear();
                            ddl.Items.Insert(0, new ListItem("Select a value", "-1"));

                            //store control name 
                            Session["CF_ClientRelationControlArea"] = controlArea;
                            Session["CF_ClientRelationChildControlName"] = ddl.ID;
                        }
                    }                    

                }
                else if (controltype == 3)    //date field
                {
                    TextBox tf = new TextBox();
                    tf.ID = "control" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-date LTIformtextbox-175px";
                    tf.TextMode = TextBoxMode.Date;
                    controlArea.Controls.Add(tf);
                    
                    if (required)   //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);

                        CompareValidator cvText = new CompareValidator();
                        cvText.Display = ValidatorDisplay.None;
                        cvText.ErrorMessage = title + " is not a valid date";
                        cvText.Type = ValidationDataType.Date;
                        cvText.Operator = ValidationCompareOperator.DataTypeCheck;
                        cvText.ControlToValidate = tf.ID;
                        cvText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvText);
                    }

                    //if (xmlString != "")    //default value
                    //{
                    //    tf.Text = xmlString;
                    //}
                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = new ListBox();
                    ddl.ID = "control" + index;
                    ddl.SelectionMode = ListSelectionMode.Multiple;
                    ddl.Height = System.Web.UI.WebControls.Unit.Pixel(80);
                    ddl.CssClass = "LTIlistbox LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvDDL);
                    }

                    if (xmlString != "" && xmlString != "<ROOT />")     //default value
                    {
                        StringReader sReader = new StringReader(xmlString);
                        DataSet dSet = new DataSet();
                        dSet.ReadXml(sReader);
                        ddl.DataSource = dSet;
                        ddl.DataValueField = "ItemValue";
                        ddl.DataTextField = "ItemText";
                        ddl.DataBind();
                    }

                }
                if (visible)
                {
                    cfLabel.Visible = true;
                    controlArea.Visible = true;
                }
            }
        }

        protected void ddlParentCFMasterField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string masterfielddata;

            //get current values from master/child fields
            DropDownList ddlMasterField = (DropDownList)sender;
            masterfielddata = ddlMasterField.SelectedItem.Value;


            if (masterfielddata == "Select a value")
            {
                DropDownList ddlChildRelation = (DropDownList)((Panel)Session["CF_ClientRelationControlArea"]).FindControl(Session["CF_ClientRelationChildControlName"].ToString());
                ddlChildRelation.Items.Clear();
                ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));
            }
            else
            {                
                DataTable dtChildData = myDBClass.getCFRelationDataForMasterField(Convert.ToInt32(Session["CF_ClientRelation_ID"]), masterfielddata);

                if (dtChildData != null)
                {
                    DropDownList ddlChildRelation = (DropDownList)((Panel)Session["CF_ClientRelationControlArea"]).FindControl(Session["CF_ClientRelationChildControlName"].ToString());
                    ddlChildRelation.DataSource = dtChildData;
                    ddlChildRelation.DataTextField = "ChildFieldData";
                    ddlChildRelation.DataValueField = "ChildFieldData";

                    ddlChildRelation.DataBind();
                    ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));
                }
                else
                {
                    DropDownList ddlChildRelation = (DropDownList)((Panel)Session["CF_ClientRelationControlArea"]).FindControl(Session["CF_ClientRelationChildControlName"].ToString());
                    ddlChildRelation.Items.Clear();
                    ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));
                }
            }
        }

        private void SetupCandidateTopCustomFields()
        {
            if (Session["candidateCFLst"] != null)
            {
                usp_client_getCandidateCustomFieldsResult cfObj = (usp_client_getCandidateCustomFieldsResult)Session["candidateCFLst"];
                if (cfObj.F1_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan1, cfObj.F1_Title, ctrlCFCanArea1, (int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Required, (bool)cfObj.F1_Visible, (bool)cfObj.F1_Encrypted, cfObj.F1_XML, 1);
                }
                if (cfObj.F2_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan2, cfObj.F2_Title, ctrlCFCanArea2, (int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Required, (bool)cfObj.F2_Visible, (bool)cfObj.F2_Encrypted, cfObj.F2_XML, 2);
                }
                if (cfObj.F3_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan3, cfObj.F3_Title, ctrlCFCanArea3, (int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Required, (bool)cfObj.F3_Visible, (bool)cfObj.F3_Encrypted, cfObj.F3_XML, 3);
                }
                if (cfObj.F4_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan4, cfObj.F4_Title, ctrlCFCanArea4, (int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Required, (bool)cfObj.F4_Visible, (bool)cfObj.F4_Encrypted, cfObj.F4_XML, 4);
                }
                if (cfObj.F5_Top.ToString() == "1")
                    SetupCustomCandidateTopLabelAndControl(lblCfCan5, cfObj.F5_Title, ctrlCFCanArea5, (int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Required, (bool)cfObj.F5_Visible, (bool)cfObj.F5_Encrypted, cfObj.F5_XML, 5);
                if (cfObj.F6_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan6, cfObj.F6_Title, ctrlCFCanArea6, (int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Required, (bool)cfObj.F6_Visible, (bool)cfObj.F6_Encrypted, cfObj.F6_XML, 6);
                }
                if (cfObj.F7_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan7, cfObj.F7_Title, ctrlCFCanArea7, (int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Required, (bool)cfObj.F7_Visible, (bool)cfObj.F7_Encrypted, cfObj.F7_XML, 7);
                }
                if (cfObj.F8_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan8, cfObj.F8_Title, ctrlCFCanArea8, (int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Required, (bool)cfObj.F8_Visible, (bool)cfObj.F8_Encrypted, cfObj.F8_XML, 8);
                }
                if (cfObj.F9_Top.ToString() == "1")
                {
                    SetupCustomCandidateTopLabelAndControl(lblCfCan9, cfObj.F9_Title, ctrlCFCanArea9, (int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Required, (bool)cfObj.F9_Visible, (bool)cfObj.F9_Encrypted, cfObj.F9_XML, 9);
                }
                if (cfObj.F10_Top.ToString() == "1")
                    SetupCustomCandidateTopLabelAndControl(lblCfCan10, cfObj.F10_Title, ctrlCFCanArea10, (int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Required, (bool)cfObj.F10_Visible, (bool)cfObj.F10_Encrypted, cfObj.F10_XML, 10);
            }
        }

        //set up an individual candidate custom field
        private void SetupCustomCandidateTopLabelAndControl(Label cfLabel, string title, Panel controlArea, int controltype, bool enabled, bool required, bool visible, bool encrypted, string xmlString, int index)
        {
            if (enabled)
            {
                cfLabel.Text = title + ":";
                cfLabel.CssClass = "LTIformlabel";
                if (controltype == 1)    //textbox
                {
                    TextBox tf = new TextBox();
                    tf.ID = "controlcan" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-175px";
                    controlArea.Controls.Add(tf);

                    if (required)  //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);
                    }

                    if (xmlString != "")    //default value
                    {
                        tf.Text = xmlString;
                    }
                }
                else if (controltype == 2)   //drop-down list
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "controlcan" + index;
                    ddl.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "-1";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvDDL);
                    }

                    if (xmlString != "")     //default value
                    {
                        StringReader sReader = new StringReader(xmlString);
                        DataSet dSet = new DataSet();
                        dSet.ReadXml(sReader);
                        ddl.DataSource = dSet;
                        ddl.DataValueField = "ItemValue";
                        ddl.DataTextField = "ItemText";
                        ddl.DataBind();
                    }

                    ddl.Items.Insert(0, new ListItem("Select a value", "-1"));                    

                }
                else if (controltype == 3)    //date field
                {
                    TextBox tf = new TextBox();
                    tf.ID = "controlcan" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-date LTIformtextbox-175px";
                    tf.TextMode = TextBoxMode.Date;
                    controlArea.Controls.Add(tf);
                    
                    if (required)   //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);

                        CompareValidator cvText = new CompareValidator();
                        cvText.Display = ValidatorDisplay.None;
                        cvText.ErrorMessage = title + " is not a valid date";
                        cvText.Type = ValidationDataType.Date;
                        cvText.Operator = ValidationCompareOperator.DataTypeCheck;
                        cvText.ControlToValidate = tf.ID;
                        cvText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvText);
                    }

                    if (xmlString != "")    //default value
                    {
                        tf.Text = xmlString;
                    }
                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = new ListBox();
                    ddl.ID = "controlcan" + index;
                    ddl.SelectionMode = ListSelectionMode.Multiple;
                    ddl.Height = System.Web.UI.WebControls.Unit.Pixel(80);
                    ddl.CssClass = "LTIlistbox LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvDDL);
                    }

                    if (xmlString != "")     //default value
                    {
                        StringReader sReader = new StringReader(xmlString);
                        DataSet dSet = new DataSet();
                        dSet.ReadXml(sReader);
                        ddl.DataSource = dSet;
                        ddl.DataValueField = "ItemValue";
                        ddl.DataTextField = "ItemText";
                        ddl.DataBind();
                    }
                }
                if (visible)
                {
                    cfLabel.Visible = true;
                    controlArea.Visible = true;
                }
            }
        }

        private void SetupCandidateCustomFields()
        {
            if (Session["candidateCFLst"] != null)
            {
                usp_client_getCandidateCustomFieldsResult cfObj = (usp_client_getCandidateCustomFieldsResult)Session["candidateCFLst"];
                if (cfObj.F1_Top.ToString() == "0")
                {
                    if (cfObj.F1_Title != "")
                    {
                        pnlcf1section.Visible = true;
                    }
                    pnlcf1brk.Visible = true;
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient1, cfObj.F1_Title, ctrlCand_Bot_CFClientArea1, (int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Required, (bool)cfObj.F1_Visible, (bool)cfObj.F1_Encrypted, cfObj.F1_XML, 1);
                }
                if (cfObj.F2_Top.ToString() == "0")
                {
                    if (cfObj.F2_Title != "")
                    {
                        pnlcf1section.Visible = true;
                    }
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient2, cfObj.F2_Title, ctrlCand_Bot_CFClientArea2, (int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Required, (bool)cfObj.F2_Visible, (bool)cfObj.F2_Encrypted, cfObj.F2_XML, 2);
                }
                if (cfObj.F3_Top.ToString() == "0")
                {
                    if (cfObj.F3_Title != "")
                    {
                        pnlcf2section.Visible = true;
                    }
                    pnlcf3brk.Visible = true;
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient3, cfObj.F3_Title, ctrlCand_Bot_CFClientArea3, (int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Required, (bool)cfObj.F3_Visible, (bool)cfObj.F3_Encrypted, cfObj.F3_XML, 3);
                }
                if (cfObj.F4_Top.ToString() == "0")
                {
                    if (cfObj.F4_Title != "")
                    {
                        pnlcf2section.Visible = true;
                    }
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient4, cfObj.F4_Title, ctrlCand_Bot_CFClientArea4, (int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Required, (bool)cfObj.F4_Visible, (bool)cfObj.F4_Encrypted, cfObj.F4_XML, 4);
                }
                if (cfObj.F5_Top.ToString() == "0")
                {
                    if (cfObj.F5_Title != "")
                    {
                        pnlcf3section.Visible = true;
                    }
                    pnlcf5brk.Visible = true;
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient5, cfObj.F5_Title, ctrlCand_Bot_CFClientArea5, (int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Required, (bool)cfObj.F5_Visible, (bool)cfObj.F5_Encrypted, cfObj.F5_XML, 5);
                }
                if (cfObj.F6_Top.ToString() == "0")
                {
                    if (cfObj.F6_Title != "")
                    {
                        pnlcf3section.Visible = true;
                    }
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient6, cfObj.F6_Title, ctrlCand_Bot_CFClientArea6, (int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Required, (bool)cfObj.F6_Visible, (bool)cfObj.F6_Encrypted, cfObj.F6_XML, 6);
                }
                if (cfObj.F7_Top.ToString() == "0")
                {
                    if (cfObj.F7_Title != "")
                    {
                        pnlcf4section.Visible = true;
                    }
                    pnlcf7brk.Visible = true;
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient7, cfObj.F7_Title, ctrlCand_Bot_CFClientArea7, (int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Required, (bool)cfObj.F7_Visible, (bool)cfObj.F7_Encrypted, cfObj.F7_XML, 7);
                }
                if (cfObj.F8_Top.ToString() == "0")
                {
                    if (cfObj.F8_Title != "")
                    {
                        pnlcf4section.Visible = true;
                    }
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient8, cfObj.F8_Title, ctrlCand_Bot_CFClientArea8, (int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Required, (bool)cfObj.F8_Visible, (bool)cfObj.F8_Encrypted, cfObj.F8_XML, 8);
                }
                if (cfObj.F9_Top.ToString() == "0")
                {
                    if (cfObj.F9_Title != "")
                    {
                        pnlcf5section.Visible = true;
                    }
                    pnlcf9brk.Visible = true;
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient9, cfObj.F9_Title, ctrlCand_Bot_CFClientArea9, (int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Required, (bool)cfObj.F9_Visible, (bool)cfObj.F9_Encrypted, cfObj.F9_XML, 9);
                }
                if (cfObj.F10_Top.ToString() == "0")
                {
                    if (cfObj.F10_Title != "")
                    {
                        pnlcf5section.Visible = true;
                    }
                    SetupCustomCandidateLabelAndControl(lblCand_Bot_CFClient10, cfObj.F10_Title, ctrlCand_Bot_CFClientArea10, (int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Required, (bool)cfObj.F10_Visible, (bool)cfObj.F10_Encrypted, cfObj.F10_XML, 10);
                }
            }
        }

        //set up an individual candidate custom field
        private void SetupCustomCandidateLabelAndControl(Label cfLabel, string title, Panel controlArea, int controltype, bool enabled, bool required, bool visible, bool encrypted, string xmlString, int index)
        {
            if (enabled)
            {                
                cfLabel.Text = title + ":";
                cfLabel.CssClass = "LTIformlabel";
                if (controltype == 1)    //textbox
                {
                    TextBox tf = new TextBox();
                    tf.ID = controlId + "_" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-175px";
                    controlArea.Controls.Add(tf);

                    if (required)  //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);
                    }

                    if (xmlString != "")    //default value
                    {
                        tf.Text = xmlString;
                    }
                }
                else if (controltype == 2)   //drop-down list
                {

                    DropDownList ddl = new DropDownList();
                    ddl.ID = controlId + "_" + index;
                    ddl.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "-1";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        pnlTestFields.Controls.Add(cvDDL);
                    }
                    
                    //if CF relationship exists
                    if ((Session["CF_HasCandidateRelationship"] != null) && ((bool)Session["CF_HasCandidateRelationship"] == true))
                    {
                        //if parent relationship dropdown, add onchange method
                        if ((Session["CF_CandidateRelation_MasterField"].ToString() == title.ToUpper()))
                        {
                            ddl.SelectedIndexChanged += new System.EventHandler(this.ddlCandidateCFMasterField_SelectedIndexChanged);
                            ddl.AutoPostBack = true;
                        }

                        //if child relationship dropdown, store name of control for future use 
                        if ((Session["CF_CandidateRelation_ChildField"].ToString() == title.ToUpper()))
                        {
                            //clear any default entries (entries will be populated when parent value selected
                            ddl.Items.Clear();

                            //store control name 
                            childFieldControlName = controlId + "_" + index;

                            //add JS code to preserve value so we can restore on postback
                            ddl.Attributes.Add("onchange", "document.getElementById('" + hfCFChildDataValue.ClientID + "').value =  document.getElementById('" + ddl.ClientID + "').value;");
                        }
                    }                    
                }
                else if (controltype == 3)    //date field
                {
                    TextBox tf = new TextBox();
                    tf.ID = controlId + "_" + index;
                    tf.CssClass = "LTIformtextbox LTIformtextbox-date LTIformtextbox-175px";
                    tf.TextMode = TextBoxMode.Date;
                    controlArea.Controls.Add(tf);                    

                    if (required)   //add required validator if needed
                    {
                        RequiredFieldValidator rfText = new RequiredFieldValidator();
                        rfText.Display = ValidatorDisplay.None;
                        rfText.ErrorMessage = title + " is required";
                        rfText.ControlToValidate = tf.ID;
                        rfText.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfText);

                        CompareValidator cvText1 = new CompareValidator();
                        cvText1.Display = ValidatorDisplay.None;
                        cvText1.ErrorMessage = title + " is not a valid date";
                        cvText1.Type = ValidationDataType.Date;
                        cvText1.Operator = ValidationCompareOperator.DataTypeCheck;
                        cvText1.ControlToValidate = tf.ID;
                        cvText1.CssClass = "LTIvalidation-error";
                        pnlTestFields.Controls.Add(cvText1);
                    }

                    if (xmlString != "")    //default value
                    {
                        tf.Text = xmlString;
                    }

                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = new ListBox();
                    ddl.ID = controlId + "_" + index;
                    ddl.SelectionMode = ListSelectionMode.Multiple;
                    ddl.Height = System.Web.UI.WebControls.Unit.Pixel(80);
                    ddl.CssClass = "LTIlistbox LTIformtextbox-175px";
                    controlArea.Controls.Add(ddl);

                    if (required)    //add required validator if needed
                    {
                        RequiredFieldValidator rfDDL = new RequiredFieldValidator();
                        rfDDL.Display = ValidatorDisplay.None;
                        rfDDL.ErrorMessage = title + " is required";
                        rfDDL.ControlToValidate = ddl.ID;
                        rfDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(rfDDL);

                        CompareValidator cvDDL = new CompareValidator();
                        cvDDL.Display = ValidatorDisplay.None;
                        cvDDL.ErrorMessage = title + " is required";
                        cvDDL.ValueToCompare = "";
                        cvDDL.Operator = ValidationCompareOperator.NotEqual;
                        cvDDL.ControlToValidate = ddl.ID;
                        cvDDL.CssClass = "LTIvalidation-error";
                        controlArea.Controls.Add(cvDDL);
                    }


                    if (xmlString != "" && xmlString != "<ROOT />")     //default value
                    {
                        StringReader sReader = new StringReader(xmlString);
                        DataSet dSet = new DataSet();
                        dSet.ReadXml(sReader);
                        if (dSet.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dSet.Tables[0].Rows.Count; i++)
                            {
                                dSet.Tables[0].Rows[i]["ItemText"] = dSet.Tables[0].Rows[i]["ItemText"].ToString().Replace("_", " ");
                                dSet.Tables[0].Rows[i]["ItemValue"] = dSet.Tables[0].Rows[i]["ItemValue"].ToString().Replace("_", " ");
                            }
                        }
                        ddl.DataSource = dSet;
                        ddl.DataValueField = "ItemValue";
                        ddl.DataTextField = "ItemText";
                        ddl.DataBind();
                    }
                }

                if (visible)
                {
                    cfLabel.Visible = true;
                    controlArea.Visible = true;
                }
            }
        }

        protected void ddlCandidateCFMasterField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string masterfielddata;
            string childfielddata;

            //get current values from master/child fields
            DropDownList ddlMasterField = (DropDownList)sender;
            masterfielddata = ddlMasterField.SelectedItem.Value;
            childfielddata = hfCFChildDataValue.Value;
            if (masterfielddata == "Select a value")
            {
                DropDownList ddlChildRelation = (DropDownList)this.FindControl(childFieldControlName);
                ddlChildRelation.Items.Clear();
                ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));
            }
            else
            {                
                DataTable dtChildData = myDBClass.getCFRelationDataForMasterField(Convert.ToInt32(Session["CF_CandidateRelation_ID"]), masterfielddata);

                if (dtChildData != null)
                {
                    DropDownList ddlChildRelation = (DropDownList)this.FindControl(childFieldControlName);
                    ddlChildRelation.DataSource = dtChildData;
                    ddlChildRelation.DataTextField = "ChildFieldData";
                    ddlChildRelation.DataValueField = "ChildFieldData";

                    ddlChildRelation.DataBind();
                    ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));

                    //restore previously selected value
                    if (ddlChildRelation.Items.FindByValue(childfielddata) != null)
                        ddlChildRelation.Items.FindByValue(childfielddata).Selected = true;
                }
                else
                {
                    DropDownList ddlChildRelation = (DropDownList)this.FindControl(childFieldControlName);
                    ddlChildRelation.Items.Clear();
                    ddlChildRelation.Items.Insert(0, new ListItem("Select a value", "-1"));
                }
            }
        }

        private void SetupEachTest()
        {
            List<usp_client_getTestsInBundleResult> testsInBundleLst = (List<usp_client_getTestsInBundleResult>)Session["bundleTestsList"];
            int producttype;
            string testname;
            int whoMakeCalls;
            int RatingGrid;
            int TestLevel;
            int SubFormId;
            bool IsRandom;
            int TestDelivery;
            bool IsSelectionbyClient;
            int customExpiryDt = 0;
            int expiryNoofDays = 0;

            TableRow tRow;
            TableCell tCell;

            //loop for each test within the bundle
            for (int i = 0; i < testsInBundleLst.Count; i++)
            {
                testname = testsInBundleLst[i].clienttestname;
                producttype = (int)testsInBundleLst[i].producttype;
                whoMakeCalls = (int)testsInBundleLst[i].WhoMakeCalls;
                RatingGrid = (int)testsInBundleLst[i].RatingGrid;
                TestLevel = (int)testsInBundleLst[i].TestFormLevel;
                SubFormId = (int)testsInBundleLst[i].SubFormId;
                IsRandom = testsInBundleLst[i].IsFormRandom;
                TestDelivery = (int)testsInBundleLst[i].TestDelivery;
                IsSelectionbyClient = testsInBundleLst[i].IsSelectionbyClient;
                customExpiryDt = testsInBundleLst[i].CustomExpiryDt;
                expiryNoofDays = testsInBundleLst[i].ExpiryNoofDays;

                if (i != 0)
                {
                    //create an empty row as a separator between each test 
                    tRow = new TableRow();
                    tCell = new TableCell();
                    tCell.ColumnSpan = 11;   //11 columns total per row
                    tRow.Cells.Add(tCell);
                    tstTable.Rows.Add(tRow);
                }

                switch (producttype)
                {
                    case 1:    //OPI
                        CreateOPIFields(testname, i, whoMakeCalls, RatingGrid);
                        break;
                    case 2:   //OPIC
                        CreateOPIcFields(testname, i, RatingGrid, IsSelectionbyClient, customExpiryDt, expiryNoofDays);
                        break;
                    case 3:   //RPT
                        CreateRPTFields(testname, i, TestLevel, SubFormId, IsRandom, customExpiryDt, expiryNoofDays);
                        break;
                    case 4:   //WPT
                        CreateWPTFields(testname, i, RatingGrid, TestDelivery, customExpiryDt, expiryNoofDays);
                        break;
                    case 5:   //LPT
                        CreateLPTFields(testname, i, TestLevel, SubFormId, IsRandom, customExpiryDt, expiryNoofDays);
                        break;
                    case 9:  //L & R
                        CreateLAndRFields(testname, i, TestLevel, SubFormId, IsRandom, customExpiryDt, expiryNoofDays);
                        break;
                    default:
                        CreateGenericTestFields(testname, i);
                        break;
                }
            }
        }

        //create OPI specific components
        private void CreateOPIFields(string testname, int index, int whoMakeCalls, int RatingGrid)
        {
            pnlOPIFields.Visible = true;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            TableRow tRow;
            TableCell tCell;
            tRow = new TableRow();
            int Controls_On_Row = 0;
            pnlTestFields.Visible = true;

            {
                tRow = new TableRow();

                Label lblCandidatePhone = new Label();
                lblCandidatePhone.Text = "Phone: ";
                lblCandidatePhone.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblCandidatePhone);
                tRow.Cells.Add(tCell);

                TextBox tfCPhone = new TextBox();
                tfCPhone.ID = controlId + "_tfCPhone_" + index;
                //tfCPhone.Width = System.Web.UI.WebControls.Unit.Pixel(100);
                tfCPhone.ToolTip = "Please enter a phone";
                tfCPhone.CssClass = "LTIformtextbox LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.Controls.Add(tfCPhone);
                tRow.Cells.Add(tCell);

                RequiredFieldValidator rfCPhone = new RequiredFieldValidator();
                rfCPhone.Display = ValidatorDisplay.None;
                rfCPhone.ErrorMessage = controlName + ": " + "Phone is required";
                rfCPhone.ControlToValidate = tfCPhone.ID;
                rfCPhone.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(rfCPhone);

                tCell = new TableCell();
                tCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                tRow.Cells.Add(tCell);

                Label lblCandidateTimeZone = new Label();
                lblCandidateTimeZone.Text = "Your Time Zone: ";
                lblCandidateTimeZone.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblCandidateTimeZone);
                tRow.Cells.Add(tCell);

                DropDownList ddlCandTimeZone = new DropDownList();
                ddlCandTimeZone.ID = controlId + "_ddlCandTimeZone_" + index;
                ddlCandTimeZone.DataSource = Session["TimeZone"];
                ddlCandTimeZone.DataTextField = "TimeZoneCode";
                ddlCandTimeZone.DataValueField = "TimeZoneID";
                ddlCandTimeZone.DataBind();
                ddlCandTimeZone.CssClass = "LTImodernformdropdown LTIformtextbox-300px";
                tCell = new TableCell();
                //tCell.ColumnSpan = 7;
                tCell.Controls.Add(ddlCandTimeZone);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLTimeZone = new CompareValidator();
                cvDDLTimeZone.Display = ValidatorDisplay.None;
                cvDDLTimeZone.ErrorMessage = controlName + ": Please choose your Time Zone";
                cvDDLTimeZone.ValueToCompare = "-1";
                cvDDLTimeZone.Operator = ValidationCompareOperator.NotEqual;
                cvDDLTimeZone.ControlToValidate = ddlCandTimeZone.ID;
                cvDDLTimeZone.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLTimeZone);

                tstTable.Rows.Add(tRow);
            }

            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblOPILanguage = new Label();
                lblOPILanguage.Text = testname + " Language:  ";
                lblOPILanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblOPILanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlOPILanguage = new DropDownList();
                ddlOPILanguage.ID = controlId + "_ddlOPILanguage_" + index;
                if (Session["testLanguagesList_1"] != null)
                    ddlOPILanguage.DataSource = Session["testLanguagesList_1"];
                else
                    ddlOPILanguage.DataSource = Session["testLanguagesList"];
                ddlOPILanguage.DataTextField = "name";
                ddlOPILanguage.DataValueField = "languageid";
                ddlOPILanguage.DataBind();
                ddlOPILanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlOPILanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLOPILanguage = new CompareValidator();
                cvDDLOPILanguage.Display = ValidatorDisplay.None;
                cvDDLOPILanguage.ErrorMessage = testname + " - Language is required";
                cvDDLOPILanguage.ValueToCompare = "-1";
                cvDDLOPILanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLOPILanguage.ControlToValidate = ddlOPILanguage.ID;
                cvDDLOPILanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLOPILanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfOPITestName = new HiddenField();
            hfOPITestName.ID = controlId + "_hfOPITestName_" + index;
            hfOPITestName.Value = testname;
            pnlTestFields.Controls.Add(hfOPITestName);
        }

        //create OPIc specific components
        private void CreateOPIcFields(string testname, int index, int RatingGrid, bool IsSelectionbyClient, int customExpiryDt, int expiryNoofDays)
        {
            TableRow tRow;
            TableCell tCell;

            tRow = new TableRow();
            int Controls_On_Row = 0;
            if (IsSelectionbyClient)
            {
                pnlTestFields.Visible = true;
                //filler
                tCell = new TableCell();
                tCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                tRow.Cells.Add(tCell);

                //levels 
                Label lblOPIcLevels = new Label();
                lblOPIcLevels.Text = "Select Test Level:  ";
                lblOPIcLevels.CssClass = "LTIformlabel";
                tCell = new TableCell();
                //tCell.HorizontalAlign = HorizontalAlign.Right;
                tCell.Controls.Add(lblOPIcLevels);
                tRow.Cells.Add(tCell);

                DropDownList ddlOPIcLevels = new DropDownList();
                ddlOPIcLevels.ID = controlId + "_ddlOPIcLevels_" + index;
                ddlOPIcLevels.DataSource = Session["testLevelsOPIcList"];
                ddlOPIcLevels.DataTextField = "SubFormName";
                ddlOPIcLevels.DataValueField = "SubformId";
                ddlOPIcLevels.DataBind();
                ddlOPIcLevels.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

                if (ddlOPIcLevels.Items.Count > 1)
                {
                    ddlOPIcLevels.Items.Insert(0, new ListItem("Select One", "-1"));
                    ddlOPIcLevels.SelectedValue = "-1";
                }
                tCell = new TableCell();
                tCell.Controls.Add(ddlOPIcLevels);
                tRow.Cells.Add(tCell);

                ddlOpicLevelsName = controlId + "_ddlOPIcLevels_" + index;

                CompareValidator cvText1 = new CompareValidator();
                cvText1.Display = ValidatorDisplay.None;
                cvText1.ErrorMessage = controlName + ": Please choose a Test level";
                cvText1.ValueToCompare = "-1";
                cvText1.Operator = ValidationCompareOperator.NotEqual;
                cvText1.ControlToValidate = ddlOPIcLevels.ID;
                cvText1.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvText1);
                Controls_On_Row = 1;
            }
            else
            {                
                ddlOpicLevelsName = "";
            }
                                    
            //store for future use
            HiddenField hfCustomExpiryDt = new HiddenField();
            hfCustomExpiryDt.ID = controlId + "_hfCustomExpiryDt_" + index;
            hfCustomExpiryDt.Value = customExpiryDt.ToString();
            pnlTestFields.Controls.Add(hfCustomExpiryDt);

            if (tRow.Cells.Count > 0 && Controls_On_Row > 0)  //Added check on 07/24/2026
            {
                tstTable.Rows.Add(tRow);
            }
                                    
            //if bundle, show language for the test
            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblOPIcLanguage = new Label();
                lblOPIcLanguage.Text = testname + " Language:  ";
                lblOPIcLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblOPIcLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlOPIcLanguage = new DropDownList();
                ddlOPIcLanguage.ID = controlId + "_ddlOPIcLanguage_" + index;
                if (Session["testLanguagesList_2"] != null)
                    ddlOPIcLanguage.DataSource = Session["testLanguagesList_2"];
                else
                    ddlOPIcLanguage.DataSource = Session["testLanguagesList"];
                ddlOPIcLanguage.DataTextField = "name";
                ddlOPIcLanguage.DataValueField = "languageid";
                ddlOPIcLanguage.DataBind();
                ddlOPIcLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

                try
                {
                    if (Session["4Skills"].ToString() == "1")
                    {
                        ddlOPIcLanguage.SelectedIndex = ddlOPIcLanguage.Items.IndexOf(ddlOPIcLanguage.Items.FindByValue("1"));
                    }
                }
                catch
                {
                }

                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlOPIcLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLOPIcLanguage = new CompareValidator();
                cvDDLOPIcLanguage.Display = ValidatorDisplay.None;
                cvDDLOPIcLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLOPIcLanguage.ValueToCompare = "-1";
                cvDDLOPIcLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLOPIcLanguage.ControlToValidate = ddlOPIcLanguage.ID;
                cvDDLOPIcLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLOPIcLanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfOPIcTestName = new HiddenField();
            hfOPIcTestName.ID = controlId + "_hfOPIcTestName_" + index;
            hfOPIcTestName.Value = testname;
            pnlTestFields.Controls.Add(hfOPIcTestName);
        }

        //create WPT specific components
        private void CreateWPTFields(string testname, int index, int RatingGrid, int TestDelivery, int customExpiryDt, int expiryNoofDays)
        {
            bool isWPTBookletServiceEnabled = true;            

            TableRow tRow;
            TableCell tCell;

            tRow = new TableRow();
            int Controls_On_Row = 0;
            
            if (TestDelivery == 3)  //both online and booklet
            {
                pnlTestFields.Visible = true;
                Label lblWPTModality = new Label();
                lblWPTModality.Text = "Modality: ";
                lblWPTModality.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblWPTModality);
                tRow.Cells.Add(tCell);

                DropDownList ddlWPTModality = new DropDownList();
                ddlWPTModality.ID = controlId + "_ddlWPTModality_" + index;
                ddlWPTModality.Items.Add(new ListItem("Booklet", "1"));
                ddlWPTModality.Items.Add(new ListItem("Internet", "2"));
                ddlWPTModality.SelectedValue = "2";
                ddlWPTModality.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

                tCell = new TableCell();
                tCell.Controls.Add(ddlWPTModality);
                tRow.Cells.Add(tCell);

                //filler
                tCell = new TableCell();
                tCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                tRow.Cells.Add(tCell);

                if (isWPTBookletServiceEnabled)
                {
                    Label lblWPTDelivery = new Label();
                    lblWPTDelivery.ID = controlId + "_lblWPTDelivery";
                    lblWPTDelivery.Text = "WPT Delivery: ";
                    lblWPTDelivery.CssClass = "LTIformlabel";
                    tCell = new TableCell();
                    tCell.Controls.Add(lblWPTDelivery);
                    tRow.Cells.Add(tCell);

                    DropDownList ddlWPTDelivery = new DropDownList();
                    ddlWPTDelivery.ID = controlId + "_ddlWPTDelivery";                    
                    ddlWPTDelivery.Items.Add(new ListItem("Email ", "1"));
                    ddlWPTDelivery.SelectedValue = "1";                                        
                    ddlWPTDelivery.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                    tCell = new TableCell();
                    tCell.Controls.Add(ddlWPTDelivery);
                    tRow.Cells.Add(tCell);

                    //validator will be added dynamically if Booklet option is chosen for Modality
                    lblWPTDelivery.Attributes.Add("style", "visibility: hidden");
                    ddlWPTDelivery.Attributes.Add("style", "visibility: hidden");                                        
                }
                Controls_On_Row = 1;
            }
            else
            {
                if (TestDelivery == 1)  //booklet
                {                    
                    if (isWPTBookletServiceEnabled)
                    {
                        pnlTestFields.Visible = true;
                        Label lblWPTDelivery = new Label();
                        lblWPTDelivery.Text = "WPT Delivery: ";
                        lblWPTDelivery.CssClass = "LTIformlabel";
                        tCell = new TableCell();
                        tCell.Controls.Add(lblWPTDelivery);
                        tRow.Cells.Add(tCell);

                        DropDownList ddlWPTDelivery = new DropDownList();
                        ddlWPTDelivery.ID = controlId + "_ddlWPTDelivery";                        
                        ddlWPTDelivery.Items.Add(new ListItem("Email ", "1"));                        
                        ddlWPTDelivery.SelectedValue = "1";
                        ddlWPTDelivery.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                        tCell = new TableCell();
                        tCell.Controls.Add(ddlWPTDelivery);
                        tRow.Cells.Add(tCell);                        
                    }
                    Controls_On_Row = 1;
                }                
            }
                        
            //store for future use
            HiddenField hfCustomExpiryDt = new HiddenField();
            hfCustomExpiryDt.ID = controlId + "_hfCustomExpiryDt_" + index;
            hfCustomExpiryDt.Value = customExpiryDt.ToString();
            pnlTestFields.Controls.Add(hfCustomExpiryDt);

            if (tRow.Cells.Count > 0 && Controls_On_Row > 0)
            {
                tstTable.Rows.Add(tRow);
            }
            
            //if bundle, show language for the test
            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblWPTLanguage = new Label();
                lblWPTLanguage.Text = testname + " Language:  ";
                lblWPTLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblWPTLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlWPTLanguage = new DropDownList();
                ddlWPTLanguage.ID = controlId + "_ddlWPTLanguage_" + index;
                if (Session["testLanguagesList_4"] != null)
                    ddlWPTLanguage.DataSource = Session["testLanguagesList_4"];
                else
                    ddlWPTLanguage.DataSource = Session["testLanguagesList"];
                ddlWPTLanguage.DataTextField = "name";
                ddlWPTLanguage.DataValueField = "languageid";
                ddlWPTLanguage.DataBind();
                ddlWPTLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

                try
                {
                    if (Session["4Skills"].ToString() == "1")
                    {
                        ddlWPTLanguage.SelectedIndex = ddlWPTLanguage.Items.IndexOf(ddlWPTLanguage.Items.FindByValue("1"));
                    }
                }
                catch
                {
                }

                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlWPTLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLWPTLanguage = new CompareValidator();
                cvDDLWPTLanguage.Display = ValidatorDisplay.None;
                cvDDLWPTLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLWPTLanguage.ValueToCompare = "-1";
                cvDDLWPTLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLWPTLanguage.ControlToValidate = ddlWPTLanguage.ID;
                cvDDLWPTLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLWPTLanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfWPTTestName = new HiddenField();
            hfWPTTestName.ID = controlId + "_hfWPTTestName_" + index;
            hfWPTTestName.Value = testname;
            pnlTestFields.Controls.Add(hfWPTTestName);
        }

        //create RPT specific components
        private void CreateRPTFields(string testname, int index, int TestLevel, int SubFormId, bool IsRandom, int customExpiryDt, int expiryNoofDays)
        {
            TableRow tRow;
            TableCell tCell;
            tRow = new TableRow();

            pnlTestFields.Visible = true;
            //RPT levels 
            Label lblRPTLevels = new Label();
            lblRPTLevels.Text = "Reading Level:  ";
            lblRPTLevels.CssClass = "LTIformlabel";
            tCell = new TableCell();
            //tCell.HorizontalAlign = HorizontalAlign.Right;
            tCell.Controls.Add(lblRPTLevels);
            tRow.Cells.Add(tCell);

            DropDownList ddlRPTLevels = new DropDownList();
            ddlRPTLevels.ID = controlId + "_ddlRPTLevels_" + index;
            ddlRPTLevels.DataSource = Session["testLevelsRPTList"];
            ddlRPTLevels.DataTextField = "formname";
            ddlRPTLevels.DataValueField = "formid";
            ddlRPTLevels.DataBind();
            ddlRPTLevels.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
            if (IsRandom != true)
            {
                ddlRPTLevels.SelectedValue = SubFormId.ToString();
                ddlRPTLevels.Enabled = false;
            }
            tCell = new TableCell();
            tCell.Controls.Add(ddlRPTLevels);
            tRow.Cells.Add(tCell);

            ddlRPTLevelsName = controlId + "_ddlRPTLevels_" + index;

            CompareValidator cvText1 = new CompareValidator();
            cvText1.Display = ValidatorDisplay.None;
            cvText1.ErrorMessage = controlName + ": " + testname + " - " + "Please choose a level";
            cvText1.ValueToCompare = "-1";
            cvText1.Operator = ValidationCompareOperator.NotEqual;
            cvText1.ControlToValidate = ddlRPTLevels.ID;
            cvText1.CssClass = "LTIvalidation-error";
            pnlTestFields.Controls.Add(cvText1);
                        
            //store for future use
            HiddenField hfCustomExpiryDt = new HiddenField();
            hfCustomExpiryDt.ID = controlId + "_hfCustomExpiryDt_" + index;
            hfCustomExpiryDt.Value = customExpiryDt.ToString();
            pnlTestFields.Controls.Add(hfCustomExpiryDt);

            // New dropdown for BYU test for accommodation added on 02/19/2026
            if (TestLevel == 217 || TestLevel == 218 || TestLevel == 221 || TestLevel == 222)
            {
                tstTable.Rows.Add(tRow);
                tRow = new TableRow();
                Label lblRPTAccommodation = new Label();
                lblRPTAccommodation.Text = "Accommodation Type: ";
                lblRPTAccommodation.CssClass = "LTIformlabel";
                tCell = new TableCell();
                //tCell.HorizontalAlign = HorizontalAlign.Right;
                tCell.Controls.Add(lblRPTAccommodation);
                tRow.Cells.Add(tCell);
                
                DataTable dtAccommodations = myDBClass.GetBYUAccommodations();

                DropDownList ddlRPTAccommodation = new DropDownList();
                ddlRPTAccommodation.ID = controlId + "_ddlRPTAccommodation_" + index;
                ddlRPTAccommodation.DataSource = dtAccommodations;
                ddlRPTAccommodation.DataTextField = "AccommodationType";
                ddlRPTAccommodation.DataValueField = "RowId";
                ddlRPTAccommodation.DataBind();
                ddlRPTAccommodation.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlRPTAccommodation);
                tRow.Cells.Add(tCell);

                CompareValidator cvAccommodation = new CompareValidator();
                cvAccommodation.Display = ValidatorDisplay.None;
                cvAccommodation.ErrorMessage = controlName + ": " + testname + " - " + "Please choose an Accommodation";
                cvAccommodation.ValueToCompare = "-1";
                cvAccommodation.Operator = ValidationCompareOperator.NotEqual;
                cvAccommodation.ControlToValidate = ddlRPTAccommodation.ID;
                cvAccommodation.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvAccommodation);
            }
            // End of accommodation

            tstTable.Rows.Add(tRow);

            //if bundle, show language for the test
            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblRPTLanguage = new Label();
                lblRPTLanguage.Text = testname + " Language:  ";
                lblRPTLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblRPTLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlRPTLanguage = new DropDownList();
                ddlRPTLanguage.ID = controlId + "_ddlRPTLanguage_" + index;
                if (Session["testLanguagesList_3"] != null)
                    ddlRPTLanguage.DataSource = Session["testLanguagesList_3"];
                else
                    ddlRPTLanguage.DataSource = Session["testLanguagesList"];
                ddlRPTLanguage.DataTextField = "name";
                ddlRPTLanguage.DataValueField = "languageid";
                ddlRPTLanguage.DataBind();
                ddlRPTLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlRPTLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLRPTLanguage = new CompareValidator();
                cvDDLRPTLanguage.Display = ValidatorDisplay.None;
                cvDDLRPTLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLRPTLanguage.ValueToCompare = "-1";
                cvDDLRPTLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLRPTLanguage.ControlToValidate = ddlRPTLanguage.ID;
                cvDDLRPTLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLRPTLanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfRPTTestName = new HiddenField();
            hfRPTTestName.ID = controlId + "_hfRPTTestName_" + index;
            hfRPTTestName.Value = testname;
            pnlTestFields.Controls.Add(hfRPTTestName);
        }

        private void CreateLPTFields(string testname, int index, int TestLevel, int SubFormId, bool IsRandom, int customExpiryDt, int expiryNoofDays)
        {
            TableRow tRow;
            TableCell tCell;
            tRow = new TableRow();
            pnlTestFields.Visible = true;
            //LPT levels 
            Label lblLPTLevels = new Label();
            lblLPTLevels.Text = "Listening Level:  ";
            lblLPTLevels.CssClass = "LTIformlabel";
            tCell = new TableCell();
            tCell.Controls.Add(lblLPTLevels);
            //tCell.HorizontalAlign = HorizontalAlign.Right;            
            tRow.Cells.Add(tCell);

            DropDownList ddlLPTLevels = new DropDownList();
            ddlLPTLevels.ID = controlId + "_ddlLPTLevels_" + index;
            ddlLPTLevels.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
            ddlLPTLevels.DataSource = Session["testLevelsLPTList"];
            ddlLPTLevels.DataTextField = "formname";
            ddlLPTLevels.DataValueField = "formid";
            ddlLPTLevels.DataBind();
            if (IsRandom != true)
            {
                ddlLPTLevels.SelectedValue = SubFormId.ToString();
                ddlLPTLevels.Enabled = false;
            }
            tCell = new TableCell();
            tCell.Controls.Add(ddlLPTLevels);
            tRow.Cells.Add(tCell);

            ddlLPTLevelsName = controlId + "_ddlLPTLevels_" + index;

            CompareValidator cvText1 = new CompareValidator();
            cvText1.Display = ValidatorDisplay.None;
            cvText1.ErrorMessage = controlName + ": " + testname + " - " + "Please choose a level";
            cvText1.ValueToCompare = "-1";
            cvText1.Operator = ValidationCompareOperator.NotEqual;
            cvText1.ControlToValidate = ddlLPTLevels.ID;
            cvText1.CssClass = "LTIvalidation-error";
            pnlTestFields.Controls.Add(cvText1);
            
            //store for future use
            HiddenField hfCustomExpiryDt = new HiddenField();
            hfCustomExpiryDt.ID = controlId + "_hfCustomExpiryDt_" + index;
            hfCustomExpiryDt.Value = customExpiryDt.ToString();
            pnlTestFields.Controls.Add(hfCustomExpiryDt);

            // New dropdown for BYU test for accommodation added on 02/19/2026
            if (TestLevel == 217 || TestLevel == 218 || TestLevel == 221 || TestLevel == 222)
            {
                tstTable.Rows.Add(tRow);
                tRow = new TableRow();
                Label lblLPTAccommodation = new Label();
                lblLPTAccommodation.Text = "Accommodation Type: ";
                lblLPTAccommodation.CssClass = "LTIformlabel";
                tCell = new TableCell();
                //tCell.HorizontalAlign = HorizontalAlign.Right;
                tCell.Controls.Add(lblLPTAccommodation);
                tRow.Cells.Add(tCell);
                
                DataTable dtAccommodations = myDBClass.GetBYUAccommodations();

                DropDownList ddlLPTAccommodation = new DropDownList();
                ddlLPTAccommodation.ID = controlId + "_ddlLPTAccommodation_" + index;
                ddlLPTAccommodation.DataSource = dtAccommodations;
                ddlLPTAccommodation.DataTextField = "AccommodationType";
                ddlLPTAccommodation.DataValueField = "RowId";
                ddlLPTAccommodation.DataBind();
                ddlLPTAccommodation.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlLPTAccommodation);
                tRow.Cells.Add(tCell);

                CompareValidator cvAccommodation = new CompareValidator();
                cvAccommodation.Display = ValidatorDisplay.None;
                cvAccommodation.ErrorMessage = controlName + ": " + testname + " - " + "Please choose an Accommodation";
                cvAccommodation.ValueToCompare = "-1";
                cvAccommodation.Operator = ValidationCompareOperator.NotEqual;
                cvAccommodation.ControlToValidate = ddlLPTAccommodation.ID;
                cvAccommodation.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvAccommodation);
            }
            // End of accommodation

            tstTable.Rows.Add(tRow);

            //if bundle, show language for the test
            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblLPTLanguage = new Label();
                lblLPTLanguage.Text = testname + " Language:  ";
                lblLPTLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblLPTLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlLPTLanguage = new DropDownList();
                ddlLPTLanguage.ID = controlId + "_ddlLPTLanguage_" + index;
                if (Session["testLanguagesList_5"] != null)
                    ddlLPTLanguage.DataSource = Session["testLanguagesList_5"];
                else
                    ddlLPTLanguage.DataSource = Session["testLanguagesList"];
                ddlLPTLanguage.DataTextField = "name";
                ddlLPTLanguage.DataValueField = "languageid";
                ddlLPTLanguage.DataBind();
                ddlLPTLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlLPTLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLLPTLanguage = new CompareValidator();
                cvDDLLPTLanguage.Display = ValidatorDisplay.None;
                cvDDLLPTLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLLPTLanguage.ValueToCompare = "-1";
                cvDDLLPTLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLLPTLanguage.ControlToValidate = ddlLPTLanguage.ID;
                cvDDLLPTLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLLPTLanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfLPTTestName = new HiddenField();
            hfLPTTestName.ID = controlId + "_hfLPTTestName_" + index;
            hfLPTTestName.Value = testname;
            pnlTestFields.Controls.Add(hfLPTTestName);
        }

        //create Generic Test 
        private void CreateGenericTestFields(string testname, int index)
        {
            TableRow tRow;
            TableCell tCell;

            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblGenericLanguage = new Label();
                lblGenericLanguage.Text = testname + " Language:  ";
                lblGenericLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblGenericLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlGenericLanguage = new DropDownList();
                ddlGenericLanguage.ID = controlId + "_ddlGenericLanguage_" + index;
                ddlGenericLanguage.DataSource = Session["testLanguagesList"];
                ddlGenericLanguage.DataTextField = "name";
                ddlGenericLanguage.DataValueField = "languageid";
                ddlGenericLanguage.DataBind();
                ddlGenericLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";
                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlGenericLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLGenericLanguage = new CompareValidator();
                cvDDLGenericLanguage.Display = ValidatorDisplay.None;
                cvDDLGenericLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLGenericLanguage.ValueToCompare = "-1";
                cvDDLGenericLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLGenericLanguage.ControlToValidate = ddlGenericLanguage.ID;
                cvDDLGenericLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLGenericLanguage);

                tstTable.Rows.Add(tRow);
            }
            HiddenField hfGenericTestName = new HiddenField();
            hfGenericTestName.ID = controlId + "_hfGenericTestName_" + index;
            hfGenericTestName.Value = testname;
            pnlTestFields.Controls.Add(hfGenericTestName);
        }

        private void CreateLAndRFields(string testname, int index, int TestLevel, int SubFormId, bool IsRandom, int customExpiryDt, int expiryNoofDays)
        {
            TableRow tRow;
            TableCell tCell;
            tRow = new TableRow();
            pnlTestFields.Visible = true;
            //L & R Test Instruction languages
            Label lblLRInstructionLang = new Label();
            lblLRInstructionLang.Text = "Instruction Language:  ";
            lblLRInstructionLang.CssClass = "LTIformlabel";
            tCell = new TableCell();
            tCell.Controls.Add(lblLRInstructionLang);
            ///tCell.HorizontalAlign = HorizontalAlign.Right;
            tRow.Cells.Add(tCell);

            DropDownList ddlLRInstructionLang = new DropDownList();
            ddlLRInstructionLang.ID = controlId + "_ddlLRInstructionLang_" + index;
            ddlLRInstructionLang.DataSource = Session["testInstrLanguagesList"];
            ddlLRInstructionLang.DataTextField = "name";
            ddlLRInstructionLang.DataValueField = "languageid";
            ddlLRInstructionLang.DataBind();
            ddlLRInstructionLang.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

            try
            {
                if (Session["4Skills"].ToString() == "1")
                {
                    ddlLRInstructionLang.SelectedIndex = ddlLRInstructionLang.Items.IndexOf(ddlLRInstructionLang.Items.FindByValue("1"));
                }
            }
            catch
            {
            }

            tCell = new TableCell();
            tCell.Controls.Add(ddlLRInstructionLang);
            tRow.Cells.Add(tCell);

            ddlLNRInstructionLangName = controlId + "_ddlLRInstructionLang_" + index;

            CompareValidator cvLRText1 = new CompareValidator();
            cvLRText1.Display = ValidatorDisplay.None;
            cvLRText1.ErrorMessage = controlName + ": " + testname + " - " + "Please choose an instruction language";
            cvLRText1.ValueToCompare = "-1";
            cvLRText1.Operator = ValidationCompareOperator.NotEqual;
            cvLRText1.ControlToValidate = ddlLRInstructionLang.ID;
            cvLRText1.CssClass = "LTIvalidation-error";
            pnlTestFields.Controls.Add(cvLRText1);
           
            //store for future use
            HiddenField hfCustomExpiryDt = new HiddenField();
            hfCustomExpiryDt.ID = controlId + "_hfCustomExpiryDt_" + index;
            hfCustomExpiryDt.Value = customExpiryDt.ToString();
            pnlTestFields.Controls.Add(hfCustomExpiryDt);

            tstTable.Rows.Add(tRow);

            if (IsBundle)
            {
                tRow = new TableRow();

                Label lblLNRLanguage = new Label();
                lblLNRLanguage.Text = testname + " Language:  ";
                lblLNRLanguage.CssClass = "LTIformlabel";
                tCell = new TableCell();
                tCell.Controls.Add(lblLNRLanguage);
                tRow.Cells.Add(tCell);

                DropDownList ddlLNRLanguage = new DropDownList();
                ddlLNRLanguage.ID = controlId + "_ddlLNRLanguage_" + index;
                if (Session["testLanguagesList_9"] != null)
                    ddlLNRLanguage.DataSource = Session["testLanguagesList_9"];
                else
                    ddlLNRLanguage.DataSource = Session["testLanguagesList"];
                ddlLNRLanguage.DataTextField = "name";
                ddlLNRLanguage.DataValueField = "languageid";
                ddlLNRLanguage.DataBind();
                ddlLNRLanguage.CssClass = "LTImodernformdropdown LTIformtextbox-175px";

                try
                {
                    if (Session["4Skills"].ToString() == "1")
                    {
                        ddlLNRLanguage.SelectedIndex = ddlLNRLanguage.Items.IndexOf(ddlLNRLanguage.Items.FindByValue("1"));
                    }
                }
                catch
                {
                }

                tCell = new TableCell();
                tCell.ColumnSpan = 10;
                tCell.Controls.Add(ddlLNRLanguage);
                tRow.Cells.Add(tCell);

                CompareValidator cvDDLLNRLanguage = new CompareValidator();
                cvDDLLNRLanguage.Display = ValidatorDisplay.None;
                cvDDLLNRLanguage.ErrorMessage = testname + " - Language is required";
                cvDDLLNRLanguage.ValueToCompare = "-1";
                cvDDLLNRLanguage.Operator = ValidationCompareOperator.NotEqual;
                cvDDLLNRLanguage.ControlToValidate = ddlLNRLanguage.ID;
                cvDDLLNRLanguage.CssClass = "LTIvalidation-error";
                pnlTestFields.Controls.Add(cvDDLLNRLanguage);

                tstTable.Rows.Add(tRow);
            }

            HiddenField hfLRTestName = new HiddenField();
            hfLRTestName.ID = controlId + "_hfLRTestName_" + index;
            hfLRTestName.Value = testname;
            pnlTestFields.Controls.Add(hfLRTestName);
        }

        //Get client custom control values
        private void GetClientCustomFieldValues(TMS_Lib.BAL.ClientOrder cOrder, usp_client_getClientCustomFieldsResult cfObj)
        {
            if (cfObj.F1_Title != null)
                cOrder.cf1Value = GetClientCustomFieldValue(ctrlCFClientArea1, (int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Encrypted, 1);
            if (cfObj.F2_Title != null)
                cOrder.cf2Value = GetClientCustomFieldValue(ctrlCFClientArea2, (int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Encrypted, 2);
            if (cfObj.F3_Title != null)
                cOrder.cf3Value = GetClientCustomFieldValue(ctrlCFClientArea3, (int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Encrypted, 3);
            if (cfObj.F4_Title != null)
                cOrder.cf4Value = GetClientCustomFieldValue(ctrlCFClientArea4, (int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Encrypted, 4);
            if (cfObj.F5_Title != null)
                cOrder.cf5Value = GetClientCustomFieldValue(ctrlCFClientArea5, (int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Encrypted, 5);
            if (cfObj.F6_Title != null)
                cOrder.cf6Value = GetClientCustomFieldValue(ctrlCFClientArea6, (int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Encrypted, 6);
            if (cfObj.F7_Title != null)
                cOrder.cf7Value = GetClientCustomFieldValue(ctrlCFClientArea7, (int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Encrypted, 7);
            if (cfObj.F8_Title != null)
                cOrder.cf8Value = GetClientCustomFieldValue(ctrlCFClientArea8, (int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Encrypted, 8);
            if (cfObj.F9_Title != null)
                cOrder.cf9Value = GetClientCustomFieldValue(ctrlCFClientArea9, (int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Encrypted, 9);
            if (cfObj.F10_Title != null)
                cOrder.cf10Value = GetClientCustomFieldValue(ctrlCFClientArea10, (int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Encrypted, 10);
        }

        //Get specific client custom control value
        private string GetClientCustomFieldValue(Panel controlArea, int controltype, bool enabled, bool encrypted, int index)
        {
            string value = "";            
            if (enabled)
            {
                if ((controltype == 1) || (controltype == 3))
                {
                    TextBox tf = (TextBox)controlArea.FindControl("control" + index);
                    value = tf.Text;
                }
                else if (controltype == 2)   //drop-down list
                {
                    DropDownList ddl = (DropDownList)controlArea.FindControl("control" + index);
                    value = ddl.SelectedItem.Value;
                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = (ListBox)controlArea.FindControl("control" + index);
                    foreach (ListItem li in ddl.Items)
                    {
                        if (li.Selected == true)
                            value = value + ";" + li.Value;
                    }
                }                
                return value;
            }
            else
            {
                return "";
            }
        }

        public TMS_Lib.BAL.BundleClientCandidateOrder GetBundleOrder()
        {
            try
            {
                TMS_Lib.BAL.BundleClientCandidateOrder bccOrder = new TMS_Lib.BAL.BundleClientCandidateOrder();

                //get static items
                if (!IsBundle)
                    bccOrder.langid = Convert.ToInt32(ddlLanguage.SelectedItem.Value);
                else
                    bccOrder.langid = -1;

                bccOrder.candidateid = "";
                bccOrder.cand_firstname = Session["User_FirstName"].ToString();
                bccOrder.cand_lastname = Session["User_LastName"].ToString();
                bccOrder.candidateemail = Session["User_EmailId"].ToString();
                bccOrder.isSlot = false;
                bccOrder.numberOfTests = 1;
                bccOrder.DOB = "";
            
                //get custom items
                if (Session["candidateCFLst"] != null)
                {
                    usp_client_getCandidateCustomFieldsResult cfObj = (usp_client_getCandidateCustomFieldsResult)Session["candidateCFLst"];
                    if (cfObj.F1_Top.ToString() == "0")
                        bccOrder.cf1Value = GetCandidateCustomFieldValue((int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Encrypted, 1, ctrlCFClientArea1);
                    if (cfObj.F2_Top.ToString() == "0")
                        bccOrder.cf2Value = GetCandidateCustomFieldValue((int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Encrypted, 2, ctrlCFClientArea2);
                    if (cfObj.F3_Top.ToString() == "0")
                        bccOrder.cf3Value = GetCandidateCustomFieldValue((int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Encrypted, 3, ctrlCFClientArea3);
                    if (cfObj.F4_Top.ToString() == "0")
                        bccOrder.cf4Value = GetCandidateCustomFieldValue((int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Encrypted, 4, ctrlCFClientArea4);
                    if (cfObj.F5_Top.ToString() == "0")
                        bccOrder.cf5Value = GetCandidateCustomFieldValue((int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Encrypted, 5, ctrlCFClientArea5);
                    if (cfObj.F6_Top.ToString() == "0")
                        bccOrder.cf6Value = GetCandidateCustomFieldValue((int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Encrypted, 6, ctrlCFClientArea6);
                    if (cfObj.F7_Top.ToString() == "0")
                        bccOrder.cf7Value = GetCandidateCustomFieldValue((int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Encrypted, 7, ctrlCFClientArea7);
                    if (cfObj.F8_Top.ToString() == "0")
                        bccOrder.cf8Value = GetCandidateCustomFieldValue((int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Encrypted, 8, ctrlCFClientArea8);
                    if (cfObj.F9_Top.ToString() == "0")
                        bccOrder.cf9Value = GetCandidateCustomFieldValue((int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Encrypted, 9, ctrlCFClientArea9);
                    if (cfObj.F10_Top.ToString() == "0")
                        bccOrder.cf10Value = GetCandidateCustomFieldValue((int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Encrypted, 10, ctrlCFClientArea10);
                }
                //get each test item
                GetTestItems(bccOrder);
                return bccOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetCandidateCustomFieldValue(int controltype, bool enabled, bool encrypted, int index, Panel controlArea)
        {
            string value = "";

            if (enabled)
            {
                if (controltype == 1)    //textbox
                {
                    TextBox tf = (TextBox)controlArea.FindControl(controlId + "_" + index);
                    value = tf.Text;
                }
                else if (controltype == 2)   //drop-down list
                {
                    DropDownList ddl = (DropDownList)controlArea.FindControl(controlId + "_" + index);
                    value = ddl.SelectedItem.Value;
                }
                else if (controltype == 3)    //date field
                {
                    TextBox tf = (TextBox)controlArea.FindControl(controlId + "_" + index);
                    value = tf.Text;
                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = (ListBox)controlArea.FindControl(controlId + "_" + index);
                    foreach (ListItem li in ddl.Items)
                    {
                        if (li.Selected == true)
                            value = value + ";" + li.Value;
                    }
                }                
                return value;
            }
            else
            {
                return "";
            }

        }

        private void GetTestItems(TMS_Lib.BAL.BundleClientCandidateOrder bccOrder)
        {
            List<usp_client_getTestsInBundleResult> testsInBundleLst = (List<usp_client_getTestsInBundleResult>)Session["bundleTestsList"];
            int producttype;
            int testid;
            int whoMakeCalls;
            int RatingGrid;
            int TestDelivery;
            bool IsSelectionbyClient;
            string strOPIHourMessage = "72";
            bccOrder.btestLst = new TMS_Lib.BAL.BundleTestItem[testsInBundleLst.Count].ToList();
            try
            {
                //loop and get data for each test in the bundle
                for (int i = 0; i < testsInBundleLst.Count; i++)
                {
                    TMS_Lib.BAL.BundleTestItem btItem = new TMS_Lib.BAL.BundleTestItem();
                    producttype = (int)testsInBundleLst[i].producttype;
                    testid = (int)testsInBundleLst[i].clienttestid;
                    whoMakeCalls = (int)testsInBundleLst[i].WhoMakeCalls;
                    RatingGrid = (int)testsInBundleLst[i].RatingGrid;
                    TestDelivery = (int)testsInBundleLst[i].TestDelivery;
                    IsSelectionbyClient = testsInBundleLst[i].IsSelectionbyClient;
                    strOPIHourMessage = testsInBundleLst[i].OPIHourMessage;

                    btItem.producttype = producttype;
                    btItem.testid = testid;
                    btItem.instructionLang = -1;  //default
                    btItem.langid = -1;
                    switch (producttype)
                    {
                        case 1:    //OPI
                            GetOPIFields(btItem, i, whoMakeCalls, RatingGrid, strOPIHourMessage);
                            break;
                        case 2:   //OPIC
                            GetOPIcFields(btItem, i, RatingGrid, IsSelectionbyClient);
                            break;
                        case 3:   //RPT
                            GetRPTFields(btItem, i);
                            break;
                        case 4:   //WPT
                            GetWPTFields(btItem, i, RatingGrid, TestDelivery);
                            break;
                        case 5:   //LPT
                            GetLPTFields(btItem, i);
                            break;
                        case 9:
                            GetLAndRFields(btItem, i);
                            break;
                        default:
                            GetGenericTestFields(btItem, i);
                            break;
                    }

                    if (IsBundle)
                        btItem.isbundle = true;
                    else
                        btItem.isbundle = false;

                    bccOrder.btestLst[i] = btItem;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //OPI
        private void GetOPIFields(TMS_Lib.BAL.BundleTestItem btItem, int index, int whoMakeCalls, int RatingGrid, string strOPIHourMessage)
        {
            try
            {                                
                DateTime Choice1St = Convert.ToDateTime(txtdtMatch.Text + " " + ddlHour.SelectedValue + ":" + ddlMinutes.SelectedValue + " " + ddlAMPM.SelectedValue);
                DateTime Choice1Ed = Convert.ToDateTime(txtdtMatch.Text + " " + ddlToHour.SelectedValue + ":" + ddlToMinutes.SelectedValue + " " + ddlToAMPM.SelectedValue);
                btItem.timerangefrom1 = Choice1St;
                btItem.timerangeto1 = Choice1Ed;
            }
            catch (Exception ex)
            {
                throw new Exception("OPI First Choice times are invalid");
            }

            if (DateTime.Compare(btItem.timerangefrom1, btItem.timerangeto1) > 0)
                throw new Exception("OPI First Choice Start Time must be less than End Time");

            if ((DateTime.Compare(btItem.timerangefrom1, DateTime.Now) < 0) || (DateTime.Compare(btItem.timerangeto1, DateTime.Now) < 0))
                throw new Exception("OPI First Choice Start Time/End Time must be greater than current time");
           
            try
            {
                DateTime Choice2St = Convert.ToDateTime(txtdtMatch2.Text + " " + ddlHour2.SelectedValue + ":" + ddlMinutes2.SelectedValue + " " + ddlAMPM2.SelectedValue);
                DateTime Choice2Ed = Convert.ToDateTime(txtdtMatch2.Text + " " + ddlToHour2.SelectedValue + ":" + ddlToMinutes2.SelectedValue + " " + ddlToAMPM2.SelectedValue);
                btItem.timerangefrom2 = Choice2St;
                btItem.timerangeto2 = Choice2Ed;
            }
            catch (Exception ex)
            {
                throw new Exception("OPI Second Choice times are invalid");
            }

            if (DateTime.Compare(btItem.timerangefrom2, btItem.timerangeto2) > 0)
                throw new Exception("OPI Second Choice Start Time must be less than End Time");

            if ((DateTime.Compare(btItem.timerangefrom2, DateTime.Now) < 0) || (DateTime.Compare(btItem.timerangeto2, DateTime.Now) < 0))
                throw new Exception("OPI Second Choice Start Time/End Time must be greater than current time");
            
            if (DateTime.Compare(btItem.timerangefrom1.Date, btItem.timerangefrom2.Date) >= 0)
                throw new Exception("Your second choice must be after your first choice");

            btItem.retest = false;
            btItem.pretest = -1;
            btItem.whoMakeCall = whoMakeCalls;

            TextBox tfCPhone = (TextBox)pnlTestFields.FindControl(controlId + "_tfCPhone_" + index);
            btItem.candidatePhone = tfCPhone.Text;
            DropDownList ddlCandTimeZone = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlCandTimeZone_" + index);
            btItem.candidateTimeZone = Convert.ToInt32(ddlCandTimeZone.SelectedItem.Value);
                        
            if (RatingGrid > 0)
                btItem.hasOPIComments = 1;  //default
            else
                btItem.hasOPIComments = 0;
            
            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfOPITestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);
            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);
            if (isBundle)
            {
                //retest
                DropDownList ddlOPILanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlOPILanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlOPILanguage.SelectedItem.Value);
            }
        }

        //OPIC
        private void GetOPIcFields(TMS_Lib.BAL.BundleTestItem btItem, int index, int RatingGrid, bool IsSelectionbyClient)
        {
            if (IsSelectionbyClient)
            {
                DropDownList ddlOPIcTestLevel = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlOPIcLevels_" + index);
                btItem.testlevel = Convert.ToInt32(ddlOPIcTestLevel.SelectedItem.Value);
            }

            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");

            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;
            btItem.pretest = -1;
            if (RatingGrid > 0)
                btItem.hasOPIComments = 1;  //default
            else
                btItem.hasOPIComments = 0;
            
            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfOPIcTestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);

            HiddenField hfCustomExpiryDt = (HiddenField)pnlTestFields.FindControl(controlId + "_hfCustomExpiryDt_" + index);
            int customExpiryDt = Convert.ToInt32(hfCustomExpiryDt.Value);
            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);            
            if (isBundle)
            {                
                DropDownList ddlOPIcLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlOPIcLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlOPIcLanguage.SelectedItem.Value);
            }
        }

        //RPT
        private void GetRPTFields(TMS_Lib.BAL.BundleTestItem btItem, int index)
        {
            DropDownList ddlRPTTestLevel = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlRPTLevels_" + index);
            btItem.testlevel = Convert.ToInt32(ddlRPTTestLevel.SelectedItem.Value);

            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");

            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;
            btItem.pretest = -1;

            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfRPTTestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);

            HiddenField hfCustomExpiryDt = (HiddenField)pnlTestFields.FindControl(controlId + "_hfCustomExpiryDt_" + index);
            int customExpiryDt = Convert.ToInt32(hfCustomExpiryDt.Value);

            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);            
            if (isBundle)
            {                
                DropDownList ddlRPTLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlRPTLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlRPTLanguage.SelectedItem.Value);
            }

            // For BYU Accommodation added on 02/19/2026 -- Below numbers are sub Form Id
            if (btItem.testlevel == 566 || btItem.testlevel == 567 || btItem.testlevel == 595 || btItem.testlevel == 598 || btItem.testlevel == 596
                || btItem.testlevel == 597 || btItem.testlevel == 599 || btItem.testlevel == 600 || btItem.testlevel == 581 || btItem.testlevel == 582)
            {
                DropDownList ddlRPTAccommodation = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlRPTAccommodation_" + index);
                try
                {
                    btItem.wpt_modality = ddlRPTAccommodation.SelectedValue;
                }
                catch
                {
                    btItem.wpt_modality = "14"; // Default set for Standard in case of error
                }
            }
        }

        //WPT
        private void GetWPTFields(TMS_Lib.BAL.BundleTestItem btItem, int index, int RatingGrid, int TestDelivery)
        {
            bool isWPTBookletServiceEnabled = false;
            isWPTBookletServiceEnabled = true;            
            btItem.wpt_delivery = 1;

            if (TestDelivery == 3)
            {
                DropDownList ddlWPTModality = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlWPTModality_" + index);
                btItem.wpt_modality = ddlWPTModality.SelectedItem.Value;

                if (ddlWPTModality.SelectedItem.Value == "1")  // if booklet
                {
                    if (isWPTBookletServiceEnabled)
                    {
                        if (pnlTestFields.FindControl(controlId + "_ddlWPTDelivery") != null)
                            btItem.wpt_delivery = Convert.ToInt32(((DropDownList)pnlTestFields.FindControl(controlId + "_ddlWPTDelivery")).SelectedItem.Value);
                        else
                            btItem.wpt_delivery = 1;   //default is Email
                    }
                }
            }
            else
            {
                if (TestDelivery == 1)  // if booklet
                {
                    if (isWPTBookletServiceEnabled)
                    {
                        if (pnlTestFields.FindControl(controlId + "_ddlWPTDelivery") != null)
                            btItem.wpt_delivery = Convert.ToInt32(((DropDownList)pnlTestFields.FindControl(controlId + "_ddlWPTDelivery")).SelectedItem.Value);
                        else
                            btItem.wpt_delivery = 1;    //default is Email
                    }
                }
                btItem.wpt_modality = "";
            }
            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");

            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;
            btItem.pretest = -1;

            if (RatingGrid > 0)
                btItem.hasOPIComments = 1;  //default
            else
                btItem.hasOPIComments = 0;
            
            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfWPTTestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);            
            HiddenField hfCustomExpiryDt = (HiddenField)pnlTestFields.FindControl(controlId + "_hfCustomExpiryDt_" + index);
            int customExpiryDt = Convert.ToInt32(hfCustomExpiryDt.Value);
            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);            
            if (isBundle)
            {
                //retest
                DropDownList ddlWPTLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlWPTLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlWPTLanguage.SelectedItem.Value);
            }
        }

        //LPT
        private void GetLPTFields(TMS_Lib.BAL.BundleTestItem btItem, int index)
        {
            DropDownList ddlLPTTestLevel = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlLPTLevels_" + index);
            btItem.testlevel = Convert.ToInt32(ddlLPTTestLevel.SelectedItem.Value);

            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");

            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;
            btItem.pretest = -1;

            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfLPTTestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);
            HiddenField hfCustomExpiryDt = (HiddenField)pnlTestFields.FindControl(controlId + "_hfCustomExpiryDt_" + index);
            int customExpiryDt = Convert.ToInt32(hfCustomExpiryDt.Value);

            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);
            
            if (isBundle)
            {            
                DropDownList ddlLPTLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlLPTLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlLPTLanguage.SelectedItem.Value);
            }

            // For BYU Accommodation added on 02/19/2026 -- Below numbers are sub Form Id
            if (btItem.testlevel == 566 || btItem.testlevel == 567 || btItem.testlevel == 595 || btItem.testlevel == 598 || btItem.testlevel == 596 || btItem.testlevel == 597
                || btItem.testlevel == 599 || btItem.testlevel == 600 || btItem.testlevel == 581 || btItem.testlevel == 582)
            {
                DropDownList ddlLPTAccommodation = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlLPTAccommodation_" + index);
                try
                {
                    btItem.wpt_modality = ddlLPTAccommodation.SelectedValue;
                }
                catch
                {
                    btItem.wpt_modality = "14"; // Default set for Standard in case of error
                }
            }            
        }


        private void GetGenericTestFields(TMS_Lib.BAL.BundleTestItem btItem, int index)
        {
            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;

            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfGenericTestName_" + index);
            btItem.testname = hf.Value;

            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);
            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);
            if (isBundle)
            {
                //retest
                DropDownList ddlGenericLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlGenericLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlGenericLanguage.SelectedItem.Value);
            }
        }

        //LPT
        private void GetLAndRFields(TMS_Lib.BAL.BundleTestItem btItem, int index)
        {
            DropDownList ddlLRInstructionLang = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlLRInstructionLang_" + index);
            btItem.instructionLang = Convert.ToInt32(ddlLRInstructionLang.SelectedItem.Value);

            //default
            btItem.timerangefrom1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2 = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto1_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangefrom2_E = Convert.ToDateTime("1/1/1900");
            btItem.timerangeto2_E = Convert.ToDateTime("1/1/1900");
            btItem.retest = false;
            btItem.pretest = -1;

            HiddenField hf = (HiddenField)pnlTestFields.FindControl(controlId + "_hfLRTestName_" + index);
            btItem.testname = hf.Value;
            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultExpirationDays"]);
            HiddenField hfCustomExpiryDt = (HiddenField)pnlTestFields.FindControl(controlId + "_hfCustomExpiryDt_" + index);
            int customExpiryDt = Convert.ToInt32(hfCustomExpiryDt.Value);
            btItem.expirationdate = System.DateTime.Today.AddDays(expirationDays);
            
            if (isBundle)
            {
                //retest
                DropDownList ddlLNRLanguage = (DropDownList)pnlTestFields.FindControl(controlId + "_ddlLNRLanguage_" + index);
                btItem.langid = Convert.ToInt32(ddlLNRLanguage.SelectedItem.Value);
            }
        }

        private void GetCandTopCustomFieldValues(TMS_Lib.BAL.BundleClientCandidateOrder bccOrder, usp_client_getCandidateCustomFieldsResult cfObj)
        {
            if (cfObj != null)
            {
                if (cfObj.F1_Title != null)
                {
                    if (cfObj.F1_Top.ToString() == "1")
                        bccOrder.cf1Value = GetCanTopCustomFieldValue(ctrlCFCanArea1, (int)cfObj.F1_Type, (bool)cfObj.F1_Enabled, (bool)cfObj.F1_Encrypted, 1);
                }
                if (cfObj.F2_Title != null)
                {
                    if (cfObj.F2_Top.ToString() == "1")
                        bccOrder.cf2Value = GetCanTopCustomFieldValue(ctrlCFCanArea2, (int)cfObj.F2_Type, (bool)cfObj.F2_Enabled, (bool)cfObj.F2_Encrypted, 2);
                }
                if (cfObj.F3_Title != null)
                {
                    if (cfObj.F3_Top.ToString() == "1")
                        bccOrder.cf3Value = GetCanTopCustomFieldValue(ctrlCFCanArea3, (int)cfObj.F3_Type, (bool)cfObj.F3_Enabled, (bool)cfObj.F3_Encrypted, 3);
                }
                if (cfObj.F4_Title != null)
                {
                    if (cfObj.F4_Top.ToString() == "1")
                        bccOrder.cf4Value = GetCanTopCustomFieldValue(ctrlCFCanArea4, (int)cfObj.F4_Type, (bool)cfObj.F4_Enabled, (bool)cfObj.F4_Encrypted, 4);
                }
                if (cfObj.F5_Title != null)
                {
                    if (cfObj.F5_Top.ToString() == "1")
                        bccOrder.cf5Value = GetCanTopCustomFieldValue(ctrlCFCanArea5, (int)cfObj.F5_Type, (bool)cfObj.F5_Enabled, (bool)cfObj.F5_Encrypted, 5);
                }
                if (cfObj.F6_Title != null)
                {
                    if (cfObj.F6_Top.ToString() == "1")
                        bccOrder.cf6Value = GetCanTopCustomFieldValue(ctrlCFCanArea6, (int)cfObj.F6_Type, (bool)cfObj.F6_Enabled, (bool)cfObj.F6_Encrypted, 6);
                }
                if (cfObj.F7_Title != null)
                {
                    if (cfObj.F7_Top.ToString() == "1")
                        bccOrder.cf7Value = GetCanTopCustomFieldValue(ctrlCFCanArea7, (int)cfObj.F7_Type, (bool)cfObj.F7_Enabled, (bool)cfObj.F7_Encrypted, 7);
                }
                if (cfObj.F8_Title != null)
                {
                    if (cfObj.F8_Top.ToString() == "1")
                        bccOrder.cf8Value = GetCanTopCustomFieldValue(ctrlCFCanArea8, (int)cfObj.F8_Type, (bool)cfObj.F8_Enabled, (bool)cfObj.F8_Encrypted, 8);
                }
                if (cfObj.F9_Title != null)
                {
                    if (cfObj.F9_Top.ToString() == "1")
                        bccOrder.cf9Value = GetCanTopCustomFieldValue(ctrlCFCanArea9, (int)cfObj.F9_Type, (bool)cfObj.F9_Enabled, (bool)cfObj.F9_Encrypted, 9);
                }
                if (cfObj.F10_Title != null)
                {
                    if (cfObj.F10_Top.ToString() == "1")
                        bccOrder.cf10Value = GetCanTopCustomFieldValue(ctrlCFCanArea10, (int)cfObj.F10_Type, (bool)cfObj.F10_Enabled, (bool)cfObj.F10_Encrypted, 10);
                }
            }
        }

        private string GetCanTopCustomFieldValue(Panel controlArea, int controltype, bool enabled, bool encrypted, int index)
        {
            string value = "";

            if (enabled)
            {
                if ((controltype == 1) || (controltype == 3))
                {
                    TextBox tf = (TextBox)controlArea.FindControl("controlcan" + index);
                    value = tf.Text;
                }
                else if (controltype == 2)   //drop-down list
                {
                    DropDownList ddl = (DropDownList)controlArea.FindControl("controlcan" + index);
                    value = ddl.SelectedItem.Value;
                }
                else if (controltype == 4)   //ListBox Multiple Selection
                {
                    ListBox ddl = (ListBox)controlArea.FindControl("controlcan" + index);
                    foreach (ListItem li in ddl.Items)
                    {
                        if (li.Selected == true)
                            value = value + ";" + li.Value;
                    }
                }
                return value;
            }
            else
            {
                return "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                TMS_Lib.BAL.BundleClientCandidateOrder bccOrder;
                List<TMS_Lib.BAL.BundleClientCandidateOrder> bundleLst = new List<TMS_Lib.BAL.BundleClientCandidateOrder>();

                int? orderid = -1;
                string txtProcName = "";
                string txtProcPhone = "";
                string txtProcEmail = "";
                DataTable dtClientUser = myDBClass.GetClientUser(Convert.ToInt32(Session["EnrollClientUserId"].ToString()));
                if (dtClientUser.Rows.Count > 0)
                {
                    txtProcName = dtClientUser.Rows[0]["ClientUserFirstName"].ToString() + " " + dtClientUser.Rows[0]["ClientUserLastName"].ToString();
                    txtProcEmail = dtClientUser.Rows[0]["Email"].ToString();
                    txtProcPhone = dtClientUser.Rows[0]["Phone"].ToString();
                }

                Session["Enroll_ClientIdCustomEmailCode"] = "";
                Session["Enroll_ClientIdTimeZoneCode"] = "Eastern Standard Time";
                int intTimeZone = 208;
                DataTable dtClientInfo = myDBClass.GetClientInfo(Convert.ToInt32(Session["EnrollClientId"].ToString()));
                if (dtClientInfo.Rows.Count > 0)
                {
                    intTimeZone = Convert.ToInt32(dtClientInfo.Rows[0]["TimeZone"].ToString());
                    Session["Enroll_ClientIdCustomEmailCode"] = dtClientInfo.Rows[0]["CustomEmailCode"].ToString();
                    if (intTimeZone == 0)
                    {
                        intTimeZone = 208;
                    }
                    DataTable dtTimeZone = myDBClass.GetTimeZoneCode(intTimeZone);
                    if (dtTimeZone.Rows.Count > 0)
                    {
                        Session["Enroll_ClientIdTimeZoneCode"] = dtTimeZone.Rows[0]["TimeZoneCode"].ToString();
                    }
                }

                TMS_Lib.BAL.ClientOrder cOrder = new TMS_Lib.BAL.ClientOrder();

                if ((Session["IsNewClient"] != null) && (Session["NewClientID"] != null) && (Session["IsNewClient"].ToString() == "1") && (Session["NewClientID"].ToString() != "-1"))
                    cOrder.clientid = Convert.ToInt32(Session["NewClientID"]);
                else
                    cOrder.clientid = Convert.ToInt32(Session["EnrollClientId"].ToString());

                cOrder.userid = Convert.ToInt32(Session["EnrollClientUserId"].ToString());
                cOrder.usertypeid = 2;
                cOrder.requesterName = txtProcPhone;
                cOrder.requesterPhone = txtProcPhone;
                cOrder.requesterExt = "";
                cOrder.requesterEmail = txtProcEmail;
                cOrder.isAdminProctor = false;
                cOrder.clientTimeZone = intTimeZone;
                cOrder.country = "";
                cOrder.additionalEmails = "";

                if (Session["ClientCustomFields"] != null)
                {
                    GetClientCustomFieldValues(cOrder, (usp_client_getClientCustomFieldsResult)Session["ClientCustomFields"]);
                }

                //loop and get data from each user control on page
                int tid;
                if ((Session["IsNewClient"] != null) && (Session["NewTestOptionID"] != null) && (Session["IsNewClient"].ToString() == "1") && (Session["NewTestOptionID"].ToString() != "-1"))
                    tid = Convert.ToInt32(Session["NewTestOptionID"]);
                else
                    tid = (int)Session["testoptionid"];

                string testname = (string)Session["TestName"];
                int i = 0;

                try
                {
                    bccOrder = GetBundleOrder();
                    bccOrder.testoptionid = tid;
                    bccOrder.proctorName = txtProcName;
                    bccOrder.proctorPhone = txtProcPhone;
                    bccOrder.proctorEmail = txtProcEmail;
                    bccOrder.clientTimeZone = cOrder.clientTimeZone;
                    bccOrder.notes = "";
                    GetCandTopCustomFieldValues(bccOrder, (usp_client_getCandidateCustomFieldsResult)Session["candidateCFLst"]);
                    bundleLst.Add(bccOrder);

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                string strRPTLang = "";
                string strLPTLang = "";
                System.Text.RegularExpressions.Regex RegHTML = new Regex(@"<\/*?(?![^>]*?\b(?:a|img)\b)[^>]*?>");
                // Check for Time difference of 3 hr for OPI
                TMS_Lib.BAL.BundleClientCandidateOrder bccCheckOPI;
                for (int ii = 0; ii < bundleLst.Count(); ii++)
                {
                    bccCheckOPI = bundleLst[ii];
                    strRPTLang = "";
                    strLPTLang = "";
                    foreach (TMS_Lib.BAL.BundleTestItem btItem in bccCheckOPI.btestLst)
                    {

                        if (RegHTML.IsMatch(bccCheckOPI.cand_firstname))
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in First name field. ";
                        }

                        if (RegHTML.IsMatch(bccCheckOPI.cand_lastname))
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in Last name field. ";
                        }

                        if (RegHTML.IsMatch(bccCheckOPI.candidateid))
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in Candidate Id field. ";
                        }

                        if ((bccCheckOPI.cand_firstname.Contains("')") || bccCheckOPI.cand_firstname.Contains("('") || bccCheckOPI.cand_firstname.Contains(";") || bccCheckOPI.cand_firstname.Contains("/>")) ||
                            (bccCheckOPI.cand_lastname.Contains("')") || bccCheckOPI.cand_lastname.Contains("('") || bccCheckOPI.cand_lastname.Contains(";") || bccCheckOPI.cand_lastname.Contains("/>")) ||
                            (bccCheckOPI.candidateid.Contains("')") || bccCheckOPI.candidateid.Contains("('") || bccCheckOPI.candidateid.Contains(";") || bccCheckOPI.candidateid.Contains("/>")))
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in Candidate First name, Last name or Id field. ";
                        }
                        else if ((bccCheckOPI.cand_firstname.Contains("'") && bccCheckOPI.cand_firstname.Contains("(") && bccCheckOPI.cand_firstname.Contains(")"))
                            || (bccCheckOPI.cand_firstname.Contains("="))
                            || (bccCheckOPI.cand_firstname.Contains("\"") && bccCheckOPI.cand_firstname.Contains("(") && bccCheckOPI.cand_firstname.Contains(")"))
                            || (bccCheckOPI.cand_firstname.Contains("\"") && bccCheckOPI.cand_firstname.Contains("'"))
                            || (bccCheckOPI.cand_firstname.Contains("/>"))
                            )
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in First name field. ";
                        }
                        else if ((bccCheckOPI.cand_lastname.Contains("'") && bccCheckOPI.cand_lastname.Contains("(") && bccCheckOPI.cand_lastname.Contains(")"))
                            || (bccCheckOPI.cand_lastname.Contains("="))
                            || (bccCheckOPI.cand_lastname.Contains("\"") && bccCheckOPI.cand_lastname.Contains("(") && bccCheckOPI.cand_lastname.Contains(")"))
                            || (bccCheckOPI.cand_lastname.Contains("\"") && bccCheckOPI.cand_lastname.Contains("'"))
                            || (bccCheckOPI.cand_lastname.Contains("/>"))
                            )
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in Last name field. ";
                        }
                        else if ((bccCheckOPI.candidateid.Contains("'") && bccCheckOPI.candidateid.Contains("(") && bccCheckOPI.candidateid.Contains(")"))
                                 || (bccCheckOPI.candidateid.Contains("="))
                                 || (bccCheckOPI.candidateid.Contains("\"") && bccCheckOPI.candidateid.Contains("(") && bccCheckOPI.candidateid.Contains(")"))
                                 || (bccCheckOPI.candidateid.Contains("\"") && bccCheckOPI.candidateid.Contains("'"))
                                 || (bccCheckOPI.candidateid.Contains("/>"))
                                 )
                        {
                            message = message + "Candidate " + (ii + 1).ToString() + ": Invalid characters in Candidate Id field. ";
                        }

                        if (btItem.producttype == 1)
                        {
                            TimeSpan DiffTime = btItem.timerangeto1.Subtract(btItem.timerangefrom1);
                            if (DiffTime.Hours < 3)
                                message = message + "<br/> First Choice Time difference must be greater than 3 hours";

                            TimeSpan DiffTime2 = btItem.timerangeto2.Subtract(btItem.timerangefrom2);
                            if (DiffTime2.Hours < 3)
                                message = message + "<br/> Second Choice Time difference must be greater than 3 hours";
                        }
                        else if (btItem.producttype == 3 || btItem.producttype == 5)
                        {
                            if (btItem.testlevel == 581)
                                strRPTLang = btItem.langid.ToString();
                            else if (btItem.testlevel == 582)
                                strLPTLang = btItem.langid.ToString();
                        }

                        // Below retest check block added on 07/24/2026
                        try
                        {
                            DataTable dtRetest = myDBClass.CheckRetest(cOrder.clientid, bccCheckOPI.cand_firstname, bccCheckOPI.cand_lastname, bccCheckOPI.candidateid, bccCheckOPI.candidateemail,
                                            btItem.isbundle ? btItem.langid : bccCheckOPI.langid, btItem.testid, btItem.producttype, 0, "");
                            if (dtRetest.Rows.Count > 0)
                            {
                                if (dtRetest.Rows[0]["RetVal"].ToString() != "0")
                                {
                                    message = message + "<br/>&nbsp;- Candidate " + (ii + 1).ToString() + dtRetest.Rows[0]["RetMessage"].ToString();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                myDBClass.ErrorLogWrite("ClientSite", ex.Message + ex.StackTrace, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "btnSubmit_Click()", "");
                            }
                            catch { }
                        }
                        // end of retest check block
                    }

                    if (strRPTLang != strLPTLang)
                    {
                        message = message + "&nbsp;<br/>Candidate " + (ii + 1).ToString() + ": ACTFL Proficiency Placement Test must have same language for both Reading and Listening test";
                    }

                }

                //if no error for either client data or candidate data, proceed
                if (message == "")
                {
                    try
                    {
                        //save client data first
                        client.setClientOrder(cOrder, ref orderid);
                        myDBClass.B2B_TestRequest(Convert.ToInt32(Session["User_B2BRowId"].ToString()), Convert.ToInt32(orderid));
                        //add orderid to each bundle candidate order
                        i = bundleLst.Count;     
                        bccOrder = bundleLst[i - 1];
                        bccOrder.orderid = (int)orderid;
                        //save candidate data
                        client.setClientBundleCandidateOrder(bundleLst.ToArray());
                    }
                    catch (Exception ex)
                    {
                        client.deleteTestOrder((int)orderid);
                        lblError.Text = "Could not process your request, please retry again.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                        return;
                    }

                    Session["Passedtestname"] = testname;
                    Session["Passedorderid"] = orderid;
                    //flag to send email from test request summary page
                    Session["SendEmail"] = "1";
                    Response.Redirect("Confirmation.aspx", false);
                }
                else
                {                    
                    message = "<script>HideProgress();showMessage('" + message + "');</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "<script>HideProgress();</script>", false);
                }
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

    }

    public class usp_client_getTestLanguagesByTestIDResult
    {
        public int languageid { get; set; }

        public string name { get; set; }
    }

    public class usp_client_getTestInstructionLanguagesResult
    {
        public int languageid { get; set; }

        public string name { get; set; }
    }
}