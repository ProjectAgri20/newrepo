USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_SessionsWithCounts]    Script Date: 06/24/2013 11:19:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_SessionsWithCounts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_SessionsWithCounts]
GO

USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_SessionsWithCounts]    Script Date: 06/24/2013 11:19:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		kyoungman
-- Create date: 5/10/2011
-- Description:	Returns a list of all Sessions with Activity and PrintJob counts
-- Updated: 5/15/2012 - Added ShutdownState to procedure. -aandrews
--          10/31/2012 - Added ExpirationDate and ScanJobCount. -bmyers
--			11/17/2012 - Modified counts to pull from ScalableTestDataLog table. -bmyers
--			1/24/2013 - Modified to use linked server connection for ScalableTestDataLog database. -bmyers
--			6/24/2013 - Added new shutdown columns. -bmyers
-- =============================================
CREATE PROCEDURE [dbo].[sel_SessionsWithCounts]
AS
BEGIN

	SET NOCOUNT ON;

	-- Create a temp table to hold the results from the ScalableTestDataLog stored procedure
	CREATE TABLE #Counts (SessionId VARCHAR(50), ActivityCount INT, PrintJobCount INT, ScanJobCount INT)
	INSERT INTO #Counts (SessionId, ActivityCount, PrintJobCount, ScanJobCount) EXEC STFData.ScalableTestDataLog.dbo.sel_SessionCounts

	SELECT
		s.SessionId, 
		s.[Status], 
		s.ShutdownState,
		s.StartDate,
		s.ExpirationDate,
		s.ScenarioName, 
		s.[Owner],
		s.ShutdownUser,
		s.ShutdownDate,
		c.ActivityCount,
		c.PrintJobCount,
		c.ScanJobCount
	FROM SessionInfo s
	INNER JOIN #Counts c on s.SessionId = c.SessionId
	ORDER BY s.StartDate desc
		
END

GO

