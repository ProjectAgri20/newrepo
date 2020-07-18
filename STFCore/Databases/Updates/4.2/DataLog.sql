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


-- danderson  Add thumbnail and timestamp to TriageData
IF EXISTS
	(
		SELECT * 
		FROM INFORMATION_SCHEMA.TABLES 
		WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'TriageData'
	)
	BEGIN
		ALTER TABLE TriageData ADD Thumbnail varbinary(MAX) NULL, TriageDateTime DateTime NULL;
	END
GO

-- fdelagarza 7/29/2016  Initialize New TriageData TriageDateTime column values using ActivityExecution and ActivityExecutionRetries Tables.
IF COL_LENGTH('dbo.TriageData', 'TriageDateTime') IS NOT NULL
BEGIN
	--- Update TriageData records that do not have retries using ae.StartDateTime
	UPDATE [dbo].[TriageData]
	   SET [TriageDateTime] = tdnrt.StartDateTime
	 FROM ( SELECT td.TriageDataId, ae.StartDateTime
			FROM TriageData td
			INNER JOIN ActivityExecution ae ON ae.ActivityExecutionId = td.ActivityExecutionId
			LEFT OUTER JOIN ActivityExecutionRetries aer ON aer.ActivityExecutionId = td.ActivityExecutionId
			WHERE aer.ActivityExecutionId is NULL
	 ) tdnrt
	 WHERE TriageData.TriageDateTime IS NULL AND TriageData.TriageDataId = tdnrt.TriageDataId;

	--- Update First TriageData records that have retries using ae.StartDateTime
	UPDATE [dbo].[TriageData]
	   SET [TriageDateTime] = td1sttry.StartDateTime
	 FROM ( SELECT td.ActivityExecutionId, td.Reason, ae.StartDateTime, MIN(td.TriageDataId) TriageDataId
			FROM TriageData td
			INNER JOIN ActivityExecution ae ON ae.ActivityExecutionId = td.ActivityExecutionId
			LEFT OUTER JOIN ActivityExecutionRetries aer ON aer.ActivityExecutionId = td.ActivityExecutionId
			WHERE aer.ActivityExecutionId Is Not NULL
			GROUP BY td.ActivityExecutionId, td.Reason, ae.StartDateTime
	 ) td1sttry
	 WHERE TriageData.TriageDateTime IS NULL AND TriageData.TriageDataId = td1sttry.TriageDataId;

	--- Update rest of TriageData records that have retries using the corresponding retry StartDateTime by cronological order
	UPDATE [dbo].[TriageData]
	   SET [TriageDateTime] = tdRetries.RetryStartDateTime
	 FROM ( SELECT td.ActivityExecutionId, td.Reason, td.TriageDataId, aer.RetryStartDateTime
	FROM ( SELECT td.ActivityExecutionId, td.Reason, td.TriageDataId, ROW_NUMBER() OVER(PARTITION BY td.ActivityExecutionId, td.Reason ORDER BY td.ActivityExecutionId, td.Reason, td.TriageDataId) AS RowNumber
			FROM TriageData td
			WHERE td.TriageDateTime Is NULL
			) td
			INNER JOIN (
			SELECT aer.ActivityExecutionId, aer.ActivityExecutionRetriesId, aer.RetryStartDateTime, ROW_NUMBER() OVER(PARTITION BY aer.ActivityExecutionId ORDER BY aer.ActivityExecutionId, aer.ActivityExecutionRetriesId) AS RowNumber
			FROM ActivityExecutionRetries aer
			) aer ON aer.ActivityExecutionId=td.ActivityExecutionId AND aer.RowNumber = td.RowNumber
	 ) tdRetries
	 WHERE TriageData.TriageDateTime IS NULL AND TriageData.TriageDataId = tdRetries.TriageDataId;
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------

-- bmyers 7/15/16  Drop stored procedure for Task/Step chart
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_TaskTotals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_TaskTotals]
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

