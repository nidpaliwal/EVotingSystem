using System;
using System.Collections.Generic;
using System.Data;
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

            // Check voter status and HasVoted
            string voterQuery = "SELECT Status, HasVoted FROM Voter WHERE Email='" + email + "'";
            DataTable dtVoter = obj.GetData(voterQuery);

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

            // Check active election
            string electionQuery = "SELECT ElectionID FROM Election WHERE IsActive=1";
            DataTable dtElection = obj.GetData(electionQuery);

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "There is no active election at this time.";
                GridView1.Visible = false;
                ButtonVote.Visible = false;
                return;
            }

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

            string email = Session["Email"].ToString();

            // Re-verify eligibility server-side (never trust client alone)
            string voterQuery = "SELECT VoterID, Status, HasVoted FROM Voter WHERE Email='" + email + "'";
            DataTable dtVoter = obj.GetData(voterQuery);

            if (dtVoter.Rows.Count == 0 || dtVoter.Rows[0]["Status"].ToString() != "Approved")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You are not eligible to vote.');", true);
                return;
            }

            bool hasVoted = Convert.ToBoolean(dtVoter.Rows[0]["HasVoted"]);
            if (hasVoted)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You have already voted.');", true);
                CheckEligibilityAndLoad();
                return;
            }

            string electionQuery = "SELECT ElectionID FROM Election WHERE IsActive=1";
            DataTable dtElection = obj.GetData(electionQuery);

            if (dtElection.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No active election.');", true);
                return;
            }

            int electionId = Convert.ToInt32(dtElection.Rows[0]["ElectionID"]);
            int voterId = Convert.ToInt32(dtVoter.Rows[0]["VoterID"]);

            // Insert the vote
            string insertVoteSql = "INSERT INTO Votes (ElectionID, PartyID) VALUES (" + electionId + ", " + selectedPartyId + ")";
            obj.SetData(insertVoteSql);

            // Mark voter as having voted
            string updateVoterSql = "UPDATE Voter SET HasVoted=1 WHERE VoterID=" + voterId;
            obj.SetData(updateVoterSql);

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Your vote has been recorded successfully. Thank you.');", true);

            GridView1.Visible = false;
            ButtonVote.Visible = false;
            lblMessage.Text = "You have already voted. Thank you for participating.";
        }
    }
}