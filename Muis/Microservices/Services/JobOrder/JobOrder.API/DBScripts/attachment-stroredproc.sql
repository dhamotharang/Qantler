/****** Object:  StoredProcedure [dbo].[InsertAttachment]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
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
		[ModifiedOn])
	VALUES (@FileID,
		@FileName,
		@Extension,
		@Size,
		GETUTCDATE(),
		GETUTCDATE())

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END

GO