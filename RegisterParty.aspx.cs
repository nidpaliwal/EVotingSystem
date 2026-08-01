using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EVotingSystem
{

    public partial class RegisterParty : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "" || TextBox2.Text == "" || TextBox3.Text == "" || TextBox4.Text == "" || TextBox5.Text == "" || TextBox6.Text == "" || TextBox7.Text == "" ||  FileUpload1.HasFile == false||FileUpload2.HasFile == false)
            {
                ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                "alert('Please fill all the fields.');",
                true);
                return;
            }

            string sql = "select Email from Admin where Email = '" + TextBox5.Text + "'";
            var existingEmails = obj.GetData(sql);
            if (existingEmails.Rows.Count == 0)
            {
                sql = "select Email from Party where Email ='" + TextBox5.Text + "'";
                existingEmails = obj.GetData(sql);
                if (existingEmails.Rows.Count == 0)
                {
                    sql = "select Email from Voter where Email ='" + TextBox5.Text + "'";
                    existingEmails = obj.GetData(sql);
                    if (existingEmails.Rows.Count == 0)
                    {
                        // New email, proceed with registration
                        // Leader Photo
                        string leaderFolder = Server.MapPath("~/Uploads/LeaderPhotos/");

                        if (!Directory.Exists(leaderFolder))
                        {
                            Directory.CreateDirectory(leaderFolder);
                        }

                        string leaderFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload1.FileName);

                        string leaderFullPath = Path.Combine(leaderFolder, leaderFileName);

                        FileUpload1.SaveAs(leaderFullPath);

                        string leaderPhotoPath = "~/Uploads/LeaderPhotos/" + leaderFileName;


                        // Party Symbol
                        string symbolFolder = Server.MapPath("~/Uploads/PartySymbols/");

                        if (!Directory.Exists(symbolFolder))
                        {
                            Directory.CreateDirectory(symbolFolder);
                        }

                        string symbolFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload2.FileName);

                        string symbolFullPath = Path.Combine(symbolFolder, symbolFileName);

                        FileUpload2.SaveAs(symbolFullPath);

                        string partySymbolPath = "~/Uploads/PartySymbols/" + symbolFileName;



                        string s = "INSERT INTO Party(PartyName,SymbolImagePath,LeaderName,LeaderPhotoPath,Objective,Email,PasswordHash,Phone) VALUES('" +
                        TextBox1.Text + "','" +
                        partySymbolPath + "','" +
                        TextBox2.Text + "','" +
                        leaderPhotoPath + "','" +
                        TextBox3.Text + "','" +
                        TextBox5.Text + "','" +
                        PasswordHelper.HashPassword(TextBox6.Text) + "','" +
                        TextBox7.Text + "')";

                        obj.SetData(s);
                        string script = "alert('Party registered successfully.'); window.location.href='Login.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(
                         this.GetType(),
                         "alert",
                         "alert('Your email is already registered.Please Login.');",
                         true);
                        return;
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        "alert('Your email is already registered.Please Login.');",
                        true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    "alert('Your email is already registered.Please Login.');",
                    true);
            }
        }


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }

    }
}