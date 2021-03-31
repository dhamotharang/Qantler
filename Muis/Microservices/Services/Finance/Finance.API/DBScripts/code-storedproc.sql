/****** Object:  StoredProcedure [dbo].[GetFinanceTransactionCode]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetFinanceTransactionCode]
@ID BIGINT NULL,
@Code VARCHAR(15) NULL,
@GLEntry VARCHAR(36) NULL,
@Description NVARCHAR(2000) NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tc.*, tc.[Code] as [Transaction code details], tc.[GLEntry] AS [GLEntry], tc.[Text] AS [Text],
	p.[ID] AS [PriceID],p.*

	FROM [dbo].[TransactionCode] tc
	 LEFT JOIN [dbo].[Price] p ON p.[TransactionCodeID] = tc.[ID]
	WHERE tc.IsDeleted=0
	AND (@ID IS NULL OR tc.[ID] = @ID)
	AND (@Code IS NULL OR tc.[Code] LIKE '%' + @Code + '%')
	AND (@GLEntry IS NULL OR tc.[GLEntry] = @GLEntry)
	AND (@Description IS NULL OR tc.[Text] LIKE '%' + @Description + '%')
 END
GO
/****** Object:  StoredProcedure [dbo].[GetFinanceTransactionCodeByID]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetFinanceTransactionCodeByID]
@ID BIGINT	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tc.*, tc.[Code] as [Transaction code details], tc.[GLEntry] AS [GLEntry], tc.[Text] AS [Text], tc.[Currency] as [Currency],
	c.[ID] AS [ConditionID],c.*,
	l.[ID] AS [LogID], l.*,	o.[Name] AS [UserName],
	p.[ID] AS [PriceID],p.*

	FROM [dbo].[TransactionCode] tc
	 LEFT JOIN [dbo].[Price] p ON p.[TransactionCodeID] = tc.[ID]
	 LEFT JOIN [dbo].[Condition] c ON c.[TransactionCodeID] = tc.[ID]
	 LEFT JOIN [dbo].[TransactionCodeLog] tcl ON tcl.[TransactionCodeID] = tc.[ID]
	 LEFT JOIN [Log] l ON l.[ID] = tcl.[LogID]
	 LEFT JOIN [LogicalLookup] ll ON ll.[ID] = c.[Logical]
	 LEFT JOIN [FieldLookup] fl ON fl.[ID] = c.[Field]
	 LEFT JOIN [OperatorLookup] ol ON ol.[ID] = c.[Operator]
	 LEFT JOIN [Officer] o ON o.[ID] = l.[UserId]
	WHERE tc.ID = @ID and tc.IsDeleted=0
	ORDER BY l.CreatedOn DESC
 END
GO
/****** Object:  StoredProcedure [dbo].[GetTransactionCodeByCode]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve transaction code base on specified code.
-- =============================================
CREATE PROCEDURE [dbo].[GetTransactionCodeByCode]
	@Code varchar(15),
	@RefDate datetime2(0) NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.*,
		p.[ID] as [PriceID], p.*,
		c.[ID] AS [ConditionID], c.*
	FROM [TransactionCode] t
	LEFT JOIN [Price] p ON p.[TransactionCodeID] = t.[ID]
	LEFT JOIN [Condition] c ON c.[TransactionCodeID] = t.[ID]
	WHERE t.[Code] = @Code
	AND (@RefDate IS NULL OR p.[EffectiveFrom] <= @RefDate)
	ORDER BY p.[EffectiveFrom] DESC
END


GO