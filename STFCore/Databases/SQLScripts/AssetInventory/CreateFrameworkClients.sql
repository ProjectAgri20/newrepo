-- CreateFrameworkClients.sql
-- Inserts new framework client VM records into Asset Inventory.

--------------------------------------------------------------------------------
-- Parameter definitions
DECLARE @startIndex int = 1
DECLARE @endIndex int = 150
DECLARE @vmPrefix varchar(40) = 'W10x64-072-'
DECLARE @sortOrderOffset int = 3000
DECLARE @vmPlatform nvarchar(50) = 'W10x64'


--------------------------------------------------------------------------------
-- Declare local variables
DECLARE @i int = @startIndex
DECLARE @name varchar(50)
DECLARE @date datetime = GETDATE()

-- Insert framework client records
WHILE @i <= @endIndex
BEGIN
    -- Build VM name
    SET @name = CONCAT(@vmPrefix, FORMAT(@i, '000'))

    -- Create VM record
    INSERT INTO FrameworkClient (FrameworkClientHostName, PowerState, UsageState, SortOrder, LastUpdated)
    VALUES (@name, 'Powered Off', 'Available', @sortOrderOffset + @i, @date)

    -- Create platform record
    INSERT INTO FrameworkClientPlatformAssoc(FrameworkClientHostName, FrameworkClientPlatformId)
    VALUES (@name, @vmPlatform)

    -- Increment counter
    SET @i = @i + 1
END


--------------------------------------------------------------------------------
-- Display results
SELECT FrameworkClient.*, FrameworkClientPlatformAssoc.FrameworkClientPlatformId
FROM FrameworkClient
INNER JOIN FrameworkClientPlatformAssoc ON FrameworkClient.FrameworkClientHostName = FrameworkClientPlatformAssoc.FrameworkClientHostName
WHERE SortOrder BETWEEN @sortOrderOffset + @startIndex AND @sortOrderOffset + @endIndex
ORDER BY SortOrder
