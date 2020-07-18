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

-- 2/2/2016 fdelagarza Migration script to New STF 4.0 base Schema including Data Migration and Views Uppdate

PRINT '*******************************************************************'
PRINT '***  Drop Views with Dependencies on all the Schema Changes... ***'
PRINT '*******************************************************************'
IF OBJECT_ID('Reports.vw_rpt_ActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityDetails...'
	DROP VIEW [Reports].[vw_rpt_ActivityDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivitySummary...'
	DROP VIEW [Reports].[vw_rpt_ActivitySummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityTaskDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityTaskStepDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskStepDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskStepDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityTaskStepExecution', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskStepExecution View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskStepExecution]
END
GO
IF OBJECT_ID('Reports.vw_rpt_BrianDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianDetails...'
	DROP VIEW [Reports].[vw_rpt_BrianDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_BrianSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianSummary...'
	DROP VIEW [Reports].[vw_rpt_BrianSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivityDetails...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendActivityDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivitySummary...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendActivitySummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_DigitalSendReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendReportDetails...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_DigitalSendReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendReportSummary...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ePrintReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ePrintReportDetails...'
	DROP VIEW [Reports].[vw_rpt_ePrintReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_MOATActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivityDetails...'
	DROP VIEW [Reports].[vw_rpt_MOATActivityDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_MOATActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivitySummary...'
	DROP VIEW [Reports].[vw_rpt_MOATActivitySummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_MOATJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATJobReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_MOATJobReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_MOATJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATJobReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_MOATJobReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PINPrintActivityReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PINPrintActivityReportDetails...'
	DROP VIEW [Reports].[vw_rpt_PINPrintActivityReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PINPrintActivityReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PINPrintActivityReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PINPrintActivityReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PrintJobReportDetails...'
	DROP VIEW [Reports].[vw_rpt_PrintJobReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PrintJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PrintJobReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PrintJobReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportDetails...'
	DROP VIEW [Reports].[vw_rpt_PullPrintActivityReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PullPrintActivityReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportDetails...'
	DROP VIEW [Reports].[vw_rpt_PullPrintJobReportDetails]
END
GO
IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PullPrintJobReportSummary]
END
GO
IF OBJECT_ID('Reports.vw_tbl_TempProduct_CTE', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempProduct_CTE...'
	DROP VIEW [Reports].[vw_tbl_TempProduct_CTE]
END
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

IF COL_LENGTH('dbo.ActivityExecution', 'UpdateType') IS NOT NULL AND COL_LENGTH('dbo.ActivityExecution', 'Status') IS NULL
BEGIN
	PRINT '**********************************************'
	PRINT '***  ActivityExecution Column changes ...  ***'
	PRINT '**********************************************'
	EXEC sp_rename 'ActivityExecution.UpdateType', 'Status', 'COLUMN';
	PRINT 'ActivityExecution UpdateType renamed to Status'
END
ELSE
	PRINT 'ActivityExecution Status Column already exists Or Column UpdateType not exists...'
GO
IF COL_LENGTH('dbo.ActivityExecution', 'ErrorMessage') IS NOT NULL AND COL_LENGTH('dbo.ActivityExecution', 'ResultMessage') IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecution.ErrorMessage', 'ResultMessage', 'COLUMN';
	PRINT 'ActivityExecution ErrorMessage renamed to ResultMessage'
END
ELSE
	PRINT 'ActivityExecution ResultMessage Column already exists Or Column ErrorMessage not exists...'
GO
IF COL_LENGTH('dbo.ActivityExecution', 'Label') IS NOT NULL AND COL_LENGTH('dbo.ActivityExecution', 'ResultCategory') IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecution.Label', 'ResultCategory', 'COLUMN';
	PRINT 'ActivityExecution Label renamed to ResultCategory'
END
ELSE
	PRINT 'ActivityExecution ResultCategory Column already exists Or Column Label not exists...'
GO
IF COL_LENGTH('dbo.ActivityExecution', 'ResultCategory') IS NOT NULL AND COL_LENGTH('dbo.ActivityExecution', 'ResultMessage') IS NOT NULL
BEGIN
	PRINT 'Initializing ResultMessage to NULL when is an empty string...'
	UPDATE dbo.ActivityExecution
	SET
		ResultMessage= NULL
	WHERE RTRIM(LTRIM(ResultMessage)) = '';

PRINT 'Initializing ResultCategory from ResultMessage column...'
	UPDATE dbo.ActivityExecution
	SET
		ResultCategory= CASE
							WHEN ResultMessage IS NULL THEN NULL
							WHEN CHARINDEX(':', ResultMessage) = 0 THEN ResultMessage
							ELSE LEFT(ResultMessage, CHARINDEX(':', ResultMessage) - 1)
						END;
PRINT 'Finished Initializing ResultCategory column... '
END
ELSE
	PRINT 'ActivityExecution ResultCategory Or ResultMessage Label not exist...'
GO

IF COL_LENGTH('dbo.ActivityExecution', 'RetryCount') IS NOT NULL
BEGIN
	ALTER TABLE dbo.ActivityExecution DROP COLUMN RetryCount ;
	PRINT 'ActivityExecution RetryCount Column dropped'
END
PRINT 'ActivityExecution Column changes completed'
PRINT ''

IF OBJECT_ID('dbo.ActivityExecutionRetries', 'U') IS NULL
BEGIN
	PRINT '**********************************************'
	PRINT '***  Create ActivityExecutionRetries Table ***'
	PRINT '**********************************************'

	CREATE TABLE [dbo].[ActivityExecutionRetries](
	[ActivityExecutionRetriesId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[Status] [varchar](50) NULL,
	[ResultMessage] [varchar](1024) NULL,
	[ResultCategory] [varchar](1024) NULL,
	[RetryStartDateTime] [datetime] NULL,
	CONSTRAINT [PK_ActivityExecutionRetries] PRIMARY KEY NONCLUSTERED ([ActivityExecutionRetriesId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY];
	
	ALTER TABLE [dbo].[ActivityExecutionRetries]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionRetries_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[ActivityExecutionRetries] CHECK CONSTRAINT [FK_ActivityExecutionRetries_SessionSummary];

	CREATE CLUSTERED INDEX [Idx_ActivityExecutionRetries_SessionId] ON [dbo].[ActivityExecutionRetries] ([SessionId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);
	
	PRINT 'Creation Of ActivityExecutionRetries table completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionDeviceUsage', 'U') IS NOT NULL
BEGIN
	PRINT '***************************************************************************'
	PRINT '***  Rename ActivityExecutionDeviceUsage To ActivityExecutionAssetUsage ***'
	PRINT '***       and column Name Changes ...                                   ***'
	PRINT '***************************************************************************'
	EXEC sp_rename 'ActivityExecutionDeviceUsage', 'ActivityExecutionAssetUsage';
	EXEC sp_rename 'ActivityExecutionAssetUsage.ActivityExecutionDeviceUsageId', 'ActivityExecutionAssetUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsage.DeviceId', 'AssetId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsage.PK_ActivityExecutionDeviceUsage', 'PK_ActivityExecutionAssetUsage', 'INDEX';
	IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'Idx_ActivityExecutionDeviceUsage_ActivityId')
	BEGIN
		EXEC sp_rename 'ActivityExecutionAssetUsage.Idx_ActivityExecutionDeviceUsage_ActivityId', 'Idx_ActivityExecutionAssetUsage_ActivityId', 'INDEX';
	END
	IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'Idx_ActivityExecutionDeviceUsage_SessionId')
	BEGIN
		EXEC sp_rename 'ActivityExecutionAssetUsage.Idx_ActivityExecutionDeviceUsage_SessionId', 'Idx_ActivityExecutionAssetUsage_SessionId', 'INDEX';
	END
	EXEC sp_rename 'FK_ActivityExecutionDeviceUsage_SessionSummary', 'FK_ActivityExecutionAssetUsage_SessionSummary';

	PRINT 'conversion Of ActivityExecutionDeviceUsage  -->  ActivityExecutionAssetUsage completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionDevice', 'U') IS NOT NULL
BEGIN
	PRINT '****************************************************'
	PRINT '***  SessionDevice Nullable Columns changes ...  ***'
	PRINT '****************************************************'
	ALTER TABLE SessionDevice ALTER COLUMN ProductName Varchar(100) NULL;
	ALTER TABLE SessionDevice ALTER COLUMN DeviceName Varchar(255) NULL;
	ALTER TABLE SessionDevice ALTER COLUMN FirmwareRevision Varchar(50) NULL;
	ALTER TABLE SessionDevice ALTER COLUMN FirmwareDatecode Varchar(10) NULL;
	ALTER TABLE SessionDevice ALTER COLUMN FirmwareType Varchar(50) NULL;
	ALTER TABLE SessionDevice ALTER COLUMN ModelNumber Varchar(50) NULL;
	PRINT 'conversion Of SessionDevice Nullable Columns completed'
END
GO

IF COL_LENGTH('dbo.ActivityExecutionAssetUsage', 'Product') IS NOT NULL AND COL_LENGTH('dbo.ActivityExecutionAssetUsage', 'FirmwareVersion') IS NOT NULL
BEGIN
	PRINT 'Inserting Device and FW history records from ActivityExecutionAssetUsage...'
	INSERT INTO dbo.SessionDevice
		SELECT DISTINCT MAX(a.ActivityExecutionAssetUsageId) AS SessionDeviceId
			,a.SessionId
			,a.AssetId AS DeviceId
			,a.Product AS ProductName
			,NULL AS DeviceName
			,a.FirmwareVersion AS FirmwareRevision
			,NULL AS FirmwareDatecode
			,NULL AS FirmwareType
			,NULL AS ModelNumber
		  FROM dbo.ActivityExecutionAssetUsage a
		  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = a.SessionId AND sd.DeviceId = a.AssetId
		WHERE sd.DeviceId is NULL
		GROUP BY a.SessionId, a.AssetId, a.Product, a.FirmwareVersion

	PRINT 'Addition of Device and FW history records completed'
	PRINT ''
END
GO

IF COL_LENGTH('dbo.ActivityExecutionAssetUsage', 'Product') IS NOT NULL
BEGIN
	ALTER TABLE dbo.ActivityExecutionAssetUsage DROP COLUMN Product ;
	PRINT 'ActivityExecutionAssetUsage Product Column dropped'
END

IF COL_LENGTH('dbo.ActivityExecutionAssetUsage', 'FirmwareVersion') IS NOT NULL
BEGIN
	ALTER TABLE dbo.ActivityExecutionAssetUsage DROP COLUMN FirmwareVersion ;
	PRINT 'ActivityExecutionAssetUsage FirmwareVersion Column dropped'
END

IF OBJECT_ID('dbo.SessionServer', 'U') IS NULL
BEGIN
	PRINT '***********************************'
	PRINT '***  Create SessionServer Table ***'
	PRINT '***********************************'

	CREATE TABLE [dbo].[SessionServer](
	[SessionServerId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ServerId] [uniqueidentifier] NULL,
	[HostName] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[OperatingSystem] [varchar](50) NULL,
	[Architecture] [varchar](10) NULL,
	[Processors] [int] NULL,
	[Cores] [int] NULL,
	[Memory] [int] NULL,
	CONSTRAINT [PK_SessionServer] PRIMARY KEY NONCLUSTERED  ([SessionServerId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) 
	
	ALTER TABLE [dbo].[SessionServer]  WITH CHECK ADD  CONSTRAINT [FK_SessionServer_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[SessionServer] CHECK CONSTRAINT [FK_SessionServer_SessionSummary]
	
	CREATE CLUSTERED INDEX [Idx_SessionServer_SessionId] ON [dbo].[SessionServer] ([SessionId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT 'Creation Of SessionServer table completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.TestDocumentUsage', 'U') IS NOT NULL
BEGIN
	PRINT '*******************************************************************'
	PRINT '***  Rename TestDocumentUsage To ActivityExecutionDocumentUsage ***'
	PRINT '*******************************************************************'
	EXEC sp_rename 'TestDocumentUsage', 'ActivityExecutionDocumentUsage';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.TestDocumentUsageId', 'ActivityExecutionDocumentUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.FileName', 'DocumentName', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.PK_TestDocumentUsage', 'PK_ActivityExecutionDocumentUsage', 'INDEX';
	DELETE FROM   ActivityExecutionDocumentUsage WHERE  SessionId NOT IN (SELECT SessionId FROM SessionSummary);
	CREATE INDEX Idx_ActivityExecutionDocumentUsage_SessionId ON dbo.ActivityExecutionDocumentUsage ( SessionId );
	ALTER TABLE dbo.ActivityExecutionDocumentUsage ADD CONSTRAINT FK_ActivityExecutionDocumentUsage_SessionSummary FOREIGN KEY ( SessionId )
	REFERENCES dbo.SessionSummary( SessionId ) ON UPDATE CASCADE ON DELETE CASCADE;

	PRINT 'conversion Of TestDocumentUsage  -->  ActivityExecutionDocumentUsage completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.TestDocumentReference', 'U') IS NOT NULL
BEGIN
	PRINT '********************************************************'
	PRINT '***  Rename TestDocumentReference To SessionDocument ***'
	PRINT '********************************************************'
	EXEC sp_rename 'TestDocumentReference', 'SessionDocument';
	EXEC sp_rename 'SessionDocument.TestDocumentReferenceId', 'SessionDocumentId', 'COLUMN';
	EXEC sp_rename 'SessionDocument.PK_TestDocumentReference', 'PK_SessionDocument', 'INDEX';
	DELETE FROM   SessionDocument WHERE  SessionId NOT IN (SELECT SessionId FROM SessionSummary);
	CREATE INDEX Idx_SessionDocument_SessionId ON dbo.SessionDocument ( SessionId );
	ALTER TABLE dbo.SessionDocument ADD CONSTRAINT FK_SessionDocument_SessionSummary FOREIGN KEY ( SessionId )
	REFERENCES dbo.SessionSummary( SessionId ) ON UPDATE CASCADE ON DELETE CASCADE;

	PRINT 'conversion Of TestDocumentReference  -->  SessionDocument completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.TaskExecution', 'U') IS NOT NULL
BEGIN
	PRINT '******************************************************'
	PRINT '***  Rename TaskExecution To ActivityTaskExecution ***'
	PRINT '******************************************************'
	EXEC sp_rename 'TaskExecution', 'ActivityTaskExecution';
	EXEC sp_rename 'ActivityTaskExecution.TaskExecutionId', 'ActivityTaskExecutionId', 'COLUMN';
	EXEC sp_rename 'ActivityTaskExecution.PK_TaskExecution', 'PK_ActivityTaskExecution', 'INDEX';
	EXEC sp_rename 'FK_TaskExecution_SessionSummary', 'FK_ActivityTaskExecution_SessionSummary';

	PRINT 'conversion Of TaskExecution  -->  ActivityTaskExecution completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.StepExecution', 'U') IS NOT NULL
BEGIN
	PRINT '**********************************************************'
	PRINT '***  Rename StepExecution To ActivityTaskStepExecution ***'
	PRINT '**********************************************************'
	EXEC sp_rename 'StepExecution', 'ActivityTaskStepExecution';
	EXEC sp_rename 'ActivityTaskStepExecution.StepExecutionId', 'ActivityTaskStepExecutionId', 'COLUMN';
	EXEC sp_rename 'ActivityTaskStepExecution.TaskExecutionId', 'ActivityTaskExecutionId', 'COLUMN';
	EXEC sp_rename 'ActivityTaskStepExecution.PK_StepExecution', 'PK_ActivityTaskStepExecution', 'INDEX';
	EXEC sp_rename 'FK_StepExecution_SessionSummary', 'FK_ActivityTaskStepExecution_SessionSummary';

	PRINT 'conversion Of StepExecution  -->  ActivityTaskStepExecution completed'
	PRINT ''
END
GO

PRINT '***************************************************************'
PRINT '***  Re Create Dropped Views compatible with Schema Changes ***'
PRINT '***************************************************************'

IF OBJECT_ID('Reports.vw_rpt_ActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityDetails]
END
GO
PRINT 'Creating vw_rpt_ActivityDetails View ...'
GO
--
-- This view is used in the 'Activity Details' report.
--
CREATE VIEW [Reports].[vw_rpt_ActivityDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.AssetId AS DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
	   ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       COALESCE((pd.[Group] + ' ' + CASE pd.Color
                                        WHEN 1 THEN 'Color'
                                        WHEN 0 THEN 'Mono'
                                        ELSE ''
                                    END + ' ' + pd.MaxMediaSize), 'NA') AS DeviceCategory,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
                          MIN(AssetId) AssetId
                     FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
                     FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_ActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivitySummary View ...'
	DROP VIEW [Reports].[vw_rpt_ActivitySummary]
END
GO
PRINT 'Creating vw_rpt_ActivitySummary View ...'
GO
--
-- This view is used in the 'Activity Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_ActivitySummary]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.ActivityType,
       ae.Status AS UpdateType,
       sd.ProductName AS Product,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
                          MIN(AssetId) AssetId
                     FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
                     FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_ActivityTaskDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskDetails]
END
GO
PRINT 'Creating vw_rpt_ActivityTaskDetails View ...'
GO
/******************************************************************/
/*****  Object:  View [Reports].[vw_rpt_ActivityTaskDetails]  *****/
/*****  Script Date:  1/11/2016                               *****/
/*****  Author:  Gary Parham                                  *****/
/******************************************************************/
CREATE VIEW [Reports].[vw_rpt_ActivityTaskDetails]
WITH SCHEMABINDING
AS
	SELECT  ae.SessionId
		   ,ae.ActivityName
		   ,ae.ActivityType
		   ,ae.Status AS ActivityStatus
		   ,ae.StartDateTime AS ActivityStartDateTime
		   ,ae.EndDateTime AS ActivityEndDataTime
		   ,DATEDIFF(SECOND, ae.StartDateTime, ae.EndDateTime) AS ActivityDurationSeconds
		   ,sd.ProductName AS Product
		   ,ISNULL(pd.[Platform], 'Jedi') AS DeviceType
		   ,ae.ResultMessage AS ErrorMessage
		   ,ae.ResultCategory AS GroupErrorMessage
		   ,ISNULL(aer.RetryCount,0) AS RetryCount
		   ,ate.TaskName
		   ,ate.[Status] AS TaskStatus
		   ,ate.StartDateTime AS TaskStartDateTime
		   ,ate.EndDateTime AS TaskEndDateTime
		   ,DATEDIFF(SECOND, ate.StartDateTime, ate.EndDateTime) AS TaskDurationSeconds
	  FROM  dbo.ActivityExecution ae
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
                          MIN(AssetId) AssetId
                     FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
                     FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
	  JOIN  dbo.ActivityTaskExecution ate ON ae.ActivityExecutionId = ate.ActivityExecutionId
	 WHERE  ae.ActivityName NOT LIKE 'SessionReset%'
GO

IF OBJECT_ID('Reports.vw_rpt_ActivityTaskStepDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskStepDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskStepDetails]
END
GO
PRINT 'Creating vw_rpt_ActivityTaskStepDetails View ...'
GO
/**********************************************************************/
/*****  Object:  View [Reports].[vw_rpt_ActivityTaskStepDetails]  *****/
/*****  Script Date:  1/11/2016                                   *****/
/*****  Author:  Gary Parham                                      *****/
/**********************************************************************/
CREATE VIEW [Reports].[vw_rpt_ActivityTaskStepDetails]
WITH SCHEMABINDING
AS
	SELECT  ae.SessionId
	   ,ae.ActivityName
	   ,ae.ActivityType
	   ,ae.StartDateTime as ActivityStartDateTime
	   ,ae.EndDateTime as ActivityEndDateTime
	   ,ae.Status AS UpdateType
	   ,sd.ProductName Product
	   ,ae.ResultMessage AS ErrorMessage
	   ,ae.ResultCategory AS GroupErrorMessage
	   ,ISNULL(aer.RetryCount,0) AS RetryCount
	   ,ISNULL(pd.[Platform], 'Jedi') AS DeviceType
	   ,FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
	   ,ate.TaskName
	   ,ate.ExecutionPath
	   ,ate.[Status] as TaskStatus
	   ,ate.StartDateTime as TaskStartDateTime
	   ,ate.EndDateTime as TaskEndDateTime
	   ,DATEDIFF(s, ate.StartDateTime, ate.EndDateTime) as TaskDurationSeconds	   
	   ,atse.StepName
	   ,atse.[Status] as StepStatus
	   ,atse.StartDateTime as StepStartDateTime
	   ,atse.EndDateTime as StepEndDateTime
	   ,DATEDIFF(s, atse.StartDateTime, atse.EndDateTime) as StepDurationSeconds
	FROM  dbo.ActivityExecution ae
	INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
	LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
	LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
	LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
	LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
	LEFT OUTER JOIN dbo.ActivityTaskExecution ate on ae.ActivityExecutionId = ate.ActivityExecutionId
	LEFT OUTER JOIN dbo.ActivityTaskStepExecution atse on ate.ActivityTaskExecutionId = atse.ActivityTaskExecutionId
	WHERE atse.StepName IS NOT NULL
GO

IF OBJECT_ID('Reports.vw_rpt_ActivityTaskStepExecution', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityTaskStepExecution View ...'
	DROP VIEW [Reports].[vw_rpt_ActivityTaskStepExecution]
END
GO
PRINT 'Creating vw_rpt_ActivityTaskStepExecution View ...'
GO
CREATE VIEW [Reports].[vw_rpt_ActivityTaskStepExecution]
WITH SCHEMABINDING
AS
	SELECT ae.SessionId,
		   ae.ActivityName,
		   ae.ActivityType,
		   ae.StartDateTime as ActivityStartDateTime,
		   ae.EndDateTime as ActivityEndDateTime,
		   ae.Status AS UpdateType,
		   sd.ProductName AS Product,
		   ae.ResultMessage AS ErrorMessage,
		   ae.ResultCategory AS GroupErrorMessage,
		   ISNULL(aer.RetryCount,0) AS RetryCount,
		   ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
		   FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour,
		   ate.TaskName,
		   ate.ExecutionPath,
		   ate.[Status] as TaskStatus,
		   ate.StartDateTime as TaskStartDateTime,
		   ate.EndDateTime as TaskEndDateTime,
		   DATEDIFF(s, ate.StartDateTime, ate.EndDateTime) as TaskDurationSeconds,	   
		   se.StepName,
		   se.[Status] as StepStatus,
		   se.StartDateTime as StepStartDateTime,
		   se.EndDateTime as StepEndDateTime,
		   DATEDIFF(s, se.StartDateTime, se.EndDateTime) as StepDurationSeconds
	  FROM dbo.ActivityExecution ae
	  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
	  LEFT OUTER JOIN (SELECT ActivityExecutionId,
							  MIN(AssetId) AssetId
						 FROM dbo.ActivityExecutionAssetUsage
						 GROUP BY ActivityExecutionId
						 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
	  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
	  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
	  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
						 FROM dbo.ActivityExecutionRetries
						 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
	  LEFT OUTER JOIN dbo.ActivityTaskExecution ate on ae.ActivityExecutionId = ate.ActivityExecutionId
	  LEFT OUTER JOIN dbo.ActivityTaskStepExecution se on ate.ActivityTaskExecutionId = se.ActivityTaskExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_BrianDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianDetails View ...'
	DROP VIEW [Reports].[vw_rpt_BrianDetails]
END
GO
PRINT 'Creating vw_rpt_BrianDetails View ...'
GO
--
-- This view is used in the 'Brian Details' report.
--
CREATE VIEW [Reports].[vw_rpt_BrianDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.AssetId AS DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       pjc.PrintJobClientId,
       pjc.[FileName],
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       COALESCE((pd.[Group] + ' ' + CASE pd.Color
                                        WHEN 1 THEN 'Color'
                                        WHEN 0 THEN 'Mono'
                                        ELSE ''
                                    END + ' ' + pd.MaxMediaSize), 'NA') AS DeviceCategory,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON pjc.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_BrianSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianSummary View ...'
	DROP VIEW [Reports].[vw_rpt_BrianSummary]
END
GO
PRINT 'Creating vw_rpt_BrianSummary View ...'
GO
--
-- This view is used in the 'Brian Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_BrianSummary]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.ActivityType,
       ae.Status AS UpdateType,
       sd.ProductName AS Product,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivityDetails View ...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendActivityDetails]
END
GO
PRINT 'Creating vw_rpt_DigitalSendActivityDetails View ...'
GO
--
-- This view is used in the 'Digital Send Activity Details' report.
--
CREATE VIEW [Reports].[vw_rpt_DigitalSendActivityDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.AssetId AS DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       CASE
           WHEN ae.ActivityName IS NULL THEN ''
           WHEN CHARINDEX('_', ae.ActivityName) = 0 THEN ''
           ELSE LEFT(ae.ActivityName, CHARINDEX('_', ae.ActivityName) - 1)
       END AS ScanTypeDetail,
       CASE
           WHEN ae.ActivityName IS NULL THEN ''
           WHEN CHARINDEX('_Non-OCR', ae.ActivityName) > 0 THEN 'Non-OCR'
           WHEN CHARINDEX('_OCR', ae.ActivityName) > 0 THEN 'OCR'
           ELSE ''
       END AS [OCR Type],
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       COALESCE((pd.[Group] + ' ' + CASE pd.Color
                                        WHEN 1 THEN 'Color'
                                        WHEN 0 THEN 'Mono'
                                        ELSE ''
                                    END + ' ' + pd.MaxMediaSize), 'NA') AS DeviceCategory,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivitySummary View ...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendActivitySummary]
END
GO
PRINT 'Creating vw_rpt_DigitalSendActivitySummary View ...'
GO
--
-- This view is used in the 'Digital Send Activity Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_DigitalSendActivitySummary]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.ActivityType,
       ae.Status AS UpdateType,
       sd.ProductName AS Product,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       CASE
           WHEN ae.ActivityName IS NULL THEN ''
           WHEN CHARINDEX('_', ae.ActivityName) = 0 THEN ''
           ELSE LEFT(ae.ActivityName, CHARINDEX('_', ae.ActivityName) - 1)
       END AS ScanTypeDetail,
       CASE
           WHEN ae.ActivityName IS NULL THEN ''
           WHEN CHARINDEX('_Non-OCR', ae.ActivityName) > 0 THEN 'Non-OCR'
           WHEN CHARINDEX('_OCR', ae.ActivityName) > 0 THEN 'OCR'
           ELSE ''
       END AS [OCR Type],
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportDetails]
END
GO
PRINT 'Creating vw_rpt_DigitalSendReportDetails View ...'
GO
--
-- This view is used in the 'Digital Send Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_DigitalSendReportDetails]
WITH SCHEMABINDING
AS
SELECT ISNULL(dsji.SessionId, dsjo.SessionId) AS SessionId,
       ae.ActivityName,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       dsji.FilePrefix,
       CASE
           WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN 'Folder DFS'
           ELSE dsji.ScanType
       END AS Scantype,
       CASE dsji.Ocr
           WHEN 1 THEN 'OCR'
           WHEN 0 THEN CASE
                           WHEN (CHARINDEX(' OCR', ae.ActivityName) > 0) OR (CHARINDEX('Non', ae.ActivityName) !> 0) THEN 'OCR'
                           ELSE 'Non-OCR'
                       END
           ELSE ''
       END AS OCR,
       dsji.DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       pd.Model,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN RTRIM(pd.MaxMediaSize) + ' Network Scanner'
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                       WHEN 1 THEN 'Color'
                                       WHEN 0 THEN 'Mono'
                                       ELSE ''
                                   END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN 'Color'
           WHEN 0 THEN 'Mono'
           ELSE ''
       END AS DeviceColor,
       pd.MaxMediaSize,
       dsji.Sender,
       dsji.[PageCount],
       dsji.DestinationCount,
       dsji.ActivityStartDateTime,
       dsji.ScanStartDateTime,
       dsji.ScanEndDateTime,
       dsji.JobEndDateTime,
       dsji.JobEndStatus,
       dsjo.FileSentDateTime,
       dsjo.FileReceivedDateTime,
       CASE
           WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
           ELSE dsjo.FileSentDateTime
       END AS FileArrived,
       dsjo.[FileName],
       dsjo.FileSizeBytes AS OutputfileSizeBytes,
       dsjo.FileLocation,
       CASE
           WHEN dsjo.FileLocation IS NULL THEN 'None'
           ELSE CASE
                    WHEN dsji.ScanType = 'Folder Multi' THEN RIGHT(dsjo.FileLocation, 1)
                    ELSE '1'
                END
       END AS Destination,
       dsjo.[PageCount] AS DestPages,
       dsjo.[PageCount] - dsji.[PageCount] AS PageDelta,
       CASE
           WHEN dsji.FilePrefix IS NOT NULL THEN CASE
                                                     WHEN dsjo.[FileName] IS NOT NULL THEN CASE
                                                                                               WHEN (dsjo.[PageCount] - dsji.[PageCount]) != 0 THEN CASE
                                                                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN 'Page Delta'
                                                                                                                                                        ELSE 'FAIL'
                                                                                                                                                    END
                                                                                               ELSE CASE
                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN 'Pass'
                                                                                                        ELSE 'FAIL'
                                                                                                    END
                                                                                           END
                                                     ELSE CASE
                                                              WHEN CHARINDEX('Complete', ae.Status) > 0 THEN 'Missing'
                                                              ELSE 'N/A'
                                                          END
                                                 END
           ELSE 'Orphaned'
       END AS ValidationResult,
       dsjo.ErrorMessage AS OutputError,
       dssj.JobType,
       dssj.CompletionStatus,
       dssj.CompletionDateTime,
       dssj.FileSizeBytes AS ServerFileSizeBytes,
       dssj.FileType,
       dssj.ScannedPages,
       dssj.DssVersion,
       dssj.ProcessedBy,
       dssj.DeviceModel,
       DATEDIFF(SECOND, dsji.ActivityStartDateTime, dsji.ScanStartDateTime) / 3600.0 / 24.0 AS Act_2_scanStart,
       DATEDIFF(SECOND, dsji.ScanStartDateTime, dsji.ScanEndDateTime) / 3600.0 / 24.0 AS ScanTime,
       CASE
           WHEN dsji.ScanEndDateTime > (CASE
                                            WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                            ELSE dsjo.FileSentDateTime
                                        END) THEN 0
           ELSE DATEDIFF(SECOND, dsji.ScanEndDateTime, CASE
                                                           WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                                           ELSE dsjo.FileSentDateTime
                                                       END) / 3600.0 / 24.0
       END AS scanEnd_2_fileSent,
       DATEDIFF(SECOND, dsji.ScanEndDateTime, dssj.CompletionDateTime) / 3600.0 / 24.0 AS scanEnd_2_DSSTime,
       DATEDIFF(SECOND, dsji.ActivityStartDateTime, CASE
                                                        WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                                        ELSE dsjo.FileSentDateTime
                                                    END) / 3600.0 / 24.0 AS TotalTime,
       CASE
           WHEN dssj.CompletionDateTime < dsji.ActivityStartDateTime THEN 'No'
           ELSE 'Yes'
       END AS ValidDSSTime,
       CASE
           WHEN dssj.DeviceModel IS NULL THEN 'No'
           ELSE CASE
                    WHEN CHARINDEX(RIGHT(pd.Model, 4), dssj.DeviceModel) > 0 THEN 'No'
                    ELSE 'Yes'
                END
       END AS [Duplicate?],
       CASE
           WHEN CHARINDEX('SIM', dsji.DeviceId) > 0 THEN 1
           ELSE 0
       END AS [SIM?],
       FLOOR((DATEDIFF(SECOND, ss.StartDateTime, dsji.ActivityStartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.DigitalSendJobInput dsji
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN dbo.DigitalSendServerJob dssj ON dsjo.[FileName] = dssj.[FileName]
                                                   AND (dssj.JobType != 'SendFax' OR dssj.CompletionDateTime IS NULL OR dssj.CompletionDateTime > dsji.ActivityStartDateTime)
                                                   AND (CHARINDEX(RIGHT(pd.Model, 10), dssj.DeviceModel) > 0 OR dssj.DeviceModel IS NULL)
                                                   AND (dsjo.[PageCount] = dssj.ScannedPages OR dssj.ScannedPages IS NULL)
                                                   AND (DATEDIFF(SECOND, dsjo.FileReceivedDateTime, dssj.CompletionDateTime) < 3600 OR dssj.CompletionStatus IS NULL)
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportSummary]
END
GO
PRINT 'Creating vw_rpt_DigitalSendReportSummary View ...'
GO
--
-- This view is used in the 'Digital Send Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_DigitalSendReportSummary]
WITH SCHEMABINDING
AS
SELECT ISNULL(dsji.SessionId, dsjo.SessionId) AS SessionId,
       ae.Status AS UpdateType,
       dsji.FilePrefix,
       CASE
           WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN 'Folder DFS'
           ELSE dsji.ScanType
       END AS Scantype,
       CASE dsji.Ocr
           WHEN 1 THEN 'OCR'
           WHEN 0 THEN CASE
                           WHEN (CHARINDEX(' OCR', ae.ActivityName) > 0) OR (CHARINDEX('Non', ae.ActivityName) !> 0) THEN 'OCR'
                           ELSE 'Non-OCR'
                       END
           ELSE ''
       END AS OCR,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN RTRIM(pd.MaxMediaSize) + ' Network Scanner'
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                       WHEN 1 THEN 'Color'
                                       WHEN 0 THEN 'Mono'
                                       ELSE ''
                                   END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       dsji.[PageCount],
       dsji.ActivityStartDateTime,
       dsji.ScanStartDateTime,
       dsji.JobEndStatus,
       CASE
           WHEN dsjo.FileLocation IS NULL THEN 'None'
           ELSE CASE
                    WHEN dsji.ScanType = 'Folder Multi' THEN RIGHT(dsjo.FileLocation, 1)
                    ELSE '1'
                END
       END AS Destination,
       CASE
           WHEN dsji.FilePrefix IS NOT NULL THEN CASE
                                                     WHEN dsjo.[FileName] IS NOT NULL THEN CASE
                                                                                               WHEN (dsjo.[PageCount] - dsji.[PageCount]) != 0 THEN CASE
                                                                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN 'Page Delta'
                                                                                                                                                        ELSE 'FAIL'
                                                                                                                                                    END
                                                                                               ELSE CASE
                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN 'Pass'
                                                                                                        ELSE 'FAIL'
                                                                                                    END
                                                                                           END
                                                     ELSE CASE
                                                              WHEN CHARINDEX('Complete', ae.Status) > 0 THEN 'Missing'
                                                              ELSE 'N/A'
                                                          END
                                                 END
           ELSE 'Orphaned'
       END AS ValidationResult,
       dssj.CompletionStatus,
       dssj.DssVersion,
       CASE
           WHEN dsji.ScanEndDateTime > (CASE
                                            WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                            ELSE dsjo.FileSentDateTime
                                        END) THEN 0
           ELSE DATEDIFF(SECOND, dsji.ScanEndDateTime, CASE
                                                           WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                                           ELSE dsjo.FileSentDateTime
                                                       END) / 3600.0 / 24.0
       END AS scanEnd_2_fileSent,
       CASE
           WHEN dssj.CompletionDateTime < dsji.ActivityStartDateTime THEN 'No'
           ELSE 'Yes'
       END AS ValidDSSTime,
       CASE
           WHEN dssj.DeviceModel IS NULL THEN 'No'
           ELSE CASE
                    WHEN CHARINDEX(RIGHT(pd.Model, 4), dssj.DeviceModel) > 0 THEN 'No'
                    ELSE 'Yes'
                END
       END AS [Duplicate?],
       CASE
           WHEN CHARINDEX('SIM', dsji.DeviceId) > 0 THEN 1
           ELSE 0
       END AS [SIM?],
       FLOOR((DATEDIFF(SECOND, ss.StartDateTime, dsji.ActivityStartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.DigitalSendJobInput dsji
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN dbo.DigitalSendServerJob dssj ON dsjo.[FileName] = dssj.[FileName]
                                                   AND (dssj.JobType != 'SendFax' OR dssj.CompletionDateTime IS NULL OR dssj.CompletionDateTime > dsji.ActivityStartDateTime)
                                                   AND (CHARINDEX(RIGHT(pd.Model, 10), dssj.DeviceModel) > 0 OR dssj.DeviceModel IS NULL)
                                                   AND (dsjo.[PageCount] = dssj.ScannedPages OR dssj.ScannedPages IS NULL)
                                                   AND (DATEDIFF(SECOND, dsjo.FileReceivedDateTime, dssj.CompletionDateTime) < 3600 OR dssj.CompletionStatus IS NULL)
GO

IF OBJECT_ID('Reports.vw_rpt_ePrintReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ePrintReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_ePrintReportDetails]
END
GO
PRINT 'Creating vw_rpt_ePrintReportDetails View ...'
GO
--
-- This view is used in the 'ePrint Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_ePrintReportDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId AS SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.Status AS UpdateType,
       ae.UserName,
       ae.HostName,
       CASE SUBSTRING(pjc.ClientOS, CHARINDEX('.', pjc.ClientOS) - 1, 3)
           WHEN '6.3' THEN 'Windows 8.1'
           WHEN '6.2' THEN 'Windows 8'
           WHEN '6.1' THEN 'Windows 7'
           WHEN '6.0' THEN 'Windows Vista'
           WHEN '5.2' THEN 'Windows XP Professional x64'
           WHEN '5.1' THEN 'Windows XP'
           ELSE pjc.ClientOS
       END AS ClientOS,
       ae.ResultMessage,
       pjc.PrintJobClientId,
       pjc.[FileName],
       pjc.FileSizeBytes,
       CASE (SUBSTRING(pjc.[FileName], CHARINDEX('.', pjc.[FileName]) + 1, 3))
           WHEN 'PDF' THEN 'Acrobat'
           WHEN 'PPT' THEN 'PowerPoint'
           WHEN 'DOC' THEN 'Word'
           WHEN 'XLS' THEN 'Excel'
           WHEN 'txt' THEN 'NotePad'
           ELSE ''
       END AS FileType,
       pjc.PrintQueue,
       pjc.PrintType,
       pjc.JobStartDateTime AS ClientJobStart,
       pjc.DevicePlatform,
       ISNULL(aesu.ServerName, CASE
                                   WHEN pjc.PrintType = 'Local' THEN ae.HostName
                                   ELSE NULL
                               END) AS ActivityServer,
       epsj.EPrintJobId,
       epsj.JobName AS ePrintJobName,
       epsj.JobStatus,
       epsj.JobStartDateTime,
       epsj.LastStatusDateTime,
       epsj.EPrintTransactionId, 
       epsj.EmailAccount,
       epsj.EmailReceivedDateTime, 
       epsj.TransactionStatus,
       epsj.PrinterName as ePrintPrinterName,
       vpj.PjlLanguage,
       vpj.FirstByteReceivedDateTime,
       vpj.LastByteReceivedDateTime,
       vpj.BytesReceived
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON aesu.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT e1.EmailAccount,
                          e1.EmailReceivedDateTime,
                          e1.EPrintJobId,
                          e1.EPrintTransactionId,
                          e1.JobName,
                          e1.JobStartDateTime,
                          e1.JobStatus,
                          e1.LastStatusDateTime,
                          e1.PrinterName,
                          ISNULL(e1.PrintJobClientId, (SELECT e2.PrintJobClientId
                                                         FROM dbo.EPrintServerJob e2
                                                        WHERE e2.EPrintTransactionId = e1.EPrintTransactionId AND e2.PrintJobClientId IS NOT NULL AND e2.SessionId = e1.SessionId)) AS PrintJobClientId,
                          e1.SessionId,
                          e1.TransactionStatus
                     FROM dbo.EPrintServerJob e1) epsj ON pjc.PrintJobClientId = epsj.PrintJobClientId
                                                                         AND epsj.SessionId = ae.SessionId
  LEFT OUTER JOIN (  SELECT SUM(v.BytesReceived) AS BytesReceived,
                            MIN(v.FirstByteReceivedDateTime) AS FirstByteReceivedDateTime,
                            MAX(v.LastByteReceivedDateTime) AS LastByteReceivedDateTime,
                            v.PjlLanguage,
                            v.PrintJobClientId
                       FROM dbo.VirtualPrinterJob v
                   GROUP BY v.PrintJobClientId,
                            v.PjlLanguage) vpj ON pjc.PrintJobClientId = vpj.PrintJobClientId
 WHERE ae.ActivityType = 'ePrint'
GO

IF OBJECT_ID('Reports.vw_tbl_TempProduct_CTE', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempProduct_CTE View ...'
	DROP VIEW [Reports].[vw_tbl_TempProduct_CTE]
END
GO
PRINT 'Creating vw_tbl_TempProduct_CTE View ...'
GO
--
-- This view is used in the 'MOAT Job Report Summary' and 'MOAT Job Report Details' reports.
--
CREATE VIEW [Reports].[vw_tbl_TempProduct_CTE]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.ActivityExecutionId,
       aeau.AssetId AS DeviceId,
       sd.FirmwareRevision AS FirmwareVersion,
       sd.ProductName AS Product,
       ISNULL(sd.ProductName, CASE
                                WHEN CHARINDEX('eprint', ae.ActivityName) > 0 THEN LTRIM(REPLACE(REPLACE(ae.ActivityName, SUBSTRING(ae.ActivityName, CHARINDEX('LTL', ae.ActivityName), 10), ''), LEFT(ae.ActivityName, CHARINDEX('-', ae.ActivityName)), ''))
                                ELSE 'N/A'
                            END) AS RevProduct,
       dsji.Ocr,
       dsji.ScanType,
       dsji.ScanStartDateTime,
       dsji.ScanEndDateTime,
       ppjr.SolutionType,
       ISNULL(dsjo.FilePrefix, pjc.[FileName]) AS FilePrefix,
       ISNULL(dsjo.FileSizeBytes, pjc.FileSizeBytes) AS FileSizeBytes,
       dsjo.FileSentDateTime,
       dsjo.FileReceivedDateTime,
       CASE
           WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
           ELSE dsjo.FileSentDateTime
       END AS JobReceivedDateTime,
       dsji.[PageCount] AS PageCountIn,
       dsjo.[PageCount] AS PageCountOut,
       DATEDIFF(SECOND, dsji.[PageCount], dsjo.[PageCount]) / 3600.0 / 24.0 AS PageDelta,
       pjc.PrintJobClientId,
       pjc.PrintQueue,
       pjc.PrintType,
       CASE
           WHEN (ae.ActivityType = 'ePrint' OR ae.ActivityType = 'HPCR' OR CHARINDEX('Auth', ae.ActivityType) > 0) THEN ae.StartDateTime
           ELSE CASE
                    WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ppjr.JobStartDateTime
                    ELSE ISNULL(dsji.ScanStartDateTime, pjc.JobStartDateTime)
                END
       END AS StartDateTime,
       CASE
           WHEN (ae.ActivityType = 'ePrint' OR ae.ActivityType = 'HPCR' OR CHARINDEX('Auth', ae.ActivityType) > 0) THEN ae.EndDateTime
           ELSE CASE
                    WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ppjr.JobEndDateTime
                    ELSE ISNULL(dsji.ScanEndDateTime, pjc.JobEndDateTime)
                END
       END AS EndDateTime
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.DigitalSendJobInput dsji ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.PullPrintJobRetrieval ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_MOATActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivityDetails View ...'
	DROP VIEW [Reports].[vw_rpt_MOATActivityDetails]
END
GO
PRINT 'Creating vw_rpt_MOATActivityDetails View ...'
GO
--
-- This view is used in the 'MOAT Activity Details' report.
--
CREATE VIEW [Reports].[vw_rpt_MOATActivityDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SafeCom', ae.ActivityName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('AC', ae.ActivityName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', psj.PrintServer) > 0 THEN CASE
                                                                                                                               WHEN CHARINDEX('LTL', psj.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                               ELSE 'Printing-Citrix-Local'
                                                                                                                           END
                                                                        ELSE CASE
                                                                                 WHEN ae.HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN dsji.ScanType IS NOT NULL THEN CASE
                                                            WHEN dsji.Ocr = 1 THEN 'ScanTo' + dsji.ScanType + '_OCR'
                                                            ELSE 'ScanTo' + dsji.ScanType + '_Non-OCR'
                                                        END
                    ELSE ae.ActivityType
                END
       END AS ActivityType,
       ae.Status AS UpdateType,
       CASE
           WHEN CHARINDEX('eprint', ae.ActivityName) > 0 THEN CASE
                                                                  WHEN CHARINDEX('LTL', ae.ActivityName) = 0 THEN ''
                                                                  ELSE SUBSTRING(ae.ActivityName, CHARINDEX('LTL', ae.ActivityName), 9)
                                                              END
           ELSE ISNULL(dsji.DeviceId, aeau.AssetId)
       END AS DeviceId,
       ISNULL(sd.ProductName, CASE
                                WHEN CHARINDEX('eprint', ae.ActivityName) > 0 THEN LTRIM(REPLACE(REPLACE(ae.ActivityName, SUBSTRING(ae.ActivityName, CHARINDEX('LTL', ae.ActivityName), 10), ''), LEFT(ae.ActivityName, CHARINDEX('-', ae.ActivityName)), ''))
                                ELSE 'N/A'
                            END) AS RevProduct,
       sd.FirmwareRevision,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       COALESCE((pd.[Group] + ' ' + CASE pd.Color
                                        WHEN 1 THEN 'Color'
                                        WHEN 0 THEN 'Mono'
                                        ELSE ''
                                    END + ' ' + pd.MaxMediaSize), 'NA') AS DeviceCategory,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.DigitalSendJobInput dsji ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_MOATActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivitySummary View ...'
	DROP VIEW [Reports].[vw_rpt_MOATActivitySummary]
END
GO
PRINT 'Creating vw_rpt_MOATActivitySummary View ...'
GO
--
-- This view is used in the 'MOAT Activity Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_MOATActivitySummary]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SafeCom', ae.ActivityName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('AC', ae.ActivityName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', psj.PrintServer) > 0 THEN CASE
                                                                                                                               WHEN CHARINDEX('LTL', psj.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                               ELSE 'Printing-Citrix-Local'
                                                                                                                           END
                                                                        ELSE CASE
                                                                                 WHEN ae.HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN dsji.ScanType IS NOT NULL THEN CASE
                                                            WHEN dsji.Ocr = 1 THEN 'ScanTo' + dsji.ScanType + '_OCR'
                                                            ELSE 'ScanTo' + dsji.ScanType + '_Non-OCR'
                                                        END
                    ELSE ae.ActivityType
                END
       END AS ActivityType,
       ae.Status AS UpdateType,
       ISNULL(sd.ProductName, CASE
                                WHEN CHARINDEX('eprint', ae.ActivityName) > 0 THEN LTRIM(REPLACE(REPLACE(ae.ActivityName, SUBSTRING(ae.ActivityName, CHARINDEX('LTL', ae.ActivityName), 10), ''), LEFT(ae.ActivityName, CHARINDEX('-', ae.ActivityName)), ''))
                                ELSE 'N/A'
                            END) AS RevProduct,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.DigitalSendJobInput dsji ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_MOATJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATJobReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_MOATJobReportDetails]
END
GO
PRINT 'Creating vw_rpt_MOATJobReportDetails View ...'
GO
--
-- This view is used in the 'MOAT Job Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_MOATJobReportDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.StartDateTime AS ActivityStart,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
	   ae.ResultCategory AS GroupErrorMessage,
       ae.UserName,
       ae.HostName,
       aesu.ServerName,
       ae.ActivityName,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SAFE', aesu.ServerName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('HPAC', aesu.ServerName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX('LTL', pcte.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                              ELSE 'Printing-Citrix-Local'
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN CHARINDEX('an', ae.ActivityType) > 0 THEN 'Scanning'
                    ELSE CASE
                             WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ('Pulling-' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityGroup,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SAFE', aesu.ServerName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('HPAC', aesu.ServerName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX('LTL', pcte.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                              ELSE 'Printing-Citrix-Local'
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN pcte.ScanType IS NOT NULL THEN CASE pcte.OCR
                                                            WHEN 1 THEN CASE
                                                                            WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ' + '_OCR')
                                                                            ELSE ('ScanTo' + pcte.ScanType + '_OCR')
                                                                        END
                                                            WHEN 0 THEN CASE
                                                                            WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ' + '_Non-OCR')
                                                                            ELSE ('ScanTo' + pcte.ScanType + '_Non-OCR')
                                                                        END
                                                            ELSE CASE
                                                                     WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ')
                                                                     ELSE ('ScanTo' + pcte.ScanType)
                                                                 END
                                                        END
                    ELSE CASE
                             WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ('Pulling-' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityDetail,
       CASE
           WHEN CHARINDEX('HP Universal', psj.PrintDriver) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
           ELSE CASE
                    WHEN CHARINDEX('Safecom', pcte.PrintQueue) > 0 THEN pcte.PrintQueue
                    ELSE psj.PrintDriver
                END
       END AS PrintDriver,
       ISNULL(CASE
                  WHEN CHARINDEX('HP Universal', psj.PrintQueue) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintQueue, LEN(psj.PrintQueue) - 22))
                  ELSE psj.PrintQueue
              END, CASE
                       WHEN CHARINDEX('HP Universal', pcte.PrintQueue) > 0 THEN ('HP UPD ' + RIGHT(pcte.PrintQueue, LEN(pcte.PrintQueue) - 22))
                       ELSE pcte.PrintQueue
                   END) AS PrintQueue,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN (CHARINDEX('Citrix', ae.HostName) > 0 AND CHARINDEX('PrintServer', aesu.ServerName) > 0) THEN 'CitrixRemote'
                                                      ELSE pcte.PrintType
                                                  END
           ELSE 'N/A'
       END AS PrintQueueType,
       CASE
           WHEN CHARINDEX('eprint', ae.ActivityName) > 0 THEN SUBSTRING(ae.ActivityName, CHARINDEX('LTL', ae.ActivityName), 9)
           ELSE ISNULL(pcte.DeviceId, pcte.DeviceId)
       END AS DeviceID,
       pcte.RevProduct,
       pcte.FirmwareVersion,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                      WHEN 1 THEN 'Color'
                                      WHEN 0 THEN 'Mono'
                                      ELSE ''
                                   END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN 'Color'
           WHEN 0 THEN 'Mono'
           ELSE ''
       END AS DeviceColor,
       pd.MaxMediaSize,
       pd.[Function],
       pcte.FilePrefix,
       pcte.PageCountIn,
       pcte.FileSizeBytes,
       pcte.StartDateTime,
       pcte.EndDateTime,
       pcte.PageCountOut,
       pcte.PageDelta,
       pcte.JobReceivedDateTime,
       CASE
           WHEN pcte.JobReceivedDateTime < pcte.EndDateTime THEN NULL
           ELSE DATEDIFF(SECOND, pcte.EndDateTime, pcte.JobReceivedDateTime) / 3600.0 / 24.0
       END AS TransferTime,
       CASE
           WHEN CHARINDEX('an', ae.ActivityType) > 0 THEN DATEDIFF(SECOND, pcte.StartDateTime, pcte.JobReceivedDateTime) / 3600.0 / 24.0
           ELSE DATEDIFF(SECOND, pcte.StartDateTime, pcte.EndDateTime) / 3600.0 / 24.0
       END AS JobTime
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN Reports.vw_tbl_TempProduct_CTE pcte ON ae.ActivityExecutionId = pcte.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pcte.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pcte.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = pcte.RevProduct
GO

IF OBJECT_ID('Reports.vw_rpt_MOATJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATJobReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_MOATJobReportSummary]
END
GO
PRINT 'Creating vw_rpt_MOATJobReportSummary View ...'
GO
--
-- This view is used in the 'MOAT Job Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_MOATJobReportSummary]
AS
SELECT ae.SessionId,
       ae.StartDateTime AS ActivityStart,
       ae.Status AS UpdateType,
       aesu.ServerName,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SAFE', aesu.ServerName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('HPAC', aesu.ServerName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX('LTL', pcte.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                              ELSE 'Printing-Citrix-Local'
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN CHARINDEX('an', ae.ActivityType) > 0 THEN 'Scanning'
                    ELSE CASE
                             WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ('Pulling-' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityGroup,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN CHARINDEX('SAFE', aesu.ServerName) > 0 THEN 'Printing-SafeCom'
                                                      ELSE CASE
                                                               WHEN CHARINDEX('HPAC', aesu.ServerName) > 0 THEN 'Printing-HPAC'
                                                               ELSE CASE
                                                                        WHEN CHARINDEX('Citrix', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX('LTL', pcte.PrintQueue) > 0 THEN 'Printing-Citrix-Remote'
                                                                                                                              ELSE 'Printing-Citrix-Local'
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN 'Printing-Local'
                                                                                 ELSE 'Printing-Remote'
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN pcte.ScanType IS NOT NULL THEN CASE pcte.OCR
                                                            WHEN 1 THEN CASE
                                                                            WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ' + '_OCR')
                                                                            ELSE ('ScanTo' + pcte.ScanType + '_OCR')
                                                                        END
                                                            WHEN 0 THEN CASE
                                                                            WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ' + '_Non-OCR')
                                                                            ELSE ('ScanTo' + pcte.ScanType + '_Non-OCR')
                                                                        END
                                                            ELSE CASE
                                                                     WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN ('ScanTo' + pcte.ScanType + ' DFS ')
                                                                     ELSE ('ScanTo' + pcte.ScanType)
                                                                 END
                                                        END
                    ELSE CASE
                             WHEN CHARINDEX('pull', ae.ActivityName) > 0 THEN ('Pulling-' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityDetail,
       pcte.RevProduct,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                      WHEN 1 THEN 'Color'
                                      WHEN 0 THEN 'Mono'
                                      ELSE ''
                                   END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory,
       pcte.StartDateTime,
       CASE
           WHEN CHARINDEX('an', ae.ActivityType) > 0 THEN DATEDIFF(SECOND, pcte.StartDateTime, pcte.JobReceivedDateTime) / 3600.0 / 24.0
           ELSE DATEDIFF(SECOND, pcte.StartDateTime, pcte.EndDateTime) / 3600.0 / 24.0
       END AS JobTime
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN Reports.vw_tbl_TempProduct_CTE pcte ON ae.ActivityExecutionId = pcte.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pcte.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pcte.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = pcte.RevProduct
GO

IF OBJECT_ID('Reports.vw_rpt_PrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PrintJobReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_PrintJobReportDetails]
END
GO
PRINT 'Creating vw_rpt_PrintJobReportDetails View ...'
GO
--
-- This view is used in the 'Print Job Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_PrintJobReportDetails]
WITH SCHEMABINDING
AS
SELECT pjc.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.Status AS UpdateType,
       ae.UserName,
       ae.HostName,
       CASE SUBSTRING(pjc.ClientOS, CHARINDEX('.', pjc.ClientOS) - 1, 3)
           WHEN '6.3' THEN 'Windows 8.1'
           WHEN '6.2' THEN 'Windows 8'
           WHEN '6.1' THEN 'Windows 7'
           WHEN '6.0' THEN 'Windows Vista'
           WHEN '5.2' THEN 'Windows XP Professional x64'
           WHEN '5.1' THEN 'Windows XP'
           ELSE pjc.ClientOS
       END AS ClientOS,
       ae.ResultMessage AS ErrorMessage,
       pjc.[FileName],
       pjc.FileSizeBytes,
       CASE (SUBSTRING(pjc.[FileName], CHARINDEX('.', pjc.[FileName]) + 1, 3))
           WHEN 'PDF' THEN 'Acrobat'
           WHEN 'PPT' THEN 'PowerPoint'
           WHEN 'DOC' THEN 'Word'
           WHEN 'XLS' THEN 'Excel'
           WHEN 'txt' THEN 'NotePad'
           ELSE ''
       END AS FileType,
       CASE 
           WHEN CHARINDEX('HP Universal', psj.PrintDriver) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
           ELSE CASE
                    WHEN CHARINDEX('Safecom', pjc.PrintQueue) > 0 THEN pjc.PrintQueue
                    ELSE psj.PrintDriver
                END
       END AS PrintDriver,
       ISNULL(CASE
                  WHEN CHARINDEX('HP Universal', psj.PrintQueue) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintQueue, LEN(psj.PrintQueue) - 22))
                  ELSE psj.PrintQueue
              END, CASE
                       WHEN CHARINDEX('HP Universal', pjc.PrintQueue) > 0 THEN ('HP UPD ' + RIGHT(pjc.PrintQueue, LEN(pjc.PrintQueue) - 22))
                       ELSE pjc.PrintQueue
                   END) AS PrintQueue,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN (CHARINDEX('Citrix', ae.HostName) > 0 AND ae.HostName != aesu.ServerName) THEN 'CitrixRemote'
                                                      ELSE pjc.PrintType
                                                  END
           ELSE 'N/A'
       END AS PrintQueueType,
       LTRIM(STR(psj.NumberUp, 10)) + '-up ' + CASE psj.ColorMode
                                                   WHEN 'Color' THEN 'Color'
                                                   ELSE 'Mono'
                                               END + CASE 
                                                         WHEN CHARINDEX('Watermark', ISNULL(psj.PrintQueue, pjc.PrintQueue)) > 0 THEN ' WaterMark'
                                                         ELSE ' Default'
                                                     END AS QueueProperties,
       CASE 
           WHEN pjc.PrintType = 'Remote' THEN CASE psj.RenderOnClient
                                                  WHEN 0 THEN 'Server'
                                                  WHEN 1 THEN 'Client'
                                                  ELSE ''
                                              END
           ELSE CASE 
                    WHEN pjc.PrintType = 'Local' THEN 'Client'
                    ELSE ''
                END
       END AS RenderLoc,
       psj.Duplex,
       psj.Copies,
       pjc.JobStartDateTime AS ClientJobStart,
       pjc.PrintStartDateTime AS ClientPrintStart,
       pjc.JobEndDateTime AS ClientJobEnd,
       ISNULL(psj.PrintServer, ISNULL(aesu.ServerName, CASE
                                                           WHEN pjc.PrintType = 'Local' THEN ae.HostName
                                                           ELSE NULL
                                                       END)) AS ActivityServer,
       CASE SUBSTRING(psj.PrintServerOS, CHARINDEX('.', psj.PrintServerOS) - 1, 3)
           WHEN '6.3' THEN 'Windows Server 2012 R2'
           WHEN '6.2' THEN 'Windows Server 2012'
           WHEN '6.1' THEN 'Windows Server 2008 R2'
           WHEN '6.0' THEN 'Windows Server 2008'
           WHEN '5.2' THEN 'Windows Server 2003'
           ELSE psj.PrintServerOS
       END AS PrintServerOS,
       psj.SubmittedDateTime,
       psj.SpoolStartDateTime,
       psj.SpoolEndDateTime,
       psj.PrintStartDateTime,
       psj.PrintEndDateTime,
       psj.DataType,
       psj.PrintedPages,
       psj.PrintedBytes,
       pjc.DevicePlatform,
       pd.Product,
       pd.[Platform],
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN 'Color'
           WHEN 0 THEN 'Mono'
           ELSE ''
       END AS DeviceColor,
       vpj.PjlLanguage,
       vpj.FirstByteReceivedDateTime,
       vpj.LastByteReceivedDateTime,
       vpj.BytesReceived,
       CASE pjc.JobEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, pjc.JobStartDateTime, pjc.PrintStartDateTime) / 3600.0 / 24.0)
       END AS ClientTime,
       CASE psj.SpoolEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.SpoolStartDateTime, psj.SpoolEndDateTime) / 3600.0 / 24.0)
       END AS SpoolTime,
       CASE psj.PrintEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.PrintStartDateTime, psj.PrintEndDateTime) / 3600.0 / 24.0)
       END AS RenderTime
  FROM dbo.PrintJobClient pjc
  LEFT OUTER JOIN dbo.ActivityExecution ae ON pjc.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pjc.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN (  SELECT s.ColorMode,
                            s.Copies,
                            s.DataType,
                            s.Duplex,
                            s.NumberUp,
                            s.PrintDriver,
                            SUM(s.PrintedBytes) AS PrintedBytes,
                            SUM(s.PrintedPages) AS PrintedPages,
                            MAX(s.PrintEndDateTime) AS PrintEndDateTime,
                            s.PrintJobClientId,
                            s.PrintQueue,
                            s.PrintServer,
                            s.PrintServerOS,
                            MIN(s.PrintStartDateTime) AS PrintStartDateTime,
                            MAX(CAST(s.RenderOnClient AS tinyint)) AS RenderOnClient,
                            MAX(s.SpoolEndDateTime) AS SpoolEndDateTime,
                            MIN(s.SpoolStartDateTime) AS SpoolStartDateTime,
                            MIN(s.SubmittedDateTime) AS SubmittedDateTime
                       FROM dbo.PrintServerJob s
                   GROUP BY s.PrintJobClientId,
                            s.PrintServer,
                            s.PrintServerOS,
                            s.PrintQueue,
                            s.PrintDriver,
                            s.DataType,
                            s.ColorMode,
                            s.Copies,
                            s.Duplex,
                            s.NumberUp) psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON sd.ProductName = pd.Product
  LEFT OUTER JOIN (  SELECT SUM(v.BytesReceived) AS BytesReceived,
                            MIN(v.FirstByteReceivedDateTime) AS FirstByteReceivedDateTime,
                            MAX(v.LastByteReceivedDateTime) AS LastByteReceivedDateTime,
                            v.PjlLanguage,
                            v.PrintJobClientId
                       FROM dbo.VirtualPrinterJob v
                   GROUP BY v.PrintJobClientId,
                            v.PjlLanguage) vpj ON pjc.PrintJobClientId = vpj.PrintJobClientId
GO

IF OBJECT_ID('Reports.vw_rpt_PrintJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PrintJobReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_PrintJobReportSummary]
END
GO
PRINT 'Creating vw_rpt_PrintJobReportSummary View ...'
GO
--
-- This view is used in the 'Print Job Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_PrintJobReportSummary]
WITH SCHEMABINDING
AS
SELECT pjc.SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.Status AS UpdateType,
       pjc.[FileName],
       CASE (SUBSTRING(pjc.[FileName], CHARINDEX('.', pjc.[FileName]) + 1, 3))
           WHEN 'PDF' THEN 'Acrobat'
           WHEN 'PPT' THEN 'PowerPoint'
           WHEN 'DOC' THEN 'Word'
           WHEN 'XLS' THEN 'Excel'
           WHEN 'txt' THEN 'NotePad'
           ELSE ''
       END AS FileType,
       CASE 
           WHEN CHARINDEX('HP Universal', psj.PrintDriver) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
           ELSE CASE
                    WHEN CHARINDEX('Safecom', pjc.PrintQueue) > 0 THEN pjc.PrintQueue
                    ELSE psj.PrintDriver
                END
       END AS PrintDriver,
       CASE
           WHEN ae.ActivityType = 'Printing' THEN CASE
                                                      WHEN (CHARINDEX('Citrix', ae.HostName) > 0 AND ae.HostName != aesu.ServerName) THEN 'CitrixRemote'
                                                      ELSE pjc.PrintType
                                                  END
           ELSE 'N/A'
       END AS PrintQueueType,
       LTRIM(STR(psj.NumberUp, 10)) + '-up ' + CASE psj.ColorMode
                                                   WHEN 'Color' THEN 'Color'
                                                   ELSE 'Mono'
                                               END + CASE 
                                                         WHEN CHARINDEX('Watermark', ISNULL(psj.PrintQueue, pjc.PrintQueue)) > 0 THEN ' WaterMark'
                                                         ELSE ' Default'
                                                     END AS QueueProperties,
       ISNULL(psj.PrintServer, ISNULL(aesu.ServerName, CASE
                                                           WHEN pjc.PrintType = 'Local' THEN ae.HostName
                                                           ELSE NULL
                                                       END)) AS ActivityServer,
       psj.PrintedBytes,
       pjc.DevicePlatform,
       pd.Product,
       pd.[Platform],
       CASE pjc.JobEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, pjc.JobStartDateTime, pjc.PrintStartDateTime) / 3600.0 / 24.0)
       END AS ClientTime,
       CASE psj.SpoolEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.SpoolStartDateTime, psj.SpoolEndDateTime) / 3600.0 / 24.0)
       END AS SpoolTime,
       CASE psj.PrintEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.PrintStartDateTime, psj.PrintEndDateTime) / 3600.0 / 24.0)
       END AS RenderTime
  FROM dbo.PrintJobClient pjc
  LEFT OUTER JOIN dbo.ActivityExecution ae ON pjc.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pjc.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN (  SELECT s.ColorMode,
                            s.NumberUp,
                            s.PrintDriver,
                            SUM(s.PrintedBytes) AS PrintedBytes,
                            MAX(s.PrintEndDateTime) AS PrintEndDateTime,
                            s.PrintJobClientId,
                            s.PrintQueue,
                            s.PrintServer,
                            MIN(s.PrintStartDateTime) AS PrintStartDateTime,
                            MAX(s.SpoolEndDateTime) AS SpoolEndDateTime,
                            MIN(s.SpoolStartDateTime) AS SpoolStartDateTime
                       FROM dbo.PrintServerJob s
                   GROUP BY s.PrintJobClientId,
                            s.PrintServer,
                            s.PrintServerOS,
                            s.PrintQueue,
                            s.PrintDriver,
                            s.DataType,
                            s.ColorMode,
                            s.Copies,
                            s.Duplex,
                            s.NumberUp) psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON sd.ProductName = pd.Product
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_PullPrintActivityReportDetails]
END
GO
PRINT 'Creating vw_rpt_PullPrintActivityReportDetails View ...'
GO
--
-- This view is used in the 'Pull Print Activity Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_PullPrintActivityReportDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ISNULL(ppjr.JobStartDateTime, ae.StartDateTime) AS StartDateTime,
       ISNULL(ppjr.JobEndDateTime, ae.EndDateTime) AS EndDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ppjr.SolutionType,
       pjc.[FileName],
       ISNULL(aer.RetryCount,0) AS RetryCount,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       aeau.AssetId AS DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                       WHEN 1 THEN 'Color'
                                       WHEN 0 THEN 'Mono'
                                       ELSE ''
                                   END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) PPJRCount
					 FROM dbo.PullPrintJobRetrieval
					 GROUP BY ActivityExecutionId) ppjrcnt ON ppjrcnt.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.JobStartDateTime,
							tp.JobEndDateTime,
							tp.SolutionType,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                   FROM dbo.PullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = ISNULL(ppjrcnt.PPJRCount, 1)
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) PJCCount
					 FROM dbo.PrintJobClient
					 GROUP BY ActivityExecutionId) pjccnt ON pjccnt.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT	tc.ActivityExecutionId,
							tc.FileName,
							ROW_NUMBER() OVER (PARTITION BY tc.SessionId, tc.ActivityExecutionId ORDER BY tc.JobStartDateTime) AS RowNumber
                     FROM dbo.PrintJobClient tc) pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId AND pjc.RowNumber = ISNULL(pjccnt.PJCCount, 1)
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_PullPrintActivityReportSummary]
END
GO
PRINT 'Creating vw_rpt_PullPrintActivityReportSummary View ...'
GO
--
-- This view is used in the 'Pull Print Activity Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_PullPrintActivityReportSummary]
AS
SELECT ae.SessionId,
       ae.ActivityName,
       ISNULL(ppjr.JobStartDateTime, ae.StartDateTime) AS StartDateTime,
       ae.ActivityType,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       sd.ProductName AS Product,
       pd.[Platform]
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) PPJRCount
					 FROM dbo.PullPrintJobRetrieval
					 GROUP BY ActivityExecutionId) ppjrcnt ON ppjrcnt.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.JobStartDateTime,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                   FROM dbo.PullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = ISNULL(ppjrcnt.PPJRCount, 1)
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportDetails View ...'
	DROP VIEW [Reports].[vw_rpt_PullPrintJobReportDetails]
END
GO
PRINT 'Creating vw_rpt_PullPrintJobReportDetails View ...'
GO
--
-- This view is used in the 'Pull Print Job Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_PullPrintJobReportDetails]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityExecutionId,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour,
       ae.StartDateTime,
       ae.UserName,
       ae.ActivityName,
       ISNULL(ppjr.SolutionType, ae.ActivityType) AS SolutionType,
       CASE ae.ActivityType
           WHEN 'Printing' THEN 'Printing'
           WHEN 'Authentication' THEN 'AuthOnly'
           ELSE 'Pulling'
       END AS ActivityGroup,
       REPLACE(ae.Status, 'Activity', '') AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ae.HostName AS ClientName,
       CASE SUBSTRING(pjc.ClientOS, CHARINDEX('.', pjc.ClientOS) - 1, 3)
           WHEN '6.3' THEN 'Windows 8.1'
           WHEN '6.2' THEN 'Windows 8'
           WHEN '6.1' THEN 'Windows 7'
           WHEN '6.0' THEN 'Windows Vista'
           WHEN '5.2' THEN 'Windows XP Professional x64'
           WHEN '5.1' THEN 'Windows XP'
           ELSE pjc.ClientOS
       END AS ClientOS,
       pjc.[FileName],
       pjc.FileSizeBytes,
       CASE (SUBSTRING(pjc.[FileName], CHARINDEX('.', pjc.[FileName]) + 1, 3))
           WHEN 'PDF' THEN 'Acrobat'
           WHEN 'PPT' THEN 'PowerPoint'
           WHEN 'DOC' THEN 'Word'
           WHEN 'XLS' THEN 'Excel'
           WHEN 'txt' THEN 'NotePad'
           ELSE ''
       END AS FileType,
       CASE
           WHEN CHARINDEX('HP Universal', psj.PrintDriver) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
           ELSE CASE
                    WHEN CHARINDEX('Safecom', pjc.PrintQueue) > 0 THEN pjc.PrintQueue
                    ELSE psj.PrintDriver
                END
       END AS PrintDriver,
       CASE SUBSTRING(psj.PrintServerOS, CHARINDEX('.', psj.PrintServerOS) - 1, 3)
           WHEN '6.3' THEN 'Windows Server 2012 R2'
           WHEN '6.2' THEN 'Windows Server 2012'
           WHEN '6.1' THEN 'Windows Server 2008 R2'
           WHEN '6.0' THEN 'Windows Server 2008'
           WHEN '5.2' THEN 'Windows Server 2003'
           ELSE psj.PrintServerOS
       END AS PrintServerOS,
       pjc.PrintType AS PrintQueueType,
       pjc.PrintQueue,
       psj.PrintedPages,
       psj.PrintedBytes,
       CASE pjc.JobEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, pjc.PrintStartDateTime, pjc.JobEndDateTime) / 3600.0 / 24.0)
       END AS ClientTime,
       CASE psj.SpoolEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.SpoolStartDateTime, psj.SpoolEndDateTime) / 3600.0 / 24.0)
       END AS SpoolTime,
       CASE psj.PrintEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, psj.PrintStartDateTime, psj.PrintEndDateTime) / 3600.0 / 24.0)
       END AS RenderTime,
       CASE psj.PrintEndDateTime
           WHEN '' THEN ''
           ELSE (DATEDIFF(SECOND, pjc.PrintStartDateTime, psj.PrintEndDateTime) / 3600.0 / 24.0)
       END AS TotalPrintTime,
       pdj.JobEndStatus,
       ISNULL(aer.RetryCount,0) AS RetryCount,
       aeau.AssetId AS DeviceId,
       sd.FirmwareRevision AS FirmwareVersion,
       CASE
           WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
           ELSE sd.ProductName
       END AS Product,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
           ELSE pd.[Group] + ' ' + CASE pd.Color
                                       WHEN 1 THEN 'Color'
                                       WHEN 0 THEN 'Mono'
                                       ELSE ''
                                    END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN 'Color'
           WHEN 0 THEN 'Mono'
           ELSE ''
       END AS DeviceColor,
       pd.MaxMediaSize,
       pd.[Function]
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
  LEFT OUTER JOIN (  SELECT s.PrintDriver,
                            SUM(s.PrintedBytes) AS PrintedBytes,
                            SUM(s.PrintedPages) AS PrintedPages,
                            MAX(s.PrintEndDateTime) AS PrintEndDateTime,
                            s.PrintJobClientId,
                            s.PrintServerOS,
                            MIN(s.PrintStartDateTime) AS PrintStartDateTime,
                            MAX(s.SpoolEndDateTime) AS SpoolEndDateTime,
                            MIN(s.SpoolStartDateTime) AS SpoolStartDateTime
                       FROM dbo.PrintServerJob s
                   GROUP BY s.PrintJobClientId,
                            s.PrintServer,
                            s.PrintServerOS,
                            s.PrintQueue,
                            s.PrintDriver,
                            s.DataType) psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) PPJRCount
					 FROM dbo.PullPrintJobRetrieval
					 GROUP BY ActivityExecutionId) ppjrcnt ON ppjrcnt.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.SolutionType,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                   FROM dbo.PullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = ISNULL(ppjrcnt.PPJRCount, 1)
  LEFT OUTER JOIN dbo.PhysicalDeviceJob pdj ON pdj.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportSummary View ...'
	DROP VIEW [Reports].[vw_rpt_PullPrintJobReportSummary]
