USE [EnterpriseTest]
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.SessionInfo') and name = 'STFVersion')
BEGIN
ALTER TABLE [dbo].[SessionInfo] ADD [STFVersion] [varchar](50) DEFAULT NULL

END