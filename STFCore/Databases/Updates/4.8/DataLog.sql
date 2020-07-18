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
-- * Check if a column exists in a specific table. (Same syntax works for views also)
-- IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '<TABLE_NAME_HERE>' and COLUMN_NAME = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/

/****** Object:  StoredProcedure [dbo].[del_SessionData]    Script Date: 6/15/2017 1:29:35 PM ******/
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

	-- Delete the TraceLog data related to the session
	-- The amount of trace log data being deleted can get very large and fill up the tran log
	-- so break up into more manageable chunks (10,000 rows)

	-- first seed the rowcount so that it will attempt the first deletion
	select top 1 * from TraceLog 
	-- keep deleting in chunks until we're through
	WHILE (@@ROWCOUNT > 0) BEGIN
		BEGIN TRANSACTION deleteTracelog
			DELETE TOP(10000) FROM TraceLog WHERE SessionId = @sessionId
		COMMIT TRANSACTION deleteTracelog
	CONTINUE END

	EXEC dbo.del_ExpiredTraceLogData	
END

GO

