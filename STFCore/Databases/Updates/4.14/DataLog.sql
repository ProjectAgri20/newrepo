USE [ScalableTestDataLog]
GO

-----------------------------------------------------------------------
-- 7/19/2018 kyoungman -- Add composite primary key for SessionConfiguration
-- Create temp table to persist exists flag between GO statements

CREATE TABLE #variables
(
    [exists] BIT NOT NULL
)
GO

-- Set the value
INSERT INTO #variables VALUES (CONVERT(BIT,
    (
        SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = 'SessionConfiguration' AND COLUMN_NAME = 'RunOrder'
    )))
GO

-- Add 3 new columns
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE SessionConfiguration ADD RunOrder TINYINT NULL;
END
GO

IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE SessionConfiguration ADD ScenarioStart DATETIME NULL;
END
GO

IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE SessionConfiguration ADD ScenarioEnd DATETIME NULL;
END
GO

-- Add default values, then set the new column to NOT NULL
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    UPDATE SessionConfiguration SET RunOrder = 1;
    ALTER TABLE SessionConfiguration ALTER COLUMN RunOrder TINYINT NOT NULL;
END
GO

-- Drop the current primary key constraint.
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE SessionConfiguration DROP CONSTRAINT PK_SessionConfiguration;
END
GO

-- Add new primary key constraint.
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE SessionConfiguration ADD CONSTRAINT PK_SessionConfiguration
    PRIMARY KEY (SessionId, RunOrder);
END
GO

-- Drop the temp table
IF OBJECT_ID('tempdb..#variables') IS NOT NULL
    DROP TABLE #variables
GO
-----------------------------------------------------------------------

-- danderson Add new column, FirmwareBundleVersion, to table SessionDevice
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SessionDevice' and COLUMN_NAME = 'FirmwareBundleVersion')
BEGIN
    ALTER TABLE SessionDevice
    ADD FirmwareBundleVersion nvarchar(50) NULL
END
GO
-----------------------------------------------------------------------

-- 8/6/2018 kyoungman -- Rename SessionConfig to SessionScenario
-- 8/14/2018 gparham -- Added guard blocks around the rename procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionConfiguration]') AND type IN (N'U'))
BEGIN
    EXEC sp_rename 'SessionConfiguration', 'SessionScenario'
END
GO
-----------------------------------------------------------------------

-- bmyers -- Add field to PrintServerJob table
-- gparham -- Added BEGIN and END statements to help the script work with the STB installer
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintServerJob' and COLUMN_NAME = 'EndStatus')
BEGIN
    ALTER TABLE [PrintServerJob] ADD [EndStatus] varchar(255) NULL;
END
GO
