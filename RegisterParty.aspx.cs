using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            string sql = "select Email from Admin where Email = @Email";
            var existingEmails = obj.GetData(sql, new SqlParameter("@Email", TextBox5.Text));
            if (existingEmails.Rows.Count == 0)
            {
                sql = "select Email from Party where Email = @Email";
                existingEmails = obj.GetData(sql, new SqlParameter("@Email", TextBox5.Text));
                if (existingEmails.Rows.Count == 0)
                {
                    sql = "select Email from Voter where Email = @Email";
                    existingEmails = obj.GetData(sql, new SqlParameter("@Email", TextBox5.Text));
                    if (existingEmails.Rows.Count == 0)
                    {
                        // New email, proceed with registration

                        // Validate both uploads (extension, size, content)
                        string leaderUploadError = UploadHelper.Validate(FileUpload1.PostedFile);
                        if (leaderUploadError != null)
                        {
                            ClientScript.RegisterStartupScript(
                                this.GetType(),
                                "alert",
                                "alert('Leader photo: " + leaderUploadError.Replace("'", "\\'") + "');",
                                true);
                            return;
                        }
                        string symbolUploadError = UploadHelper.Validate(FileUpload2.PostedFile);
                        if (symbolUploadError != null)
                        {
                            ClientScript.RegisterStartupScript(
                                this.GetType(),
                                "alert",
                                "alert('Party symbol: " + symbolUploadError.Replace("'", "\\'") + "');",
                                true);
                            return;
                        }

                        // Leader Photo
                        string leaderFolder = Server.MapPath("~/Uploads/LeaderPhotos/");

                        if (!Directory.Exists(leaderFolder))
                        {
                            Directory.CreateDirectory(leaderFolder);
                        }

                        string leaderFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload1.FileName).ToLowerInvariant();

                        string leaderFullPath = Path.Combine(leaderFolder, leaderFileName);

                        FileUpload1.SaveAs(leaderFullPath);

                        string leaderPhotoPath = "~/Uploads/LeaderPhotos/" + leaderFileName;


                        // Party Symbol
                        string symbolFolder = Server.MapPath("~/Uploads/PartySymbols/");

                        if (!Directory.Exists(symbolFolder))
                        {
                            Directory.CreateDirectory(symbolFolder);
                        }

                        string symbolFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload2.FileName).ToLowerInvariant();

                        string symbolFullPath = Path.Combine(symbolFolder, symbolFileName);

                        FileUpload2.SaveAs(symbolFullPath);

                        string partySymbolPath = "~/Uploads/PartySymbols/" + symbolFileName;



                        string s = "INSERT INTO Party(PartyName,SymbolImagePath,LeaderName,LeaderPhotoPath,Objective,LegalHistory,Email,PasswordHash,Phone) VALUES(@PartyName,@SymbolImagePath,@LeaderName,@LeaderPhotoPath,@Objective,@LegalHistory,@Email,@PasswordHash,@Phone)";
                        try
                        {
                            obj.SetData(s,
                            new SqlParameter("@PartyName", TextBox1.Text),
                            new SqlParameter("@SymbolImagePath", partySymbolPath),
                            new SqlParameter("@LeaderName", TextBox2.Text),
                            new SqlParameter("@LeaderPhotoPath", leaderPhotoPath),
                            new SqlParameter("@Objective", TextBox3.Text),
                            new SqlParameter("@LegalHistory", TextBox4.Text),
                            new SqlParameter("@Email", TextBox5.Text),
                            new SqlParameter("@PasswordHash", PasswordHelper.HashPassword(TextBox6.Text)),
                            new SqlParameter("@Phone", TextBox7.Text));
                        }
                        catch (SqlException)
                        {
                            try { File.Delete(leaderFullPath); } catch { /* best effort */ }
                            try { File.Delete(symbolFullPath); } catch { /* best effort */ }
                            ClientScript.RegisterStartupScript(
                                this.GetType(),
                                "alert",
                                "alert('Registration failed. That email or phone may already be registered.');",
                                true);
                            return;
                        }
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
