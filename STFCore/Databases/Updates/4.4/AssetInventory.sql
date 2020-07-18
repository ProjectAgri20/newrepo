USE [AssetInventory]
GO
----------------------------------------------------------------------------
--dsnyder 10/11/2016: Adding a column to the FrameworkServer table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].FrameworkServer') AND name = 'Description')
BEGIN	
	ALTER TABLE [dbo].[FrameworkServer] ADD Description VARCHAR(255) NULL SET ANSI_PADDING OFF
END
GO

--dsnyder 10/11/2016: Adding a column 'Password' to the Printer table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].Printer') AND name = 'Password')
BEGIN	
	ALTER TABLE [dbo].[Printer] ADD Password VARCHAR(50) NULL 
END
GO

--dsnyder 10/11/2016: Adding a column 'Password' to the DeviceSimulator table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].DeviceSimulator') AND name = 'Password')
BEGIN	
	ALTER TABLE [dbo].[DeviceSimulator] ADD Password VARCHAR(50) NULL 
END
GO

--kyoungman 11/3/2016: Disable the foreign key constraint on Badge table, allowing nulls to be inserted.
ALTER TABLE Badge
ALTER COLUMN BadgeBoxId uniqueidentifier NULL
GO
ALTER TABLE Badge NOCHECK CONSTRAINT FK_Badge_BadgeBox
