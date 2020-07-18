USE [EnterpriseTest]
GO

/********* MANUAL SERVER CONFIGURATION *********

The following steps must be performed on STFSystem0X and STFData0X:
	Open Control Panel -> Administrative Tools -> Component Services.
	Navigate to Component Services\Computers\My Computer\Distributed Transaction Coordinator\Local DTC.
	Right-click Local DTC and select Properties.
	Check the following boxes:
		Network DTC Access
		Allow Inbound
		Allow Outbound
		Mutual Authentication Required
	Click OK and accept the prompt to restart the DTC service.

Detailed version here:
http://technet.microsoft.com/en-us/library/cc753510%28WS.10%29.aspx


The remainder of this script will need to be executed as SA from the EnterpriseTest database.

*/


-- Set the appropriate server name here
DECLARE @datasrc VARCHAR(4000)
SET @datasrc = N'STFData01'		-- Production
--SET @datasrc = N'STFData02'	-- Beta
--SET @datasrc = N'STFData03'	-- Development





/****** Object:  LinkedServer [STFDATA]    Script Date: 01/24/2013 13:20:48 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'STFDATA', @srvproduct=N'sql_server', @provider=N'SQLNCLI10', @datasrc=@datasrc
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'STFDATA',@useself=N'False',@locallogin=NULL,@rmtuser=NULL,@rmtpassword=NULL
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'STFDATA',@useself=N'False',@locallogin=N'enterprise_admin',@rmtuser=N'enterprise_data',@rmtpassword='enterprise_data'

GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'rpc', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'rpc out', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'collation name', @optvalue=NULL
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'STFDATA', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO