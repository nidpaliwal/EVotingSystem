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
    public partial class VoterVoteHistory : System.Web.UI.Page
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
                LoadHistory();
            }
        }

        private void LoadHistory()
        {
            string email = Session["Email"].ToString();
            string voterQuery = "SELECT VoterID FROM Voter WHERE Email=@Email";
            DataTable dtVoter = obj.GetData(voterQuery, new SqlParameter("@Email", email));

            if (dtVoter.Rows.Count == 0)
            {
                lblMessage.Text = "Voter record not found.";
                GridView1.Visible = false;
                return;
            }

            int voterId = Convert.ToInt32(dtVoter.Rows[0]["VoterID"]);

            string query = @"
                SELECT e.Title AS ElectionTitle,
                       CONVERT(varchar, e.StartDate, 106) + ' to ' + CONVERT(varchar, e.EndDate, 106) AS ElectionPeriod,
                       p.PartyName,
                       p.SymbolImagePath,
                       v.VotedOn
                FROM Votes v
                INNER JOIN Election e ON v.ElectionID = e.ElectionID
                INNER JOIN Party p ON v.PartyID = p.PartyID
                WHERE v.VoterID = @VoterID
                ORDER BY v.VotedOn DESC";

            DataTable dt = obj.GetData(query, new SqlParameter("@VoterID", voterId));

            if (dt.Rows.Count == 0)
            {
                lblMessage.Text = "You have not voted in any election yet.";
                GridView1.Visible = false;
                return;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}