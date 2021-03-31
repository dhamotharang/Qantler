/****** Object:  StoredProcedure [dbo].[GetCustomerByID]    Script Date: 18/3/2021 11:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 05-Oct-2020
-- Description:	Stored procedure to get Details  of customer
-- =============================================
-- Modified Detail
-- Author:		Ramesh
-- Create date: 18-Mar-2021
-- Description:	Included officer
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerByID]	
	@ID as uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  c.*,
		cc.[ID] as [CodeID], cc.*,
		gc.[ID] as [GroupCodeID], gc.*,
		offi.ID AS [OfficerID], offi.*
	FROM [Customer] c
	LEFT JOIN [Code] cc ON cc.[ID] = c.[CodeID]
	LEFT JOIN [Code] gc ON gc.[ID] = c.[GroupCodeID]
	LEFT JOIN [Officer] offi ON offi.[ID] = c.[OfficerInCharge]
	WHERE c.ID = @ID
END
GO

/****** Object:  StoredProcedure [dbo].[InsertCustomer]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rameshkumar
-- Create date: 05/10/2020
-- Description:	 Store Procedure Insert customer 
-- =============================================
CREATE PROCEDURE [dbo].[InsertCustomer]  
	@CustomerID uniqueidentifier,
  @CustomName NVARCHAR(150) NULL,  
	@IDType SMALLINT NULL,
	@AltID NVARCHAR(30) NULL
AS
BEGIN	
	SET NOCOUNT ON;
	INSERT INTO Customer([ID], [Name], [IDType], [AltID])
	VALUES (@CustomerID, @CustomName, @IDType, @AltID)	
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateCustomer]    Script Date: 18/3/2021 11:07:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 05/10/2020
-- Description:	 Store Procedure to update customer
-- =============================================
-- Modified Details
-- Author:		Ramesh M
-- Create date: 18/03/2021
-- Description:	 Included new column OfficerInCharge
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomer]  
	@ID UNIQUEIDENTIFIER,
  @Name NVARCHAR(150) NULL,
	@CodeID BIGINT NULL,
	@GroupCOdeID BIGINT NULL,
	@OfficerInCharge  UNIQUEIDENTIFIER = NULL
AS
BEGIN	
	SET NOCOUNT ON;

	UPDATE [Customer]
	SET [Name] = @Name,
		[CodeID] = @CodeID,
		[GroupCodeID] = @GroupCodeID,
		[OfficerInCharge] = @OfficerInCharge
	WHERE [ID] = @ID
END
GO

/****** Object:  StoredProcedure [dbo].[SelectCustomer]    Script Date: 18/3/2021 1:08:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 09-Mar-2021
-- Description:	Stored procedure to retrieve customer with specified parameters
-- =============================================
-- Modified Detail
-- Author:		Ramesh
-- Create date: 18-Mar-2021
-- Description:	Included officer
-- =============================================
CREATE PROCEDURE [dbo].[SelectCustomer]
	@ID UNIQUEIDENTIFIER NULL,
	@Code VARCHAR(36) NULL,
	@GroupCode VARCHAR(36) NULL,
	@CertificateNo VARCHAR(60) NULL,
	@Name NVARCHAR(150) NULL,
	@Premise NVARCHAR(150) NULL,
	@PremiseID BIGINT NULL,
	@Status [SmallIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	c.*, csl.[Text] AS [StatusText], sl.[Text] as [SchemeText], susl.[Text] as [SubSchemeText], cu.[ID] AS [CustID], cu.*,
						co.ID AS [CustCodeID], co.*, cg.ID AS [CustCodeGroupID], cg.*,
						p.[ID] as [PremID], p.*, offi.ID AS [OfficerID], offi.*
    FROM [Certificate360] c
				LEFT JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
				LEFT JOIN [SubSchemeLookup] susl ON susl.[ID] = c.[SubScheme]
				RIGHT JOIN [Customer] cu ON c.CustomerID = cu.ID
				LEFT JOIN [Code] co ON co.[ID] = cu.[CodeID] AND co.[Type]=0
				LEFT JOIN [Code] cg ON cg.[ID] = cu.[GroupCodeID] AND cg.[Type]=1
				LEFT JOIN [Officer] offi ON offi.[ID] = cu.[OfficerInCharge]
				LEFT JOIN [CertificateStatusLookup] csl ON csl.[ID] = c.[Status]
				LEFT JOIN (SELECT ip.*,
                dbo.FormatPremise([Name], [BlockNo], [Address1], [Address2], [FloorNo], [UnitNo], [BuildingName], [Province], [City], [Country], [Postal]) AS [Text]
                FROM [Premise] ip) p ON p.ID = c.[PremiseID]
    WHERE (NOT EXISTS (SELECT 1 FROM @Status) OR c.[Status] IN (SELECT [Val] FROM @Status))
					AND (@Code IS NULL OR co.[Value] = @Code)
					AND (@GroupCode IS NULL OR cg.[Value] = @GroupCode)
					AND (@ID IS NULL OR c.[CustomerID] = @ID)
					AND (@Name IS NULL OR cu.[Name] LIKE '%' + @Name + '%')
					AND (@Premise IS NULL OR p.[Text] LIKE '%' + @Premise + '%')
					AND (@PremiseID IS NULL OR p.[ID] = @PremiseID)
					AND (@CertificateNo IS NULL OR c.Number = @CertificateNo)
END
GO

/****** Object:  StoredProcedure [dbo].[GetCustomerRecentRequest]    Script Date: 11/3/2021 11:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 10-Mar-2021
-- Description:	Stored procedure to retrieve recent application for specified customer
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerRecentRequest]	
	@ID as uniqueidentifier,
	@RowFrom AS BIGINT,
	@RowCount AS BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT r.*, rsl.[Text] as [StatusText], rtl.[Text] as [TypeText], 
        rli.[ID] as [LineItemID], rli.*, sl.[Text] as [SchemeText], [ssl].[Text] as [SubSchemeText]
    FROM [Request] r
		INNER JOIN [RequestStatusLookup] rsl ON rsl.[ID] = r.[Status]
		LEFT JOIN [RequestStatusMinorLookup] rsml ON rsml.[ID] = r.[StatusMinor]
		INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = r.[Type]    
		LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
		LEFT JOIN [SchemeLookup] sl ON sl.[ID] = rli.[Scheme]
		LEFT JOIN [SubSchemeLookup] [ssl] ON [ssl].[ID] = rli.[SubScheme]
	WHERE r.[CustomerID] = @ID
	ORDER BY r.[SubmittedOn] DESC
	OFFSET @RowFrom ROWS 
	FETCH FIRST @RowCount ROWS ONLY;
END
GO

/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 19/3/2021 11:33:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Mar-2021
-- Description:	Stored procedure to get customers
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomers]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT CUS.*,
	C.ID AS 'CusCodeID',C.*,
	GC.ID AS 'CusGroupCodeID',GC.* FROM [dbo].[Customer] AS CUS
	LEFT JOIN [dbo].[Code] AS C ON C.[ID] = CUS.[CodeID]
	LEFT JOIN [dbo].[Code] AS GC ON GC.[ID] = CUS.[GroupCodeID]
	
END