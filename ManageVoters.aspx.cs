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
    public partial class ManageVoters : System.Web.UI.Page
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
                string s = "SELECT VoterID,Name,VoterIDNumber, Email, Phone, Status,PhotoPath FROM Voter";
                DataTable dt = new DataTable();
                dt = obj.GetData(s);
                if (dt != null)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string term = TextBox1.Text;
            if (string.IsNullOrEmpty(term)) return;

            // Defensive cap: prevents oversized inputs crashing the query
            // (SqlParameter without a size defaults to nvarchar(4000)).
            if (term.Length > 200) term = term.Substring(0, 200);

            string s = "SELECT VoterID,Name,VoterIDNumber, Email, Phone, Status,PhotoPath FROM Voter where Name like @Search or VoterIDNumber like @Search";
            try
            {
                DataTable dt = obj.GetData(s, new SqlParameter("@Search", SqlDbType.NVarChar, -1) { Value = "%" + term + "%" });
                if (dt != null)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (SqlException)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Search failed. Please try again.');", true);
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string email = e.CommandArgument.ToString();

            // Get VoterID for this voter's email
            string idQuery = "SELECT VoterID FROM Voter WHERE Email=@Email";
            DataTable dtId = obj.GetData(idQuery, new SqlParameter("@Email", email));
            if (dtId.Rows.Count == 0)
                return;
            int voterId = Convert.ToInt32(dtId.Rows[0]["VoterID"]);

            // Get AdminID using the logged-in Admin's email from Session
            string adminEmail = Session["Email"].ToString();
            string adminQuery = "SELECT AdminID FROM Admin WHERE Email=@Email";
            DataTable dtAdmin = obj.GetData(adminQuery, new SqlParameter("@Email", adminEmail));
            if (dtAdmin.Rows.Count == 0)
                return;
            int adminId = Convert.ToInt32(dtAdmin.Rows[0]["AdminID"]);

            if (e.CommandName == "Approve")
            {
                string updateSql = "UPDATE Voter SET Status='Approved' WHERE Email=@Email";
                obj.SetData(updateSql, new SqlParameter("@Email", email));

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES (@AdminID, @Action, 'Voter', @TargetID)";
                obj.SetData(logSql,
                    new SqlParameter("@AdminID", adminId),
                    new SqlParameter("@Action", "Approved"),
                    new SqlParameter("@TargetID", voterId));

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voter approved successfully.');", true);
            }
            else if (e.CommandName == "Decline")
            {
                string reason = hdnDeclineReason.Value;

                // Server-side enforcement: a decline always needs a reason
                // (client-side prompt can be bypassed with a direct POST).
                if (string.IsNullOrWhiteSpace(reason))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please provide a reason for declining.');", true);
                    return;
                }

                string updateSql = "UPDATE Voter SET Status='Declined', DeclineReason=@Reason WHERE Email=@Email";
                obj.SetData(updateSql,
                    new SqlParameter("@Reason", reason),
                    new SqlParameter("@Email", email));

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES (@AdminID, @Action, 'Voter', @TargetID)";
                obj.SetData(logSql,
                    new SqlParameter("@AdminID", adminId),
                    new SqlParameter("@Action", "Declined: " + reason),
                    new SqlParameter("@TargetID", voterId));

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voter declined.');", true);
            }

            Button1_Click(sender, e);
        }
    }
}
