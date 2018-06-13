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
using Ragnarok.Modules.RagnarokRegistration;

namespace Ragnarok.Modules.RagnarokRegistration.Admin.Camps
{
    public partial class View : RagnarokRegistrationModuleBase, IActionable
    {
        private const string PROCEDURE_GET_CAMP_GRID = "GetAdminCamps";
        private const string PROCEDURE_GET_CAMP = "GetCamp";
        private const string PROCEDURE_SAVE_CAMP = "SaveCamp";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ViewState["sortColumn"] = "";
                    ViewState["sortDirection"] = "";
                    PopulateCampsGrid();
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

        private void PopulateCampsGrid()
        {
            gvCamps.DataSource = GetCampsData();
            gvCamps.DataBind();
        }

        private DataTable GetCampsData()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CAMP_GRID, conn))
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

        protected void gvCamps_Sorting(object sender, GridViewSortEventArgs e)
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

            gvCamps.DataSource = SortData();
            gvCamps.DataBind();
        }

        private DataTable SortData()
        {
            DataTable dtData = GetCampsData();
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

        protected void gvCamps_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow gvr = gvCamps.Rows[index];
                ClearCamp();
                LoadCampData(gvCamps.DataKeys[gvr.RowIndex].Value.ToString());

                pDetails.Visible = true;
                pGrid.Visible = false;
            }
        }

        private void LoadCampData(string id)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_GET_CAMP, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@c_id", id);

                    conn.Open();

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblCampId.Text = id;
                            txtCampName.Text = dr["CampName"].ToString();
                            txtCampLocation.Text = dr["CampLocation"].ToString();
                            txtFirstName.Text = dr["FirstName"].ToString();
                            txtLastName.Text = dr["LastName"].ToString();
                            txtCharacterName.Text = dr["CharacterName"].ToString();
                            txtEmail.Text = dr["Email"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopulateCampsGrid();

            pDetails.Visible = false;
            pGrid.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();

            PopulateCampsGrid();

            pDetails.Visible = false;
            pGrid.Visible = true;
        }

        private void Save()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand(PROCEDURE_SAVE_CAMP, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@c_id", lblCampId.Text);
                    comm.Parameters.AddWithValue("@c_CampName", txtCampName.Text);
                    comm.Parameters.AddWithValue("@c_CampLocation", txtCampLocation.Text);
                    comm.Parameters.AddWithValue("@cm_FirstName", txtFirstName.Text);
                    comm.Parameters.AddWithValue("@cm_LastName", txtLastName.Text);
                    comm.Parameters.AddWithValue("@cm_CharacterName", txtCharacterName.Text);
                    comm.Parameters.AddWithValue("@cm_Email", txtEmail.Text);
                    comm.Parameters.AddWithValue("@c_Year", ddlYear.SelectedValue);

                    conn.Open();

                    int retVal = 0;
                    try
                    {
                        retVal = comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "An error occurred with the save.";
                        lblStatus.Visible = true;
                    }

                    if (retVal == 0)
                    {
                        lblStatus.Text = "An error occurred with the save.";
                        lblStatus.Visible = true;
                    }
                    else
                    {
                        PopulateCampsGrid();

                        pDetails.Visible = false;
                        pGrid.Visible = true;
                    }
                }
            }
        }

        protected void gvCamps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCamps.PageIndex = e.NewPageIndex;
            gvCamps.DataSource = SortData();
            gvCamps.DataBind();
        }

        protected void btnAddCamp_Click(object sender, EventArgs e)
        {
            ClearCamp();
            lblCampId.Text = "0";

            pDetails.Visible = true;
            pGrid.Visible = false;
        }

        private void ClearCamp()
        {
            lblCampId.Text = "0";
            txtCampName.Text = "";
            txtCampLocation.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCharacterName.Text = "";
            txtEmail.Text = "";
        }

        protected void btnGeneratePrintOut_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCampsData();
            string strPrintOut = "";

            var orderedData = dt.AsEnumerable().OrderBy(r => r["CampName"].ToString()).CopyToDataTable();

            foreach (DataRow dr in orderedData.Rows)
            {
                if (strPrintOut.Length > 0)
                {
                    strPrintOut += "<br />";
                }
                strPrintOut += dr["CampName"].ToString() + " - " + dr["CampCount"].ToString();
            }

            litPrintOut.Text = strPrintOut;

            pGrid.Visible = false;
            pPrintOut.Visible = true;
        }

        protected void btnPrintCancel_Click(object sender, EventArgs e)
        {
            PopulateCampsGrid();

            pPrintOut.Visible = false;
            pGrid.Visible = true;
        }

        protected void ddlViewYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCampsGrid();
        }
    }
}