using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Ragnarok.Modules.RagnarokRegistration.FinancialSummary
{
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_GET_ADULT_PREREGS = "GetAdultPreRegs";
        private const string PROCEDURE_GET_CHILD_PREREGS = "GetChildPreRegs";
        private const string PROCEDURE_GET_MERCHANT_PREREGS = "GetMerchantPreRegs";
        private const string PROCEDURE_GET_ADULT_REGS = "GetAdultRegs";
        private const string PROCEDURE_GET_CHILD_REGS = "GetChildRegs";
        private const string PROCEDURE_GET_PRIOR_YEAR_BALANCE = "GetPriorYearBalance";
        private const string PROCEDURE_GET_AUTH_NET_FEES = "GetAuthNetFees";
        private const string PROCEDURE_GET_COOPERS_CALCULATION = "GetCoopersCalculation";
        private const string PROCEDURE_GET_INCOME_CATEGORIES = "GetIncomeCategories";
        private const string PROCEDURE_GET_EXPENSE_CATEGORIES = "GetExpenseCategories";
        private const string PROCEDURE_GET_BUDGET_ITEMS = "GetBudgetItems";

        private double priorYearTotal = 0;
        private double registrationsTotal = 0;
        private double incomesTotal = 0;
        private double expensesTotal = 0;
        private double subtotal = 0;
        private double authNetFees = 0;
        private double coopersEstimate = 0;
        private double coopersActual = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CalculateBudget();
                }
            }
            catch (Exception exc)
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

        private void CalculateBudget()
        {
            GetPriorYearBalance();

            GetAdultPreRegs();
            GetChildPreRegs();
            GetMerchantPreRegs();
            GetAdultRegs();
            GetChildRegs();
            lblRegistrationsSubtotal.Text = registrationsTotal.ToString("C");

            GetIncomeCategories();

            GetExpenseCategories();

            GetAuthNetFees();

            GetTotal();

            GetCoopersCalculations();
        }

        private void GetPriorYearBalance()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_PRIOR_YEAR_BALANCE, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblEndBalance.Text = Convert.ToDouble(dr["Amount"].ToString()).ToString("C");

                            priorYearTotal += Convert.ToDouble(dr["Amount"].ToString());
                        }
                    }
                }
            }
        }

        private void GetAdultPreRegs()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_ADULT_PREREGS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblAdultPreRegNo.Text = dr["AdultPreRegNo"].ToString();
                            lblAdultPreRegTotal.Text = Convert.ToDouble(dr["AdultPreRegTotal"].ToString()).ToString("C");

                            registrationsTotal += Convert.ToDouble(dr["AdultPreRegTotal"].ToString());
                        }
                    }
                }
            }
        }

        private void GetChildPreRegs()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CHILD_PREREGS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblChildPreRegNo.Text = dr["ChildPreRegNo"].ToString();
                            lblChildPreRegTotal.Text = Convert.ToDouble(dr["ChildPreRegTotal"].ToString()).ToString("C");

                            registrationsTotal += Convert.ToDouble(dr["ChildPreRegTotal"].ToString());
                        }
                    }
                }
            }
        }

        private void GetMerchantPreRegs()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_MERCHANT_PREREGS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblMerchantPreRegNo.Text = dr["MerchantPreRegNo"].ToString();
                            lblMerchantPreRegTotal.Text = Convert.ToDouble(dr["MerchantPreRegTotal"].ToString()).ToString("C");

                            registrationsTotal += Convert.ToDouble(dr["MerchantPreRegTotal"].ToString());
                        }
                    }
                }
            }
        }

        private void GetAdultRegs()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_ADULT_REGS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblAdultRegNo.Text = dr["AdultRegNo"].ToString();
                            lblAdultRegTotal.Text = Convert.ToDouble(dr["AdultRegTotal"].ToString()).ToString("C");

                            registrationsTotal += Convert.ToDouble(dr["AdultRegTotal"].ToString());
                        }
                    }
                }
            }
        }

        private void GetChildRegs()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CHILD_REGS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblChildRegNo.Text = dr["ChildRegNo"].ToString();
                            lblChildRegTotal.Text = Convert.ToDouble(dr["ChildRegTotal"].ToString()).ToString("C");

                            registrationsTotal += Convert.ToDouble(dr["ChildRegTotal"].ToString());
                        }
                    }
                }
            }
        }

        private void GetAuthNetFees()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_AUTH_NET_FEES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblAuthNetFees.Text = Convert.ToDouble(dr["AuthNetTotal"].ToString()).ToString("C");

                            authNetFees += Convert.ToDouble(dr["AuthNetTotal"].ToString());

                            if (authNetFees < 0)
                            {
                                lblAuthNetFees.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void GetTotal()
        {
            double total = priorYearTotal + registrationsTotal + incomesTotal + expensesTotal + authNetFees;

            lblCurrentTotalBalance.Text = total.ToString("C");

            if (total < 0)
            {
                lblCurrentTotalBalance.ForeColor = Color.Red;
            }
        }

        private void GetCoopersCalculations()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_COOPERS_CALCULATION, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblActualCooperPayment.Text = Convert.ToDouble(dr["Actual"].ToString()).ToString("C");
                            lblExpectedCooperPayment.Text = Convert.ToDouble(dr["PreRegEstimate"].ToString()).ToString("C");

                            coopersActual += Convert.ToDouble(dr["Actual"].ToString());
                            coopersEstimate += Convert.ToDouble(dr["PreRegEstimate"].ToString());

                            if (coopersActual < 0)
                            {
                                lblActualCooperPayment.ForeColor = Color.Red;
                            }
                            if (coopersEstimate < 0)
                            {
                                lblExpectedCooperPayment.ForeColor = Color.Red;
                            }

                            double actualTotal = priorYearTotal + registrationsTotal + incomesTotal + expensesTotal + authNetFees + coopersActual;

                            lblCurrentAvailableBalance.Text = actualTotal.ToString("C");

                            if (actualTotal < 0)
                            {
                                lblCurrentAvailableBalance.ForeColor = Color.Red;
                            }

                            double estimatedTotal = priorYearTotal + registrationsTotal + incomesTotal + expensesTotal + authNetFees + coopersEstimate;

                            lblCurrentEstimatedBalance.Text = estimatedTotal.ToString("C");

                            if (estimatedTotal < 0)
                            {
                                lblCurrentEstimatedBalance.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void GetIncomeCategories()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_INCOME_CATEGORIES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            repIncome.DataSource = ds;
                            repIncome.DataBind();
                        }
                    }
                }
            }
        }

        protected void repIncome_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Literal litCategory = (Literal)e.Item.FindControl("litCategory");
                litCategory.Text = "<b>" + drv["Category"].ToString() + "</b>";

                Repeater repIncomeItem = (Repeater)e.Item.FindControl("repIncomeItem");
                GetBudgetItems(drv["CategoryId"].ToString(), repIncomeItem);
            }
        }

        protected void repIncomeItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                
                Literal litText = (Literal)e.Item.FindControl("litText");
                litText.Text = GetText(drv);

                if (drv["Number"] != null && !string.IsNullOrEmpty(drv["Number"].ToString()))
                {
                    Label lblNo = (Label)e.Item.FindControl("lblNo");
                    lblNo.Text = drv["Number"].ToString();
                }

                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                double amount = Convert.ToDouble(drv["Amount"].ToString());
                lblTotal.Text = amount.ToString("C");

                if (amount < 0)
                {
                    lblTotal.ForeColor = Color.Red;
                }

                subtotal += amount;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
                lblSubtotal.Text = subtotal.ToString("C");

                if (subtotal < 0)
                {
                    lblSubtotal.ForeColor = Color.Red;
                }

                incomesTotal += subtotal;
            }
        }

        private void GetExpenseCategories()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_EXPENSE_CATEGORIES, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            repExpense.DataSource = ds;
                            repExpense.DataBind();
                        }
                    }
                }
            }
        }

        protected void repExpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Literal litCategory = (Literal)e.Item.FindControl("litCategory");
                litCategory.Text = "<b>" + drv["Category"].ToString() + "</b>";

                Repeater repExpenseItem = (Repeater)e.Item.FindControl("repExpenseItem");
                GetBudgetItems(drv["CategoryId"].ToString(), repExpenseItem);
            }
        }

        protected void repExpenseItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                Literal litText = (Literal)e.Item.FindControl("litText");
                litText.Text = GetText(drv);

                if (drv["Number"] != null && !string.IsNullOrEmpty(drv["Number"].ToString()))
                {
                    Label lblNo = (Label)e.Item.FindControl("lblNo");
                    lblNo.Text = drv["Number"].ToString();
                }

                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                double amount = Convert.ToDouble(drv["Amount"].ToString());
                lblTotal.Text = amount.ToString("C");

                if (amount < 0)
                {
                    lblTotal.ForeColor = Color.Red;
                }

                subtotal += amount;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
                lblSubtotal.Text = subtotal.ToString("C");

                if (subtotal < 0)
                {
                    lblSubtotal.ForeColor = Color.Red;
                }

                expensesTotal += subtotal;
            }
        }

        private void GetBudgetItems(string id, Repeater repItem)
        {
            subtotal = 0;

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_BUDGET_ITEMS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@c_id", id);

                    conn.Open();

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            repItem.DataSource = ds;
                            repItem.DataBind();
                        }
                    }
                }
            }
        }

        private string GetText(DataRowView drv)
        {
            string text = "<b>" + drv["Text"].ToString() + "</b>";
            string subText = "";
            string subSubText = "";

            if (drv["Description"] != null && !string.IsNullOrEmpty(drv["Description"].ToString()))
            {
                subText += drv["Description"].ToString();
            }

            if (drv["Recipient"] != null && !string.IsNullOrEmpty(drv["Recipient"].ToString()))
            {
                subSubText += "Recipient - " + drv["Recipient"].ToString();
            }
            if (drv["PayedBy"] != null && !string.IsNullOrEmpty(drv["PayedBy"].ToString()))
            {
                if (subSubText != "")
                {
                    subSubText += " | ";
                }
                subSubText += "Payed By - " + drv["PayedBy"].ToString();
            }
            if (drv["PayedDate"] != null && !string.IsNullOrEmpty(drv["PayedDate"].ToString()))
            {
                if (subSubText != "")
                {
                    subSubText += " | ";
                }
                subSubText += "Paid Date - " + drv["PayedDate"].ToString();
            }
            if (drv["CheckNo"] != null && !string.IsNullOrEmpty(drv["CheckNo"].ToString()))
            {
                if (subSubText != "")
                {
                    subSubText += " | ";
                }
                subSubText += "Check #" + drv["CheckNo"].ToString();
            }

            if (subSubText != "")
            {
                if (subText != "")
                {
                    subText += "<br />";
                }

                subText += subSubText;
            }

            if (subText != "")
            {
                text += "<br /><span class=\"small\">" + subText + "</span>";
            }

            return text;
        }
    }
}