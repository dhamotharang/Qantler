/****** Object:  StoredProcedure [dbo].[GetEmailTemplate]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve email template
-- =============================================
CREATE PROCEDURE [dbo].[GetEmailTemplate]
    @Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [EmailTemplate]
    WHERE [Type] = @Type
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateEmailTemplate]    Script Date: 17/12/2020 7:06:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateEmailTemplate]
	@ID BIGINT,
	@From VARCHAR(255),
	@Cc VARCHAR(255),
	@Bcc VARCHAR(255),
	@Title VARCHAR(65),
	@Body VARCHAR(MAX),
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
		
	Update [EmailTemplate]
	SET [From] = @From,
	[Cc] = @Cc, 
	[Bcc] = @Bcc,
	[Title] = @Title, 
	[Body] = @Body,
	ModifiedOn = GETUTCDATE()
	WHERE ID = @ID

	RETURN 0
END

