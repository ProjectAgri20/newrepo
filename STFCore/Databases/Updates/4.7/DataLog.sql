USE [ScalableTestDataLog]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING DATALOG TABLES
-- Not all systems may have the DataLog table that is being updated.  This is
-- especially true with the STB database.  So, if you are altering the database
-- schema then please wrap the SQL commands with the appropriate check below.
-- This will help to ensure that an error does not occur when the SQL script
-- is ran.
--
-- * Check if table exists.
-- IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.<YOUR_TABLE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR TABLE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a table does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a view exists.
-- IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID('dbo.<YOUR_VIEW_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR VIEW ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a view does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a stored procedure exists.
-- IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.<YOUR_PROCEDURE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR PROCEDURE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a stored procedure does not exist, change the 'IF'
--       statement to
--          IF NOT EXISTS ( ... )
--
-- * Check if a column exists in a specific table. (Same syntax works for views also)
-- IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '<TABLE_NAME_HERE>' and COLUMN_NAME = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/
-- 5/2/2017 kyoungman Add Columns to SessionSummary table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.SessionSummary') AND name IN ('Type','Cycle','Reference'))
BEGIN
    ALTER TABLE SessionSummary
    ADD [Type] VARCHAR(50) NULL DEFAULT(''),
    [Cycle] VARCHAR(50) NULL DEFAULT(''),
    [Reference] VARCHAR(255) NULL DEFAULT('')
END
GO

-- 5/18/2017 parham Add column to PullPrintJobRetrieval table
IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.PullPrintJobRetrieval'))
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PullPrintJobRetrieval' and COLUMN_NAME = 'NumberOfCopies')
    BEGIN
        ALTER TABLE PullPrintJobRetrieval
            ADD [NumberOfCopies] INT DEFAULT((1)) NOT NULL
    END
END
GO

-- 5/18/2017 parham Change column name
IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.PullPrintJobRetrieval'))
BEGIN
    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PullPrintJobRetrieval' and COLUMN_NAME = 'Username')
    BEGIN
        EXEC sp_rename 'PullPrintJobRetrieval.Username', 'UserName', 'COLUMN'
    END
END
GO
