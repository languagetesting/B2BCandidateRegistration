using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace B2BCandidateRegistration
{
    public class B2BCandRegistration
    {
        String appName = "B2BCandidateRegistration";
        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["LTI_TMSConnectionString"].ToString();
        }

        public DataTable ErrorLogWrite(string strAppName, string strErrMsg, string strFilePath, string strUserBrowser, string strHostName, string strMethodName, string strUserId)
        {
            try
            {
                Object[] objParam = new Object[8];
                objParam[0] = strAppName;
                objParam[1] = strErrMsg;
                objParam[2] = strFilePath;
                objParam[3] = strUserBrowser;
                objParam[4] = strHostName;
                objParam[5] = strMethodName;
                objParam[6] = strUserId;
                objParam[7] = 0;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_ErrorLogAdd", objParam);
            }
            catch (Exception ee)
            {
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable CreateB2BRegistation(string strEmail, string strFirstName, string strLastName, string strPassword, string strUserIP, string strUserAgent)
        {
            try
            {
                Object[] objParam = new Object[6];
                objParam[0] = strEmail;
                objParam[1] = strFirstName;
                objParam[2] = strLastName;
                objParam[3] = strPassword;
                objParam[4] = strUserIP;
                objParam[5] = strUserAgent;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_Create", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "CreateB2BRegistation()", "Email: " + strEmail);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable OTP_Validate(string strEmail, int intOTP, string strCallerType, string strUserIP, string strUserAgent)
        {
            try
            {
                Object[] objParam = new Object[5];
                objParam[0] = strEmail;
                objParam[1] = intOTP;
                objParam[2] = strCallerType;                
                objParam[3] = strUserIP;
                objParam[4] = strUserAgent;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_MFA_OTP", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "OTP_Validate()", "Email: " + strEmail);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable Login_Validate(string strEmail, string strPassword, string strUserIP, string strUserAgent)
        {
            try
            {
                Object[] objParam = new Object[4];
                objParam[0] = strEmail;
                objParam[1] = strPassword;                
                objParam[2] = strUserIP;
                objParam[3] = strUserAgent;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_Login", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "Login_Validate()", "Email: " + strEmail);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable Forgot_Password(string strEmail)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = strEmail;                
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_ForgotPassword", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "Forgot_Password()", "Email: " + strEmail);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable ChangePassword(string strEmail, string strPassword, int intB2BRowId, string strOldPassword)
        {
            try
            {
                Object[] objParam = new Object[4];
                objParam[0] = strEmail;
                objParam[1] = intB2BRowId;
                objParam[2] = strPassword;
                objParam[3] = strOldPassword;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_ChangePassword", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "ChangePassword()", "Email: " + strEmail);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable EnrollCode_Validate(string strEnrollCode)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = strEnrollCode;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_EnrollCode_Check", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "EnrollCode_Validate()", "Enroll Code: " + strEnrollCode);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable EnrollCode_TestOptions(int intEnrollId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = intEnrollId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_Enrolled_TestOptions", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "EnrollCode_TestOptions()", "Enroll Id: " + intEnrollId.ToString());
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public string GetProctorTitle(int testId)
        {
            String strProctorTitle = "";
            Object[] objParam = new Object[1];
            objParam[0] = testId;
            DataTable dtProctorTitle = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_getProctorTitle", objParam);
            strProctorTitle = dtProctorTitle.Rows[0][0].ToString();
            return strProctorTitle;
        }

        public DataTable getCustomFieldRelationInfoForClient(int clientid)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = clientid;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getCustomFieldRelationInfoForClient", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getCustomFieldRelationInfoForClient()", "clientid = " + clientid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public List<usp_client_getClientDetailedInfoResult> getClientDetailedInfo(int clientid, int clientuserid)
        {
            try
            {
                List<usp_client_getClientDetailedInfoResult> lstClientInfo = new List<usp_client_getClientDetailedInfoResult>();

                Object[] objParam = new Object[2];
                objParam[0] = clientid;
                objParam[1] = clientuserid;

                DataTable dtClientInfo = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getClientDetailedInfo_Aes", objParam);
                if (dtClientInfo != null)
                {
                    usp_client_getClientDetailedInfoResult lst = new usp_client_getClientDetailedInfoResult();

                    lst.firstname = dtClientInfo.Rows[0][0].ToString();
                    lst.lastname = dtClientInfo.Rows[0][1].ToString();
                    lst.phone = dtClientInfo.Rows[0][2].ToString();
                    lst.ext = dtClientInfo.Rows[0][3].ToString();
                    lst.email = dtClientInfo.Rows[0][4].ToString();
                    lst.timezone = Convert.ToInt32(dtClientInfo.Rows[0][5]);
                    lst.countryname = dtClientInfo.Rows[0][6].ToString();
                    lst.IsPrePostFieldsRequired = Convert.ToBoolean(dtClientInfo.Rows[0][7]);
                    lst.ApplTemplate = dtClientInfo.Rows[0][8].ToString();
                    lst.userphone = dtClientInfo.Rows[0][9].ToString();

                    lstClientInfo.Add(lst);
                }

                return lstClientInfo;

            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getClientDetailedInfo()", "userid=" + clientuserid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable Check4Skills(int intClientId, int intTestOptionId)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = intClientId;
                objParam[1] = intTestOptionId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_check4Skills", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "usp_client_check4Skills", "ClientId = ");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public string GetClientsCustomFieldMapping(int clientid)
        {
            try
            {
                String mappedCustomFieldName = "";

                Object[] objParam = new Object[1];
                objParam[0] = clientid;
                DataTable dtCustomField = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getClientsCustomFieldMapping", objParam);

                if (dtCustomField != null)
                {
                    if (dtCustomField.Rows.Count > 0)
                    {
                        if (dtCustomField.Rows[0][0].ToString() != "")
                            mappedCustomFieldName = dtCustomField.Rows[0][0].ToString();
                    }
                }
                return mappedCustomFieldName;
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetClientsCustomFieldMapping()", "clientid=" + clientid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable getCFRelationDataForMasterField(int relationshipid, string masterfielddata)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = relationshipid;
                objParam[1] = masterfielddata;

                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getCFRelationDataForMasterField", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "usp_client_getCFRelationDataForMasterField()", "relationshipid = " + relationshipid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable GetClientFormSelection(int intClientTestId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = intClientTestId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getClientFormSelection", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetClientFormSelection()", "clienttestid=" + intClientTestId);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public List<usp_client_getTestInstructionLanguagesResult> getTestInstructionLanguages(int clientid, int testid)
        {
            try
            {
                List<usp_client_getTestInstructionLanguagesResult> lstTestInstruction = new List<usp_client_getTestInstructionLanguagesResult>();
                Object[] objParam = new Object[2];
                objParam[0] = clientid;
                objParam[1] = testid;

                DataTable dtTestInstructionList = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getTestInstructionLanguages", objParam);
                if (dtTestInstructionList != null)
                {
                    for (int i = 0; i < dtTestInstructionList.Rows.Count; i++)
                    {
                        usp_client_getTestInstructionLanguagesResult lst = new usp_client_getTestInstructionLanguagesResult();
                        lst.languageid = Convert.ToInt32(dtTestInstructionList.Rows[i][0]);
                        lst.name = dtTestInstructionList.Rows[i][1].ToString();

                        lstTestInstruction.Add(lst);
                    }
                }
                return lstTestInstruction;
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getClientTestInstructionLanguages()", "testid=" + testid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public List<usp_client_getTestLanguagesByTestIDResult> getTestLanguagesByTestID(int clientid, int testid)
        {
            try
            {
                List<usp_client_getTestLanguagesByTestIDResult> lstTestLanguagesByTestID = new List<usp_client_getTestLanguagesByTestIDResult>();
                Object[] objParam = new Object[2];
                objParam[0] = clientid;
                objParam[1] = testid;

                DataTable dtTestLanguagesList = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getTestLanguagesByTestID", objParam);
                if (dtTestLanguagesList != null)
                {
                    for (int i = 0; i < dtTestLanguagesList.Rows.Count; i++)
                    {
                        usp_client_getTestLanguagesByTestIDResult lst = new usp_client_getTestLanguagesByTestIDResult();
                        lst.languageid = Convert.ToInt32(dtTestLanguagesList.Rows[i][0]);
                        lst.name = dtTestLanguagesList.Rows[i][1].ToString();

                        lstTestLanguagesByTestID.Add(lst);
                    }
                }
                return lstTestLanguagesByTestID;
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getTestLanguagesByTestID()", "testid=" + testid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable GetBYUAccommodations()
        {
            try
            {
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_GetBYUAccommodationTypes");
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetBYUAccommodations()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable GetClientUser(int ClientUserId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = ClientUserId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_getClientUser", objParam);                
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetClientUser()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable GetClientInfo(int ClientId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = ClientId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_getClientInfo", objParam);                
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetClientInfo()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }
        public DataTable GetTimeZoneCode(int TimeZoneId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = TimeZoneId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_getTimeZoneCode", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetTimeZoneCode()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }


        public DataTable CheckRetest(int intClientId, string strFirstName, string strLastName, string strCandId, string strEmailId, int intLanguageId, int intTestId, int intProductType, int intTestOptionId, string strLanguageName)
        {
            try
            {
                Object[] objParam = new Object[10];
                objParam[0] = intClientId;
                objParam[1] = strFirstName;
                objParam[2] = strLastName;
                objParam[3] = strCandId;
                objParam[4] = strEmailId;
                objParam[5] = intLanguageId;
                objParam[6] = intTestId;
                objParam[7] = intProductType;
                objParam[8] = intTestOptionId;
                objParam[9] = strLanguageName;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_RetestCheck", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "usp_client_RetestCheck()", "ClientId=" + intClientId.ToString());
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public List<usp_client_getClientOrderInfoResult> getClientOrderInfo(int orderid)
        {
            try
            {
                List<usp_client_getClientOrderInfoResult> lstClientOrderInfo = new List<usp_client_getClientOrderInfoResult>();

                Object[] objParam = new Object[1];
                objParam[0] = orderid;

                DataTable dtInfo = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getClientOrderInfo_Aes", objParam);
                if (dtInfo != null)
                {
                    usp_client_getClientOrderInfoResult lst = new usp_client_getClientOrderInfoResult();

                    lst.requestername = dtInfo.Rows[0][0].ToString();
                    lst.requesterphone = dtInfo.Rows[0][1].ToString();
                    lst.requestphoneext = dtInfo.Rows[0][2].ToString();
                    lst.ClientName = dtInfo.Rows[0][3].ToString();
                    lst.requesteremail = dtInfo.Rows[0][4].ToString();
                    lst.ProctorName = dtInfo.Rows[0][5].ToString();
                    lst.ProctorPhone = dtInfo.Rows[0][6].ToString();
                    lst.ProctorEmail = dtInfo.Rows[0][7].ToString();
                    lst.OrderDate = Convert.ToDateTime(dtInfo.Rows[0][8]);
                    lst.TimeZoneCode = dtInfo.Rows[0][9].ToString();
                    lst.Country = dtInfo.Rows[0][10].ToString();

                    lst.cf_1_value = dtInfo.Rows[0][11].ToString();
                    lst.cf_1_enabled = Convert.ToBoolean(dtInfo.Rows[0][12]);
                    lst.cf_1_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][13]);
                    lst.cf_1_title = dtInfo.Rows[0][14].ToString();

                    lst.cf_2_value = dtInfo.Rows[0][15].ToString();
                    lst.cf_2_enabled = Convert.ToBoolean(dtInfo.Rows[0][16]);
                    lst.cf_2_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][17]);
                    lst.cf_2_title = dtInfo.Rows[0][18].ToString();

                    lst.cf_3_value = dtInfo.Rows[0][19].ToString();
                    lst.cf_3_enabled = Convert.ToBoolean(dtInfo.Rows[0][20]);
                    lst.cf_3_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][21]);
                    lst.cf_3_title = dtInfo.Rows[0][22].ToString();

                    lst.cf_4_value = dtInfo.Rows[0][23].ToString();
                    lst.cf_4_enabled = Convert.ToBoolean(dtInfo.Rows[0][24]);
                    lst.cf_4_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][25]);
                    lst.cf_4_title = dtInfo.Rows[0][26].ToString();

                    lst.cf_5_value = dtInfo.Rows[0][27].ToString();
                    lst.cf_5_enabled = Convert.ToBoolean(dtInfo.Rows[0][28]);
                    lst.cf_5_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][29]);
                    lst.cf_5_title = dtInfo.Rows[0][30].ToString();

                    lst.cf_6_value = dtInfo.Rows[0][31].ToString();
                    lst.cf_6_enabled = Convert.ToBoolean(dtInfo.Rows[0][32]);
                    lst.cf_6_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][33]);
                    lst.cf_6_title = dtInfo.Rows[0][34].ToString();

                    lst.cf_7_value = dtInfo.Rows[0][35].ToString();
                    lst.cf_7_enabled = Convert.ToBoolean(dtInfo.Rows[0][36]);
                    lst.cf_7_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][37]);
                    lst.cf_7_title = dtInfo.Rows[0][38].ToString();

                    lst.cf_8_value = dtInfo.Rows[0][39].ToString();
                    lst.cf_8_enabled = Convert.ToBoolean(dtInfo.Rows[0][40]);
                    lst.cf_8_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][41]);
                    lst.cf_8_title = dtInfo.Rows[0][42].ToString();

                    lst.cf_9_value = dtInfo.Rows[0][43].ToString();
                    lst.cf_9_enabled = Convert.ToBoolean(dtInfo.Rows[0][44]);
                    lst.cf_9_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][45]);
                    lst.cf_9_title = dtInfo.Rows[0][46].ToString();

                    lst.cf_10_value = dtInfo.Rows[0][47].ToString();
                    lst.cf_10_enabled = Convert.ToBoolean(dtInfo.Rows[0][48]);
                    lst.cf_10_isEncrypted = Convert.ToBoolean(dtInfo.Rows[0][49]);
                    lst.cf_10_title = dtInfo.Rows[0][50].ToString();

                    lst.additionalEmails = dtInfo.Rows[0][51].ToString();

                    lstClientOrderInfo.Add(lst);

                }

                return lstClientOrderInfo;

            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getClientOrderInfo()", "orderid=" + orderid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public List<usp_client_getClientCandidateOrderResultV2> getClientCandidateOrderBundleV2(int orderid)
        {
            List<usp_client_getClientCandidateOrderResultV2> lstCandidateOrders = new List<usp_client_getClientCandidateOrderResultV2>();

            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = orderid;
                DataTable dtInfo = GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_client_getClientCandidateOrderBundleV2", objParam);

                if (dtInfo != null)
                {
                    for (int i = 0; i < dtInfo.Rows.Count; i++)
                    {
                        usp_client_getClientCandidateOrderResultV2 lst = new usp_client_getClientCandidateOrderResultV2();

                        lst.candidateid = dtInfo.Rows[i][0].ToString();
                        lst.firstname = dtInfo.Rows[i][1].ToString();
                        lst.lastname = dtInfo.Rows[i][2].ToString();
                        lst.email = dtInfo.Rows[i][3].ToString();
                        lst.languagename = dtInfo.Rows[i][4].ToString();
                        lst.retest = Convert.ToBoolean(dtInfo.Rows[i][5]);
                        lst.opi_timerangefrom1 = Convert.ToDateTime(dtInfo.Rows[i][6]);
                        lst.opi_timerangeto1 = Convert.ToDateTime(dtInfo.Rows[i][7]);
                        lst.opi_timerangefrom2 = Convert.ToDateTime(dtInfo.Rows[i][8]);
                        lst.opi_timerangeto2 = Convert.ToDateTime(dtInfo.Rows[i][9]);
                        lst.opi_timerangefrom3 = Convert.ToDateTime(dtInfo.Rows[i][10]);
                        lst.opi_timerangeto3 = Convert.ToDateTime(dtInfo.Rows[i][11]);
                        lst.testlevel = dtInfo.Rows[i][12].ToString();
                        lst.testlevel2 = dtInfo.Rows[i][13].ToString();
                        lst.wpt_modality = dtInfo.Rows[i][14].ToString();
                        lst.login = dtInfo.Rows[i][15].ToString();
                        lst.password = dtInfo.Rows[i][16].ToString();
                        lst.cf_1_value = dtInfo.Rows[i][17].ToString();
                        lst.cf_1_enabled = Convert.ToBoolean(dtInfo.Rows[i][18]);
                        lst.cf_1_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][19]);
                        lst.cf_1_title = dtInfo.Rows[i][20].ToString();
                        lst.cf_2_value = dtInfo.Rows[i][21].ToString();
                        lst.cf_2_enabled = Convert.ToBoolean(dtInfo.Rows[i][22]);
                        lst.cf_2_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][23]);
                        lst.cf_2_title = dtInfo.Rows[i][24].ToString();
                        lst.cf_3_value = dtInfo.Rows[i][25].ToString();
                        lst.cf_3_enabled = Convert.ToBoolean(dtInfo.Rows[i][26]);
                        lst.cf_3_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][27]);
                        lst.cf_3_title = dtInfo.Rows[i][28].ToString();
                        lst.cf_4_value = dtInfo.Rows[i][29].ToString();
                        lst.cf_4_enabled = Convert.ToBoolean(dtInfo.Rows[i][30]);
                        lst.cf_4_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][31]);
                        lst.cf_4_title = dtInfo.Rows[i][32].ToString();
                        lst.cf_5_value = dtInfo.Rows[i][33].ToString();
                        lst.cf_5_enabled = Convert.ToBoolean(dtInfo.Rows[i][34]);
                        lst.cf_5_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][35]);
                        lst.cf_5_title = dtInfo.Rows[i][36].ToString();
                        lst.cf_6_value = dtInfo.Rows[i][37].ToString();
                        lst.cf_6_enabled = Convert.ToBoolean(dtInfo.Rows[i][38]);
                        lst.cf_6_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][39]);
                        lst.cf_6_title = dtInfo.Rows[i][40].ToString();
                        lst.cf_7_value = dtInfo.Rows[i][41].ToString();
                        lst.cf_7_enabled = Convert.ToBoolean(dtInfo.Rows[i][42]);
                        lst.cf_7_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][43]);
                        lst.cf_7_title = dtInfo.Rows[i][44].ToString();
                        lst.cf_8_value = dtInfo.Rows[i][45].ToString();
                        lst.cf_8_enabled = Convert.ToBoolean(dtInfo.Rows[i][46]);
                        lst.cf_8_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][47]);
                        lst.cf_8_title = dtInfo.Rows[i][48].ToString();
                        lst.cf_9_value = dtInfo.Rows[i][49].ToString();
                        lst.cf_9_enabled = Convert.ToBoolean(dtInfo.Rows[i][50]);
                        lst.cf_9_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][51]);
                        lst.cf_9_title = dtInfo.Rows[i][52].ToString();
                        lst.cf_10_value = dtInfo.Rows[i][53].ToString();
                        lst.cf_10_enabled = Convert.ToBoolean(dtInfo.Rows[i][54]);
                        lst.cf_10_isEncrypted = Convert.ToBoolean(dtInfo.Rows[i][55]);
                        lst.cf_10_title = dtInfo.Rows[i][56].ToString();
                        lst.clienttestname = dtInfo.Rows[i][57].ToString();
                        lst.Proctorless = Convert.ToBoolean(dtInfo.Rows[i][58]);
                        lst.SpeakerCode = Convert.ToInt32(dtInfo.Rows[i][59]);
                        lst.TestStatus = dtInfo.Rows[i][60].ToString();
                        lst.ProductType = Convert.ToInt32(dtInfo.Rows[i][61]);
                        lst.TestDelivery = Convert.ToInt32(dtInfo.Rows[i][62]);
                        lst.RatingCertificationName = dtInfo.Rows[i][63].ToString();
                        lst.ExpirationDate = dtInfo.Rows[i][64].ToString();
                        lst.NumSlots = Convert.ToInt32(dtInfo.Rows[i][65]);
                        lst.IsDefaultForm = Convert.ToInt32(dtInfo.Rows[i][66]);
                        lst.InstructionLang = dtInfo.Rows[i][67].ToString();
                        lst.ClientName = dtInfo.Rows[i][68].ToString();
                        lst.PromptGeneration = Convert.ToInt32(dtInfo.Rows[i][69].ToString());
                        lst.TOPIC_Status = dtInfo.Rows[i][70].ToString();
                        lst.testid = Convert.ToInt32(dtInfo.Rows[i][71]);

                        lstCandidateOrders.Add(lst);
                    }

                }

                return lstCandidateOrders;
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getClientCandidateOrderBundleV2()", "orderid = " + orderid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        private DataTable GetWorkflowEmailTemplateByTemplateId(int templateId)
        {
            try
            {
                Object[] objParam = new Object[1];
                objParam[0] = templateId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_tools_WorkflowEmail_GetEmailTemplateByTemplateId", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "usp_tools_WorkflowEmail_GetEmailTemplateByTemplateId", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public WorkflowEmailTemplateModel GetEmailTemplateByTemplateId(int templateId)
        {
            WorkflowEmailTemplateModel emailTemplate = new WorkflowEmailTemplateModel();
            var dataTable = GetWorkflowEmailTemplateByTemplateId(templateId);
            try
            {
                //There should only be one row 
                var row = dataTable.Rows[0];
                emailTemplate.TemplateId = (int)row["TemplateId"];
                emailTemplate.TemplateTypeId = (int)row["TemplateTypeId"];
                emailTemplate.TemplateTypeName = (row["TemplateTypeName"] == DBNull.Value ? "" : (string)row["TemplateTypeName"]);
                emailTemplate.TemplateName = (row["TemplateName"] == DBNull.Value ? "" : (string)row["TemplateName"]);
                emailTemplate.EmailSubject = (row["EmailSubject"] == DBNull.Value ? "" : (string)row["EmailSubject"]);
                emailTemplate.EmailTemplate =
                    (row["EmailTemplate"] == DBNull.Value ? "" : (string)row["EmailTemplate"]);
                emailTemplate.ModifiedBy = (row["ModifiedBy"] == DBNull.Value ? "" : (string)row["ModifiedBy"]);
                emailTemplate.ModifiedOn = (row["ModifiedOn"] == DBNull.Value)
                    ? ""
                    : ((DateTime)row["ModifiedOn"]).ToString("MM/dd/yyyy hh:mmtt");
            }
            catch (Exception)
            {
                return null;
            }
            return emailTemplate;
        }

        public int getFormIDForWPT(int clientid, string username)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = clientid;
                objParam[1] = username;
                int formid = (int)GlobalUtils.DBMgmt.ExecuteScalar(GetConnectionString(), "usp_client_getFormIDForWPT", objParam);
                return formid;
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "getFormIDForWPT()", "clientid = " + clientid);
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable B2B_TestRequest(int intB2BRowId, int intOrderId)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = intB2BRowId;
                objParam[1] = intOrderId;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_TestRequest", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "B2B_TestRequest()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable B2B_TestsTobeScheduled(int intB2BRowId, string strEmailID)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = intB2BRowId;
                objParam[1] = strEmailID;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_TestsToBeScheduled", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "B2B_TestsTobeScheduled()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable B2B_TestsScheduled(int intB2BRowId, string strEmailID)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = intB2BRowId;
                objParam[1] = strEmailID;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_TestsScheduled", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "B2B_TestsScheduled()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable B2B_GetAppointmentId(int intOrderId, string strLoginId, string strPassword, int intProductType, string strLanguage)
        {
            try
            {
                Object[] objParam = new Object[5];
                objParam[0] = intOrderId;
                objParam[1] = strLoginId;
                objParam[2] = strPassword;
                objParam[3] = intProductType;
                objParam[4] = strLanguage;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BCandidateRegistration_GetApptId", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "GetB2BAppointmentId()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

        public DataTable B2B_GetInstructions(int intAppointmentId, string strEmailID)
        {
            try
            {
                Object[] objParam = new Object[2];
                objParam[0] = intAppointmentId;
                objParam[1] = strEmailID;
                return GlobalUtils.DBMgmt.ExecuteDataTable(GetConnectionString(), "usp_B2BGetCandidateInstruction", objParam);
            }
            catch (Exception ee)
            {
                ErrorLogWrite(appName, ee.Message, "", "", "", "B2B_GetInstructions()", "");
                throw new ArgumentException(ee.Message.ToString());
            }
        }

    }

    public class usp_client_getClientDetailedInfoResult
    {
        public string ApplTemplate { get; set; }
        public string countryname { get; set; }
        public string email { get; set; }
        public string ext { get; set; }
        public string firstname { get; set; }
        public bool IsPrePostFieldsRequired { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public int? timezone { get; set; }
        public string userphone { get; set; }

    }

    public class usp_client_getClientOrderInfoResult
    {
        public string ClientName { get; set; }
        public string Country { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProctorEmail { get; set; }
        public string ProctorName { get; set; }
        public string ProctorPhone { get; set; }
        public string requesteremail { get; set; }
        public string requestername { get; set; }
        public string requesterphone { get; set; }
        public string requestphoneext { get; set; }
        public string TimeZoneCode { get; set; }

        public string additionalEmails { get; set; }

        public bool cf_1_enabled { get; set; }
        public bool cf_1_isEncrypted { get; set; }
        public string cf_1_title { get; set; }
        public string cf_1_value { get; set; }
        public bool cf_10_enabled { get; set; }
        public bool cf_10_isEncrypted { get; set; }
        public string cf_10_title { get; set; }
        public string cf_10_value { get; set; }
        public bool cf_2_enabled { get; set; }
        public bool cf_2_isEncrypted { get; set; }
        public string cf_2_title { get; set; }
        public string cf_2_value { get; set; }
        public bool cf_3_enabled { get; set; }
        public bool cf_3_isEncrypted { get; set; }
        public string cf_3_title { get; set; }
        public string cf_3_value { get; set; }
        public bool cf_4_enabled { get; set; }
        public bool cf_4_isEncrypted { get; set; }
        public string cf_4_title { get; set; }
        public string cf_4_value { get; set; }
        public bool cf_5_enabled { get; set; }
        public bool cf_5_isEncrypted { get; set; }
        public string cf_5_title { get; set; }
        public string cf_5_value { get; set; }
        public bool cf_6_enabled { get; set; }
        public bool cf_6_isEncrypted { get; set; }
        public string cf_6_title { get; set; }
        public string cf_6_value { get; set; }
        public bool cf_7_enabled { get; set; }
        public bool cf_7_isEncrypted { get; set; }
        public string cf_7_title { get; set; }
        public string cf_7_value { get; set; }
        public bool cf_8_enabled { get; set; }
        public bool cf_8_isEncrypted { get; set; }
        public string cf_8_title { get; set; }
        public string cf_8_value { get; set; }
        public bool cf_9_enabled { get; set; }
        public bool cf_9_isEncrypted { get; set; }
        public string cf_9_title { get; set; }
        public string cf_9_value { get; set; }
    }

    public partial class usp_client_getClientCandidateOrderResultV2
    {

        private string _candidateid;

        private string _firstname;

        private string _lastname;

        private string _email;

        private string _languagename;

        private System.Nullable<bool> _retest;

        private System.Nullable<System.DateTime> _opi_timerangefrom1;

        private System.Nullable<System.DateTime> _opi_timerangeto1;

        private System.Nullable<System.DateTime> _opi_timerangefrom2;

        private System.Nullable<System.DateTime> _opi_timerangeto2;

        private System.Nullable<System.DateTime> _opi_timerangefrom3;

        private System.Nullable<System.DateTime> _opi_timerangeto3;

        private string _testlevel;

        private string _testlevel2;

        private string _wpt_modality;

        private string _login;

        private string _password;

        private string _cf_1_value;

        private bool _cf_1_enabled;

        private bool _cf_1_isEncrypted;

        private string _cf_1_title;

        private string _cf_2_value;

        private bool _cf_2_enabled;

        private bool _cf_2_isEncrypted;

        private string _cf_2_title;

        private string _cf_3_value;

        private bool _cf_3_enabled;

        private bool _cf_3_isEncrypted;

        private string _cf_3_title;

        private string _cf_4_value;

        private bool _cf_4_enabled;

        private bool _cf_4_isEncrypted;

        private string _cf_4_title;

        private string _cf_5_value;

        private bool _cf_5_enabled;

        private bool _cf_5_isEncrypted;

        private string _cf_5_title;

        private string _cf_6_value;

        private bool _cf_6_enabled;

        private bool _cf_6_isEncrypted;

        private string _cf_6_title;

        private string _cf_7_value;

        private bool _cf_7_enabled;

        private bool _cf_7_isEncrypted;

        private string _cf_7_title;

        private string _cf_8_value;

        private bool _cf_8_enabled;

        private bool _cf_8_isEncrypted;

        private string _cf_8_title;

        private string _cf_9_value;

        private bool _cf_9_enabled;

        private bool _cf_9_isEncrypted;

        private string _cf_9_title;

        private string _cf_10_value;

        private bool _cf_10_enabled;

        private bool _cf_10_isEncrypted;

        private string _cf_10_title;

        private string _clienttestname;

        private System.Nullable<bool> _Proctorless;

        private System.Nullable<int> _SpeakerCode;

        private string _TestStatus;

        private System.Nullable<int> _ProductType;

        private System.Nullable<int> _TestDelivery;

        private string _RatingCertificationName;

        private string _ExpirationDate;

        private int _NumSlots;

        private System.Nullable<int> _IsDefaultForm;

        private string _InstructionLang;

        private string _clientname;

        private int _promptgeneration;

        private string _topic_status;

        private int _testid;

        public usp_client_getClientCandidateOrderResultV2()
        {
        }


        public string candidateid
        {
            get
            {
                return this._candidateid;
            }
            set
            {
                if ((this._candidateid != value))
                {
                    this._candidateid = value;
                }
            }
        }


        public string firstname
        {
            get
            {
                return this._firstname;
            }
            set
            {
                if ((this._firstname != value))
                {
                    this._firstname = value;
                }
            }
        }


        public string lastname
        {
            get
            {
                return this._lastname;
            }
            set
            {
                if ((this._lastname != value))
                {
                    this._lastname = value;
                }
            }
        }


        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                if ((this._email != value))
                {
                    this._email = value;
                }
            }
        }


        public string languagename
        {
            get
            {
                return this._languagename;
            }
            set
            {
                if ((this._languagename != value))
                {
                    this._languagename = value;
                }
            }
        }


        public System.Nullable<bool> retest
        {
            get
            {
                return this._retest;
            }
            set
            {
                if ((this._retest != value))
                {
                    this._retest = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangefrom1
        {
            get
            {
                return this._opi_timerangefrom1;
            }
            set
            {
                if ((this._opi_timerangefrom1 != value))
                {
                    this._opi_timerangefrom1 = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangeto1
        {
            get
            {
                return this._opi_timerangeto1;
            }
            set
            {
                if ((this._opi_timerangeto1 != value))
                {
                    this._opi_timerangeto1 = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangefrom2
        {
            get
            {
                return this._opi_timerangefrom2;
            }
            set
            {
                if ((this._opi_timerangefrom2 != value))
                {
                    this._opi_timerangefrom2 = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangeto2
        {
            get
            {
                return this._opi_timerangeto2;
            }
            set
            {
                if ((this._opi_timerangeto2 != value))
                {
                    this._opi_timerangeto2 = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangefrom3
        {
            get
            {
                return this._opi_timerangefrom3;
            }
            set
            {
                if ((this._opi_timerangefrom3 != value))
                {
                    this._opi_timerangefrom3 = value;
                }
            }
        }


        public System.Nullable<System.DateTime> opi_timerangeto3
        {
            get
            {
                return this._opi_timerangeto3;
            }
            set
            {
                if ((this._opi_timerangeto3 != value))
                {
                    this._opi_timerangeto3 = value;
                }
            }
        }


        public string testlevel
        {
            get
            {
                return this._testlevel;
            }
            set
            {
                if ((this._testlevel != value))
                {
                    this._testlevel = value;
                }
            }
        }


        public string testlevel2
        {
            get
            {
                return this._testlevel2;
            }
            set
            {
                if ((this._testlevel2 != value))
                {
                    this._testlevel2 = value;
                }
            }
        }


        public string wpt_modality
        {
            get
            {
                return this._wpt_modality;
            }
            set
            {
                if ((this._wpt_modality != value))
                {
                    this._wpt_modality = value;
                }
            }
        }


        public string login
        {
            get
            {
                return this._login;
            }
            set
            {
                if ((this._login != value))
                {
                    this._login = value;
                }
            }
        }


        public string password
        {
            get
            {
                return this._password;
            }
            set
            {
                if ((this._password != value))
                {
                    this._password = value;
                }
            }
        }


        public string cf_1_value
        {
            get
            {
                return this._cf_1_value;
            }
            set
            {
                if ((this._cf_1_value != value))
                {
                    this._cf_1_value = value;
                }
            }
        }


        public bool cf_1_enabled
        {
            get
            {
                return this._cf_1_enabled;
            }
            set
            {
                if ((this._cf_1_enabled != value))
                {
                    this._cf_1_enabled = value;
                }
            }
        }


        public bool cf_1_isEncrypted
        {
            get
            {
                return this._cf_1_isEncrypted;
            }
            set
            {
                if ((this._cf_1_isEncrypted != value))
                {
                    this._cf_1_isEncrypted = value;
                }
            }
        }


        public string cf_1_title
        {
            get
            {
                return this._cf_1_title;
            }
            set
            {
                if ((this._cf_1_title != value))
                {
                    this._cf_1_title = value;
                }
            }
        }

        public string cf_2_value
        {
            get
            {
                return this._cf_2_value;
            }
            set
            {
                if ((this._cf_2_value != value))
                {
                    this._cf_2_value = value;
                }
            }
        }


        public bool cf_2_enabled
        {
            get
            {
                return this._cf_2_enabled;
            }
            set
            {
                if ((this._cf_2_enabled != value))
                {
                    this._cf_2_enabled = value;
                }
            }
        }

        public bool cf_2_isEncrypted
        {
            get
            {
                return this._cf_2_isEncrypted;
            }
            set
            {
                if ((this._cf_2_isEncrypted != value))
                {
                    this._cf_2_isEncrypted = value;
                }
            }
        }


        public string cf_2_title
        {
            get
            {
                return this._cf_2_title;
            }
            set
            {
                if ((this._cf_2_title != value))
                {
                    this._cf_2_title = value;
                }
            }
        }


        public string cf_3_value
        {
            get
            {
                return this._cf_3_value;
            }
            set
            {
                if ((this._cf_3_value != value))
                {
                    this._cf_3_value = value;
                }
            }
        }


        public bool cf_3_enabled
        {
            get
            {
                return this._cf_3_enabled;
            }
            set
            {
                if ((this._cf_3_enabled != value))
                {
                    this._cf_3_enabled = value;
                }
            }
        }


        public bool cf_3_isEncrypted
        {
            get
            {
                return this._cf_3_isEncrypted;
            }
            set
            {
                if ((this._cf_3_isEncrypted != value))
                {
                    this._cf_3_isEncrypted = value;
                }
            }
        }


        public string cf_3_title
        {
            get
            {
                return this._cf_3_title;
            }
            set
            {
                if ((this._cf_3_title != value))
                {
                    this._cf_3_title = value;
                }
            }
        }


        public string cf_4_value
        {
            get
            {
                return this._cf_4_value;
            }
            set
            {
                if ((this._cf_4_value != value))
                {
                    this._cf_4_value = value;
                }
            }
        }


        public bool cf_4_enabled
        {
            get
            {
                return this._cf_4_enabled;
            }
            set
            {
                if ((this._cf_4_enabled != value))
                {
                    this._cf_4_enabled = value;
                }
            }
        }


        public bool cf_4_isEncrypted
        {
            get
            {
                return this._cf_4_isEncrypted;
            }
            set
            {
                if ((this._cf_4_isEncrypted != value))
                {
                    this._cf_4_isEncrypted = value;
                }
            }
        }


        public string cf_4_title
        {
            get
            {
                return this._cf_4_title;
            }
            set
            {
                if ((this._cf_4_title != value))
                {
                    this._cf_4_title = value;
                }
            }
        }


        public string cf_5_value
        {
            get
            {
                return this._cf_5_value;
            }
            set
            {
                if ((this._cf_5_value != value))
                {
                    this._cf_5_value = value;
                }
            }
        }


        public bool cf_5_enabled
        {
            get
            {
                return this._cf_5_enabled;
            }
            set
            {
                if ((this._cf_5_enabled != value))
                {
                    this._cf_5_enabled = value;
                }
            }
        }


        public bool cf_5_isEncrypted
        {
            get
            {
                return this._cf_5_isEncrypted;
            }
            set
            {
                if ((this._cf_5_isEncrypted != value))
                {
                    this._cf_5_isEncrypted = value;
                }
            }
        }


        public string cf_5_title
        {
            get
            {
                return this._cf_5_title;
            }
            set
            {
                if ((this._cf_5_title != value))
                {
                    this._cf_5_title = value;
                }
            }
        }


        public string cf_6_value
        {
            get
            {
                return this._cf_6_value;
            }
            set
            {
                if ((this._cf_6_value != value))
                {
                    this._cf_6_value = value;
                }
            }
        }


        public bool cf_6_enabled
        {
            get
            {
                return this._cf_6_enabled;
            }
            set
            {
                if ((this._cf_6_enabled != value))
                {
                    this._cf_6_enabled = value;
                }
            }
        }


        public bool cf_6_isEncrypted
        {
            get
            {
                return this._cf_6_isEncrypted;
            }
            set
            {
                if ((this._cf_6_isEncrypted != value))
                {
                    this._cf_6_isEncrypted = value;
                }
            }
        }


        public string cf_6_title
        {
            get
            {
                return this._cf_6_title;
            }
            set
            {
                if ((this._cf_6_title != value))
                {
                    this._cf_6_title = value;
                }
            }
        }


        public string cf_7_value
        {
            get
            {
                return this._cf_7_value;
            }
            set
            {
                if ((this._cf_7_value != value))
                {
                    this._cf_7_value = value;
                }
            }
        }


        public bool cf_7_enabled
        {
            get
            {
                return this._cf_7_enabled;
            }
            set
            {
                if ((this._cf_7_enabled != value))
                {
                    this._cf_7_enabled = value;
                }
            }
        }


        public bool cf_7_isEncrypted
        {
            get
            {
                return this._cf_7_isEncrypted;
            }
            set
            {
                if ((this._cf_7_isEncrypted != value))
                {
                    this._cf_7_isEncrypted = value;
                }
            }
        }

        public string cf_7_title
        {
            get
            {
                return this._cf_7_title;
            }
            set
            {
                if ((this._cf_7_title != value))
                {
                    this._cf_7_title = value;
                }
            }
        }


        public string cf_8_value
        {
            get
            {
                return this._cf_8_value;
            }
            set
            {
                if ((this._cf_8_value != value))
                {
                    this._cf_8_value = value;
                }
            }
        }


        public bool cf_8_enabled
        {
            get
            {
                return this._cf_8_enabled;
            }
            set
            {
                if ((this._cf_8_enabled != value))
                {
                    this._cf_8_enabled = value;
                }
            }
        }


        public bool cf_8_isEncrypted
        {
            get
            {
                return this._cf_8_isEncrypted;
            }
            set
            {
                if ((this._cf_8_isEncrypted != value))
                {
                    this._cf_8_isEncrypted = value;
                }
            }
        }


        public string cf_8_title
        {
            get
            {
                return this._cf_8_title;
            }
            set
            {
                if ((this._cf_8_title != value))
                {
                    this._cf_8_title = value;
                }
            }
        }


        public string cf_9_value
        {
            get
            {
                return this._cf_9_value;
            }
            set
            {
                if ((this._cf_9_value != value))
                {
                    this._cf_9_value = value;
                }
            }
        }


        public bool cf_9_enabled
        {
            get
            {
                return this._cf_9_enabled;
            }
            set
            {
                if ((this._cf_9_enabled != value))
                {
                    this._cf_9_enabled = value;
                }
            }
        }


        public bool cf_9_isEncrypted
        {
            get
            {
                return this._cf_9_isEncrypted;
            }
            set
            {
                if ((this._cf_9_isEncrypted != value))
                {
                    this._cf_9_isEncrypted = value;
                }
            }
        }


        public string cf_9_title
        {
            get
            {
                return this._cf_9_title;
            }
            set
            {
                if ((this._cf_9_title != value))
                {
                    this._cf_9_title = value;
                }
            }
        }


        public string cf_10_value
        {
            get
            {
                return this._cf_10_value;
            }
            set
            {
                if ((this._cf_10_value != value))
                {
                    this._cf_10_value = value;
                }
            }
        }


        public bool cf_10_enabled
        {
            get
            {
                return this._cf_10_enabled;
            }
            set
            {
                if ((this._cf_10_enabled != value))
                {
                    this._cf_10_enabled = value;
                }
            }
        }


        public bool cf_10_isEncrypted
        {
            get
            {
                return this._cf_10_isEncrypted;
            }
            set
            {
                if ((this._cf_10_isEncrypted != value))
                {
                    this._cf_10_isEncrypted = value;
                }
            }
        }


        public string cf_10_title
        {
            get
            {
                return this._cf_10_title;
            }
            set
            {
                if ((this._cf_10_title != value))
                {
                    this._cf_10_title = value;
                }
            }
        }

        public string clienttestname
        {
            get
            {
                return this._clienttestname;
            }
            set
            {
                if ((this._clienttestname != value))
                {
                    this._clienttestname = value;
                }
            }
        }


        public System.Nullable<bool> Proctorless
        {
            get
            {
                return this._Proctorless;
            }
            set
            {
                if ((this._Proctorless != value))
                {
                    this._Proctorless = value;
                }
            }
        }


        public System.Nullable<int> SpeakerCode
        {
            get
            {
                return this._SpeakerCode;
            }
            set
            {
                if ((this._SpeakerCode != value))
                {
                    this._SpeakerCode = value;
                }
            }
        }


        public string TestStatus
        {
            get
            {
                return this._TestStatus;
            }
            set
            {
                if ((this._TestStatus != value))
                {
                    this._TestStatus = value;
                }
            }
        }


        public System.Nullable<int> ProductType
        {
            get
            {
                return this._ProductType;
            }
            set
            {
                if ((this._ProductType != value))
                {
                    this._ProductType = value;
                }
            }
        }


        public System.Nullable<int> TestDelivery
        {
            get
            {
                return this._TestDelivery;
            }
            set
            {
                if ((this._TestDelivery != value))
                {
                    this._TestDelivery = value;
                }
            }
        }


        public string RatingCertificationName
        {
            get
            {
                return this._RatingCertificationName;
            }
            set
            {
                if ((this._RatingCertificationName != value))
                {
                    this._RatingCertificationName = value;
                }
            }
        }


        public string ExpirationDate
        {
            get
            {
                return this._ExpirationDate;
            }
            set
            {
                if ((this._ExpirationDate != value))
                {
                    this._ExpirationDate = value;
                }
            }
        }


        public int NumSlots
        {
            get
            {
                return this._NumSlots;
            }
            set
            {
                if ((this._NumSlots != value))
                {
                    this._NumSlots = value;
                }
            }
        }


        public System.Nullable<int> IsDefaultForm
        {
            get
            {
                return this._IsDefaultForm;
            }
            set
            {
                if ((this._IsDefaultForm != value))
                {
                    this._IsDefaultForm = value;
                }
            }
        }


        public string InstructionLang
        {
            get
            {
                return this._InstructionLang;
            }
            set
            {
                if ((this._InstructionLang != value))
                {
                    this._InstructionLang = value;
                }
            }
        }

        public string ClientName
        {
            get
            {
                return this._clientname;
            }
            set
            {
                if ((this._clientname != value))
                {
                    this._clientname = value;
                }
            }
        }

        public int PromptGeneration
        {
            get
            {
                return this._promptgeneration;
            }
            set
            {
                if ((this._promptgeneration != value))
                {
                    this._promptgeneration = value;
                }
            }
        }

        public string TOPIC_Status
        {
            get
            {
                return this._topic_status;
            }
            set
            {
                if ((this._topic_status != value))
                {
                    this._topic_status = value;
                }
            }
        }

        public int testid
        {
            get
            {
                return this._testid;
            }
            set
            {
                if ((this._testid != value))
                {
                    this._testid = value;
                }
            }
        }
    }

    public class WorkflowEmailTemplateModel
    {
        public int TemplateId { get; set; }
        public int TemplateTypeId { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTemplate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Placeholders { get; set; }
    }

    public enum EmailTemplates
    {
        #region TestRequestResults.aspx.cs
        //SendEmail()
        EmailTestRequestCLSTemplate = 152,
        EmailTestRequestTemplate = 153,
        OPIProctorlessSpeakerReco = 154,
        //SendEmailToCandidate()
        EmailTestRequestScheduledTelOPIcTemplate = 155,
        EmailTestRequestScheduledWPTTemplate_PayPal = 156,
        EmailTestRequestScheduledWPTTemplate_Marriott = 157,
        EmailTestRequestScheduledWPTTemplate_WestJet = 158,
        EmailTestRequestScheduledWPTTemplate_195 = 159,
        EmailTestRequestScheduledWPTTemplate = 160,
        EmailTestRequestScheduledOPIcTemplate_Marriott = 161,
        EmailTestRequestScheduledOPIcTemplate_PayPal = 162,
        EmailTestRequestScheduledOPIcTemplate_WestJet = 163,
        EmailTestRequestScheduledOPIcTemplate = 164,
        #endregion

        #region TestRequestResults_topic.aspx.cs
        //SendEmailToCAndidate() && btnSendEmail_Click()
        EmailTestRequestScheduledTelOPIcTemplate_Verizon = 165,
        EmailTestRequestScheduledTelOPIcTemplate_PNC = 166,
        EmailTestRequestScheduledTelOPIcTemplate_ATTINT = 167,
        EmailTestRequestScheduledTelOPIcTemplateATTRSC = 168,
        //EmailTestRequestScheduledTelOPIcTemplate, //Already on TestRequestResults.aspx.cs
        #endregion     
     
    }

}