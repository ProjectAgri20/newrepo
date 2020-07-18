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

/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotalsDetails]    Script Date: 3/17/2016 2:29:09 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityErrorTotalsDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails]
GO

/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotalsDetails]    Script Date: 3/17/2016 2:29:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityErrorTotalsDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails] AS' 
END
GO



-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a summary of error messages starting with a specific prefix
-- Edited:		11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- 				02/04/16  fdelagarza, Updated for new STF 4.0 Log DB Schema
-- =============================================
ALTER PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails] 
	@sessionId nvarchar(50) = 0,
	@status nvarchar(50) = 0,
	@category nvarchar(255) = 0
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
		and owa.Status = @status
		and owa.ResultCategory = @category
	GROUP BY
		owa.ResultMessage
END



GO


