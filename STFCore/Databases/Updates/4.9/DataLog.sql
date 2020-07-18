/*===========================================================================*/
--
-- Merge the table EnterpriseTest.dbo.SessionInfo into
-- ScalableTestDataLog.dbo.SessionSummary.
--
/*===========================================================================*/

SET XACT_ABORT ON;
GO

USE [ScalableTestDataLog]
GO

-- Start the transaction.
BEGIN TRANSACTION

-- Add columns to the SessionSummary table.
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SessionSummary')
BEGIN
    -- Notes column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'Notes')
    BEGIN
        ALTER TABLE SessionSummary ADD Notes varchar(MAX) NULL;
    END
    -- ShutdownState column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'ShutdownState')
    BEGIN
        ALTER TABLE SessionSummary ADD ShutdownState varchar(50) NOT NULL CONSTRAINT DF_SessionSummary_ShutdownState DEFAULT 'Unknown';
    END
    -- ProjectedEndDateTime column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'ProjectedEndDateTime')
    BEGIN
        ALTER TABLE SessionSummary ADD ProjectedEndDateTime datetime NULL;
    END
    -- ExpirationDateTime column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'ExpirationDateTime')
    BEGIN
        ALTER TABLE SessionSummary ADD ExpirationDateTime datetime NOT NULL DEFAULT DATEADD(week, (2), GETDATE());
        -- ALTER TABLE SessionSummary ADD DEFAULT DATEADD(week, (2), GETDATE()) FOR ExpirationDateTime;
    END
    -- ShutdownUser column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'ShutdownUser')
    BEGIN
        ALTER TABLE SessionSummary ADD ShutdownUser varchar(50) NULL;
    END
    -- ShutdownDateTime column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionSummary' AND COLUMN_NAME = 'ShutdownDateTime')
    BEGIN
        ALTER TABLE SessionSummary ADD ShutdownDateTime datetime NULL;
    END
END
ELSE
    THROW 51000, 'ScalableTestDataLog table does not exist in destination database instance.', 1;

-- Merge the data from SessionInfo into SessionSummary.
DECLARE @mergeSql nvarchar(2048) = 'UPDATE SessionSummary SET Notes = si.Notes, ShutdownState = si.ShutdownState, ProjectedEndDateTime = si.ProjectedEndDate, ExpirationDateTime = si.ExpirationDate, ShutdownUser = si.ShutdownUser, ShutdownDateTime = si.ShutdownDate FROM EnterpriseTest.dbo.SessionInfo AS si WHERE SessionSummary.SessionId = si.SessionId';
EXEC sys.sp_executesql @query = @mergeSql;

-- Drop the SessionInfo table.
USE [EnterpriseTest]

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SessionInfo')
BEGIN
    DROP TABLE [SessionInfo];
END

-- Commit the transaction.
COMMIT;
GO

USE [ScalableTestDataLog]
GO

/****** Object:  Index [Idx_ActivityExecutionPerformance_ActivityExecutionId_EventIndex] ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPerformance') AND name = 'Idx_ActivityExecutionPerformance_ActivityExecutionId_EventIndex')
BEGIN
    CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_ActivityExecutionId_EventIndex] ON [dbo].[ActivityExecutionPerformance]
    (
	    [ActivityExecutionId] ASC,
	    [EventIndex] ASC
    )
    INCLUDE ( 	[EventLabel],
	    [EventDateTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

/****** Object:  Index [Idx_ActivityExecutionPerformance_EventLabel] ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPerformance') AND name = 'Idx_ActivityExecutionPerformance_EventLabel')
BEGIN
    CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_EventLabel] ON [dbo].[ActivityExecutionPerformance]
    (
	    [EventLabel] ASC
    )
    INCLUDE ( 	[ActivityExecutionId],
	    [EventIndex]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

/****** Object:  Index [Idx_ActivityExecutionPerformance_SessionId] ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPerformance') AND name = 'Idx_ActivityExecutionPerformance_SessionId')
BEGIN
    CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_SessionId] ON [dbo].[ActivityExecutionPerformance]
    (
	    [SessionId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

/****** Object:  Index [Idx_ActivityExecution_SessionId] ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.ActivityExecution') AND name = 'Idx_ActivityExecution_SessionId')
BEGIN
    CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_SessionId] ON [dbo].[ActivityExecution]
    (
	    [SessionId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO


USE [ScalableTestDataLog]
GO

-- Add columns to the SessionProductAssoc table.
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SessionProductAssoc')
BEGIN
    -- Name column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionProductAssoc' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE SessionProductAssoc ADD Name varchar(50) NULL;
    END
    -- Vendor column
    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionProductAssoc' AND COLUMN_NAME = 'Vendor')
    BEGIN
        ALTER TABLE SessionProductAssoc ADD Vendor nvarchar(100) NULL;
    END
   
END
ELSE
    THROW 51000, 'ScalableTestDataLog table does not exist in destination database instance.', 1;




/*===========================================================================*/
--
-- Remove TraceLog table and related stored procedures.
--
/*===========================================================================*/

