using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class ResultParty : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Party")
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
            int electionId;

            if (Request.QueryString["id"] != null)
            {
                electionId = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                string fallbackQuery = "SELECT TOP 1 ElectionID FROM Election ORDER BY StartDate DESC";
                DataTable dtFallback = obj.GetData(fallbackQuery);
                if (dtFallback.Rows.Count == 0)
                {
                    lblMessage.Text = "No election found.";
                    GridView1.Visible = false;
                    return;
                }
                electionId = Convert.ToInt32(dtFallback.Rows[0]["ElectionID"]);
            }

            string electionQuery = "SELECT Title, EndDate, IsActive FROM Election WHERE ElectionID=" + electionId;
            DataTable dtElection = obj.GetData(electionQuery);

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "Election not found.";
                GridView1.Visible = false;
                return;
            }

            string title = dtElection.Rows[0]["Title"].ToString();
            DateTime endDate = Convert.ToDateTime(dtElection.Rows[0]["EndDate"]);
            bool isActive = Convert.ToBoolean(dtElection.Rows[0]["IsActive"]);

            bool isOngoing = isActive && (endDate > DateTime.Now);

            if (isOngoing)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Result will be out soon. \"" + title + "\" is currently in progress.";
                GridView1.Visible = false;
                return;
            }

            GridView1.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Black;
            lblMessage.Text = "Results for: " + title;

            string resultsQuery = @"
                SELECT p.PartyName, p.SymbolImagePath, COUNT(v.VoteID) AS VoteCount
                FROM Party p
                LEFT JOIN Votes v ON p.PartyID = v.PartyID AND v.ElectionID = " + electionId + @"
                WHERE p.Status = 'Approved'
                GROUP BY p.PartyName, p.SymbolImagePath
                ORDER BY VoteCount DESC";

            DataTable dtResults = obj.GetData(resultsQuery);
            GridView1.DataSource = dtResults;
            GridView1.DataBind();
        }
    }
}