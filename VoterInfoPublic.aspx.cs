using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class VoterInfoPublic : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string totalQuery = "SELECT COUNT(*) AS Total FROM Voter";
                DataTable dtTotal = obj.GetData(totalQuery);
                lblTotalVoters.Text = dtTotal.Rows[0]["Total"].ToString();

                string approvedQuery = "SELECT COUNT(*) AS Approved FROM Voter WHERE Status='Approved'";
                DataTable dtApproved = obj.GetData(approvedQuery);
                lblApprovedVoters.Text = dtApproved.Rows[0]["Approved"].ToString();
            }
        }
    }
}