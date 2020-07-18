USE [ScalableTestDataLog]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS  
                WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ActivityExecution' and COLUMN_NAME = 'ResourceInstanceId')
BEGIN
    ALTER TABLE ActivityExecution ADD [ResourceInstanceId] [nvarchar](50) NOT NULL CONSTRAINT [ResourceInstanceIdConstraint]  DEFAULT ('')
END
GO
