using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class ElectionControl : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string s = "SELECT ElectionID, Title, StartDate, EndDate, IsActive, AuthorityName, AuthorityNumber FROM Election";
            DataTable dt = obj.GetData(s);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBoxTitle.Text == "" || TextBoxStart.Text == "" || TextBoxEnd.Text == "" || TextBoxAuthorityName.Text == "" || TextBoxAuthorityNumber.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill all fields.');", true);
                return;
            }

            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParseExact(TextBoxStart.Text.Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startDate)
                || !DateTime.TryParseExact(TextBoxEnd.Text.Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endDate))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter dates in yyyy-MM-dd format.');", true);
                return;
            }

            if (endDate <= startDate)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('End date must be after start date.');", true);
                return;
            }

            string insertSql = "INSERT INTO Election (Title, StartDate, EndDate, IsActive, AuthorityName, AuthorityNumber) VALUES (@Title, @StartDate, @EndDate, 0, @AuthorityName, @AuthorityNumber)";
            obj.SetData(insertSql,
                new SqlParameter("@Title", TextBoxTitle.Text.Trim()),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@AuthorityName", TextBoxAuthorityName.Text.Trim()),
                new SqlParameter("@AuthorityNumber", TextBoxAuthorityNumber.Text.Trim()));

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election created successfully.');", true);
            BindGrid();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int electionId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out electionId))
                return;

            if (e.CommandName == "Activate")
            {
                // Find the row's new EndDate textbox
                GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtNewEndDate = (TextBox)row.FindControl("txtNewEndDate");

                if (string.IsNullOrWhiteSpace(txtNewEndDate.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a new End Date to reactivate this election.');", true);
                    BindGrid();
                    return;
                }

                DateTime newEndDate;
                if (!DateTime.TryParse(txtNewEndDate.Text, out newEndDate))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid End Date format.');", true);
                    BindGrid();
                    return;
                }

                if (newEndDate <= DateTime.Now)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('End Date must be in the future to reactivate.');", true);
                    BindGrid();
                    return;
                }

                // Multiple elections can be active at the same time, so
                // do NOT touch other elections here. Activate this one
                // with its new EndDate only.
                obj.SetData("UPDATE Election SET IsActive = 1, EndDate = @EndDate WHERE ElectionID = @ElectionID",
                    new SqlParameter("@EndDate", newEndDate),
                    new SqlParameter("@ElectionID", electionId));

                // Per-election voting eligibility is tracked in the Votes
                // table, so there is no global "fresh voting round" flag to
                // reset (HasVoted is a legacy summary and stays as-is).

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election activated with new End Date.');", true);
            }
            else if (e.CommandName == "Deactivate")
            {
                obj.SetData("UPDATE Election SET IsActive = 0 WHERE ElectionID = @ElectionID",
                    new SqlParameter("@ElectionID", electionId));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election deactivated.');", true);
            }

            BindGrid();
        }
    }
}
