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

            DateTime startDate = Convert.ToDateTime(TextBoxStart.Text);
            DateTime endDate = Convert.ToDateTime(TextBoxEnd.Text);

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
                // Deactivate all elections first, so only one is ever active
                obj.SetData("UPDATE Election SET IsActive = 0");

                // Activate the selected one
                obj.SetData("UPDATE Election SET IsActive = 1 WHERE ElectionID = " + electionId);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Election activated.');", true);
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