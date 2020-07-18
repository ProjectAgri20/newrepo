USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[upd_ReserveVMsByPlatformContiguous]    Script Date: 07/17/2012 11:14:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_ScenarioPrintQueueUsage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_ScenarioPrintQueueUsage]
GO

USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[upd_ReserveVMsByPlatformContiguous]    Script Date: 07/17/2012 11:14:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <unknown>
-- Create date: <unknown>
-- Description:	Returns a list of all Scenarios used by the passed-in Framework Server
-- Updated: 2/14/2014 - Changed parameter name @sessionId to @serverId
-- =============================================
CREATE PROC [dbo].[sel_ScenarioPrintQueueUsage] ( @serverId varchar(50) )
AS
BEGIN
    SELECT
        es.Name AS Scenario,
        vr.Name AS VirtualResource,
        vrm.ResourceType,
        vrm.Name AS MetadataDescription,
        vrm.MetadataType,
        fs.HostName AS PrintServer,
        rpq.Name AS PrintQueue
    FROM
        FrameworkServer fs inner join RemotePrintQueue rpq on fs.ServerId = rpq.PrintServerId
        INNER JOIN MetadataResourceUsage mru ON CAST(rpq.PrintQueueId AS VARCHAR(255)) = mru.ResourceId 
        INNER JOIN VirtualResourceMetadata vrm ON mru.VirtualResourceMetadataId = vrm.VirtualResourceMetadataId
        INNER JOIN VirtualResource vr ON vrm.VirtualResourceId = vr.VirtualResourceId
        INNER JOIN EnterpriseScenario es ON vr.EnterpriseScenarioId = es.EnterpriseScenarioId
    WHERE 
        fs.ServerId = @serverId
	
END


GO
