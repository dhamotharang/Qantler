/****** Object:  StoredProcedure [dbo].[AddIngredientReview]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to add review to ingredient
-- =============================================
CREATE PROCEDURE [dbo].[AddIngredientReview]
	@ID BIGINT,
    @Approved BIT NULL,
    @Remarks NVARCHAR(2000) NULL,
    @ReviewedBy UNIQUEIDENTIFIER,
    @ReviewedByName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @ReviewedBy, @ReviewedByName

    UPDATE [Ingredient]
    SET [Approved] = @Approved,
        [Remarks] = @Remarks,
        [ReviewedBy] = @ReviewedBy,
        [ReviewedOn] = GETUTCDATE(),
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO

/****** Object:  StoredProcedure [dbo].[AddIngredients]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Imam, Naqi
-- Create date: 03-Nov-2020
-- Description:	Stored procedure to add list of ingredients
-- =============================================
CREATE PROCEDURE [dbo].[AddIngredients]
	@Text NVARCHAR(500) NULL,
	@SubText NVARCHAR(2000) NULL,
	@RiskCategory SMALLINT,
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

	INSERT INTO [Ingredient]([Text],SubText,RiskCategory,ChangeType,RequestID,Undeclared,Approved,Remarks,ReviewedBy,CreatedOn,ModifiedOn,ReviewedOn) 

    SELECT @Text,@SubText,@RiskCategory,@ChangeType,@RequestID,@Undeclared,@Approved,@Remarks,@ReviewedBy,GETUTCDATE(),GETUTCDATE(),GETUTCDATE()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[QueryIngredient]    Script Date: 4/3/2021 8:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to query ingredients base on parameters
-- =============================================
-- Author:		Ramesh
-- Create date: 04-Mar-2021
-- Description:	Added riskcategory parameter
-- =============================================
CREATE PROCEDURE [dbo].[QueryIngredient]
	@RequestID BIGINT NULL,
	@RiskCategory [SmallIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT I.*, ISL.Text as IngredientStatusText, CBL.Text as CertifyingBodyStatusText
    FROM [Ingredient] as I
	  LEFT JOIN [IngredientStatusLookup] AS ISL ON I.[Status] = ISL.ID
	  LEFT JOIN [CertifyingBodyStatusLookup] AS CBL ON I.[CertifyingBodyStatus] = CBL.ID
    WHERE	(@RequestID IS NULL OR I.[RequestID] = @RequestID) 
			AND (NOT EXISTS (SELECT 1 FROM @RiskCategory) OR I.[RiskCategory] IN (SELECT [Val] FROM @RiskCategory))
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateIngredient]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to update ingredient
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIngredient]
	@ID BIGINT,
	@Text NVARCHAR(500),
	@SubText NVARCHAR(2000),
    @RiskCategory SMALLINT,
	@ChangeType SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Ingredient]
    SET [Text] = @Text,
        [SubText] = @SubText,
        [RiskCategory] = @RiskCategory,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO