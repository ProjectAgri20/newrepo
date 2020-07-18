IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trg_InsertPending]'))
DROP TRIGGER [dbo].[trg_InsertPending]
GO
CREATE TRIGGER [dbo].[trg_InsertPending] ON [dbo].[job] AFTER INSERT, UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
	DECLARE @id AS INT
	SELECT @id = ID FROM INSERTED
	IF NOT EXISTS (SELECT JobId FROM [EPrintMonitor].[dbo].[Pending] WHERE JobId = @id)
	BEGIN
		INSERT INTO [EPrintMonitor].[dbo].[Pending] VALUES (@id, 1)
	END
END