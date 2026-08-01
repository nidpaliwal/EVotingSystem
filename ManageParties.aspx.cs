using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace EVotingSystem
{
    public partial class ManageParties : System.Web.UI.Page
    {
        Datacon obj=new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                string s = "SELECT PartyID,PartyName,LeaderName,Status,Email,Phone,SymbolImagePath from Party";
                DataTable dt = new DataTable();
                dt = obj.GetData(s);
                if (dt != null)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string s = "SELECT  PartyID,PartyName,LeaderName,Status,Email,Phone,SymbolImagePath from Party where PartyName like '%" +TextBox1.Text+ "%' or CAST(PartyId as varchar) like '%" + TextBox1.Text+"%'" ;
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

            // Get PartyID for this party's email
            string idQuery = "SELECT PartyID FROM Party WHERE Email='" + email + "'";
            DataTable dtId = obj.GetData(idQuery);
            int partyId = Convert.ToInt32(dtId.Rows[0]["PartyID"]);

            // Get AdminID using the logged-in Admin's email from Session
            string adminEmail = Session["Email"].ToString();
            string adminQuery = "SELECT AdminID FROM Admin WHERE Email='" + adminEmail + "'";
            DataTable dtAdmin = obj.GetData(adminQuery);
            int adminId = Convert.ToInt32(dtAdmin.Rows[0]["AdminID"]);

            if (e.CommandName == "Approve")
            {
                string updateSql = "UPDATE Party SET Status='Approved' WHERE Email='" + email + "'";
                obj.SetData(updateSql);

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES ("
                    + adminId + ", 'Approved', 'Party', " + partyId + ")";
                obj.SetData(logSql);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Party approved successfully.');", true);
            }
            else if (e.CommandName == "Decline")
            {
                string reason = hdnDeclineReason.Value;

                string updateSql = "UPDATE Party SET Status='Declined', DeclineReason='" + reason + "' WHERE Email='" + email + "'";
                obj.SetData(updateSql);

                string logSql = "INSERT INTO AuditLog (AdminID, Action, TargetType, TargetID) VALUES ("
                    + adminId + ", 'Declined: " + reason.Replace("'", "''") + "', 'Party', " + partyId + ")";
                obj.SetData(logSql);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Party declined.');", true);
            }

            Button1_Click(sender, e);
        }

    }
}