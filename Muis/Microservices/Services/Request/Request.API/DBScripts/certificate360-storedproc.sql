/****** Object:  StoredProcedure [dbo].[DeleteAllCertificate360Ingredients]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Delete all certificate360 ingredients mapping
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllCertificate360Ingredients]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [Certificate360Ingredients]
	WHERE [CertificateID] = @ID

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[DeleteAllCertificate360Menu]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Delete all certificate360 menu mapping
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllCertificate360Menu]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [Certificate360Menus]
	WHERE [CertificateID] = @ID

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[DeleteAllCertificate360Teams]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Delete all certificate360 teams mapping
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllCertificate360Teams]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [Certificate360Teams]
	WHERE [CertificateID] = @ID

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[GetCertificate360ByNo]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve vertificate 360 by specified number.
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360ByNo]
    @Number VARCHAR(60)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Certificate360]
    WHERE [Number] = @Number
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertificate360Detail]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TEBS
-- Create date: 24-Aug-2020
-- Description:	Get certificate 360 details
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360Detail]
	-- Add the parameters for the stored procedure here
	@CertificateNo VARCHAR(60)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT C.*, CSL.Text AS STATUSTEXT,SL.Text AS SCHEMETEXT, SSL.Text AS SUBSCHEMETEXT, 
	CH.ID AS CertificateHistoryID, CH.*,
	M.ID AS MenuID, M.*,
	I.ID AS IngredientID, I.*,
	P.ID AS PremiseID , P.*,
	CUS.ID AS CustomerID, CUS.*,
	H.ID AS HalalTeamID, H.*
	
		FROM [Certificate360] C
		INNER JOIN CertificateStatusLookup CSL ON C.Status = CSL.ID
		LEFT JOIN SchemeLookup SL ON C.Scheme = SL.ID
		LEFT JOIN SubSchemeLookup SSL ON C.SubScheme = SSL.ID
		LEFT JOIN Certificate360Menus CM ON C.ID = CM.CertificateID
		LEFT JOIN Menu M ON M.ID= CM.MenuID
		LEFT JOIN Certificate360Ingredients CI ON C.ID = CI.CertificateID
		LEFT JOIN Ingredient I on I.ID = CI.IngredientID
		LEFT JOIN Premise P ON C.PremiseID = P.ID
		LEFT JOIN Customer CUS ON CUS.ID = C.CustomerID
		LEFT JOIN Certificate360History CH ON CH.CertificateID = C.ID
		LEFT JOIN Certificate360Teams CT ON CT.CertificateID = C.ID
		LEFT JOIN HalalTeam H ON H.ID = CT.HalalTeamID
			WHERE C.Number = @CertificateNo
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertsForAutoRenewal]    Script Date: 18/12/2020 1:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TEBS
-- Create date: 24-Aug-2020
-- Description:	Get certificates for auto renewal trigger
-- =============================================
-- Modification History
-- 09/12/2020 SathiyaPriya
-- Included scheme and subscheme. join with renewal log
-- =============================================
CREATE PROCEDURE [dbo].[GetCertsForAutoRenewal]
	-- Add the parameters for the stored procedure here
	@RenewalPeriod int, -- renewal period in months
	@Scheme int,
	@Subscheme int NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--DECLARE @CALRENEWALPERIOD DATETIME2 = DATEADD(MONTH,-3,GETUTCDATE()); 

	SELECT A.[ID]		
			  ,A.[Number]
			  ,A.[Status]
			  ,A.[Template]
			  ,A.[Scheme]
			  ,A.[SubScheme]
			  ,A.[IssuedOn]
			  ,A.[ExpiresOn]
			  ,A.[SerialNo]
			  ,A.[CustomerID]
			  ,A.[CustomerName]
			  ,A.[PremiseID]
			  ,A.[CreatedOn]
			  ,A.[ModifiedOn]
			  ,A.[IsDeleted]
		  FROM [dbo].[Certificate360] A
		  LEFT JOIN [dbo].[AutoRenewalTriggerLog] B ON
		  A.Number = B.Number AND A.ExpiresOn = B.ExpiresOn
		  where A.IsDeleted = 0 AND
		  A.Scheme = @Scheme AND A.SubScheme = @Subscheme AND
		  A.[Status] = 100 
		  AND 
			 GETUTCDATE() >= DATEADD(MONTH,-@RenewalPeriod,A.[ExpiresOn])
			 AND 
			 GETUTCDATE() <= A.[ExpiresOn] 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertCertificate360]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Insert certificate360 instance
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertificate360]
	@Number VARCHAR(60),
	@Status SMALLINT,
	@Template SMALLINT,
	@Scheme SMALLINT,
	@SubScheme SMALLINT,
	@IssuedOn DATETIME2(0) NULL,
	@ExpiresOn DATETIME2(0),
	@SerialNo VARCHAR(36),
	@CustomerID UNIQUEIDENTIFIER,
	@CustomerName NVARCHAR(150),
	@RequestorID UNIQUEIDENTIFIER,
	@RequestorName NVARCHAR(150),
	@AgentID UNIQUEIDENTIFIER NULL,
	@AgentName NVARCHAR(150),
	@PremiseID BIGINT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Certificate360] ([Number],
		[Status],
		[Template],
		[Scheme],
		[SubScheme],
		[IssuedOn],
		[ExpiresOn],
		[SerialNo],
		[CustomerID],
		[CustomerName],
		[RequestorID],
		[RequestorName],
		[AgentID],
		[AgentName],
		[PremiseID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@Number,
		@Status,
		@Template,
		@Scheme,
		@SubScheme,
		@IssuedOn,
		@ExpiresOn,
		@SerialNo,
		@CustomerID,
		@CustomerName,
		@RequestorID,
		@RequestorName,
		@AgentID,
		@AgentName,
		@PremiseID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END
/****** Object:  StoredProcedure [dbo].[InsertCertificate360]    Script Date: 3/11/2020 7:16:31 PM ******/
SET ANSI_NULLS ON

