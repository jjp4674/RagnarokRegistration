/*
' Copyright (c) 2017  DagorhirRagnarok.com
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
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Globalization;
using System.Net.Mail;
using System.Collections.Generic;
using QRCoder;
using System.IO;
using System.Web.UI;
using Ragnarok.Modules.RagnarokRegistration.Models;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.CheckIn
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from RagnarokAdminAttendeesModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_GET_ATTENDEE = "GetAttendeeCheckIn";
        private const string PROCEDURE_GET_ATTENDEES = "GetAttendeeCheckIns";
        private const string PROCEDURE_SAVE_ATTENDEE = "SaveRegistration";
        private const string PROCEDURE_GET_CAMPS = "GetCamps";
        private const string PROCEDURE_GET_CAMP = "GetCamp";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

                if (!Page.IsPostBack)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["pid"]) && Request.QueryString["pid"] != "0")
                    {
                        LoadAttendeeData(Request.QueryString["pid"]);
                    }
                    else if (Request.QueryString["pid"] == "0")
                    {
                        pConfirm.Visible = false;
                        pEdit.Visible = true;
                        pEditBottom.Visible = true;

                        string script = "showSignature();";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);

                        LoadAttendeeEditData();
                    }
                    else
                    {
                        pConfirm.Visible = false;
                        pSearch.Visible = true;
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
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

        private void LoadAttendeeData(string id)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_ATTENDEE, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@par_id", id);

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            bool noProceed = false;
                            Attendee attendee = new Attendee(dr);

                            litFirstName.Text = attendee.FirstName;
                            litLastName.Text = attendee.LastName;
                            litDOB.Text = attendee.DateOfBirth.ToString("MM/dd/yyyy");
                            litAddress1.Text = attendee.Address.Address1;
                            litAddress2.Text = attendee.Address.Address2;
                            litCity.Text = attendee.Address.City;
                            litState.Text = attendee.Address.State;
                            litZip.Text = attendee.Address.Zip;
                            litHomePhone.Text = attendee.ContactInformation.HomePhone;
                            litCellPhone.Text = attendee.ContactInformation.CellPhone;
                            litEmail.Text = attendee.ContactInformation.Email;
                            litCharacterName.Text = attendee.CharacterName;
                            litUnitName.Text = attendee.UnitName;
                            litChapterName.Text = attendee.ChapterName;
                            if (string.IsNullOrEmpty(attendee.CampName))
                            {
                                lblCamp.Text = "No Camp Selected";
                                lblCamp.Font.Bold = true;
                                lblCamp.ForeColor = System.Drawing.Color.Red;

                                noProceed = true;
                            }
                            else
                            {
                                lblCamp.Text = attendee.CampName;
                                lblCamp.Font.Bold = false;
                                lblCamp.ForeColor = System.Drawing.Color.Black;
                            }
                            litEmergencyContact.Text = attendee.EmergencyContact.Name;
                            litEmergencyPhone.Text = attendee.EmergencyContact.Phone;
                            if (attendee.MinorParentTagNumber != null)
                            {
                                litMinorParentTagNumber.Text = attendee.MinorParentTagNumber.ToString();
                            }
                            litHealthIssues.Text = string.IsNullOrEmpty(attendee.HealthIssues) ? "None" : attendee.HealthIssues;

                            // Check that the signature exists
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/images/Signatures/" + attendee.EventYear.Year + "/" + id + ".png")))
                            {
                                imgSignature.ImageUrl = "/images/Signatures/" + attendee.EventYear.Year + "/" + id + ".png?" + DateTime.Now.ToString();

                                imgSignature.Visible = true;
                                lblNoSignature.Visible = false;
                            }
                            else
                            {
                                lblNoSignature.Visible = true;
                                imgSignature.Visible = false;

                                noProceed = true;
                            }

                            pIsMinor.Visible = attendee.IsMinor;
                            pMinor.Visible = attendee.IsMinor;
                            pIsMerchant.Visible = attendee.IsMerchant;
                            pCheckedIn.Visible = false;
                            if (attendee.ArrivalDate.HasValue)
                            {
                                pCheckedIn.Visible = true;
                            }
                            pArrivalDate.Visible = false;
                            if (attendee.RegistrationDate.HasValue)
                            {
                                pArrivalDate.Visible = true;
                                litArrival.Text = attendee.RegistrationDate.Value.DayOfWeek.ToString();
                            }
                            pNotPaid.Visible = false;
                            if (attendee.Status != "Paid" && attendee.Status != "Checked In" && attendee.Status != "Duplicate")
                            {
                                pNotPaid.Visible = true;
                            }
                            if (attendee.Status == "Duplicate")
                            {
                                pDuplicate.Visible = true;
                            }

                            SetViewStateItem("attendee", attendee);

                            if (noProceed)
                            {
                                btnInformationCorrect.Enabled = false;
                                btnInformationCorrect.CssClass = "btn btn-lg btn-default right";
                            }
                            else
                            {
                                btnInformationCorrect.Enabled = true;
                                btnInformationCorrect.CssClass = "btn btn-lg btn-success right";
                            }
                        }
                    }
                }
            }
        }

        private void LoadAttendeeEditData()
        {
            LoadCamps();
            ClearAttendeeEditData();

            // If it's a new registration
            if (!String.IsNullOrEmpty(Request.QueryString["pid"]) && Request.QueryString["pid"] == "0")
            {
                Attendee attendee = new Attendee();
                attendee.Id = 0;

                SetViewStateItem("attendee", attendee);
            }
            else
            {
                Attendee attendee = (Attendee)GetViewStateItem("attendee");
                if (attendee != null)
                {
                    txtFirstName.Text = attendee.FirstName;
                    txtLastName.Text = attendee.LastName;
                    txtDOB.Text = attendee.DateOfBirth.ToString("MM/dd/yyyy");
                    cbxIsMinor.Checked = attendee.IsMinor;
                    if (attendee.MinorParentTagNumber != null)
                    {
                        txtMinorParentTagNumber.Text = attendee.MinorParentTagNumber.ToString();
                    }
                    txtAddress1.Text = attendee.Address.Address1;
                    txtAddress2.Text = attendee.Address.Address2;
                    txtCity.Text = attendee.Address.City;
                    ddlState.SelectedValue = attendee.Address.State;
                    txtZip.Text = attendee.Address.Zip;
                    txtHomePhone.Text = attendee.ContactInformation.HomePhone;
                    txtCellPhone.Text = attendee.ContactInformation.CellPhone;
                    txtEmail.Text = attendee.ContactInformation.Email;
                    txtCharacterName.Text = attendee.CharacterName;
                    txtUnitName.Text = attendee.UnitName;
                    txtChapterName.Text = attendee.ChapterName;
                    ddlCamp.SelectedValue = attendee.CampId.ToString();
                    cbxIsMerchant.Checked = attendee.IsMerchant;
                    txtEmergencyContactName.Text = attendee.EmergencyContact.Name;
                    txtEmergencyContactPhone.Text = attendee.EmergencyContact.Phone;
                    txtHealthIssues.Text = attendee.HealthIssues;

                    byte[] bytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/images/Signatures/" + attendee.EventYear.Year + "/" + attendee.Id + ".png"));
                    string dataURL = "data:image/png;base64," + Convert.ToBase64String(bytes);
                    signatureCode.Value = dataURL;
                    string script = "signaturePad.fromDataURL('" + dataURL + "');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
        }

        private string SaveAttendee(Attendee attendee)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_SAVE_ATTENDEE, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@par_id", attendee.Id);
                    if (attendee.TagNumber.HasValue)
                    {
                        comm.Parameters.AddWithValue("@par_TagNumber", attendee.TagNumber.Value);
                    }
                    if (attendee.ArrivalDate.HasValue)
                    {
                        comm.Parameters.AddWithValue("@par_ArrDate", attendee.ArrivalDate);
                    }
                    comm.Parameters.AddWithValue("@par_c_id", attendee.CampId);
                    comm.Parameters.AddWithValue("@par_FirstName", attendee.FirstName);
                    comm.Parameters.AddWithValue("@par_LastName", attendee.LastName);
                    comm.Parameters.AddWithValue("@par_DOB", attendee.DateOfBirth);
                    comm.Parameters.AddWithValue("@par_CharacterName", attendee.CharacterName);
                    comm.Parameters.AddWithValue("@par_ChapterName", attendee.ChapterName);
                    comm.Parameters.AddWithValue("@par_UnitName", attendee.UnitName);
                    comm.Parameters.AddWithValue("@par_IsMinor", attendee.IsMinor ? "Y" : "N");
                    comm.Parameters.AddWithValue("@par_IsMerchant", attendee.IsMerchant ? "Y" : "N");
                    comm.Parameters.AddWithValue("@par_Status", attendee.Status);
                    comm.Parameters.AddWithValue("@add_Address1", attendee.Address.Address1);
                    comm.Parameters.AddWithValue("@add_Address2", attendee.Address.Address2);
                    comm.Parameters.AddWithValue("@add_City", attendee.Address.City);
                    comm.Parameters.AddWithValue("@add_State", attendee.Address.State);
                    comm.Parameters.AddWithValue("@add_Zip", attendee.Address.Zip);
                    comm.Parameters.AddWithValue("@ci_HomePhone", attendee.ContactInformation.HomePhone);
                    comm.Parameters.AddWithValue("@ci_CellPhone", attendee.ContactInformation.CellPhone);
                    comm.Parameters.AddWithValue("@ci_Email", attendee.ContactInformation.Email);
                    comm.Parameters.AddWithValue("@ec_Name", attendee.EmergencyContact.Name);
                    comm.Parameters.AddWithValue("@ec_Phone", attendee.EmergencyContact.Phone);
                    comm.Parameters.AddWithValue("@hi_HealthIssue", attendee.HealthIssues);
                    comm.Parameters.AddWithValue("@par_MinorParentTagNumber", attendee.MinorParentTagNumber);

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return dr["Id"].ToString();
                        }
                    }
                }
            }

            return "0";
        }

        private void ClearAttendeeEditData()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtDOB.Text = "";
            cbxIsMinor.Checked = false;
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCity.Text = "";
            ddlState.SelectedIndex = 0;
            txtZip.Text = "";
            txtHomePhone.Text = "";
            txtCellPhone.Text = "";
            txtEmail.Text = "";
            txtCharacterName.Text = "";
            txtUnitName.Text = "";
            txtChapterName.Text = "";
            ddlCamp.SelectedIndex = 0;
            cbxIsMerchant.Checked = false;
            txtEmergencyContactName.Text = "";
            txtEmergencyContactPhone.Text = "";
            txtHealthIssues.Text = "";
        }

        private void LoadCamps()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CAMPS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        ListItem li = new ListItem("-- Select a Camp --", "0");
                        ddlCamp.Items.Add(li);

                        while (dr.Read())
                        {
                            li = new ListItem(dr["CampName"].ToString(), dr["Id"].ToString());
                            ddlCamp.Items.Add(li);
                        }
                    }
                }
            }
        }

        protected void btnMakeChanges_Click(object sender, EventArgs e)
        {
            pConfirm.Visible = false;
            pEdit.Visible = true;
            pEditBottom.Visible = true;

            string script = "showSignature();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            
            LoadAttendeeEditData();
        }

        protected void btnInformationCorrect_Click(object sender, EventArgs e)
        {
            Page.Validate("Edit");
            if (Page.IsValid)
            {
                pCheckIn.Visible = true;
                pConfirm.Visible = false;

                string script = "hideSignature();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pEdit.Visible = false;
            pEditBottom.Visible = false;
            pConfirm.Visible = true;

            string script = "hideSignature();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = "0";
            Attendee attendee = (Attendee)GetViewStateItem("attendee");
            if (attendee != null)
            {
                attendee.FirstName = txtFirstName.Text;
                attendee.LastName = txtLastName.Text;
                attendee.DateOfBirth = Convert.ToDateTime(txtDOB.Text);
                attendee.IsMinor = cbxIsMinor.Checked;
                attendee.MinorParentTagNumber = Convert.ToInt32(txtMinorParentTagNumber.Text);
                if (attendee.Address == null)
                {
                    Address address = new Address();
                    attendee.Address = address;
                }
                attendee.Address.Address1 = txtAddress1.Text;
                attendee.Address.Address2 = txtAddress2.Text;
                attendee.Address.City = txtCity.Text;
                attendee.Address.State = ddlState.SelectedValue;
                attendee.Address.Zip = txtZip.Text;
                if (attendee.ContactInformation == null)
                {
                    ContactInformation contactInformation = new ContactInformation();
                    attendee.ContactInformation = contactInformation;
                }
                attendee.ContactInformation.HomePhone = txtHomePhone.Text;
                attendee.ContactInformation.CellPhone = txtCellPhone.Text;
                attendee.ContactInformation.Email = txtEmail.Text;
                attendee.CharacterName = txtCharacterName.Text;
                attendee.UnitName = txtUnitName.Text;
                attendee.ChapterName = txtChapterName.Text;
                attendee.CampId = Convert.ToInt32(ddlCamp.SelectedValue);
                attendee.IsMerchant = cbxIsMerchant.Checked;
                if (attendee.EmergencyContact == null)
                {
                    EmergencyContact emergencyContact = new EmergencyContact();
                    attendee.EmergencyContact = emergencyContact;
                }
                attendee.EmergencyContact.Name = txtEmergencyContactName.Text;
                attendee.EmergencyContact.Phone = txtEmergencyContactPhone.Text;
                attendee.HealthIssues = txtHealthIssues.Text;
                attendee.Status = "Paid";
                attendee.EventYear = DateTime.Now;

                id = SaveAttendee(attendee);

                if (id != "0")
                {
                    SaveSignature(Convert.ToInt32(id));
                }
            }

            LoadAttendeeData(id);

            pEdit.Visible = false;
            pEditBottom.Visible = false;
            pConfirm.Visible = true;

            string script = "hideSignature();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private void SaveSignature(int participantId)
        {
            var encodedImage = signatureCode.Value.Split(',')[1];
            var decodedImage = Convert.FromBase64String(encodedImage);

            File.WriteAllBytes(Server.MapPath("~/images/Signatures/2019/" + participantId + ".png"), decodedImage);
        }

        private void SetViewStateItem(string key, object obj)
        {
            ViewState[key] = obj;
        }

        private object GetViewStateItem(string key)
        {
            if (ViewState[key] != null)
            {
                return ViewState[key];
            }
            else
            {
                return null;
            }
        }

        protected void btnCheckIn_Click(object sender, EventArgs e)
        {
            Page.Validate("CheckIn");
            if (Page.IsValid)
            {
                Attendee attendee = (Attendee)GetViewStateItem("attendee");
                if (attendee != null)
                {
                    attendee.TagNumber = Convert.ToInt32(txtTagNumber.Text);
                    attendee.Status = "Checked In";
                    if (!attendee.ArrivalDate.HasValue)
                    {
                        attendee.ArrivalDate = DateTime.Now;
                    }

                    SaveAttendee(attendee);

                    pCheckIn.Visible = false;
                    pThankYou.Visible = true;
                }
            }
        }

        protected void repAttendees_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    LoadAttendeeData(e.CommandArgument.ToString());

                    pConfirm.Visible = true;
                    pSearch.Visible = false;
                }
            }
        }

        protected void btnNewAttendee_Click(object sender, EventArgs e)
        {
            Response.Redirect("/CheckIn.aspx?pid=0");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Validate("Search");
            if (Page.IsValid)
            {
                List<Attendee> attendees = GetAttendees();
                if (attendees != null)
                {
                    if (attendees.Count > 0)
                    {
                        repAttendees.DataSource = attendees.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
                        repAttendees.DataBind();

                        pAttendeesList.Visible = true;
                        pNoneFound.Visible = false;
                    }
                    else
                    {
                        pAttendeesList.Visible = false;
                        pNoneFound.Visible = true;
                    }
                }
            }
        }

        private List<Attendee> GetAttendees()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_ATTENDEES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(txtSearchFirstName.Text))
                    {
                        comm.Parameters.AddWithValue("@par_FirstName", txtSearchFirstName.Text);
                    }
                    if (!string.IsNullOrEmpty(txtSearchLastName.Text))
                    {
                        comm.Parameters.AddWithValue("@par_LastName", txtSearchLastName.Text);
                    }
                    if (!string.IsNullOrEmpty(txtSearchRagTag.Text))
                    {
                        comm.Parameters.AddWithValue("@par_TagNumber", txtSearchRagTag.Text);
                    }

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        List<Attendee> attendees = new List<Attendee>();
                        while (dr.Read())
                        {
                            attendees.Add(new Attendee(dr));
                        };

                        if (attendees.Count > 0)
                        {
                            return attendees;
                        }

                        return null;
                    }
                }
            }
        }
    }
}