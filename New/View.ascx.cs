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

namespace Ragnarok.Modules.RagnarokRegistration.New
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
        private const string PROCEDURE_ADD_REGISTRATION = "AddRegistration";
        private const string PROCEDURE_ADD_HEALTH_ISSUE = "AddHealthIssue";
        private const string PROCEDURE_ADD_ERROR = "AddError";
        private const string PROCEDURE_GET_CAMP = "GetCamp";

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
                dt.Rows.Add(new Object[] { camp.Id.ToString(), camp.CampName + " - " + camp.CampMaster.FirstName + " " + camp.CampMaster.LastName });
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
                businessName.Visible = false;
                if (ddlCost.SelectedItem.Text.Contains("Merchant"))
                {
                    businessName.Visible = true;
                }

                pParticipantInfo.Visible = true;
                pParticipantFooter.Visible = true;

                pRegistrationInfo.Visible = false;
                pRegistrationFooter.Visible = false;
            }
        }

        protected void btnNextCharacterInfo_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("Participant");
            if (Page.IsValid)
            {
                unitName.Visible = true;
                if (ddlCost.SelectedItem.Text.Contains("Merchant"))
                {
                    unitName.Visible = false;
                }

                pCharacterInfo.Visible = true;
                pCharacterFooter.Visible = true;

                pParticipantInfo.Visible = false;
                pParticipantFooter.Visible = false;
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
                if (ddlCost.SelectedItem.Text.Contains("Child"))
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
            if (ddlCost.SelectedItem.Text.Contains("Child"))
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
            bool valid = true;
            lblCheckboxError.Visible = false;
            lblSignatureError.Visible = false;
            string script = "";

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
                if (ProcessPayment())
                {
                    pWaiverInfo.Visible = false;
                    pWaiverFooter.Visible = false;

                    pConfirmation.Visible = true;
                }

                script = "hideSignature();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }

            script = "hideOverlay();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);


            Page.Validate("Payment");
            if (Page.IsValid)
            {
                try
                {
                    if (ProcessPayment())
                    {
                        pPaymentInfo.Visible = false;
                        pPaymentFooter.Visible = false;

                        pConfirmation.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());

                    pPaymentInfo.Visible = false;
                    pPaymentFooter.Visible = false;

                    pError.Visible = true;
                }
            }

            //string script = "hideOverlay();";
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private bool ProcessPayment()
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                // Dev Account: name = "3QeAr3WX7z",
                name = "2B4f4zGR",
                ItemElementName = ItemChoiceType.transactionKey,
                //Item = "9f896x36Uy242FQf"
                Item = "678PGs573esM4QJ6"
            };

            var creditCard = new creditCardType
            {
                cardNumber = txtCreditCardNumber.Text,
                expirationDate = (ddlExpirationMonth.SelectedValue + ddlExpirationYear.SelectedValue)
            };

            var paymentType = new paymentType { Item = creditCard };

            var billToType = new customerAddressType
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                email = txtEmail.Text
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = Convert.ToDecimal(ddlCost.SelectedValue.ToString()),
                payment = paymentType,
                billTo = billToType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            try
            {
                var controller = new createTransactionController(request);
                controller.Execute();

                var response = controller.GetApiResponse();

                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    AddRegistration("", "");
                }
                else
                {
                    cardError.Text = "<br /><br />Error!  " + response.messages.message[0].code + " " + response.messages.message[0].text;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                cardError.Text = "<br /><br />Error! There was a problem submitting your payment.  Please try again.  If you are still unsuccessful, please contact Troll.";
                return false;
            }
        }

        private void AddRegistration(string authCode, string transId)
        {
            int participantId = 0;

            DateTime? regDate = null;
            string isMinor = "N";
            string isMerchant = "N";
            if (ddlCost.SelectedItem.Text.Contains("Saturday"))
            {
                regDate = new DateTime(2017, 06, 17);
            }
            else if (ddlCost.SelectedItem.Text.Contains("Sunday"))
            {
                regDate = new DateTime(2017, 06, 18);
            }

            if (ddlCost.SelectedItem.Text.Contains("Child"))
            {
                isMinor = "Y";
            }
            else if (ddlCost.SelectedItem.Text.Contains("Merchant"))
            {
                isMerchant = "Y";
            }

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_c_id", ddlCamp.SelectedValue);
                cmd.Parameters.AddWithValue("@par_FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@par_LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@par_DOB", txtDateOfBirth.Text);
                cmd.Parameters.AddWithValue("@par_CharacterName", txtCharacterName.Text);
                cmd.Parameters.AddWithValue("@par_ChapterName", txtChapterName.Text);
                if (isMerchant == "Y")
                {
                    cmd.Parameters.AddWithValue("@par_UnitName", txtBusinessName.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@par_UnitName", txtUnitName.Text);
                }
                cmd.Parameters.AddWithValue("@par_IsMinor", isMinor);
                cmd.Parameters.AddWithValue("@par_IsMerchant", isMerchant);
                if (regDate != null)
                {
                    cmd.Parameters.AddWithValue("@par_RegDate", (DateTime)regDate);
                }

                // Payment
                cmd.Parameters.AddWithValue("@pm_AuthCode", authCode);
                cmd.Parameters.AddWithValue("@pm_TransId", transId);
                //cmd.Parameters.AddWithValue("@pm_Amount", Convert.ToDecimal(ddlCost.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@pm_Amount", 0.00);

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

                // Signature
                cmd.Parameters.AddWithValue("@s_Signature", signatureCode.Value);

                conn.Open();

                participantId = (int)cmd.ExecuteScalar();
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

            SendConfirmationEmail(participantId, regCamp, isMinor, isMerchant, regDate);

            if (regCamp.Id != 0)
            {
                SendCampMasterEmail(regCamp, isMinor, isMerchant, regDate);
            }
        }

        private void SendConfirmationEmail(int participantId, Camp regCamp, string isMinor, string isMerchant, DateTime? regDate)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(txtEmail.Text));
            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
            if (isMerchant == "Y")
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

            string body = "<p><b>" + txtCharacterName.Text + "</b>,</p>";
            body += "<p>Thank you for registering to attend <b>Ragnarok XXXII</b>";
            if (isMerchant == "Y")
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

            if (isMerchant != "Y")
            {
                body += "<p>Your selected arrival date is <b>" + ((DateTime)regDate).ToString("MM/dd/yyyy") + "</b>.</p>";
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

            using (SmtpClient client = new SmtpClient(server))
            {
                client.Send(mailMessage);
            }
        }

        private void SendCampMasterEmail(Camp regCamp, string isMinor, string isMerchant, DateTime? regDate)
        {
            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
            mailMessage.To.Add(new MailAddress(regCamp.CampMaster.Email));
            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
            //mailMessage.To.Add(new MailAddress("jjp4674@gmail.com"));
            mailMessage.Subject = "Ragnarok XXXII - Camp Attendee Registration Confirmation";
            mailMessage.IsBodyHtml = true;

            string server = hostSettings["SMTPServer"];
            string authentication = hostSettings["SMTPAuthentication"];
            string password = hostSettings["SMTPPassword"];
            string username = hostSettings["SMTPUsername"];

            string body = "<p><b>" + regCamp.CampMaster.FirstName + "</b>,</p>";
            body += "<p>An attendee has registered to attend <b>Ragnarok XXXII</b> as part of <b>" + regCamp.CampName + "</b>!</p>";
            body += "<p>Attendee Information<br />";
            if (isMerchant != "Y")
            {
                body += "Arrival Date: <b>" + ((DateTime)regDate).ToString("MM/dd/yyyy") + "</b><br />";
            }
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

            using (SmtpClient client = new SmtpClient(server))
            {
                client.Send(mailMessage);
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
    }
}