/****** Object:  StoredProcedure [dbo].[InsertOrReplaceOfficer]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertOrReplaceOfficer]
    @ID UNIQUEIDENTIFIER,
    @Name NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [Officer] WHERE [ID] = @ID)
    BEGIN
        INSERT INTO [Officer] ([ID],
            [Name],
            [IsDeleted])
        VALUES (@ID,
            @Name,
            0)
    END
    ELSE
    BEGIN
        UPDATE [Officer]
        SET [Name] = @Name
        WHERE [ID] = @ID
    END

    RETURN 0
END


GO