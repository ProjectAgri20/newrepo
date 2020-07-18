/*****************************************************************************
 * This script performs the following actions:
 *   1. Disable change tracking on all tables.
 *   2. Disable change tracking on the database itself.
 *   3. Drop views.
 *   4. Drop unused tables.
 *   5. Drop default constraints in all tables.
 *   6. Drop all indexes.
 *   7. Drop user defined functions.
 *   8. Modify schema of select tables.
 *   9. Set clustered indexes on tables to the foreign key column.
 *  10. Copy data into table.
 *  11. Create other new indexes.
 *  12. Rebuild or reorganize fragmented indexes.
 *  13. Remove all elements from the plan cache of the database server instance.
 *  14. Update statistics.
 *****************************************************************************/
USE ScalableTestDataLog
GO

/*****************************************************************************/
/*****   1. Disable change tracking on all tables.                       *****/
/*****************************************************************************/
PRINT 'Disable change tracking on all tables'
DECLARE @disableChangeTracking varchar(max) = ''
SELECT @disableChangeTracking = 'IF EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(''?'')) ALTER TABLE ? DISABLE CHANGE_TRACKING'
EXEC sp_MSforeachtable @disableChangeTracking
GO

/*****************************************************************************/
/*****   2. Disable change tracking on the database itself.              *****/
/*****************************************************************************/
PRINT 'Disable change tracking on the database'
IF EXISTS(SELECT 1 FROM sys.change_tracking_databases WHERE database_id = DB_ID())
BEGIN
    USE master
    ALTER DATABASE ScalableTestDataLog SET CHANGE_TRACKING = OFF
    USE ScalableTestDataLog
END
GO

/*****************************************************************************/
/*****   3. Drop views.                                                  *****/
/*****************************************************************************/
PRINT 'Drop views in the Reports domain'
PRINT 'Drop dependent views first'

IF EXISTS(SELECT 1 FROM sys.views v JOIN sys.schemas s ON v.schema_id = s.schema_id WHERE s.[name] = 'Reports' AND v.[name] = 'vw_rpt_MOATJobReportDetails')
BEGIN
    PRINT ' ... vw_rpt_MOATJobReportDetails'
    DROP VIEW Reports.vw_rpt_MOATJobReportDetails
END
GO

IF EXISTS(SELECT 1 FROM sys.views v JOIN sys.schemas s ON v.schema_id = s.schema_id WHERE s.[name] = 'Reports' AND v.[name] = 'vw_rpt_MOATJobReportSummary')
BEGIN
    PRINT ' ... vw_rpt_MOATJobReportSummary'
    DROP VIEW Reports.vw_rpt_MOATJobReportSummary
END
GO

IF EXISTS(SELECT 1 FROM sys.views v JOIN sys.schemas s ON v.schema_id = s.schema_id WHERE s.[name] = 'Reports' AND v.[name] = 'vw_rpt_DigitalSendReportSummary')
BEGIN
    PRINT ' ... vw_rpt_DigitalSendReportSummary'
    DROP VIEW Reports.vw_rpt_DigitalSendReportSummary
END
GO

IF EXISTS(SELECT 1 FROM sys.views v JOIN sys.schemas s ON v.schema_id = s.schema_id WHERE s.[name] = 'Reports' AND v.[name] = 'vw_rpt_DigitalSendReportDetails')
BEGIN
    PRINT ' ... vw_rpt_DigitalSendReportDetails'
    DROP VIEW Reports.vw_rpt_DigitalSendReportDetails
END
GO

IF EXISTS(SELECT 1 FROM sys.views v JOIN sys.schemas s ON v.schema_id = s.schema_id WHERE s.[name] = 'Reports' AND v.[name] = 'vw_tbl_TempProduct_CTE')
BEGIN
    PRINT ' ... vw_tbl_TempProduct_CTE'
    DROP VIEW Reports.vw_tbl_TempProduct_CTE
END
GO

PRINT 'Drop remaining views in the Reports domain'
DECLARE @dropReportsViews varchar(max) = ''
SELECT @dropReportsViews = @dropReportsViews + 'DROP VIEW [' + s.[name] + '].[' + v.[name] + '];'
FROM sys.views v
JOIN sys.schemas s ON v.schema_id = s.schema_id
WHERE s.[name] = 'Reports'
IF LEN(@dropReportsViews) > 1
    EXEC(@dropReportsViews)
GO

/*****************************************************************************/
/*****   4. Drop unused tables.                                         *****/
/*****************************************************************************/
PRINT 'Drop unused tables'

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'ActivityExecutionPacing')
BEGIN
    PRINT ' ... ActivityExecutionPacing'
    DROP TABLE dbo.ActivityExecutionPacing
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'DemoActivityData')
BEGIN
    PRINT ' ... DemoActivityData'
    DROP TABLE dbo.DemoActivityData
END
GO

-- Only drop this table if it exists in the STB database, not STF.
IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'DirectorySnapshot') AND
   EXISTS(SELECT 1 FROM master.sys.fn_listextendedproperty(N'STF Revision', NULL, NULL, NULL, NULL, NULL, NULL))
BEGIN
    PRINT ' ... DirectorySnapshot'
    DROP TABLE dbo.DirectorySnapshot
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'ResourceWindowsEvent')
BEGIN
    PRINT ' ... ResourceWindowsEvent'
    DROP TABLE dbo.ResourceWindowsEvent
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'ResourceWindowsPerfMon')
BEGIN
    PRINT ' ... ResourceWindowsPerfMon'
    DROP TABLE dbo.ResourceWindowsPerfMon
END
GO

/*****************************************************************************/
/*****   5. Drop default constraints in all tables.                      *****/
/*****************************************************************************/
PRINT 'Drop default constraints'
DECLARE @schemaName varchar(max) = ''
DECLARE @constraintName varchar(max) = ''
DECLARE @tableName varchar(max) = ''

WHILE EXISTS(SELECT c.name
             FROM sys.objects c
             JOIN sys.tables t ON c.parent_object_id = t.object_id
             JOIN sys.schemas s ON t.schema_id = s.schema_id
             WHERE c.type = 'D' AND
                   t.is_ms_shipped = 0 AND
                   c.name > @constraintName)
