using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class Login : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string email = TextBox1.Text.Trim();
            string enteredHash = PasswordHelper.HashPassword(TextBox2.Text.Trim());

            // Step 1: Check Admin
            string sAdmin = "Select * from Admin where Email='" + email + "' and PasswordHash='" + enteredHash + "'";
            var dtAdmin = obj.GetData(sAdmin);

            if (dtAdmin.Rows.Count > 0)
            {
                Session["Role"] = "Admin";
                Session["Email"] = email;
                Response.Redirect("AdminDashboard.aspx");
                return;
            }

            // Step 2: Check Party
            string sParty = "Select * from Party where Email='" + email + "' and PasswordHash='" + enteredHash + "'";
            var dtParty = obj.GetData(sParty);

            if (dtParty.Rows.Count > 0)
            {
                Session["Role"] = "Party";
                Session["Email"] = email;
                Response.Redirect("PartyDashboard.aspx");
            return;
        }

        // Step 3: Check Voter
        string sVoter = "Select * from Voter where Email='" + email + "' and PasswordHash='" + enteredHash + "'";
        var dtVoter = obj.GetData(sVoter);

            if (dtVoter.Rows.Count > 0)
            {
                Session["Role"] = "Voter";
                Session["Email"] = email;
                Response.Redirect("VoterDashboard.aspx");
                return;
            }

    // No match anywhere
    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid Email or Password.');", true);

    }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}