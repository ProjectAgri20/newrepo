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

/******************************************************************************
 Updating some of these tables may take a long time. Therefore, this script is
 to be ran while the database is still online and production testing is
 continuing.
******************************************************************************/
-- WindowsEventLog
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'WindowsEventLog' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [WindowsEventLog]
        SET [GeneratedDateTime] = [GeneratedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'WindowsEventLog' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [WindowsEventLog] DROP CONSTRAINT UTC_default_WindowsEventLog;
    ALTER TABLE [WindowsEventLog] DROP COLUMN [UpdatedToUTC];
END; 

-- DigitalSendJobNotification
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobNotification' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DigitalSendJobNotification]
        SET [NotificationReceivedDateTime] = [NotificationReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobNotification' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DigitalSendJobNotification] DROP CONSTRAINT UTC_default_DigitalSendJobNotification;
    ALTER TABLE [DigitalSendJobNotification] DROP COLUMN [UpdatedToUTC];
END; 

-- DirectorySnapshot
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DirectorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DirectorySnapshot]
        SET [SnapshotDateTime] = [SnapshotDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DirectorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DirectorySnapshot] DROP CONSTRAINT UTC_default_DirectorySnapshot;
    ALTER TABLE [DirectorySnapshot] DROP COLUMN [UpdatedToUTC];
END; 

-- JetAdvantageUpload
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageUpload' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [JetAdvantageUpload]
        SET [CompletionDateTime] = [CompletionDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [FileReceivedDateTime] = [FileReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [FileSentDateTime] = [FileSentDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageUpload' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [JetAdvantageUpload] DROP CONSTRAINT UTC_default_JetAdvantageUpload;
    ALTER TABLE [JetAdvantageUpload] DROP COLUMN [UpdatedToUTC];
END; 

-- JetAdvantagePullPrintJobRetrieval
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantagePullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [JetAdvantagePullPrintJobRetrieval]
        SET [JobEndDateTime] = [JobEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [JobStartDateTime] = [JobStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantagePullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [JetAdvantagePullPrintJobRetrieval] DROP CONSTRAINT UTC_default_JetAdvantagePullPrintJobRetrieval;
    ALTER TABLE [JetAdvantagePullPrintJobRetrieval] DROP COLUMN [UpdatedToUTC];
END; 

-- JetAdvantageLinkDeviceMemorySnapshot
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageLinkDeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [JetAdvantageLinkDeviceMemorySnapshot]
        SET [SnapshotDateTime] = [SnapshotDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JetAdvantageLinkDeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [JetAdvantageLinkDeviceMemorySnapshot] DROP CONSTRAINT UTC_default_JetAdvantageLinkDeviceMemorySnapshot;
    ALTER TABLE [JetAdvantageLinkDeviceMemorySnapshot] DROP COLUMN [UpdatedToUTC];
END; 

-- VirtualResourceInstanceStatus
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualResourceInstanceStatus' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [VirtualResourceInstanceStatus]
        SET [TimeStamp] = [TimeStamp] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualResourceInstanceStatus' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [VirtualResourceInstanceStatus] DROP CONSTRAINT UTC_default_VirtualResourceInstanceStatus;
    ALTER TABLE [VirtualResourceInstanceStatus] DROP COLUMN [UpdatedToUTC];
END; 

-- DeviceEvent
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceEvent' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DeviceEvent]
        SET [EventDateTime] = [EventDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [StartDateTime] = [StartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceEvent' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DeviceEvent] DROP CONSTRAINT UTC_default_DeviceEvent;
    ALTER TABLE [DeviceEvent] DROP COLUMN [UpdatedToUTC];
END; 

-- TriageData
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TriageData' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [TriageData]
        SET [TriageDateTime] = [TriageDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TriageData' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [TriageData] DROP CONSTRAINT UTC_default_TriageData;
    ALTER TABLE [TriageData] DROP COLUMN [UpdatedToUTC];
END; 

-- ActivityExecutionRetries
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionRetries' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [ActivityExecutionRetries]
        SET [RetryStartDateTime] = [RetryStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionRetries' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [ActivityExecutionRetries] DROP CONSTRAINT UTC_default_ActivityExecutionRetries;
    ALTER TABLE [ActivityExecutionRetries] DROP COLUMN [UpdatedToUTC];
END; 

-- EPrintServerJob
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EPrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [EPrintServerJob]
        SET [EmailReceivedDateTime] = [EmailReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [JobStartDateTime] = [JobStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [LastStatusDateTime] = [LastStatusDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EPrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [EPrintServerJob] DROP CONSTRAINT UTC_default_EPrintServerJob;
    ALTER TABLE [EPrintServerJob] DROP COLUMN [UpdatedToUTC];
END; 

-- DigitalSendJobTempFile
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobTempFile' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DigitalSendJobTempFile]
        SET [FirstFileInDateTime] = [FirstFileInDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [LastFileOutDateTime] = [LastFileOutDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [RenameDateTime] = [RenameDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobTempFile' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DigitalSendJobTempFile] DROP CONSTRAINT UTC_default_DigitalSendJobTempFile;
    ALTER TABLE [DigitalSendJobTempFile] DROP COLUMN [UpdatedToUTC];
END; 

-- DigitalSendTempSnapshot
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendTempSnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DigitalSendTempSnapshot]
        SET [SnapshotDateTime] = [SnapshotDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendTempSnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DigitalSendTempSnapshot] DROP CONSTRAINT UTC_default_DigitalSendTempSnapshot;
    ALTER TABLE [DigitalSendTempSnapshot] DROP COLUMN [UpdatedToUTC];
END; 

-- DigitalSendServerJob
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DigitalSendServerJob]
        SET [CompletionDateTime] = [CompletionDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DigitalSendServerJob] DROP CONSTRAINT UTC_default_DigitalSendServerJob;
    ALTER TABLE [DigitalSendServerJob] DROP COLUMN [UpdatedToUTC];
END; 

-- DeviceMemorySnapshot
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DeviceMemorySnapshot]
        SET [SnapshotDateTime] = [SnapshotDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceMemorySnapshot' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DeviceMemorySnapshot] DROP CONSTRAINT UTC_default_DeviceMemorySnapshot;
    ALTER TABLE [DeviceMemorySnapshot] DROP COLUMN [UpdatedToUTC];
END; 

-- DigitalSendJobOutput
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobOutput' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [DigitalSendJobOutput]
        SET [FileReceivedDateTime] = [FileReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [FileSentDateTime] = [FileSentDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DigitalSendJobOutput' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [DigitalSendJobOutput] DROP CONSTRAINT UTC_default_DigitalSendJobOutput;
    ALTER TABLE [DigitalSendJobOutput] DROP COLUMN [UpdatedToUTC];
END; 

-- PhysicalDeviceJob
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PhysicalDeviceJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [PhysicalDeviceJob]
        SET [JobEndDateTime] = [JobEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [JobStartDateTime] = [JobStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [MonitorEndDateTime] = [MonitorEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [MonitorStartDateTime] = [MonitorStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PhysicalDeviceJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [PhysicalDeviceJob] DROP CONSTRAINT UTC_default_PhysicalDeviceJob;
    ALTER TABLE [PhysicalDeviceJob] DROP COLUMN [UpdatedToUTC];
END; 

-- VirtualPrinterJob
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualPrinterJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [VirtualPrinterJob]
        SET [FirstByteReceivedDateTime] = [FirstByteReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [LastByteReceivedDateTime] = [LastByteReceivedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualPrinterJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [VirtualPrinterJob] DROP CONSTRAINT UTC_default_VirtualPrinterJob;
    ALTER TABLE [VirtualPrinterJob] DROP COLUMN [UpdatedToUTC];
END; 

-- PullPrintJobRetrieval
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [PullPrintJobRetrieval]
        SET [JobEndDateTime] = [JobEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [JobStartDateTime] = [JobStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PullPrintJobRetrieval' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [PullPrintJobRetrieval] DROP CONSTRAINT UTC_default_PullPrintJobRetrieval;
    ALTER TABLE [PullPrintJobRetrieval] DROP COLUMN [UpdatedToUTC];
END; 

-- PrintServerJob
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [PrintServerJob]
        SET [PrintEndDateTime] = [PrintEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [PrintStartDateTime] = [PrintStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [SpoolEndDateTime] = [SpoolEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [SpoolStartDateTime] = [SpoolStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [SubmittedDateTime] = [SubmittedDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintServerJob' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [PrintServerJob] DROP CONSTRAINT UTC_default_PrintServerJob;
    ALTER TABLE [PrintServerJob] DROP COLUMN [UpdatedToUTC];
END; 

-- PrintJobClient
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintJobClient' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [PrintJobClient]
        SET [JobEndDateTime] = [JobEndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [JobStartDateTime] = [JobStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [PrintStartDateTime] = [PrintStartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintJobClient' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [PrintJobClient] DROP CONSTRAINT UTC_default_PrintJobClient;
    ALTER TABLE [PrintJobClient] DROP COLUMN [UpdatedToUTC];
END; 

-- ActivityExecutionDetail
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionDetail' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [ActivityExecutionDetail]
        SET [DetailDateTime] = [DetailDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionDetail' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [ActivityExecutionDetail] DROP CONSTRAINT UTC_default_ActivityExecutionDetail;
    ALTER TABLE [ActivityExecutionDetail] DROP COLUMN [UpdatedToUTC];
END; 

-- ActivityExecution
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecution' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [ActivityExecution]
        SET [EndDateTime] = [EndDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [StartDateTime] = [StartDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecution' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [ActivityExecution] DROP CONSTRAINT UTC_default_ActivityExecution;
    ALTER TABLE [ActivityExecution] DROP COLUMN [UpdatedToUTC];
END; 

-- ActivityExecutionPerformance
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionPerformance' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    DECLARE @rows int;
    DECLARE @batchSize int = 3000;
    SET @rows = @batchSize;
    WHILE @rows = @batchSize
    BEGIN
        UPDATE TOP (@batchSize) [ActivityExecutionPerformance]
        SET [EventDateTime] = [EventDateTime] AT TIME ZONE 'Mountain Standard Time' AT TIME ZONE 'UTC',
            [UpdatedToUTC] = 1
        WHERE [UpdatedToUTC] IS NULL;
        SET @rows = @@ROWCOUNT;
    END;
END;

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ActivityExecutionPerformance' AND COLUMN_NAME = 'UpdatedToUTC')
BEGIN
    ALTER TABLE [ActivityExecutionPerformance] DROP CONSTRAINT UTC_default_ActivityExecutionPerformance;
    ALTER TABLE [ActivityExecutionPerformance] DROP COLUMN [UpdatedToUTC];
END; 

