using System;
using System.Collections.Generic;
using System.Data;
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
            string s = "SELECT  VoterID,Name,VoterIDNumber, Email, Phone, Status,PhotoPath FROM Voter where Name like '%" + TextBox1.Text + "%' or VoterIDNumber like '%" + TextBox1.Text + "%'";
            obj.GetData(s);
            DataTable dt = new DataTable();
            dt = obj.GetData(s);
            if (dt != null)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string email = e.CommandArgument.ToString();

            // Get VoterID for this voter's email
            string idQuery = "SELECT VoterID FROM Voter WHERE Email='" + email + "'";
            DataTable dtId = obj.GetData(idQuery);
            int voterId = Convert.ToInt32(dtId.Rows[0]["VoterID"]);

            // Get AdminID using the logged-in Admin's email from Session
            string adminEmail = Session["Email"].ToString();
            string adminQuery = "SELECT AdminID FROM Admin WHERE Email='" + adminEmail + "'";
            DataTable dtAdmin = obj.GetData(adminQuery);
            int adminId = Convert.ToInt32(dtAdmin.Rows[0]["AdminID"]);

            if (e.CommandName == "Approve")
            {
                string updateSql = "UPDATE Voter SET Status='Approved' WHERE Email='" + email + "'";
                obj.SetData(updateSql);

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES ("
                    + adminId + ", 'Approved', 'Voter', " + voterId + ")";
                obj.SetData(logSql);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voter approved successfully.');", true);
            }
            else if (e.CommandName == "Decline")
            {
                string reason = hdnDeclineReason.Value;

                string updateSql = "UPDATE Voter SET Status='Declined', DeclineReason='" + reason + "' WHERE Email='" + email + "'";
                obj.SetData(updateSql);

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES ("
                    + adminId + ", 'Declined: " + reason.Replace("'", "''") + "', 'Voter', " + voterId + ")";
                obj.SetData(logSql);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Voter declined.');", true);
            }

            Button1_Click(sender, e);
        }
    }
}