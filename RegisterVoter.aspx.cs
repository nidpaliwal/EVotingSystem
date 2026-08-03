using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace EVotingSystem
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }

    public partial class RegisterVoter : System.Web.UI.Page
    {
        Datacon obj = new Datacon();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (TextBox1.Text == "" || TextBox2.Text == "" || TextBox3.Text == "" || TextBox4.Text == "" || TextBox5.Text == "" || TextBox6.Text == "" || TextBox7.Text == ""|| DropDownList1.SelectedItem.Text == ""|| FileUpload1.HasFile == false)
            {
                ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                "alert('Please fill all the fields.');",
                true);
                return;
            }

            // Age validation — must be at least 18 years old
            DateTime dob;
            if (!DateTime.TryParse(TextBox2.Text.Trim(), out dob))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid date of birth.');", true);
                return;
            }
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--; // adjusts if birthday hasn't occurred yet this year

            if (age < 18)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You must be at least 18 years old to register as a voter.');", true);
                return;
            }

            // New: Check duplicate Aadhar/VoterIDNumber
            string aadharCheck = "SELECT VoterID FROM Voter WHERE VoterIDNumber=@VoterIDNumber";
            DataTable dtAadhar = obj.GetData(aadharCheck, new SqlParameter("@VoterIDNumber", TextBox4.Text.Trim()));

            if (dtAadhar.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This Aadhar ID is already registered.');", true);
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

                        string folderPath = Server.MapPath("~/Uploads/VoterPhotos/");
                        

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload1.FileName);
                        string fullPath = Path.Combine(folderPath, fileName);


                        try
                        {
                            FileUpload1.SaveAs(fullPath);
                        }
                        catch (Exception ex)
                        {
                            ClientScript.RegisterStartupScript(
                                this.GetType(),
                                "alert",
                                "alert('Photo upload failed: " + ex.Message.Replace("'", "\\'") + "');",
                                true);
                            return;
                        }

                        // Save relative path in database
                        string photoPath = "~/Uploads/VoterPhotos/" + fileName;

                        string s = "insert into Voter(Name,DOB,Gender,Address,VoterIDNumber,Email,PasswordHash,Phone,PhotoPath) values(@Name,@DOB,@Gender,@Address,@VoterIDNumber,@Email,@PasswordHash,@Phone,@PhotoPath)";
                        obj.SetData(s,
                            new SqlParameter("@Name", TextBox1.Text),
                            new SqlParameter("@DOB", dob),
                            new SqlParameter("@Gender", DropDownList1.SelectedItem.Text),
                            new SqlParameter("@Address", TextBox3.Text),
                            new SqlParameter("@VoterIDNumber", TextBox4.Text),
                            new SqlParameter("@Email", TextBox5.Text),
                            new SqlParameter("@PasswordHash", PasswordHelper.HashPassword(TextBox6.Text)),
                            new SqlParameter("@Phone", TextBox7.Text),
                            new SqlParameter("@PhotoPath", photoPath));
                        string script = "alert('You are registered successfully.'); window.location.href='Login.aspx';";
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
