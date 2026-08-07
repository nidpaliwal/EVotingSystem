using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class VoterParticipants : System.Web.UI.Page
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
                LoadParticipants();
            }
        }

        private void LoadParticipants()
        {
            string query = "SELECT Name, VoterIDNumber, Gender, DOB, Address, PhotoPath FROM Voter WHERE Status='Approved' ORDER BY Name";
            DataTable dt = obj.GetData(query);

            if (dt.Rows.Count == 0)
            {
                lblMessage.Text = "No approved participants yet.";
                GridView1.Visible = false;
                return;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}