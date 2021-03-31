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
	@JobOrderID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @CreatedBy, @CreatedByName

    INSERT INTO [Notes] ([Text],
        [CreatedBy],
        [JobOrderID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@Text,
        @CreatedBy,
        @JobOrderID,
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
    @AttachmentIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [NotesAttachments]
    SELECT @NotesID, [Val]
    FROM @AttachmentIDs

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[SelectNotes]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 08-Jan-2021
-- Description:	Stored procedure to select notes
-- =============================================
CREATE PROCEDURE [dbo].[SelectNotes]
	@JobOrderID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT n.*,
        o.[ID] as [CreatedBy], o.*,
        a.[ID] as [Attachment], a.*
    FROM [Notes] n
    INNER JOIN [Officer] o ON o.[ID] = n.[CreatedBy]
    LEFT JOIN [NotesAttachments] na ON na.[NotesID] = n.[ID]
    LEFT JOIN [Attachment] a ON a.[ID] = na.[AttachmentID]
    WHERE n.[JobOrderID] = @JobOrderID
    AND n.[IsDeleted] = 0

    RETURN 0
END


GO