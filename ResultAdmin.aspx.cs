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
            int electionId;

            if (Request.QueryString["id"] != null)
            {
                if (!int.TryParse(Request.QueryString["id"], out electionId))
                {
                    lblMessage.Text = "Invalid election id.";
                    divSessionInfo.Visible = false;
                    GridView1.Visible = false;
                    return;
                }
            }
            else
            {
                string fallbackQuery = "SELECT TOP 1 ElectionID FROM Election ORDER BY StartDate DESC";
                DataTable dtFallback = obj.GetData(fallbackQuery);
                if (dtFallback.Rows.Count == 0)
                {
                    lblMessage.Text = "No election found.";
                    divSessionInfo.Visible = false;
                    GridView1.Visible = false;
                    return;
                }
                electionId = Convert.ToInt32(dtFallback.Rows[0]["ElectionID"]);
            }

            string electionQuery = "SELECT Title, StartDate, EndDate, IsActive, AuthorityName, AuthorityNumber FROM Election WHERE ElectionID=@ElectionID";
            DataTable dtElection = obj.GetData(electionQuery, new SqlParameter("@ElectionID", electionId));

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "Election not found.";
                divSessionInfo.Visible = false;
                GridView1.Visible = false;
                return;
            }

            DataRow er = dtElection.Rows[0];
            string title = er["Title"].ToString();
            DateTime startDate = Convert.ToDateTime(er["StartDate"]);
            DateTime endDate = Convert.ToDateTime(er["EndDate"]);
            bool isActive = Convert.ToBoolean(er["IsActive"]);

            bool isOngoing = isActive && (endDate > DateTime.Now);

            // Session info header (printed with the result sheet)
            lblElectionTitle.Text = title;
            lblElectionPeriod.Text = startDate.ToString("dd-MMM-yyyy hh:mm tt") + " to " + endDate.ToString("dd-MMM-yyyy hh:mm tt");
            lblAuthorityName.Text = er["AuthorityName"] == DBNull.Value || string.IsNullOrWhiteSpace(er["AuthorityName"].ToString()) ? "—" : er["AuthorityName"].ToString();
            lblAuthorityNumber.Text = er["AuthorityNumber"] == DBNull.Value || string.IsNullOrWhiteSpace(er["AuthorityNumber"].ToString()) ? "—" : er["AuthorityNumber"].ToString();
            lblElectionStatus.Text = isOngoing ? "Ongoing (Live)" : "Concluded";
            lblTotalVotes.Text = GetTotalVotes(electionId).ToString();
            lblTotalVoters.Text = GetTotalVoters().ToString();

            // No "ongoing" check here — Admin always sees live results, even mid-election
            GridView1.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Black;
            lblMessage.Text = "Results for: " + title;

            string resultsQuery = @"
                SELECT p.PartyName, p.LeaderName, p.SymbolImagePath, p.LeaderPhotoPath, COUNT(v.VoteID) AS VoteCount
                FROM Party p
                LEFT JOIN Votes v ON p.PartyID = v.PartyID AND v.ElectionID = @ElectionID
                WHERE p.Status = 'Approved'
                GROUP BY p.PartyName, p.LeaderName, p.SymbolImagePath, p.LeaderPhotoPath
                ORDER BY VoteCount DESC";

            DataTable dtResults = obj.GetData(resultsQuery, new SqlParameter("@ElectionID", electionId));
            GridView1.DataSource = dtResults;
            GridView1.DataBind();
        }

        private int GetTotalVotes(int electionId)
        {
            DataTable dt = obj.GetData("SELECT COUNT(*) AS TotalVotes FROM Votes WHERE ElectionID=@ElectionID", new SqlParameter("@ElectionID", electionId));
            return dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0]["TotalVotes"]);
        }

        private int GetTotalVoters()
        {
            DataTable dt = obj.GetData("SELECT COUNT(*) AS TotalVoters FROM Voter WHERE Status='Approved'");
            return dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0]["TotalVoters"]);
        }

    }
}