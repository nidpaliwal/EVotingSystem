using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class PartyElectionHistory : System.Web.UI.Page
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
                string s = "SELECT ElectionID, Title, StartDate, EndDate, IsActive FROM Election ORDER BY StartDate DESC";
                DataTable dt = obj.GetData(s);
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
        }
    }
}