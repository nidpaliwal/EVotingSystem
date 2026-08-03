using System;
using System.Collections.Generic;
using System.Data;
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
            DateTime dob = Convert.ToDateTime(TextBox2.Text.Trim()); // adjust TextBox2 to your actual DOB field
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--; // adjusts if birthday hasn't occurred yet this year

            if (age < 18)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You must be at least 18 years old to register as a voter.');", true);
                return;
            }

            // Existing email check (keep as-is)...

            // New: Check duplicate Aadhar/VoterIDNumber
            string aadharCheck = "SELECT VoterID FROM Voter WHERE VoterIDNumber='" + TextBox4.Text.Trim() + "'";
            DataTable dtAadhar = obj.GetData(aadharCheck);

            if (dtAadhar.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This Aadhar ID is already registered.');", true);
                return;
            }

            string sql = "select Email from Admin where Email = '"+TextBox5.Text+"'";
            var existingEmails = obj.GetData(sql);
            if (existingEmails.Rows.Count == 0) 
            {
                sql = "select Email from Party where Email ='"+TextBox5.Text+"'";
                existingEmails = obj.GetData(sql);
                if (existingEmails.Rows.Count == 0)
                {
                    sql = "select Email from Voter where Email ='"+TextBox5.Text +"'";
                    existingEmails = obj.GetData(sql);
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

                        string s = "insert into Voter(Name,DOB,Gender,Address,VoterIDNumber,Email,PasswordHash,Phone,PhotoPath) values('" + TextBox1.Text + "','" + TextBox2.Text + "','" + DropDownList1.SelectedItem.Text + "','" + TextBox3.Text + "','" + TextBox4.Text + "','" + TextBox5.Text + "','" + PasswordHelper.HashPassword(TextBox6.Text) + "','" + TextBox7.Text + "','" + photoPath + "')";
                        obj.SetData(s);
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