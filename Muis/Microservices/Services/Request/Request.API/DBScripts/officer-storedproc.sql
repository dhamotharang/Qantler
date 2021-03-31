/****** Object:  StoredProcedure [dbo].[InsertOrReplaceOfficer]    Script Date: 2020-11-19 11:59:13 AM ******/
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
/****** Object:  StoredProcedure [dbo].[SetRequestOfficer]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to set the assigned officer to specified request.
-- =============================================
CREATE PROCEDURE [dbo].[SetRequestOfficer]
	@ID BIGINT,
    @OfficerID UNIQUEIDENTIFIER NULL,
    @OfficerName NVARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;

    IF @OfficerID IS NOT NULL
    BEGIN
        EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName
    END

    UPDATE [Request]
    SET [AssignedTo] = @OfficerID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO