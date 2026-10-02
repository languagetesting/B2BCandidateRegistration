using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Configuration;

namespace B2BCandidateRegistration
{
    public class Utility
    {
        public static int SendMails(string strFrom, string strTo, string strSubject, string strBody, int intToCnt, bool boolIsHtml, int intApptId)
        {
            return SendMails(strFrom, strTo, strSubject, strBody, intToCnt, boolIsHtml, null, "", intApptId);
        }

        public static int SendMails(string strFrom, string strTo, string strSubject, string strBody, int intToCnt, bool boolIsHtml, byte[] attachmentArry, string attachmentName, int intApptId)
        {
            //get email config info from config file
            string strHostName = System.Configuration.ConfigurationManager.AppSettings["Hostname"].ToString();
            string strEmailUserName = System.Configuration.ConfigurationManager.AppSettings["EmailUserName"].ToString();
            string strEmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"].ToString();

            string strLogoName = System.Configuration.ConfigurationManager.AppSettings["LTILogo"].ToString();            
            String strFileCompanyLogo = strLogoName;
            //set email properties
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            smtpClient.Host = strHostName;
            System.Net.NetworkCredential basicCrenntial = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCrenntial;

            String[] strToAdd = null;
            strToAdd = strTo.Split(',');
            intToCnt = strToAdd.Length;

            try
            {
                MailAddress fromAddress = new MailAddress(strFrom);
                message.From = fromAddress;

                //loop and add all recipients to the email
                for (int i = 0; i <= intToCnt - 1; i++)
                {
                    message.To.Add(strToAdd[i]);
                }
                message.Subject = strSubject;
                message.IsBodyHtml = boolIsHtml;

                if (strFileCompanyLogo != "")
                {
                    string contentID = Path.GetFileName(strFileCompanyLogo).Replace(".", "");
                    Attachment inline = new Attachment(strFileCompanyLogo);
                    inline.ContentDisposition.Inline = true;
                    inline.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;
                    inline.ContentId = contentID;
                    inline.ContentType.MediaType = "image/jpg";
                    inline.ContentType.Name = Path.GetFileName(strFileCompanyLogo);
                    strBody = strBody.Replace("cid:CompanyLogo", "cid:" + contentID);
                    message.Attachments.Add(inline);

                }

                AlternateView HtmlView = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                message.AlternateViews.Add(HtmlView);

                if (attachmentArry != null)
                {
                    MemoryStream mStream = new MemoryStream(attachmentArry);
                    Attachment attachment = new Attachment(mStream, attachmentName); //create the attachment
                    message.Attachments.Add(attachment);    //add the attachment
                }

                //send the email
                message.Body = strBody.ToString();
                smtpClient.Send(message);
                SaveEmailToDatabase(strFrom, strTo, strSubject, strBody, "B2BCandidateRegistration", intApptId);
                return 1;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static void SaveEmailToDatabase(string strFrom, string strTo, string strSubject, string strBody, string userid, int appointmentid)
        {
            SqlConnection con = null;
            int returnValue = 0;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LTI_TMSConnectionString"].ConnectionString;
                if (connectionString == null)
                    throw new Exception("Database connection string not defined in config file.");
                else
                    con = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand("usp_EmailLogInsert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MailFrom", SqlDbType.VarChar));
                cmd.Parameters[0].Value = strFrom;
                cmd.Parameters.Add(new SqlParameter("@MailTo", SqlDbType.VarChar));
                cmd.Parameters[1].Value = strTo;
                cmd.Parameters.Add(new SqlParameter("@MailSubject", SqlDbType.VarChar));
                cmd.Parameters[2].Value = strSubject;
                cmd.Parameters.Add(new SqlParameter("@MailBody", SqlDbType.VarChar));
                cmd.Parameters[3].Value = strBody;
                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar));
                cmd.Parameters[4].Value = userid;
                cmd.Parameters.Add(new SqlParameter("@AppointmentId", SqlDbType.Int));
                cmd.Parameters[5].Value = appointmentid;
                cmd.Parameters.Add(new SqlParameter("@DigitalSigned", SqlDbType.Int));
                cmd.Parameters[6].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@ReturnId", SqlDbType.Int));
                cmd.Parameters[7].Value = 0;
                cmd.Parameters[7].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                returnValue = (int)cmd.Parameters[6].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

    }
}