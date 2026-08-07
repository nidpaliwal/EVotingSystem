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
        /// transaction with row locks (closes the check-then-act race),
        /// inserts the vote tied to the voter and to the given election.
        /// The UNIQUE(ElectionID, VoterID) constraint is the final
        /// backstop against double voting in the same election; this method
        /// also makes sure that constraint exists (idempotently) so the
        /// guarantee holds even if a deployment never ran the migration.
        /// </summary>
        /// <returns>null on success, otherwise a user-facing error message.</returns>
        public string CastVote(int voterId, int partyId, int electionId)
        {
            EnsureVoteIntegrity();

            SqlConnection conn = new SqlConnection(con.ConnectionString);
            SqlTransaction tx = null;
            try
            {
                conn.Open();
                tx = conn.BeginTransaction();

                // 1. Lock the voter row so concurrent vote requests from
                //    the same voter are serialized, not double-processed.
                SqlCommand voterCmd = new SqlCommand(
                    "SELECT Status FROM Voter WITH (UPDLOCK, HOLDLOCK) WHERE VoterID=@voterId",
                    conn, tx);
                voterCmd.Parameters.AddWithValue("@voterId", voterId);

                using (SqlDataReader rd = voterCmd.ExecuteReader())
                {
                    if (!rd.Read())
                        return "Voter record not found.";
                    if (rd["Status"].ToString() != "Approved")
                        return "Your registration is not approved. You cannot vote.";
                }

                // 2. The chosen election must be active and its voting
                //    window (StartDate..EndDate) must cover the current time.
                SqlCommand electionCmd = new SqlCommand(
                    "SELECT ElectionID FROM Election WITH (UPDLOCK) WHERE ElectionID=@electionId AND IsActive=1 AND GETDATE() BETWEEN StartDate AND EndDate",
                    conn, tx);
                electionCmd.Parameters.AddWithValue("@electionId", electionId);
                object electionResult = electionCmd.ExecuteScalar();
                if (electionResult == null || electionResult == DBNull.Value)
                    return "The election is not currently open for voting.";

                // 3. The voter must not have already voted in this election.
                SqlCommand votedCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Votes WITH (UPDLOCK, HOLDLOCK) WHERE ElectionID=@electionId AND VoterID=@voterId",
                    conn, tx);
                votedCmd.Parameters.AddWithValue("@electionId", electionId);
                votedCmd.Parameters.AddWithValue("@voterId", voterId);
                if (Convert.ToInt32(votedCmd.ExecuteScalar()) > 0)
                    return "You have already voted in this election.";

                // 4. The selected party must exist and be approved.
                SqlCommand partyCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Party WHERE PartyID=@partyId AND Status='Approved'",
                    conn, tx);
                partyCmd.Parameters.AddWithValue("@partyId", partyId);
                if (Convert.ToInt32(partyCmd.ExecuteScalar()) == 0)
                    return "The selected party is not eligible.";

                // 5. Record the vote.
                SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO Votes (ElectionID, PartyID, VoterID, VotedOn) VALUES (@electionId, @partyId, @voterId, GETDATE())",
                    conn, tx);
                insertCmd.Parameters.AddWithValue("@electionId", electionId);
                insertCmd.Parameters.AddWithValue("@partyId", partyId);
                insertCmd.Parameters.AddWithValue("@voterId", voterId);
                insertCmd.ExecuteNonQuery();

                // 6. Mark the voter as having voted (legacy summary flag;
                //    per-election eligibility is enforced via the Votes table).
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
                    return "You have already voted in this election.";
                throw;
            }
            finally
            {
                if (conn != null) conn.Dispose();
            }
        }

        // Runs once per process: guarantees the UNIQUE(ElectionID, VoterID)
        // index on Votes exists so one voter can never cast more than one
        // vote in the same election, even on databases where the migration
        // was not applied. Mirrors Database/Migration_01_Votes_VoterID.sql.
        private static bool _voteIntegrityChecked;
        private static readonly object VoteIntegrityLock = new object();

        private void EnsureVoteIntegrity()
        {
            if (_voteIntegrityChecked) return;

            lock (VoteIntegrityLock)
            {
                if (_voteIntegrityChecked) return;

                if (TableExists("Votes") && !ColumnExists("Votes", "VoterID"))
                {
                    // Votes has no VoterID yet (pre-migration database):
                    // legacy rows cannot be attributed to anyone, so drop
                    // them, add the column, then add the unique constraint.
                    using (SqlConnection conn = new SqlConnection(con.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(
                            "DELETE FROM Votes; ALTER TABLE Votes ADD VoterID int NOT NULL;",
                            conn);
                        cmd.ExecuteNonQuery();
                    }
                }

EnsureVotesUniqueConstraint();
                _voteIntegrityChecked = true;
            }
        }

        private bool TableExists(string table)
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM sys.tables WHERE name=@name",
                    conn);
                cmd.Parameters.AddWithValue("@name", table);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool ColumnExists(string table, string column)
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM sys.columns WHERE object_id=OBJECT_ID(@table) AND name=@column",
                    conn);
                cmd.Parameters.AddWithValue("@table", table);
                cmd.Parameters.AddWithValue("@column", column);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void EnsureVotesUniqueConstraint()
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"IF NOT EXISTS (SELECT 1 FROM sys.indexes
                                     WHERE name = 'UQ_Votes_ElectionVoter' AND object_id = OBJECT_ID('Votes'))
                      BEGIN
                          ALTER TABLE Votes ADD CONSTRAINT UQ_Votes_ElectionVoter UNIQUE (ElectionID, VoterID);
                      END",
                    conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

