using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class PartyListPublic : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string s = "SELECT PartyName, LeaderName, Objective, LegalHistory, SymbolImagePath, LeaderPhotoPath FROM Party WHERE Status='Approved'";
                DataTable dt = obj.GetData(s);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
    }
}