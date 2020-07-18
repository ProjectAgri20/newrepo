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

/****** Object:  Table [dbo].[DeviceMemoryXML] ******/
 IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.DeviceMemoryXML'))
 BEGIN
    CREATE TABLE [dbo].[DeviceMemoryXML](
	    [DeviceMemoryXmlId] [uniqueidentifier] NOT NULL,
	    [DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	    [MemoryXML] [xml] NULL,
     CONSTRAINT [PK_DeviceMemoryXML] PRIMARY KEY CLUSTERED 
    (
	    [DeviceMemoryXmlId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[DeviceMemoryXML]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryXML_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
    REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
    ON UPDATE CASCADE
    ON DELETE CASCADE

    ALTER TABLE [dbo].[DeviceMemoryXML] CHECK CONSTRAINT [FK_DeviceMemoryXML_DeviceMemorySnapshot]
END
GO

-- bermudew  Add bit field to denote deleted field to avoid conflict with ODS
USE ScalableTestDataLog
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionProductAssoc' and COLUMN_NAME = 'IsDeleted')
BEGIN
ALTER TABLE SessionProductAssoc
ADD IsDeleted BIT NOT NULL DEFAULT 0
END

-- parham  Change EventLabels in ActivityExecutionPerformance from 'AppButtonPress=' to 'AppButtonPress_'
UPDATE ActivityExecutionPerformance
SET EventLabel = REPLACE(EventLabel, 'AppButtonPress=', 'AppButtonPress_')
WHERE EventLabel LIKE 'AppButtonPress=%'

-- parham  Change 'ScanJob' event labels to 'SendingJob' only if the label comes after a 'JobBuildEnd' label.
UPDATE ActivityExecutionPerformance
SET EventLabel = REPLACE(aepsj.EventLabel, 'ScanJob', 'SendingJob')
FROM ActivityExecutionPerformance aep
JOIN (SELECT *
      FROM ActivityExecutionPerformance
      WHERE EventLabel IN ('ScanJobBegin', 'ScanJobEnd')) aepsj ON aepsj.ActivityExecutionPerformanceId = aep.ActivityExecutionPerformanceId
JOIN (SELECT *
      FROM ActivityExecutionPerformance
      WHERE EventLabel = 'JobBuildEnd') aepjb ON aepsj.ActivityExecutionId = aepjb.ActivityExecutionId
WHERE aepsj.EventIndex > aepjb.EventIndex
