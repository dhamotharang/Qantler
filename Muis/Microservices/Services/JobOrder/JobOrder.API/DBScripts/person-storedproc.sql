/****** Object:  StoredProcedure [dbo].[GetPerson]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dev, Sahu
-- Create date: 18-July-2020
-- Description:	Stored procedure to get person by an AltID
-- =============================================
CREATE PROCEDURE [dbo].[GetPerson]
	@AltID varchar(30)	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT p.*, it.Text as IDTypeText
	FROM Person p
	LEFT JOIN IDTypeLookup it on it.ID = p.IDType
	WHERE AltID = @AltID
	AND IsDeleted = 0

END


GO
/****** Object:  StoredProcedure [dbo].[GetPersonByID]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 22-June-2020
-- Description:	Stored procedure to get person by id
-- =============================================
CREATE PROCEDURE [dbo].[GetPersonByID]
   @ID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*, idl.[Text] as [IDTypeText],
		c.[ID] as [ContactID], c.*, cil.[Text] as [TypeText]
	FROM [Person] p
	LEFT JOIN [IDTypeLookup] idl ON idl.[ID] = p.[IDType]
	LEFT JOIN [PersonContacts] pc ON pc.[PersonID] = p.[ID]
	LEFT JOIN [ContactInfo] c ON c.[ID] = pc.[ContactID]
	LEFT JOIN [ContactInfoTypeLookup] cil ON cil.[ID] = c.[Type]
	WHERE p.[ID] = @ID;
END
GO

/****** Object:  StoredProcedure [dbo].[ValidatePersonStatus]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dev, Sahu
-- Create date: 22-July-2020
-- Description:	Stored procedure to validate person by AltID
-- =============================================
CREATE PROCEDURE [dbo].[ValidatePersonStatus]
	@AltID varchar(30),
	@valid bit output
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM Person WHERE AltID = @AltID)
		BEGIN
			SET @valid = 0
		END
    ELSE
		BEGIN
			SET @valid = 1
		END	
END
GO

/****** Object:  StoredProcedure [dbo].[MapContactInfoToPerson]    Script Date: 26-02-2021 10:48:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25 Feb 2021
-- Description:	Insert records to person contacts
-- =============================================
CREATE PROCEDURE [dbo].[MapContactInfoToPerson]
	@PersonID uniqueidentifier,
	@ContactID bigint
AS
BEGIN
	SET NOCOUNT ON;

	Insert into PersonContacts([PersonID],[ContactID])
	values (@PersonID,@ContactID)

	Return 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertPerson]    Script Date: 08-03-2021 17:08:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25-Feb-2021
-- Description:	Stored procedure to insert person
-- =============================================
CREATE PROCEDURE [dbo].[InsertPerson]
    @ID UNIQUEIDENTIFIER,
    @Salutation nvarchar(10) null,
	@Name nvarchar(150) null,
	@DOB datetime null,
	@Designation nvarchar(100) null,
	@DesignationOther nvarchar(100) null,
	@IDType smallint null,
	@AltID nvarchar(30) null,
	@PassportIssuingCountry nvarchar(100) null
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Person] ([ID],
		[Salutation],
		[Name],
		[DOB],
		[Designation],
		[DesignationOther],
		[IDType],
		[AltID],
		[PassportIssuingCountry],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
    VALUES (@ID,
		@Salutation,
		@Name,
		@DOB,
		@Designation,
		@DesignationOther,
		@IDType,
		@AltID,
		@PassportIssuingCountry,
        GETUTCDATE(),
        GETUTCDATE(),
        0);      

    RETURN 0;
END