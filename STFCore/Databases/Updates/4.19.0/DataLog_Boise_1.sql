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

-- parham: function fn_CalcLocalDateTime
IF OBJECT_ID(N'fn_CalcLocalDateTime', N'FN') IS NOT NULL
BEGIN
    DROP FUNCTION fn_CalcLocalDateTime;
END;
GO

-- ============================================================================
-- Author:      Gary Parham
-- Create date: 2019/07/09
-- Description: Calculate a local datetime value from a UTC datetime.
-- ============================================================================
CREATE FUNCTION [dbo].[fn_CalcLocalDateTime]
(
    @utcDateTime datetime
)
RETURNS datetime
AS
BEGIN
    -- Declare the return variable here
    DECLARE @result datetime;

    -- Add the T-SQL statements to compute the return value here
    DECLARE @timeZoneName varchar(50);

    EXEC master.dbo.xp_regread 'HKEY_LOCAL_MACHINE', 'SYSTEM\CurrentControlSet\Control\TimeZoneInformation', 'TimeZoneKeyName', @timeZoneName out;

    SET @result = CAST(@utcDateTime AT TIME ZONE 'UTC' AT TIME ZONE @timeZoneName AS datetime);

    -- Return the result of the function
    RETURN @result;
END
GO

SET NOCOUNT ON;

/* Add a tracking column to all the tables. */
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'WindowsEventLog')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'WindowsEventLog' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [WindowsEventLog] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_WindowsEventLog DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendJobNotification')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobNotification' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DigitalSendJobNotification] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DigitalSendJobNotification DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DirectorySnapshot')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DirectorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DirectorySnapshot] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DirectorySnapshot DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JetAdvantageUpload')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageUpload' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [JetAdvantageUpload] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_JetAdvantageUpload DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JetAdvantagePullPrintJobRetrieval')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantagePullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [JetAdvantagePullPrintJobRetrieval] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_JetAdvantagePullPrintJobRetrieval DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JetAdvantageLinkDeviceMemorySnapshot')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageLinkDeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [JetAdvantageLinkDeviceMemorySnapshot] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_JetAdvantageLinkDeviceMemorySnapshot DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VirtualResourceInstanceStatus')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualResourceInstanceStatus' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [VirtualResourceInstanceStatus] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_VirtualResourceInstanceStatus DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeviceEvent')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceEvent' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DeviceEvent] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DeviceEvent DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TriageData')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TriageData' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [TriageData] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_TriageData DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ActivityExecutionRetries')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionRetries' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [ActivityExecutionRetries] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_ActivityExecutionRetries DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EPrintServerJob')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EPrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [EPrintServerJob] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_EPrintServerJob DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendJobTempFile')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobTempFile' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DigitalSendJobTempFile] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DigitalSendJobTempFile DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendTempSnapshot')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendTempSnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DigitalSendTempSnapshot] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DigitalSendTempSnapshot DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendServerJob')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DigitalSendServerJob] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DigitalSendServerJob DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeviceMemorySnapshot')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DeviceMemorySnapshot] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DeviceMemorySnapshot DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendJobOutput')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobOutput' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [DigitalSendJobOutput] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_DigitalSendJobOutput DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PhysicalDeviceJob')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PhysicalDeviceJob' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [PhysicalDeviceJob] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_PhysicalDeviceJob DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VirtualPrinterJob')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualPrinterJob' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [VirtualPrinterJob] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_VirtualPrinterJob DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PullPrintJobRetrieval')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [PullPrintJobRetrieval] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_PullPrintJobRetrieval DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintServerJob')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [PrintServerJob] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_PrintServerJob DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintJobClient')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintJobClient' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [PrintJobClient] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_PrintJobClient DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ActivityExecutionDetail')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionDetail' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [ActivityExecutionDetail] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_ActivityExecutionDetail DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ActivityExecution')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecution' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [ActivityExecution] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_ActivityExecution DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ActivityExecutionPerformance')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionPerformance' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [ActivityExecutionPerformance] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_ActivityExecutionPerformance DEFAULT (1);
    END;
END;
GO



/*************************************************************
 Process a couple of special tables.
 The remaining tables will be processed in a separate script.
*************************************************************/
-- SessionScenario
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SessionScenario')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionScenario' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [SessionScenario] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_SessionScenario DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionScenario' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [SessionScenario]
        SET [ScenarioEnd] = [ScenarioEnd] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [ScenarioStart] = [ScenarioStart] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionScenario' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [SessionScenario] DROP CONSTRAINT UTC_default_SessionScenario;
    ALTER TABLE [SessionScenario] DROP COLUMN [UpdatedToUTC];
END; 

-- SessionSummary
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SessionSummary')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'UpdatedToUTC')
    BEGIN
        ALTER TABLE [SessionSummary] ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_SessionSummary DEFAULT (1);
    END;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [SessionSummary]
        SET [EndDateTime] = [EndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [ExpirationDateTime] = [ExpirationDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [ProjectedEndDateTime] = [ProjectedEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [ShutdownDateTime] = [ShutdownDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [StartDateTime] = [StartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [SessionSummary] DROP CONSTRAINT UTC_default_SessionSummary;
    ALTER TABLE [SessionSummary] DROP COLUMN [UpdatedToUTC];
END; 
