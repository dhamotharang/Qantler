/****** Object:  StoredProcedure [dbo].[InsertPremise]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to insert premise
-- =============================================
CREATE PROCEDURE [dbo].[InsertPremise]
	@ID bigint,
	@IsLocal bit,
	@Name nvarchar(150) NULL,
	@Type smallint,
	@Area float NULL,
	@Schedule varchar(50) NULL,
	@BlockNo varchar(15) NULL,
	@UnitNo varchar(5) NULL,
	@FloorNo varchar(5) NULL,
	@BuildingName nvarchar(100) NULL,
	@Address1 nvarchar(150) NULL,
	@Address2 nvarchar(150) NULL,
	@City nvarchar(100) NULL,
	@Province nvarchar(100) NULL,
	@Country nvarchar(100) NULL,
	@Postal varchar(20) NULL,
	@Longitude decimal(9, 6) NULL,
	@Latitude decimal(9, 6) NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM [dbo].[Premise] WHERE [ID] = @ID)
	BEGIN
		INSERT INTO [dbo].[Premise] ([ID],
			[IsLocal],
			[Name],
			[Type],
			[Area],
			[Schedule],
			[BlockNo],
			[UnitNo],
			[FloorNo],
			[BuildingName],
			[Address1],
			[Address2],
			[City],
			[Province],
			[Country],
			[Postal],
			[Longitude],
			[Latitude],
			[CreatedOn],
			[ModifiedOn],
			[IsDeleted])
		VALUES (@ID,
			@IsLocal,
			@Name,
			@Type,
			@Area,
			@Schedule,
			@BlockNo,
			@UnitNo,
			@FloorNo,
			@BuildingName,
			@Address1,
			@Address2,
			@City,
			@Province,
			@Country,
			@Postal,
			@Longitude,
			@Latitude,
			GETUTCDATE(),
			GETUTCDATE(),
			0)
	END

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[GetPremises]    Script Date: 4/3/2021 2:04:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to Get premises
-- =============================================
CREATE PROCEDURE [dbo].[GetPremises]
	@CaseID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Premise] AS P
	INNER JOIN [dbo].[CasePremises] AS CP ON P.ID = CP.PremiseID
	WHERE [IsDeleted] = 0 
	AND (@CaseID IS NULL OR CP.CaseID = @CaseID)

END