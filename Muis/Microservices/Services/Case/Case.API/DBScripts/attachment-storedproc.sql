/****** Object:  StoredProcedure [dbo].[InsertAttachment]    Script Date: 5/2/2021 12:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to insert attachment
-- =============================================
CREATE PROCEDURE [dbo].[InsertAttachment]
	@FileID UNIQUEIDENTIFIER,
	@FileName NVARCHAR(150),
	@Extension NVARCHAR(30),
	@Size BIGINT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Attachment] ([FileID],
		[FileName],
		[Extension],
		[Size],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@FileID,
		@FileName,
		@Extension,
		@Size,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[MapCaseAttachments]    Script Date: 5/2/2021 12:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 04-Feb-2021
-- Description:	Stored procedure to map attachment to case
-- =============================================
CREATE PROCEDURE [dbo].[MapCaseAttachments]
	@CaseID BIGINT,
    @AttachmentIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [CaseAttachments]
    SELECT @CaseID, [Val]
    FROM @AttachmentIDs

    RETURN 0
END
GO
