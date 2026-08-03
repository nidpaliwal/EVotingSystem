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
    public partial class Result : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadResults();
            }
        }

        private void LoadResults()
        {
            int electionId;

            if (Request.QueryString["id"] != null)
            {
                if (!int.TryParse(Request.QueryString["id"], out electionId))
                {
                    lblMessage.Text = "Invalid election id.";
                    GridView1.Visible = false;
                    return;
                }
            }
            else
            {
                string fallbackQuery = "SELECT TOP 1 ElectionID FROM Election ORDER BY StartDate DESC";
                DataTable dtFallback = obj.GetData(fallbackQuery);
                if (dtFallback.Rows.Count == 0)
                {
                    lblMessage.Text = "No election found.";
                    GridView1.Visible = false;
                    return;
                }
                electionId = Convert.ToInt32(dtFallback.Rows[0]["ElectionID"]);
            }

            string electionQuery = "SELECT Title FROM Election WHERE ElectionID=@ElectionID";
            DataTable dtElection = obj.GetData(electionQuery, new SqlParameter("@ElectionID", electionId));

            if (dtElection.Rows.Count == 0)
            {
                lblMessage.Text = "Election not found.";
                GridView1.Visible = false;
                return;
            }

            string title = dtElection.Rows[0]["Title"].ToString();
            // No "ongoing" check here — Admin always sees live results, even mid-election
            GridView1.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Black;
            lblMessage.Text = "Results for: " + title;

            string resultsQuery = @"
                SELECT p.PartyName, p.SymbolImagePath, COUNT(v.VoteID) AS VoteCount
                FROM Party p
                LEFT JOIN Votes v ON p.PartyID = v.PartyID AND v.ElectionID = @ElectionID
                WHERE p.Status = 'Approved'
                GROUP BY p.PartyName, p.SymbolImagePath
                ORDER BY VoteCount DESC";

            DataTable dtResults = obj.GetData(resultsQuery, new SqlParameter("@ElectionID", electionId));
            GridView1.DataSource = dtResults;
            GridView1.DataBind();
        }

    }
    }