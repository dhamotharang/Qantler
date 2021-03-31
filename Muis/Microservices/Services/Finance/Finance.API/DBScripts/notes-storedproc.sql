/****** Object:  StoredProcedure [dbo].[InsertNotes]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 08-Jan-2021
-- Description:	Stored procedure to insert notes
-- =============================================
CREATE PROCEDURE [dbo].[InsertNotes]
	@Text NVARCHAR(4000),
    @CreatedBy UNIQUEIDENTIFIER,
    @CreatedByName NVARCHAR(150),
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @CreatedBy, @CreatedByName

    INSERT INTO [Notes] ([Text],
        [CreatedBy],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@Text,
        @CreatedBy,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapNotesAttachments]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 08-Jan-2021
-- Description:	Stored procedure to map attachment to notes
-- =============================================
CREATE PROCEDURE [dbo].[MapNotesAttachments]
	@NotesID BIGINT,
    @AttachmentID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [NotesAttachments]
    VALUES(@NotesID, @AttachmentID)

    RETURN 0
END


GO
