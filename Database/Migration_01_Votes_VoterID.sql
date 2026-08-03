-- =============================================================
-- Migration 01: Votes integrity
-- Adds VoterID to Votes (ties every vote to a voter), foreign
-- keys, and a UNIQUE(ElectionID, VoterID) constraint so that a
-- voter can cast at most one vote per election.
-- Idempotent: safe to run more than once.
-- =============================================================

SET XACT_ABORT ON;

BEGIN TRANSACTION;

-- Add the VoterID column on first run only.
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('Votes') AND name = 'VoterID')
BEGIN
    -- Legacy rows contain no VoterID and cannot be attributed to any
    -- voter. Approved removal of 8 test rows before tightening schema.
    DELETE FROM Votes;

    -- Vote must always know who cast it
    ALTER TABLE Votes ADD VoterID int NOT NULL;
END

-- Foreign key to Voter (Election/Party FKs already existed)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Votes_Voter')
    AND EXISTS (SELECT 1 FROM sys.columns
                WHERE object_id = OBJECT_ID('Votes') AND name = 'VoterID')
BEGIN
    ALTER TABLE Votes WITH CHECK ADD CONSTRAINT FK_Votes_Voter
        FOREIGN KEY (VoterID) REFERENCES Voter (VoterID);
END

-- Hard guarantee: one vote per voter per election.
-- The HasVoted bit is a fast-path optimization; this constraint is
-- the authoritative enforcement (works even under concurrency).
IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE name = 'UQ_Votes_ElectionVoter' AND object_id = OBJECT_ID('Votes'))
BEGIN
    ALTER TABLE Votes ADD CONSTRAINT UQ_Votes_ElectionVoter
        UNIQUE (ElectionID, VoterID);
END

COMMIT TRANSACTION;
