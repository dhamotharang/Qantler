/****** Object:  StoredProcedure [dbo].[InsertCase]    Script Date: 13/2/2021 12:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 12-Feb-2021
-- Description:	Stored procedure to create activity
-- =============================================
CREATE PROCEDURE [dbo].[InsertActivity]
	@Type SMALLINT NULL,
	@RefID VARCHAR(36) NULL,
	@Action NVARCHAR(2000),
	@Notes NVARCHAR(4000) NULL,
	@CaseID BIGINT,
	@UserID UNIQUEIDENTIFIER NULL,
	@Out BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Activity] ([Type],
		[RefID],
		[Action],
		[Notes],
		[CaseID],
		[UserId],
		[CreatedOn],
		[IsDeleted])
	VALUES (@Type,
		@RefID,
		@Action,
		@Notes,
		@CaseID,
		@UserID,
		GETUTCDATE(),
		0)

	SET @Out = SCOPE_IDENTITY()

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapActivityAttachments]    Script Date: 13/2/2021 12:43:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 12-Feb-2021
-- Description:	Stored procedure to map attachment to activity
-- =============================================
CREATE PROCEDURE [dbo].[MapActivityAttachments]
	@ActivityID BIGINT,
    @AttachmentIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [ActivityAttachments]
    SELECT @ActivityID, [Val]
    FROM @AttachmentIDs

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapActivityLetter]    Script Date: 19/2/2021 11:30:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Feb-2021
-- Description:	Stored procedure to map Letter to activity
-- =============================================
CREATE PROCEDURE [dbo].[MapActivityLetter]
	@ActivityID BIGINT,
  @LetterID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[ActivityLetters]
    SELECT @ActivityID, @LetterID

    RETURN 0
END