BEGIN
    -- First get the constraint
    SELECT @constraintName = MIN(c.name)
    FROM sys.objects c
    JOIN sys.tables t ON c.parent_object_id = t.object_id
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE c.type = 'D' AND
          t.is_ms_shipped = 0 AND
          c.name > @constraintName

    -- Then select the table and schema associated with the current constraint
    SELECT @tableName = t.name
          ,@schemaName = s.name
    FROM sys.objects c
    JOIN sys.tables t ON c.parent_object_id = t.object_id
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE c.name = @constraintName

    -- Then print to the output and drop the constraint
    PRINT 'Dropping constraint ' + @constraintName + ' on table ' + @tableName
    EXEC('ALTER TABLE [' + @schemaName + '].[' + @tableName + '] DROP CONSTRAINT [' + @constraintName + '];')
END
GO

/*****************************************************************************/
/*****   6. Drop all indexes.                                            *****/
/*****************************************************************************/
PRINT 'Drop all indexes'
DECLARE @dropIndexes varchar(max) = ''
SELECT @dropIndexes += 'DROP INDEX [' + i.name + '] ON [' + o.name + '];'
FROM sys.indexes i
JOIN sys.objects o ON i.object_id = o.object_id
WHERE o.type = 'U' and i.name IS NOT NULL AND o.is_ms_shipped = 0 AND i.is_primary_key <> 1
IF LEN(@dropIndexes) > 1
    EXEC(@dropIndexes)
GO

/*****************************************************************************/
/*****   7. Drop user defined functions.                                 *****/
/*****************************************************************************/
PRINT 'Drop function SessionIdsMatchingPatterns'
IF EXISTS(SELECT 1 FROM sys.objects WHERE [type] = 'TF' AND [name] = 'SessionIdsMatchingPatterns')
    DROP FUNCTION dbo.SessionIdsMatchingPatterns
GO

PRINT 'Drop function ValueListToContainedPatterns'
IF EXISTS(SELECT 1 FROM sys.objects WHERE [type] = 'TF' AND [name] = 'ValueListToContainedPatterns')
    DROP FUNCTION dbo.ValueListToContainedPatterns
GO

/*****************************************************************************/
/*****   8. Modify schema of select tables.                              *****/
/*****************************************************************************/
PRINT 'Modify table schemas'

PRINT 'Modify ActivityExecution'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecution')
BEGIN
    ALTER TABLE ActivityExecution ALTER COLUMN ResourceInstanceId nvarchar(50) NULL
    ALTER TABLE ActivityExecution ALTER COLUMN UserName nvarchar(50) NULL
    ALTER TABLE ActivityExecution ALTER COLUMN HostName nvarchar(50) NULL
    ALTER TABLE ActivityExecution ALTER COLUMN [Status] varchar(20) NOT NULL
    ALTER TABLE ActivityExecution ALTER COLUMN ResultMessage nvarchar(1024) NULL
END
GO

PRINT 'Modify ActivityExecutionAssetUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionAssetUsage')
BEGIN
    ALTER TABLE ActivityExecutionAssetUsage ALTER COLUMN AssetId nvarchar(50) NOT NULL
END
GO

PRINT 'Create ActivityExecutionDetail'
IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionDetail')
BEGIN
    CREATE TABLE [dbo].[ActivityExecutionDetail](
	    [ActivityExecutionDetailId] [uniqueidentifier] NOT NULL,
	    [SessionId] [varchar](50) NOT NULL,
	    [ActivityExecutionId] [uniqueidentifier] NOT NULL,
	    [Label] [nvarchar](255) NULL,
	    [Message] [nvarchar](1024) NULL,
	    [DetailDateTime] [datetime] NULL,
     CONSTRAINT [PK_ActivityExecutionDetail] PRIMARY KEY NONCLUSTERED 
    (
	    [ActivityExecutionDetailId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ActivityExecutionDetail]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionDetail_SessionSummary] FOREIGN KEY([SessionId])
    REFERENCES [dbo].[SessionSummary] ([SessionId])
    ON UPDATE CASCADE
    ON DELETE CASCADE

    ALTER TABLE [dbo].[ActivityExecutionDetail] CHECK CONSTRAINT [FK_ActivityExecutionDetail_SessionSummary]
END
GO

PRINT 'Modify ActivityExecutionDocumentUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionDocumentUsage')
BEGIN
    ALTER TABLE ActivityExecutionDocumentUsage ALTER COLUMN DocumentName nvarchar(255) NOT NULL
END
GO

PRINT 'Modify ActivityExecutionPerformance'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionPerformance')
BEGIN
    UPDATE ActivityExecutionPerformance SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE ActivityExecutionPerformance ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
END
GO

PRINT 'Modify ActivityExecutionRetries'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionRetries')
BEGIN
    UPDATE ActivityExecutionRetries SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE ActivityExecutionRetries ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE ActivityExecutionRetries ALTER COLUMN [Status] varchar(20) NULL
    ALTER TABLE ActivityExecutionRetries ALTER COLUMN ResultMessage nvarchar(1024) NULL
    ALTER TABLE ActivityExecutionRetries ALTER COLUMN ResultCategory nvarchar(1024) NULL
END
GO

PRINT 'Modify ActivityExecutionServerUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionServerUsage')
BEGIN
    UPDATE ActivityExecutionServerUsage SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    UPDATE ActivityExecutionServerUsage SET ServerName = '' WHERE ServerName IS NULL
    UPDATE ActivityExecutionServerUsage SET ServerName = LEFT(ServerName, 50) WHERE LEN(ServerName) > 50
    ALTER TABLE ActivityExecutionServerUsage ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE ActivityExecutionServerUsage ALTER COLUMN ServerName nvarchar(50) NOT NULL
END
GO

