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

	SELECT p.*,
		c.[ID] as [ContactID], c.*, cil.[Text] as [TypeText]
	FROM [Person] p
	LEFT JOIN [PersonContacts] pc ON pc.[PersonID] = p.[ID]
	LEFT JOIN [ContactInfo] c ON c.[ID] = pc.[ContactID]
	LEFT JOIN [ContactInfoTypeLookup] cil ON cil.[ID] = c.[Type]
	WHERE p.[ID] = @ID;
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
CREATE PROCEDURE [dbo].[MapPersonContactInfo]
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
/****** Object:  StoredProcedure [dbo].[GetTranslation]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-June-2020
-- Description:	Stored procedure to insert person
-- =============================================
CREATE PROCEDURE [dbo].[InsertPerson]
	@ID [UniqueIdentifier],
	@Name [nvarchar](150)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Person] ([ID],
		[Name],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@ID,
		@Name,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

		RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[UpdatePerson]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-June-2020
-- Description:	Stored procedure to update person
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePerson]
	@ID [UniqueIdentifier],
	@Name [nvarchar](150)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Person]
	SET [Name] = @Name,
			[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

		RETURN 0
END

GO