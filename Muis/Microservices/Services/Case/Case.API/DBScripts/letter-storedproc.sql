/****** Object:  StoredProcedure [dbo].[InsertActivity]    Script Date: 19/2/2021 11:19:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Feb-2021
-- Description:	Stored procedure to create letter
-- =============================================
CREATE PROCEDURE [dbo].[InsertLetter]
	@Type SMALLINT,
	@Body NVARCHAR(MAX)NULL,
	@EmailID BIGINT NULL,
	@Status SMALLINT,
	@Out BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Letter] ([Type],
		[Status],
		[Body],
		[EmailID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@Type,
		@Status,
		@Body,
		@EmailID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @Out = SCOPE_IDENTITY()

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateLetter]    Script Date: 19/2/2021 11:19:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Feb-2021
-- Description:	Stored procedure to update letter
-- =============================================
CREATE PROCEDURE [dbo].[UpdateLetter]
	@ID BIGINT,
	@Type SMALLINT,
	@Body NVARCHAR(MAX) NULL,
	@EmailID BIGINT NULL,
	@Status SMALLINT,
	@Out BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[Letter] 
	SET [Type] = @Type,
	[Status] = @Status,
	[Body] = @Body,
	[EmailID] = @EmailID,
	[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

	SET @Out = @ID

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[GetLetterByID]    Script Date: 16/3/2021 11:19:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 16-Mar-2021
-- Description:	Stored procedure to retrieve letter with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetLetterByID]
    @ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT L.*, Lt.Text as 'TypeText' FROM [Letter] as L 
	INNER JOIN [LetterTypeLookup] as LT on L.Type = LT.ID
    WHERE L.[ID] = @ID

END
