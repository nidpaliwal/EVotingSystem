using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EVotingSystem
{
    public class Datacon
    {
        SqlConnection con;
        public Datacon() {
            con = new SqlConnection(@"Data Source=LAPTOP-HKGA59LC\SQLEXPRESS;Initial Catalog=EVotingDB;Integrated Security=True;");
        }

        // Method to retrieve data (SELECT)
        public DataTable GetData(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // Method to insert, update or delete data
        public int SetData(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
    }
}