END
GO
PRINT 'Creating vw_rpt_PullPrintJobReportSummary View ...'
GO
--
-- This view is used in the 'Pull Print Job Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_PullPrintJobReportSummary]
WITH SCHEMABINDING
AS
SELECT ae.SessionId,
       ae.ActivityExecutionId,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour,
       ae.StartDateTime,
       ae.UserName,
       CASE ae.ActivityType
           WHEN 'Printing' THEN 'Printing'
           ELSE 'Pulling'
       END AS ActivityGroup,
       ae.Status AS UpdateType,
       ae.ResultCategory AS GroupErrorMessage,
       psj.PrintedPages,
       psj.PrintedBytes,
      CASE psj.PrintEndDateTime
          WHEN '' THEN ''
          ELSE (DATEDIFF(s, psj.PrintStartDateTime, psj.PrintEndDateTime) / 3600.0 / 24.0)
      END AS RenderTime,
      ISNULL(aer.RetryCount,0) AS RetryCount,
      aeau.AssetId AS DeviceId,
      CASE
          WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
          ELSE sd.ProductName
      END AS Product
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
  LEFT OUTER JOIN (  SELECT SUM(s.PrintedBytes) AS PrintedBytes,
                            SUM(s.PrintedPages) AS PrintedPages,
                            MAX(s.PrintEndDateTime) AS PrintEndDateTime,
                            s.PrintJobClientId,
                            MIN(s.PrintStartDateTime) AS PrintStartDateTime
                       FROM dbo.PrintServerJob s
                   GROUP BY s.PrintJobClientId,
                            s.PrintServer,
                            s.PrintServerOS,
                            s.PrintQueue,
                            s.PrintDriver,
                            s.DataType) psj ON pjc.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN (SELECT ActivityExecutionId,
						  MIN(AssetId) AssetId
					 FROM dbo.ActivityExecutionAssetUsage
					 GROUP BY ActivityExecutionId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN (SELECT ActivityExecutionId , COUNT(*) RetryCount
					 FROM dbo.ActivityExecutionRetries
					 GROUP BY ActivityExecutionId) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
GO

PRINT '********************************************************************'
PRINT '***  Update Stored Procedures compatible with Schema Changes ... ***'
PRINT '********************************************************************'

IF OBJECT_ID('dbo.sel_Chart_ActivityErrorsPerMinute', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityErrorsPerMinute Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorsPerMinute]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityErrorsPerMinute Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker error messages per minute
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorsPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ErrorMessage as ErrorMessage,
		owa.UpdateType,
		COUNT(owa.ErrorMessage) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
	(
		SELECT 
			i.ResultCategory AS ErrorMessage,
			i.Status AS UpdateType,
			COUNT(i.ResultCategory) AS InnerCount,
			i.EndDateTime
		FROM
			ActivityExecution i
		WHERE
			i.SessionId = @sessionId
			and ResultMessage IS NOT NULL
			and (@time = 0 or (@time > 0 and i.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))			
		GROUP BY
			i.ResultCategory,
			i.Status,
			i.EndDateTime
	) owa

	GROUP BY
		owa.ErrorMessage,
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityErrorTotalsDetails', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityErrorTotalsDetails Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityErrorTotalsDetails Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a summary of error messages starting with a specific prefix
-- Edited:		11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails] 
	@sessionId nvarchar(50) = 0,
	@shortError nvarchar(255) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ResultMessage AS ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ResultCategory = @shortError
	GROUP BY
		owa.ResultMessage
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityErrorTotals', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityErrorTotals Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorTotals]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityErrorTotals Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker errors grouped by error message.
-- Edited:		11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotals]
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		i.ErrorMessage,
		i.UpdateType,
		SUM(i.InnerCount) as Count
	FROM 
	(
		SELECT 
			owa.ResultCategory as ErrorMessage,
			owa.Status as UpdateType,
			COUNT(*) as InnerCount
		FROM
			ActivityExecution owa
		WHERE
			owa.SessionId = @sessionId
			and ResultMessage IS NOT NULL
		GROUP BY
			owa.ResultCategory,
			owa.Status
	) i
	GROUP BY
		i.ErrorMessage,
		i.UpdateType
	ORDER BY
		i.UpdateType
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityInstancesPerMinute', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityInstancesPerMinute Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityInstancesPerMinute]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityInstancesPerMinute Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by instance.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstancesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityName as Description,
		owa.ActivityType,
		owa.Status as UpdateType,
		COUNT(owa.ActivityType) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))
	GROUP BY
		owa.ActivityName,
		owa.ActivityType,
		owa.Status,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityInstanceTotalsErrors', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityInstanceTotalsErrors Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotalsErrors]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityInstanceTotalsErrors Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for an activity/updatetype pair
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@description nvarchar(50) = 0,
	@updateType nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ResultMessage as ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ActivityName = @description
		and owa.Status = @updateType
	GROUP BY
		owa.ResultMessage
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityInstanceTotals', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityInstanceTotals Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotals]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityInstanceTotals Stored Procedure ...'
GO
-- ==========================================================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata instance.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- ==========================================================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityName as Description,
		owa.ActivityType,
		owa.Status as UpdateType,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		owa.ActivityName,
		owa.ActivityType,
		owa.Status
	ORDER BY
		owa.ActivityName desc,
		owa.Status desc
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityTypesPerMinute', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityTypesPerMinute Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityTypesPerMinute]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityTypesPerMinute Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by metadata type.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityType,
		owa.Status AS UpdateType,
		COUNT(owa.ActivityType) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))
	GROUP BY
		owa.ActivityType,
		owa.Status,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityTypeTotalsErrors', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityTypeTotalsErrors Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityTypeTotalsErrors Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for a metadatatype/updatetype pair
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@activityType nvarchar(50) = 0,
	@updateType nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ResultMessage AS ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ActivityType = @activityType
		and owa.Status = @updateType
	GROUP BY
		owa.ResultMessage
