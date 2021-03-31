/****** Object:  StoredProcedure [dbo].[InsertLog]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertLog]
    @Type SMALLINT NULL,
    @RefID VARCHAR(36) NULL,
    @Action NVARCHAR(2000),
    @Raw NVARCHAR(2000) NULL,
    @Notes NVARCHAR(4000) NULL,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150),
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @UserID, @UserName

    IF @Raw IS NULL
    BEGIN
        SET @Raw = '[]'
    END

	INSERT INTO [Log]
    VALUES (@Type,
        @REfID,
        @Action,
        @Raw,
        @Notes,
        @UserID,
        GETUTCDATE(),
        0);
    
    SET @ID = SCOPE_IDENTITY();

    RETURN 0;
END

GO