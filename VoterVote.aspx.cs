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
    public partial class VoterVote : System.Web.UI.Page
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
                CheckEligibilityAndLoad();
            }
        }

        private void CheckEligibilityAndLoad()
        {
            string email = Session["Email"].ToString();

            string voterQuery = "SELECT Status, HasVoted FROM Voter WHERE Email=@Email";
            DataTable dtVoter = obj.GetData(voterQuery, new SqlParameter("@Email", email));

            if (dtVoter.Rows.Count == 0)
            {
                lblMessage.Text = "Voter record not found.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            string status = dtVoter.Rows[0]["Status"].ToString();
            bool hasVoted = Convert.ToBoolean(dtVoter.Rows[0]["HasVoted"]);

            if (status != "Approved")
            {
                lblMessage.Text = "Your registration is not yet approved. You cannot vote.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            if (hasVoted)
            {
                lblMessage.Text = "You have already voted. Thank you for participating.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            // Check active election whose voting window covers now
            string electionQuery = "SELECT ElectionID, Title, StartDate, EndDate FROM Election WHERE IsActive=1 AND GETDATE() BETWEEN StartDate AND EndDate";
            DataTable dtElection = obj.GetData(electionQuery);

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "The election is not currently open for voting.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            string title = dtElection.Rows[0]["Title"].ToString();
            DateTime startDate = Convert.ToDateTime(dtElection.Rows[0]["StartDate"]);
            DateTime endDate = Convert.ToDateTime(dtElection.Rows[0]["EndDate"]);

            lblElectionInfo.Text = title + " (" + startDate.ToString("dd-MMM-yyyy hh:mm tt") + " to " + endDate.ToString("dd-MMM-yyyy hh:mm tt") + ")";

            // All checks passed — load approved parties
            string partyQuery = "SELECT PartyID, PartyName, SymbolImagePath FROM Party WHERE Status='Approved'";
            DataTable dtParty = obj.GetData(partyQuery);
            GridView1.DataSource = dtParty;
            GridView1.DataBind();
        }

        protected void ButtonVote_Click(object sender, EventArgs e)
        {
            string selectedPartyId = Request.Form["PartyChoice"];

            if (string.IsNullOrEmpty(selectedPartyId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a party.');", true);
                CheckEligibilityAndLoad();
                return;
            }

            // The radio value arrives as untrusted client input — validate it
            // is a plain integer before using it.
            int partyId;
            if (!int.TryParse(selectedPartyId, out partyId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid party selection.');", true);
                CheckEligibilityAndLoad();
                return;
            }

            string email = Session["Email"].ToString();

            // Fast-path pre-check for good UX; the authoritative re-checks
            // happen inside CastVote's transaction.
            string voterQuery = "SELECT VoterID FROM Voter WHERE Email=@Email";
            DataTable dtVoter = obj.GetData(voterQuery, new SqlParameter("@Email", email));

            if (dtVoter.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You are not eligible to vote.');", true);
                return;
            }

            int voterId = Convert.ToInt32(dtVoter.Rows[0]["VoterID"]);

            string error;
            try
            {
                error = obj.CastVote(voterId, partyId);
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voting is temporarily unavailable. Please try again.');", true);
                CheckEligibilityAndLoad();
                return;
            }
            if (error != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + error.Replace("'", "\\'") + "');", true);
                CheckEligibilityAndLoad();
                return;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Your vote has been recorded successfully. Thank you.');", true);

            GridView1.Visible = false;
            ButtonVote.Visible = false;
            lblMessage.Text = "You have already voted. Thank you for participating.";
        }
    }
}