PRINT 'Modify DeviceEvent'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceEvent')
BEGIN
    ALTER TABLE DeviceEvent ALTER COLUMN AssetId nvarchar(50) NULL
    ALTER TABLE DeviceEvent ALTER COLUMN [Address] nvarchar(50) NULL
    ALTER TABLE DeviceEvent ALTER COLUMN EventType nvarchar(20) NULL
    ALTER TABLE DeviceEvent ALTER COLUMN EventCode nvarchar(20) NULL
    ALTER TABLE DeviceEvent ALTER COLUMN EventDescription nvarchar(1024) NULL
END
GO

PRINT 'Modify DeviceMemoryCount'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryCount')
BEGIN
    ALTER TABLE DeviceMemoryCount ALTER COLUMN DataLabel varchar(255) NOT NULL
END
GO

PRINT 'Modify DeviceMemorySnapshot'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemorySnapshot')
BEGIN
    ALTER TABLE DeviceMemorySnapshot ALTER COLUMN DeviceId nvarchar(50) NOT NULL
    ALTER TABLE DeviceMemorySnapshot ALTER COLUMN SnapshotLabel nvarchar(50) NULL
END
GO

PRINT 'Modify DeviceMemoryXML'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryXML')
BEGIN
    EXEC sp_rename 'dbo.DeviceMemoryXML', 'DeviceMemoryXml'
    EXEC sp_rename 'dbo.DeviceMemoryXml.MemoryXML', 'MemoryXml'
END
GO

PRINT 'Modify DigitalSendJobInput'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobInput')
BEGIN
    UPDATE DigitalSendJobInput SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN FilePrefix nvarchar(255) NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN Sender nvarchar(50) NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN DeviceId nvarchar(50) NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN ScanType nvarchar(50) NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN JobEndStatus nvarchar(50) NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN [PageCount] smallint NULL
    ALTER TABLE DigitalSendJobInput ALTER COLUMN DestinationCount smallint NULL
END
GO

PRINT 'Modify DigitalSendJobNotification'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobNotification')
BEGIN
    ALTER TABLE DigitalSendJobNotification ALTER COLUMN FilePrefix nvarchar(255) NULL
    ALTER TABLE DigitalSendJobNotification ALTER COLUMN NotificationDestination nvarchar(255) NULL
    ALTER TABLE DigitalSendJobNotification ALTER COLUMN NotificationResult nvarchar(50) NULL
END
GO

PRINT 'Modify DigitalSendJobOutput'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobOutput')
BEGIN
    ALTER TABLE DigitalSendJobOutput ALTER COLUMN FilePrefix nvarchar(255) NULL
    ALTER TABLE DigitalSendJobOutput ALTER COLUMN [FileName] nvarchar(255) NULL
    ALTER TABLE DigitalSendJobOutput ALTER COLUMN FileLocation nvarchar(255) NULL
    ALTER TABLE DigitalSendJobOutput ALTER COLUMN [PageCount] smallint NULL
    ALTER TABLE DigitalSendJobOutput ALTER COLUMN ErrorMessage nvarchar(1024) NULL
END
GO

PRINT 'Modify DigitalSendJobTempFile'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobTempFile')
BEGIN
    ALTER TABLE DigitalSendJobTempFile ALTER COLUMN TotalBytes bigint NULL
END
GO

PRINT 'Modify DigitalSendServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendServerJob')
BEGIN
    ALTER TABLE DigitalSendServerJob ALTER COLUMN JobType nvarchar(50) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN CompletionStatus nvarchar(50) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN FileType nvarchar(50) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN [FileName] nvarchar(255) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN FileSizeBytes bigint NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN ScannedPages smallint NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN DeviceModel nvarchar(50) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN DssVersion nvarchar(50) NULL
    ALTER TABLE DigitalSendServerJob ALTER COLUMN ProcessedBy nvarchar(50) NULL
END
GO

PRINT 'Modify DigitalSendTempSnapshot'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendTempSnapshot')
BEGIN
    ALTER TABLE DigitalSendTempSnapshot ALTER COLUMN HostName nvarchar(50) NULL
END
GO

PRINT 'Modify DirectorySnapshot'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DirectorySnapshot')
BEGIN
    ALTER TABLE DirectorySnapshot ALTER COLUMN SnapshotDateTime datetime NULL
    ALTER TABLE DirectorySnapshot ALTER COLUMN HostName nvarchar(50) NULL
    ALTER TABLE DirectorySnapshot ALTER COLUMN DirectoryName nvarchar(1024) NULL
    ALTER TABLE DirectorySnapshot ALTER COLUMN TotalFiles int NULL
    ALTER TABLE DirectorySnapshot ALTER COLUMN TotalBytes bigint NULL
END
GO

PRINT 'Modify EPrintServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'EPrintServerJob')
BEGIN
    ALTER TABLE EPrintServerJob ALTER COLUMN EmailAccount nvarchar(50) NULL
    ALTER TABLE EPrintServerJob ALTER COLUMN PrinterName nvarchar(50) NULL
    ALTER TABLE EPrintServerJob ALTER COLUMN JobName nvarchar(255) NULL
    ALTER TABLE EPrintServerJob ALTER COLUMN JobFolderId nvarchar(255) NULL
    ALTER TABLE EPrintServerJob ALTER COLUMN JobStatus nvarchar(50) NULL
    ALTER TABLE EPrintServerJob ALTER COLUMN TransactionStatus nvarchar(50) NULL
END
GO

PRINT 'Modify JetAdvantagePullPrintJobRetrieval'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'JetAdvantagePullPrintJobRetrieval')
BEGIN
    UPDATE JetAdvantagePullPrintJobRetrieval SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN Username nvarchar(50) NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN JetAdvantageLoginId nvarchar(50) NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN DeviceId nvarchar(50) NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN SolutionType nvarchar(50) NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN JobEndStatus nvarchar(50) NULL
    ALTER TABLE JetAdvantagePullPrintJobRetrieval ALTER COLUMN ErrorMessage nvarchar(1024) NULL
END
GO

PRINT 'Modify JetAdvantageUpload'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'JetAdvantageUpload')
BEGIN
    UPDATE JetAdvantageUpload SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN LoginId nvarchar(50) NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN DestinationUrl nvarchar(255) NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN [FileName] nvarchar(255) NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN FileType nvarchar(50) NULL
    ALTER TABLE JetAdvantageUpload ALTER COLUMN CompletionStatus nvarchar(50) NULL
