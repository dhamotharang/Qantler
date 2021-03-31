/****** Object:  StoredProcedure [dbo].[GetEmailByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve email by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetEmailByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Email]
    WHERE [ID] = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[GetEmailTemplate]    Script Date: 2020-11-19 11:59:13 AM ******/
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
/****** Object:  StoredProcedure [dbo].[InsertEmail]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert email
-- =============================================
CREATE PROCEDURE [dbo].[InsertEmail]
    @To VARCHAR(255),
    @From VARCHAR(255),
    @Cc VARCHAR(255) NULL,
    @Bcc VARCHAR(255) NULL,
    @Title NVARCHAR(65),
    @Body NVARCHAR(max),
    @IsBodyHtml BIT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Email] ([To],
        [From],
        [Cc],
        [Bcc],
        [Title],
        [Body],
        [IsBodyHtml],
        [CreatedOn],
        [ModifiedOn],
        [IsDEleted])
    VALUES (@To,
        @From,
        @Cc,
        @Bcc,
        @Title,
        @Body,
        @IsBodyHtml,
        GETUTCDATE(),
        GETUTCDATE(),
        0)
    
    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateRejectionEmailTemplate]    Script Date: 2020-11-19 11:59:13 AM ******/
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
GO