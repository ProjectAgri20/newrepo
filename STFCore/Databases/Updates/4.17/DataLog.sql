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

-- danderson Add new column, DeviceWarnings, to table TriageData
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TriageData' and COLUMN_NAME = 'DeviceWarnings')
BEGIN
    ALTER TABLE TriageData
    ADD DeviceWarnings nvarchar(MAX) NULL
END
GO

-- gparham Add foreign key to VirtualResourceInstanceStatus table
IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.VirtualResourceInstanceStatus'))
BEGIN
    DELETE
    FROM VirtualResourceInstanceStatus
    WHERE SessionId NOT IN (SELECT SessionId FROM SessionSummary)

    ALTER TABLE [dbo].[VirtualResourceInstanceStatus]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary] FOREIGN KEY([SessionId])
    REFERENCES [dbo].[SessionSummary] ([SessionId])
    ON UPDATE CASCADE
    ON DELETE CASCADE

    ALTER TABLE [dbo].[VirtualResourceInstanceStatus] CHECK CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary]
END
GO

-- danderson Add new column, UIDumpData, to table TriageData
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TriageData' and COLUMN_NAME = 'UIDumpData')
BEGIN
    ALTER TABLE TriageData
    ADD UIDumpData nvarchar(MAX) NULL
END
GO