END
GO

PRINT 'Modify PhysicalDeviceJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PhysicalDeviceJob')
BEGIN
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN DeviceId nvarchar(50) NULL
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN JobId nvarchar(50) NULL
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN JobName nvarchar(255) NULL
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN JobApplicationName nvarchar(255) NULL
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN JobCategory nvarchar(50) NULL
    ALTER TABLE PhysicalDeviceJob ALTER COLUMN JobEndStatus nvarchar(50) NULL
END
GO

PRINT 'Modify PrintJobClient'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PrintJobClient')
BEGIN
    ALTER TABLE PrintJobClient ALTER COLUMN [FileName] nvarchar(255) NULL
    ALTER TABLE PrintJobClient ALTER COLUMN FileSizeBytes bigint NULL
    ALTER TABLE PrintJobClient ALTER COLUMN ClientOS nvarchar(50) NULL
    ALTER TABLE PrintJobClient ALTER COLUMN PrintQueue nvarchar(255) NULL
    ALTER TABLE PrintJobClient ALTER COLUMN PrintType nvarchar(50) NULL
    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintJobClient' AND COLUMN_NAME = 'DevicePlatform')
    BEGIN
        ALTER TABLE PrintJobClient DROP COLUMN DevicePlatform
    END
END
GO

PRINT 'Modify PrintServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PrintServerJob')
BEGIN
    ALTER TABLE PrintServerJob ALTER COLUMN PrintServer nvarchar(50) NULL
    ALTER TABLE PrintServerJob ALTER COLUMN PrintServerOS nvarchar(50) NULL
    ALTER TABLE PrintServerJob ALTER COLUMN PrintQueue nvarchar(255) NULL
    ALTER TABLE PrintServerJob ALTER COLUMN PrintDriver nvarchar(255) NULL
    ALTER TABLE PrintServerJob ALTER COLUMN DataType varchar(20) NULL
    ALTER TABLE PrintServerJob ALTER COLUMN JobTotalPages smallint NULL
    ALTER TABLE PrintServerJob ALTER COLUMN PrintedPages smallint NULL
    ALTER TABLE PrintServerJob ALTER COLUMN Copies smallint NULL
    ALTER TABLE PrintServerJob ALTER COLUMN NumberUp smallint NULL
END
GO

PRINT 'Modify PullPrintJobRetrieval'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PullPrintJobRetrieval')
BEGIN
    UPDATE PullPrintJobRetrieval SET ActivityExecutionId = '00000000-0000-0000-0000-000000000000' WHERE ActivityExecutionId IS NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN ActivityExecutionId uniqueidentifier NOT NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN UserName nvarchar(50) NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN DeviceId nvarchar(50) NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN SolutionType nvarchar(50) NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN JobEndStatus nvarchar(50) NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN InitialJobCount smallint NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN FinalJobCount smallint NULL
    ALTER TABLE PullPrintJobRetrieval ALTER COLUMN NumberOfCopies smallint NULL
END
GO

PRINT 'Modify SessionConfiguration'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionConfiguration')
BEGIN
    UPDATE SessionConfiguration SET ConfigurationData = '' WHERE ConfigurationData IS NULL
    ALTER TABLE SessionConfiguration ALTER COLUMN ConfigurationData xml NOT NULL
END
GO

PRINT 'Modify SessionDevice'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionDevice')
BEGIN
    ALTER TABLE SessionDevice ALTER COLUMN DeviceId nvarchar(50) NOT NULL
    ALTER TABLE SessionDevice ALTER COLUMN ProductName nvarchar(255) NULL
    ALTER TABLE SessionDevice ALTER COLUMN DeviceName nvarchar(255) NULL
    ALTER TABLE SessionDevice ALTER COLUMN FirmwareRevision nvarchar(50) NULL
    ALTER TABLE SessionDevice ALTER COLUMN FirmwareDatecode nvarchar(20) NULL
    ALTER TABLE SessionDevice ALTER COLUMN FirmwareType nvarchar(50) NULL
    ALTER TABLE SessionDevice ALTER COLUMN ModelNumber nvarchar(50) NULL
    ALTER TABLE SessionDevice ALTER COLUMN NetworkCardModel nvarchar(50) NULL
    ALTER TABLE SessionDevice ALTER COLUMN NetworkInterfaceVersion nvarchar(255) NULL
    ALTER TABLE SessionDevice ALTER COLUMN IpAddress nvarchar(50) NULL
END
GO

PRINT 'Modify SessionDocument'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionDocument')
BEGIN
    ALTER TABLE SessionDocument ALTER COLUMN [FileName] nvarchar(255) NULL
    ALTER TABLE SessionDocument ALTER COLUMN Extension nvarchar(10) NULL
    ALTER TABLE SessionDocument ALTER COLUMN FileType nvarchar(50) NULL
    ALTER TABLE SessionDocument ALTER COLUMN FileSizeKilobytes bigint NULL
    ALTER TABLE SessionDocument ALTER COLUMN Pages smallint NULL
    ALTER TABLE SessionDocument ALTER COLUMN ColorMode varchar(10) NULL
    ALTER TABLE SessionDocument ALTER COLUMN Orientation varchar(10) NULL
    ALTER TABLE SessionDocument ALTER COLUMN DefectId nvarchar(50) NULL
    ALTER TABLE SessionDocument ALTER COLUMN Tag nvarchar(255) NULL
END
GO

PRINT 'Modify SessionProductAssoc'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionProductAssoc')
BEGIN
    ALTER TABLE SessionProductAssoc ALTER COLUMN [Version] nvarchar(1024) NULL
    ALTER TABLE SessionProductAssoc ALTER COLUMN [Name] nvarchar(50) NULL
    ALTER TABLE SessionProductAssoc ALTER COLUMN Vendor nvarchar(255) NULL
END
GO

