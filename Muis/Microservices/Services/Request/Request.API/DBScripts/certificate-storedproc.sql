/****** Object:  StoredProcedure [dbo].[AcknowledgeCertificateBatch]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to acknowledge a certificate batch.
-- =============================================
CREATE PROCEDURE [dbo].[AcknowledgeCertificateBatch]
    @BatchID BIGINT,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @acknowledgeOn DATETIME2(0) = GETUTCDATE()

    -- Insert log to Request
    DECLARE @LogID BIGINT
    DECLARE @LogTextTemplate NVARCHAR(2000)
    DECLARE @LogText NVARCHAR(2000)

    EXEC GetTranslationText 0, 'MuftiAcknowledge', @ActionText = @LogTextTemplate OUTPUT

    DECLARE @requestID BIGINT
    DECLARE @number VARCHAR(60)

    DECLARE request_cursor CURSOR FOR
    SELECT c.[RequestID], c.[Number]
    FROM [Request] r
    INNER JOIN [Certificate] c ON c.[RequestID] = r.[ID]
    INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
    WHERE bc.[BatchID] = @BatchID

    OPEN request_cursor

    FETCH NEXT FROM request_cursor
    INTO @requestID, @number

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @LogText = FORMATMESSAGE(@LogTextTemplate, @number)

        EXEC InsertLog 0, NULL, @LogText, NULL, NULL, NULL, NULL, @ID = @LogID OUTPUT

        INSERT INTO [RequestLog]
        VALUES (@requestID, @LogID)

        FETCH NEXT FROM request_cursor
        INTO @requestID, @number
    END

    CLOSE request_cursor
    DEALLOCATE request_cursor

    -- Update certificate issued on
    UPDATE [c]
    SET [IssuedOn] = @acknowledgeOn,
        [ModifiedOn] = @acknowledgeOn
    FROM [Certificate] c
    INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
    WHERE bc.[BatchID] = @BatchID

    UPDATE [Certificate360History]
    SET [IssuedOn] = @acknowledgeOn
    WHERE [ID] IN (SELECT hist.[ID]
        FROM [Certificate360History] hist
        INNER JOIN [Certificate360] c360 ON c360.[ID] = hist.[CertificateID]
        INNER JOIN [Certificate] c ON c.[Number] = c360.[Number]
        INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
        WHERE bc.[BatchID] = @BatchID
        AND c360.[IssuedOn] IS NULL
        AND hist.[IssuedOn] IS NULL)

    UPDATE [c360]
    SET [IssuedOn] = @acknowledgeOn,
        [ModifiedOn] = @acknowledgeOn
    FROM [Certificate360] c360
    INNER JOIN [Certificate] c ON c.[Number] = c360.[Number]
    INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
    WHERE bc.[BatchID] = @BatchID
    AND c360.[IssuedOn] IS NULL

    -- Update certificate batch status
    UPDATE [CertificateBatch]
    SET [Status] = 200,
        [AcknowledgedOn] = @acknowledgeOn,
        [LastAction] = @acknowledgeOn,
        [ModifiedOn] = @acknowledgeOn
    WHERE [ID] = @BatchID

    RETURN 0;
END

GO
/****** Object:  StoredProcedure [dbo].[GenerateCertificateSequence]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to generate certificate sequence no.
-- =============================================
CREATE PROCEDURE [dbo].[GenerateCertificateSequence]
	@Result INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @year INT = YEAR(GETUTCDATE())

    IF NOT EXISTS (SELECT 1 FROM [SerialNoMap] WHERE [Val] = @year AND [Key] = 1)
    BEGIN
        ALTER SEQUENCE [CertificateSequence]
        RESTART WITH 1

        INSERT INTO [SerialNoMap]
        VALUES (1, @year)
    END

    SET @Result = NEXT VALUE FOR [CertificateSequence]
END


GO
/****** Object:  StoredProcedure [dbo].[GenerateCertificateSerialNo]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to generate certificate serial no.
-- =============================================
CREATE PROCEDURE [dbo].[GenerateCertificateSerialNo]
	@Result INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @year INT = YEAR(GETUTCDATE())

    IF NOT EXISTS (SELECT 1 FROM [SerialNoMap] WHERE [Val] = @year AND [Key] = 0)
    BEGIN
        ALTER SEQUENCE [CertificateSerialNo]
        RESTART WITH 1

        INSERT INTO [SerialNoMap]
        VALUES (0, @year)
    END

    SET @Result = NEXT VALUE FOR [CertificateSerialNo]
END


GO
/****** Object:  StoredProcedure [dbo].[GetCertificateBatchByCode]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate batch by specified code
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificateBatchByCode]
	@Code VARCHAR(15)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [CertificateBatch]
    WHERE [Code] = @Code
END


GO
/****** Object:  StoredProcedure [dbo].[GetCertificateBatchByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate batch with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificateBatchByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT b.*, ctl.[Text] AS [TemplateText], cbsl.[Text] as [StatusText],
        c.[ID] AS [CertificateID], c.*, rtl.[Text] as [RequestTypeText], sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
        p.[ID] as [PremID], p.*,
        mp.[ID] as [MailPremID], mp.*,
        co.[ID] as [CommentID], co.*, coo.[Name] as [UserName]
    FROM [CertificateBatch] b
    INNER JOIN [CertificateTemplateLookup] ctl ON ctl.[ID] = b.[Template]
    INNER JOIN [CertificateBatchStatusLookup] cbsl ON cbsl.[ID] = b.[Status]
    INNER JOIN [BatchCertificates] bc ON bc.[BatchID] = b.[ID]
    INNER JOIN [Certificate] c ON c.[ID] = bc.[CertificateID]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = c.[RequestType]
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]
    INNER JOIN [Premise] p ON c.[PremiseID] = p.[ID]
    INNER JOIN [Premise] mp ON c.[MailingPremiseID] = mp.[ID]
    LEFT JOIN [Comment] co ON co.[BatchID] = b.[ID]
    LEFT JOIN [Officer] coo ON coo.[ID] = co.[UserID]
    WHERE b.[ID] = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[GetCertificateBatchByIDBasic]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate batch basic details with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificateBatchByIDBasic]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [CertificateBatch]
    WHERE [ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[GetCertificateBatchByIDFull]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate batch with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificateBatchByIDFull]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT b.*, ctl.[Text] as [TemplateText], cbsl.[Text] as [StatusText],
        c.[ID] AS [CertificateID], c.*, rtl.[Text] as [RequestTypeText], sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
        p.[ID] as [PremID], p.*,
        mp.[ID] as [MailPremID], mp.*,
        co.[ID] as [CommentID], co.*, coo.[Name] as [UserName],
        m.[ID] as [MenuID], m.*
    FROM [CertificateBatch] b
    INNER JOIN [CertificateTemplateLookup] ctl ON ctl.[ID] = b.[Template]
    INNER JOIN [CertificateBatchStatusLookup] cbsl ON cbsl.[ID] = b.[Status]
    INNER JOIN [BatchCertificates] bc ON bc.[BatchID] = b.[ID]
    INNER JOIN [Certificate] c ON c.[ID] = bc.[CertificateID]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = c.[RequestType]
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]
    INNER JOIN [Premise] p ON c.[PremiseID] = p.[ID]
    INNER JOIN [Premise] mp ON c.[PremiseID] = mp.[ID]
    LEFT JOIN [Comment] co ON co.[BatchID] = b.[ID]
    LEFT JOIN [Officer] coo ON coo.[ID] = co.[UserID]
    LEFT JOIN [CertificateMenus] cm ON cm.[CertificateID] = c.[ID]
    LEFT JOIN [Menu] m ON m.[ID] = cm.[MenuID]
    WHERE b.[ID] = @ID
END
GO

/****** Object:  StoredProcedure [dbo].[InsertCertificate]    Script Date: 29/1/2021 2:47:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to insert certificate.
-- =============================================
-- Modification History
-- 29/01/2021 Rameshkumar
-- Included Status and CodeID new columns
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertificate]
    @RequestType SMALLINT,
    @Number VARCHAR(60),
	  @Status SMALLINT,
	  @CodeID BIGINT,
    @Template SMALLINT,
    @IsCTC BIT,
    @Scheme SMALLINT,
    @SubScheme SMALLINT NULL,
    @IssuedOn DATETIME2(0),
    @StartsFrom DATETIME2(0),
    @ExpiresOn DATETIME2(0),
    @SerialNo VARCHAR(36),
    @CustomerID UNIQUEIDENTIFIER,
    @CustomerName NVARCHAR(150),
    @PremiseID BIGINT,
    @MailingPremiseID BIGINT,
    @Remarks NVARCHAR(500),
    @RequestID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Certificate] ([RequestType],
        [Number],
		    [Status],
        [CodeID],
        [Template],
        [IsCertifiedTrueCopy],
        [Scheme],
        [SubScheme],
        [IssuedOn],
        [StartsFrom],
        [ExpiresOn],
        [SerialNo],
        [CustomerID],
        [CustomerName],
        [PremiseID],
        [MailingPremiseID],
        [Remarks],
        [RequestID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@RequestType,
        @Number,
		    @Status,
		    @CodeID,
        @Template,
        @IsCTC,
        @Scheme,
        @SubScheme,
        @IssuedOn,
        @StartsFrom,
        @ExpiresOn,
        @SerialNo,
        @CustomerID,
        @CustomerName,
        @PremiseID,
        @MailingPremiseID,
        @Remarks,
        @RequestID,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()
END
GO

/****** Object:  StoredProcedure [dbo].[InsertCertificateBatch]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to create certificate batch
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertificateBatch]
	@Code VARCHAR(12),
    @Description VARCHAR(255),
    @Template SMALLINT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [CertificateBatch]
    VALUES (@Code,
        @Description,
        100,
        @Template,
        NULL,
        NULL,
        NULL,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()
    
	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapCertificate]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to map Generated certificate.
-- =============================================
CREATE PROCEDURE [dbo].[MapCertificate]
    @BatchID BIGINT,
    @RequestID BIGINT,
    @Scheme SMALLINT,
    @CertificateID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [BatchCertificates]
    VALUES (@BatchID, @CertificateID)

    UPDATE [RequestLineItem]
    SET [CertificateID] = @CertificateID
    WHERE [RequestID] = @RequestID
    AND [Scheme] = @Scheme

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[MapCertificateBatchToFile]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to map cetificate batch to file.
-- =============================================
CREATE PROCEDURE [dbo].[MapCertificateBatchToFile]
    @BatchID BIGINT,
    @FileID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [CertificateBatch]
    SET [FileID] = @FileID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @BatchID

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[MapMenuToCertificate]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to map menu to certificate
-- =============================================
CREATE PROCEDURE [dbo].[MapMenuToCertificate]
    @CertificateID BIGINT,
    @MenuID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [CertificateMenus]
    VALUES (@CertificateID, @MenuID)

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[SelectCertificate]    Script Date: 1/2/2021 4:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate with specified parameters
-- =============================================
-- Modification History
-- 01/02/2021 Ramesh
-- Included @PremiseID new columns
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificate]
	@PremiseID BIGINT NULL,
    @IDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT c.[ID] AS [CertificateID], c.*, rtl.[Text] as [RequestTypeText], sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
        p.[ID] as [PremID], p.*,
        mp.[ID] as [MailPremID], mp.*,
        m.[ID] as [MenuID], m.*
    FROM [Certificate] c
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = c.[RequestType]
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]
    INNER JOIN [Premise] p ON c.[PremiseID] = p.[ID]
    INNER JOIN [Premise] mp ON mp.[ID] = c.[MailingPremiseID]
    LEFT JOIN [CertificateMenus] cm ON cm.[CertificateID] = c.[ID]
    LEFT JOIN [Menu] m ON m.[ID] = cm.[MenuID]
    WHERE c.[ID] IN (SELECT [Val] FROM @IDs)
	AND (@PremiseID IS NULL OR p.ID = @PremiseID)
END

GO
/****** Object:  StoredProcedure [dbo].[SelectCertificateBatch]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve certificate batch with specified parameters
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificateBatch]
	@From DATETIME2(0),
    @To DATETIME2(0),
    @RequestID BIGINT NULL,
    @Status SMALLINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT b.*, sl.[Text] as [StatusText], ctl.[Text] as [TemplateText]
    FROM [CertificateBatch] b
    INNER JOIN [CertificateTemplateLookup] ctl ON ctl.[ID] = b.[Template]
    INNER JOIN [CertificateBatchStatusLookup] sl ON sl.[ID] = b.[Status]
    INNER JOIN [BatchCertificates] bc ON bc.[BatchID] = b.[ID]
    INNER JOIN [Certificate] c ON c.[ID] = bc.[certificateID]
    WHERE b.[CreatedOn] >= @From
    AND b.[Status] IN (SELECT [Val] FROM @Status)
    AND b.[CreatedOn] < CONVERT(DATE, @To)
    AND (@RequestID IS NULL OR c.[RequestID] = @RequestID)
    
	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateCertificateBatchStatus]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to update certificate batch status.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCertificateBatchStatus]
    @BatchID BIGINT,
    @Status SMALLINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LogID BIGINT
    DECLARE @LogTextTemplate NVARCHAR(2000)
    DECLARE @LogText NVARCHAR(2000)

    IF @Status = 400
    BEGIN
        EXEC GetTranslationText 0, 'BatchCertificateSentToCourier', @ActionText = @LogTextTemplate OUTPUT
    END
    ELSE IF @Status = 500
    BEGIN
        EXEC GetTranslationText 0, 'BatchCertificateDelivered', @ActionText = @LogTextTemplate OUTPUT
    END

    -- Update request status to Close if payment is completed
    IF @Status = 500
    BEGIN
        UPDATE r
        SET [Status] = 900,
            [ModifiedOn] = GETUTCDATE()
        FROM [Request] r
        INNER JOIN [Certificate] c ON c.[RequestID] = r.[ID]
        INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
        WHERE r.[Status] >= 800
        AND r.[Status] < 900
        AND bc.[BatchID] = @BatchID
        AND r.[ID] NOT IN (SELECT [RequestID] FROM [Certificate] ic, [CertificateBatch] icb, [BatchCertificates] ibc
            WHERE ic.[ID] = ibc.[CertificateID]
            AND icb.[ID] = ibc.[BatchID]
            AND icb.[ID] <> @BatchID
            AND icb.[Status] <> 500)
    END

    -- Insert logs to request
    IF @LogTextTemplate IS NOT NULL
    BEGIN
        DECLARE @requestID BIGINT
        DECLARE @number VARCHAR(60)

        DECLARE request_cursor CURSOR FOR
        SELECT c.[RequestID], c.[Number]
        FROM [Request] r
        INNER JOIN [Certificate] c ON c.[RequestID] = r.[ID]
        INNER JOIN [BatchCertificates] bc ON bc.[CertificateID] = c.[ID]
        WHERE bc.[BatchID] = @BatchID

        OPEN request_cursor

        FETCH NEXT FROM request_cursor
        INTO @requestID, @number

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @LogText = FORMATMESSAGE(@LogTextTemplate, @number)

            EXEC InsertLog 0, NULL, @LogText, NULL, NULL, NULL, NULL, @ID = @LogID OUTPUT

            INSERT INTO [RequestLog]
            VALUES (@requestID, @LogID)

            FETCH NEXT FROM request_cursor
            INTO @requestID, @number
        END

        CLOSE request_cursor
        DEALLOCATE request_cursor

    END

    UPDATE [CertificateBatch]
    SET [Status] = @Status,
        [LastAction] = GETUTCDATE(),
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @BatchID

    RETURN 0;
END
GO

/****** Object:  StoredProcedure [dbo].[ExecCertificateDelivery]    Script Date: 14/1/2021 3:41:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 13-Jan-2021
-- Description:	Stored procedure to Update certificate delivery status
-- =============================================
CREATE PROCEDURE [dbo].[ExecCertificateDelivery]
	-- Add the parameters for the stored procedure here
	@CertificateID BIGINT,
    @Status SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @LogID BIGINT
    DECLARE @LogTextTemplate NVARCHAR(2000)
    DECLARE @LogText NVARCHAR(2000)  
    
    EXEC GetTranslationText 0, 'BatchCertificateDelivered', @ActionText = @LogTextTemplate OUTPUT    

    -- Update request status to Close if payment is completed     
       
	UPDATE [Request] 
	SET [Status] = 900,
		[ModifiedOn] = GETUTCDATE()
	FROM [Request] r 
	INNER JOIN [Certificate] c ON r.[ID] = c.[RequestID]
	WHERE C.ID = @CertificateID AND	r.[Status] >= 800 AND r.[Status] < 900
			
	UPDATE [Request] 
	SET [StatusMinor] = 810,
		[ModifiedOn] = GETUTCDATE()
	FROM [Request] r 
	INNER JOIN [Certificate] c ON r.[ID] = c.[RequestID]
	WHERE C.ID = @CertificateID	 AND r.[Status] = 700	
   

    -- Insert logs to request
    IF @LogTextTemplate IS NOT NULL
	BEGIN
        DECLARE @requestID BIGINT
        DECLARE @number VARCHAR(60)

		SET @number = (SELECT [Number] FROM [Certificate] WHERE ID = @CertificateID)
		SET @requestID = (SELECT [RequestID] FROM [Certificate] WHERE ID = @CertificateID)

        SET @LogText = FORMATMESSAGE(@LogTextTemplate, @number)

        EXEC InsertLog 0, NULL, @LogText, NULL, NULL, NULL, NULL, @ID = @LogID OUTPUT

        INSERT INTO [RequestLog]
        VALUES (@requestID, @LogID)
	END	          

    UPDATE [Certificate]
    SET [Status] = @Status,        
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @CertificateID

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertificatesForDelivery]    Script Date: 14/1/2021 3:41:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 11-Jan-2021
-- Description:	Stored procedure to retrieve certificates list with specified params
-- =============================================
CREATE PROCEDURE [dbo].[GetCertificatesForDelivery]
	@CustomerCode varchar(36),
	@CustomerName nvarchar(150),
	@Premise nvarchar(150),
	@PostalCode varchar(20),
	@Status [SmallIntType] READONLY,
	@IssuedOnFrom datetime2,
	@IssuedOnTo datetime2,
	@SerialNo varchar(36)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 	
        c.*, rtl.[Text] as [RequestTypeText], sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
		cdsl.[Text] as [StatusText], req.[RefID] as RefID,       
        p.[ID] as [PremID], p.*,
        mp.[ID] as [MailPremID], mp.*,
		cd.[ID] as [CodeID], cd.*
      
    FROM [CertificateBatch] b
    INNER JOIN [CertificateTemplateLookup] ctl ON ctl.[ID] = b.[Template]
    INNER JOIN [CertificateBatchStatusLookup] cbsl ON cbsl.[ID] = b.[Status]
    INNER JOIN [BatchCertificates] bc ON bc.[BatchID] = b.[ID]
    INNER JOIN [Certificate] c ON c.[ID] = bc.[CertificateID]
	INNER JOIN [CertificateDeliveryStatusLookup] cdsl ON cdsl.ID = c.[Status]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = c.[RequestType]
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
	INNER JOIN [Code] cd on c.CodeID = cd.ID AND cd.[Type] = 0
	INNER JOIN [Request] req on c.RequestID = req.ID
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]

	LEFT JOIN (SELECT ip.*,
                irp.[RequestID],
                irp.[ChangeType],
                irp.[IsPrimary],
                dbo.FormatPremiseWOPostal([Name], [BlockNo], [Address1], [Address2], [FloorNo], [UnitNo], [BuildingName], [Province], [City], [Country]) AS [Text]
                FROM [Premise] ip,
                    [RequestPremise] irp
                WHERE irp.[IsPrimary] = 1
                AND [IsDeleted] = 0
                AND ip.[ID] = irp.[PremiseID]) p ON p.[RequestID] = c.RequestID
    LEFT JOIN [PremiseTypeLookup] ptl ON ptl.[ID] = p.[Type]

   
    INNER JOIN [Premise] mp ON c.[MailingPremiseID] = mp.[ID]
 
    WHERE 
	 (@CustomerCode IS NULL OR cd.[Value] LIKE '%' + @CustomerCode + '%') AND
	 (@CustomerName IS NULL OR c.CustomerName LIKE '%' + @CustomerName + '%') AND
	 (@PostalCode IS NULL OR p.Postal LIKE @PostalCode + '%') AND	
	 (NOT EXISTS (SELECT 1 FROM @Status) OR c.[Status] IN (SELECT [Val] FROM @Status)) AND
	 (@SerialNo IS NULL OR c.SerialNo = @SerialNo) AND
	 (@Premise IS NULL OR p.[Text] LIKE '%' + @Premise + '%') AND
	 (@IssuedOnFrom IS NULL OR c.IssuedOn >= @IssuedOnFrom ) AND
	 (@IssuedOnTo IS NULL OR c.IssuedOn <= DATEADD(day, 1, @IssuedOnTo) )
	 AND b.[Status] >= 300
		 
END
GO

/****** Object:  StoredProcedure [dbo].[SelectCertificateBatchFull]    Script Date: 8/3/2021 12:03:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 05-Mar-2021
-- Description:	Stored procedure to retrieve certificate batch (scheme,subscheme,premise) with specified parameters
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificateBatchFull]
	@From DATETIME2(0),
    @To DATETIME2(0),
    @RequestID BIGINT NULL,
    @Status SMALLINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT b.*, ctl.[Text] AS [TemplateText], cbsl.[Text] as [StatusText],
        c.[ID] AS [CertificateID], c.*, rtl.[Text] as [RequestTypeText], sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
        p.[ID] as [PremID], p.*,
        mp.[ID] as [MailPremID], mp.*,
        co.[ID] as [CommentID], co.*, coo.[Name] as [UserName]
    FROM [CertificateBatch] b
    INNER JOIN [CertificateTemplateLookup] ctl ON ctl.[ID] = b.[Template]
    INNER JOIN [CertificateBatchStatusLookup] cbsl ON cbsl.[ID] = b.[Status]
    INNER JOIN [BatchCertificates] bc ON bc.[BatchID] = b.[ID]
    INNER JOIN [Certificate] c ON c.[ID] = bc.[CertificateID]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = c.[RequestType]
    INNER JOIN [SchemeLookup] sl ON sl.[ID] = c.[Scheme]
    LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = c.[SubScheme]
    INNER JOIN [Premise] p ON c.[PremiseID] = p.[ID]
    INNER JOIN [Premise] mp ON c.[MailingPremiseID] = mp.[ID]
    LEFT JOIN [Comment] co ON co.[BatchID] = b.[ID]
    LEFT JOIN [Officer] coo ON coo.[ID] = co.[UserID]
    WHERE b.[CreatedOn] >= @From
    AND b.[Status] IN (SELECT [Val] FROM @Status)
    AND b.[CreatedOn] < CONVERT(DATE, @To)
    AND (@RequestID IS NULL OR c.[RequestID] = @RequestID)
    
	RETURN 0
END
GO