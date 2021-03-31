/****** Object:  StoredProcedure [dbo].[Insertpremise]    Script Date: 19/3/2021 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19/03/2021
-- Description:	 Store Procedure Insert premises 
-- =============================================
CREATE PROCEDURE [dbo].[InsertPremise]  
	@Type SMALLINT,
    @BuildingName NVARCHAR(100) NULL,
    @BlockNo VARCHAR(15) NULL,
    @FloorNo VARCHAR(5) NULL,
    @UnitNo VARCHAR(5) NULL,
    @Street NVARCHAR(150) NULL,
    @Postal VARCHAR(20) NULL,
		@IsLocal BIT,
		@CustomerID UNIQUEIDENTIFIER NULL,
		@ID BIGINT OUTPUT
AS
BEGIN	
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Premise]([Type],
		[BuildingName],
		[BlockNo],
		[FloorNo],
		[UnitNo],
		[Address1],
		[Postal],
		[IsLocal],
		[CustomerID],
		[CreatedOn],
		[ModifiedOn])
	VALUES (@Type,
		@BuildingName,
		@BlockNo,
		@FloorNo,
		@UnitNo,
		@Street,
		@Postal,
		@IsLocal,
		@CustomerID,
		GETUTCDATE(),
		GETUTCDATE())

  SET @ID = SCOPE_IDENTITY()
END
GO

/****** Object:  StoredProcedure [dbo].[SelectPremise]    Script Date: 19/3/2021 6:15:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Feb-2021 
-- Description:	Stored procedure to retrieve premise.
-- =============================================
CREATE PROCEDURE [dbo].[SelectPremise]
	@CustomerID UNIQUEIDENTIFIER NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT P.*, PTL.[Text] AS TypeText,
			C.[ID] AS 'PremiseCustomerID', C.*,IDT.[Text] AS 'IDTypeText'
    FROM 
	[dbo].[Premise] AS P
	INNER JOIN [dbo].[PremiseTypeLookup] AS PTL ON PTL.[ID] = P.[Type]
	LEFT JOIN [dbo].[Customer] AS C ON P.[CustomerID] = C.[ID]
	LEFT JOIN [dbo].[IDTypeLookup] AS IDT ON C.[IDType] = IDT.[ID]
	WHERE (@CustomerID IS NULL OR P.[CustomerID] = @CustomerID)
    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[GetPremiseByID]    Script Date: 19/3/2021 6:15:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Feb-2021 
-- Description:	Stored procedure to retrieve premise.
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiseByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT P.*, PTL.[Text] AS TypeText,
			C.[ID] AS 'PremiseCustomerID', C.*,IDT.[Text] AS 'IDTypeText'
    FROM 
	[dbo].[Premise] AS P
	INNER JOIN [dbo].[PremiseTypeLookup] AS PTL ON PTL.[ID] = P.[Type]
	LEFT JOIN [dbo].[Customer] AS C ON P.[CustomerID] = C.[ID]
	LEFT JOIN [dbo].[IDTypeLookup] AS IDT ON C.[IDType] = IDT.[ID]
	WHERE  P.ID = @ID

    RETURN 0
END