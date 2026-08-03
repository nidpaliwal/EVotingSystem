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
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const string Prefix = "PBKDF2$";

        /// <summary>
        /// Hashes a password for storage using salted PBKDF2 (RFC 2898).
        /// Format: PBKDF2$&lt;iterations&gt;$&lt;saltHex&gt;$&lt;hashHex&gt;
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            byte[] hash = Derive(password, salt, Iterations);
            return Prefix + Iterations + "$" + ToHex(salt) + "$" + ToHex(hash);
        }

        /// <summary>
        /// Verifies a password against a stored hash. Supports both the
        /// current PBKDF2 format and legacy unsalted SHA256 hashes.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(password))
                return false;

            if (storedHash.StartsWith(Prefix))
            {
                string[] parts = storedHash.Split('$');
                if (parts.Length != 4)
                    return false;

                int iterations;
                if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    return false;

                byte[] salt;
                byte[] expected;
                try
                {
                    salt = FromHex(parts[2]);
                    expected = FromHex(parts[3]);
                }
                catch
                {
                    return false;
                }

                byte[] test = Derive(password, salt, iterations);
                return FixedTimeEquals(test, expected);
            }

            // Legacy unsalted SHA256 hash (pre-migration accounts)
            return string.Equals(storedHash, LegacySha256(password), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>True if the stored hash uses the old unsalted SHA256 scheme.</summary>
        public static bool NeedsUpgrade(string storedHash)
        {
            return !string.IsNullOrEmpty(storedHash) && !storedHash.StartsWith(Prefix);
        }

        /// <summary>Legacy unsalted SHA256 (kept only to verify old accounts).</summary>
        public static string LegacySha256(string password)
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

        private static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        private static string ToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static byte[] FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
                throw new FormatException("Invalid hex length");
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return result;
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
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

                        // Validate the uploaded photo (extension, size, content)
                        string uploadError = UploadHelper.Validate(FileUpload1.PostedFile);
                        if (uploadError != null)
                        {
                            ClientScript.RegisterStartupScript(
                                this.GetType(),
                                "alert",
                                "alert('" + uploadError.Replace("'", "\\'") + "');",
                                true);
                            return;
                        }

                        string folderPath = Server.MapPath("~/Uploads/VoterPhotos/");
                        

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload1.FileName).ToLowerInvariant();
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
