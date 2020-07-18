USE [ScalableTestDataLog]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING DATALOG TABLES
-- Not all systems may have the DataLog table that is being updated.  This is
-- especially true with the STB database.  So, if you are altering the database
-- schema then please wrap the SQL commands with the appropriate check below.
-- This will help to ensure that an error does not occur when the SQL script
-- is ran.
--
-- * Check if table exists.
-- IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.<YOUR_TABLE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR TABLE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a table does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a view exists.
-- IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID('dbo.<YOUR_VIEW_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR VIEW ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a view does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a stored procedure exists.
-- IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.<YOUR_PROCEDURE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR PROCEDURE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a stored procedure does not exist, change the 'IF'
--       statement to
--          IF NOT EXISTS ( ... )
--
-- * Check if a column exists in a specific table. (Same syntax works for views also)
-- IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '<TABLE_NAME_HERE>' and COLUMN_NAME = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/

/******************************************************************************
 Author: Gary Parham

 The following set of commands will walk through any table in the database that
 contains a column with data type of datetime and change the values in these
 columns from local time to UTC time.
******************************************************************************/
SET NOCOUNT ON;

DECLARE @tableName nvarchar(255);
DECLARE @columnName nvarchar(255);
DECLARE @sqlCmd nvarchar(4000);
DECLARE @currentTable nvarchar(255);
DECLARE @currentColumn nvarchar(255);
DECLARE @utcOffset int;

/********************************************************
 Create a temp table to hold the table and column names.
********************************************************/
IF OBJECT_ID('tempdb..#DateTimeColumns') IS NOT NULL
BEGIN
    DROP TABLE #DateTimeColumns;
END;

SELECT tbl.TABLE_NAME,
       col.COLUMN_NAME
INTO   #DateTimeColumns
FROM   INFORMATION_SCHEMA.COLUMNS AS col
JOIN   INFORMATION_SCHEMA.TABLES AS tbl ON col.TABLE_NAME = tbl.TABLE_NAME
WHERE  col.DATA_TYPE = 'datetime'
       AND tbl.TABLE_TYPE <> 'VIEW'
ORDER BY tbl.TABLE_NAME, col.COLUMN_NAME;

INSERT INTO #DateTimeColumns VALUES ('z', 'z'); -- Dummy values.

/******************************************************************************
 Add the 'UpdatedToUTC' column to every table that contains a datetime column.
******************************************************************************/
DECLARE tableNameCursor CURSOR SCROLL
FOR SELECT DISTINCT TABLE_NAME
    FROM   #DateTimeColumns;

OPEN tableNameCursor;

FETCH FIRST FROM tableNameCursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @tableName <> 'z'
    BEGIN
        SET @sqlCmd = 'IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''' + @tableName + ''' AND COLUMN_NAME = ''UpdatedToUTC'') BEGIN ';
        SET @sqlCmd = @sqlCmd + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' ADD [UpdatedToUTC] BIT CONSTRAINT UTC_default_' + @tableName + ' DEFAULT (1); END;';
    END;

    --PRINT @sqlCmd;
    EXEC sp_executesql @sqlCmd;

    FETCH NEXT FROM tableNameCursor INTO @tableName;
END;

/************************************************************************************
 Convert the data in columns holding local datetime values into UTC datetime values.
************************************************************************************/
-- Get the server's local time zone offset.
SET @utcOffset = DATEDIFF(HOUR, GETUTCDATE(), GETDATE());

-- Create the cursor used to walk through the rows in the temp table above.
DECLARE dateTimeCursor CURSOR SCROLL
FOR SELECT TABLE_NAME,
           COLUMN_NAME
    FROM   #DateTimeColumns
    ORDER BY TABLE_NAME, COLUMN_NAME;

OPEN dateTimeCursor;

-- Get the first table-column pair.
FETCH FIRST FROM dateTimeCursor INTO @tableName, @columnName;

-- Remember the table and column names for later.
SET @currentTable = @tableName;
SET @currentColumn = @columnName;

-- Start the SQL update statement for the first table.
SET @sqlCmd = 'DECLARE @rows int; DECLARE @batchSize int = 3000; SET @rows = @batchSize; ';
SET @sqlCmd = @sqlCmd + 'WHILE @rows = @batchSize BEGIN ';
SET @sqlCmd = @sqlCmd + 'UPDATE TOP (@batchSize) ' + QUOTENAME(@tableName) + ' SET ' + QUOTENAME(@columnName) + ' = DATEADD(HOUR, ' + CAST(@utcOffset AS nvarchar(5)) + ', ' + QUOTENAME(@columnName) + ')';

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @currentTable = @tableName
    BEGIN
        IF @currentColumn <> @columnName
        BEGIN
            -- Add the column if it was not already included.
            SET @sqlCmd = @sqlCmd + ', ' + QUOTENAME(@columnName) + ' = DATEADD(HOUR, ' + CAST(@utcOffset AS nvarchar(5)) + ', ' + QUOTENAME(@columnName) + ')';
        END;
    END;
    ELSE
    BEGIN
        -- Execute the SQL update statment and begin constructing the SQL for the next table.
        SET @sqlCmd = @sqlCmd + ', [UpdatedToUTC] = 1 WHERE [UpdatedToUTC] IS NULL; SET @rows = @@ROWCOUNT; END;';

        --PRINT @sqlCmd;
        EXEC sp_executesql @sqlCmd;

        SET @sqlCmd = 'DECLARE @rows int; DECLARE @batchSize int = 3000; SET @rows = @batchSize; ';
        SET @sqlCmd = @sqlCmd + 'WHILE @rows = @batchSize BEGIN ';
        SET @sqlCmd = @sqlCmd + 'UPDATE TOP (@batchSize) ' + QUOTENAME(@tableName) + ' SET ' + QUOTENAME(@columnName) + ' = DATEADD(HOUR, ' + CAST(@utcOffset AS nvarchar(5)) + ', ' + QUOTENAME(@columnName) + ')';

        SET @currentTable = @tableName;
        SET @currentColumn = @columnName;
    END;

    FETCH NEXT FROM dateTimeCursor INTO @tableName, @columnName;
END;

CLOSE dateTimeCursor;

DEALLOCATE dateTimeCursor;

/*********************************************************************************
 Drop the 'UpdatedToUTC' column from every table that contains a datetime column.
*********************************************************************************/
FETCH FIRST FROM tableNameCursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @tableName <> 'z'
    BEGIN
        SET @sqlCmd = 'IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''' + @tableName + ''' AND COLUMN_NAME = ''UpdatedToUTC'') BEGIN ';
        SET @sqlCmd = @sqlCmd + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' DROP CONSTRAINT UTC_default_' + @tableName + '; ';
        SET @sqlCmd = @sqlCmd + 'ALTER TABLE ' + QUOTENAME(@tableName) + ' DROP COLUMN [UpdatedToUTC]; END; ';
    END;

    --PRINT @sqlCmd;
    EXEC sp_executesql @sqlCmd;

    FETCH NEXT FROM tableNameCursor INTO @tableName;
END;

CLOSE tableNameCursor;

DEALLOCATE tableNameCursor;


-- parham: function fn_CalcLocalDateTime
IF OBJECT_ID(N'fn_CalcLocalDateTime', N'FN') IS NOT NULL
BEGIN
    DROP FUNCTION fn_CalcLocalDateTime;
END;
GO

-- ============================================================================
-- Author:      Gary Parham
-- Create date: 2019/07/09
-- Description: Calculate a local datetime value from a UTC datetime.
-- ============================================================================
CREATE FUNCTION [dbo].[fn_CalcLocalDateTime]
(
    @utcDateTime datetime
)
RETURNS datetime
AS
BEGIN
    -- Declare the return variable here
    DECLARE @result datetime;

    -- Add the T-SQL statements to compute the return value here
    SET @result = DATEADD(HOUR, DATEDIFF(HOUR, GETUTCDATE(), GETDATE()), @utcDateTime);

    -- Return the result of the function
    RETURN @result;
END
GO