-- bmyers 7/15/16  Add some length to the ActivityExecutionPerformance.EventLabel field
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]') AND type in (N'U'))
BEGIN
	-- Drop view that refers to this column
	IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_tbl_TempActivityExecutionPerformance]'))
	DROP VIEW [Reports].[vw_tbl_TempActivityExecutionPerformance]

	-- Alter column
	ALTER TABLE ActivityExecutionPerformance ALTER COLUMN EventLabel varchar(255) NULL

	-- Re-add view
	EXEC dbo.sp_executesql @statement = N'
	--
	-- This view is used in the ''Pull Print Job Report Summary'' and ''Pull Print Job Report Details'' reports.
	--
	CREATE VIEW [Reports].[vw_tbl_TempActivityExecutionPerformance]
	WITH SCHEMABINDING
	AS
	SELECT aep.SessionId,
		   aep.ActivityExecutionId,
		   aep.EventDateTime,
		   aep.EventLabel
	  FROM dbo.ActivityExecutionPerformance aep
	' 
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

-- bmyers 7/15/16  Migrate data from Task/Step tables to performance table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityTaskExecution]') AND type in (N'U'))
BEGIN
	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, TaskName + '-TaskBegin', NULL, StartDateTime FROM ActivityTaskExecution

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, TaskName + '-TaskEnd', NULL, EndDateTime FROM ActivityTaskExecution

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, TaskName + '.' + StepName + '-StepBegin', NULL, StartDateTime FROM ActivityTaskStepExecution

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, TaskName + '.' + StepName + '-StepEnd', NULL, StartDateTime FROM ActivityTaskStepExecution
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

--bmyers 7/15/16  Drop Task/Step views and tables
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_ActivityTaskDetails]'))
DROP VIEW [Reports].[vw_rpt_ActivityTaskDetails]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_ActivityTaskStepDetails]'))
DROP VIEW [Reports].[vw_rpt_ActivityTaskStepDetails]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_ActivityTaskStepExecution]'))
DROP VIEW [Reports].[vw_rpt_ActivityTaskStepExecution]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityTaskStepExecution]') AND type in (N'U'))
DROP TABLE [dbo].[ActivityTaskStepExecution]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityTaskExecution]') AND type in (N'U'))
DROP TABLE [dbo].[ActivityTaskExecution]
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

--bmyers 7/15/16  Migrate timestamps from DigitalSendJobInput table to performance table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]') AND type in (N'U'))
BEGIN
	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, 'ActivityStart', NULL, ActivityStartDateTime FROM DigitalSendJobInput WHERE ActivityStartDateTime IS NOT NULL

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, 'ScanStart', NULL, ScanStartDateTime FROM DigitalSendJobInput WHERE ScanStartDateTime IS NOT NULL

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, 'ScanEnd', NULL, ScanEndDateTime FROM DigitalSendJobInput WHERE ScanEndDateTime IS NOT NULL

	INSERT INTO [dbo].[ActivityExecutionPerformance]([ActivityExecutionPerformanceId],[SessionId],[ActivityExecutionId],[EventLabel],[EventIndex],[EventDateTime])
	SELECT NEWID(), SessionId, ActivityExecutionId, 'JobEnd', NULL, JobEndDateTime FROM DigitalSendJobInput WHERE JobEndDateTime IS NOT NULL
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

--bmyers 7/15/16  Update report views to get digital send timestamps from the performance table


