USE [ScalableTestDataLog]
GO

/*====================================================================================================================*/
-- IF YOU ARE UPDATING DATALOG TABLES
-- Not all systems may have the Datalog table that is being updated.  This is especially 
-- true with STB. So if you are altering an existing Datalog table, please wrap
-- the alter code with the following code.  If the table does not exist then
-- there won't be an error. This applies to views as well.
--IF EXISTS
--	(
--		SELECT * 
--		FROM INFORMATION_SCHEMA.TABLES 
--		WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = '<YOUR_TABLE_HERE>'
--	)
--	BEGIN
--		-- <ENTER YOUR ALTER TABLE SCRIPT HERE>
--	END
--GO
/*====================================================================================================================*/

-- fdelagarza 8/8/2016  Delete TriageData rows of deleted Sessions.
DELETE [ScalableTestDataLog].[dbo].[TriageData]
FROM [ScalableTestDataLog].[dbo].[TriageData] td
LEFT OUTER JOIN [ScalableTestDataLog].[dbo].[SessionSummary] ss ON ss.SessionId = td.SessionId
WHERE ss.SessionId IS NULL

-- fdelagarza 8/8/2016  Add SessionId foreign key SessionSummary reference with Cascading Deletes constraint to TriageData. 
IF OBJECT_ID('FK_TriageData_SessionSummary', 'F') IS NULL
BEGIN
	ALTER TABLE [dbo].[TriageData]  WITH CHECK ADD  CONSTRAINT [FK_TriageData_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[TriageData] CHECK CONSTRAINT [FK_TriageData_SessionSummary]
END
GO

-- fdelagarza 8/30/2016  Add new Table-valued Functions to support delimited list of tags for filtering. 
IF object_id('SessionIdsMatchingPatterns', 'TF') IS NOT NULL
    DROP FUNCTION [dbo].[SessionIdsMatchingPatterns]
GO

IF object_id('ValueListToContainedPatterns', 'TF') IS NOT NULL
    DROP FUNCTION [dbo].[ValueListToContainedPatterns]
GO

-- =======================================================================
-- Author:		Fernando De La Garza
-- Create date: 8/10/2016
-- Description:	Table Valued Function that extracts comma separated values and returns
--              Table with LIKE statement Contained patterns of each value
-- =======================================================================
CREATE FUNCTION [dbo].[ValueListToContainedPatterns]( @list Varchar(255))
RETURNS 
@ContainedPatterns TABLE (
   Pattern Varchar(255) 
)
AS
BEGIN
   DECLARE @pos        int,
           @nextpos    int,
           @valuelen   int

   SELECT @pos = 0, @nextpos = 1

   WHILE @nextpos > 0
   BEGIN
      SELECT @nextpos = charindex(',', @list, @pos + 1)
      SELECT @valuelen = CASE WHEN @nextpos > 0
                              THEN @nextpos
                              ELSE len(@list) + 1
                         END - @pos - 1
      INSERT @ContainedPatterns (Pattern)
         VALUES ('%,'+substring(@list, @pos + 1, @valuelen)+',%')
      SELECT @pos = @nextpos
   END
   RETURN
END
GO

-- =======================================================================
-- Author:		Fernando De La Garza
-- Create date: 8/10/2016
-- Description:	Table Valued Function that returns all of the SessionId's
--              that contain tags in passed value list
-- =======================================================================
CREATE FUNCTION [dbo].[SessionIdsMatchingPatterns]( @list Varchar(255))
RETURNS 
@SessionIds TABLE (
   SessionId Varchar(50) 
)
AS
BEGIN
   IF(@list IS NOT NULL AND len(@list)>0)
   BEGIN
      INSERT INTO @SessionIds
	  SELECT DISTINCT SessionId FROM SessionSummary ss WITH(NOLOCK)
	  INNER JOIN ValueListToContainedPatterns(@list) p ON ','+ss.Tags+',' LIKE p.Pattern
   END
   ELSE
   BEGIN
      INSERT INTO @SessionIds
	  SELECT DISTINCT SessionId FROM SessionSummary WITH(NOLOCK)
   END
   RETURN
END
GO




