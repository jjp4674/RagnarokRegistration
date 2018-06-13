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

namespace Ragnarok.Modules.RagnarokRegistration.CoopersPayment
{
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_GET_COOPER_TOTALS = "GetCooperTotals";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    GetCoopersCalculations();
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

        private void GetCoopersCalculations()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_COOPER_TOTALS, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["ArrivalDate"] != DBNull.Value)
                            {
                                if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 15)) // Friday 1
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblFriday1AdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblFriday1ChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 16)) // Saturday 1
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblSaturday1AdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblSaturday1ChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 17)) // Sunday
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblSundayAdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblSundayChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 18)) // Monday
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblMondayAdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblMondayChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 19)) // Tuesday
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblTuesdayAdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblTuesdayChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 20)) // Wednesday
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblWednesdayAdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblWednesdayChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 21)) // Thursday
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblThursdayAdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblThursdayChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 22)) // Friday 2
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblFriday2AdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblFriday2ChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                                else if (Convert.ToDateTime(dr["ArrivalDate"]) == new DateTime(2018, 6, 23)) // Saturday 2
                                {
                                    if (dr["IsMinor"].ToString() == "N")
                                    {
                                        lblSaturday2AdultNo.Text = dr["Total"].ToString();
                                    }
                                    else
                                    {
                                        lblSaturday2ChildNo.Text = dr["Total"].ToString();
                                    }
                                }
                            }
                        }


                        // Calculate Totals
                        lblFriday1SubtotalNo.Text = (Convert.ToInt32(lblFriday1AdultNo.Text) + Convert.ToInt32(lblFriday1ChildNo.Text)).ToString();
                        lblSaturday1SubtotalNo.Text = (Convert.ToInt32(lblSaturday1AdultNo.Text) + Convert.ToInt32(lblSaturday1ChildNo.Text)).ToString();
                        lblSundaySubtotalNo.Text = (Convert.ToInt32(lblSundayAdultNo.Text) + Convert.ToInt32(lblSundayChildNo.Text)).ToString();
                        lblMondaySubtotalNo.Text = (Convert.ToInt32(lblMondayAdultNo.Text) + Convert.ToInt32(lblMondayChildNo.Text)).ToString();
                        lblTuesdaySubtotalNo.Text = (Convert.ToInt32(lblTuesdayAdultNo.Text) + Convert.ToInt32(lblTuesdayChildNo.Text)).ToString();
                        lblWednesdaySubtotalNo.Text = (Convert.ToInt32(lblWednesdayAdultNo.Text) + Convert.ToInt32(lblWednesdayChildNo.Text)).ToString();
                        lblThursdaySubtotalNo.Text = (Convert.ToInt32(lblThursdayAdultNo.Text) + Convert.ToInt32(lblThursdayChildNo.Text)).ToString();
                        lblFriday2SubtotalNo.Text = (Convert.ToInt32(lblFriday2AdultNo.Text) + Convert.ToInt32(lblFriday2ChildNo.Text)).ToString();
                        lblSaturday2SubtotalNo.Text = (Convert.ToInt32(lblSaturday2AdultNo.Text) + Convert.ToInt32(lblSaturday2ChildNo.Text)).ToString();

                        // Calculate Costs

                        // Friday 1 = 10 days
                        // Saturday 1 = 9 days
                        // Sunday 1 = 8 days
                        // Monday = 7 days
                        // Tuesday = 6 days
                        // Wednesday = 5 days
                        // Thursday = 4 days
                        // Friday 2 = 3 days
                        // Saturday 2 = 2 days
                        // Sunday 2 = 1 day

                        double total = 0;
                        double subtotal = 0;

                        lblFriday1AdultTotal.Text = (Convert.ToInt32(lblFriday1AdultNo.Text) * (10 * 6)).ToString("C");
                        lblFriday1ChildTotal.Text = (Convert.ToInt32(lblFriday1ChildNo.Text) * (10 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblFriday1AdultNo.Text) * (10 * 6)) + (Convert.ToInt32(lblFriday1ChildNo.Text) * (10 * 3));
                        lblFriday1SubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblSaturday1AdultTotal.Text = (Convert.ToInt32(lblSaturday1AdultNo.Text) * (9 * 6)).ToString("C");
                        lblSaturday1ChildTotal.Text = (Convert.ToInt32(lblSaturday1ChildNo.Text) * (9 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblSaturday1AdultNo.Text) * (9 * 6)) + (Convert.ToInt32(lblSaturday1ChildNo.Text) * (9 * 3));
                        lblSaturday1SubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblSundayAdultTotal.Text = (Convert.ToInt32(lblSundayAdultNo.Text) * (8 * 6)).ToString("C");
                        lblSundayChildTotal.Text = (Convert.ToInt32(lblSundayChildNo.Text) * (8 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblSundayAdultNo.Text) * (8 * 6)) + (Convert.ToInt32(lblSundayChildNo.Text) * (8 * 3));
                        lblSundaySubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblMondayAdultTotal.Text = (Convert.ToInt32(lblMondayAdultNo.Text) * (7 * 6)).ToString("C");
                        lblMondayChildTotal.Text = (Convert.ToInt32(lblMondayChildNo.Text) * (7 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblMondayAdultNo.Text) * (7 * 6)) + (Convert.ToInt32(lblMondayChildNo.Text) * (7 * 3));
                        lblMondaySubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblTuesdayAdultTotal.Text = (Convert.ToInt32(lblTuesdayAdultNo.Text) * (6 * 6)).ToString("C");
                        lblTuesdayChildTotal.Text = (Convert.ToInt32(lblTuesdayChildNo.Text) * (6 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblTuesdayAdultNo.Text) * (6 * 6)) + (Convert.ToInt32(lblTuesdayChildNo.Text) * (6 * 3));
                        lblTuesdaySubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblWednesdayAdultTotal.Text = (Convert.ToInt32(lblWednesdayAdultNo.Text) * (5 * 6)).ToString("C");
                        lblWednesdayChildTotal.Text = (Convert.ToInt32(lblWednesdayChildNo.Text) * (5 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblWednesdayAdultNo.Text) * (5 * 6)) + (Convert.ToInt32(lblWednesdayChildNo.Text) * (5 * 3));
                        lblWednesdaySubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblThursdayAdultTotal.Text = (Convert.ToInt32(lblThursdayAdultNo.Text) * (4 * 6)).ToString("C");
                        lblThursdayChildTotal.Text = (Convert.ToInt32(lblThursdayChildNo.Text) * (4 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblThursdayAdultNo.Text) * (4 * 6)) + (Convert.ToInt32(lblThursdayChildNo.Text) * (4 * 3));
                        lblThursdaySubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblFriday2AdultTotal.Text = (Convert.ToInt32(lblFriday2AdultNo.Text) * (3 * 6)).ToString("C");
                        lblFriday2ChildTotal.Text = (Convert.ToInt32(lblFriday2ChildNo.Text) * (3 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblFriday2AdultNo.Text) * (3 * 6)) + (Convert.ToInt32(lblFriday2ChildNo.Text) * (3 * 3));
                        lblFriday2SubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblSaturday2AdultTotal.Text = (Convert.ToInt32(lblSaturday2AdultNo.Text) * (2 * 6)).ToString("C");
                        lblSaturday2ChildTotal.Text = (Convert.ToInt32(lblSaturday2ChildNo.Text) * (2 * 3)).ToString("C");
                        subtotal = (Convert.ToInt32(lblSaturday2AdultNo.Text) * (2 * 6)) + (Convert.ToInt32(lblSaturday2ChildNo.Text) * (2 * 3));
                        lblSaturday2SubtotalTotal.Text = subtotal.ToString("C");
                        total += subtotal;

                        lblTotalNo.Text = (Convert.ToInt32(lblFriday1AdultNo.Text) + Convert.ToInt32(lblFriday1ChildNo.Text) +
                                          Convert.ToInt32(lblSaturday1AdultNo.Text) + Convert.ToInt32(lblSaturday1ChildNo.Text) +
                                          Convert.ToInt32(lblSundayAdultNo.Text) + Convert.ToInt32(lblSundayChildNo.Text) +
                                          Convert.ToInt32(lblMondayAdultNo.Text) + Convert.ToInt32(lblMondayChildNo.Text) +
                                          Convert.ToInt32(lblTuesdayAdultNo.Text) + Convert.ToInt32(lblTuesdayChildNo.Text) +
                                          Convert.ToInt32(lblWednesdayAdultNo.Text) + Convert.ToInt32(lblWednesdayChildNo.Text) +
                                          Convert.ToInt32(lblThursdayAdultNo.Text) + Convert.ToInt32(lblThursdayChildNo.Text) +
                                          Convert.ToInt32(lblFriday2AdultNo.Text) + Convert.ToInt32(lblFriday2ChildNo.Text) +
                                          Convert.ToInt32(lblSaturday2AdultNo.Text) + Convert.ToInt32(lblSaturday2ChildNo.Text)).ToString();
                        lblTotalTotal.Text = total.ToString("C");
                    }
                }
            }
        }
    }
}