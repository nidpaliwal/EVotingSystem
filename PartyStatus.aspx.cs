using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class PartyStatus : System.Web.UI.Page
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
                LoadStatus();
            }
        }

        private void LoadStatus()
        {
            string email = Session["Email"].ToString();
            string s = "SELECT PartyName, Status, DeclineReason FROM Party WHERE Email='" + email + "'";
            DataTable dt = obj.GetData(s);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblPartyName.Text = row["PartyName"].ToString();
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
            }
        }
    }
}