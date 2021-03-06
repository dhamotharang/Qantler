/****** Object:  StoredProcedure [dbo].[InsertOrReplaceAccount]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertOrReplaceAccount]
    @ID UNIQUEIDENTIFIER,
    @Name NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [Account] WHERE [ID] = @ID)
    BEGIN
        INSERT INTO [Account]
        VALUES (@ID,
            'Test',
            0)
    END
    ELSE
    BEGIN
        UPDATE [Account]
        SET [Name] = 'Test'
        WHERE [ID] = @ID
    END

    RETURN 0
END

GO