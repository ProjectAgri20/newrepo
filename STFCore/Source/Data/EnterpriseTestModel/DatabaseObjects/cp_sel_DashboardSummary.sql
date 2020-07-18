USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_DashboardSummary]    Script Date: 07/17/2012 11:12:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_DashboardSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_DashboardSummary]
GO

USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_DashboardSummary]    Script Date: 07/17/2012 11:12:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		kyoungman
-- Create date: 6/15/2012
-- Description:	Returns a list of VM usage information
-- Modified date:
-- Description:
-- =============================================
CREATE PROCEDURE [dbo].[sel_DashboardSummary]
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @now DATETIME
	SET @now = GETDATE()

	SELECT DISTINCT v.SessionId, 
		i.ScenarioName,		
		i.[Owner],
		i.Dispatcher,		
		i.StartDate,
		i.ProjectedEndDate,
		i.[Status],
		CONVERT(VARCHAR(10), DATEDIFF(HOUR, i.StartDate, @now)/24) + ' Days ' +  CONVERT(VARCHAR(20),DATEDIFF(HOUR, i.StartDate, @now)%(24)) + ' Hours' as TimeRunning,
		(SELECT COUNT(Name) FROM VirtualMachine WHERE SessionId = v.SessionId AND Name LIKE 'X%') AS XPCount,
		(SELECT COUNT(Name) FROM VirtualMachine WHERE SessionId = v.SessionId AND Name LIKE 'W%') AS W7Count
	FROM VirtualMachine v LEFT JOIN SessionInfo i on v.SessionId = i.SessionId
	WHERE v.SessionId IS NOT null
		
END

GO

