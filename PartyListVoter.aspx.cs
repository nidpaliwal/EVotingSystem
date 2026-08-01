using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class PartyListVoter : System.Web.UI.Page
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
                string s = "SELECT PartyName, LeaderName, Objective, SymbolImagePath FROM Party WHERE Status='Approved'";
                DataTable dt = obj.GetData(s);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
    }
}