using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class Result : System.Web.UI.Page
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
                LoadResults();
            }
        }

        private void LoadResults()
        {
            string electionQuery = "SELECT TOP 1 ElectionID FROM Election ORDER BY StartDate DESC";
            DataTable dtElection = obj.GetData(electionQuery);

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "No election found.";
                GridView1.Visible = false;
                return;
            }

            int electionId = Convert.ToInt32(dtElection.Rows[0]["ElectionID"]);

            string resultsQuery = @"

                SELECT p.PartyName, p.SymbolImagePath, COUNT(v.VoteID) AS VoteCount

                FROM Party p

                LEFT JOIN Votes v ON p.PartyID = v.PartyID AND v.ElectionID = " + electionId + @"

                WHERE p.Status = 'Approved'

                GROUP BY p.PartyName, p.SymbolImagePath

                ORDER BY VoteCount DESC";



            DataTable dtResults = obj.GetData(resultsQuery);

            GridView1.Visible = true;

            lblMessage.Text = "";

            GridView1.DataSource = dtResults;

            GridView1.DataBind();

        }

    }
}