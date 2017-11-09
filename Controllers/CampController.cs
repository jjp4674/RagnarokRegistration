using DotNetNuke.Common.Utilities;
using Ragnarok.Modules.RagnarokRegistration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using AuthorizeNet;
using AuthorizeNet.Api;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;
using QRCoder;

namespace Ragnarok.Modules.RagnarokRegistration.Controllers
{
    public class CampController
    {
        private const string PROCEDURE_GET_CAMPS = "GetCamps";
        private const string PROCEDURE_GET_CAMP = "GetCamp";
        private const string PROCEDURE_ADD_REGISTRATION = "AddRegistration";
        private const string PROCEDURE_ADD_HEALTH_ISSUE = "AddHealthIssue";
        private const string PROCEDURE_ADD_ERROR = "AddError";

        /// <summary>
        /// Gets the list of camps for the registration page.
        /// </summary>
        /// <returns>A list of camps.</returns>
        public IList<Camp> GetCamps()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_GET_CAMPS;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        // throw exception
                    }

                    var camps = new List<Camp>();

                    while (reader.Read())
                    {
                        Camp camp = new Camp();
                        camp.Id = Convert.ToInt32(reader["Id"].ToString());
                        camp.CampName = reader["CampName"].ToString();
                        camp.CampLocation = reader["CampLocation"].ToString();

                        CampMaster campMaster = new CampMaster();
                        campMaster.FirstName = reader["FirstName"].ToString();
                        campMaster.LastName = reader["LastName"].ToString();

                        camp.CampMaster = campMaster;

                        camps.Add(camp);
                    }

