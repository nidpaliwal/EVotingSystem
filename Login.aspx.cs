using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            if (IsPostBack)
            {
                if (LoginGuard.IsLocked(ClientIp))
                {
                    lblLocked.Visible = true;
                    TextBox1.Enabled = false;
                    TextBox2.Enabled = false;
                    Button1.Enabled = false;
                }
            }
        }

        private string ClientIp
        {
            get
            {
                string ip = Request.UserHostAddress;
                return string.IsNullOrEmpty(ip) ? "unknown" : ip;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (LoginGuard.IsLocked(ClientIp))
            {
                lblLocked.Visible = true;
                TextBox1.Enabled = false;
                TextBox2.Enabled = false;
                Button1.Enabled = false;
                return;
            }

            string email = TextBox1.Text.Trim();
            string password = TextBox2.Text.Trim();

            // Defensive caps: prevents oversized values reaching the DB
            // (un-sized SqlParameter defaults to nvarchar(4000) -> 500).
            if (email.Length > 100 || password.Length > 128)
            {
                LoginGuard.RegisterFailure(ClientIp);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid Email or Password.');", true);
                return;
            }

            if (TryLogin("Admin", "Admin", email, password, "AdminDashboard.aspx")
                || TryLogin("Party", "Party", email, password, "PartyDashboard.aspx")
                || TryLogin("Voter", "Voter", email, password, "VoterDashboard.aspx"))
            {
                return; // TryLogin already cleared the lockout and redirected
            }

            LoginGuard.RegisterFailure(ClientIp);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid Email or Password.');", true);
        }

        /// <summary>
        /// Attempts to authenticate against one role table. Returns true
        /// (and redirects) on success; upgrades legacy SHA256 hashes to
        /// the PBKDF2 format on successful login.
        /// </summary>
        private bool TryLogin(string table, string role, string email, string password, string redirectPage)
        {
            string query = "SELECT PasswordHash FROM " + table + " WHERE Email=@Email";
            DataTable dt = obj.GetData(query, new SqlParameter("@Email", email));

            if (dt.Rows.Count == 0)
                return false;

            string storedHash = dt.Rows[0]["PasswordHash"].ToString();

            if (!PasswordHelper.VerifyPassword(password, storedHash))
                return false;

            // Successful login: upgrade legacy hash in place
            if (PasswordHelper.NeedsUpgrade(storedHash))
            {
                string newHash = PasswordHelper.HashPassword(password);
                obj.SetData("UPDATE " + table + " SET PasswordHash=@Hash WHERE Email=@Email",
                    new SqlParameter("@Hash", newHash),
                    new SqlParameter("@Email", email));
            }

            // Reset the per-IP failure counter BEFORE the redirect: the
            // redirect below terminates the request (ThreadAbortException),
            // so any code after it never runs.
            LoginGuard.Clear(ClientIp);

            Session["Role"] = role;
            Session["Email"] = email;
            Response.Redirect(redirectPage);
            return true;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