GO
/****** Object:  StoredProcedure [dbo].[InsertCertificate360History]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Insert certificate360 history instance
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertificate360History]
	@RequestID BIGINT,
	@RefID VARCHAR(36) NULL,
	@RequestorID UNIQUEIDENTIFIER,
	@RequestorName NVARCHAR(150),
	@AgentID UNIQUEIDENTIFIER,
	@AgentName NVARCHAR(150),
	@Duration SMALLINT,
	@IssuedOn DATETIME2(0) NULL,
	@ExpiresOn DATETIME2(0),
	@SerialNo VARCHAR(36),
	@ApprovedOn DATETIME2(0),
	@ApprovedBy UNIQUEIDENTIFIER NULL,
	@CertificateID BIGINT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Certificate360History] ([RequestID],
		[RefID],
		[RequestorID],
		[RequestorName],
		[AgentID],
		[AgentName],
		[Duration],
		[IssuedOn],
		[ExpiresOn],
		[SerialNo],
		[ApprovedOn],
		[ApprovedBy],
		[CertificateID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@RequestID,
		@RefID,
		@RequestorID,
		@RequestorName,
		@AgentID,
		@AgentName,
		@Duration,
		@IssuedOn,
		@ExpiresOn,
		@SerialNo,
		@ApprovedOn,
		@ApprovedBy,
		@CertificateID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapCertificate360ToIngredients]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Map Certificate360 to ingredients
-- =============================================
CREATE PROCEDURE [dbo].[MapCertificate360ToIngredients]
	@ID BIGINT,
	@IngredientIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [Certificate360Ingredients]
	SELECT @ID, [Val]
	FROM @IngredientIDs
	WHERE [Val] NOT IN (SELECT [IngredientID]
						FROM [Certificate360Ingredients]
						WHERE [CertificateID] = @ID)

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapCertificate360ToMenus]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Map Certificate360 to menus
-- =============================================
CREATE PROCEDURE [dbo].[MapCertificate360ToMenus]
	@ID BIGINT,
	@MenuIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [Certificate360Menus]
	SELECT @ID, [Val]
	FROM @MenuIDs
	WHERE [Val] NOT IN (SELECT [MenuID]
						FROM [Certificate360Menus]
						WHERE [CertificateID] = @ID)

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapCertificate360ToTeams]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Map Certificate360 to team
-- =============================================
CREATE PROCEDURE [dbo].[MapCertificate360ToTeams]
	@ID BIGINT,
	@TeamIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [Certificate360Teams]
	SELECT @ID, [Val]
	FROM @TeamIDs
	WHERE [Val] NOT IN (SELECT [HalalTeamID]
						FROM [Certificate360Teams]
						WHERE [CertificateID] = @ID)

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateCertificate360]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 24-Aug-2020
-- Description:	Update certificate360 instance
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCertificate360]
	@ID BIGINT,
	@Number VARCHAR(60),
	@Status SMALLINT,
	@Template SMALLINT,
	@Scheme SMALLINT,
	@SubScheme SMALLINT,
	@IssuedOn DATETIME2(0) NULL,
	@ExpiresOn DATETIME2(0),
	@SerialNo VARCHAR(36),
	@CustomerID UNIQUEIDENTIFIER,
	@CustomerName NVARCHAR(150),
	@RequestorID UNIQUEIDENTIFIER,
	@RequestorName NVARCHAR(150),
	@AgentID UNIQUEIDENTIFIER NULL,
	@AgentName NVARCHAR(150),
	@PremiseID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Certificate360]
	SET [Number] = @Number,
		[Status] = @Status,
		[Template] = @Template,
		[Scheme] = @Scheme,
		[SubScheme] = @SubScheme,
		[IssuedOn] = @IssuedOn,
		[ExpiresOn] = @ExpiresOn,
		[SerialNo] = @SerialNo,
		[CustomerID] = @CustomerID,
		[CustomerName] = @CustomerID,
		[RequestorID] = @RequestorID,
		[RequestorName] = @RequestorName,
		[AgentID] = @AgentID,
		[AgentName] = @AgentName,
		[PremiseID] = @PremiseID,
		[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertAutoRenewalLog]    Script Date: 18/12/2020 1:37:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 9/12/2020
-- Description:	Insert auto renewal log
-- =============================================
CREATE PROCEDURE [dbo].[InsertAutoRenewalLog]
	-- Add the parameters for the stored procedure here
	@Number varchar(60),
	@ExpiresOn datetime2(0),
	@ID BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[AutoRenewalTriggerLog]([Number],[ExpiresOn],[CreatedOn],[ModifiedOn])
	VALUES(@Number,@ExpiresOn,GETUTCDATE(),GETUTCDATE())

	SET @ID = SCOPE_IDENTITY()


END
GO

/****** Object:  StoredProcedure [dbo].[SelectCertificate360]    Script Date: 2/2/2021 7:15:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 02-Feb-2021
-- Description:	Stored procedure to retrieve certificate360 with specified parameters
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificate360]
	@PremiseIDs [BigIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT c.[ID] AS [CertificateID], c.*, sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
        p.[ID] as [PremID], p.*,
        m.[ID] as [MenuID], m.*
    FROM [Certificate360] c
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]
    INNER JOIN [Premise] p ON c.[PremiseID] = p.[ID]
    LEFT JOIN [Certificate360Menus] cm ON cm.[CertificateID] = c.[ID]
    LEFT JOIN [Menu] m ON m.[ID] = cm.[MenuID]
    WHERE (NOT EXISTS (SELECT 1 FROM @PremiseIDs) OR C.PremiseID IN (SELECT [Val] FROM @PremiseIDs))

END
GO

/****** Object:  StoredProcedure [dbo].[GetCertificate360ByID]    Script Date: 16/3/2021 5:37:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 16-Mar-2021
-- Description:	Get certificate 360 details by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360ByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		cer.*, 
		csl.[Text] AS StatusText,
		scl.[Text] AS SchemeText, 
		susl.[Text] AS SubSchemeText, 
		pre.ID AS PremiseID , 
		pre.*,
		ptl.[Text] AS TypeText, 
		ht.ID AS HalalTeamID,
		ht.*
	FROM [dbo].[Certificate360] cer
		INNER JOIN [dbo].CertificateStatusLookup csl ON cer.[Status] = csl.ID
		LEFT JOIN [dbo].SchemeLookup scl ON cer.Scheme = scl.ID
		LEFT JOIN [dbo].SubSchemeLookup susl ON cer.SubScheme = susl.ID
		LEFT JOIN [dbo].Premise pre ON cer.PremiseID = pre.ID
		LEFT JOIN [dbo].[PremiseTypeLookup] ptl ON pre.[Type] = ptl.ID
		LEFT JOIN [dbo].Certificate360Teams ct ON ct.CertificateID = cer.ID
		LEFT JOIN [dbo].HalalTeam ht ON ht.ID = ct.HalalTeamID
	WHERE cer.ID = @ID
END
GO

/****** Object:  StoredProcedure [dbo].[GetCertificate360History]    Script Date: 16/3/2021 6:20:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 16-Mar-2021
-- Description:	Get Certificate 360 History by Certificate ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360History]
	@CertificateID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	ch.*
		FROM		[dbo].[Certificate360History] ch
		WHERE		ch.CertificateID = @CertificateID
END
GO

/****** Object:  StoredProcedure [dbo].[GetCertificate360Ingredients]    Script Date: 16/3/2021 6:20:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 16-Mar-2021
-- Description:	Get Certificate360 Ingredients by Certificate ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360Ingredients]
	@CertificateID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	ing.*
		FROM	[dbo].Certificate360Ingredients cis
			LEFT JOIN Ingredient ing on ing.ID = cis.IngredientID
		WHERE	cis.CertificateID = @CertificateID
END
GO

/****** Object:  StoredProcedure [dbo].[GetCertificate360Menus]    Script Date: 16/3/2021 6:20:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 16-Mar-2021
-- Description:	Get Certificate360 Menus by Certificate ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360Menus]
	@CertificateID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	men.*
		FROM	[dbo].Certificate360Menus cms
			LEFT JOIN Menu men on men.ID = cms.MenuID
		WHERE	cms.CertificateID = @CertificateID
END
GO

/****** Object:  StoredProcedure [dbo].[GetCertificate360WithIngredient]    Script Date: 22/3/2021 2:50:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 22-Mar-2021
-- Description:	Get certificate 360 detail with ingredient
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate360WithIngredient]
	@Name NVARCHAR(255),
	@Brand NVARCHAR(255) NULL,
	@SupplierName NVARCHAR(255) NULL,
	@CertifyingBody NVARCHAR(255) NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	cer.*, csl.[Text] AS StatusText,
			sl.[Text] AS SchemeText, 
			susl.[Text] AS SubSchemeText, 			
			cus.[ID] AS CustID, 
			cus.*,
			co.ID AS [CustCodeID], 
			co.*, 
			cg.ID AS [CustCodeGroupID], 
			cg.*,
			offi.ID AS [OfficerID], 
			offi.*,
			prem.[ID] AS PremID, 
			prem.*
	FROM	[Certificate360] cer
			INNER JOIN CertificateStatusLookup csl ON cer.[Status] = csl.[ID]
			LEFT JOIN SchemeLookup sl ON cer.[Scheme] = sl.[ID]
			LEFT JOIN SubSchemeLookup susl ON cer.[SubScheme] = susl.[ID]
			LEFT JOIN Certificate360Ingredients ci ON cer.[ID] = ci.[CertificateID]
			LEFT JOIN Ingredient ing on ing.ID = ci.[IngredientID]
			LEFT JOIN Premise prem ON cer.[PremiseID] = prem.[ID]
			LEFT JOIN Customer cus ON cer.[CustomerID] = cus.[ID]
			LEFT JOIN [Code] co ON co.[ID] = cus.[CodeID] AND co.[Type]=0
			LEFT JOIN [Code] cg ON cg.[ID] = cus.[GroupCodeID] AND cg.[Type]=1
			LEFT JOIN [Officer] offi ON offi.[ID] = cus.[OfficerInCharge]
	WHERE 	ing.IsDeleted = 0
			AND (@Name IS NULL OR ing.[Text] = @Name)
			AND (@Brand IS NULL OR ing.[BrandName] = @Brand)
			AND (@SupplierName IS NULL OR ing.[SupplierName] = @SupplierName)
			AND (@CertifyingBody IS NULL OR ing.[CertifyingBodyName] = @CertifyingBody)
END
GO