/****** Object:  View [Reports].[vw_rpt_DigitalSendReportDetails]    Script Date: 7/18/2016 1:32:43 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_DigitalSendReportDetails]'))
BEGIN
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportDetails]
	EXEC dbo.sp_executesql @statement = N'



--
-- This view is used in the ''Digital Send Report Details'' report.
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
           WHEN CHARINDEX(''DFS'', ae.ActivityName) > 0 THEN ''Folder DFS''
           ELSE dsji.ScanType
       END AS Scantype,
       CASE dsji.Ocr
           WHEN 1 THEN ''OCR_1''
           WHEN 0 THEN CASE
                           WHEN (CHARINDEX(''OCR'', ae.ActivityName) > 0) AND ((CHARINDEX(''Non'', ae.ActivityName) !> 0) OR (CHARINDEX(''NOCR'', ae.ActivityName) !> 0)) THEN ''OCR''
                           ELSE ''Non-OCR''
                       END
           ELSE ''''
       END AS OCR,
       dsji.DeviceId,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       pd.Model,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = ''Scan'' THEN RTRIM(pd.MaxMediaSize) + '' Network Scanner''
           ELSE pd.[Group] + '' '' + CASE pd.Color
                                       WHEN 1 THEN ''Color''
                                       WHEN 0 THEN ''Mono''
                                       ELSE ''''
                                   END + '' '' + RTRIM(pd.MaxMediaSize) + '' '' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN ''Color''
           WHEN 0 THEN ''Mono''
           ELSE ''''
       END AS DeviceColor,
       pd.MaxMediaSize,
       dsji.Sender,
       dsji.[PageCount],
       dsji.DestinationCount,
       aep.ActivityStart,
       aep.ScanStart,
       aep.ScanEnd,
       aep.JobEnd,
       dsji.JobEndStatus,
       dsjo.FileSentDateTime,
       dsjo.FileReceivedDateTime,
       CASE
           WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
           ELSE dsjo.FileSentDateTime
       END AS FileArrived,
       dsjo.[FileName],
       dsjo.FileSizeBytes AS OutputfileSizeBytes,
       dsjo.FileLocation,
       CASE
           WHEN dsjo.FileLocation IS NULL THEN ''None''
           ELSE CASE
                    WHEN dsji.ScanType = ''Folder Multi'' THEN RIGHT(dsjo.FileLocation, 1)
                    ELSE ''1''
                END
       END AS Destination,
       dsjo.[PageCount] AS DestPages,
       dsjo.[PageCount] - dsji.[PageCount] AS PageDelta,
       CASE
           WHEN dsji.FilePrefix IS NOT NULL THEN CASE
                                                     WHEN dsjo.[FileName] IS NOT NULL THEN CASE
                                                                                               WHEN (dsjo.[PageCount] - dsji.[PageCount]) != 0 THEN CASE
                                                                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN ''Page Delta''
                                                                                                                                                        ELSE ''FAIL''
                                                                                                                                                    END
                                                                                               ELSE CASE
                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN ''Pass''
                                                                                                        ELSE ''FAIL''
                                                                                                    END
                                                                                           END
                                                     ELSE CASE
                                                              WHEN CHARINDEX(''Complete'', ae.Status) > 0 THEN ''Missing''
                                                              ELSE ''N/A''
                                                          END
                                                 END
           ELSE ''Orphaned''
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

	   DATEDIFF(SECOND, aep.ActivityStart, aep.ScanStart) / 3600.0 / 24.0 AS Act_2_scanStart,

	   DATEDIFF(SECOND, aep.ScanStart, aep.ScanEnd) / 3600.0 / 24.0 AS ScanTime,

	   CASE
           WHEN aep.ScanEnd > (CASE
                                            WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                            ELSE dsjo.FileSentDateTime
                                        END) THEN 0
           ELSE DATEDIFF(SECOND, aep.ScanEnd, CASE
                                                           WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                                           ELSE dsjo.FileSentDateTime
                                                       END) / 3600.0 / 24.0
       END AS scanEnd_2_fileSent,

	   DATEDIFF(SECOND, aep.ScanEnd, dssj.CompletionDateTime) / 3600.0 / 24.0 AS scanEnd_2_DSSTime,

       DATEDIFF(SECOND, aep.ActivityStart, 
	   CASE WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
            ELSE dsjo.FileSentDateTime
        END) / 3600.0 / 24.0 AS TotalTime,

       CASE
           WHEN dssj.CompletionDateTime < aep.ActivityStart THEN ''No''
           ELSE ''Yes''
       END AS ValidDSSTime,

       CASE
           WHEN dssj.DeviceModel IS NULL THEN ''No''
           ELSE CASE
                    WHEN CHARINDEX(RIGHT(pd.Model, 4), dssj.DeviceModel) > 0 THEN ''No''
                    ELSE ''Yes''
                END
       END AS [Duplicate?],
       CASE
           WHEN CHARINDEX(''SIM'', dsji.DeviceId) > 0 THEN 1
           ELSE 0
       END AS [SIM?],
       FLOOR((DATEDIFF(SECOND, ss.StartDateTime, aep.ActivityStart) - 480) / 3600) + 1 AS DurHour
  FROM dbo.DigitalSendJobInput dsji
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  
  LEFT OUTER JOIN (SELECT pvt.ActivityExecutionId,
                            pvt.DeviceLockBegin,
                            pvt.AppButtonPress,
                            pvt.AuthType,
                            pvt.AuthenticationBegin,
                            pvt.AuthenticationEnd,
                            pvt.ActivityStart,
                            pvt.ScanStart,
							pvt.ScanEnd,
							pvt.JobEnd,
                            pvt.DeviceSignOutBegin,
                            pvt.DeviceSignOutEnd
                       FROM (SELECT SessionId, ActivityExecutionId, CASE WHEN LEFT(EventLabel, 9)=''AuthType='' THEN ''AuthType'' ELSE EventLabel END AS EventLabel, EventDateTime
                               FROM Reports.vw_tbl_TempActivityExecutionPerformance
                       ) AS p
                       PIVOT (MAX(p.EventDateTime) FOR p.EventLabel IN ([DeviceLockBegin], [AppButtonPress], [AuthType], [AuthenticationBegin], [AuthenticationEnd], [ActivityStart], [ScanStart], [ScanEnd],[JobEnd],
                                                                        [DeviceSignOutBegin], [DeviceSignOutEnd])) AS pvt) aep ON ae.ActivityExecutionId = aep.ActivityExecutionId

  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN dbo.DigitalSendServerJob dssj ON dsjo.[FileName] = dssj.[FileName]
                                                   AND (dssj.JobType != ''SendFax'' OR dssj.CompletionDateTime IS NULL OR dssj.CompletionDateTime > aep.ScanStart)
                                                   AND (CHARINDEX(RIGHT(pd.Model, 10), dssj.DeviceModel) > 0 OR dssj.DeviceModel IS NULL)
                                                   AND (dsjo.[PageCount] = dssj.ScannedPages OR dssj.ScannedPages IS NULL)
                                                   AND (DATEDIFF(SECOND, dsjo.FileReceivedDateTime, dssj.CompletionDateTime) < 3600 OR dssj.CompletionStatus IS NULL)




' 

END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  View [Reports].[vw_rpt_DigitalSendReportSummary]    Script Date: 7/18/2016 1:32:43 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_DigitalSendReportSummary]'))
BEGIN
	DROP VIEW [Reports].[vw_rpt_DigitalSendReportSummary]
	EXEC dbo.sp_executesql @statement = N'

--
-- This view is used in the ''Digital Send Report Summary'' report.
--
CREATE VIEW [Reports].[vw_rpt_DigitalSendReportSummary]
WITH SCHEMABINDING
AS
SELECT ISNULL(dsji.SessionId, dsjo.SessionId) AS SessionId,
       ae.Status AS UpdateType,
       dsji.FilePrefix,
       CASE
           WHEN CHARINDEX(''DFS'', ae.ActivityName) > 0 THEN ''Folder DFS''
           ELSE dsji.ScanType
       END AS Scantype,
       CASE dsji.Ocr
           WHEN 1 THEN ''OCR''
           WHEN 0 THEN CASE
                           WHEN (CHARINDEX('' OCR'', ae.ActivityName) > 0) OR (CHARINDEX(''Non'', ae.ActivityName) !> 0) THEN ''OCR''
                           ELSE ''Non-OCR''
                       END
           ELSE ''''
       END AS OCR,
       sd.ProductName AS Product,
       sd.FirmwareRevision AS FirmwareVersion,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = ''Scan'' THEN RTRIM(pd.MaxMediaSize) + '' Network Scanner''
           ELSE pd.[Group] + '' '' + CASE pd.Color
                                       WHEN 1 THEN ''Color''
                                       WHEN 0 THEN ''Mono''
                                       ELSE ''''
                                   END + '' '' + RTRIM(pd.MaxMediaSize) + '' '' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       dsji.[PageCount],
       aep.ActivityStart,
       aep.ScanStart,
       dsji.JobEndStatus,
       CASE
           WHEN dsjo.FileLocation IS NULL THEN ''None''
           ELSE CASE
                    WHEN dsji.ScanType = ''Folder Multi'' THEN RIGHT(dsjo.FileLocation, 1)
                    ELSE ''1''
                END
       END AS Destination,
       CASE
           WHEN dsji.FilePrefix IS NOT NULL THEN CASE
                                                     WHEN dsjo.[FileName] IS NOT NULL THEN CASE
                                                                                               WHEN (dsjo.[PageCount] - dsji.[PageCount]) != 0 THEN CASE
                                                                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN ''Page Delta''
                                                                                                                                                        ELSE ''FAIL''
                                                                                                                                                    END
                                                                                               ELSE CASE
                                                                                                        WHEN dsjo.ErrorMessage IS NULL THEN ''Pass''
                                                                                                        ELSE ''FAIL''
                                                                                                    END
                                                                                           END
                                                     ELSE CASE
                                                              WHEN CHARINDEX(''Complete'', ae.Status) > 0 THEN ''Missing''
                                                              ELSE ''N/A''
                                                          END
                                                 END
           ELSE ''Orphaned''
       END AS ValidationResult,
       dssj.CompletionStatus,
       dssj.DssVersion,
       CASE
           WHEN aep.ScanEnd > (CASE
                                            WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                            ELSE dsjo.FileSentDateTime
                                        END) THEN 0
           ELSE DATEDIFF(SECOND, aep.ScanEnd, CASE
                                                           WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
                                                           ELSE dsjo.FileSentDateTime
                                                       END) / 3600.0 / 24.0
       END AS scanEnd_2_fileSent,
       CASE
           WHEN dssj.CompletionDateTime < aep.ActivityStart THEN ''No''
           ELSE ''Yes''
       END AS ValidDSSTime,
       CASE
           WHEN dssj.DeviceModel IS NULL THEN ''No''
           ELSE CASE
                    WHEN CHARINDEX(RIGHT(pd.Model, 4), dssj.DeviceModel) > 0 THEN ''No''
                    ELSE ''Yes''
                END
       END AS [Duplicate?],
       CASE
           WHEN CHARINDEX(''SIM'', dsji.DeviceId) > 0 THEN 1
           ELSE 0
       END AS [SIM?],
       FLOOR((DATEDIFF(SECOND, ss.StartDateTime, aep.ActivityStart) - 480) / 3600) + 1 AS DurHour
  FROM dbo.DigitalSendJobInput dsji
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.ActivityExecution ae ON ae.ActivityExecutionId = dsji.ActivityExecutionId

  LEFT OUTER JOIN (SELECT pvt.ActivityExecutionId,
                            pvt.DeviceLockBegin,
                            pvt.AppButtonPress,
                            pvt.AuthType,
                            pvt.AuthenticationBegin,
                            pvt.AuthenticationEnd,
                            pvt.ActivityStart,
                            pvt.ScanStart,
							pvt.ScanEnd,
							pvt.JobEnd,
                            pvt.DeviceSignOutBegin,
                            pvt.DeviceSignOutEnd
                       FROM (SELECT SessionId, ActivityExecutionId, CASE WHEN LEFT(EventLabel, 9)=''AuthType='' THEN ''AuthType'' ELSE EventLabel END AS EventLabel, EventDateTime
                               FROM Reports.vw_tbl_TempActivityExecutionPerformance
                       ) AS p
                       PIVOT (MAX(p.EventDateTime) FOR p.EventLabel IN ([DeviceLockBegin], [AppButtonPress], [AuthType], [AuthenticationBegin], [AuthenticationEnd], [ActivityStart], [ScanStart], [ScanEnd],[JobEnd],
                                                                        [DeviceSignOutBegin], [DeviceSignOutEnd])) AS pvt) aep ON ae.ActivityExecutionId = aep.ActivityExecutionId

  LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = sd.ProductName
  LEFT OUTER JOIN dbo.DigitalSendServerJob dssj ON dsjo.[FileName] = dssj.[FileName]
                                                   AND (dssj.JobType != ''SendFax'' OR dssj.CompletionDateTime IS NULL OR dssj.CompletionDateTime > aep.ActivityStart)
                                                   AND (CHARINDEX(RIGHT(pd.Model, 10), dssj.DeviceModel) > 0 OR dssj.DeviceModel IS NULL)
                                                   AND (dsjo.[PageCount] = dssj.ScannedPages OR dssj.ScannedPages IS NULL)
                                                   AND (DATEDIFF(SECOND, dsjo.FileReceivedDateTime, dssj.CompletionDateTime) < 3600 OR dssj.CompletionStatus IS NULL)


' 
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  View [Reports].[vw_rpt_MOATActivityDetails]    Script Date: 7/18/2016 1:36:00 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Reports].[vw_rpt_MOATJobReportDetails]'))
BEGIN
	DROP VIEW [Reports].[vw_rpt_MOATJobReportDetails]
	DROP VIEW [Reports].[vw_tbl_TempProduct_CTE]

	EXEC dbo.sp_executesql @statement = N'

--
-- This view is used in the ''MOAT Job Report Summary'' and ''MOAT Job Report Details'' reports.
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
                                WHEN CHARINDEX(''eprint'', ae.ActivityName) > 0 THEN LTRIM(REPLACE(REPLACE(ae.ActivityName, SUBSTRING(ae.ActivityName, CHARINDEX(''LTL'', ae.ActivityName), 10), ''''), LEFT(ae.ActivityName, CHARINDEX(''-'', ae.ActivityName)), ''''))
                                ELSE ''N/A''
                            END) AS RevProduct,
       dsji.Ocr,
       dsji.ScanType,
	   aep.ScanStart,
       aep.ScanEnd,
       ppjr.SolutionType,
       ISNULL(dsjo.FilePrefix, pjc.[FileName]) AS FilePrefix,
       ISNULL(dsjo.FileSizeBytes, pjc.FileSizeBytes) AS FileSizeBytes,
       dsjo.FileSentDateTime,
       dsjo.FileReceivedDateTime,
       CASE
           WHEN CHARINDEX(''email'', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
           ELSE dsjo.FileSentDateTime
       END AS JobReceivedDateTime,
       dsji.[PageCount] AS PageCountIn,
       dsjo.[PageCount] AS PageCountOut,
       DATEDIFF(SECOND, dsji.[PageCount], dsjo.[PageCount]) / 3600.0 / 24.0 AS PageDelta,
       pjc.PrintJobClientId,
       pjc.PrintQueue,
       pjc.PrintType,
       CASE
           WHEN (ae.ActivityType = ''ePrint'' OR ae.ActivityType = ''HPCR'' OR CHARINDEX(''Auth'', ae.ActivityType) > 0) THEN ae.StartDateTime
           ELSE CASE
                    WHEN CHARINDEX(''pull'', ae.ActivityName) > 0 THEN ppjr.JobStartDateTime
                    ELSE ISNULL(aep.ScanStart, pjc.JobStartDateTime)
                END
       END AS StartDateTime,
       CASE
           WHEN (ae.ActivityType = ''ePrint'' OR ae.ActivityType = ''HPCR'' OR CHARINDEX(''Auth'', ae.ActivityType) > 0) THEN ae.EndDateTime
           ELSE CASE
                    WHEN CHARINDEX(''pull'', ae.ActivityName) > 0 THEN ppjr.JobEndDateTime
                    ELSE ISNULL(aep.ScanEnd, pjc.JobEndDateTime)
                END
       END AS EndDateTime
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau ON aeau.ActivityExecutionId = ae.ActivityExecutionId
  LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
  LEFT OUTER JOIN dbo.DigitalSendJobInput dsji ON ae.ActivityExecutionId = dsji.ActivityExecutionId
  LEFT OUTER JOIN (SELECT pvt.ActivityExecutionId,
                        pvt.DeviceLockBegin,
                        pvt.AppButtonPress,
                        pvt.AuthType,
                        pvt.AuthenticationBegin,
                        pvt.AuthenticationEnd,
                        pvt.ActivityStart,
                        pvt.ScanStart,
						pvt.ScanEnd,
						pvt.JobEnd,
                        pvt.DeviceSignOutBegin,
                        pvt.DeviceSignOutEnd
                    FROM (SELECT SessionId, ActivityExecutionId, CASE WHEN LEFT(EventLabel, 9)=''AuthType='' THEN ''AuthType'' ELSE EventLabel END AS EventLabel, EventDateTime
                            FROM Reports.vw_tbl_TempActivityExecutionPerformance
                            ) AS p
                    PIVOT (MAX(p.EventDateTime) FOR p.EventLabel IN ([DeviceLockBegin], [AppButtonPress], [AuthType], [AuthenticationBegin], [AuthenticationEnd], [ActivityStart], [ScanStart], [ScanEnd],[JobEnd],
                                                                    [DeviceSignOutBegin], [DeviceSignOutEnd])) AS pvt) aep ON ae.ActivityExecutionId = aep.ActivityExecutionId
  LEFT OUTER JOIN dbo.PullPrintJobRetrieval ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId
  FULL OUTER JOIN dbo.DigitalSendJobOutput dsjo ON dsji.FilePrefix = dsjo.FilePrefix
  LEFT OUTER JOIN dbo.PrintJobClient pjc ON ae.ActivityExecutionId = pjc.ActivityExecutionId


' 

	EXEC dbo.sp_executesql @statement = N'

--
-- This view is used in the ''MOAT Job Report Details'' report.
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
           WHEN ae.ActivityType = ''Printing'' THEN CASE
                                                      WHEN CHARINDEX(''SAFE'', aesu.ServerName) > 0 THEN ''Printing-SafeCom''
                                                      ELSE CASE
                                                               WHEN CHARINDEX(''HPAC'', aesu.ServerName) > 0 THEN ''Printing-HPAC''
                                                               ELSE CASE
                                                                        WHEN CHARINDEX(''Citrix'', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX(''LTL'', pcte.PrintQueue) > 0 THEN ''Printing-Citrix-Remote''
                                                                                                                              ELSE ''Printing-Citrix-Local''
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN ''Printing-Local''
                                                                                 ELSE ''Printing-Remote''
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN CHARINDEX(''an'', ae.ActivityType) > 0 THEN ''Scanning''
                    ELSE CASE
                             WHEN CHARINDEX(''pull'', ae.ActivityName) > 0 THEN (''Pulling-'' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityGroup,
       CASE
           WHEN ae.ActivityType = ''Printing'' THEN CASE
                                                      WHEN CHARINDEX(''SAFE'', aesu.ServerName) > 0 THEN ''Printing-SafeCom''
                                                      ELSE CASE
                                                               WHEN CHARINDEX(''HPAC'', aesu.ServerName) > 0 THEN ''Printing-HPAC''
                                                               ELSE CASE
                                                                        WHEN CHARINDEX(''Citrix'', pcte.PrintType) > 0 THEN CASE
                                                                                                                              WHEN CHARINDEX(''LTL'', pcte.PrintQueue) > 0 THEN ''Printing-Citrix-Remote''
                                                                                                                              ELSE ''Printing-Citrix-Local''
                                                                                                                          END
                                                                        ELSE CASE
                                                                                 WHEN HostName = psj.PrintServer THEN ''Printing-Local''
                                                                                 ELSE ''Printing-Remote''
                                                                             END
                                                                    END
                                                           END
                                                  END
           ELSE CASE
                    WHEN pcte.ScanType IS NOT NULL THEN CASE pcte.OCR
                                                            WHEN 1 THEN CASE
                                                                            WHEN CHARINDEX(''DFS'', ae.ActivityName) > 0 THEN (''ScanTo'' + pcte.ScanType + '' DFS '' + ''_OCR'')
                                                                            ELSE (''ScanTo'' + pcte.ScanType + ''_OCR'')
                                                                        END
                                                            WHEN 0 THEN CASE
                                                                            WHEN CHARINDEX(''DFS'', ae.ActivityName) > 0 THEN (''ScanTo'' + pcte.ScanType + '' DFS '' + ''_Non-OCR'')
                                                                            ELSE (''ScanTo'' + pcte.ScanType + ''_Non-OCR'')
                                                                        END
                                                            ELSE CASE
                                                                     WHEN CHARINDEX(''DFS'', ae.ActivityName) > 0 THEN (''ScanTo'' + pcte.ScanType + '' DFS '')
                                                                     ELSE (''ScanTo'' + pcte.ScanType)
                                                                 END
                                                        END
                    ELSE CASE
                             WHEN CHARINDEX(''pull'', ae.ActivityName) > 0 THEN (''Pulling-'' + pcte.SolutionType)
                             ELSE ActivityType
                         END
                END
       END AS ActivityDetail,
       CASE
           WHEN CHARINDEX(''HP Universal'', psj.PrintDriver) > 0 THEN (''HP UPD '' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
           ELSE CASE
                    WHEN CHARINDEX(''Safecom'', pcte.PrintQueue) > 0 THEN pcte.PrintQueue
                    ELSE psj.PrintDriver
                END
       END AS PrintDriver,
       ISNULL(CASE
                  WHEN CHARINDEX(''HP Universal'', psj.PrintQueue) > 0 THEN (''HP UPD '' + RIGHT(psj.PrintQueue, LEN(psj.PrintQueue) - 22))
                  ELSE psj.PrintQueue
              END, CASE
                       WHEN CHARINDEX(''HP Universal'', pcte.PrintQueue) > 0 THEN (''HP UPD '' + RIGHT(pcte.PrintQueue, LEN(pcte.PrintQueue) - 22))
                       ELSE pcte.PrintQueue
                   END) AS PrintQueue,
       CASE
           WHEN ae.ActivityType = ''Printing'' THEN CASE
                                                      WHEN (CHARINDEX(''Citrix'', ae.HostName) > 0 AND CHARINDEX(''PrintServer'', aesu.ServerName) > 0) THEN ''CitrixRemote''
                                                      ELSE pcte.PrintType
                                                  END
           ELSE ''N/A''
       END AS PrintQueueType,
       CASE
           WHEN CHARINDEX(''eprint'', ae.ActivityName) > 0 THEN SUBSTRING(ae.ActivityName, CHARINDEX(''LTL'', ae.ActivityName), 9)
           ELSE ISNULL(pcte.DeviceId, pcte.DeviceId)
       END AS DeviceID,
       pcte.RevProduct,
       pcte.FirmwareVersion,
       pd.[Platform],
       CASE
           WHEN pd.[Function] = ''Scan'' THEN (RTRIM(pd.MaxMediaSize) + '' Network Scanner'')
           ELSE pd.[Group] + '' '' + CASE pd.Color
                                      WHEN 1 THEN ''Color''
                                      WHEN 0 THEN ''Mono''
                                      ELSE ''''
                                   END + '' '' + RTRIM(pd.MaxMediaSize) + '' '' + pd.[Function]
       END AS DeviceCategory,
       pd.[Group],
       CASE pd.Color
           WHEN 1 THEN ''Color''
           WHEN 0 THEN ''Mono''
           ELSE ''''
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
           WHEN CHARINDEX(''an'', ae.ActivityType) > 0 THEN DATEDIFF(SECOND, pcte.StartDateTime, pcte.JobReceivedDateTime) / 3600.0 / 24.0
           ELSE DATEDIFF(SECOND, pcte.StartDateTime, pcte.EndDateTime) / 3600.0 / 24.0
       END AS JobTime
  FROM dbo.ActivityExecution ae
  LEFT OUTER JOIN Reports.vw_tbl_TempProduct_CTE pcte ON ae.ActivityExecutionId = pcte.ActivityExecutionId
  LEFT OUTER JOIN dbo.ActivityExecutionServerUsage aesu ON pcte.ActivityExecutionId = aesu.ActivityExecutionId
  LEFT OUTER JOIN dbo.PrintServerJob psj ON pcte.PrintJobClientId = psj.PrintJobClientId
  LEFT OUTER JOIN dbo.ProductDetails pd ON pd.Product = pcte.RevProduct


' 
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------------

--bmyers 7/15/16  Remove timestamp columns from DigitialSendJobInput table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]') AND type in (N'U'))
ALTER TABLE DigitalSendJobInput DROP COLUMN ActivityStartDateTime, ScanStartDateTime, ScanEndDateTime, JobEndDateTime
GO

-- parham 7/28/2016  Add a new column to the SessionSummary table to hold the comma-delimited list of selected tags.
IF COL_LENGTH('SessionSummary', 'Tags') IS NULL
BEGIN
	ALTER TABLE dbo.SessionSummary ADD Tags VARCHAR(255) NULL;
END
