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
-- * Check if a column exists in a specific table.
-- IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.<TABLE_NAME_HERE>') and name = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/

/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypeTotalsErrors]    Script Date: 2/3/2017 9:49:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for a metadatatype/updatetype pair
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
--              02/02/17  parham - Added no table locking to the query
-- =============================================
ALTER PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors] 
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
    WITH(NOLOCK)
	WHERE
		owa.SessionId = @sessionId
		and owa.ActivityType = @activityType
		and owa.Status = @updateType
	GROUP BY
		owa.ResultMessage
END

GO
------------------------------------------------------------------------------------------

--kyoungman 2/8/2017 Added missing columns to vw_rpt_PrintJobReportDetails

 IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'vw_rpt_PrintJobReportDetails')
 BEGIN
     DROP VIEW [Reports].[vw_rpt_PrintJobReportDetails]
 END
 GO

CREATE VIEW [Reports].[vw_rpt_PrintJobReportDetails]
WITH SCHEMABINDING
AS
SELECT 
		FLOOR((DATEDIFF(s,ss.StartDateTime, ae.StartDateTime)-480)/3600)+1 AS DurHour,
		pjc.SessionId,
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
		END AS RenderTime,
		CASE  WHEN pjc.JobStartDateTime > LastByteReceivedDateTime  THEN ''  
			WHEN pjc.JobStartDateTime IS NULL THEN ''  
			ELSE 
			CASE WHEN LastByteReceivedDateTime = '' THEN 
			CASE WHEN PrintEndDateTime IS NULL THEN '' 
				ELSE (CAST(DATEDIFF(s, (pjc.JobStartDateTime), (PrintEndDateTime)) AS float) / 3600.0 / 24.0) END
			ELSE (CAST(DATEDIFF(s, pjc.JobStartDateTime, LastByteReceivedDateTime) AS float) / 3600 / 24) 
				END
			END AS LastPageOutTime
	FROM dbo.PrintJobClient pjc
	LEFT OUTER JOIN dbo.ActivityExecution ae ON pjc.ActivityExecutionId = ae.ActivityExecutionId
	LEFT OUTER JOIN (SELECT ActivityExecutionId,
							MIN(AssetId) AssetId
						FROM dbo.ActivityExecutionAssetUsage
						GROUP BY ActivityExecutionId
						) aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
	LEFT OUTER JOIN dbo.SessionDevice sd ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
	LEFT OUTER JOIN dbo.SessionSummary ss ON ss.SessionId = ae.SessionId
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

