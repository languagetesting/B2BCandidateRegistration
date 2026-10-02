using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TMS_Lib.DAL.ClientSite;
using System.Configuration;

namespace B2BCandidateRegistration
{
    public partial class Confirmation : System.Web.UI.Page
    {
        B2BCandRegistration myDBClass = new B2BCandRegistration();
        TMS_Lib.BAL.ClientSite client = new TMS_Lib.BAL.ClientSite();
        public string testname;
        public int orderid;
        public List<usp_client_getClientCandidateOrderResult> candidateOrderList;
        public List<usp_client_getClientCandidateOrderResultV2> candidateOrderListBundle;
        public List<usp_client_getTOpicClientCandidateOrderResult> candidateOrderListTOPIc;
        public usp_client_getClientCandidateOrderResult cfObj;
        public int productType;
        public int subproductType;
        bool isSlot;
        bool isBundle;
        public List<usp_client_getClientOrderInfoResult> clientOrderList;
        public int ApptId;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    testname = Session["Passedtestname"].ToString();
                    orderid = Convert.ToInt32(Session["Passedorderid"].ToString());                   
                    productType = (int)Session["ProductType"];
                    subproductType = (int)Session["SubProductType"];
                    isSlot = (bool)Session["IsSlot"];
                    isBundle = (bool)Session["IsBundle"];
                    ApptId = -1;
                    clientOrderList = myDBClass.getClientOrderInfo(orderid).ToList();
                    if (isBundle)
                    {
                        candidateOrderListBundle = myDBClass.getClientCandidateOrderBundleV2(orderid).ToList();
                        for (int i = 0; i < candidateOrderListBundle.Count; i++)
                        {
                            ApptId = -1;
                            try
                            {
                                DataTable dtApptId = myDBClass.B2B_GetAppointmentId(orderid, candidateOrderListBundle[i].login, candidateOrderListBundle[i].password, (int)candidateOrderListBundle[i].ProductType, candidateOrderListBundle[i].languagename);
                                if (dtApptId.Rows.Count > 0)
                                {
                                    ApptId = Convert.ToInt32(dtApptId.Rows[0]["AppointmentID"].ToString());
                                }
                            }
                            catch { }
                            if ((candidateOrderListBundle[i].RatingCertificationName.ToUpper().StartsWith("UNOFFICIAL")) && (candidateOrderListBundle[i].TestStatus.ToUpper() == "SCHEDULED"))
                            {
                                if ((candidateOrderListBundle[i].email != null) && (candidateOrderListBundle[i].email != ""))
                                {
                                    if (((int)candidateOrderListBundle[i].ProductType == 2) && ((int)candidateOrderListBundle[i].TestDelivery == 1))
                                    {
                                        SendEmailToCandidateTOPIc(candidateOrderListBundle[i].firstname + " " + candidateOrderListBundle[i].lastname,
                                                   candidateOrderListBundle[i].email,
                                                   candidateOrderListBundle[0].ClientName,
                                                   candidateOrderListBundle[i].languagename,
                                                   candidateOrderListBundle[i].login,
                                                   candidateOrderListBundle[i].password,
                                                   String.Format("{0:M/d/yyyy}", candidateOrderListBundle[i].ExpirationDate),
                                                   (int)candidateOrderListBundle[i].ProductType,
                                                   (int)candidateOrderListBundle[i].TestDelivery,
                                                   candidateOrderListBundle[i].clienttestname,
                                                   (int)candidateOrderListBundle[i].PromptGeneration,
                                                   "",
                                                   (Session["Enroll_ClientIdCustomEmailCode"].ToString()),
                                                   ApptId);

                                    }
                                    else
                                    {
                                        SendEmailToCandidate(candidateOrderList[i].firstname + " " + candidateOrderList[i].lastname,
                                                            candidateOrderList[i].email,
                                                            clientOrderList[0].ClientName,
                                                            candidateOrderList[i].languagename,
                                                            candidateOrderList[i].login,
                                                            candidateOrderList[i].password,
                                                            candidateOrderList[i].ExpirationDate,
                                                            (int)candidateOrderList[i].ProductType,
                                                            (int)candidateOrderList[i].TestDelivery,
                                                            candidateOrderList[i].clienttestname,
                                                            (bool)candidateOrderList[i].Proctorless,
                                                            (Session["Enroll_ClientIdCustomEmailCode"].ToString()),
                                                            ApptId);
                                    }
                                }
                            }
                        }
                    }
                    else if (((int)Session["ProductType"] == 2) && ((int)Session["TestDelivery"] == 1))
                    {
                        candidateOrderListTOPIc = client.getTOPIcClientCandidateOrder(orderid).ToList();
                        for (int i = 0; i < candidateOrderListTOPIc.Count; i++)
                        {
                            ApptId = -1;
                            try
                            {
                                DataTable dtApptId = myDBClass.B2B_GetAppointmentId(orderid, candidateOrderListTOPIc[i].EnrollmentID, candidateOrderListTOPIc[i].Password, (int)candidateOrderListTOPIc[i].ProductType, candidateOrderListTOPIc[i].LanguageName);
                                if (dtApptId.Rows.Count > 0)
                                {
                                    ApptId = Convert.ToInt32(dtApptId.Rows[0]["AppointmentID"].ToString());
                                }
                            }
                            catch { }

                            if ((candidateOrderListTOPIc[i].CandidateEmail != null) && (candidateOrderListTOPIc[i].CandidateEmail != ""))
                            {
                                SendEmailToCandidateTOPIc(candidateOrderListTOPIc[i].Cand_FirstName + " " + candidateOrderListTOPIc[i].Cand_LastName,
                                                        candidateOrderListTOPIc[i].CandidateEmail,
                                                        candidateOrderListTOPIc[i].ClientName,
                                                        candidateOrderListTOPIc[i].LanguageName,
                                                        candidateOrderListTOPIc[i].EnrollmentID,
                                                        candidateOrderListTOPIc[i].Password,
                                                        String.Format("{0:M/d/yyyy}", candidateOrderListTOPIc[i].ExpirationDate),
                                                        (int)candidateOrderListTOPIc[i].ProductType,
                                                        (int)candidateOrderListTOPIc[i].TestDelivery,
                                                        testname,
                                                        (int)candidateOrderListTOPIc[i].PromptGeneration,
                                                        candidateOrderListTOPIc[i].CF10,
                                                        (Session["Enroll_ClientIdCustomEmailCode"].ToString()),
                                                        ApptId);
                            }
                        }
                    }
                    else
                    {
                        candidateOrderList = client.getClientCandidateOrder_AES(orderid).ToList();
                        for (int i = 0; i < candidateOrderList.Count; i++)
                        {
                            ApptId = -1;
                            try
                            {
                                DataTable dtApptId = myDBClass.B2B_GetAppointmentId(orderid, candidateOrderList[i].login, candidateOrderList[i].password, (int)candidateOrderList[i].ProductType, candidateOrderList[i].languagename);
                                if (dtApptId.Rows.Count > 0)
                                {
                                    ApptId = Convert.ToInt32(dtApptId.Rows[0]["AppointmentID"].ToString());
                                }
                            }
                            catch { }

                            if ((candidateOrderList[i].email != null) && (candidateOrderList[i].email != ""))
                            {
                                SendEmailToCandidate(candidateOrderList[i].firstname + " " + candidateOrderList[i].lastname,
                                                        candidateOrderList[i].email,
                                                        clientOrderList[0].ClientName,
                                                        candidateOrderList[i].languagename,
                                                        candidateOrderList[i].login,
                                                        candidateOrderList[i].password,
                                                        candidateOrderList[i].ExpirationDate,
                                                        (int)candidateOrderList[i].ProductType,
                                                        (int)candidateOrderList[i].TestDelivery,
                                                        testname,
                                                        (bool)candidateOrderList[i].Proctorless,
                                                        (Session["Enroll_ClientIdCustomEmailCode"].ToString()),
                                                        ApptId);
                            }
                        }
                    }
                    
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Your Test Request is accept, go to Scheduled Tests to view it.";
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "Page_Load", "");
                }
                catch { }
            }
        }

        private void SendEmailToCandidate(string candidatename, string candidateemail, string clientname, string language, string loginid, string password, string expirationdate, int producttype, int testdelivery, string testname, bool isproctorless, string customemailcode, int intApptId)
        {

            try
            {                
                string strBody = "";
                string strSubject = "";

                //Use email template to send email
                if ((producttype == 2) && (testdelivery == 1))  //Tel OPIc
                {
                    var EmailTestRequestScheduledTelOPIcTemplate = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplate);                    
                    strSubject = EmailTestRequestScheduledTelOPIcTemplate.EmailSubject;
                    strSubject = strSubject.Replace("[strTestname]", testname);

                    //replace tokens with custom data
                    strBody = strBody.Replace("[strCanFullName]", candidatename);
                    strBody = strBody.Replace("[strCompany]", clientname);
                    strBody = strBody.Replace("[strLanguage]", language);
                    strBody = strBody.Replace("[strLoginId]", loginid);
                }
                else if (producttype == 4)   //WPT
                {
                    if (customemailcode.ToUpper() == "PAYPAL")
                    {                     
                        var EmailTestRequestScheduledWPTTemplate_PayPal = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledWPTTemplate_PayPal);
                        strBody = EmailTestRequestScheduledWPTTemplate_PayPal.EmailTemplate;
                        strSubject = EmailTestRequestScheduledWPTTemplate_PayPal.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);

                        strBody = strBody.Replace("[strPayPalRep]", ConfigurationManager.AppSettings["PayPay_EmailTemplate_Rep"]);
                    }
                    else if (customemailcode.ToUpper() == "MARRIOTT")
                    {                        
                        var EmailTestRequestScheduledWPTTemplate_Marriott = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledWPTTemplate_Marriott);
                        strBody = EmailTestRequestScheduledWPTTemplate_Marriott.EmailTemplate;
                        strSubject = EmailTestRequestScheduledWPTTemplate_Marriott.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                    }
                    else if (customemailcode.ToUpper() == "WESTJET")
                    {                        
                        var EmailTestRequestScheduledWPTTemplate_WestJet = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledWPTTemplate_WestJet);
                        strBody = EmailTestRequestScheduledWPTTemplate_WestJet.EmailTemplate;
                        strSubject = EmailTestRequestScheduledWPTTemplate_WestJet.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                    }
                    else
                    {
                        int formid = myDBClass.getFormIDForWPT(Convert.ToInt32(Session["EnrollClientId"].ToString()), loginid);
                        if (formid.ToString() == ConfigurationManager.AppSettings["WPT_FormID"])
                        {                            
                            var EmailTestRequestScheduledWPTTemplate_195 = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledWPTTemplate_195);
                            strBody = EmailTestRequestScheduledWPTTemplate_195.EmailTemplate;

                            strSubject = EmailTestRequestScheduledWPTTemplate_195.EmailSubject;
                            strSubject = strSubject.Replace("[strTestname]", testname);
                        }
                        else
                        {                            
                            var EmailTestRequestScheduledWPTTemplate = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledWPTTemplate);
                            strBody = EmailTestRequestScheduledWPTTemplate.EmailTemplate;

                            strSubject = EmailTestRequestScheduledWPTTemplate.EmailSubject;
                            strSubject = strSubject.Replace("[strTestname]", testname);
                        }
                    }
                    strBody = strBody.Replace("[strCanFullName]", candidatename);
                    strBody = strBody.Replace("[strCompany]", clientname);
                    strBody = strBody.Replace("[strLanguage]", language);
                    strBody = strBody.Replace("[strLoginId]", loginid);
                    strBody = strBody.Replace("[strAccessCode]", password);
                }
                else if ((producttype == 2) && (customemailcode.ToUpper() == "MARRIOTT"))   //OPIC - Marriott
                {
                    var EmailTestRequestScheduledOPIcTemplate_Marriott = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledOPIcTemplate_Marriott);
                    strBody = EmailTestRequestScheduledOPIcTemplate_Marriott.EmailTemplate;

                    strSubject = EmailTestRequestScheduledOPIcTemplate_Marriott.EmailSubject;
                    strSubject = strSubject.Replace("[strTestname]", testname);

                    //replace tokens with custom data
                    strBody = strBody.Replace("[strLoginId]", loginid);
                    strBody = strBody.Replace("[strAccessCode]", password);
                }
                else if ((producttype == 2) && ((Session["ProctorType"].ToString() == "2") || (Session["ProctorType"].ToString() == "3")))   //Unproctored OPIc
                {
                    if (customemailcode.ToUpper() == "PAYPAL")
                    {
                        var EmailTestRequestScheduledOPIcTemplate_PayPal = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledOPIcTemplate_PayPal);
                        strBody = EmailTestRequestScheduledOPIcTemplate_PayPal.EmailTemplate;

                        strSubject = EmailTestRequestScheduledOPIcTemplate_PayPal.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                        strBody = strBody.Replace("[strPayPalRep]", ConfigurationManager.AppSettings["PayPay_EmailTemplate_Rep"]);
                    }
                    else if (customemailcode.ToUpper() == "WESTJET")
                    {
                        var EmailTestRequestScheduledOPIcTemplate_WestJet = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledOPIcTemplate_WestJet);
                        strBody = EmailTestRequestScheduledOPIcTemplate_WestJet.EmailTemplate;
                        strSubject = EmailTestRequestScheduledOPIcTemplate_WestJet.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                    }
                    else
                    {
                        var EmailTestRequestScheduledOPIcTemplate = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledOPIcTemplate);
                        strBody = EmailTestRequestScheduledOPIcTemplate.EmailTemplate;
                        strSubject = EmailTestRequestScheduledOPIcTemplate.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                        int intclientid = Convert.ToInt32(Session["EnrollClientId"].ToString());
                        string strClientIdsforNewOPIc = System.Configuration.ConfigurationManager.AppSettings["NewOPIcClientIds"].ToString();
                        if (strClientIdsforNewOPIc.Contains(intclientid.ToString() + ","))
                            strBody = strBody.Replace("[strOPIcURL]", ConfigurationManager.AppSettings["New_Confirm_OPIc"]);
                        else
                            strBody = strBody.Replace("[strOPIcURL]", ConfigurationManager.AppSettings["Confirm_OPIc"]);
                    }

                    //replace tokens with custom data
                    strBody = strBody.Replace("[strCanFullName]", candidatename);
                    strBody = strBody.Replace("[strCompany]", clientname);
                    strBody = strBody.Replace("[strLanguage]", language);
                    strBody = strBody.Replace("[strLoginId]", loginid);
                    strBody = strBody.Replace("[strAccessCode]", password);
                }
                DateTime dtExpDate = Convert.ToDateTime(expirationdate);
                strBody = strBody.Replace("[strExpirationDt]", dtExpDate.ToString("MMMM") + " " + dtExpDate.Day.ToString() + ", " + dtExpDate.Year.ToString());
                if ((strBody != "") && (candidateemail != ""))
                {
                    string strFrom = ConfigurationManager.AppSettings["EmailSender"];
                    Utility.SendMails(strFrom, candidateemail, strSubject, strBody, 1, true, intApptId);
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Test request is accepted but error when attempting to send email to for test request.";
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "SendEmailToCandidate", "");
                }
                catch { }
            }
        }

        private void SendEmailToCandidateTOPIc(string candidatename, string candidateemail, string clientname, string language, string loginid, string password, string expirationdate, int producttype, int testdelivery, string testname, int promptgeneration, string customField, string customemailcode, int intApptId)
        {

            try
            {                
                string strBody = "";
                string strSubject = "";

                //Use email template to send email
                if ((producttype == 2) && (testdelivery == 1))  //Tel OPIc
                {
                    if ((customemailcode.ToUpper() == "VERIZON") && (promptgeneration == 2))  //if client is Verizon and the test is Unofficial Fixed Form Tel Opic
                    {                        
                        var EmailTestRequestScheduledTelOPIcTemplate_Verizon = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplate_Verizon);
                        strBody = EmailTestRequestScheduledTelOPIcTemplate_Verizon.EmailTemplate;
                        strSubject = EmailTestRequestScheduledTelOPIcTemplate_Verizon.EmailSubject;
                        strBody = strBody.Replace("[strCanFullName]", candidatename);
                        strBody = strBody.Replace("[strLoginId]", loginid);                        
                        strBody = strBody.Replace("[strPositionName]", customField);   //custom field  for verizon is Position Name
                    }
                    else if ((customemailcode.ToUpper() == "PNC") && (promptgeneration == 2))  //if client is PNC and the test is Unofficial Fixed Form Tel Opic
                    {
                        if (customField == null)
                            customField = "";
                        var EmailTestRequestScheduledTelOPIcTemplate_PNC = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplate_PNC);
                        strBody = EmailTestRequestScheduledTelOPIcTemplate_PNC.EmailTemplate;
                        strSubject = EmailTestRequestScheduledTelOPIcTemplate_PNC.EmailSubject;
                        strBody = strBody.Replace("[strCanFullName]", candidatename);
                        strBody = strBody.Replace("[strCompany]", clientname);
                        strBody = strBody.Replace("[strLanguage]", language);
                        strBody = strBody.Replace("[strLoginId]", loginid);                
                        strBody = strBody.Replace("[strTimeZone]", Session["Enroll_ClientIdTimeZoneCode"].ToString());
                    }
                    else if ((customemailcode.ToUpper() == "ATTINT") && (promptgeneration == 2))  //if client is AT&T International and fixed form Tel Opic
                    {                        
                        var EmailTestRequestScheduledTelOPIcTemplate_ATTINT = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplate_ATTINT);
                        strBody = EmailTestRequestScheduledTelOPIcTemplate_ATTINT.EmailTemplate;
                        strSubject = EmailTestRequestScheduledTelOPIcTemplate_ATTINT.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                        strBody = strBody.Replace("[strCanFullName]", candidatename);
                        strBody = strBody.Replace("[strCompany]", clientname);
                        strBody = strBody.Replace("[strLanguage]", language);
                        strBody = strBody.Replace("[strLoginId]", loginid);                        
                    }
                    else if ((customemailcode.ToUpper() == "ATTRSC") && (promptgeneration == 2))  //if client is AT&T RSC Role -Miami/Non Miami and fixed form Tel Opic
                    {                        
                        var EmailTestRequestScheduledTelOPIcTemplateATTRSC = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplateATTRSC);
                        strBody = EmailTestRequestScheduledTelOPIcTemplateATTRSC.EmailTemplate;
                        strSubject = EmailTestRequestScheduledTelOPIcTemplateATTRSC.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);                        
                        strBody = strBody.Replace("[strCanFullName]", candidatename);
                        strBody = strBody.Replace("[strCompany]", clientname);
                        strBody = strBody.Replace("[strLanguage]", language);
                        strBody = strBody.Replace("[strLoginId]", loginid);
                    }
                    else
                    {                        
                        var EmailTestRequestScheduledTelOPIcTemplate = myDBClass.GetEmailTemplateByTemplateId((int)EmailTemplates.EmailTestRequestScheduledTelOPIcTemplate);
                        strBody = EmailTestRequestScheduledTelOPIcTemplate.EmailTemplate;
                        strSubject = EmailTestRequestScheduledTelOPIcTemplate.EmailSubject;
                        strSubject = strSubject.Replace("[strTestname]", testname);
                        strBody = strBody.Replace("[strCanFullName]", candidatename);
                        strBody = strBody.Replace("[strCompany]", clientname);
                        strBody = strBody.Replace("[strLanguage]", language);
                        strBody = strBody.Replace("[strLoginId]", loginid);                        
                    }
                }
                DateTime dtExpDate = Convert.ToDateTime(expirationdate);
                strBody = strBody.Replace("[strExpirationDt]", dtExpDate.ToString("MMMM") + " " + dtExpDate.Day.ToString() + ", " + dtExpDate.Year.ToString());
                if ((strBody != "") && (candidateemail != ""))
                {
                    string strFrom = ConfigurationManager.AppSettings["EmailSender"];         
                    Utility.SendMails(strFrom, candidateemail, strSubject, strBody, 1, true, intApptId);
                }
            }
            catch (Exception Ex)
            {
                lblError.Text = "Test request is accepted but error when attempting to send email to for test request.";
                try
                {
                    myDBClass.ErrorLogWrite("B2BCandidateRegistration", Ex.Message, Server.MapPath(this.AppRelativeVirtualPath), Request.UserAgent.ToString(), Request.UserHostName.ToString(), "SendEmailToCandidate", "");
                }
                catch { }
            }
        }

    }
}