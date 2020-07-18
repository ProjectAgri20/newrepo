USE [ScalableTestDataLog]
GO

/*====================================================================================================================*/
-- IF YOU ARE UPDATING DATALOG TABLES
-- Not all systems may have the Datalog table that is being updated.  This is especially 
-- true with STB. So if you are altering an existing Datalog table, please wrap
-- the alter code with the following code.  If the table does not exist then
-- there won't be an error. This applies to views as well.
--IF EXISTS
--	(
--		SELECT * 
--		FROM INFORMATION_SCHEMA.TABLES 
--		WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = '<YOUR_TABLE_HERE>'
--	)
--	BEGIN
--		-- <ENTER YOUR ALTER TABLE SCRIPT HERE>
--	END
--GO
/*====================================================================================================================*/

IF EXISTS
	(
		SELECT * 
		FROM INFORMATION_SCHEMA.TABLES 
		WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'SessionDevice'
	)
	BEGIN
		--dwander 10/14/2016: Adding a column 'IpAddress' to the SessionDevice table
		IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].SessionDevice') AND name = 'IpAddress')
		BEGIN	
			ALTER TABLE [dbo].SessionDevice ADD IpAddress VARCHAR(25) NULL 
		END		
	END
GO

USE [master]
GO

/****** Object:  Login [report_viewer]    Script Date: 11/14/2016 11:37:25 AM ******/
/* -- gparham - Create a login for a read-only account. */
IF NOT EXISTS(SELECT name FROM master.sys.server_principals WHERE name = 'report_viewer')
BEGIN
    CREATE LOGIN [report_viewer] WITH
        PASSWORD = N'report_viewer'
       ,DEFAULT_DATABASE = [ScalableTestDataLog]
       ,DEFAULT_LANGUAGE = [us_english]
       ,CHECK_EXPIRATION = OFF
       ,CHECK_POLICY = OFF;
END
GO

USE [ScalableTestDataLog]
GO

/****** Object:  User [report_viewer]    Script Date: 11/14/2016 11:54:48 AM ******/
IF NOT EXISTS(SELECT name FROM sys.database_principals WHERE name = 'report_viewer')
BEGIN
    CREATE USER [report_viewer] FOR LOGIN [report_viewer] WITH DEFAULT_SCHEMA=[dbo];
END
GO

-- Add the new user to db_datareader role
ALTER ROLE db_datareader ADD MEMBER report_viewer;
GO

-----------------------------------------------------------------------------
--kyoungman 11/15/2016 : Check for existence of SessionDeviceId column in DeviceMemorySnapshot table.
--			 If exists, drop the table and recreate it.

-- Create temp table to persist rebuild flag between GO statements
CREATE TABLE #variables
(
    rebuild BIT NOT NULL
)
GO

-- Set the rebuild flag
INSERT INTO #variables VALUES (CONVERT(BIT,
    (
        SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = 'DeviceMemorySnapshot' AND COLUMN_NAME = 'SessionDeviceId'
    )))
GO

-- Drop FK Constraints between DeviceMemoryCount and SessionSummary
IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	BEGIN
	ALTER TABLE DeviceMemoryCount
	DROP CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot
	END
GO

-- Drop the table
IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	DROP TABLE DeviceMemorySnapshot
GO

-- Recreate the table
IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	BEGIN
	CREATE TABLE [dbo].[DeviceMemorySnapshot](
	    [DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	    [SessionId] [varchar](50) NOT NULL,
	    [DeviceId] [varchar](50) NOT NULL,
	    [SnapshotLabel] [varchar](50) NULL,
	    [UsageCount] [int] NULL,
	    [SnapshotDateTime] [datetime] NOT NULL,
	    CONSTRAINT [PK_DeviceMemorySnapshot] PRIMARY KEY CLUSTERED (
		[DeviceMemorySnapshotId] ASC
	    )
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) 
	ON [PRIMARY]
	END
GO

-- Recreate the FK constraint with SessionSummary
IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	BEGIN
	ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	END
GO

IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary]
GO

-- Recreate the FK constraint between DeviceMemoryCount and SessionSummary
IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	BEGIN
	ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
	REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	END
GO

IF EXISTS (SELECT rebuild from #variables WHERE rebuild = 1)
	ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot]
GO

-- Drop the temp table
IF OBJECT_ID('tempdb..#variables') IS NOT NULL
    DROP TABLE #variables

	
--Add Column to sessionSummary table
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Summary') and name = 'STFVersion')
BEGIN
ALTER TABLE [dbo].[SessionSummary] ADD [STFVersion] [varchar](50) DEFAULT NULL

END

-----------------------------------------------------------------------------
-- gparham  11/28/2016 : Clean the ActivityExecutionPerformance table & add a foreign key reference to SessionSummary.
IF EXISTS(SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPerformance')) AND
   EXISTS(SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.SessionSummary')) AND
   EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.SessionSummary') AND name = 'SessionId')
BEGIN
    DECLARE @DeletedRows INT;
    SET @DeletedRows = 1;
    WHILE (@DeletedRows > 0)
    BEGIN
        DELETE TOP (10000) FROM [dbo].[ActivityExecutionPerformance] WHERE [SessionId] NOT IN (SELECT DISTINCT [SessionId] FROM [dbo].[SessionSummary])
        SET @DeletedRows = @@ROWCOUNT;
    END
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_ActivityExecutionPerformance_SessionSummary')
BEGIN
    ALTER TABLE [dbo].[ActivityExecutionPerformance] WITH CHECK ADD CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary] FOREIGN KEY([SessionId])
    REFERENCES [dbo].[SessionSummary] ([SessionId])
    ON UPDATE CASCADE
    ON DELETE CASCADE
    
    ALTER TABLE [dbo].[ActivityExecutionPerformance] CHECK CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary]
END
GO
