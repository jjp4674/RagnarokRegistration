using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ragnarok.Modules.RagnarokRegistration.RagU
{
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_COUNT_PARTICIPANTS = "CountParticipants";
        private const string PROCEDURE_ADD_RAGU_PARTICIPANT = "AddRagUParticipant";
        private const string PROCEDURE_ADD_ERROR = "AddError";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int remaining = 15 - GetSpaces("Leather");
                if (remaining == 0)
                {
                    cbxLeather.Enabled = false;
                    lblLeatherRemaining.ForeColor = System.Drawing.Color.Red;
                }
                lblLeatherRemaining.Text = remaining.ToString();

                remaining = 10 - GetSpaces("Plastidip");
                if (remaining == 0)
                {
                    cbxPlastiDip.Enabled = false;
                    lblPlastiRemaining.ForeColor = System.Drawing.Color.Red;
                }
                lblPlastiRemaining.Text = remaining.ToString();

                CalculateCost();
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

        private void CalculateCost()
        {
            decimal cost = 0.0m;

            if (cbxLeather.Checked)
            {
                cost += 55;
            }
            if (cbxPlastiDip.Checked)
            {
                cost += 100;
            }

            lblCost.Text = cost.ToString("C");
            litCost.Text = cost.ToString();
        }

        private int GetSpaces(string track)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = PROCEDURE_COUNT_PARTICIPANTS;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Track", track);

                    conn.Open();

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        protected void cbxLeather_CheckedChanged(object sender, EventArgs e)
        {
            CalculateCost();
        }

        protected void cbxPlastiDip_CheckedChanged(object sender, EventArgs e)
        {
            CalculateCost();
        }

        protected void btnRegister_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (ProcessPayment())
                    {
                        pTrack.Visible = false;
                        pConfirmation.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());

                    pTrack.Visible = false;
                    pError.Visible = true;
                }
            }
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
                amount = Convert.ToDecimal(litCost.Text),
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
                    RegisterClasses();
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
                cardError.Text = "<br /><br />Error! There was a problem submitting your payment.  Please try again.  If you are still unsuccessful, please contact Ragnarok University.";
                return false;
            }
        }

        public void RegisterClasses()
        {
            if (cbxLeather.Checked)
            {
                RegisterClass("Leather");
            }

            if (cbxPlastiDip.Checked)
            {
                RegisterClass("Plastidip");
            }
        }

        public void RegisterClass(string track)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = PROCEDURE_ADD_RAGU_PARTICIPANT;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Track", track);
                    cmd.Parameters.AddWithValue("@FName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);

                    conn.Open();

                    cmd.ExecuteNonQuery();
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

        protected void cusChecks_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (!cbxLeather.Checked && !cbxPlastiDip.Checked)
            {
                args.IsValid = false;
            }
        }
    }
}