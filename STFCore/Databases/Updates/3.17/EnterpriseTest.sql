USE [EnterpriseTest]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
            WHERE ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_SCHEMA = 'dbo' and ROUTINE_NAME = 'sel_SessionsWithCounts')
BEGIN
    DROP PROCEDURE [dbo].[sel_SessionsWithCounts]
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'SessionInfo' AND COLUMN_NAME = 'ArchiveState')
BEGIN
    ALTER TABLE [dbo].[SessionInfo] ADD [ArchiveState] [varchar](50) NOT NULL DEFAULT ('Active')

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Active, Delete, Archive', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SessionInfo', @level2type=N'COLUMN', @level2name=N'ArchiveState'
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'PluginSetting')
BEGIN
    CREATE TABLE [dbo].[PluginSetting](
        [Plugin] [varchar](50) NOT NULL,
        [Name] [varchar](50) NOT NULL,
        [Value] [nvarchar](max) NOT NULL,
        [Description] [varchar](255) NULL,
     CONSTRAINT [PK_PluginSetting] PRIMARY KEY CLUSTERED 
    (
        [Plugin] ASC,
        [Name] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SystemSetting 
                WHERE [Type] = 'FrameworkSetting' AND [Name]='SessionRemovalDelay_Days')
BEGIN
    INSERT INTO [dbo].[SystemSetting]
                      ([Type]
                      ,[Name]
                      ,[Value]
                      ,[Description])
                VALUES
                      (N'FrameworkSetting'
                      ,N'SessionRemovalDelay_Days'
                      ,N'7'
                      ,N'The number of days past the expiration date to delay before deleting the session records.')
END
GO
