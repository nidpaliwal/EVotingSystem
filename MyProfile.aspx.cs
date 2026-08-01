using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class MyProfile : System.Web.UI.Page
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
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            string email = Session["Email"].ToString();
            string s = "SELECT PartyName, LeaderName, Status, DeclineReason, Objective, LegalHistory FROM Party WHERE Email='" + email + "'";
            DataTable dt = obj.GetData(s);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblPartyName.Text = row["PartyName"].ToString();
                lblLeaderName.Text = row["LeaderName"].ToString();
                lblStatus.Text = row["Status"].ToString();
                TextBoxObjective.Text = row["Objective"] == DBNull.Value ? "" : row["Objective"].ToString();
                TextBoxLegalHistory.Text = row["LegalHistory"] == DBNull.Value ? "" : row["LegalHistory"].ToString();

                // Only show Decline Reason if status is actually Declined
                if (row["Status"].ToString() == "Declined")
                {
                    lblDeclineReason.Text = row["DeclineReason"] == DBNull.Value ? "" : row["DeclineReason"].ToString();
                    lblDeclineReason.Visible = true;
                    trDeclineReason.Visible = true;  // hides the whole table row, not just the label
                }
                else
                {
                    lblDeclineReason.Visible = false;
                    trDeclineReason.Visible = false;
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string email = Session["Email"].ToString();

            string updateSql = "UPDATE Party SET Objective='" + TextBoxObjective.Text.Replace("'", "''")
                + "', LegalHistory='" + TextBoxLegalHistory.Text.Replace("'", "''")
                + "' WHERE Email='" + email + "'";

            obj.SetData(updateSql);

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Profile updated successfully.');", true);
        }
    }
}