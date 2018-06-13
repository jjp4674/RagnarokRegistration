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

namespace Ragnarok.Modules.RagnarokRegistration.Admin.Attendees
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
        private const string PROCEDURE_GET_ATTENDEE_GRID = "GetAttendeeGrid";
        private const string PROCEDURE_GET_ATTENDEE = "GetAttendee";
        private const string PROCEDURE_GET_CAMPS = "GetCamps";
        private const string PROCEDURE_GET_HEALTH_ISSUES = "GetHealthIssues";
        private const string PROCEDURE_GET_CAMP = "GetCamp";
        private const string PROCEDURE_GET_CAMP_ATTENDEES = "GetCampAttendees";
        private const string PROCEDURE_SAVE_ATTENDEE = "SaveAttendee";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ViewState["sortColumn"] = "";
                    ViewState["sortDirection"] = "";
                    PopulateAttendeesGrid();
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

        protected void gvAttendees_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewState["sortColumn"].ToString()) && ViewState["sortColumn"].ToString() == e.SortExpression)
            {
                ViewState["sortDirection"] = "DESC";
            }
            else
            {
                ViewState["sortDirection"] = "ASC";
            }
            ViewState["sortColumn"] = e.SortExpression;

            gvAttendees.DataSource = SortData();
            gvAttendees.DataBind();
        }

        protected void gvAttendees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAttendees.PageIndex = e.NewPageIndex;
            gvAttendees.DataSource = SortData();
            gvAttendees.DataBind();
        }

        private void PopulateAttendeesGrid()
        {
            gvAttendees.DataSource = GetAttendeesData();
            gvAttendees.DataBind();
        }

        private DataTable GetAttendeesData()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_ATTENDEE_GRID, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(ddlViewYear.SelectedValue))
                    {
                        comm.Parameters.AddWithValue("@Year", ddlViewYear.SelectedValue);
                    }

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                    }
                }
            }

            return new DataTable();
        }

        private DataTable SortData()
        {
            DataTable dtData = GetAttendeesData();
            var enumerableData = dtData.AsEnumerable();

            if (!string.IsNullOrEmpty(ViewState["sortColumn"].ToString()))
            {
                if (string.IsNullOrEmpty(ViewState["sortDirection"].ToString()) || ViewState["sortDirection"].ToString() == "ASC")
                {
                    enumerableData = enumerableData.OrderBy(r => r.Field<string>(ViewState["sortColumn"].ToString()));
                }
                else
                {
                    enumerableData = enumerableData.OrderByDescending(r => r.Field<string>(ViewState["sortColumn"].ToString()));
                }
            }

            return enumerableData.CopyToDataTable();
        }

        protected void gvAttendees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow gvr = gvAttendees.Rows[index];
                LoadCamps();
                LoadAttendeeData(gvAttendees.DataKeys[gvr.RowIndex].Value.ToString());

                pDetails.Visible = true;
                pGrid.Visible = false;
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
                            lblParticipantId.Text = id;
                            txtRagTag.Text = dr["TagNumber"].ToString() == "0" ? "" : dr["TagNumber"].ToString();
                            txtFirstName.Text = dr["FirstName"].ToString();
                            txtLastName.Text = dr["LastName"].ToString();
                            txtDateOfBirth.Text = dr["DateOfBirth"].ToString();
                            cbxIsMinor.Checked = dr["IsMinor"].ToString() == "Y" ? true : false;
                            txtAddress1.Text = dr["Address1"].ToString();
                            txtAddress2.Text = dr["Address2"].ToString();
                            txtCity.Text = dr["City"].ToString();
                            ddlState.SelectedValue = dr["State"].ToString();
                            txtZip.Text = dr["Zip"].ToString();
                            txtHomePhone.Text = dr["HomePhone"].ToString();
                            txtCellPhone.Text = dr["CellPhone"].ToString();
                            txtEmail.Text = dr["Email"].ToString();
                            txtCharacterName.Text = dr["CharacterName"].ToString();
                            txtChapterName.Text = dr["ChapterName"].ToString();
                            txtUnitName.Text = dr["UnitName"].ToString();
                            ddlCamp.SelectedValue = dr["CampId"].ToString();
                            cbxIsMerchant.Checked = dr["IsMerchant"].ToString() == "Y" ? true : false;
                            txtAuthCode.Text = dr["AuthCode"].ToString();
                            txtTransactionId.Text = dr["TransId"].ToString();
                            txtAmount.Text = Convert.ToDouble(dr["Amount"].ToString()).ToString("C", CultureInfo.CurrentCulture);
                            ddlStatus.SelectedValue = dr["Status"].ToString();
                            txtEmergencyContactName.Text = dr["EmergencyName"].ToString();
                            txtEmergencyContactPhone.Text = dr["EmergencyPhone"].ToString();
                            GetHealthIssues(id);
                            imgSignature.ImageUrl = dr["Signature"].ToString();
                        }
                    }
                }
            }
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
                        ListItem li = new ListItem("Camp Unknown - Will Select Later", "0");
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

        private void GetHealthIssues(string id)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_HEALTH_ISSUES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@par_id", id);

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            repHealthIssues.DataSource = ds;
                            repHealthIssues.DataBind();
                        }
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopulateAttendeesGrid();

            pDetails.Visible = false;
            pGrid.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveAttendee();

            PopulateAttendeesGrid();

            pDetails.Visible = false;
            pGrid.Visible = true;
        }

        private void SaveAttendee()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_SAVE_ATTENDEE, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@par_id", lblParticipantId.Text);
                    comm.Parameters.AddWithValue("@par_c_id", ddlCamp.SelectedValue);
                    if (!String.IsNullOrEmpty(txtRagTag.Text))
                    {
                        comm.Parameters.AddWithValue("@par_TagNumber", txtRagTag.Text);
                    }
                    comm.Parameters.AddWithValue("@par_FirstName", txtFirstName.Text);
                    comm.Parameters.AddWithValue("@par_LastName", txtLastName.Text);
                    comm.Parameters.AddWithValue("@par_DOB", txtDateOfBirth.Text);
                    comm.Parameters.AddWithValue("@par_CharacterName", txtCharacterName.Text);
                    comm.Parameters.AddWithValue("@par_ChapterName", txtChapterName.Text);
                    if (!String.IsNullOrEmpty(txtUnitName.Text))
                    {
                        comm.Parameters.AddWithValue("@par_UnitName", txtUnitName.Text);
                    }
                    comm.Parameters.AddWithValue("@par_Status", ddlStatus.SelectedValue);
                    comm.Parameters.AddWithValue("@par_IsMinor", cbxIsMinor.Checked ? "Y" : "N");
                    comm.Parameters.AddWithValue("@par_IsMerchant", cbxIsMerchant.Checked ? "Y" : "N");
                    // Address
                    comm.Parameters.AddWithValue("@add_Address1", txtAddress1.Text);
                    if (!String.IsNullOrEmpty(txtAddress2.Text))
                    {
                        comm.Parameters.AddWithValue("@add_Address2", txtAddress2.Text);
                    }
                    comm.Parameters.AddWithValue("@add_City", txtCity.Text);
                    comm.Parameters.AddWithValue("@add_State", ddlState.SelectedValue);
                    comm.Parameters.AddWithValue("@add_Zip", txtZip.Text);
                    // Contact Information
                    if (!String.IsNullOrEmpty(txtHomePhone.Text))
                    {
                        comm.Parameters.AddWithValue("@ci_HomePhone", txtHomePhone.Text);
                    }
                    if (!String.IsNullOrEmpty(txtCellPhone.Text))
                    {
                        comm.Parameters.AddWithValue("@ci_CellPhone", txtCellPhone.Text);
                    }
                    comm.Parameters.AddWithValue("@ci_Email", txtEmail.Text);
                    // Emergency Contact Information
                    comm.Parameters.AddWithValue("@ec_Name", txtEmergencyContactName.Text);
                    comm.Parameters.AddWithValue("@ec_Phone", txtEmergencyContactPhone.Text);

                    conn.Open();

                    comm.ExecuteNonQuery();
                }
            }
        }

        protected void btnResendConfirmation_Click(object sender, EventArgs e)
        {
            SendConfirmationEmail(lblParticipantId.Text);
        }

        private void SendConfirmationEmail(string id)
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
                            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

                            MailMessage mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
                            mailMessage.To.Add(new MailAddress(dr["Email"].ToString()));
                            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
                            if (dr["IsMerchant"].ToString() == "Y")
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

                            string body = "<p><b>" + dr["CharacterName"].ToString() + "</b>,</p>";
                            body += "<p>Thank you for registering to attend <b>Ragnarok XXXII</b>";
                            if (dr["IsMerchant"].ToString() == "Y")
                            {
                                body += " as a merchant";
                            }
                            body += "!</p>";

                            if (dr["CampId"].ToString() != "0")
                            {
                                string strCamp = "";
                                using (SqlConnection conn2 = new SqlConnection(Config.GetConnectionString()))
                                {
                                    using (SqlCommand comm2 = new SqlCommand(PROCEDURE_GET_CAMP, conn2))
                                    {
                                        comm2.CommandType = CommandType.StoredProcedure;
                                        comm2.Parameters.AddWithValue("@c_id", dr["CampId"].ToString());

                                        conn2.Open();

                                        using (SqlDataReader dr2 = comm2.ExecuteReader())
                                        {
                                            if (dr2.Read())
                                            {
                                                strCamp = dr2["CampName"].ToString();
                                            }
                                        }
                                    }
                                }

                                body += "<p>You are marked down as an attendee staying in <b>" + strCamp + "</b>. If your camp changes, please contact ";
                                body += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to change your camp, or you can change it ";
                                body += "when you arrive at Ragnarok.</p>";
                            }
                            else
                            {
                                body += "<p>You are marked down as not having a camp selected yet. If you choose a camp, please contact ";
                                body += "<a href=\"mailto:troll@dagorhirragnarok.com\">troll@dagorhirragnarok.com</a> to select your camp, or you can select your ";
                                body += "camp when you arrive at Ragnarok.</p>";
                            }

                            if (dr["IsMerchant"].ToString() != "Y")
                            {
                                body += "<p>Your selected arrival date is <b>" + (Convert.ToDateTime(dr["RegistrationDate"].ToString())).ToString("MM/dd/yyyy") + "</b>.</p>";
                            }

                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://dagorhirragnarok.com/CheckIn.aspx?pid=" + id, QRCodeGenerator.ECCLevel.Q);
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
                    }
                }
            }
        }

        protected void btnCampMasterUpdate_Click(object sender, EventArgs e)
        {
            SendCampMasterAttendees();
        }

        private void SendCampMasterAttendees()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CAMPS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Dictionary<string, string> hostSettings = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary();

                            MailMessage mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress("troll@dagorhirragnarok.com", "Ragnarok Troll");
                            mailMessage.To.Add(new MailAddress(dr["Email"].ToString()));
                            mailMessage.Bcc.Add(new MailAddress("jjp4674@gmail.com"));
                            mailMessage.Subject = "Ragnarok XXXIII - Camp Master Attendee Update";
                            mailMessage.IsBodyHtml = true;

                            string server = hostSettings["SMTPServer"];
                            string authentication = hostSettings["SMTPAuthentication"];
                            string password = hostSettings["SMTPPassword"];
                            string username = hostSettings["SMTPUsername"];

                            string body = "<p><b>" + dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + "</b>,</p>";
                            body += "<p>Here is an up-to-date list of the individuals who have registered to attend Ragnarok as part of <b>" + dr["CampName"].ToString() + "</b>: ";

                            body += GetCampAttendees(Convert.ToInt32(dr["Id"].ToString()));

                            body += "<p>We hope this information is helpful for your camp master duties.</p>";
                            body += "<p>Sincerely,<br />Ragnarok Staff</p>";

                            mailMessage.Body = body;

                            using (SmtpClient client = new SmtpClient(server))
                            {
                                client.Send(mailMessage);
                            }
                        }
                    }
                }
            }
        }

        private string GetCampAttendees(int campId)
        {
            string retText = "";

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CAMP_ATTENDEES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@CampId", campId);

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (retText != "")
                            {
                                retText += "<br />";
                            }

                            retText += "<b>" + dr["par_FirstName"].ToString() + " " + dr["par_LastName"].ToString() + "</b> - " + dr["par_CharacterName"].ToString() + " - Arriving: " + dr["par_RegistrationDate"].ToString();
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(retText))
            {
                retText = "None yet registered.";
            }

            return "<p>" + retText + "</p>";
        }

        protected void ddlViewYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateAttendeesGrid();
        }
    }
}