USE [AssetInventory]
GO

---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop Endpoint Responder assets and table

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EndpointResponder]') AND type in (N'U'))
DROP TABLE [dbo].[EndpointResponder]
GO

DELETE FROM Asset WHERE AssetType = 'EndpointResponder'


---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop Robot assets and table

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Robot]') AND type in (N'U'))
DROP TABLE [dbo].[Robot]
GO

DELETE FROM Asset WHERE AssetType = 'Robot'


---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop unused RemotePrintQueue.DevicePlatform column

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RemotePrintQueue_DeviceType]') AND type = 'D')
ALTER TABLE [dbo].[RemotePrintQueue] DROP CONSTRAINT [DF_RemotePrintQueue_DeviceType]
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RemotePrintQueue' AND COLUMN_NAME = 'DevicePlatform')
ALTER TABLE RemotePrintQueue DROP COLUMN DevicePlatform
GO


---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop unused FrameworkServer.DatabaseHostName column

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'DatabaseHostName')
ALTER TABLE FrameworkServer DROP COLUMN DatabaseHostName
GO


---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop unused VirtualMachineReservation.OsType column

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualMachineReservation' AND COLUMN_NAME = 'OsType')
ALTER TABLE VirtualMachineReservation DROP COLUMN OsType
GO


---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop unused upd_ClearExpiredSessions stored procedure
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[upd_ClearExpiredSessions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[upd_ClearExpiredSessions]
GO


---------------------------------------------------------------------------------------------------------
-- bmyers -- Create constraint to prevent overlapping asset reservations

IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] DROP CONSTRAINT [CK_DuplicateReservations]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_OverlappingReservationExists]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[udf_OverlappingReservationExists]
GO

-- Checks for the existence of asset reservations that overlap with the proposed reservation.
-- Intended for use with the CK_DuplicateReservation constraint on the AssetReservation table.
CREATE FUNCTION [udf_OverlappingReservationExists]
(
    @ReservationId AS UNIQUEIDENTIFIER,
    @AssetId AS VARCHAR(50),
    @Start AS DATETIME,
    @End AS DATETIME
)
RETURNS BIT
AS
BEGIN
    DECLARE @retval BIT
    IF EXISTS
    (
        SELECT * FROM AssetReservation
        WHERE @ReservationId <> ReservationId
          AND @AssetId = AssetId
          AND @Start < [End]
          AND @End > [Start]
    )
        SET @retval=1
    ELSE
        SET @retval=0
    RETURN @retval
END
GO

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation]  WITH NOCHECK ADD  CONSTRAINT [CK_DuplicateReservations] CHECK  (([dbo].[udf_OverlappingReservationExists]([ReservationId],[AssetId],[Start],[End])=(0)))
GO

IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] CHECK CONSTRAINT [CK_DuplicateReservations]
GO




---------------------------------------------------------------------------------------------------------