USE [ScalableTestDataLog]
GO

/****** Object:  StoredProcedure [dbo].[del_SessionData]    Script Date: 9/1/2017 1:45:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		BJ Myers
-- Create date: 1/24/2013
-- Description:	Deletes all session data for the given session id.
-- Updated:		
--	 6/4/2014 - Added removal of TraceLog data
--  10/3/2014 - Revise TraceLog data cleanup
--  6/15/2017 - Walk through a few of the larger tables and delete 10,000 records
--              at a time to reduce the possibility of filling up the tran log.
--   9/1/2017 - Removed TraceLog table
-- =============================================
ALTER PROCEDURE [dbo].[del_SessionData] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

    -- ActivityExecutionPerformance
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPerformance'))
    BEGIN
        SELECT TOP 1 * FROM ActivityExecutionPerformance
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM ActivityExecutionPerformance
            WHERE SessionId = @sessionId
        END
    END

    -- ActivityExecution
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecution'))
    BEGIN
        SELECT TOP 1 * FROM ActivityExecution
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM ActivityExecution
            WHERE SessionId = @sessionId
        END
    END

    -- ActivityExecutionPacing
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecutionPacing'))
    BEGIN
        SELECT TOP 1 * FROM ActivityExecutionPacing
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM ActivityExecutionPacing
            WHERE SessionId = @sessionId
        END
    END

    -- ActivityExecutionAssetUsage
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecutionAssetUsage'))
    BEGIN
        SELECT TOP 1 * FROM ActivityExecutionAssetUsage
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM ActivityExecutionAssetUsage
            WHERE SessionId = @sessionId
        END
    END

    -- ActivityExecutionServerUsage
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ActivityExecutionServerUsage'))
    BEGIN
        SELECT TOP 1 * FROM ActivityExecutionServerUsage
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM ActivityExecutionServerUsage
            WHERE SessionId = @sessionId
        END
    END

    -- DeviceMemorySnapshot
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.DeviceMemorySnapshot'))
    BEGIN
        SELECT TOP 1 * FROM DeviceMemorySnapshot
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (1000) FROM DeviceMemorySnapshot
            WHERE SessionId = @sessionId
        END
    END

    -- SessionSummary
	DELETE FROM SessionSummary WHERE SessionId = @sessionId

END


GO


/****** Object:  StoredProcedure [dbo].[del_ExpiredTraceLogData]    Script Date: 9/1/2017 1:52:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[del_ExpiredTraceLogData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[del_ExpiredTraceLogData]
GO

/****** Object:  StoredProcedure [dbo].[del_ExpiredTraceLogData_Experimental]    Script Date: 9/1/2017 1:53:01 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[del_ExpiredTraceLogData_Experimental]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[del_ExpiredTraceLogData_Experimental]
GO

/****** Object:  StoredProcedure [dbo].[ins_TraceLog]    Script Date: 9/1/2017 1:57:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ins_TraceLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ins_TraceLog]
GO

/****** Object:  Table [dbo].[TraceLog]    Script Date: 9/1/2017 1:57:57 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TraceLog]') AND type in (N'U'))
DROP TABLE [dbo].[TraceLog]
GO


/*===========================================================================*/
--
-- Increase the size of the FirmwareDatecode field in the SessionDevice table.
--
/*===========================================================================*/

USE [ScalableTestDataLog]
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionDevice' and COLUMN_NAME = 'FirmwareDatecode')
BEGIN
   ALTER TABLE SessionDevice ALTER COLUMN FirmwareDatecode VARCHAR(20)
END
GO

/*===========================================================================*/
--
-- Fill in values for the Name & Vendor columns in the SessionProductAssoc table.
--
/*===========================================================================*/

USE [ScalableTestDataLog]
GO

IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.SessionProductAssoc')) AND
   EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionProductAssoc' AND COLUMN_NAME = 'Name') AND
   EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionProductAssoc' AND COLUMN_NAME = 'Vendor')
BEGIN
    UPDATE [ScalableTestDataLog].[dbo].[SessionProductAssoc]
    SET [dbo].[SessionProductAssoc].[Name] = [EnterpriseTest].[dbo].[AssociatedProduct].[Name]
       ,[dbo].[SessionProductAssoc].[Vendor] = [EnterpriseTest].[dbo].[AssociatedProduct].[Vendor]
    FROM [ScalableTestDataLog].[dbo].[SessionProductAssoc]
    INNER JOIN [EnterpriseTest].[dbo].[AssociatedProduct] ON [ScalableTestDataLog].[dbo].[SessionProductAssoc].[EnterpriseTestAssociatedProductId] = [EnterpriseTest].[dbo].[AssociatedProduct].[AssociatedProductId]
    WHERE [ScalableTestDataLog].[dbo].[SessionProductAssoc].[Name] IS NULL AND [ScalableTestDataLog].[dbo].[SessionProductAssoc].[Vendor] IS NULL
END
