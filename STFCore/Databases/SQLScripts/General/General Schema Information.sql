
/*****************************************************************************
 * Title:        General Schema Information
 * Author:       Gary Parham
 * Created Date: 2019-02-12
 * Description:  This script displays general information about the schema of
 *               a database.
 *****************************************************************************/

/* Show table column information */
SELECT TABLE_NAME
      ,COLUMN_NAME
      ,COLUMN_DEFAULT
      ,IS_NULLABLE
      ,DATA_TYPE
      ,CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME, COLUMN_NAME

/* Show constraints on table columns */
SELECT ccu.TABLE_NAME
      ,ccu.COLUMN_NAME
      ,ccu.CONSTRAINT_NAME
      ,tc.CONSTRAINT_TYPE
FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON ccu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE ccu.TABLE_SCHEMA = 'dbo'
ORDER BY ccu.TABLE_NAME, ccu.COLUMN_NAME

/* Show update and delete rules for foreign keys */
SELECT CONSTRAINT_NAME
      ,UPDATE_RULE
      ,DELETE_RULE
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
ORDER BY CONSTRAINT_NAME

/* Show views in the database */
SELECT VIEW_SCHEMA
      ,VIEW_NAME
      ,TABLE_SCHEMA
      ,TABLE_NAME
      ,COLUMN_NAME
FROM INFORMATION_SCHEMA.VIEW_COLUMN_USAGE
ORDER BY VIEW_NAME, TABLE_NAME, COLUMN_NAME

/* Show information about the indexes on the tables */
SELECT [name] AS Index_Name
      ,[type_desc] AS Index_Type
      ,is_primary_key AS Is_Primary_Key
FROM sys.indexes
WHERE [name] LIKE 'PK%' OR [name] LIKE 'IDX%'