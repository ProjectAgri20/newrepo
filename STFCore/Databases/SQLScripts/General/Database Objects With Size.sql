
/*****************************************************************************
 * Title:        Database Objects With Size
 * Author:       Gary Parham
 * Created Date: 2019-02-12
 * Description:  List database objects with space information.
 *****************************************************************************/

SELECT SCHEMA_NAME(o.schema_id) + '.' + OBJECT_NAME(p.object_id) AS [Name]
      ,CONVERT(decimal(18, 2), SUM(reserved_page_count) * 8 / 1024.0) AS Total_space_used_MB
      ,CONVERT(decimal(18, 2), SUM(CASE WHEN index_id < 2 THEN reserved_page_count ELSE 0 END ) * 8 / 1024.0) AS Table_space_used_MB
      ,CONVERT(decimal(18, 2), SUM(CASE WHEN index_id > 1 THEN reserved_page_count ELSE 0 END ) * 8 / 1024.0) AS Nonclustered_index_spaced_used_MB
      ,MAX(row_count) AS Row_count
FROM sys.dm_db_partition_stats AS p
INNER JOIN sys.all_objects AS o ON p.object_id = o.object_id
WHERE o.is_ms_shipped = 0
GROUP BY SCHEMA_NAME(o.schema_id) + '.' + OBJECT_NAME(p.object_id)
ORDER BY Total_space_used_MB desc
