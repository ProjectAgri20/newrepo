
/*****************************************************************************
 * Title:        Database Size Info
 * Author:       Gary Parham
 * Created Date: 2019-02-12
 * Description:  List databases with size information.
 *****************************************************************************/

SELECT db.[name]
      ,SUM(CASE WHEN [type] = 0 THEN mf.size * 8 / 1024 ELSE 0 END) AS DataFileSizeMB
      ,SUM(CASE WHEN [type] = 1 THEN mf.size * 8 / 1024 ELSE 0 END) AS LogFileSizeMB
FROM sys.master_files mf
JOIN sys.databases db ON db.database_id = mf.database_id
WHERE db.source_database_id IS NULL -- Exclude snapshots
GROUP BY db.[name]
ORDER BY DataFileSizeMB DESC
