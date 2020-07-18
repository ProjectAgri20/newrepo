USE [AssetInventory]
GO
----------------------------------------------------------------------------
--kyoungman 5/9/2018 - Add stored proc to clear expired SessionIds from VirtualMachineReservation
USE [AssetInventory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[upd_ClearExpiredSessions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[upd_ClearExpiredSessions]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Create date: 5/9/2018
-- Description:	Clears reservation information for the given SessionId
-- Updated:		
-- =============================================
CREATE PROCEDURE [dbo].[upd_ClearExpiredSessions] (
    @sessionId varchar(50)
    )
AS
BEGIN
    SET NOCOUNT ON;
	UPDATE VirtualMachineReservation set SessionId = null, PlatformUsage = '', LastUpdated = GETDATE() where SessionId = @sessionId
END
GO
----------------------------------------------------------------------------
