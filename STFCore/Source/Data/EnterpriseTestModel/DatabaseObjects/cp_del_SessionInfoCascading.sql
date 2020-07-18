USE [EnterpriseTest]
GO
/****** Object:  StoredProcedure [dbo].[del_SessionInfoCascading]    Script Date: 01/24/2013 14:00:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[del_SessionInfoCascading]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[del_SessionInfoCascading]
GO

USE [EnterpriseTest]
GO
/****** Object:  StoredProcedure [dbo].[del_SessionInfoCascading]    Script Date: 01/24/2013 14:00:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Kelly Youngman
-- Create date: 4/6/2011
-- Description:	Deletes a SessionInfo record and all associated OfficeWorkerActivityData records
--              Also clears the PrintJobLog table.
-- Updated:		2/16/2011 - VirtualMachineInfo and VirtualResourceInfo tables deleted. -bmyers
--				8/1/2012 - Added removal of data from EventLog and PerfMonLog tables - kyoungman
--				9/27/2012 - Added removal of data from ScanJobLog table. -bmyers
--				11/2/2012 - Clear out session-specific data from the VirtualMachine table - kyoungman
--				11/17/2012 - Changed references to tables that were moved to ScalableTestDataLog. -bmyers
--				1/24/2013 - Modified to use linked server connection for ScalableTestDataLog database. -bmyers
--				8/21/2014 - Removed the call to reset VirtualMachine columns since that table has moved to AssetInventory - kyoungman
-- =============================================
CREATE PROCEDURE [dbo].[del_SessionInfoCascading] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC STFData.ScalableTestDataLog.dbo.del_SessionData @sessionId
	DELETE SessionInfo WHERE SessionId = @sessionId

END





GO

