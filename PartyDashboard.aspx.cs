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
    public partial class PartyDashboard : System.Web.UI.Page
    {
        Datacon obj=new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Party")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string email = Session["Email"].ToString();
                string s = "SELECT LeaderName,Email FROM Party WHERE Email=@Email";
                DataTable dt = obj.GetData(s, new SqlParameter("@Email", email));

                if (dt.Rows.Count > 0)
                {
                    lblLeaderName.Text = dt.Rows[0]["LeaderName"].ToString();
                    lblEmail.Text = dt.Rows[0]["Email"].ToString();
                }
            }
        }
    }
}