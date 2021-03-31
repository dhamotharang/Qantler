/****** Object:  StoredProcedure [dbo].[GetBillByID]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to get bill based on specified id
-- =============================================
CREATE PROCEDURE [dbo].[GetBillByID]
    @ID BIGINT NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.*, bsl.[Text] AS [StatusText], brtl.[Text] as [RequestTypeText], btl.[Text] as [TypeText],
        bli.[ID] AS [LineItemID], bli.*,
        p.[ID] as [PaymentID], p.*, psl.[Text] AS [StatusText], pml.[Text] AS [ModeText], pmtl.[Text] AS [MethodText]
    FROM [Bill] b
    INNER JOIN [BillStatusLookup] bsl ON bsl.[ID] = b.[Status]
    LEFT JOIN [BillRequestTypeLookup] brtl ON brtl.[ID] = b.[RequestType]
    INNER JOIN [BillTypeLookup] btl ON btl.[ID] = b.[Type]
    LEFT JOIN [BillLineItem] bli ON bli.[BillID] = b.[ID]
    LEFT JOIN [PaymentBills] pb ON pb.[BillID] = b.[ID]
    LEFT JOIN [Payment] p ON p.[ID] = pb.[PaymentID]
    LEFT JOIN [PaymentStatusLookup] psl ON psl.[ID] = p.[Status]
    LEFT JOIN [PaymentModeLookup] pml ON pml.[ID] = p.[Mode]
    LEFT JOIN [PaymentMethodLookup] pmtl ON pmtl.[ID] = p.[Method]
    WHERE b.[ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[DeleteAllBillLineItem]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to update bill.
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllBillLineItem]
    @BillID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [BillLineItem]
    WHERE [BillID] = @BillID

    RETURN;
END


GO
/****** Object:  StoredProcedure [dbo].[InsertBill]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to insert bill.
-- =============================================
CREATE PROCEDURE [dbo].[InsertBill]
    @RefNo VARCHAR(36),
    @Status SMALLINT,
    @Type SMALLINT,
    @RequestType SMALLINT,
    @AccountID UNIQUEIDENTIFIER,
    @AccountName NVARCHAR(150),
    @InvoiceNo VARCHAR(36),
    @Amount DECIMAL(12,2),
    @GSTAmount DECIMAL(12,2),
    @GST DECIMAL(12,2),
    @IssuedOn DATETIME2(0),
    @DueOn DATETIME2(0),
    @RequestID BIGINT NULL,
    @RefID VARCHAR(36) NULL,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceAccount @AccountID, ''

	INSERT INTO [Bill]
    VALUES (@RefNo,
        @Status,
        @Type,
        @RequestType,
        @AccountID,
        @InvoiceNo,
        @Amount,
        @GSTAmount,
        @GST,
        @IssuedOn,
        @DueOn,
        @RequestID,
        @RefID,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertBillLineItem]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to insert bill lineitem.
-- =============================================
CREATE PROCEDURE [dbo].[InsertBillLineItem]
    @SectionIndex SMALLINT,
    @Section NVARCHAR(150),
    @Index SMALLINT,
    @CodeID BIGINT,
    @Code VARCHAR(15),
    @Descr NVARCHAR(255),
    @Qty DECIMAL(12,2),
    @UnitPrice DECIMAL(12,2),
    @Amount DECIMAL(12,2),
    @GSTAMount DECIMAL(12,2),
    @GST DECIMAL(12,2),
    @WillRecord BIT,
    @BillID BIGINT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [BillLineItem]
    VALUES (@SectionIndex ,
        @Section ,
        @Index ,
        @CodeID ,
        @Code,
        @Descr ,
        @Qty ,
        @UnitPrice,
        @Amount,
        @GSTAMount,
        @GST,
        @WillRecord,
        @BillID,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[SelectBill]    Script Date: 22-02-2021 12:15:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to select bill base on specified fields.
-- =============================================
CREATE PROCEDURE [dbo].[SelectBill]
    @ID BIGINT NULL,
	  @RequestID BIGINT NULL,
    @RefNo VARCHAR(36) NULL,
	  @CustomerName NVARCHAR(150) NULL,
    @RefID NVARCHAR(36) NULL,
    @Status SMALLINT NULL,
    @From DATETIME2(0) NULL,
    @To DATETIME2(0) NULL,
	  @InvoiceNo VARCHAR(36) NULL,
	  @Type SMALLINT NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.*, bsl.[Text] AS [StatusText], brtl.[Text] as [RequestTypeText], btl.[Text] as [TypeText], acc.[Name] as CustomerName,
        bli.[ID] AS [LineItemID], bli.*,
        p.[ID] as [PaymentID], p.*, psl.[Text] AS [StatusText], pml.[Text] AS [ModeText], pmtl.[Text] AS [MethodText]
    FROM [Bill] b
    INNER JOIN [BillStatusLookup] bsl ON bsl.[ID] = b.[Status]
    LEFT JOIN [BillRequestTypeLookup] brtl ON brtl.[ID] = b.[RequestType]
    INNER JOIN [BillTypeLookup] btl ON btl.[ID] = b.[Type]
	  INNER JOIN [Account] acc on b.[AccountID] = acc.[ID]
    LEFT JOIN [BillLineItem] bli ON bli.[BillID] = b.[ID]
    LEFT JOIN [PaymentBills] pb ON pb.[BillID] = b.[ID]
    LEFT JOIN [Payment] p ON p.[ID] = pb.[PaymentID]
    LEFT JOIN [PaymentStatusLookup] psl ON psl.[ID] = p.[Status]
    LEFT JOIN [PaymentModeLookup] pml ON pml.[ID] = p.[Mode]
    LEFT JOIN [PaymentMethodLookup] pmtl ON pmtl.[ID] = p.[Method]
    WHERE (@RequestID IS NULL OR b.[RequestID] = @RequestID)
    AND (@ID IS NULL OR b.[ID] = @ID)
	  AND (@InvoiceNo IS NULL OR b.[InvoiceNo] LIKE CONCAT('%', @InvoiceNo, '%'))
    AND (@RefNo IS NULL OR b.[RefNo] LIKE CONCAT('%', @RefNo, '%'))
    AND (@CustomerName IS NULL OR acc.[Name] LIKE CONCAT('%', @CustomerName, '%'))
    AND (@RefID IS NULL OR b.[RefID] LIKE CONCAT('%', @RefID, '%'))
    AND (@Status IS NULL OR b.[Status] = @Status)
	  AND (@Type IS NULL OR b.[Type] = @Type)
    AND (@From IS NULL OR b.[IssuedOn] >= CAST(@From AS DATE))
    AND (@To IS NULL OR b.[IssuedOn] < DATEADD(DD, 1, CAST(@To AS DATE)))

    RETURN 0
END
/****** Object:  StoredProcedure [dbo].[UpdateBill]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to update bill.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBill]
    @ID BIGINT,
    @RefNo VARCHAR(36),
    @Status SMALLINT,
    @Type SMALLINT,
    @RequestType SMALLINT,
    @AccountID UNIQUEIDENTIFIER,
    @AccountName NVARCHAR(150),
    @InvoiceNo VARCHAR(36),
    @Amount DECIMAL(12,2),
    @GSTAmount DECIMAL(12,2),
    @GST DECIMAL(12,2),
    @IssuedOn DATETIME2(0),
    @DueOn DATETIME2(0),
    @RequestID BIGINT NULL,
    @RefID VARCHAR(36) NULL
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceAccount @AccountID, @AccountName

    UPDATE [Bill]
    SET [RefNo] = @RefNo,
        [Status] = @Status,
        [Type] = @Type,
        [RequestType] = @RequestType,
        [AccountID] = @AccountID,
        [InvoiceNo] = @InvoiceNo,
        [Amount] = @Amount,
        [GSTAmount] = @GSTAmount,
        [GST] = @GST,
        [IssuedOn] = @IssuedOn,
        [DueOn] = @DueOn,
        [RequestID] = @RequestID,
        [RefID] = @RefID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID
        
    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateBillStatus]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to update bill status.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBillStatus]
	@ID BIGINT,
    @Status SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Bill]
    SET [Status] = @Status,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END

GO