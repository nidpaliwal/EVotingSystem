using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // End the session immediately upon arriving here: clear all
            // session data, abandon the session, and expire the cookie.
            if (Session != null)
            {
                Session.Clear();
                Session.Abandon();
            }
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpCookie expiryCookie = new HttpCookie("ASP.NET_SessionId");
                expiryCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(expiryCookie);
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
