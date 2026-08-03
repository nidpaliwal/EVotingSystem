using System;
using System.Collections.Generic;
using System.Data;
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
            string s = "SELECT ElectionID, Title, StartDate, EndDate, IsActive FROM Election";
            DataTable dt = obj.GetData(s);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBoxTitle.Text == "" || TextBoxStart.Text == "" || TextBoxEnd.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill all fields.');", true);
                return;
            }

            DateTime startDate = DateTime.ParseExact(TextBoxStart.Text.Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(TextBoxEnd.Text.Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);


            if (endDate <= startDate)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('End date must be after start date.');", true);
                return;
            }

            string insertSql = "INSERT INTO Election (Title, StartDate, EndDate, IsActive) VALUES ('"
                + TextBoxTitle.Text.Trim() + "', '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '"
                + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";

            obj.SetData(insertSql);

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election created successfully.');", true);
            BindGrid();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int electionId = Convert.ToInt32(e.CommandArgument);

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

                DateTime newEndDate = Convert.ToDateTime(txtNewEndDate.Text);

                if (newEndDate <= DateTime.Now)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('End Date must be in the future to reactivate.');", true);
                    BindGrid();
                    return;
                }

                // Deactivate all elections first
                obj.SetData("UPDATE Election SET IsActive = 0");

                // Activate this one with the new EndDate
                obj.SetData("UPDATE Election SET IsActive = 1, EndDate = '" + newEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE ElectionID = " + electionId);

                // Fresh voting round
                obj.SetData("UPDATE Voter SET HasVoted = 0");

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election reactivated with new End Date.');", true);
            }
            else if (e.CommandName == "Deactivate")
            {
                obj.SetData("UPDATE Election SET IsActive = 0 WHERE ElectionID = " + electionId);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election deactivated.');", true);
            }

            BindGrid();
        }
    }
}