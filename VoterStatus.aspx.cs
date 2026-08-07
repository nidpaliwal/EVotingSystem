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
    public partial class VoterStatus : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Voter")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStatus();
            }
        }

        private void LoadStatus()
        {
            string email = Session["Email"].ToString();
            string s = "SELECT VoterID, Name, Status, DeclineReason FROM Voter WHERE Email=@Email";
            DataTable dt = obj.GetData(s, new SqlParameter("@Email", email));

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblName.Text = row["Name"].ToString();
                lblStatus.Text = row["Status"].ToString();

                if (row["Status"].ToString() == "Declined")
                {
                    lblDeclineReason.Text = row["DeclineReason"] == DBNull.Value ? "" : row["DeclineReason"].ToString();
                    trDeclineReason.Visible = true;
                }
                else
                {
                    trDeclineReason.Visible = false;
                }

                int voterId = Convert.ToInt32(row["VoterID"]);
                string voteQuery = "SELECT COUNT(*) AS VoteCount FROM Votes WHERE VoterID=@VoterID";
                DataTable dtVotes = obj.GetData(voteQuery, new SqlParameter("@VoterID", voterId));
                int voteCount = dtVotes.Rows.Count > 0 ? Convert.ToInt32(dtVotes.Rows[0]["VoteCount"]) : 0;

                lblHasVoted.Text = voteCount == 0
                    ? "You have not voted in any election yet"
                    : (voteCount == 1
                        ? "You have voted in 1 election"
                        : "You have voted in " + voteCount + " elections");
            }
        }

    }
}