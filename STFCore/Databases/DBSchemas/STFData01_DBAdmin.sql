
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[DB_Space_Used]  ******/
USE [DBAdmin]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DB_Space_Used]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DB_Space_Used](
	[DB_Space_Used_Id] [int] IDENTITY(1,1) NOT NULL,
	[CollectDateTime] [datetime] NULL,
	[DatabaseName] [varchar](50) NULL,
	[DatabaseSize_MB] [decimal](19, 2) NULL,
	[UnallocatedSpace_MB] [decimal](19, 2) NULL,
	[ReservedSpace_KB] [decimal](19, 2) NULL,
	[DataSpace_KB] [decimal](19, 2) NULL,
	[IndexSize_KB] [decimal](19, 2) NULL,
	[UnusedSpace_KB] [decimal](19, 2) NULL,
 CONSTRAINT [PK_DB_Space_Used] PRIMARY KEY CLUSTERED 
(
	[DB_Space_Used_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'CollectDateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time that the database information was gathered.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'CollectDateTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'DatabaseName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the current database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'DatabaseSize_MB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Size of the current database in megabytes. Includes both data and log files.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'DatabaseSize_MB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'UnallocatedSpace_MB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Space in the database that has not been reserved for database objects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'UnallocatedSpace_MB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'ReservedSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space allocated by objects in the database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'ReservedSpace_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'DataSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space used by data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'DataSpace_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'IndexSize_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space used by indexes.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'IndexSize_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DB_Space_Used', N'COLUMN',N'UnusedSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space reserved for objects in the database, but not yet used.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DB_Space_Used', @level2type=N'COLUMN',@level2name=N'UnusedSpace_KB'
GO

/******  Object:  Table [dbo].[DBSchema_Change_Log]  ******/
USE [DBAdmin]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DBSchema_Change_Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DBSchema_Change_Log](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[EventTime] [datetime] NULL,
	[LoginName] [varchar](50) NULL,
	[UserName] [varchar](50) NULL,
	[DatabaseName] [varchar](50) NULL,
	[SchemaName] [varchar](50) NULL,
	[ObjectName] [varchar](50) NULL,
	[ObjectType] [varchar](50) NULL,
	[DDLCommand] [varchar](max) NULL,
 CONSTRAINT [PK_DBSchema_Change_Log] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[SchmaChangeNotification]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[SchmaChangeNotification]
   ON [DBAdmin].[dbo].[DBSchema_Change_Log]
   AFTER INSERT, UPDATE
   AS 
BEGIN

SET NOCOUNT ON;

DECLARE @mailbody NVARCHAR(MAX)
DECLARE @subject NVARCHAR(MAX)
DECLARE @tableHTML NVARCHAR(MAX)

SET @subject = ''STF Data 01 Schema Change Notification''

SET @tableHTML =
    N''<style type="text/css">
    #box-table
    {
        font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
        font-size: 12px;
        text-align: center;
        border-collapse: collapse;
        border-top: 7px solid #9baff1;
        border-bottom: 7px solid #9baff1;
    }
    #box-table th
    {
        font-size: 13px;
        font-weight: normal;
        background: #b9c9fe;
        border-right: 2px solid #9baff1;
        border-left: 2px solid #9baff1;
        border-bottom: 2px solid #9baff1;
        color: #039;
    }
    #box-table td
    {
        border-right: 1px solid #aabcfe;
        border-left: 1px solid #aabcfe;
        border-bottom: 1px solid #aabcfe;
        color: #669;
    }
    </style>'' +
    N''<h3>Last row from [DBAdmin].[dbo].[DBSchema_Change_Log]</h3>'' +
    N''<table id="box-table">'' +
    N''<tr>
        <th>RecordId</th>
        <th>EventTime</th>
        <th>LoginName</th>
        <th>UserName</th>
        <th>DatabaseName</th>
        <th>SchemaName</th>
        <th>ObjectName</th>
        <th>ObjectType</th>
        <th>DDLCommand</th>
    </tr>'' +
    CAST((SELECT TOP 1
                 RecordId AS ''td'', ''''
                ,EventTime AS ''td'', ''''
                ,LoginName AS ''td'', ''''
                ,UserName AS ''td'', ''''
                ,DatabaseName AS ''td'', ''''
                ,SchemaName AS ''td'', ''''
                ,ObjectName AS ''td'', ''''
                ,ObjectType AS ''td'', ''''
                ,DDLCommand AS ''td''
          FROM DBSchema_Change_Log
          ORDER BY RecordId DESC
          FOR XML PATH(''tr''), TYPE) AS NVARCHAR(MAX)) +
    N''</table>''

SET @mailbody = ''<html><body>'' +
                @tableHTML +
                ''<p /><p>Verify if the database replication process or documentation files need to be modified.</p>'' +
                ''</body></html>''

--print @mailbody

EXEC msdb.dbo.sp_send_dbmail
    @profile_name = ''DBChange_Configuration'',
    @body = @mailbody,
    @body_format = ''HTML'',
    @recipients = ''stf.developers@hp.com'',
    @subject = ''STF Data 01 Schema Change Notification'';
END

' 
GO
ALTER TABLE [dbo].[DBSchema_Change_Log] ENABLE TRIGGER [SchmaChangeNotification]
GO

/******  Object:  Table [dbo].[Tables_Space_Used]  ******/
USE [DBAdmin]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tables_Space_Used]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tables_Space_Used](
	[Tables_Space_Used_Id] [int] IDENTITY(1,1) NOT NULL,
	[CollectDateTime] [datetime] NULL,
	[TableName] [varchar](50) NULL,
	[NumRows] [int] NULL,
	[ReservedSpace_KB] [int] NULL,
	[DataSpace_KB] [int] NULL,
	[IndexSize_KB] [int] NULL,
	[UnusedSpace_KB] [int] NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'CollectDateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time the table information was gathered.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'CollectDateTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'TableName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the table for which space usage information was requested.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'TableName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'NumRows'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of rows existing in the table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'NumRows'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'ReservedSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of reserved space for table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'ReservedSpace_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'DataSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space used by data in table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'DataSpace_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'IndexSize_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space used by indexes in table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'IndexSize_KB'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Tables_Space_Used', N'COLUMN',N'UnusedSpace_KB'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of space reserved for table but not yet used.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tables_Space_Used', @level2type=N'COLUMN',@level2name=N'UnusedSpace_KB'
GO

/*****************************************************************************
 * INDEXES                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/******  Object:  Function [dbo].[udfGetTableInfo]  ******/
USE [DBAdmin]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udfGetTableInfo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- ============================================================================
-- Author:      Gary Parham
-- Create date: 2019-04-22
-- Description: Returns table size information for the past 30 days on the
--              specified table.
-- ============================================================================
CREATE FUNCTION [dbo].[udfGetTableInfo] (@DataLogTableName nvarchar(50))
RETURNS TABLE 
AS
RETURN 
(
    SELECT CAST(CollectDateTime AS DATE) AS CollectDateTime, NumRows, DataSpace_KB
    FROM Tables_Space_Used
    WHERE TableName = @DataLogTableName AND
          CollectDateTime BETWEEN DATEADD(DAY, -30, GETDATE()) AND GETDATE()
)
' 
END
GO

/*****************************************************************************
 * VIEWS                                                                     *
 *****************************************************************************/

/*****************************************************************************
 * TRIGGERS                                                                  *
 *****************************************************************************/

/******  Object:  Trigger [SchmaChangeNotification]  ******/

/*****************************************************************************
 * USERS                                                                     *
 *****************************************************************************/

/******  Object:  User [report_viewer]  ******/
USE [DBAdmin]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'report_viewer')
CREATE USER [report_viewer] FOR LOGIN [report_viewer] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [report_viewer]
GO
