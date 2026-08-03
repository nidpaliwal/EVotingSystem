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
            string s = "SELECT PartyName, LeaderName, Status, DeclineReason, Objective, LegalHistory FROM Party WHERE Email=@Email";
            DataTable dt = obj.GetData(s, new SqlParameter("@Email", email));

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

            string updateSql = "UPDATE Party SET Objective=@Objective, LegalHistory=@LegalHistory, Status='Pending', DeclineReason=NULL WHERE Email=@Email";
            obj.SetData(updateSql,
                new SqlParameter("@Objective", TextBoxObjective.Text),
                new SqlParameter("@LegalHistory", TextBoxLegalHistory.Text),
                new SqlParameter("@Email", email));

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Profile updated. Your changes have been submitted for Admin re-approval.');", true);

            // Refresh the displayed labels to reflect the new Pending status
            LoadProfile();
        }
    }
}
