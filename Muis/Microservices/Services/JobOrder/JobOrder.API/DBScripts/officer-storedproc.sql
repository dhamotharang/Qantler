/****** Object:  StoredProcedure [dbo].[GetOfficerByID]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to retrieve officer with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetOfficerByID]
    @ID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Officer]
    WHERE [ID] = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[InsertOrReplaceOfficer]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to insert officer if does not exists, otherwise update info
-- =============================================
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