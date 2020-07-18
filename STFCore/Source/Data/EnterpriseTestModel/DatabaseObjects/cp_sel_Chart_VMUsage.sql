USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_Chart_VMUsage]    Script Date: 07/17/2012 11:11:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_VMUsage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_VMUsage]
GO

USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[sel_Chart_VMUsage]    Script Date: 07/17/2012 11:11:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		kyoungman
-- Create date: 6/15/2012
-- Description:	Returns a list of VM usage counts
-- Modified date:
-- Description:
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_VMUsage]
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @now DATETIME
	SET @now = GETDATE()

	SELECT v.SessionId, 
		COUNT(v.Name)as VMCount
	FROM VirtualMachine v LEFT JOIN SessionInfo i on v.SessionId = i.SessionId
	WHERE v.SessionId IS NOT null GROUP BY v.SessionId
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachine v WHERE SessionId IS null AND UsageState != 'Available' GROUP BY v.UsageState
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachine v WHERE SessionId IS null AND UsageState = 'Available' GROUP BY v.UsageState
		
END
GO