PRINT 'Modify SessionServer'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionServer')
BEGIN
    UPDATE SessionServer SET ServerId = '00000000-0000-0000-0000-000000000000' WHERE ServerId IS NULL
    ALTER TABLE SessionServer ALTER COLUMN ServerId uniqueidentifier NOT NULL
    ALTER TABLE SessionServer ALTER COLUMN HostName nvarchar(50) NULL
    ALTER TABLE SessionServer ALTER COLUMN [Address] nvarchar(50) NULL
    ALTER TABLE SessionServer ALTER COLUMN OperatingSystem nvarchar(50) NULL
    ALTER TABLE SessionServer ALTER COLUMN Processors smallint NULL
    ALTER TABLE SessionServer ALTER COLUMN Cores smallint NULL
END
GO

PRINT 'Modify SessionSummary'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionSummary')
BEGIN
    ALTER TABLE SessionSummary ALTER COLUMN [Owner] nvarchar(50) NULL
    ALTER TABLE SessionSummary ALTER COLUMN Dispatcher nvarchar(50) NULL
    ALTER TABLE SessionSummary ALTER COLUMN [Status] varchar(20) NULL
    ALTER TABLE SessionSummary ALTER COLUMN Tags nvarchar(255) NULL
    ALTER TABLE SessionSummary ALTER COLUMN STFVersion nvarchar(50) NULL
    ALTER TABLE SessionSummary ALTER COLUMN [Type] nvarchar(50) NULL
    ALTER TABLE SessionSummary ALTER COLUMN Cycle nvarchar(50) NULL
    ALTER TABLE SessionSummary ALTER COLUMN Reference nvarchar(255) NULL
    ALTER TABLE SessionSummary ALTER COLUMN Notes nvarchar(max) NULL
    ALTER TABLE SessionSummary ALTER COLUMN ShutdownState varchar(20) NULL
    ALTER TABLE SessionSummary ALTER COLUMN ExpirationDateTime datetime NULL
    ALTER TABLE SessionSummary ALTER COLUMN ShutdownUser nvarchar(50) NULL
END
GO

PRINT 'Modify TriageData'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'TriageData')
BEGIN
    ALTER TABLE TriageData ALTER COLUMN ControlIds nvarchar(max) NULL
    ALTER TABLE TriageData ALTER COLUMN Reason nvarchar(max) NULL
END
GO

PRINT 'Modify VirtualPrinterJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'VirtualPrinterJob')
BEGIN
    ALTER TABLE VirtualPrinterJob ALTER COLUMN PjlJobName nvarchar(255) NULL
    ALTER TABLE VirtualPrinterJob ALTER COLUMN PjlLanguage nvarchar(50) NULL
END
GO

/*****************************************************************************/
/*****   9. Set clustered indexes on tables to the foreign key column.   *****/
/*****************************************************************************/
PRINT 'Reset clustered indexes'

