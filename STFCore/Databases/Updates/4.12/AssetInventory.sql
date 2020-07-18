USE [AssetInventory]
GO
----------------------------------------------------------------------------
--kyoungman 2/14/2018 - Add SimulatorType to DeviceSimulator table

-- Create temp table to persist exists flag between GO statements
CREATE TABLE #variables
(
    [exists] BIT NOT NULL
)
GO

-- Set the flag
INSERT INTO #variables VALUES (CONVERT(BIT,
    (
        SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = 'DeviceSimulator' AND COLUMN_NAME = 'SimulatorType'
    )))
GO

-- Add the column
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    ALTER TABLE dbo.DeviceSimulator ADD SimulatorType VARCHAR(50) NULL;
END
GO

-- Add default values, then set the new column to NOT NULL
IF EXISTS (SELECT [exists] from #variables WHERE [exists] = 0)
BEGIN
    UPDATE dbo.DeviceSimulator SET SimulatorType = 'Jedi' WHERE SimulatorType IS NULL;
    ALTER TABLE dbo.DeviceSimulator ALTER COLUMN SimulatorType VARCHAR(50) NOT NULL;
END
GO

-- Drop the temp table
IF OBJECT_ID('tempdb..#variables') IS NOT NULL
    DROP TABLE #variables
GO
----------------------------------------------------------------------------

--kyoungman 2/28/2018 - Change Directory & Email type to be more descriptive
UPDATE MonitorConfig SET MonitorType = 'OutputEmail' WHERE MonitorType = 'Email'
UPDATE MonitorConfig SET MonitorType = 'OutputDirectory' WHERE MonitorType = 'Directory'
GO
----------------------------------------------------------------------------

-- Ognomo Daniel Bakyono 03/28/2018 - Add Camera table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Camera]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Camera](
	[AssetId] [varchar](50) NOT NULL,
	[IPAddress] [varchar](50) NULL,
	[PrinterId] [varchar](50) NULL,
	[CameraServer] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Camera] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
