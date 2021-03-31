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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Dev,Sahu
-- Create date: 21-JULY-2020>
-- Description:	Stored procedure to Insert person Details
-- =============================================
-- Modification History
-- Author:		Rameshu
-- Create date: 02-OCT-2020>
-- Description:	Stored procedure to Insert personContact Details and removed pesonID from ContactInfo insert.
-- 23-11-2020 Sathiya Priya
-- fix issue on multiple insert in percontacts table
-- =============================================
CREATE PROCEDURE [dbo].[InsertPerson] 
  @Person PersonType READONLY,
  @ContactInfo ContactInfoType READONLY

AS
BEGIN
 
	SET NOCOUNT ON;
 
    INSERT INTO Person([ID], [Salutation], [Name], [Nationality], [Gender],
	[DOB], [Designation],[DesignationOther],[IDTYPE], [AltID],
	[PassportIssuingCountry],[CreatedOn],[ModifiedOn],[IsDeleted])
	
	SELECT [ID],[Salutation], [Name], [Nationality], [Gender],
	[DOB],[Designation],[DesignationOther],[IDType],[AltID],
	[PassportIssuingCountry],(SELECT GETUTCDATE()),(SELECT GETUTCDATE()),0
	FROM @Person;

	declare @personid uniqueidentifier
	set @personid = (select [ID] from person where AltID = (select top 1 altid from @Person))
	

	IF EXISTS (SELECT 1 FROM @ContactInfo)

	BEGIN
		CREATE TABLE #CTINFO(ROWNUM INT, [Type] SMALLINT, [Value] NVARCHAR(2000),[IsPrimary] bit)
		INSERT INTO #CTINFO
		SELECT ROW_NUMBER() OVER (ORDER BY [Type]) AS ROWNUM,[Type], [Value] ,[IsPrimary] FROM @ContactInfo

		DECLARE @TOTCINFO INT
		DECLARE @CID BIGINT
		DECLARE @CINDEX INT
		SET @CINDEX = 1;
		SET @TOTCINFO = (SELECT COUNT(*) FROM #CTINFO)	
		WHILE(@CINDEX <= @TOTCINFO)
			BEGIN
				INSERT INTO ContactInfo([Type],[Value],[IsPrimary],[CreatedOn],[ModifiedOn],[IsDeleted])

				SELECT [Type], [Value], [IsPrimary] ,(SELECT GETUTCDATE()), (SELECT GETUTCDATE()), 0
				FROM #CTINFO
				WHERE ROWNUM = @CINDEX 

				SET @CID = SCOPE_IDENTITY();

				INSERT INTO [dbo].[PersonContacts]
				(PersonID  ,ContactID)
				VALUES(@personid,@CID)

				SET @CINDEX = @CINDEX + 1
			END
		
	END

END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TEBS
-- Create date: 5/8/2020
-- Description:	Update Person and Person Contact Info
-- =============================================
-- Modification History
-- 23/11/2020 SathiyaPriya
-- Issue fix, multiple insert into personcontacts
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePerson]
	-- Add the parameters for the stored procedure here
	@Person PersonType READONLY,
	@ContactInfo ContactInfoType READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Person]
		SET 
		[Salutation] = (SELECT Salutation FROM @Person),
		[Name] = (SELECT [Name] FROM @Person),
		[Nationality] = (SELECT Nationality FROM @Person),
		[Gender] = (SELECT Gender FROM @Person),
		[DOB] = (SELECT DOB FROM @Person),
		[Designation] = (SELECT Designation FROM @Person),
		[DesignationOther] = (SELECT DesignationOther FROM @Person),
		[IDType] = (SELECT IDType FROM @Person),		
		[PassportIssuingCountry] = (SELECT PassportIssuingCountry FROM @Person),		
		[ModifiedOn] = (SELECT GETUTCDATE())		
	WHERE [AltID] = (SELECT ALTID FROM @Person)

	IF EXISTS (SELECT 1 FROM @ContactInfo)
	BEGIN
		CREATE TABLE #CTINFO(ROWNUM INT, [Type] SMALLINT, [Value] NVARCHAR(2000),[IsPrimary] bit, [PersonID] uniqueidentifier)
		INSERT INTO #CTINFO
		SELECT ROW_NUMBER() OVER (ORDER BY [Type]) AS ROWNUM,[Type], [Value] ,[IsPrimary], [PersonID] FROM @ContactInfo

		DECLARE @TOTCINFO INT
		DECLARE @CID BIGINT
		DECLARE @CINDEX INT
		SET @CINDEX = 1;
		SET @TOTCINFO = (SELECT COUNT(*) FROM #CTINFO)	
		WHILE(@CINDEX <= @TOTCINFO)
			BEGIN
				IF EXISTS (select top 1 p.id from person p
								inner join PersonContacts pc on p.ID = pc.PersonID
								inner join ContactInfo c on pc.ContactID = c.ID
								where p.id = (SELECT ID FROM @Person) and c.[Type] = (select [Type] from #CTINFO where ROWNUM = @CINDEX ))				
					BEGIN
						UPDATE ContactInfo SET
						   [Value] = (select [Value] from #CTINFO where ROWNUM = @CINDEX ),
						   [IsPrimary] =(select [IsPrimary] from #CTINFO where ROWNUM = @CINDEX ),
						   [ModifiedOn] = (SELECT GETUTCDATE())
						   WHERE ID = (select top 1 c.id from person p
								inner join PersonContacts pc on p.ID = pc.PersonID
								inner join ContactInfo c on pc.ContactID = c.ID
								where p.id = (SELECT ID FROM @Person) and c.[Type] = (select [Type] from #CTINFO where ROWNUM = @CINDEX ))		
					END
				ELSE
					BEGIN
						INSERT INTO ContactInfo([Type],[Value],[IsPrimary],[CreatedOn],[ModifiedOn],[IsDeleted])

						SELECT [Type], [Value], [IsPrimary] ,(SELECT GETUTCDATE()), (SELECT GETUTCDATE()), 0
						FROM #CTINFO
						WHERE ROWNUM = @CINDEX 

						SET @CID = SCOPE_IDENTITY();

						INSERT INTO [dbo].[PersonContacts]
						(PersonID  ,ContactID)
						VALUES((SELECT ID FROM @Person),@CID)
					END

				SET @CINDEX = @CINDEX + 1
			END
		
	END	

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