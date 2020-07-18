USE [AssetInventory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  
               WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'License')
BEGIN
    CREATE TABLE [dbo].[License](
        [LicenseId] [uniqueidentifier] NOT NULL,
        [ServerId] [uniqueidentifier] NOT NULL,
        [Solution] [varchar](50) NOT NULL,
        [SolutionVersion] [varchar](50) NOT NULL,
        [InstallationKey] [varchar](50) NULL,
        [Seats] [int] NOT NULL,
        [ExpirationDate] [datetime] NOT NULL,
        [Owner] [varchar](50) NOT NULL,
        [ExpirationNoticeDays] [int] NOT NULL,
     CONSTRAINT [PK_Licence] PRIMARY KEY CLUSTERED 
    (
        [LicenseId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]    
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  
                WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'LicenseKey')
BEGIN
    CREATE TABLE [dbo].[LicenseKey](
        [KeyId] [UNIQUEIDENTIFIER] NOT NULL,
        [LicenseId] [UNIQUEIDENTIFIER] NOT NULL,
        [KeyName] [VARCHAR](50) NOT NULL,
        [Key] [VARCHAR](100) NOT NULL,
     CONSTRAINT [PK_LicenseKey] PRIMARY KEY CLUSTERED 
    (
        [KeyId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[LicenseKey]  WITH CHECK ADD  CONSTRAINT [FK_LicenseKey_Licence] FOREIGN KEY([LicenseId])
    REFERENCES [dbo].[License] ([LicenseId])

    ALTER TABLE [dbo].[LicenseKey] CHECK CONSTRAINT [FK_LicenseKey_Licence]
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'LicenseOwner')
BEGIN
    CREATE TABLE [dbo].[LicenseOwner](
        [LicenseId] [UNIQUEIDENTIFIER] NOT NULL,
        [Contact] [VARCHAR](50) NOT NULL,
     CONSTRAINT [PK_LicenseOwner] PRIMARY KEY CLUSTERED 
    (
        [LicenseId] ASC,
        [Contact] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[LicenseOwner]  WITH CHECK ADD  CONSTRAINT [FK_LicenseOwner_License] FOREIGN KEY([LicenseId])
    REFERENCES [dbo].[License] ([LicenseId])

    ALTER TABLE [dbo].[LicenseOwner] CHECK CONSTRAINT [FK_LicenseOwner_License]
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS  
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Asset' AND COLUMN_NAME = 'Capability')
BEGIN
    ALTER TABLE Asset
    ADD Capability INT NOT NULL DEFAULT(0)

    DECLARE @sql NVARCHAR(2048) = 
    'DELETE Asset WHERE AssetType = ''CardReader'';' 
    + 'UPDATE Asset SET Capability = 1 WHERE AssetType = ''VirtualPrinter'';'
    + 'UPDATE Asset SET Capability = 6 WHERE AssetType = ''DeviceSimulator'';'
    + 'UPDATE Asset SET Capability = (
                                    CASE 
                                        WHEN p.PrinterType = ''MFP'' THEN 7
                                        WHEN p.PrinterType = ''SFP'' THEN 5
                                        WHEN p.PrinterType = ''DS'' THEN 6
                                        ELSE 1
                                    END)
        FROM Asset a INNER JOIN Printer p ON a.AssetId = p.AssetId
        WHERE a.AssetType = ''Printer'';'

    EXEC sys.sp_executesql @query = @sql;
END
GO

