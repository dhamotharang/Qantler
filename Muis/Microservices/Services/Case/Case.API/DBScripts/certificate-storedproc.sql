-- =============================================
-- Author:		Israel
-- Create date: 10-Feb-2021
-- Description:	Stored procedure to insert certificate
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertificate]
	@Number varchar(60),
	@Scheme smallint,
	@SubScheme smallint NULL,
	@IssuedOn datetime2(0) NULL,
	@StartsFrom datetime2(0) NULL,
	@ExpiresOn datetime2(0),
	@SerialNo varchar(36) NULL,
	@PremiseID bigint,
	@CaseID bigint,
	@Status smallint,
	@SuspendedUntil datetime2(0) NULL
		
AS
BEGIN
	SET NOCOUNT ON;

		INSERT INTO [dbo].[Certificate] ([Number],
			[Scheme],			
			[SubScheme],					
			[IssuedOn],			
			[StartsFrom],						
			[ExpiresOn],
			[SerialNo],
			[SuspendedUntil],
			[Status],
			[PremiseID],
			[CaseID],
			[CreatedOn],
			[ModifiedOn],
			[IsDeleted])
		VALUES (@Number,
			@Scheme,
			@SubScheme,
			@IssuedOn,
			@StartsFrom,
			@ExpiresOn,
			@SerialNo,
			@SuspendedUntil,
			@Status,
			@PremiseID,
			@CaseID,
			GETUTCDATE(),
			GETUTCDATE(),
			0)

	RETURN 0
END

/****** Object:  StoredProcedure [dbo].[GetCertificate]    Script Date: 4/3/2021 3:45:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to Get certificate
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificate]
	@CaseID BIGINT =  NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.* FROM [dbo].[Certificate] AS C
	INNER JOIN [dbo].[CertificateStatusLookup] AS CL ON C.[Status] = CL.[ID]
	WHERE [IsDeleted] = 0 AND (@CaseID IS NULL OR C.[CaseID] = @CaseID)

END

/****** Object:  StoredProcedure [dbo].[UpdateCertificate]    Script Date: 4/3/2021 4:43:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 10-Feb-2021
-- Description:	Stored procedure to update certificate
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCertificate]
	@ID bigint,
	@Number varchar(60),
	@Scheme smallint,
	@SubScheme smallint NULL,
	@IssuedOn datetime2(0) NULL,
	@StartsFrom datetime2(0) NULL,
	@ExpiresOn datetime2(0),
	@SerialNo varchar(36) NULL,
	@PremiseID bigint,
	@CaseID bigint,
	@Status smallint,
	@SuspendedUntil datetime2(0) NULL
		
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE[dbo].[Certificate] 
		SET [Number] = @Number,
		[Scheme] = @Scheme,			
		[SubScheme] = @SubScheme,					
		[IssuedOn] = @IssuedOn,			
		[StartsFrom] = @StartsFrom,						
		[ExpiresOn] = @ExpiresOn,
		[SerialNo] = @SerialNo,
		[Status] = @Status,
		[SuspendedUntil] = @SuspendedUntil,
		[PremiseID] = @PremiseID,
		[CaseID] = @CaseID,
		[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

	RETURN 0
END

