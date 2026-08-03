using System;
using System.Collections.Generic;
using System.Configuration;
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
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["EVotingDB"];
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new InvalidOperationException("Connection string 'EVotingDB' is missing from Web.config.");
            con = new SqlConnection(settings.ConnectionString);
        }

        // Method to retrieve data (SELECT).
        // Query text must never contain user input; pass values via parameters.
        public DataTable GetData(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Method to insert, update or delete data.
        // Query text must never contain user input; pass values via parameters.
        public int SetData(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result;
            }
        }

        /// <summary>
        /// Records a vote atomically: re-verifies eligibility inside a
        /// transaction with a row lock (closes the check-then-act race),
        /// inserts the vote tied to the voter, and marks the voter as
        /// having voted. The UNIQUE(ElectionID, VoterID) constraint is
        /// the final backstop against double voting.
        /// </summary>
        /// <returns>null on success, otherwise a user-facing error message.</returns>
        public string CastVote(int voterId, int partyId)
        {
            SqlConnection conn = new SqlConnection(con.ConnectionString);
            SqlTransaction tx = null;
            try
            {
                conn.Open();
                tx = conn.BeginTransaction();

                // 1. Lock the voter row so concurrent vote requests from
                //    the same voter are serialized, not double-processed.
                SqlCommand voterCmd = new SqlCommand(
                    "SELECT Status, HasVoted FROM Voter WITH (UPDLOCK, HOLDLOCK) WHERE VoterID=@voterId",
                    conn, tx);
                voterCmd.Parameters.AddWithValue("@voterId", voterId);

                using (SqlDataReader rd = voterCmd.ExecuteReader())
                {
                    if (!rd.Read())
                        return "Voter record not found.";
                    if (rd["Status"].ToString() != "Approved")
                        return "Your registration is not approved. You cannot vote.";
                    if (Convert.ToBoolean(rd["HasVoted"]))
                        return "You have already voted.";
                }

                // 2. There must be an active election whose voting window
                //    (StartDate..EndDate) covers the current time.
                SqlCommand electionCmd = new SqlCommand(
                    "SELECT TOP 1 ElectionID FROM Election WITH (UPDLOCK) WHERE IsActive=1 AND GETDATE() BETWEEN StartDate AND EndDate",
                    conn, tx);
                object electionResult = electionCmd.ExecuteScalar();
                if (electionResult == null || electionResult == DBNull.Value)
                    return "The election is not currently open for voting.";
                int electionId = Convert.ToInt32(electionResult);

                // 3. The selected party must exist and be approved.
                SqlCommand partyCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Party WHERE PartyID=@partyId AND Status='Approved'",
                    conn, tx);
                partyCmd.Parameters.AddWithValue("@partyId", partyId);
                if (Convert.ToInt32(partyCmd.ExecuteScalar()) == 0)
                    return "The selected party is not eligible.";

                // 4. Record the vote.
                SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO Votes (ElectionID, PartyID, VoterID, VotedOn) VALUES (@electionId, @partyId, @voterId, GETDATE())",
                    conn, tx);
                insertCmd.Parameters.AddWithValue("@electionId", electionId);
                insertCmd.Parameters.AddWithValue("@partyId", partyId);
                insertCmd.Parameters.AddWithValue("@voterId", voterId);
                insertCmd.ExecuteNonQuery();

                // 5. Mark the voter as having voted.
                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Voter SET HasVoted=1 WHERE VoterID=@voterId",
                    conn, tx);
                updateCmd.Parameters.AddWithValue("@voterId", voterId);
                updateCmd.ExecuteNonQuery();

                tx.Commit();
                return null;
            }
            catch (SqlException ex)
            {
                if (tx != null) tx.Rollback();
                if (ex.Number == 2601 || ex.Number == 2627) // duplicate key
                    return "You have already voted.";
                throw;
            }
            finally
            {
                if (conn != null) conn.Dispose();
            }
        }
    }
}