PRINT 'Reset ActivityExecution'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecution')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_ActivityExecution' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecution DROP CONSTRAINT PK_ActivityExecution
        ALTER TABLE dbo.ActivityExecution ADD CONSTRAINT PK_ActivityExecution PRIMARY KEY NONCLUSTERED
        (ActivityExecutionId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecution_SessionId ON dbo.ActivityExecution
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset ActivityExecutionAssetUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionAssetUsage')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_ActivityExecutionAssetUsage' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecutionAssetUsage DROP CONSTRAINT PK_ActivityExecutionAssetUsage
        ALTER TABLE dbo.ActivityExecutionAssetUsage ADD CONSTRAINT PK_ActivityExecutionAssetUsage PRIMARY KEY NONCLUSTERED
        (ActivityExecutionAssetUsageId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecutionAssetUsage_SessionId ON dbo.ActivityExecutionAssetUsage
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset ActivityExecutionDetail'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionDetail')
BEGIN
    CREATE CLUSTERED INDEX [Idx_ActivityExecutionDetail_SessionId] ON [dbo].[ActivityExecutionDetail]
    (
	    [SessionId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

PRINT 'Reset ActivityExecutionDocumentUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionDocumentUsage')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_ActivityExecutionDocumentUsage' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecutionDocumentUsage DROP CONSTRAINT PK_ActivityExecutionDocumentUsage
        ALTER TABLE dbo.ActivityExecutionDocumentUsage ADD CONSTRAINT PK_ActivityExecutionDocumentUsage PRIMARY KEY NONCLUSTERED
        (ActivityExecutionDocumentUsageId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecutionDocumentUsage_SessionId ON dbo.ActivityExecutionDocumentUsage
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset ActivityExecutionPerformance'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionPerformance')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'Pk_ActivityExecutionPerformance' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecutionPerformance DROP CONSTRAINT Pk_ActivityExecutionPerformance
        ALTER TABLE dbo.ActivityExecutionPerformance ADD CONSTRAINT PK_ActivityExecutionPerformance PRIMARY KEY NONCLUSTERED
        (ActivityExecutionPerformanceId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecutionPerformance_SessionId ON dbo.ActivityExecutionPerformance
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset ActivityExecutionRetries'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionRetries')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_ActivityExecutionRetries' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecutionRetries DROP CONSTRAINT PK_ActivityExecutionRetries
        ALTER TABLE dbo.ActivityExecutionRetries ADD CONSTRAINT PK_ActivityExecutionRetries PRIMARY KEY NONCLUSTERED
        (ActivityExecutionRetriesId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecutionRetries_SessionId ON dbo.ActivityExecutionRetries
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset ActivityExecutionServerUsage'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'ActivityExecutionServerUsage')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_ActivityExecutionServerUsage' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.ActivityExecutionServerUsage DROP CONSTRAINT PK_ActivityExecutionServerUsage
        ALTER TABLE dbo.ActivityExecutionServerUsage ADD CONSTRAINT PK_ActivityExecutionServerUsage PRIMARY KEY NONCLUSTERED
        (ActivityExecutionServerUsageId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_ActivityExecutionServerUsage_SessionId ON dbo.ActivityExecutionServerUsage
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DeviceEvent'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceEvent')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DeviceEvent' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DeviceEvent DROP CONSTRAINT PK_DeviceEvent
        ALTER TABLE dbo.DeviceEvent ADD CONSTRAINT PK_DeviceEvent PRIMARY KEY NONCLUSTERED
        (DeviceEventId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DeviceEvent_SessionId ON dbo.DeviceEvent
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DeviceMemoryCount'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryCount')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DeviceMemoryCount' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DeviceMemoryCount DROP CONSTRAINT PK_DeviceMemoryCount
        ALTER TABLE dbo.DeviceMemoryCount ADD CONSTRAINT PK_DeviceMemoryCount PRIMARY KEY NONCLUSTERED
        (DeviceMemoryCountId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DeviceMemoryCount_DeviceMemorySnapshotId ON dbo.DeviceMemoryCount
    (DeviceMemorySnapshotId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DeviceMemorySnapshot'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryCount') AND
   EXISTS(SELECT 1 FROM sys.foreign_keys WHERE [name] = 'FK_DeviceMemoryCount_DeviceMemorySnapshot')
BEGIN
    ALTER TABLE dbo.DeviceMemoryCount DROP CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryXml') AND
   EXISTS(SELECT 1 FROM sys.foreign_keys WHERE [name] = 'FK_DeviceMemoryXML_DeviceMemorySnapshot')
BEGIN
    ALTER TABLE dbo.DeviceMemoryXml DROP CONSTRAINT FK_DeviceMemoryXML_DeviceMemorySnapshot
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemorySnapshot')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DeviceMemorySnapshot' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DeviceMemorySnapshot DROP CONSTRAINT PK_DeviceMemorySnapshot
        ALTER TABLE dbo.DeviceMemorySnapshot ADD CONSTRAINT PK_DeviceMemorySnapshot PRIMARY KEY NONCLUSTERED
        (DeviceMemorySnapshotId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

        IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryCount')
        BEGIN
            ALTER TABLE dbo.DeviceMemoryCount WITH CHECK ADD CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot FOREIGN KEY(DeviceMemorySnapshotId)
            REFERENCES dbo.DeviceMemorySnapshot (DeviceMemorySnapshotId)
            ON UPDATE CASCADE
            ON DELETE CASCADE
            ALTER TABLE dbo.DeviceMemoryCount CHECK CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot
        END

        IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryXml')
        BEGIN
            ALTER TABLE dbo.DeviceMemoryXml WITH CHECK ADD CONSTRAINT FK_DeviceMemoryXml_DeviceMemorySnapshot FOREIGN KEY(DeviceMemorySnapshotId)
            REFERENCES dbo.DeviceMemorySnapshot (DeviceMemorySnapshotId)
            ON UPDATE CASCADE
            ON DELETE CASCADE
            ALTER TABLE dbo.DeviceMemoryXml CHECK CONSTRAINT FK_DeviceMemoryXml_DeviceMemorySnapshot
        END
    END

    CREATE CLUSTERED INDEX Idx_DeviceMemorySnapshot_SessionId ON dbo.DeviceMemorySnapshot
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DeviceMemoryXml'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DeviceMemoryXml')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DeviceMemoryXML' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DeviceMemoryXml DROP CONSTRAINT PK_DeviceMemoryXML
        ALTER TABLE dbo.DeviceMemoryXml ADD CONSTRAINT PK_DeviceMemoryXML PRIMARY KEY NONCLUSTERED
        (DeviceMemoryXmlId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DeviceMemoryXml_DeviceMemorySnapshotId ON dbo.DeviceMemoryXml
    (DeviceMemorySnapshotId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendJobInput'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobInput')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DigitalSendJobInput' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendJobInput DROP CONSTRAINT PK_DigitalSendJobInput
        ALTER TABLE dbo.DigitalSendJobInput ADD CONSTRAINT PK_DigitalSendJobInput PRIMARY KEY NONCLUSTERED
        (DigitalSendJobInputId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendJobInput_SessionId ON dbo.DigitalSendJobInput
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendJobNotification'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobNotification')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'INDEX_NAME_ON_PRIMARY_KEY' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendJobNotification DROP CONSTRAINT PK_DigitalSendJobNotification
        ALTER TABLE dbo.DigitalSendJobNotification ADD CONSTRAINT PK_DigitalSendJobNotification PRIMARY KEY NONCLUSTERED
        (DigitalSendJobNotificationId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendJobNotification_SessionId ON dbo.DigitalSendJobNotification
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendJobOutput'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobOutput')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DigitalSendJobOutput' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendJobOutput DROP CONSTRAINT PK_DigitalSendJobOutput
        ALTER TABLE dbo.DigitalSendJobOutput ADD CONSTRAINT PK_DigitalSendJobOutput PRIMARY KEY NONCLUSTERED
        (DigitalSendJobOutputId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendJobOutput_SessionId ON dbo.DigitalSendJobOutput
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendJobTempFile'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobTempFile')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DigitalSendJobTempFile' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendJobTempFile DROP CONSTRAINT PK_DigitalSendJobTempFile
        ALTER TABLE dbo.DigitalSendJobTempFile ADD CONSTRAINT PK_DigitalSendJobTempFile PRIMARY KEY NONCLUSTERED
        (DigitalSendJobTempFileId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendJobTempFile_SessionId ON dbo.DigitalSendJobTempFile
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendServerJob')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DigitalSendServerJob' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendServerJob DROP CONSTRAINT PK_DigitalSendServerJob
        ALTER TABLE dbo.DigitalSendServerJob ADD CONSTRAINT PK_DigitalSendServerJob PRIMARY KEY NONCLUSTERED
        (DigitalSendServerJobId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendServerJob_SessionId ON dbo.DigitalSendServerJob
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset DigitalSendTempSnapshot'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendTempSnapshot')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_DigitalSendTempSnapshot' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.DigitalSendTempSnapshot DROP CONSTRAINT PK_DigitalSendTempSnapshot
        ALTER TABLE dbo.DigitalSendTempSnapshot ADD CONSTRAINT PK_DigitalSendTempSnapshot PRIMARY KEY NONCLUSTERED
        (DigitalSendTempSnapshotId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_DigitalSendTempSnapshot_SessionId ON dbo.DigitalSendTempSnapshot
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset EPrintServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'EPrintServerJob')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_EPrintServerJob' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.EPrintServerJob DROP CONSTRAINT PK_EPrintServerJob
        ALTER TABLE dbo.EPrintServerJob ADD CONSTRAINT PK_EPrintServerJob PRIMARY KEY NONCLUSTERED
        (EPrintServerJobId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_EPrintServerJob_SessionId ON dbo.EPrintServerJob
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset JetAdvantagePullPrintJobRetrieval'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'JetAdvantagePullPrintJobRetrieval')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_JetAdvantagePullPrintJobRetrieval' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.JetAdvantagePullPrintJobRetrieval DROP CONSTRAINT PK_JetAdvantagePullPrintJobRetrieval
        ALTER TABLE dbo.JetAdvantagePullPrintJobRetrieval ADD CONSTRAINT PK_JetAdvantagePullPrintJobRetrieval PRIMARY KEY NONCLUSTERED
        (PK_JetAdvantagePullPrintJobRetrieval ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_JetAdvantagePullPrintJobRetrieval_SessionId ON dbo.JetAdvantagePullPrintJobRetrieval
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset JetAdvantageUpload'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'JetAdvantageUpload')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_JetAdvantageUpload' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.JetAdvantageUpload DROP CONSTRAINT PK_JetAdvantageUpload
        ALTER TABLE dbo.JetAdvantageUpload ADD CONSTRAINT PK_JetAdvantageUpload PRIMARY KEY NONCLUSTERED
        (JetAdvantageUploadId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_JetAdvantageUpload_SessionId ON dbo.JetAdvantageUpload
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset PhysicalDeviceJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PhysicalDeviceJob')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_PhysicalDeviceJob' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.PhysicalDeviceJob DROP CONSTRAINT PK_PhysicalDeviceJob
        ALTER TABLE dbo.PhysicalDeviceJob ADD CONSTRAINT PK_PhysicalDeviceJob PRIMARY KEY NONCLUSTERED
        (PhysicalDeviceJobId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_PhysicalDeviceJob_SessionId ON dbo.PhysicalDeviceJob
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset PrintJobClient'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PrintJobClient')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_PrintJobClient' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.PrintJobClient DROP CONSTRAINT PK_PrintJobClient
        ALTER TABLE dbo.PrintJobClient ADD CONSTRAINT PK_PrintJobClient PRIMARY KEY NONCLUSTERED
        (PrintJobClientId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_PrintJobClient_SessionId ON dbo.PrintJobClient
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset PrintServerJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PrintServerJob')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_PrintServerJob' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.PrintServerJob DROP CONSTRAINT PK_PrintServerJob
        ALTER TABLE dbo.PrintServerJob ADD CONSTRAINT PK_PrintServerJob PRIMARY KEY NONCLUSTERED
        (PrintServerJobId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_PrintServerJob_PrintJobClientId ON dbo.PrintServerJob
    (PrintJobClientId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset PullPrintJobRetrieval'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'PullPrintJobRetrieval')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_PullPrintJobRetrieval' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.PullPrintJobRetrieval DROP CONSTRAINT PK_PullPrintJobRetrieval
        ALTER TABLE dbo.PullPrintJobRetrieval ADD CONSTRAINT PK_PullPrintJobRetrieval PRIMARY KEY NONCLUSTERED
        (PullPrintJobRetrievalId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_PullPrintJobRetrieval_SessionId ON dbo.PullPrintJobRetrieval
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset SessionDevice'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionDevice')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_SessionDevice' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.SessionDevice DROP CONSTRAINT PK_SessionDevice
        ALTER TABLE dbo.SessionDevice ADD CONSTRAINT PK_SessionDevice PRIMARY KEY NONCLUSTERED
        (SessionDeviceId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_SessionDevice_SessionId ON dbo.SessionDevice
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset SessionDocument'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionDocument')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_SessionDocument' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.SessionDocument DROP CONSTRAINT PK_SessionDocument
        ALTER TABLE dbo.SessionDocument ADD CONSTRAINT PK_SessionDocument PRIMARY KEY NONCLUSTERED
        (SessionDocumentId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_SessionDocument_SessionId ON dbo.SessionDocument
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset SessionServer'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'SessionServer')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_SessionServer' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.SessionServer DROP CONSTRAINT PK_SessionServer
        ALTER TABLE dbo.SessionServer ADD CONSTRAINT PK_SessionServer PRIMARY KEY NONCLUSTERED
        (SessionServerId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_SessionServer_SessionId ON dbo.SessionServer
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset TriageData'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'TriageData')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_TriageData' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.TriageData DROP CONSTRAINT PK_TriageData
        ALTER TABLE dbo.TriageData ADD CONSTRAINT PK_TriageData PRIMARY KEY NONCLUSTERED
        (TriageDataId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_TriageData_SessionId ON dbo.TriageData
    (SessionId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

PRINT 'Reset VirtualPrinterJob'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'VirtualPrinterJob')
BEGIN
    -- Recreate the index on the primary key if it is clustered.
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'PK_VirtualPrinterJob' AND type = 1 AND is_primary_key = 1)
    BEGIN
        ALTER TABLE dbo.VirtualPrinterJob DROP CONSTRAINT PK_VirtualPrinterJob
        ALTER TABLE dbo.VirtualPrinterJob ADD CONSTRAINT PK_VirtualPrinterJob PRIMARY KEY NONCLUSTERED
        (VirtualPrinterJobId ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END

    CREATE CLUSTERED INDEX Idx_VirtualPrinterJob_PrintJobClientId ON dbo.VirtualPrinterJob
    (PrintJobClientId ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/*****************************************************************************/
/***** 10. Copy data into table.                                         *****/
/*****************************************************************************/
PRINT 'Copy data into table'

PRINT 'Copy data from ActivityExecutionPerformance to ActivityExecutionDetail'
INSERT INTO ActivityExecutionDetail (ActivityExecutionDetailId, SessionId, ActivityExecutionId, Label, [Message], DetailDateTime)
SELECT aep.ActivityExecutionPerformanceId
      ,aep.SessionId
      ,aep.ActivityExecutionId
      ,CASE
           WHEN aep.EventLabel LIKE '%[_]%' THEN LEFT(aep.EventLabel, CHARINDEX('_', aep.EventLabel) - 1)
           ELSE LEFT(aep.EventLabel, CHARINDEX('=', aep.EventLabel) - 1)
       END
      ,CASE
           WHEN aep.EventLabel LIKE '%[_]%' THEN SUBSTRING(aep.EventLabel, CHARINDEX('_', aep.EventLabel) + 1, 1024)
           ELSE SUBSTRING(aep.EventLabel, CHARINDEX('=', aep.EventLabel) + 1, 1024)
       END
      ,aep.EventDateTime
FROM ActivityExecutionPerformance aep
WHERE aep.EventLabel LIKE '%[_=]%' AND
      aep.ActivityExecutionPerformanceId NOT IN (SELECT ActivityExecutionDetailId FROM ActivityExecutionDetail)

/*****************************************************************************/
/***** 11. Create other new indexes.                                     *****/
/*****************************************************************************/
PRINT 'Create additional indexes'

PRINT 'Create Idx_ActivityExecutionAssetUsage_ActivityExecutionId'
CREATE NONCLUSTERED INDEX Idx_ActivityExecutionAssetUsage_ActivityExecutionId ON dbo.ActivityExecutionAssetUsage
(ActivityExecutionId ASC)
INCLUDE (AssetId)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

PRINT 'Create Idx_ActivityExecutionPerformance_EventLabel'
CREATE NONCLUSTERED INDEX Idx_ActivityExecutionPerformance_EventLabel ON dbo.ActivityExecutionPerformance
(EventLabel ASC)
INCLUDE (ActivityExecutionId, EventIndex)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

PRINT 'Create Idx_ActivityExecutionServerUsage_ActivityExecutionId'
CREATE NONCLUSTERED INDEX Idx_ActivityExecutionServerUsage_ActivityExecutionId ON dbo.ActivityExecutionServerUsage
(ActivityExecutionId ASC)
INCLUDE (ServerName)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

PRINT 'Create Idx_DigitalSendJobInput_ActivityExecutionId'
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND [name] = 'DigitalSendJobInput')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_DigitalSendJobInput_ActivityExecutionId ON dbo.DigitalSendJobInput
    (ActivityExecutionId ASC)
    INCLUDE (FilePrefix, Ocr)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

PRINT 'Create Idx_PrintJobClient_ActivityExecutionId'
CREATE NONCLUSTERED INDEX Idx_PrintJobClient_ActivityExecutionId ON dbo.PrintJobClient
(ActivityExecutionId ASC)
INCLUDE (PrintJobClientId, [FileName], FileSizeBytes, ClientOS, PrintQueue, JobEndDateTime, PrintStartDateTime)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*****************************************************************************/
/***** 12. Rebuild or reorganize fragmented indexes.                     *****/
/*****************************************************************************/
PRINT 'Defragmenting indexes'
DECLARE @fragTemp AS TABLE
(
    id int identity(1, 1),
    [objectId][int] null,
    [indexId][int] null,
    [partitionNum][int] null,
    [frag][float] null
)

DECLARE @count int
DECLARE @i tinyint = 1
DECLARE @schemaName sysname
DECLARE @objectName sysname
DECLARE @indexName sysname
DECLARE @objectId int
DECLARE @indexId int
DECLARE @partitionNum bigint
DECLARE @partitionCount bigint
DECLARE @sqlCmd varchar(max)
DECLARE @fragPercent float

INSERT INTO @fragTemp
SELECT object_id AS objectId
      ,index_id AS indexId
      ,partition_number AS partitionNum
      ,avg_fragmentation_in_percent AS frag
FROM sys.dm_db_index_physical_stats(DB_ID(), null, null, null, 'LIMITED')
WHERE avg_fragmentation_in_percent >= 5.0 and index_id > 0

SELECT @count = COUNT(*) FROM @fragTemp

WHILE(@i <= @count)
BEGIN
    SELECT @objectId = objectId
          ,@indexId = indexId
          ,@partitionNum = partitionNum
          ,@fragPercent = frag
    FROM @fragTemp
    WHERE id = @i

    -- Get table name and its schema
    SELECT @objectName = o.name
          ,@schemaName = c.name
    FROM sys.objects o
    INNER JOIN sys.schemas c ON o.schema_id = c.schema_id
    WHERE o.object_id = @objectId

    -- Get index name
    SELECT @indexName = name
    FROM sys.indexes
    WHERE index_id = @indexId AND object_id = @objectId

    -- Get partition count
    SELECT @partitionCount = COUNT(*)
    FROM sys.partitions
    WHERE object_id = @objectId AND index_id = @indexId

    IF @fragPercent < 30.0
        SELECT @sqlCmd = 'ALTER INDEX ' + @indexName + ' ON ' + @schemaName + '.' + @objectName + ' REORGANIZE'
    if @fragPercent >= 30.0
        SELECT @sqlCmd = 'ALTER INDEX ' + @indexName + ' ON ' + @schemaName + '.' + @objectName + ' REBUILD'
    IF(@partitionCount > 1) SELECT @sqlCmd = @sqlCmd + ' PARTITION = ' + CONVERT(CHAR, @partitionNum)

    EXEC(@sqlCmd)

    PRINT 'Executed: ' + @sqlCmd + ', Fragmentation was ' + CONVERT(VARCHAR, @fragPercent) + '%' + CHAR(13) + CHAR(10)

    -- Increment count
    SET @i = @i + 1
END
GO

/*****************************************************************************/
/***** 13. Remove all elements from the plan cache of the database       *****/
/*****     server instance.                                              *****/
/*****************************************************************************/
PRINT 'Clearing the plan cache of the database server'
DBCC FREEPROCCACHE
GO

/*****************************************************************************/
/***** 14. Update statistics.                                            *****/
/*****************************************************************************/
PRINT 'Updating the statistics in the database'
EXEC SP_UPDATESTATS
GO
