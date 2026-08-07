-- =============================================================
-- Migration 02: Election authority details
-- Adds AuthorityName (election commission / authority) and
-- AuthorityNumber (notification / gazette number) to Election so
-- every election session carries its issuing authority.
-- Idempotent: safe to run more than once.
-- =============================================================

SET XACT_ABORT ON;

BEGIN TRANSACTION;

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('Election') AND name = 'AuthorityName')
BEGIN
    ALTER TABLE Election ADD AuthorityName nvarchar(150) NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('Election') AND name = 'AuthorityNumber')
BEGIN
    ALTER TABLE Election ADD AuthorityNumber nvarchar(50) NULL;
END

COMMIT TRANSACTION;
