/****** Object:  StoredProcedure [dbo].[GetCustomerByID]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chandrasekhar N
-- Create date: 28-July-2020
-- Description:	Stored procedure to get Details  of customer
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerByID]	
	@ID as uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  c.*, cs.Text as [StatusText], it.Text as [IDTypeText],
		p.[ID] as [PID], p.*
	FROM Customer c
	LEFT JOIN [CustomerStatusLookup] cs on cs.ID = c.Status
	LEFT JOIN [IDTypeLookup] it on it.ID = c.IDType
	LEFT JOIN [Customer] p ON p.[ID] = c.[ParentID]
	WHERE c.ID = @ID AND c.isDeleted = 0 

END


GO

/****** Object:  StoredProcedure [dbo].[GetCustomerContactInfo]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 22-June-2020
-- Description:	Stored procedure to get contact info of customer
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerContactInfo]
	@Name as nvarchar(150),
	@AltID as varchar(30)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT c.*, cs.Text as StatusText, it.Text as IDText
	into #custemptable
	FROM Customer c
	LEFT JOIN CustomerStatusLookup cs on cs.ID = c.Status
	LEFT JOIN IDTypeLookup it on it.ID = c.IDType
	WHERE Name = @Name AND AltID = @AltID
	AND IsDeleted = 0

	
	SELECT p.*, pt.Text as TypeText
	into #premisetemptable
	FROM Premise p
	LEFT JOIN PremiseTypeLookup pt on pt.ID = p.Type
	WHERE p.CustomerID = (SELECT ID FROM CUSTOMER WHERE Name = @Name
	AND AltID = @AltID AND IsDeleted = 0) AND p.IsDeleted = 0;

	SELECT  c.*,  
	pr.ID as [PremiseID], pr.*	
	FROM #custemptable c
	LEFT JOIN #premisetemptable pr on pr.CustomerID = c.ID
	WHERE c.Name = @Name AND c.AltID = @AltID AND c.isDeleted = 0 

	
END

GO
/****** Object:  StoredProcedure [dbo].[InsertCustomer]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert customer.
-- =============================================
CREATE PROCEDURE [dbo].[InsertCustomer]
	@ID UNIQUEIDENTIFIER,
	@Name NVARCHAR(150),
	@IDType SMALLINT,
	@AltID NVARCHAR(30),
	@status SMALLINT,
	@ParentID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Customer] ([ID],
		[Name],
		[IDType],
		[AltID],
		[Status],
		[ParentID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@ID,
		@Name,
		@IDType,
		@AltID,
		@Status,
		@ParentID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

END
GO
/****** Object:  StoredProcedure [dbo].[SelectCustomer]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to select customer.
-- =============================================
CREATE PROCEDURE [dbo].[SelectCustomer]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT c.*, itl.[Text] as [IDTypeText], csl.[Text] as [StatusText]
    FROM [Customer] c
    LEFT JOIN [CustomerStatusLookup] csl on csl.ID = c.Status
    LEFT JOIN [IDTypeLookup] itl ON itl.[ID] = c.[IDType]
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateCustomer]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to update customer.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomer]
	@ID UNIQUEIDENTIFIER,
	@Name NVARCHAR(150),
	@IDType SMALLINT,
	@AltID NVARCHAR(30),
	@status SMALLINT,
	@ParentID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Customer]
	SET [Name] = @Name,
		[IDType] = @IDType,
		[AltID] = @AltID,
		[Status] = @Status,
		[ParentID] = @ParentID,
		[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[ValidateCustomerStatus]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chandrasekhar N
-- Create date: 12-June-2020
-- Description:	Stored procedure to validate customer sttaus by AltID
-- =============================================
CREATE PROCEDURE [dbo].[ValidateCustomerStatus] 
@AltID nvarchar(1000),
@Name nvarchar(1000),
@valid bit output
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Customer WHERE AltID = @AltID AND Name=@Name)
    BEGIN
        SET @valid = 0
    END
    ELSE
    BEGIN
        SET @valid = 1
    END
END
GO