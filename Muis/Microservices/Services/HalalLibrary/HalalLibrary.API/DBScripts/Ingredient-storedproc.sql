/****** Object:  StoredProcedure [dbo].[DeleteIngredient]    Script Date: 4/1/2021 12:24:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 29-Dec-2020
-- Description:	Stored procedure to delete ingredient
-- =============================================
CREATE PROCEDURE [dbo].[DeleteIngredient]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Ingredient] SET [IsDeleted]=1 WHERE [ID] = @ID

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[InsertIngredient]    Script Date: 4/1/2021 12:24:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 29-Dec-2020
-- Description:	Stored procedure to insert ingredient
-- =============================================
CREATE PROCEDURE [dbo].[InsertIngredient]
	@Out BIGINT OUT,
	@Name NVARCHAR(255),
	@Brand NVARCHAR(255) NULL,
	@RiskCategory SMALLINT,
	@Status SMALLINT,
	@SupplierID BIGINT NULL,
	@CertifyingBodyID BIGINT NULL,
	@VerifiedByID UNIQUEIDENTIFIER NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS( SELECT 1 FROM [dbo].[Ingredient] 
	WHERE Lower([Name]) = Lower(@Name) AND 
	Lower([Brand]) = Lower(@Brand) AND 
	(@SupplierID IS NULL or 
		(@SupplierID IS NOT NULL AND  [SupplierID] = @SupplierID))AND
	[IsDeleted] = 0)
	BEGIN
		SET @Out = 0;
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Ingredient] ([Name],
		[Brand],
		[RiskCategory],
		[Status],
		[SupplierID],
		[CertifyingBodyID],
		[VerifiedByID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted]
		)
		VALUES (@Name,
		@Brand,
		@RiskCategory,
		@Status,
		@SupplierID,
		@CertifyingBodyID,
		@VerifiedByID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)	

		SET @Out = SCOPE_IDENTITY();
	END

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[SelectHalalLibrary]    Script Date: 4/1/2021 12:24:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 28-Sep-2020
-- Description:	Stored procedure to get list of halal library
-- =============================================
CREATE PROCEDURE [dbo].[SelectHalalLibrary] -- [SelectHalalLibrary] 15, 0,null,null,null,null,null,1,null
	@NextRows BIGINT,
	@OffsetRows BIGINT,
	@Name NVARCHAR(255) NULL,
	@Brand NVARCHAR(255) NULL,
	@Supplier NVARCHAR(255) NULL, 
	@CertifyingBody NVARCHAR(255) NULL,
	@RiskCategory SMALLINT NULL,
	@Status SMALLINT NULL,
	@VerifiedBy NVARCHAR(255) NULL
AS
BEGIN
	SET NOCOUNT ON;
	Declare @TotalData BIGINT = 0;

	SELECT @TotalData = count(*)
	FROM [Ingredient] AS I 
	LEFT JOIN [RiskCategoryLookup] AS R ON I.RiskCategory = R.ID
	LEFT JOIN [IngredientStatusLookup] AS ISL ON I.Status = ISL.ID
	LEFT JOIN [Supplier] AS S ON I.SupplierID = S.ID
	LEFT JOIN [CertifyingBody] AS C ON I.CertifyingBodyID = C.ID
	LEFT JOIN [Officer] AS O ON I.VerifiedByID = O.ID
	Where I.IsDeleted = 0
	AND (@Name IS NULL OR I.Name LIKE '%' + @Name + '%')
	AND (@Brand IS NULL OR I.Brand LIKE '%' + @Brand + '%')
	AND (@Supplier IS NULL OR S.Name LIKE '%' + @Supplier + '%')
	AND (@CertifyingBody IS NULL OR C.Name LIKE '%' + @CertifyingBody + '%')
	AND (@VerifiedBy IS NULL OR O.Name LIKE '%' + @VerifiedBy + '%')
	AND (@RiskCategory IS NULL OR I.RiskCategory = @RiskCategory)
	AND (@Status IS NULL OR I.Status = @Status)

	SELECT @TotalData as totalData, 
	I.ID as IngredientID,
	I.*,
	R.Text AS RiskCategoryText,
	ISL.Text AS StatusText,
	S.ID as SupplierID,
	S.*,
	C.ID as CertifyingBodyID,
	C.* ,
	O.ID as OfficerID,
	O.*
	FROM [Ingredient] AS I 
	LEFT JOIN [RiskCategoryLookup] AS R ON I.RiskCategory = R.ID
	LEFT JOIN [IngredientStatusLookup] AS ISL ON I.Status = ISL.ID
	LEFT JOIN [Supplier] AS S ON I.SupplierID = S.ID
	LEFT JOIN [CertifyingBody] AS C ON I.CertifyingBodyID = C.ID
	LEFT JOIN [Officer] AS O ON I.VerifiedByID = O.ID
	Where I.IsDeleted = 0
	AND (@Name IS NULL OR I.Name LIKE '%' + @Name + '%')
	AND (@Brand IS NULL OR I.Brand LIKE '%' + @Brand + '%')
	AND (@Supplier IS NULL OR S.Name LIKE '%' + @Supplier + '%')
	AND (@CertifyingBody IS NULL OR C.Name LIKE '%' + @CertifyingBody + '%')
	AND (@VerifiedBy IS NULL OR O.Name LIKE '%' + @VerifiedBy + '%')
	AND (@RiskCategory IS NULL OR I.RiskCategory = @RiskCategory)
	AND (@Status IS NULL OR I.Status = @Status)
	ORDER BY I.ID OFFSET @OffsetRows ROWS FETCH NEXT @NextRows ROWS ONLY

END
GO

/****** Object:  StoredProcedure [dbo].[UpdateIngredient]    Script Date: 4/1/2021 12:24:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 29-Dec-2020
-- Description:	Stored procedure to update ingredient
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIngredient]
	@Out BIGINT OUT,
	@ID BIGINT,
	@Name NVARCHAR(255),
	@Brand NVARCHAR(255) NULL,
	@RiskCategory SMALLINT,
	@Status SMALLINT,
	@SupplierID BIGINT NULL,
	@CertifyingBodyID BIGINT NULL,
	@VerifiedByID UNIQUEIDENTIFIER NULL
AS
BEGIN
	SET NOCOUNT ON;


	IF EXISTS( SELECT 1 FROM [dbo].[Ingredient] 
	WHERE Lower([Name]) = LOWER(@Name) AND 
	LOWER([Brand]) = LOWER(@Brand) AND 
	(@SupplierID IS NULL or 
		(@SupplierID IS NOT NULL AND  [SupplierID] = @SupplierID)) AND
	[ID] != @ID AND
	[IsDeleted] = 0)
	BEGIN
		
		SET @Out = 0;
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Ingredient] SET [Name] = @Name,
		[Brand] =@Brand,
		[RiskCategory] = @RiskCategory,
		[Status] = @Status,
		[SupplierID] = @SupplierID,
		[CertifyingBodyID] = @CertifyingBodyID,
		[VerifiedByID]= @VerifiedByID,
		[ModifiedOn] = GETUTCDATE()
		WHERE [ID] = @ID

		SET @Out = @ID;
	END

	RETURN 0	
END
GO

/****** Object:  StoredProcedure [dbo].[CheckIngredient]    Script Date: 4/3/2021 1:07:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 02-Mar-2021
-- Description:	Stored procedure to check the ingredient already exists
-- =============================================
CREATE PROCEDURE [dbo].[CheckIngredient]
	@Name NVARCHAR(255),
	@Brand NVARCHAR(255) NULL,
	@SupplierName NVARCHAR(255) NULL,
	@CertifyingBody NVARCHAR(255) NULL,
	@Result 		[bit] OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SET @Result = 0
    DECLARE @Count [int]=0

	SELECT @Count = count(*)
	FROM [Ingredient] AS I 
	LEFT JOIN [Supplier] AS S ON I.SupplierID = S.ID
	LEFT JOIN [CertifyingBody] AS C ON I.CertifyingBodyID = C.ID
	Where I.IsDeleted = 0
	AND (@Name IS NULL OR I.[Name] = @Name)
	AND (@Brand IS NULL OR I.[Brand] = @Brand)
	AND (@SupplierName IS NULL OR S.[Name] = @SupplierName)
	AND (@CertifyingBody IS NULL OR C.[Name] = @CertifyingBody)

	IF @Count = 0
		SET @Result = 1

    RETURN 0	
END
GO