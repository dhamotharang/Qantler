/****** Object:  StoredProcedure [dbo].[InsertOrReplaceCertificate]    Script Date: 22-01-2021 18:31:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25 Jan 2021
-- Description:	Stored procedure to insert or replace certificate
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrReplaceCertificate] 
	@Number VARCHAR(60),
	@Status BIGINT,
	@Scheme SMALLINT,
	@SubScheme SMALLINT NULL,
	@IssuedOn DATETIME NULL,
	@StartsFrom DATETIME NULL,
	@ExpiresOn DATETIME NULL,
	@CustomerID UNIQUEIDENTIFIER,
	@PremiseID BIGINT,
	@IsDeleted BIT
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [Certificate] WHERE [NUMBER] = @Number and [IsDeleted] = 0)
    BEGIN
        UPDATE [Certificate]
        SET [Status]=@Status,
			        [Scheme]=@Scheme,
			        [SubScheme]=@SubScheme,
			        [IssuedOn]=@IssuedOn,
			        [StartsFrom]=@StartsFrom,
			        [ExpiresOn]=@ExpiresOn,
			        [CustomerID]=@CustomerID,
			        [PremiseID]=@PremiseID,
              [ModifiedOn] = GETUTCDATE()
        WHERE [NUMBER] = @Number and [IsDeleted] = 0
    END
    ELSE
    BEGIN
        INSERT INTO [Certificate] ([Number],
            [Status],
            [Scheme],
            [SubScheme],
            [IssuedOn],
            [StartsFrom],
            [ExpiresOn],
            [PremiseID],
			      [CustomerID],
            [CreatedOn],
            [ModifiedOn],
            [IsDeleted])
        VALUES (@Number,
			      @Status,
			      @Scheme,
			      @SubScheme,
			      @IssuedOn,
			      @StartsFrom,
			      @ExpiresOn,
			      @PremiseID,
			      @CustomerID,
            GETUTCDATE(),
            GETUTCDATE(),
            0);
    END
    
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCertificateByPremise]    Script Date: 28-01-2021 18:00:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 28-Jan-2021
-- Description:	Stored procedure to delete certificate by premise with same scheme and subscheme
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCertificateByPremise]
	@PremiseID bigint,
	@Scheme smallint,
	@SubScheme smallint NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Certificate] SET [IsDeleted] = 1, [ModifiedOn] = GETUTCDATE() WHERE [PremiseID] = @PremiseID and [Scheme] = @Scheme 
	and ([SubScheme] IS NULL or [SubScheme] = @SubScheme) and [IsDeleted] = 0

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[SelectCertificateBasic]    Script Date: 02-02-2021 17:11:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 29-Jan-2021
-- Description:	Stored procedure to select certificate by number, premise, scheme, subscheme and customer
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificateBasic] 
	@Number VARCHAR(60) NULL,
	@Status BIGINT NULL,
	@Scheme SMALLINT NULL,
	@SubScheme SMALLINT NULL,
	@CustomerID UNIQUEIDENTIFIER NULL,
	@PremiseID BIGINT NULL,
	@IsDeleted BIT NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [Certificate] 
	WHERE (@Number IS NULL OR [NUMBER] = @Number)
    AND (@Status IS NULL OR [Status] = @Status)
	AND (@Scheme IS NULL OR [Scheme] = @Scheme)
	AND	(@SubScheme IS NULL OR [SubScheme] = @SubScheme)
    AND (@CustomerID IS NULL OR [CustomerID] = @CustomerID)
	AND (@PremiseID IS NULL OR [PremiseID] = @PremiseID)
	AND (@IsDeleted IS NULL or [IsDeleted] = @IsDeleted)
END
GO