END
GO

IF OBJECT_ID('dbo.sel_Chart_ActivityTypeTotals', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_ActivityTypeTotals Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_ActivityTypeTotals]
END
GO
PRINT 'Creating dbo.sel_Chart_ActivityTypeTotals Stored Procedure ...'
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata type.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityType,
		owa.Status AS UpdateType,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		owa.ActivityType,
		owa.Status
	ORDER BY
		owa.ActivityType desc,
		owa.Status desc
END
GO

IF OBJECT_ID('dbo.sel_Chart_TaskTotals', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sel_Chart_TaskTotals Stored Procedure  ...'
	DROP PROCEDURE [dbo].[sel_Chart_TaskTotals]
END
GO
PRINT 'Creating dbo.sel_Chart_TaskTotals Stored Procedure ...'
GO
CREATE PROCEDURE [dbo].[sel_Chart_TaskTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ate.TaskName,
		ate.ExecutionPath,
		ate.Status,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	JOIN ActivityTaskExecution ate on owa.ActivityExecutionId = ate.ActivityExecutionId
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		ate.TaskName,
		ate.ExecutionPath,		
		ate.[Status]
	ORDER BY
		ate.TaskName,
		ate.ExecutionPath, 
		ate.[Status]
END
GO

PRINT '*********************************************'
PRINT '***  Delete DirectorySnapshot Old Records ***'
PRINT '*********************************************'

IF OBJECT_ID('dbo.DirectorySnapshot', 'U') IS NOT NULL
BEGIN
	PRINT 'Deleting Older records from DirectorySnapshot Table ...'
	DELETE FROM [dbo].[DirectorySnapshot]
	 WHERE SnapshotDateTime < (SELECT MIN(StartDateTime) FROM [dbo].[SessionSummary])
END
GO

PRINT '************************************'
PRINT '***  Deleting Deprecated Tables  ***'
PRINT '************************************'

IF OBJECT_ID('dbo.STFAssetUtilization', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping STFAssetUtilization Table ...'
	DROP Table [dbo].[STFAssetUtilization]
END
GO

IF OBJECT_ID('dbo.STFClientUtilization', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping STFClientUtilization Table ...'
	DROP Table [dbo].[STFClientUtilization]
END
GO

IF OBJECT_ID('dbo.STFSessionUtilization', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping STFSessionUtilization Table ...'
	DROP Table [dbo].[STFSessionUtilization]
END
GO

IF OBJECT_ID('dbo.PinPrintJobRetrieval', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping PinPrintJobRetrieval Table ...'
	DROP Table [dbo].[PinPrintJobRetrieval]
END
GO

IF OBJECT_ID('dbo.RemoteQueueInstall', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping RemoteQueueInstall Table ...'
	DROP Table [dbo].[RemoteQueueInstall]
END
GO



