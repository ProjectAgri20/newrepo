cd d:\backup
sqlcmd -U enterprise_admin -P enterprise_admin -i d:\backup\Backup-ETF.sql
sqlcmd -U enterprise_admin -P enterprise_admin -i d:\backup\CleanupIndexes.sql
