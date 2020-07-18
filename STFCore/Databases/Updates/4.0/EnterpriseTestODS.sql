USE [EnterpriseTestODS]
GO

-- 2/5/2016 Fdelagarza Migration script to New STF 4.0 base Schema including Data Migration and Views Uppdate

PRINT '******************************************************************'
PRINT '***  Drop Views with Dependencies on all the Schema Changes... ***'
PRINT '******************************************************************'
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByDay', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByDay...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByDay]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByDay_SessionId', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByDay_SessionId...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByDay_SessionId]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByDay_SessionId_ActivityName', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByDay_SessionId_ActivityName...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByDay_SessionId_ActivityName]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByHour', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByHour...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByHour]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByHour_SessionId', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByHour_SessionId...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByHour_SessionId]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByHour_SessionId_ActivityName', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByHour_SessionId_ActivityName...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByHour_SessionId_ActivityName]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByMonth', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByMonth...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByMonth]
END
GO
IF OBJECT_ID('Reports.vw_rpt_ActivityExecutionByWeek', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityExecutionByWeek...'
	DROP VIEW [Reports].[vw_rpt_ActivityExecutionByWeek]
END
GO
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
IF OBJECT_ID('Reports.vw_tbl_TempActivityExecutionDeviceUsage', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempActivityExecutionDeviceUsage...'
	DROP VIEW [Reports].[vw_tbl_TempActivityExecutionDeviceUsage]
END
GO
IF OBJECT_ID('Reports.vw_tbl_TempPrintJobClient', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempPrintJobClient...'
	DROP VIEW [Reports].[vw_tbl_TempPrintJobClient]
END
GO
IF OBJECT_ID('Reports.vw_tbl_TempProduct_CTE', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempProduct_CTE...'
	DROP VIEW [Reports].[vw_tbl_TempProduct_CTE]
END
GO
IF OBJECT_ID('Reports.vw_tbl_TempPullPrintJobRetrieval', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempPullPrintJobRetrieval...'
	DROP VIEW [Reports].[vw_tbl_TempPullPrintJobRetrieval]
END
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

PRINT '***********************************************************************'
PRINT '***  Create new ODS ETL Metadata configuration Stored Procedures... ***'
PRINT '***********************************************************************'

IF OBJECT_ID('dbo.setETLTable', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.setETLTable Stored Procedure...'
	DROP PROCEDURE [dbo].[setETLTable]
END
GO

PRINT 'Creating dbo.setETLTable Stored Procedure...'
GO

CREATE PROCEDURE [dbo].[setETLTable]
	@ETLTableId int,
	@DB varchar(100) = NULL,
	@SrcTable varchar(100) = NULL,
	@TargTable varchar(100) = NULL,
	@SrcKey varchar(100) = NULL,
	@TargKey varchar(100) = NULL,
	@TableType varchar(100) = NULL,
	@Op varchar(10) = 'Insert',  -- Insert is Default Operation also supports Move and Reset 
	@ToETLTableId int = NULL 
AS
BEGIN
	DECLARE @ValError bit
	SET @ValError = 0
	
	IF @SrcTable IS NOT NULL AND @SrcKey IS NULL SET @SrcKey = @SrcTable+'Id'
	IF @TargTable IS NULL AND @SrcTable IS NOT NULL SET @TargTable = @SrcTable
	IF @TargKey IS NULL AND @TargTable IS NOT NULL SET @TargKey = @TargTable+'Id'
	
	IF @Op = 'Insert'
	BEGIN
		IF @ETLTableId IS NULL BEGIN PRINT 'ERROR Insert Op: @ETLTableId is required.' SET @ValError = 1 END
		IF @DB IS NULL SET @DB = 'ScalableTestDataLog'
		IF @DB NOT IN('ScalableTestDataLog','EnterpriseTest') BEGIN
			PRINT 'ERROR Insert Op: @DB='+@DB+' is invalid.' SET @ValError = 1 END
		IF @SrcTable IS NULL BEGIN PRINT 'ERROR Insert Op: @SrcTable is required.' SET @ValError = 1 END
		IF @SrcKey IS NULL BEGIN PRINT 'ERROR Insert Op: @SrcKey is required.' SET @ValError = 1 END
		IF @TargTable IS NULL BEGIN PRINT 'ERROR Insert Op: @TargTable is required.' SET @ValError = 1 END
		IF @TargKey IS NULL BEGIN PRINT 'ERROR Insert Op: @TargKey is required.' SET @ValError = 1 END
		IF @TableType IS NULL BEGIN PRINT 'ERROR Insert Op: @TableType is required.' SET @ValError = 1 END
		IF EXISTS(SELECT * FROM dbo.ETLTable WHERE ETLTableId=@ETLTableId) BEGIN
			PRINT 'ERROR Insert Operation: A Record with @ETLTableId='+CONVERT(varchar, @ETLTableId)+' already exists.' SET @ValError = 1 END
		
		IF @ValError=1 RETURN(1) --Return with error code 1 if did not passed Insert Op Validation
		
		PRINT 'Inserting @ETLTableId='+CONVERT(varchar, @ETLTableId);
		
		INSERT INTO [dbo].[ETLTable]
				   ([ETLTableId]
				   ,[SourceDatabase]
				   ,[SourceTableName]
				   ,[TargetTableName]
				   ,[ValTableName]
				   ,[SourceKeyCol1Name]
				   ,[SourceKeyCol2Name]
				   ,[TargetKeyColName]
				   ,[TableType]
				   ,[TableProcessingGroup]
				   ,[TargetTableTotalRows]
				   ,[ValTableTotalRows]
				   ,[SelectMode]
				   ,[CTEnabled]
				   ,[LastLodadedCTVersion]
				   ,[LastMinValidCTVersion]
				   ,[LastUpdateTime])
			 VALUES
				   (@ETLTableId
				   ,@DB
				   ,@SrcTable
				   ,@TargTable
				   ,@TargTable+'Val'
				   ,@SrcKey
				   ,''
				   ,@TargKey
				   ,@TableType
				   ,'ODSLOAD'
				   ,NULL
				   ,NULL
				   ,'CHTRACK'
				   ,1
				   ,NULL
				   ,NULL
				   ,NULL)
		RETURN;
	END
	
	IF @Op = 'Move'
	BEGIN
		IF @ETLTableId IS NULL BEGIN PRINT 'ERROR Move Op: @ETLTableId is required.' SET @ValError = 1 END;
		IF @ToETLTableId IS NULL AND @ETLTableId IS NOT NULL BEGIN SET @ToETLTableId = @ETLTableId END ;
		IF @ToETLTableId IS NULL BEGIN PRINT 'ERROR Move Op: @ToETLTableId is required.' SET @ValError = 1 END;
		IF @DB IS NOT NULL AND @DB NOT IN('ScalableTestDataLog','EnterpriseTest') BEGIN
			PRINT 'ERROR Move Op: @DB='+@DB+' is invalid.' SET @ValError = 1 END;
		IF @SrcTable IS NOT NULL AND @SrcKey IS NULL BEGIN PRINT 'ERROR Insert Op: @SrcKey is required.' SET @ValError = 1 END;
		IF @TargTable IS NOT NULL AND @TargKey IS NULL BEGIN PRINT 'ERROR Insert Op: @TargKey is required.' SET @ValError = 1 END;
		IF NOT EXISTS(SELECT * FROM dbo.ETLTable WHERE ETLTableId = @ETLTableId) BEGIN
			PRINT 'ERROR Move Operation: A Record with @ETLTableId='+CONVERT(varchar, @ETLTableId)+' does not exist.' SET @ValError = 1 END;
		IF EXISTS(SELECT * FROM dbo.ETLTable WHERE ETLTableId != @ETLTableId AND ETLTableId = @ToETLTableId ) BEGIN
			PRINT 'ERROR Move Operation: A Record with target @ToETLTableId='+CONVERT(varchar, @ToETLTableId)+' already exists.' SET @ValError = 1 END;
		
		IF @ValError=1 RETURN(1); --Return with error code 1 if did not passed Move Op Validation
		
		PRINT 'Moving @ETLTableId='+CONVERT(varchar, @ETLTableId)+' to @ToETLTableId='+CONVERT(varchar, @ToETLTableId);
		
		UPDATE [dbo].[ETLTable]
		   SET [ETLTableId] = @ToETLTableId
		 WHERE ETLTableId = @ETLTableId;

		IF @DB IS NOT NULL
			UPDATE [dbo].[ETLTable] SET [SourceDatabase] = @DB WHERE ETLTableId = @ToETLTableId;

		IF @SrcTable IS NOT NULL
			UPDATE [dbo].[ETLTable]
			   SET [SourceTableName] = @SrcTable
				  ,[SourceKeyCol1Name] = @SrcKey
				  ,[SourceKeyCol2Name] = ''
			 WHERE ETLTableId = @ToETLTableId;

		IF @TargTable IS NOT NULL
			 UPDATE [dbo].[ETLTable]
			   SET [TargetTableName] = @TargTable
				  ,[ValTableName] = @TargTable+'Val'
				  ,[TargetKeyColName] = @TargKey
			 WHERE ETLTableId = @ToETLTableId;
		
		IF @TableType IS NOT NULL
			UPDATE [dbo].[ETLTable] SET [TableType] = @TableType WHERE ETLTableId = @ToETLTableId;
	
		RETURN;
	END
	
	IF @Op = 'Reset'
	BEGIN
		IF @ETLTableId IS NULL BEGIN PRINT 'ERROR Move Op: @ETLTableId is required.' SET @ValError = 1 END
		IF NOT EXISTS(SELECT * FROM dbo.ETLTable WHERE ETLTableId = @ETLTableId) BEGIN
			PRINT 'ERROR Move Operation: A Record with @ETLTableId='+CONVERT(varchar, @ETLTableId)+' does not exist.' SET @ValError = 1 END
		
		IF @ValError=1 RETURN(1) --Return with error code 1 if did not passed Reset Op Validation
		
		PRINT 'Reseting @ETLTableId='+CONVERT(varchar, @ETLTableId);
		
		UPDATE [dbo].[ETLTable]
		   SET [TargetTableTotalRows] = NULL
			  ,[ValTableTotalRows] = NULL
			  ,[SelectMode] = NULL
			  ,[LastLodadedCTVersion] = NULL
			  ,[LastMinValidCTVersion] = NULL
			  ,[LastUpdateTime] = NULL
		 WHERE ETLTableId = @ETLTableId

		RETURN;
	END

	PRINT ' ERROR: Operation not valid @Op=' + @Op
	RETURN (100) 
END
GO

IF OBJECT_ID('dbo.setETLJob', 'P') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.setETLJob Stored Procedure...'
	DROP PROCEDURE [dbo].[setETLJob]
END
GO

PRINT 'Creating dbo.setETLJob Stored Procedure...'
GO

CREATE PROCEDURE [dbo].[setETLJob]
	@ETLJobId int,
	@JobName varchar(250)= NULL,
	@JobType varchar(20) = NULL,
	@Op varchar(10) = 'Insert',  -- Insert is Default Operation also supports Move and Delete
	@ToETLJobId int = NULL
AS
BEGIN
	DECLARE @ValError bit
	SET @ValError = 0
	
	IF @Op = 'Insert'
	BEGIN
		IF @JobType IS NULL SET @JobType = 'ETL'
		IF @ETLJobId IS NULL BEGIN PRINT 'ERROR Insert Op: @ETLJobId is required.' SET @ValError = 1 END
		IF @JobName IS NULL BEGIN PRINT 'ERROR Insert Op: @JobName is required.' SET @ValError = 1 END
		IF EXISTS(SELECT * FROM dbo.ETLJob WHERE ETLJobId = @ETLJobId) BEGIN
			PRINT 'ERROR Insert Operation: A Record with @ETLJobId='+CONVERT(varchar, @ETLJobId)+' already exists.' SET @ValError = 1 END
		
		IF @ValError=1 RETURN(1) --Return with error code 1 if did not passed Insert Op Validation
		
		PRINT 'Inserting @ETLJobId='+CONVERT(varchar, @ETLJobId);
		
		INSERT INTO [dbo].[ETLJob]
				   ([ETLJobId]
				   ,[JobName]
				   ,[JobType])
			 VALUES
				   (@ETLJobId
				   ,@JobName
				   ,@JobType)
		RETURN;
	END
	
	IF @Op = 'Move'
	BEGIN
		IF @ETLJobId IS NULL BEGIN PRINT 'ERROR Move Op: @ETLJobId is required.' SET @ValError = 1 END;
		IF @ToETLJobId IS NULL AND @ETLJobId IS NOT NULL BEGIN SET @ToETLJobId = @ETLJobId END;
		IF @ToETLJobId IS NULL BEGIN PRINT 'ERROR Move Op: @ToETLJobId is required.' SET @ValError = 1 END;
	
		IF NOT EXISTS(SELECT * FROM dbo.ETLJob WHERE ETLJobId = @ETLJobId) BEGIN
			PRINT 'ERROR Move Operation: A Record with @ETLJobId='+CONVERT(varchar, @ETLJobId)+' does not exist.' SET @ValError = 1 END;
		IF EXISTS(SELECT * FROM dbo.ETLJob WHERE ETLJobId != @ETLJobId AND ETLJobId = @ToETLJobId) BEGIN
			PRINT 'ERROR Move Operation: A Record with target @ToETLJobId='+CONVERT(varchar, @ToETLJobId)+' already exists.' SET @ValError = 1 END;
		
		IF @ValError=1 RETURN(1) --Return with error code 1 if did not passed Move Op Validation
		
		PRINT 'Moving @ETLJobId='+CONVERT(varchar, @ETLJobId)+' to @ToETLJobId='+CONVERT(varchar, @ToETLJobId);
		
		ALTER TABLE [dbo].[ETLRunTableSummary] DROP CONSTRAINT [Fk_ETLRunTableSummary_ETLJob];
		
		UPDATE [dbo].[ETLJob] SET [ETLJobId] = @ToETLJobId WHERE ETLJobId = @ETLJobId;
		
		IF @JobName IS NOT NULL
			UPDATE [dbo].[ETLJob] SET [JobName] = @JobName WHERE ETLJobId = @ToETLJobId;
		
		IF @JobType IS NOT NULL
			UPDATE [dbo].[ETLJob] SET [JobType] = @JobType WHERE ETLJobId = @ToETLJobId;
		
		UPDATE [dbo].[ETLRunTableSummary] SET [ETLRunTableJobId] = @ToETLJobId WHERE ETLRunTableJobId = @ETLJobId;
		
		ALTER TABLE [dbo].[ETLRunTableSummary]  WITH CHECK ADD  CONSTRAINT [Fk_ETLRunTableSummary_ETLJob] FOREIGN KEY([ETLRunTableJobId])
			REFERENCES [dbo].[ETLJob] ([ETLJobId]);

		ALTER TABLE [dbo].[ETLRunTableSummary] CHECK CONSTRAINT [Fk_ETLRunTableSummary_ETLJob];
	
		RETURN;
	END
	
	IF @Op = 'Delete' -- May not work due to referential integrity, in meantime move job to be deleted to unused number.
	BEGIN
		IF @ETLJobId IS NULL BEGIN PRINT 'ERROR Delete Op: @ETLJobId is required.' SET @ValError = 1 END
	
		IF NOT EXISTS(SELECT * FROM dbo.ETLJob WHERE ETLJobId=@ETLJobId) BEGIN
			PRINT 'ERROR Delete Operation: A Record with @ETLJobId='+CONVERT(varchar, @ETLJobId)+' does not exist.' SET @ValError = 1 END
		
		IF @ValError=1 RETURN(1) --Return with error code 1 if did not passed Delete Op Validation
		
		PRINT 'Deleting @ETLJobId='+CONVERT(varchar, @ETLJobId);
		
		DELETE FROM [dbo].[ETLJob] WHERE ETLJobId=@ETLJobId;
	
		RETURN;
	END
	
	PRINT ' ERROR: Operation not valid @Op=' + @Op
	RETURN (100) 
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'Idx_ETLRunTableSummary_ETLJobId')
BEGIN
    PRINT 'Creating Idx_ETLRunTableSummary_ETLJobId index... '
	CREATE NONCLUSTERED INDEX [Idx_ETLRunTableSummary_ETLJobId] ON [dbo].[ETLRunTableSummary]
	([ETLRunTableJobId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF COL_LENGTH ( 'dbo.ActivityExecution' , 'UpdateType' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecution' , 'Status' ) IS NULL
BEGIN
	PRINT '***************************************************'
	PRINT '***  ActivityExecution Column Name changes ...  ***'
	PRINT '***************************************************'
	EXEC sp_rename 'ActivityExecution.UpdateType', 'Status', 'COLUMN';
	PRINT 'ActivityExecution UpdateType renamed to Status'
END
ELSE
	PRINT 'ActivityExecution Status Column already exists Or Column UpdateType not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecution' , 'ErrorMessage' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecution' , 'ResultMessage' ) IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecution.ErrorMessage', 'ResultMessage', 'COLUMN';
	PRINT 'ActivityExecution ErrorMessage renamed to ResultMessage'
END
ELSE
	PRINT 'ActivityExecution ResultMessage Column already exists Or Column ErrorMessage not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecution' , 'Label' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecution' , 'ResultCategory' ) IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecution.Label', 'ResultCategory', 'COLUMN';
	PRINT 'ActivityExecution Label renamed to ResultCategory'
END
ELSE
	PRINT 'ActivityExecution ResultCategory Column already exists Or Column Label not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecution' , 'ResultCategory' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecution' , 'ResultMessage' ) IS NOT NULL
BEGIN
	PRINT 'Initializing ResultCategory Null Values from ResultMessage column... '
	UPDATE dbo.ActivityExecution
	SET
		ResultCategory= CASE
							WHEN CHARINDEX(':', ResultMessage) = 0 THEN ResultMessage
							ELSE LEFT(ResultMessage, CHARINDEX(':', ResultMessage) - 1)
						END
	WHERE ResultCategory IS NULL
PRINT 'Finished Initializing ResultCategory column... '
END
ELSE
	PRINT 'ActivityExecution ResultCategory Or ResultMessage Label not exist...'
GO

PRINT 'ActivityExecution Column Name changes completed'
PRINT ''
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'Idx_ActivityExecution_StartDateTime')
BEGIN
    PRINT 'Creating Idx_ActivityExecution_StartDateTime index... '
	CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_StartDateTime] ON [dbo].[ActivityExecution]
	([StartDateTime] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'UpdateType' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'Status' ) IS NULL
BEGIN
	PRINT '******************************************************'
	PRINT '***  ActivityExecutionVal Column Name changes ...  ***'
	PRINT '******************************************************'
	EXEC sp_rename 'ActivityExecutionVal.UpdateType', 'Status', 'COLUMN';
	PRINT 'ActivityExecutionVal UpdateType renamed to Status'
END
ELSE
	PRINT 'ActivityExecutionVal Status Column already exists Or Column UpdateType not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'ErrorMessage' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'ResultMessage' ) IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecutionVal.ErrorMessage', 'ResultMessage', 'COLUMN';
	PRINT 'ActivityExecutionVal ErrorMessage renamed to ResultMessage'
END
ELSE
	PRINT 'ActivityExecutionVal ResultMessage Column already exists Or Column ErrorMessage not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'Label' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'ResultCategory' ) IS NULL
BEGIN
	EXEC sp_rename 'ActivityExecutionVal.Label', 'ResultCategory', 'COLUMN';
	PRINT 'ActivityExecutionVal Label renamed to ResultCategory'
END
ELSE
	PRINT 'ActivityExecutionVal ResultCategory Column already exists Or Column Label not exists...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'ResultCategory' ) IS NOT NULL AND COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'ResultMessage' ) IS NOT NULL
BEGIN
	PRINT 'Initializing ResultCategory Null Values from ResultMessage column... '
	UPDATE dbo.ActivityExecutionVal
	SET
		ResultCategory= CASE
							WHEN CHARINDEX(':', ResultMessage) = 0 THEN ResultMessage
							ELSE LEFT(ResultMessage, CHARINDEX(':', ResultMessage) - 1)
						END
	WHERE ResultCategory IS NULL
PRINT 'Finished Initializing ResultCategory column... '
END
ELSE
	PRINT 'ActivityExecutionVal ResultCategory Or ResultMessage Label not exist...'
GO
IF COL_LENGTH ( 'dbo.ActivityExecutionVal' , 'RetryCount' ) IS NOT NULL
BEGIN
	ALTER TABLE dbo.ActivityExecutionVal DROP COLUMN RetryCount ;
	PRINT 'ActivityExecutionVal RetryCount Column dropped'
END
PRINT 'ActivityExecutionVal Column changes completed'
PRINT ''

IF OBJECT_ID('dbo.ActivityExecutionRetries', 'U') IS NULL
BEGIN
	PRINT '**********************************************'
	PRINT '***  Create ActivityExecutionRetries Table ***'
	PRINT '**********************************************'

	CREATE TABLE [dbo].[ActivityExecutionRetries](
		[ActivityExecutionRetriesId] [bigint] NOT NULL,
		[STF_ActivityExecutionRetriesId] [uniqueidentifier] NOT NULL,
		[ActivityExecutionId] [bigint] NOT NULL,
		[Status] [varchar](50) NULL,
		[ResultMessage] [varchar](1024) NULL,
		[ResultCategory] [varchar](1024) NULL,
		[RetryStartDateTime] [datetime] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_ActivityExecutionRetries] PRIMARY KEY CLUSTERED 
	(
		[ActivityExecutionRetriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[ActivityExecutionRetries] ADD  CONSTRAINT [defo_ActivityExecutionRetries_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetries_STF_ActivityExecutionRetriesId] ON [dbo].[ActivityExecutionRetries]
	([STF_ActivityExecutionRetriesId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityExecutionRetries]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityExecutionRetries_ActivityExecution] FOREIGN KEY([ActivityExecutionId])
	REFERENCES [dbo].[ActivityExecution] ([ActivityExecutionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ActivityExecutionRetries] CHECK CONSTRAINT [Fk_ActivityExecutionRetries_ActivityExecution]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetries_ActivityExecutionId] ON [dbo].[ActivityExecutionRetries]
	([ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityExecutionRetries]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityExecutionRetries_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityExecutionRetries] CHECK CONSTRAINT [Fk_ActivityExecutionRetries_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetries_ETLRunTableId] ON [dbo].[ActivityExecutionRetries]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created ActivityExecutionRetries Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionRetriesVal', 'U') IS NULL
BEGIN
	PRINT '*************************************************'
	PRINT '***  Create ActivityExecutionRetriesVal Table ***'
	PRINT '*************************************************'

	CREATE TABLE [dbo].[ActivityExecutionRetriesVal](
		[ActivityExecutionRetriesId] [bigint] NOT NULL,
		[STF_ActivityExecutionRetriesId] [uniqueidentifier] NULL,
		[SessionId] [varchar](50) NULL,
		[STF_SessionId] [varchar](50) NULL,
		[ActivityExecutionId] [bigint] NULL,
		[STF_ActivityExecutionId] [uniqueidentifier] NULL,
		[Status] [varchar](50) NULL,
		[ResultMessage] [varchar](1024) NULL,
		[ResultCategory] [varchar](1024) NULL,
		[RetryStartDateTime] [datetime] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_ActivityExecutionRetriesVal] PRIMARY KEY CLUSTERED 
	(
		[ActivityExecutionRetriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[ActivityExecutionRetriesVal] ADD  CONSTRAINT [defo_ActivityExecutionRetriesVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetriesVal_STF_ActivityExecutionRetriesId] ON [dbo].[ActivityExecutionRetriesVal]
	([STF_ActivityExecutionRetriesId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetriesVal_ActivityExecutionId] ON [dbo].[ActivityExecutionRetriesVal]
	([ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityExecutionRetriesVal]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityExecutionRetriesVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityExecutionRetriesVal] CHECK CONSTRAINT [Fk_ActivityExecutionRetriesVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetriesVal_ETLRunTableId] ON [dbo].[ActivityExecutionRetries]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetriesVal_STF_SessionId] ON [dbo].[ActivityExecutionRetriesVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	
	PRINT ' Created ActivityExecutionRetriesVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionRetries', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityExecutionRetriesVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure ActivityExecutionRetries ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=16, @SrcTable='ActivityExecutionRetries', @TableType='ACTIVITY_EXECUTION';
	EXECUTE dbo.setETLJob @ETLJobId=46, @JobName='ODS_L3ah_ActivityExecutionRetries';
END

IF OBJECT_ID('dbo.ActivityExecutionDeviceUsage', 'U') IS NOT NULL
BEGIN
	PRINT '***************************************************************************'
	PRINT '***  Rename ActivityExecutionDeviceUsage To ActivityExecutionAssetUsage ***'
	PRINT '***       and column Name Changes ...                                   ***'
	PRINT '***************************************************************************'
	EXEC sp_rename 'ActivityExecutionDeviceUsage', 'ActivityExecutionAssetUsage';
	EXEC sp_rename 'ActivityExecutionAssetUsage.ActivityExecutionDeviceUsageId', 'ActivityExecutionAssetUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsage.STF_ActivityExecutionDeviceUsageId', 'STF_ActivityExecutionAssetUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsage.DeviceId', 'AssetId', 'COLUMN';
	EXEC sp_rename 'dbo.defo_ActivityExecutionDeviceUsage_LoadOperation', 'dbo.defo_ActivityExecutionAssetUsage_LoadOperation', 'OBJECT';
	EXEC sp_rename 'ActivityExecutionAssetUsage.PK_ActivityExecutionDeviceUsage', 'PK_ActivityExecutionAssetUsage', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsage.Idx_ActivityExecutionDeviceUsage_ActivityExecutionId', 'Idx_ActivityExecutionAssetUsage_ActivityExecutionId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsage.Idx_ActivityExecutionDeviceUsage_ETLRunTableId', 'Idx_ActivityExecutionAssetUsage_ETLRunTableId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsage.Idx_ActivityExecutionDeviceUsage_STF_DeviceUsageId', 'Idx_ActivityExecutionAssetUsage_ActivityExecutionAssetUsageId', 'INDEX';
	EXEC sp_rename 'Fk_ActivityExecutionDeviceUsage_ActivityExecution', 'FK_ActivityExecutionAssetUsage_ActivityExecution';
	EXEC sp_rename 'Fk_ActivityExecutionDeviceUsage_ETLRunTableSummary', 'FK_ActivityExecutionAssetUsage_ETLRunTableSummary';
	

	PRINT 'conversion Of ActivityExecutionDeviceUsage  -->  ActivityExecutionAssetUsage completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionDeviceUsageVal', 'U') IS NOT NULL
BEGIN
	PRINT '*********************************************************************************'
	PRINT '***  Rename ActivityExecutionDeviceUsageVal To ActivityExecutionAssetUsageVal ***'
	PRINT '***       and column Name Changes ...                                         ***'
	PRINT '*********************************************************************************'
	EXEC sp_rename 'ActivityExecutionDeviceUsageVal', 'ActivityExecutionAssetUsageVal';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.ActivityExecutionDeviceUsageId', 'ActivityExecutionAssetUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.STF_ActivityExecutionDeviceUsageId', 'STF_ActivityExecutionAssetUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.DeviceId', 'AssetId', 'COLUMN';
	EXEC sp_rename 'dbo.defo_ActivityExecutionDeviceUsageVal_LoadOperation', 'dbo.defo_ActivityExecutionAssetUsageVal_LoadOperation', 'OBJECT';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.PK_WorkerActivityDeviceUsageVal', 'PK_ActivityExecutionAssetUsageVal', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.Idx_ActivityExecutionDeviceUsageVal_STF_ActivityExecutionDeviceUsageId', 'Idx_ActivityExecutionAssetUsageVal_STF_ActivityExecutionAssetUsageId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.Idx_ActivityExecutionDeviceUsageVal_STF_ActivityExecutionId', 'Idx_ActivityExecutionAssetUsageVal_STF_ActivityExecutionId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.Idx_ActivityExecutionDeviceUsageVal_ETLRunTableId', 'Idx_ActivityExecutionAssetUsageVal_ETLRunTableId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionAssetUsageVal.Idx_ActivityExecutionDeviceUsageVal_STF_SessionId', 'Idx_ActivityExecutionAssetUsageVal_STF_SessionId', 'INDEX';
	EXEC sp_rename 'Fk_ActivityExecutionDeviceUsageVal_ETLRunTableSummary', 'FK_ActivityExecutionAssetUsageVal_ETLRunTableSummary';
	PRINT 'conversion Of ActivityExecutionDeviceUsageVal  -->  ActivityExecutionAssetUsageVal completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionPerformance', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityExecutionPerformanceVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Move ETL Metadata from ActivityExecutionPerformance from ETLTableId=11 to 15, @ETLJobId=42 to 45 ';
	EXECUTE dbo.setETLTable @Op='Move', @ETLTableId=11, @ToETLTableId=15;
	EXECUTE dbo.setETLJob @Op='Move', @ETLJobId=42, @ToETLJobId=45;
END
GO

IF OBJECT_ID('dbo.ActivityExecutionAssetUsage', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityExecutionAssetUsageVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Move ETL Metadata from ActivityExecActivityExecutionDeviceUsage to ActivityExecutionAssetUsage';
	EXECUTE dbo.setETLTable @Op='Move', @ETLTableId=12, @SrcTable='ActivityExecutionAssetUsage';
	EXECUTE dbo.setETLJob  @Op='Move', @ETLJobId=41, @ToETLJobId=42, @JobName='ODS_L3ab_ActivityExecutionAssetUsage';
END
GO

IF OBJECT_ID('dbo.TestDocumentUsage', 'U') IS NOT NULL
BEGIN
	PRINT '*******************************************************************'
	PRINT '***  Rename TestDocumentUsage To ActivityExecutionDocumentUsage ***'
	PRINT '*******************************************************************'
	EXEC sp_rename 'TestDocumentUsage', 'ActivityExecutionDocumentUsage';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.TestDocumentUsageId', 'ActivityExecutionDocumentUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.STF_TestDocumentUsageId', 'STF_ActivityExecutionDocumentUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.FileName', 'DocumentName', 'COLUMN';
	EXEC sp_rename 'dbo.defo_TestDocumentUsage_LoadOperation', 'dbo.defo_ActivityExecutionDocumentUsage_LoadOperation', 'OBJECT';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.PK_TestDocumentUsage', 'PK_ActivityExecutionDocumentUsage', 'INDEX';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.idx_TestDocumentUsage_ActivityExecutionId', 'Idx_ActivityExecutionDocumentUsage_ActivityExecutionId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.idx_TestDocumentUsage_ETLRunTableId', 'Idx_ActivityExecutionDocumentUsage_ETLRunTableId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionDocumentUsage.Idx_TestDocumentUsage_STF_TestDocumentUsageId', 'Idx_ActivityExecutionDocumentUsage_STF_ActivityExecutionDocumentUsageId', 'INDEX';

	PRINT 'conversion Of TestDocumentUsage  -->  ActivityExecutionDocumentUsage completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.TestDocumentUsageVal', 'U') IS NOT NULL
BEGIN
	PRINT '*************************************************************************'
	PRINT '***  Rename TestDocumentUsageVal To ActivityExecutionDocumentUsageVal ***'
	PRINT '*************************************************************************'
	EXEC sp_rename 'TestDocumentUsageVal', 'ActivityExecutionDocumentUsageVal';
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.TestDocumentUsageId', 'ActivityExecutionDocumentUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.STF_TestDocumentUsageId', 'STF_ActivityExecutionDocumentUsageId', 'COLUMN';
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.FileName', 'DocumentName', 'COLUMN';
	EXEC sp_rename 'dbo.defo_TestDocumentUsageVal_LoadOperation', 'dbo.defo_ActivityExecutionDocumentUsageVal_LoadOperation', 'OBJECT';
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.PK_TestDocumentUsageVal', 'PK_ActivityExecutionDocumentUsageVal', 'INDEX';
	DROP INDEX [idx_TestDocumentUsageVal_ActivityExecutionId] ON [dbo].[ActivityExecutionDocumentUsageVal]
	CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionDocumentUsageVal_STF_ActivityExecutionId] ON [dbo].[ActivityExecutionDocumentUsageVal]
	([STF_ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.idx_TestDocumentUsageVal_ETLRunTableId', 'Idx_ActivityExecutionDocumentUsageVal_ETLRunTableId', 'INDEX';
	EXEC sp_rename 'ActivityExecutionDocumentUsageVal.Idx_TestDocumentUsageVal_STF_TestDocumentUsageId', 'Idx_ActivityExecutionDocumentUsageVal_STF_ActivityExecutionDocumentUsageId', 'INDEX';

	PRINT 'conversion Of TestDocumentUsageVal  -->  ActivityExecutionDocumentUsageVal completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityExecutionDocumentUsage', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityExecutionDocumentUsageVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Move ETL Metadata from TestDocumentUsage to ActivityExecutionDocumentUsage';
	EXECUTE dbo.setETLTable @Op='Move', @ETLTableId=82, @ToETLTableId=11, @SrcTable='ActivityExecutionDocumentUsage', @TableType='ACTIVITY_EXECUTION';
	EXECUTE dbo.setETLJob @Op='Move', @ETLJobId=82, @ToETLJobId=41, @JobName='ODS_L3ac_ActivityExecutionDocumentUsage';
END
GO

IF OBJECT_ID('dbo.TestDocumentReference', 'U') IS NOT NULL
BEGIN
	PRINT '********************************************************'
	PRINT '***  Rename TestDocumentReference To SessionDocument ***'
	PRINT '********************************************************'
	EXEC sp_rename 'TestDocumentReference', 'SessionDocument';
	EXEC sp_rename 'SessionDocument.TestDocumentReferenceId', 'SessionDocumentId', 'COLUMN';
	EXEC sp_rename 'SessionDocument.STF_TestDocumentReferenceId', 'STF_SessionDocumentId', 'COLUMN';
	EXEC sp_rename 'dbo.defo_TestDocumentReference_LoadOperation', 'dbo.defo_SessionDocument_LoadOperation', 'OBJECT';
	EXEC sp_rename 'SessionDocument.PK_TestDocumentReference', 'PK_SessionDocument', 'INDEX';
	EXEC sp_rename 'SessionDocument.idx_TestDocumentReference_ETLRunTableId', 'Idx_SessionDocument_ETLRunTableId', 'INDEX';
	EXEC sp_rename 'SessionDocument.idx_TestDocumentReference_SessionId', 'Idx_SessionDocument_SessionId', 'INDEX';
	EXEC sp_rename 'SessionDocument.Idx_TestDocumentReference_STF_TestDocumentReferenceId', 'Idx_SessionDocument_STF_SessionDocumentId', 'INDEX';
	
	PRINT 'conversion Of TestDocumentReference  -->  SessionDocument completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.TestDocumentReferenceVal', 'U') IS NOT NULL
BEGIN
	PRINT '**************************************************************'
	PRINT '***  Rename TestDocumentReferenceVal To SessionDocumentVal ***'
	PRINT '**************************************************************'
	EXEC sp_rename 'TestDocumentReferenceVal', 'SessionDocumentVal';
	EXEC sp_rename 'SessionDocumentVal.TestDocumentReferenceId', 'SessionDocumentId', 'COLUMN';
	EXEC sp_rename 'SessionDocumentVal.STF_TestDocumentReferenceId', 'STF_SessionDocumentId', 'COLUMN';
	EXEC sp_rename 'dbo.defo_TestDocumentReferenceVal_LoadOperation', 'dbo.defo_SessionDocumentVal_LoadOperation', 'OBJECT';
	EXEC sp_rename 'SessionDocumentVal.PK_TestDocumentReferenceVal', 'PK_SessionDocumentVal', 'INDEX';
	EXEC sp_rename 'SessionDocumentVal.idx_TestDocumentReferenceVal_ETLRunTableId', 'Idx_SessionDocumentVal_ETLRunTableId', 'INDEX';
	DROP INDEX [idx_TestDocumentReferenceVal_SessionId] ON [dbo].[SessionDocumentVal]
	CREATE NONCLUSTERED INDEX [Idx_SessionDocumentVal_STF_SessionId] ON [dbo].[SessionDocumentVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	EXEC sp_rename 'SessionDocumentVal.Idx_TestDocumentReferenceVal_STF_TestDocumentReferenceId', 'Idx_SessionDocumentVal_STF_SessionDocumentId', 'INDEX';
	
	PRINT 'conversion Of TestDocumentReferenceVal  -->  SessionDocumentVal completed'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionDocument', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.SessionDocumentVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Move ETL Metadata from TestDocumentReference to SessionDocumentVal';
	EXECUTE dbo.setETLTable @Op='Move', @ETLTableId=81, @ToETLTableId=4, @SrcTable='SessionDocument', @TableType='SESSION';
	EXECUTE dbo.setETLJob @Op='Move', @ETLJobId=31, @ToETLJobId=1031;  -- moved to id 1031 to avoid referential integrity issues if record is deleted. 
	EXECUTE dbo.setETLJob @Op='Move', @ETLJobId=81, @ToETLJobId=31, @JobName='ODS_L2a2_SessionDocument';
END
GO

IF OBJECT_ID('dbo.ActivityTaskExecution', 'U') IS NULL
BEGIN
	PRINT '*******************************************'
	PRINT '***  Create ActivityTaskExecution Table ***'
	PRINT '*******************************************'
	CREATE TABLE [dbo].[ActivityTaskExecution](
		[ActivityTaskExecutionId] [bigint] NOT NULL,
		[STF_ActivityTaskExecutionId] [uniqueidentifier] NULL,
		[ActivityExecutionId] [bigint] NOT NULL,
		[TaskName] [varchar](50) NULL,
		[Status] [varchar](50) NULL,
		[StartDateTime] [datetime] NULL,
		[EndDateTime] [datetime] NULL,
		[ExecutionPath] [varchar](50) NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [Pk_ActivityTaskExecution] PRIMARY KEY CLUSTERED 
	(
		[ActivityTaskExecutionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ActivityTaskExecution] ADD  CONSTRAINT [defo_ActivityTaskExecution_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecution_STF_ActivityTaskExecutionId] ON [dbo].[ActivityTaskExecution]
	([STF_ActivityTaskExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityTaskExecution]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskExecution_ActivityExecution] FOREIGN KEY([ActivityExecutionId])
	REFERENCES [dbo].[ActivityExecution] ([ActivityExecutionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ActivityTaskExecution] CHECK CONSTRAINT [Fk_ActivityTaskExecution_ActivityExecution]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecution_ActivityExecutionId] ON [dbo].[ActivityTaskExecution]
	([ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityTaskExecution]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskExecution_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityTaskExecution] CHECK CONSTRAINT [Fk_ActivityTaskExecution_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecution_ETLRunTableId] ON [dbo].[ActivityTaskExecution]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created ActivityTaskExecution Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityTaskExecutionVal', 'U') IS NULL
BEGIN
	PRINT '**********************************************'
	PRINT '***  Create ActivityTaskExecutionVal Table ***'
	PRINT '**********************************************'
	CREATE TABLE [dbo].[ActivityTaskExecutionVal](
		[ActivityTaskExecutionId] [bigint] NOT NULL,
		[STF_ActivityTaskExecutionId] [uniqueidentifier] NULL,
		[ActivityExecutionId] [bigint] NOT NULL,
		[STF_ActivityExecutionId] [uniqueidentifier] NULL,
		[SessionId] [int] NULL,
		[STF_SessionId] [varchar](50) NULL,
		[TaskName] [varchar](50) NULL,
		[Status] [varchar](50) NULL,
		[StartDateTime] [datetime] NULL,
		[EndDateTime] [datetime] NULL,
		[ExecutionPath] [varchar](50) NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [Pk_ActivityTaskExecutionVar] PRIMARY KEY CLUSTERED 
	(
		[ActivityTaskExecutionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ActivityTaskExecutionVal] ADD  CONSTRAINT [defo_ActivityTaskExecutionVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_STF_ActivityTaskExecutionId] ON [dbo].[ActivityTaskExecutionVal]
	([STF_ActivityTaskExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_STF_ActivityExecutionId] ON [dbo].[ActivityTaskExecutionVal]
	([STF_ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityTaskExecutionVal]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskExecutionVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityTaskExecutionVal] CHECK CONSTRAINT [Fk_ActivityTaskExecutionVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_ETLRunTableId] ON [dbo].[ActivityTaskExecutionVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_STF_SessionId] ON [dbo].[ActivityTaskExecutionVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created ActivityTaskExecutionVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityTaskExecution', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityTaskExecutionVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure ActivityTaskExecution ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=17, @SrcTable='ActivityTaskExecution', @TableType='ACTIVITY_EXECUTION';
	EXECUTE dbo.setETLJob @ETLJobId=47, @JobName='ODS_L3af_ActivityTaskExecution';
END

IF OBJECT_ID('dbo.ActivityTaskStepExecution', 'U') IS NULL
BEGIN
	PRINT '***********************************************'
	PRINT '***  Create ActivityTaskStepExecution Table ***'
	PRINT '***********************************************'
	CREATE TABLE [dbo].[ActivityTaskStepExecution](
		[ActivityTaskStepExecutionId] [bigint] NOT NULL,
		[STF_ActivityTaskStepExecutionId] [uniqueidentifier] NOT NULL,
		[ActivityTaskExecutionId] [bigint] NOT NULL,
		[StepName] [varchar](50) NULL,
		[Status] [varchar](50) NULL,
		[StartDateTime] [datetime] NULL,
		[EndDateTime] [datetime] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [Pk_ActivityTaskStepExecution] PRIMARY KEY CLUSTERED 
	(
	[ActivityTaskStepExecutionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[ActivityTaskStepExecution] ADD  CONSTRAINT [defo_ActivityTaskStepExecution_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskStepExecution_STF_ActivityTaskStepExecutionId] ON [dbo].[ActivityTaskStepExecution]
	([STF_ActivityTaskStepExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityTaskStepExecution]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskStepExecution_ActivityTaskExecution] FOREIGN KEY([ActivityTaskExecutionId])
	REFERENCES [dbo].[ActivityTaskExecution] ([ActivityTaskExecutionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ActivityTaskStepExecution] CHECK CONSTRAINT [Fk_ActivityTaskStepExecution_ActivityTaskExecution]

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskStepExecution_ActivityTaskExecutionId] ON [dbo].[ActivityTaskStepExecution]
	([ActivityTaskExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[ActivityTaskStepExecution]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskStepExecution_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityTaskStepExecution] CHECK CONSTRAINT [Fk_ActivityTaskStepExecution_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecution_ETLRunTableId] ON [dbo].[ActivityTaskStepExecution]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created ActivityTaskStepExecution Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityTaskStepExecutionVal', 'U') IS NULL
BEGIN
	PRINT '**************************************************'
	PRINT '***  Create ActivityTaskStepExecutionVal Table ***'
	PRINT '**************************************************'
	CREATE TABLE [dbo].[ActivityTaskStepExecutionVal](
		[ActivityTaskStepExecutionId] [bigint] NOT NULL,
		[STF_ActivityTaskStepExecutionId] [uniqueidentifier] NULL,
		[ActivityTaskExecutionId] [bigint] NOT NULL,
		[STF_ActivityTaskExecutionId] [uniqueidentifier] NULL,
		[STF_ActivityExecutionId] [uniqueidentifier] NULL,
		[SessionId] [int] NULL,
		[STF_SessionId] [varchar](50) NULL,
		[StepName] [varchar](50) NULL,
		[Status] [varchar](50) NULL,
		[StartDateTime] [datetime] NULL,
		[EndDateTime] [datetime] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [Pk_ActivityTaskStepExecutionVal] PRIMARY KEY CLUSTERED 
	(
		[ActivityTaskStepExecutionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ActivityTaskStepExecutionVal] ADD  CONSTRAINT [defo_ActivityTaskStepExecutionVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskStepExecutionVal_STF_ActivityTaskStepExecutionId] ON [dbo].[ActivityTaskStepExecutionVal]
	([STF_ActivityTaskStepExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskStepExecutionVal_STF_ActivityTaskExecutionId] ON [dbo].[ActivityTaskStepExecutionVal]
	([STF_ActivityTaskExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskStepExecutionVal_STF_ActivityExecutionId] ON [dbo].[ActivityTaskStepExecutionVal]
	([STF_ActivityExecutionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[ActivityTaskStepExecutionVal]  WITH CHECK ADD  CONSTRAINT [Fk_ActivityTaskStepExecutionVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[ActivityTaskStepExecutionVal] CHECK CONSTRAINT [Fk_ActivityTaskStepExecutionVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_ETLRunTableId] ON [dbo].[ActivityTaskStepExecutionVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_ActivityTaskExecutionVal_STF_SessionId] ON [dbo].[ActivityTaskStepExecutionVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created ActivityTaskStepExecutionVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.ActivityTaskStepExecution', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.ActivityTaskStepExecutionVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure ActivityTaskStepExecution ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=18, @SrcTable='ActivityTaskStepExecution', @TableType='ACTIVITY_EXECUTION';
	EXECUTE dbo.setETLJob @ETLJobId=48, @JobName='ODS_L3ag_ActivityTaskStepExecution';
END

IF OBJECT_ID('dbo.SessionDevice', 'U') IS NULL
BEGIN
	PRINT '***********************************'
	PRINT '***  Create SessionDevice Table ***'
	PRINT '***********************************'

	CREATE TABLE [dbo].[SessionDevice](
		[SessionDeviceId] [bigint] NOT NULL,
		[STF_SessionDeviceId] [uniqueidentifier] NULL,
		[SessionId] [int] NOT NULL,
		[DeviceId] [varchar](50) NOT NULL,
		[ProductName] [varchar](100) NULL,
		[DeviceName] [varchar](255) NULL,
		[FirmwareRevision] [varchar](50) NULL,
		[FirmwareDatecode] [varchar](10) NULL,
		[FirmwareType] [varchar](50) NULL,
		[ModelNumber] [varchar](50) NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_SessionDevice] PRIMARY KEY CLUSTERED 
	(
		[SessionDeviceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[SessionDevice] ADD  CONSTRAINT [defo_SessionDevice_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_SessionDevice_STF_SessionDeviceId] ON [dbo].[SessionDevice]
	([STF_SessionDeviceId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[SessionDevice]  WITH CHECK ADD  CONSTRAINT [Fk_SessionDevice_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[SessionDevice] CHECK CONSTRAINT [Fk_SessionDevice_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionDevice_ETLRunTableId] ON [dbo].[SessionDevice]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[SessionDevice]  WITH CHECK ADD  CONSTRAINT [Fk_SessionDevice_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SessionDevice] CHECK CONSTRAINT [Fk_SessionDevice_SessionSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionDevice_SessionId] ON [dbo].[SessionDevice]
	([SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created SessionDevice Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionDeviceVal', 'U') IS NULL
BEGIN
	PRINT '**************************************'
	PRINT '***  Create SessionDeviceVal Table ***'
	PRINT '**************************************'
	CREATE TABLE [dbo].[SessionDeviceVal](
		[SessionDeviceId] [bigint] NOT NULL,
		[STF_SessionDeviceId] [uniqueidentifier] NULL,
		[SessionId] [int] NOT NULL,
		[STF_SessionId] [varchar](50) NULL,
		[DeviceId] [varchar](50) NULL,
		[ProductName] [varchar](100) NULL,
		[DeviceName] [varchar](255) NULL,
		[FirmwareRevision] [varchar](50) NULL,
		[FirmwareDatecode] [varchar](10) NULL,
		[FirmwareType] [varchar](50) NULL,
		[ModelNumber] [varchar](50) NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_SessionDeviceVal] PRIMARY KEY CLUSTERED 
	(
		[SessionDeviceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[SessionDeviceVal] ADD  CONSTRAINT [defo_SessionDeviceVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_SessionDeviceVal_STF_SessionDeviceId] ON [dbo].[SessionDeviceVal]
	([STF_SessionDeviceId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[SessionDeviceVal]  WITH CHECK ADD  CONSTRAINT [Fk_SessionDeviceVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[SessionDeviceVal] CHECK CONSTRAINT [Fk_SessionDeviceVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionDeviceVal_ETLRunTableId] ON [dbo].[SessionDeviceVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_SessionDeviceVal_STF_SessionId] ON [dbo].[SessionDeviceVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created SessionDeviceVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionDevice', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.SessionDeviceVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure SessionDevice ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=5, @SrcTable='SessionDevice', @TableType='SESSION';
	EXECUTE dbo.setETLJob @ETLJobId=32, @JobName='ODS_L2l_SessionDevice';
END

IF OBJECT_ID('dbo.SessionServer', 'U') IS NULL
BEGIN
	PRINT '***********************************'
	PRINT '***  Create SessionServer Table ***'
	PRINT '***********************************'

	CREATE TABLE [dbo].[SessionServer](
		[SessionServerId] [bigint] NOT NULL,
		[STF_SessionServerId] [uniqueidentifier] NOT NULL,
		[SessionId] [int] NOT NULL,
		[ServerId] [uniqueidentifier] NULL,
		[HostName] [varchar](50) NULL,
		[Address] [varchar](50) NULL,
		[OperatingSystem] [varchar](50) NULL,
		[Architecture] [varchar](10) NULL,
		[Processors] [int] NULL,
		[Cores] [int] NULL,
		[Memory] [int] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_SessionServer] PRIMARY KEY CLUSTERED 
	(
		[SessionServerId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[SessionServer] ADD  CONSTRAINT [defo_SessionServer_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_SessionServer_STF_SessionServerId] ON [dbo].[SessionServer]
	([STF_SessionServerId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[SessionServer]  WITH CHECK ADD  CONSTRAINT [Fk_SessionServer_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[SessionServer] CHECK CONSTRAINT [Fk_SessionServer_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionServer_ETLRunTableId] ON [dbo].[SessionServer]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[SessionServer]  WITH CHECK ADD  CONSTRAINT [Fk_SessionServer_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SessionServer] CHECK CONSTRAINT [Fk_SessionServer_SessionSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionServer_SessionId] ON [dbo].[SessionServer]
	([SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created SessionServer Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionServerVal', 'U') IS NULL
BEGIN
	PRINT '**************************************'
	PRINT '***  Create SessionServerVal Table ***'
	PRINT '**************************************'
	CREATE TABLE [dbo].[SessionServerVal](
		[SessionServerId] [bigint] NOT NULL,
		[STF_SessionServerId] [uniqueidentifier] NULL,
		[SessionId] [int] NOT NULL,
		[STF_SessionId] [varchar](50) NULL,
		[ServerId] [uniqueidentifier] NULL,
		[HostName] [varchar](50) NULL,
		[Address] [varchar](50) NULL,
		[OperatingSystem] [varchar](50) NULL,
		[Architecture] [varchar](10) NULL,
		[Processors] [int] NULL,
		[Cores] [int] NULL,
		[Memory] [int] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_SessionServerVal] PRIMARY KEY CLUSTERED
	(
		[SessionServerId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[SessionServerVal] ADD  CONSTRAINT [defo_SessionServerVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_SessionServerVal_STF_SessionServerId] ON [dbo].[SessionServerVal]
	([STF_SessionServerId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[SessionServerVal]  WITH CHECK ADD  CONSTRAINT [Fk_SessionServerVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE
	
	ALTER TABLE [dbo].[SessionServerVal] CHECK CONSTRAINT [Fk_SessionServerVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_SessionServerVal_ETLRunTableId] ON [dbo].[SessionServerVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_SessionServerVal_STF_SessionId] ON [dbo].[SessionServerVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created SessionServerVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.SessionServer', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.SessionServerVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure SessionServer ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=6, @SrcTable='SessionServer', @TableType='SESSION';
	EXECUTE dbo.setETLJob @ETLJobId=33, @JobName='ODS_L2o_SessionServer';
END

IF OBJECT_ID('dbo.DeviceMemorySnapshot', 'U') IS NULL
BEGIN
	PRINT '******************************************'
	PRINT '***  Create DeviceMemorySnapshot Table ***'
	PRINT '******************************************'
	CREATE TABLE [dbo].[DeviceMemorySnapshot](
		[DeviceMemorySnapshotId] [bigint] NOT NULL,
		[STF_DeviceMemorySnapshotId] [uniqueidentifier] NULL,
		[SessionId] [int] NOT NULL,
		[DeviceId] [varchar](50) NOT NULL,
		[SnapshotLabel] [varchar](50) NULL,
		[UsageCount] [int] NOT NULL,
		[SnapshotDateTime] [datetime] NOT NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_DeviceMemorySnapshot] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemorySnapshotId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[DeviceMemorySnapshot] ADD  CONSTRAINT [defo_DeviceMemorySnapshot_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshot_STF_DeviceMemorySnapshotId] ON [dbo].[DeviceMemorySnapshot]
	([STF_DeviceMemorySnapshotId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemorySnapshot_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [Fk_DeviceMemorySnapshot_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshot_ETLRunTableId] ON [dbo].[DeviceMemorySnapshot]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemorySnapshot_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [Fk_DeviceMemorySnapshot_SessionSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshot_SessionId] ON [dbo].[DeviceMemorySnapshot]
	([SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created DeviceMemorySnapshot Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.DeviceMemorySnapshotVal', 'U') IS NULL
BEGIN
	PRINT '*********************************************'
	PRINT '***  Create DeviceMemorySnapshotVal Table ***'
	PRINT '*********************************************'
	CREATE TABLE [dbo].[DeviceMemorySnapshotVal](
		[DeviceMemorySnapshotId] [bigint] NOT NULL,
		[STF_DeviceMemorySnapshotId] [uniqueidentifier] NULL,
		[SessionId] [int] NOT NULL,
		[STF_SessionId] [varchar](50) NULL,
		[DeviceId] [varchar](50) NULL,
		[SnapshotLabel] [varchar](50) NULL,
		[UsageCount] [int] NOT NULL,
		[SnapshotDateTime] [datetime] NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_DeviceMemorySnapshotVal] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemorySnapshotId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[DeviceMemorySnapshotVal] ADD  CONSTRAINT [defo_DeviceMemorySnapshotVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshotVal_STF_DeviceMemorySnapshotId] ON [dbo].[DeviceMemorySnapshotVal]
	([STF_DeviceMemorySnapshotId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[DeviceMemorySnapshotVal]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemorySnapshotVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[DeviceMemorySnapshotVal] CHECK CONSTRAINT [Fk_DeviceMemorySnapshotVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshotVal_ETLRunTableId] ON [dbo].[DeviceMemorySnapshotVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshotVal_STF_SessionId] ON [dbo].[DeviceMemorySnapshotVal]
	([STF_SessionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created DeviceMemorySnapshotVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.DeviceMemorySnapshot', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.DeviceMemorySnapshotVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure DeviceMemorySnapshot ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=62, @SrcTable='DeviceMemorySnapshot', @TableType='DEVICE';
	EXECUTE dbo.setETLJob @ETLJobId=34, @JobName='ODS_L2n_DeviceMemorySnapshot';
END

IF OBJECT_ID('dbo.DeviceMemoryCount', 'U') IS NULL
BEGIN
	PRINT '***************************************'
	PRINT '***  Create DeviceMemoryCount Table ***'
	PRINT '***************************************'
	CREATE TABLE [dbo].[DeviceMemoryCount](
		[DeviceMemoryCountId] [bigint] NOT NULL,
		[STF_DeviceMemoryCountId] [uniqueidentifier] NULL,
		[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
		[CategoryName] [varchar](255) NULL,
		[DataLabel] [varchar](100) NULL,
		[DataValue] [bigint] NOT NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_DeviceMemoryCount] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemoryCountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[DeviceMemoryCount] ADD  CONSTRAINT [defo_DeviceMemoryCount_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCount_STF_DeviceMemoryCountId] ON [dbo].[DeviceMemoryCount]
	([STF_DeviceMemoryCountId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemoryCount_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemoryCountId])
	REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [Fk_DeviceMemoryCount_DeviceMemorySnapshot]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCount_DeviceMemorySnapshotId] ON [dbo].[DeviceMemoryCount]
	([DeviceMemorySnapshotId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemoryCount_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [Fk_DeviceMemoryCount_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCount_ETLRunTableId] ON [dbo].[DeviceMemoryCount]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	PRINT ' Created DeviceMemoryCount Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.DeviceMemoryCountVal', 'U') IS NULL
BEGIN
	PRINT '******************************************'
	PRINT '***  Create DeviceMemoryCountVal Table ***'
	PRINT '******************************************'
	CREATE TABLE [dbo].[DeviceMemoryCountVal](
		[DeviceMemoryCountId] [bigint] NOT NULL,
		[STF_DeviceMemoryCountId] [uniqueidentifier] NULL,
		[DeviceMemorySnapshotId] [bigint] NOT NULL,
		[STF_DeviceMemorySnapshotId] [uniqueidentifier] NULL,
		[CategoryName] [varchar](255) NULL,
		[DataLabel] [varchar](100) NULL,
		[DataValue] [bigint] NOT NULL,
		[ETLRunTableId] [int] NOT NULL,
		[LoadOperation] [char](1) NOT NULL,
		[CRC] [bigint] NULL,
	 CONSTRAINT [PK_DeviceMemoryCountVal] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemoryCountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[DeviceMemoryCountVal] ADD  CONSTRAINT [defo_DeviceMemoryCountVal_LoadOperation]  DEFAULT ('I') FOR [LoadOperation]

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCountVal_STF_DeviceMemoryCountId] ON [dbo].[DeviceMemoryCountVal]
	([STF_DeviceMemoryCountId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCountVal_STF_DeviceMemorySnapshotId] ON [dbo].[DeviceMemoryCountVal]
	([STF_DeviceMemorySnapshotId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	ALTER TABLE [dbo].[DeviceMemoryCountVal]  WITH CHECK ADD  CONSTRAINT [Fk_DeviceMemoryCountVal_ETLRunTableSummary] FOREIGN KEY([ETLRunTableId])
	REFERENCES [dbo].[ETLRunTableSummary] ([ETLRunTableId])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[DeviceMemoryCountVal] CHECK CONSTRAINT [Fk_DeviceMemoryCountVal_ETLRunTableSummary]
	
	CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCountVal_ETLRunTableId] ON [dbo].[DeviceMemoryCountVal]
	([ETLRunTableId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	PRINT ' Created DeviceMemoryCountVal Table'
	PRINT ''
END
GO

IF OBJECT_ID('dbo.DeviceMemoryCount', 'U') IS NOT NULL AND
   OBJECT_ID('dbo.DeviceMemoryCountVal', 'U') IS NOT NULL
BEGIN
	PRINT 'Configure DeviceMemoryCount ETL Metadata';
	EXECUTE dbo.setETLTable @ETLTableId=63, @SrcTable='DeviceMemoryCount', @TableType='DEVICE';
	EXECUTE dbo.setETLJob @ETLJobId=35, @JobName='ODS_L2m_DeviceMemoryCount';
END

PRINT '************************************************************'
PRINT '***  Opdate ETLTable TableProcessingGroup To DEPRECATED  ***'
PRINT '***  On Log DB Deprecated Tables:                        ***'
PRINT '***               STFSessionUtilization                  ***'
PRINT '***               STFAssetUtilization                    ***'
PRINT '***               STFClientUtilization                   ***'
PRINT '***               PinPrintJobRetrieval                   ***'
PRINT '***               RemoteQueueInstall                     ***'
PRINT '************************************************************'
GO
UPDATE dbo.ETLTable
   SET TableProcessingGroup = 'DEPRECATED'
 WHERE SourceTableName IN('STFSessionUtilization','STFAssetUtilization','STFClientUtilization','PinPrintJobRetrieval','RemoteQueueInstall')
GO

PRINT '***************************************************************'
PRINT '***  Re Create Dropped Views compatible with Schema Changes ***'
PRINT '***************************************************************'

IF OBJECT_ID('Reports.vw_tbl_TempActivityExecutionDeviceUsage', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempActivityExecutionDeviceUsage...'
	DROP VIEW [Reports].[vw_tbl_TempActivityExecutionDeviceUsage]
END
GO
IF OBJECT_ID('Reports.vw_tbl_TempActivityExecutionAssetUsage', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempActivityExecutionAssetUsage...'
	DROP VIEW [Reports].vw_tbl_TempActivityExecutionAssetUsage
END
GO
PRINT 'Creating vw_tbl_TempActivityExecutionAssetUsage View ...'
GO
--
-- This view is used in multiple reports.
--
CREATE VIEW [Reports].[vw_tbl_TempActivityExecutionAssetUsage]
WITH SCHEMABINDING
AS
SELECT aeau.ActivityExecutionId,
       aeau.AssetId,
       aeau.FirmwareVersion, 
       aeau.Product,
       ae.SessionId
  FROM dbo.ActivityExecutionAssetUsage aeau
  INNER JOIN dbo.ActivityExecution ae ON aeau.ActivityExecutionId = ae.ActivityExecutionId

GO

IF OBJECT_ID('Reports.vw_tbl_TempPrintJobClient', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempPrintJobClient...'
	DROP VIEW [Reports].[vw_tbl_TempPrintJobClient]
END
GO
--
-- This view is used in multiple reports.
--
CREATE VIEW [Reports].[vw_tbl_TempPrintJobClient]
WITH SCHEMABINDING
AS
SELECT pjc.ActivityExecutionId,
       pjc.[FileName],
       pjc.JobStartDateTime,
       ae.SessionId
  FROM dbo.PrintJobClient pjc
  INNER JOIN dbo.ActivityExecution ae ON pjc.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_tbl_TempProduct_CTE', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempProduct_CTE...'
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.DigitalSendJobInput dsji ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.PullPrintJobRetrieval ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_tbl_TempPullPrintJobRetrieval', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_tbl_TempPullPrintJobRetrieval...'
	DROP VIEW [Reports].[vw_tbl_TempPullPrintJobRetrieval]
END
GO
--
-- This view is used in multiple reports.
--
CREATE VIEW [Reports].[vw_tbl_TempPullPrintJobRetrieval]
WITH SCHEMABINDING
AS
SELECT ppjr.ActivityExecutionId,
       ppjr.JobEndDateTime,
       ppjr.JobStartDateTime,
       ppjr.SolutionType,
       ae.SessionId
  FROM dbo.PullPrintJobRetrieval ppjr
  INNER JOIN dbo.ActivityExecution ae ON ppjr.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_ActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivityDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.AssetId AS DeviceId,
       aeau.ProductName AS Product,
       aeau.FirmwareRevision AS FirmwareVersion,
       ae.ResultCategory AS GroupErrorMessage,
       ae.RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       COALESCE((pd.[Group] + ' ' + CASE pd.Color
                                        WHEN 1 THEN 'Color'
                                        WHEN 0 THEN 'Mono'
                                        ELSE ''
                                    END + ' ' + pd.MaxMediaSize), 'NA') AS DeviceCategory,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          u.AssetId,
                          sd.FirmwareRevision,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_ActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_ActivitySummary...'
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
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.ProductName AS Product,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ae.RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                   FROM reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
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
SELECT ss.STF_SessionId AS SessionId
	   ,ae.ActivityName
	   ,ae.ActivityType
	   ,ae.Status AS ActivityStatus
	   ,ae.StartDateTime AS ActivityStartDateTime
	   ,ae.EndDateTime AS ActivityEndDataTime
	   ,DATEDIFF(SECOND, ae.StartDateTime, ae.EndDateTime) AS ActivityDurationSeconds
	   ,aeau.ProductName AS Product
	   ,ISNULL(pd.[Platform], 'Jedi') AS DeviceType
	   ,ae.ResultMessage AS ErrorMessage
	   ,ae.ResultCategory AS GroupErrorMessage
       ,ae.RetryCount
       ,ate.TaskName
       ,ate.[Status] AS TaskStatus
       ,ate.StartDateTime AS TaskStartDateTime
       ,ate.EndDateTime AS TaskEndDateTime
       ,DATEDIFF(SECOND, ate.StartDateTime, ate.EndDateTime) AS TaskDurationSeconds
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ae.SessionId = ss.SessionId
  INNER JOIN dbo.ActivityTaskExecution ate ON ae.ActivityExecutionId = ate.ActivityExecutionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId
	  					,sd.ProductName
						,ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
	  				FROM  reports.vw_tbl_TempActivityExecutionAssetUsage u
					LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
  WHERE ae.ActivityName NOT LIKE 'SessionReset%'
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
SELECT ss.STF_SessionId AS SessionId
	  ,ae.ActivityName
	  ,ae.ActivityType
	  ,ae.StartDateTime as ActivityStartDateTime
	  ,ae.EndDateTime as ActivityEndDateTime
	  ,ae.Status AS UpdateType
	  ,aeau.ProductName Product
	  ,ae.ResultMessage AS ErrorMessage
	  ,ae.ResultCategory AS GroupErrorMessage
	  ,ae.RetryCount
	  ,ISNULL(pd.[Platform], 'Jedi') AS DeviceType
	  ,FLOOR((DATEDIFF(SECOND, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
	  ,ate.TaskName
	  ,ate.ExecutionPath
	  ,ate.[Status] as TaskStatus
	  ,ate.StartDateTime as TaskStartDateTime
	  ,ate.EndDateTime as TaskEndDateTime
	  ,DATEDIFF(SECOND, ate.StartDateTime, ate.EndDateTime) as TaskDurationSeconds
	  ,atse.StepName
	  ,atse.[Status] as StepStatus
	  ,atse.StartDateTime as StepStartDateTime
	  ,atse.EndDateTime as StepEndDateTime
	  ,DATEDIFF(SECOND, atse.StartDateTime, atse.EndDateTime) as StepDurationSeconds
  FROM dbo.ActivityExecution ae
  INNER  JOIN dbo.SessionSummary ss ON ae.SessionId = ss.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId
						 ,sd.ProductName
						 ,ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
					 FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
					 ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
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
	   aeau.ProductName AS Product,
	   ae.ResultMessage AS ErrorMessage,
	   ae.ResultCategory AS GroupErrorMessage,
	   ae.RetryCount,
	   ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
	   FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
	   ,ate.TaskName
	   ,ate.ExecutionPath
	   ,ate.[Status] as TaskStatus
	   ,ate.StartDateTime as TaskStartDateTime
	   ,ate.EndDateTime as TaskEndDateTime
	   ,DATEDIFF(s, ate.StartDateTime, ate.EndDateTime) as TaskDurationSeconds	   
	   ,se.StepName
	   ,se.[Status] as StepStatus
	   ,se.StartDateTime as StepStartDateTime
	   ,se.EndDateTime as StepEndDateTime
	   ,DATEDIFF(s, se.StartDateTime, se.EndDateTime) as StepDurationSeconds
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
						  sd.ProductName,
						  ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
					 FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
								LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
						) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
  LEFT OUTER JOIN dbo.ActivityTaskExecution ate on ae.ActivityExecutionId = ate.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityTaskStepExecution se on ate.ActivityTaskExecutionId = se.ActivityTaskExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_BrianDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianDetails...'
	DROP VIEW [Reports].[vw_rpt_BrianDetails]
END
PRINT 'Creating vw_rpt_BrianDetails View ...'
GO
--
-- This view is used in the 'Brian Details' report.
--
CREATE VIEW [Reports].[vw_rpt_BrianDetails]
WITH SCHEMABINDING
AS
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.AssetId AS DeviceId,
       aeau.ProductName AS Product,
       aeau.FirmwareRevision AS FirmwareVersion,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ae.RetryCount,
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
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          u.AssetId,
                          sd.FirmwareRevision,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON pjc.ActivityExecutionId = ae.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_BrianSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_BrianSummary...'
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
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ae.StartDateTime,
       ae.ActivityType,
       ae.Status AS UpdateType,
       aeau.ProductName AS Product,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ae.RetryCount,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivityDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendActivitySummary...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_DigitalSendReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_DigitalSendReportDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
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
	PRINT 'Dropping vw_rpt_DigitalSendReportSummary...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
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
	PRINT 'Dropping vw_rpt_ePrintReportDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
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
                                                         FROM Reports.vw_tbl_TempEPrintServerJob e2
                                                        WHERE e2.EPrintTransactionId = e1.EPrintTransactionId AND e2.PrintJobClientId IS NOT NULL AND e2.SessionId = e1.SessionId)) AS PrintJobClientId,
                          e1.SessionId,
                          e1.TransactionStatus
                     FROM Reports.vw_tbl_TempEPrintServerJob e1) epsj ON pjc.PrintJobClientId = epsj.PrintJobClientId
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

IF OBJECT_ID('Reports.vw_rpt_MOATActivityDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivityDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
GO

IF OBJECT_ID('Reports.vw_rpt_MOATActivitySummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_MOATActivitySummary...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
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
SELECT ss.STF_SessionId AS SessionId,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
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
WITH SCHEMABINDING
AS
SELECT ss.STF_SessionId AS SessionId,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN Reports.vw_tbl_TempProduct_CTE pcte ON ae.ActivityExecutionId = pcte.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pcte.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pcte.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = pcte.RevProduct
GO

IF OBJECT_ID('Reports.vw_rpt_PINPrintActivityReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PINPrintActivityReportDetails...'
	DROP VIEW [Reports].[vw_rpt_PINPrintActivityReportDetails]
END
GO
PRINT 'Creating vw_rpt_PINPrintActivityReportDetails View ...'
GO
--
-- This view is used in the 'PIN Print Activity Report Details' report.
--
CREATE VIEW [Reports].[vw_rpt_PINPrintActivityReportDetails]
WITH SCHEMABINDING
AS
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ppjr.JobStartDateTime,
       ppjr.JobEndDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       'PIN Print' AS UpdateType,
       aeau.AssetId AS DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
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
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  JOIN dbo.PinPrintJobRetrieval ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_PINPrintActivityReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PINPrintActivityReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PINPrintActivityReportSummary]
END
GO
PRINT 'Creating vw_rpt_PINPrintActivityReportSummary View ...'
GO
--
-- This view is used in the 'PIN Print Activity Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_PINPrintActivityReportSummary]
WITH SCHEMABINDING
AS
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ppjr.JobStartDateTime,
       ae.ActivityType,
       'PIN Print' AS UpdateType,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       ISNULL(pd.[Platform], 'Jedi') AS DeviceType,
       FLOOR((DATEDIFF(s, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS DurHour
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN Reports.vw_tbl_TempActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  JOIN dbo.PinPrintJobRetrieval ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId
GO

IF OBJECT_ID('Reports.vw_rpt_PrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PrintJobReportDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
						  sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
		     LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
		     ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
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
  LEFT OUTER JOIN dbo.ProductDetails pd ON aeau.ProductName = pd.Product
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
	PRINT 'Dropping vw_rpt_PrintJobReportSummary...'
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
SELECT ss.STF_SessionId AS SessionId,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
						  sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
		     LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
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
  LEFT OUTER JOIN dbo.ProductDetails pd ON aeau.ProductName = pd.Product
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ISNULL(ppjr.JobStartDateTime, ae.StartDateTime) AS StartDateTime,
       ISNULL(ppjr.JobEndDateTime, ae.EndDateTime) AS EndDateTime,
       ae.UserName,
       ae.HostName,
       ae.ActivityType,
       ppjr.SolutionType,
       pjc.[FileName],
       ae.RetryCount,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       aeau.AssetId AS DeviceId,
       aeau.ProductName AS Product,
       aeau.FirmwareRevision AS FirmwareVersion,
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
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
						  u.AssetId,
						  sd.ProductName,
						  sd.FirmwareRevision,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON aeau.ProductName = pd.Product
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.JobStartDateTime,
							tp.JobEndDateTime,
							tp.SolutionType,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                     FROM Reports.vw_tbl_TempPullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN (SELECT	tc.ActivityExecutionId,
							tc.FileName,
							ROW_NUMBER() OVER (PARTITION BY tc.SessionId, tc.ActivityExecutionId ORDER BY tc.JobStartDateTime) AS RowNumber
                     FROM Reports.vw_tbl_TempPrintJobClient tc) pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId AND pjc.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintActivityReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintActivityReportSummary...'
	DROP VIEW [Reports].[vw_rpt_PullPrintActivityReportSummary]
END
GO
PRINT 'Creating vw_rpt_PullPrintActivityReportSummary View ...'
GO
--
-- This view is used in the 'Pull Print Activity Report Summary' report.
--
CREATE VIEW [Reports].[vw_rpt_PullPrintActivityReportSummary]
WITH SCHEMABINDING
AS
SELECT ss.STF_SessionId AS SessionId,
       ae.ActivityName,
       ISNULL(ppjr.JobStartDateTime, ae.StartDateTime) AS StartDateTime,
       ae.ActivityType,
       ae.RetryCount,
       ae.Status AS UpdateType,
       ae.ResultMessage AS ErrorMessage,
       ae.ResultCategory AS GroupErrorMessage,
       aeau.ProductName AS Product,
       pd.[Platform]
  FROM dbo.ActivityExecution ae
  INNER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
 LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
						  sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.ProductDetails pd ON aeau.ProductName = pd.Product
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.JobStartDateTime,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                     FROM Reports.vw_tbl_TempPullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportDetails', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportDetails...'
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
SELECT ss.STF_SessionId AS SessionId,
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
       ae.RetryCount,
       aeau.AssetId AS DeviceId,
       aeau.FirmwareRevision AS FirmwareVersion,
       CASE
           WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
           ELSE aeau.ProductName
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
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          u.AssetId,
                          sd.FirmwareRevision,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN (SELECT	tp.ActivityExecutionId,
							tp.SolutionType,
							ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                     FROM Reports.vw_tbl_TempPullPrintJobRetrieval tp) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
  LEFT OUTER JOIN dbo.PhysicalDeviceJob pdj ON pdj.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = aeau.ProductName
 WHERE ae.UserName != 'Jawa' AND ae.Status IN ('Completed', 'Failed')
GO

IF OBJECT_ID('Reports.vw_rpt_PullPrintJobReportSummary', 'V') IS NOT NULL
BEGIN
	PRINT 'Dropping vw_rpt_PullPrintJobReportSummary...'
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
SELECT ss.STF_SessionId AS SessionId,
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
      ae.RetryCount,
      aeau.AssetId AS DeviceId,
      CASE
          WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
          ELSE aeau.ProductName
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
  LEFT OUTER JOIN (SELECT u.ActivityExecutionId,
                          u.AssetId,
                          sd.ProductName,
                          ROW_NUMBER() OVER (PARTITION BY u.SessionId, u.ActivityExecutionId ORDER BY u.AssetId) AS RowNumber
                     FROM Reports.vw_tbl_TempActivityExecutionAssetUsage u
					 LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = u.SessionId AND sd.DeviceId = u.AssetId
				  ) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId AND aeau.RowNumber = (ISNULL(ae.RetryCount, 0) + 1)
GO
