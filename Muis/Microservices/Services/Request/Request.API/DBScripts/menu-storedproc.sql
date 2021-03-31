/****** Object:  StoredProcedure [dbo].[AddMenuReview]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to add review to menu
-- =============================================
CREATE PROCEDURE [dbo].[AddMenuReview]
	@ID BIGINT,
    @Approved BIT NULL,
    @Remarks NVARCHAR(2000) NULL,
    @ReviewedBy UNIQUEIDENTIFIER,
    @ReviewedByName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @ReviewedBy, @ReviewedByName

    UPDATE [Menu]
    SET [Approved] = @Approved,
        [Remarks] = @Remarks,
        [ReviewedBy] = @ReviewedBy,
        [ReviewedOn] = GETUTCDATE(),
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[AddMenus]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Imam, Naqi
-- Create date: 03-Nov-2020
-- Description:	Stored procedure to add list of menus
-- =============================================
CREATE PROCEDURE [dbo].[AddMenus]
	@Scheme SMALLINT,
	@Text NVARCHAR(500) NULL,
	@SubText NVARCHAR(2000) NULL,
	@ChangeType SMALLINT,
	@RequestID BIGINT,
	@Undeclared BIT NULL,
    @Approved BIT NULL,
    @Remarks NVARCHAR(2000) NULL,
    @ReviewedBy UNIQUEIDENTIFIER,
    @ReviewedByName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @ReviewedBy, @ReviewedByName

	INSERT INTO [Menu](Scheme,[Text],SubText,ChangeType,RequestID,Undeclared,Approved,Remarks,ReviewedBy,CreatedOn,ModifiedOn,ReviewedOn) 

    SELECT @Scheme,@Text,@SubText,@ChangeType,@RequestID,@Undeclared,@Approved,@Remarks,@ReviewedBy,GETUTCDATE(),GETUTCDATE(),GETUTCDATE()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertMenu]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to insert menu
-- =============================================
CREATE PROCEDURE [dbo].[InsertMenu]
	@Group SMALLINT,
    @Index SMALLINT,
    @Scheme SMALLINT,
    @Text NVARCHAR(500),
    @SubText NVARCHAR(2000),
    @ChangeType SMALLINT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Menu]([Group],
        [Index],
        [Scheme],
        [Text],
        [SubText],
        [ChangeType],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@Group,
        @Index,
        @Scheme,
        @Text,
        @SubText,
        @ChangeType,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[QueryMenu]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to query menu base on parameters
-- =============================================
CREATE PROCEDURE [dbo].[QueryMenu]
	@RequestID BIGINT NULL,
    @Scheme SMALLINT NULL = NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Menu]
    WHERE (@RequestID IS NULL OR [RequestID] = @RequestID)
    AND (@Scheme IS NULL OR [Scheme] = @Scheme)
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateMenu]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to update menu
-- =============================================
CREATE PROCEDURE [dbo].[UpdateMenu]
	@ID BIGINT,
    @Scheme SMALLINT,
	@Text NVARCHAR(500),
	@SubText NVARCHAR(2000),
	@ChangeType SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Menu]
    SET [Scheme] = @Scheme,
        [Text] = @Text,
        [SubText] = @SubText,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO