declare @path nvarchar(40)
declare @file nvarchar(16)

select @file = 'ETFDaily-' + CONVERT(CHAR(1), DATEPART(dw, GETDATE())) + '.bak'
select @path = 'D:\Backup\' + @file
BACKUP DATABASE [EnterpriseTest] TO  DISK = @path WITH NOFORMAT, INIT,  NAME = N'EnterpriseTest-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO
