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

-- parham - Grant execute permission to the report_viewer login account.
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE [name] = 'report_viewer') AND
   EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'dbo' AND SPECIFIC_NAME = 'fn_CalcLocalDateTime' AND ROUTINE_TYPE = 'FUNCTION')
BEGIN
    GRANT EXECUTE ON OBJECT::dbo.fn_CalcLocalDateTime TO report_viewer;
END
GO
