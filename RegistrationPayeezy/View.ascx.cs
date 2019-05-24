/*
' Copyright (c) 2016  DagorhirRagnarok.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using Ragnarok.Modules.RagnarokRegistration.Controllers;
using Ragnarok.Modules.RagnarokRegistration.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using System.Net.Mail;
using QRCoder;
using System.IO;
using System.Security.Authentication;
using System.Net;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Ragnarok.Modules.RagnarokRegistration.Payeezy
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from RagnarokRegistrationModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_CREATE_REGISTRATION = "CreateRegistration";
        private const string PROCEDURE_COMPLETE_REGISTRATION = "CompleteRegistration";
        private const string PROCEDURE_UPDATE_REGISTRATION = "UpdateRegistration";
        private const string PROCEDURE_ADD_HEALTH_ISSUE = "AddHealthIssue";
        private const string PROCEDURE_ADD_ERROR = "AddError";
        private const string PROCEDURE_GET_CAMP = "GetCamp";

        private const string REG_TYPE_VIEW_STATE = "regType";

        private string pageBody;
        private byte[] qrCodeBytes;

        //const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        //const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            if (!Page.IsPostBack)
            {
                GetCamps();
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        private void GetCamps()
        {
            CampController cc = new CampController();
            IList<Camp> camps = cc.GetCamps();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("CampName");

            dt.Rows.Add(new Object[] { "", "-- Select a Camp --" });
            dt.Rows.Add(new Object[] { "0", "Camp Unknown - Will Select Later" });
            dt.Rows.Add(new Object[] { "", "-------------------" });

            foreach (Camp camp in camps.OrderBy(c => c.CampName))
            {
                dt.Rows.Add(new Object[] { camp.Id.ToString(), camp.CampName +  " - " + camp.CampMaster.CharacterName });
            }

            ddlCamp.DataSource = dt;
            ddlCamp.DataTextField = "CampName";
            ddlCamp.DataValueField = "Id";
            ddlCamp.DataBind();
        }

        protected void btnNextParticipantInfo_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Type");
            if (Page.IsValid)
            {
                string type = GetSelectedType();
                List<RegistrationType> regTypes = PopulateRegistrationTypes();
                
                if (!string.IsNullOrWhiteSpace(type))
                {
                    RegistrationType selectedRegType = regTypes.Where(x => x.Text == type).FirstOrDefault();
                    WriteToViewState(REG_TYPE_VIEW_STATE, selectedRegType);

                    businessName.Visible = false;
                    if (selectedRegType.IsMerchant)
                    {
                        businessName.Visible = true;
                    }

                    pParticipantInfo.Visible = true;
                    pParticipantFooter.Visible = true;

                    pRegistrationInfo.Visible = false;
                    pRegistrationFooter.Visible = false;
                }
            }
        }

        protected void btnNextVolunteerInfo_Click(object sender, ImageClickEventArgs e)
        {
            pVolunteerInfo.Visible = true;
            pVolunteerFooter.Visible = true;

            pParticipantInfo.Visible = false;
            pParticipantFooter.Visible = false;
        }

        protected void btnNextCharacterInfo_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Participant");
            if (Page.IsValid)
            {
                RegistrationType regType = (RegistrationType)GetFromViewState(REG_TYPE_VIEW_STATE);
                //lblDebug.Text = "Text: " + regType.Text + " | Day: " + regType.Day + " | Date: " + regType.ArrivalDate + " | Cost: " + regType.Cost + " | Minor: " + regType.IsMinor + " | Merchant: " + regType.IsMerchant;

                unitName.Visible = true;
                if (regType.IsMerchant)
                {
                    unitName.Visible = false;
                }

                pCharacterInfo.Visible = true;
                pCharacterFooter.Visible = true;

                pVolunteerInfo.Visible = false;
                pVolunteerFooter.Visible = false;
            }
        }

        protected void btnNextEmergencyInfo_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Character");
            if (Page.IsValid)
            {
                pEmergencyInfo.Visible = true;
                pEmergencyFooter.Visible = true;

                pCharacterInfo.Visible = false;
                pCharacterFooter.Visible = false;
            }
        }

        protected void btnNextWaiverInfo_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Emergency");
            if (Page.IsValid)
            {
                RegistrationType regType = (RegistrationType)GetFromViewState(REG_TYPE_VIEW_STATE);
                if (regType.IsMinor)
                {
                    pPaymentInfo.Visible = true;
                    pPaymentFooter.Visible = true;
                }
                else
                {
                    pWaiverInfo.Visible = true;
                    pWaiverFooter.Visible = true;

                    string script = "showSignature();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
                }

                pEmergencyInfo.Visible = false;
                pEmergencyFooter.Visible = false;
            }
        }

        protected void btnNextPaymentInfo_Click(object sender, ImageClickEventArgs e)
        {
            bool valid = true;
            lblCheckboxError.Visible = false;
            lblSignatureError.Visible = false;

            if (!cbxAcceptActivities.Checked || !cbxAcceptOver18.Checked || !cbxAcceptRelease.Checked)
            {
                lblCheckboxError.Visible = true;
                valid = false;
            }

            if (string.IsNullOrEmpty(signatureCode.Value) || signatureCode.Value == "data:," || signatureCode.Value == "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAloAAADKCAYAAAB9ou6AAAAKEUlEQVR4Xu3WMREAAAgDMerfNCp+CwI65Bh+5wgQIECAAAECBBKBJatGCRAgQIAAAQIETmh5AgIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEBAaPkBAgQIECBAgEAkILQiWLMECBAgQIAAAaHlBwgQIECAAAECkYDQimDNEiBAgAABAgSElh8gQIAAAQIECEQCQiuCNUuAAAECBAgQEFp+gAABAgQIECAQCQitCNYsAQIECBAgQEBo+QECBAgQIECAQCQgtCJYswQIECBAgAABoeUHCBAgQIAAAQKRgNCKYM0SIECAAAECBISWHyBAgAABAgQIRAJCK4I1S4AAAQIECBAQWn6AAAECBAgQIBAJCK0I1iwBAgQIECBAQGj5AQIECBAgQIBAJCC0IlizBAgQIECAAAGh5QcIECBAgAABApGA0IpgzRIgQIAAAQIEhJYfIECAAAECBAhEAkIrgjVLgAABAgQIEBBafoAAAQIECBAgEAkIrQjWLAECBAgQIEDgAUHgAMs2/Ea2AAAAAElFTkSuQmCC")
            {
                lblSignatureError.Visible = true;
                valid = false;
            }

            if (valid)
            {
                pPaymentInfo.Visible = true;
                pPaymentFooter.Visible = true;

                pWaiverInfo.Visible = false;
                pWaiverFooter.Visible = false;

                string script = "hideSignature();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        protected void btnPreviousRegistrationInfo_Click(object sender, ImageClickEventArgs e)
        {
            pRegistrationInfo.Visible = true;
            pRegistrationFooter.Visible = true;

            pParticipantInfo.Visible = false;
            pParticipantFooter.Visible = false;
        }

        protected void btnPreviousParticipationInfo_Click(object sender, ImageClickEventArgs e)
        {
            pParticipantInfo.Visible = true;
            pParticipantFooter.Visible = true;

            pVolunteerInfo.Visible = false;
            pVolunteerFooter.Visible = false;
        }

        protected void btnPreviousVolunteerInfo_Click(object sender, ImageClickEventArgs e)
        {
            pVolunteerInfo.Visible = true;
            pVolunteerFooter.Visible = true;

            pCharacterInfo.Visible = false;
            pCharacterFooter.Visible = false;
        }

        protected void btnPreviousCharacterInfo_Click(object sender, ImageClickEventArgs e)
        {
            pCharacterInfo.Visible = true;
            pCharacterFooter.Visible = true;

            pEmergencyInfo.Visible = false;
            pEmergencyFooter.Visible = false;
        }

        protected void btnPreviousEmergencyInfo_Click(object sender, ImageClickEventArgs e)
        {
            pEmergencyInfo.Visible = true;
            pEmergencyFooter.Visible = true;

            pWaiverInfo.Visible = false;
            pWaiverFooter.Visible = false;

            string script = "hideSignature();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        protected void btnPreviousWaiverInfo_Click(object sender, ImageClickEventArgs e)
        {
            RegistrationType regType = (RegistrationType)GetFromViewState(REG_TYPE_VIEW_STATE);
            if (regType.IsMinor)
            {
                pEmergencyInfo.Visible = true;
                pEmergencyFooter.Visible = true;
            }
            else
            {
                pWaiverInfo.Visible = true;
                pWaiverFooter.Visible = true;

                string script = "showSignature();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }

            pPaymentInfo.Visible = false;
            pPaymentFooter.Visible = false;
        }

        protected void btnRegister_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Payment");
            if (Page.IsValid)
            {
                try
                {
                    btnRegister.Enabled = false;

                    if (ProcessPayeezyPayment())
                    {
                        pPaymentInfo.Visible = false;
                        pPaymentFooter.Visible = false;

                        litConfirmation.Text = pageBody;
                        pConfirmation.Visible = true;
                    }

                    btnRegister.Enabled = true;
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());

                    pPaymentInfo.Visible = false;
                    pPaymentFooter.Visible = false;

                    pError.Visible = true;
                }
            }

            string script = "hideOverlay();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private bool ProcessPayeezyPayment()
        {
            RegistrationType regType = (RegistrationType)GetFromViewState(REG_TYPE_VIEW_STATE);

            // Create registration record
            int participantId = CreateRegistration(regType);

            string endpoint = "https://api.globalgatewaye4.firstdata.com/transaction/v27"; // PRODUCTION
            //string endpoint = "https://api.demo.globalgatewaye4.firstdata.com/transaction/v27"; // DEMO

            string gatewayID = "S52101-25";
            string password = "wO1Vg3XmcGf5FuEk19yWxIYMTsbq81Sc";

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (XmlTextWriter xmlw = new XmlTextWriter(sw))
                {
                    xmlw.Formatting = Formatting.Indented;
                    xmlw.WriteStartElement("Transaction");
                    xmlw.WriteElementString("ExactID", gatewayID);
                    xmlw.WriteElementString("Password", password);
                    xmlw.WriteElementString("Transaction_Type", "00");
                    xmlw.WriteElementString("DollarAmount", regType.Cost.ToString());
                    xmlw.WriteElementString("CardHoldersName", txtFirstName.Text + " " + txtLastName.Text);
                    xmlw.WriteElementString("Card_Number", txtCreditCardNumber.Text);
                    xmlw.WriteElementString("Expiry_Date", ddlExpirationMonth.SelectedValue + ddlExpirationYear.SelectedValue);
                    xmlw.WriteElementString("Reference_No", "2019 - " + participantId);
                }
            }

            string xml = sb.ToString();

            // SHA1 hash on XML string
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] xmlBytes = encoder.GetBytes(xml);
            SHA1CryptoServiceProvider sha1_crypto = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(sha1_crypto.ComputeHash(xmlBytes)).Replace("-", "");
            string hashed_content = hash.ToLower();

            // Assign values to hashing and header variables
            string keyID = "610530"; // Key ID
            string key = "~_XOMPftfmrM9GhjYxxGF4QCzIWLj99D"; // HMAC Key
            string method = "POST\n";
            string type = "application/xml"; // REST XML
            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string uri = "/transaction/v27";
            string hash_data = method + type + "\n" + hashed_content + "\n" + time + "\n" + uri;
            HMAC hmac_sha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            byte[] hmac_data = hmac_sha1.ComputeHash(Encoding.UTF8.GetBytes(hash_data));

            string base64_hash = Convert.ToBase64String(hmac_data);

            // Begin HttpWebRequest
            HttpWebRequest web_request = (HttpWebRequest)WebRequest.Create(endpoint);
            web_request.Method = "POST";
            web_request.ContentType = type;
            web_request.Accept = "*/*";
            web_request.Headers.Add("x-gge4-date", time);
            web_request.Headers.Add("x-gge4-content-sha1", hashed_content);
            web_request.Headers.Add("Authorization", "GGE4_API " + keyID + ":" + base64_hash);
            web_request.ContentLength = xml.Length;

            // Write and Send Request Data
            using (StreamWriter sw = new StreamWriter(web_request.GetRequestStream()))
            {
                sw.Write(xml);
            }

            // Get Response and Read into String
            XDocument response;
            try
            {
                bool approved = false;
                bool errored = false;
                string responseCode = "";
                string responseMessage = "";
                string authNum = "";
                string transTag = "";
                string errorNum = "";
                string errorDesc = "";
                string bankRespCode = "";
                string bankMessage = "";

                using (HttpWebResponse web_response = (HttpWebResponse)web_request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(web_response.GetResponseStream()))
                    {
                        response = XDocument.Load(sr);
                    }

                    approved = response.Descendants().FirstOrDefault(e => e.Name == "Transaction_Approved").Value == "true" ? true : false;
                    errored = response.Descendants().FirstOrDefault(e => e.Name == "Transaction_Error").Value == "true" ? true : false;
                    responseCode = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "exact_resp_code").Value;
                    responseMessage = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "exact_message").Value;
                    authNum = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "authorization_num").Value;
                    transTag = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "transaction_tag").Value;

                    var errnum = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "error_number");
                    if (errnum != null)
                    {
                        errorNum = errnum.Value;
                    }
                    var errdesc = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "error_description");
                    if (errdesc != null)
                    {
                        errorDesc = errdesc.Value;
                    }

                    bankRespCode = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "bank_resp_code").Value;
                    bankMessage = response.Descendants().FirstOrDefault(e => e.Name.ToString().ToLower() == "bank_message").Value;

                    if (approved) // The charge was approved
                    {
                        if (!regType.IsMinor)
                        {
                            SaveSignature(participantId);
                        }

                        CompleteRegistration(participantId, authNum, transTag, regType);

                        UpdateRegistration(participantId, "Paid");
                    }
                    else if (errored) // An error happened
                    {
                        cardError.Text = "<br /><br />Error! " + responseCode + " - " + responseMessage;

                        AddError("Error - ParticipantId: " + participantId + ": " + responseCode + " - " + responseMessage);

                        UpdateRegistration(participantId, "Errored");

                        return false;
                    }
                    else // The charge was declined
                    {
                        if (responseCode != "00") // Error occurred due to invalid values sent in request
                        {
                            cardError.Text = "<br /><br />Error! " + errorNum + " - " + errorDesc;

                            AddError("Error - ParticipantId: " + participantId + ": " + responseCode + " - " + responseMessage);

                            UpdateRegistration(participantId, "Errored");

                            return false;
                        }
                        else // The charge was declined
                        {
                            cardError.Text = "<br /><br />Error! " + bankRespCode + " - " + bankMessage;

                            AddError("Declined payment - ParticipantId: " + participantId + ": " + bankRespCode + " - " + bankMessage);

                            UpdateRegistration(participantId, "Declined");

                            return false;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse error_response = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(error_response.GetResponseStream()))
                        {
                            string remote_ex = reader.ReadToEnd();

                            cardError.Text = "<br /><br />Error! There was a problem submitting your payment.  Please try again.  If you are still unsuccessful, please contact Troll.";

                            AddError(remote_ex);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private int CreateRegistration(RegistrationType regType)
        {
            int participantId = 0;

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_CREATE_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_c_id", ddlCamp.SelectedValue);
                cmd.Parameters.AddWithValue("@par_FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@par_LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@par_DOB", txtDateOfBirth.Text);
                cmd.Parameters.AddWithValue("@par_CharacterName", txtCharacterName.Text);
                cmd.Parameters.AddWithValue("@par_ChapterName", txtChapterName.Text);
                if (regType.IsMerchant)
                {
                    cmd.Parameters.AddWithValue("@par_UnitName", txtBusinessName.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@par_UnitName", txtUnitName.Text);
                }
                cmd.Parameters.AddWithValue("@par_IsMinor", regType.IsMinor ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_IsMerchant", regType.IsMerchant ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_RegDate", regType.ArrivalDate);
                cmd.Parameters.AddWithValue("@par_VolunteerTroll", cbxVolunteerTroll.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerSafety", cbxVolunteerSafety.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerMedic", cbxVolunteerMedic.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerDay", cbxVolunteerDay.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerNight", cbxVolunteerNight.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerWeapon", cbxVolunteerWeapon.Checked ? "Y" : "N");
                cmd.Parameters.AddWithValue("@par_VolunteerRagU", cbxVolunteerRagU.Checked ? "Y" : "N");

                conn.Open();

                participantId = (int)cmd.ExecuteScalar();
            }

            return participantId;
        }

        private void SaveSignature(int participantId)
        {
            var encodedImage = signatureCode.Value.Split(',')[1];
            var decodedImage = Convert.FromBase64String(encodedImage);

            File.WriteAllBytes(Server.MapPath("~/images/Signatures/2019/" + participantId + ".png"), decodedImage);
        }

        private void CompleteRegistration(int participantId, string authCode, string transId, RegistrationType regType)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_COMPLETE_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_id", participantId);

                // Payment
                cmd.Parameters.AddWithValue("@pm_AuthCode", authCode);
                cmd.Parameters.AddWithValue("@pm_TransId", transId);
                cmd.Parameters.AddWithValue("@pm_Amount", Convert.ToDecimal(regType.Cost));
                //cmd.Parameters.AddWithValue("@pm_Amount", 0.00);

                // Address
                cmd.Parameters.AddWithValue("@add_Address1", txtAddress1.Text);
                cmd.Parameters.AddWithValue("@add_Address2", txtAddress2.Text);
                cmd.Parameters.AddWithValue("@add_City", txtCity.Text);
                cmd.Parameters.AddWithValue("@add_State", ddlState.SelectedValue);
                cmd.Parameters.AddWithValue("@add_Zip", txtZip.Text);

                // Contact Information
                cmd.Parameters.AddWithValue("@ci_HomePhone", txtHomePhone.Text);
                cmd.Parameters.AddWithValue("@ci_CellPhone", txtCellPhone.Text);
                cmd.Parameters.AddWithValue("@ci_Email", txtEmail.Text);

                // Emergency Contact Information
                cmd.Parameters.AddWithValue("@ec_Name", txtEmergencyContactName.Text);
                cmd.Parameters.AddWithValue("@ec_Phone", txtEmergencyContactPhone.Text);

                conn.Open();

                cmd.ExecuteNonQuery();
            }

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_HEALTH_ISSUE;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@hi_par_id", participantId);
                cmd.Parameters.AddWithValue("@hi_HealthIssue", txtHealthIssue.Text);

                conn.Open();

                cmd.ExecuteScalar();
            }

            Camp regCamp = new Camp();
            if (ddlCamp.SelectedValue != "0")
            {
                regCamp = GetCamp(ddlCamp.SelectedValue);
            }
            else
            {
                regCamp.Id = 0;
            }

            SendConfirmationEmail(participantId, regCamp, regType);
            CreateBody(participantId, regCamp, regType);

            if (regCamp.Id != 0)
            {
                SendCampMasterEmail(regCamp, regType);
            }
        }

        private void UpdateRegistration(int participantId, string status)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_UPDATE_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_id", participantId);
                cmd.Parameters.AddWithValue("@par_status", status);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private void SendConfirmationEmail(int participantId, Camp regCamp, RegistrationType regType)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(txtEmail.Text));
            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
            if (regType.IsMerchant)
            {
                mailMessage.Subject = "Ragnarok XXXIV - Merchant Registration Confirmation";
            }
            else
            {
                mailMessage.Subject = "Ragnarok XXXIV - Registration Confirmation";
            }
            mailMessage.IsBodyHtml = true;

            string server = hostSettings["SMTPServer"];
            string authentication = hostSettings["SMTPAuthentication"];
            string password = hostSettings["SMTPPassword"];
            string username = hostSettings["SMTPUsername"];

            string body = "<p><b>" + txtCharacterName.Text + "</b>,</p>";
            body += "<p>Thank you for registering to attend <b>Ragnarok XXXIV</b>";
            if (regType.IsMerchant)
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

            body += "<p>Your selected arrival date is <b>" + regType.ArrivalDate.ToString("MM/dd/yyyy") + "</b>.</p>";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://dagorhirragnarok.com/CheckIn.aspx?pid=" + participantId, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            qrCodeBytes = qrCode.GetGraphic(20);

            LinkedResource linkedQR;
            using (var ms = new MemoryStream(qrCodeBytes))
            {
                linkedQR = new LinkedResource(ms, "image/jpeg");

                linkedQR.ContentId = "qrCode";

                body += "<p>For expedited check-in at Troll when you arrive at Ragnarok, please present this QR Code to the Troll staff:</p>";
                body += "<center><p><img style=\"height: 300px; width: 300px;\" src=cid:qrCode /></p></center>";
                body += "<p>We look forward to seeing you at Ragnarok!</p>";
                body += "<p>----------------------------------------------<br />Troll</p>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);
                av.LinkedResources.Add(linkedQR);
                mailMessage.AlternateViews.Add(av);

                try
                {
                    using (SmtpClient client = new SmtpClient(server))
                    {
                        client.Send(mailMessage);
                    }
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                }
            }
        }

        private void CreateBody(int participantId, Camp regCamp, RegistrationType regType)
        {
            pageBody = "<p><b>" + txtCharacterName.Text + "</b>,</p>";
            pageBody += "<p>Thank you for registering to attend <b>Ragnarok XXXIV</b>";
            if (regType.IsMerchant)
            {
                pageBody += " as a merchant";
            }
            pageBody += "!</p>";

            if (regCamp.Id != 0)
            {
                pageBody += "<p>You are marked down as an attendee staying in <b>" + regCamp.CampName + "</b>. If your camp changes, please contact ";
                pageBody += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to change your camp, or you can change it ";
                pageBody += "when you arrive at Ragnarok.</p>";
            }
            else
            {
                pageBody += "<p>You are marked down as not having a camp selected yet. If you choose a camp, please contact ";
                pageBody += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to select your camp, or you can select your ";
                pageBody += "camp when you arrive at Ragnarok.</p>";
            }

            pageBody += "<p>Your selected arrival date is <b>" + regType.ArrivalDate.ToString("MM/dd/yyyy") + "</b>.</p>";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://dagorhirragnarok.com/CheckIn.aspx?pid=" + participantId, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            qrCodeBytes = qrCode.GetGraphic(20);

            string imgSource = "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes, Base64FormattingOptions.None);

            pageBody += "<p>For expedited check-in at Troll when you arrive at Ragnarok, please present this QR Code to the Troll staff:</p>";
            pageBody += "<center><p><img style=\"height: 300px; width: 300px;\" src=\"" + imgSource + "\" /></p></center>";
            pageBody += "<p>We look forward to seeing you at Ragnarok!</p>";
            pageBody += "<p>----------------------------------------------<br />Troll</p>";
        }

        private void SendCampMasterEmail(Camp regCamp, RegistrationType regType)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(regCamp.CampMaster.Email));
            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
            //mailMessage.To.Add(new MailAddress("jjp4674@gmail.com"));
            mailMessage.Subject = "Ragnarok XXXIV - Camp Attendee Registration Confirmation";
            mailMessage.IsBodyHtml = true;

            string server = hostSettings["SMTPServer"];
            string authentication = hostSettings["SMTPAuthentication"];
            string password = hostSettings["SMTPPassword"];
            string username = hostSettings["SMTPUsername"];

            string body = "<p><b>" + regCamp.CampMaster.FirstName + "</b>,</p>";
            body += "<p>An attendee has registered to attend <b>Ragnarok XXXIV</b> as part of <b>" + regCamp.CampName + "</b>!</p>";
            body += "<p>Attendee Information<br />";
            body += "Arrival Date: <b>" + regType.ArrivalDate.ToString("MM/dd/yyyy") + "</b><br />";
            body += "Name: <b>" + txtFirstName.Text + " " + txtLastName.Text + "</b><br />";
            body += "Character Name: <b>" + txtCharacterName.Text + "</b><br />";
            body += "Unit Name: <b>" + txtUnitName.Text + "</b><br />";
            body += "Chapter Name: <b>" + txtChapterName.Text + "</b><br />";
            body += "Email Address: <b><a href=\"mailto:" + txtEmail.Text + "\">" + txtEmail.Text + "</a></b></p>";
            body += "<p>If this attendee should not be part of your camp, please contact them and/or <a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> ";
            body += "to resolve the issue.</p>";
            body += "<p>We look forward to seeing you at Ragnarok!</p>";
            body += "<p>----------------------------------------------<br />Troll</p>";

            mailMessage.Body = body;

            try
            {
                using (SmtpClient client = new SmtpClient(server))
                {
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                AddError(ex.ToString());
            }
        }

        private Camp GetCamp(string campId)
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

        private string GetSelectedType()
        {
            if (rbAdultSaturday.Checked)
                return rbAdultSaturday.Text;

            if (rbAdultSunday.Checked)
                return rbAdultSunday.Text;

            if (rbAdultMonday.Checked)
                return rbAdultMonday.Text;

            if (rbAdultTuesday.Checked)
                return rbAdultTuesday.Text;

            if (rbAdultWednesday.Checked)
                return rbAdultWednesday.Text;

            if (rbAdultThursday.Checked)
                return rbAdultThursday.Text;

            if (rbAdultFriday.Checked)
                return rbAdultFriday.Text;

            if (rbAdultSaturday2.Checked)
                return rbAdultSaturday2.Text;

            if (rbChildSaturday.Checked)
                return rbChildSaturday.Text;

            if (rbChildSunday.Checked)
                return rbChildSunday.Text;

            if (rbChildMonday.Checked)
                return rbChildMonday.Text;

            if (rbChildTuesday.Checked)
                return rbChildTuesday.Text;

            if (rbChildWednesday.Checked)
                return rbChildWednesday.Text;

            if (rbChildThursday.Checked)
                return rbChildThursday.Text;

            if (rbChildFriday.Checked)
                return rbChildFriday.Text;

            if (rbChildSaturday2.Checked)
                return rbChildSaturday2.Text;

            if (rbMerchant2020.Checked)
                return rbMerchant2020.Text;

            if (rbMerchant4020.Checked)
                return rbMerchant4020.Text;

            return "";
        }

        private List<RegistrationType> PopulateRegistrationTypes()
        {
            List<RegistrationType> registrationTypes = new List<RegistrationType>();

            // Adult Registrations
            RegistrationType rt = new RegistrationType
            {
                Text = "Saturday (06/15/2019) - $85", 
                Day = "Saturday", 
                ArrivalDate = Convert.ToDateTime("06/15/2019"), 
                IsMinor = false, 
                IsMerchant = false, 
                Cost = 85
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Sunday (06/16/2019) - $60",
                Day = "Sunday",
                ArrivalDate = Convert.ToDateTime("06/16/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 60
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Monday (06/17/2019) - $60",
                Day = "Monday",
                ArrivalDate = Convert.ToDateTime("06/17/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 60
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Tuesday (06/18/2019) - $60",
                Day = "Tuesday",
                ArrivalDate = Convert.ToDateTime("06/18/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 60
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Wednesday (06/19/2019) - $60",
                Day = "Wednesday",
                ArrivalDate = Convert.ToDateTime("06/19/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 60
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Thursday (06/20/2019) - $55",
                Day = "Thursday",
                ArrivalDate = Convert.ToDateTime("06/20/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 55
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Friday (06/21/2019) - $45",
                Day = "Friday",
                ArrivalDate = Convert.ToDateTime("06/21/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 45
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Saturday (06/22/2019) - $35",
                Day = "Saturday",
                ArrivalDate = Convert.ToDateTime("06/22/2019"),
                IsMinor = false,
                IsMerchant = false,
                Cost = 35
            };
            registrationTypes.Add(rt);


            // Minor Registrations
            rt = new RegistrationType
            {
                Text = "Saturday (06/15/2019) - $60",
                Day = "Saturday",
                ArrivalDate = Convert.ToDateTime("06/15/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 60
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Sunday (06/16/2019) - $45",
                Day = "Sunday",
                ArrivalDate = Convert.ToDateTime("06/16/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 45
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Monday (06/17/2019) - $45",
                Day = "Monday",
                ArrivalDate = Convert.ToDateTime("06/17/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 45
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Tuesday (06/18/2019) - $45",
                Day = "Tuesday",
                ArrivalDate = Convert.ToDateTime("06/18/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 45
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Wednesday (06/19/2019) - $45",
                Day = "Wednesday",
                ArrivalDate = Convert.ToDateTime("06/19/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 45
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Thursday (06/20/2019) - $30",
                Day = "Thursday",
                ArrivalDate = Convert.ToDateTime("06/20/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 30
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Friday (06/21/2019) - $20",
                Day = "Friday",
                ArrivalDate = Convert.ToDateTime("06/21/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 20
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "Saturday (06/22/2019) - $15",
                Day = "Saturday",
                ArrivalDate = Convert.ToDateTime("06/22/2019"),
                IsMinor = true,
                IsMerchant = false,
                Cost = 15
            };
            registrationTypes.Add(rt);


            // Merchant Registrations
            rt = new RegistrationType
            {
                Text = "20x20 Booth - $120",
                Day = "Friday",
                ArrivalDate = Convert.ToDateTime("06/14/2019"),
                IsMinor = false,
                IsMerchant = true,
                Cost = 120
            };
            registrationTypes.Add(rt);

            rt = new RegistrationType
            {
                Text = "40x20 Booth - $150",
                Day = "Friday",
                ArrivalDate = Convert.ToDateTime("06/14/2019"),
                IsMinor = false,
                IsMerchant = true,
                Cost = 150
            };
            registrationTypes.Add(rt);

            return registrationTypes;
        }

        private void WriteToViewState(string viewStateName, object objectToWrite)
        {
            ViewState[viewStateName] = objectToWrite;
        }

        private object GetFromViewState(string viewStateName)
        {
            return ViewState[viewStateName];
        }
    }
}