                    return camps;
                }
            }
        }

        public void AddRegistration(Registration registration)
        {
            var response = HandlePayment(registration.Participant.Payment, registration.Participant);
            int participantId = 0;

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_c_id", registration.Participant.CampId);
                cmd.Parameters.AddWithValue("@par_FirstName", registration.Participant.FirstName);
                cmd.Parameters.AddWithValue("@par_LastName", registration.Participant.LastName);
                cmd.Parameters.AddWithValue("@par_DOB", registration.Participant.DateOfBirth);
                cmd.Parameters.AddWithValue("@par_CharacterName", registration.Participant.CharacterName);
                cmd.Parameters.AddWithValue("@par_ChapterName", registration.Participant.ChapterName);
                cmd.Parameters.AddWithValue("@par_UnitName", registration.Participant.UnitName);
                cmd.Parameters.AddWithValue("@par_IsMinor", registration.Participant.IsMinor);
                cmd.Parameters.AddWithValue("@par_IsMerchant", registration.Participant.IsMerchant);
                if (registration.Participant.RegistrationDate != null)
                {
                    cmd.Parameters.AddWithValue("@par_RegDate", (DateTime)registration.Participant.RegistrationDate);
                }

                // Payment
                if (response.transactionResponse != null)
                {
                    cmd.Parameters.AddWithValue("@pm_AuthCode", response.transactionResponse.authCode);
                    cmd.Parameters.AddWithValue("@pm_TransId", response.transactionResponse.transId);
                    cmd.Parameters.AddWithValue("@pm_Amount", registration.Participant.Payment.Amount);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pm_AuthCode", "");
                    cmd.Parameters.AddWithValue("@pm_TransId", "");
                    cmd.Parameters.AddWithValue("@pm_Amount", registration.Participant.Payment.Amount);
                }

                // Address
                cmd.Parameters.AddWithValue("@add_Address1", registration.Participant.Address.Address1);
                cmd.Parameters.AddWithValue("@add_Address2", registration.Participant.Address.Address2);
                cmd.Parameters.AddWithValue("@add_City", registration.Participant.Address.City);
                cmd.Parameters.AddWithValue("@add_State", registration.Participant.Address.State);
                cmd.Parameters.AddWithValue("@add_Zip", registration.Participant.Address.Zip);

                // Contact Information
                cmd.Parameters.AddWithValue("@ci_HomePhone", registration.Participant.ContactInformation.HomePhone);
                cmd.Parameters.AddWithValue("@ci_CellPhone", registration.Participant.ContactInformation.CellPhone);
                cmd.Parameters.AddWithValue("@ci_Email", registration.Participant.ContactInformation.Email);

                // Emergency Contact Information
                cmd.Parameters.AddWithValue("@ec_Name", registration.Participant.EmergencyContact.Name);
                cmd.Parameters.AddWithValue("@ec_Phone", registration.Participant.EmergencyContact.Phone);

                // Signature
                cmd.Parameters.AddWithValue("@s_Signature", registration.Participant.Signature);

                conn.Open();

                participantId = (int)cmd.ExecuteScalar();
            }

            foreach (HealthIssue healthIssue in registration.Participant.HealthIssues)
            {
                using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = PROCEDURE_ADD_HEALTH_ISSUE;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@hi_par_id", participantId);
                    cmd.Parameters.AddWithValue("@hi_HealthIssue", healthIssue.Issue);

                    conn.Open();

                    cmd.ExecuteScalar();
                }
            }

            Camp regCamp = new Camp();
            if (registration.Participant.CampId != 0)
            {
                regCamp = GetCamp(registration.Participant.CampId);
            }
            else
            {
                regCamp.Id = 0;
            }

            SendConfirmationEmail(participantId, registration, regCamp);

            if (regCamp.Id != 0)
            {
                SendCampMasterEmail(registration, regCamp);
            }
        }

        public void AddError(string errorMessage)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_ERROR;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@err_ErrorMessage", errorMessage);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private createTransactionResponse HandlePayment(Payment payment, Participant participant)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
            {
                // Dev Account: name = "3QeAr3WX7z",
                name = "2B4f4zGR",
                ItemElementName = ItemChoiceType.transactionKey,
                //Item = "9f896x36Uy242FQf"
                Item = "5D3E9PK7Ms6p3Txr"
            };

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = payment.DataDescriptor, 
                dataValue = payment.DataValue
            };

            var paymentType = new paymentType { Item = opaqueData };

            var billToType = new customerAddressType
            {
                firstName = participant.FirstName, 
                lastName = participant.LastName
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(), 
                amount = payment.Amount, 
                payment = paymentType, 
                billTo = billToType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            

            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                return response;
            }
            else
            {
                throw new Exception("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text + " | Response: " + response.ToJson());
            }
        }

        private void SendConfirmationEmail(int participantId, Registration registration, Camp regCamp)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(registration.Participant.ContactInformation.Email));
            if (registration.Participant.IsMerchant == "Y")
            {
                mailMessage.Subject = "Ragnarok XXXII - Merchant Registration Confirmation";
            }
            else
            {
                mailMessage.Subject = "Ragnarok XXXII - Registration Confirmation";
            }
            mailMessage.IsBodyHtml = true;

            string server = hostSettings["SMTPServer"];
            string authentication = hostSettings["SMTPAuthentication"];
            string password = hostSettings["SMTPPassword"];
            string username = hostSettings["SMTPUsername"];

            string body = "<p><b>" + registration.Participant.CharacterName + "</b>,</p>";
            body += "<p>Thank you for registering to attend <b>Ragnarok XXXII</b>";
            if (registration.Participant.IsMerchant == "Y")
            {
                body += " as a merchant";
            }
            body += "!</p>";

            if (regCamp.Id != 0)
            {
                body += "<p>You are marked down as an attendee staying in <b>" + regCamp.CampName + "</b>. If your camp changes, please contact ";
                body += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to change your camp, or you can change it ";
                body += "when you arrive at Ragnarok.</p>";
            }
            else
            {
                body += "<p>You are marked down as not having a camp selected yet. If you choose a camp, please contact ";
                body += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to select your camp, or you can select your ";
                body += "camp when you arrive at Ragnarok.</p>";
            }

            if (registration.Participant.IsMerchant != "Y")
            {
                body += "<p>Your selected arrival date is <b>" + ((DateTime)registration.Participant.RegistrationDate).ToString("MM/dd/yyyy") + "</b>.</p>";
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://dagorhirragnarok.com/CheckIn.aspx?pid=" + participantId, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            MemoryStream qr = new MemoryStream(qrCodeImage);
            LinkedResource linkedQR = new LinkedResource(qr, "image/jpeg");
            linkedQR.ContentId = "qrCode";

            body += "<p>For expedited check-in at Troll when you arrive at Ragnarok, please present this QR Code to the Troll staff:</p>";
            body += "<center><p><img style=\"height: 300px; width: 300px;\" src=cid:qrCode /></p></center>";
            body += "<p>We look forward to seeing you at Ragnarok!</p>";
            body += "<p>----------------------------------------------<br />Troll</p>";

            AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);
            av.LinkedResources.Add(linkedQR);
            mailMessage.AlternateViews.Add(av);

            SmtpClient client = new SmtpClient(server);
            client.Send(mailMessage);
        }

        private void SendCampMasterEmail(Registration registration, Camp regCamp)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(regCamp.CampMaster.Email));
            mailMessage.Subject = "Ragnarok XXXII - Camp Attendee Registration Confirmation";
            mailMessage.IsBodyHtml = true;

            string server = hostSettings["SMTPServer"];
            string authentication = hostSettings["SMTPAuthentication"];
            string password = hostSettings["SMTPPassword"];
            string username = hostSettings["SMTPUsername"];

            string body = "<p><b>" + regCamp.CampMaster.FirstName + "</b>,</p>";
            body += "<p>An attendee has registered to attend <b>Ragnarok XXXII</b> as part of <b>" + regCamp.CampName + "</b>!</p>";
            body += "<p>Attendee Information<br />";
            if (registration.Participant.IsMerchant != "Y")
            {
                body += "Arrival Date: <b>" + ((DateTime)registration.Participant.RegistrationDate).ToString("MM/dd/yyyy") + "</b><br />";
            }
            body += "Name: <b>" + registration.Participant.FirstName + " " + registration.Participant.LastName + "</b><br />";
            body += "Character Name: <b>" + registration.Participant.CharacterName + "</b><br />";
            body += "Unit Name: <b>" + registration.Participant.UnitName + "</b><br />";
            body += "Chapter Name: <b>" + registration.Participant.ChapterName + "</b><br />";
            body += "Email Address: <b><a href=\"mailto:" + registration.Participant.ContactInformation.Email + "\">" + registration.Participant.ContactInformation.Email + "</a></b></p>";
            body += "<p>If this attendee should not be part of your camp, please contact them and/or <a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> ";
            body += "to resolve the issue.</p>";
            body += "<p>We look forward to seeing you at Ragnarok!</p>";
            body += "<p>----------------------------------------------<br />Troll</p>";

            mailMessage.Body = body;

            SmtpClient client = new SmtpClient(server);
            client.Send(mailMessage);
        }

        private Camp GetCamp(int campId)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_GET_CAMP;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@c_id", campId);

                conn.Open();
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        // throw exception
                    }

                    var camps = new List<Camp>();

                    while (reader.Read())
                    {
                        Camp camp = new Camp();
                        camp.Id = Convert.ToInt32(reader["Id"].ToString());
                        camp.CampName = reader["CampName"].ToString();
                        camp.CampLocation = reader["CampLocation"].ToString();

                        CampMaster campMaster = new CampMaster();
                        campMaster.FirstName = reader["FirstName"].ToString();
                        campMaster.LastName = reader["LastName"].ToString();
                        campMaster.Email = reader["Email"].ToString();

                        camp.CampMaster = campMaster;

                        camps.Add(camp);
                    }

                    return camps.FirstOrDefault();
                }
            }
        }
    }
}