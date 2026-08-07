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
        private int currentVoterId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Voter")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            ResolveCurrentVoterId();

            if (!IsPostBack)
            {
                CheckEligibilityAndLoad();
            }
        }

        // Resolves the logged-in voter's ID on every request (not just the
        // initial GET). Without this, postbacks ran the per-election
        // "already voted" check with VoterID = -1 and wrongly showed the
        // ballot for elections the voter had already voted in.
        private void ResolveCurrentVoterId()
        {
            if (Session["Email"] == null) return;

            string email = Session["Email"].ToString();
            DataTable dtVoter = obj.GetData(
                "SELECT VoterID FROM Voter WHERE Email=@Email",
                new SqlParameter("@Email", email));
            if (dtVoter.Rows.Count > 0)
                currentVoterId = Convert.ToInt32(dtVoter.Rows[0]["VoterID"]);
        }

        private void CheckEligibilityAndLoad()
        {
            string email = Session["Email"].ToString();

            string voterQuery = "SELECT VoterID, Status FROM Voter WHERE Email=@Email";
            DataTable dtVoter = obj.GetData(voterQuery, new SqlParameter("@Email", email));

            if (dtVoter.Rows.Count == 0)
            {
                lblMessage.Text = "Voter record not found.";
                ddlElection.Visible = false;
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            string status = dtVoter.Rows[0]["Status"].ToString();
            currentVoterId = Convert.ToInt32(dtVoter.Rows[0]["VoterID"]);

            if (status != "Approved")
            {
                lblMessage.Text = "Your registration is not yet approved. You cannot vote.";
                ddlElection.Visible = false;
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            // Load all active elections whose voting window covers now.
            string electionQuery = "SELECT ElectionID, Title, StartDate, EndDate FROM Election WHERE IsActive=1 AND GETDATE() BETWEEN StartDate AND EndDate ORDER BY StartDate";
            DataTable dtElections = obj.GetData(electionQuery);

            if (dtElections.Rows.Count == 0)
            {
                lblMessage.Text = "No election is currently open for voting.";
                ddlElection.Visible = false;
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            ddlElection.DataSource = dtElections;
            ddlElection.DataTextField = "Title";
            ddlElection.DataValueField = "ElectionID";
            ddlElection.DataBind();
            ddlElection.Visible = true;

            LoadBallotForSelectedElection();
        }

        protected void ddlElection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBallotForSelectedElection();
        }

        private void LoadBallotForSelectedElection()
        {
            int electionId = int.Parse(ddlElection.SelectedValue);

            string electionQuery = "SELECT Title, StartDate, EndDate FROM Election WHERE ElectionID=@ElectionID";
            DataTable dtElection = obj.GetData(electionQuery, new SqlParameter("@ElectionID", electionId));

            if (dtElection.Rows.Count == 0)
            {
                lblElectionInfo.Text = "";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            DateTime startDate = Convert.ToDateTime(dtElection.Rows[0]["StartDate"]);
            DateTime endDate = Convert.ToDateTime(dtElection.Rows[0]["EndDate"]);
            lblElectionInfo.Text = dtElection.Rows[0]["Title"].ToString() + " (" + startDate.ToString("dd-MMM-yyyy hh:mm tt") + " to " + endDate.ToString("dd-MMM-yyyy hh:mm tt") + ")";

            // Per-election check: has this voter already voted here?
            string votedQuery = "SELECT COUNT(*) AS Voted FROM Votes WHERE ElectionID=@ElectionID AND VoterID=@VoterID";
            DataTable dtVoted = obj.GetData(votedQuery,
                new SqlParameter("@ElectionID", electionId),
                new SqlParameter("@VoterID", currentVoterId));

            bool voted = dtVoted.Rows.Count > 0 && Convert.ToInt32(dtVoted.Rows[0]["Voted"]) > 0;

            if (voted)
            {
                lblMessage.Text = "You have already voted in this election. Thank you for participating.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

            lblMessage.Text = "";
            GridView1.Visible = true;
            ButtonVote.Visible = true;

            string partyQuery = "SELECT PartyID, PartyName, SymbolImagePath, LeaderPhotoPath FROM Party WHERE Status='Approved'";
            DataTable dtParty = obj.GetData(partyQuery);
            GridView1.DataSource = dtParty;
            GridView1.DataBind();
        }

        protected void ButtonVote_Click(object sender, EventArgs e)
        {
            string selectedPartyId = Request.Form["PartyChoice"];

            if (ddlElection.SelectedValue == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select an election.');", true);
                return;
            }

            if (string.IsNullOrEmpty(selectedPartyId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a party.');", true);
                LoadBallotForSelectedElection();
                return;
            }

            // The radio value arrives as untrusted client input — validate it
            // is a plain integer before using it.
            int partyId;
            if (!int.TryParse(selectedPartyId, out partyId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid party selection.');", true);
                LoadBallotForSelectedElection();
                return;
            }

            int electionId;
            if (!int.TryParse(ddlElection.SelectedValue, out electionId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid election selection.');", true);
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
                error = obj.CastVote(voterId, partyId, electionId);
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voting is temporarily unavailable. Please try again.');", true);
                LoadBallotForSelectedElection();
                return;
            }
            if (error != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + error.Replace("'", "\\'") + "');", true);
                LoadBallotForSelectedElection();
                return;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Your vote has been recorded successfully. Thank you.');", true);

            LoadBallotForSelectedElection();
        }
    }
}