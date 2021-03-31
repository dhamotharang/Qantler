/****** Object:  StoredProcedure [dbo].[GetPaymentByID]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve payment info.
-- =============================================
CREATE PROCEDURE [dbo].[GetPaymentByID]
	@ID BIGINT NULL
AS
BEGIN
	SET NOCOUNT ON;

  SELECT p.*, psl.[Text] AS [StatusText], pml.[Text] AS [ModeText], pmtl.[Text] AS [MethodText], o.[Name] as [ProcessedByName],
        b.[ID] AS [BillID], b.*, bsl.[Text] AS [StatusText], brtl.[Text] AS [RequestTypeText], btl.[Text] AS [TypeText],
        bli.[ID] as [BillLineItemID], bli.*,
		[no].[ID] as [NoteID], [no].*,
		att.ID as [AttachmentID], att.*,
        [off].[ID] as [OfficierID], [off].*,
        [log].[ID] as [LogID], [log].*,
        [per].[ID] as [PersonID], [per].*,
        [ci].[ID] as [ContactID], [ci].*, [cil].[Text] as [TypeText]
    FROM [Payment] p
    INNER JOIN [PaymentStatusLookup] psl ON psl.[ID] = p.[Status]
    INNER JOIN [PaymentModeLookup] pml ON pml.[ID] = p.[Mode]
    INNER JOIN [PaymentMethodLookup] pmtl ON pmtl.[ID] = p.[Method]	
    LEFT JOIN [Officer] o ON o.[ID] = p.[ProcessedBy]
    LEFT JOIN [PaymentBills] pb ON pb.[PaymentID] = p.[ID]
    LEFT JOIN [Bill] b ON b.[ID] = pb.[BillID]
    LEFT JOIN [BillStatusLookup] bsl ON bsl.[ID] = b.[Status]
    LEFT JOIN [BillRequestTypeLookup] brtl ON brtl.[ID] = b.[RequestType]
    LEFT JOIN [BillTypeLookup] btl ON btl.[ID] = b.[Type]
    LEFT JOIN [BillLineItem] bli ON bli.[BillID] = b.[ID]
	LEFT JOIN [PaymentNotes] pno ON pno.[PayID] = p.[ID]
	LEFT JOIN [Notes] [no] on pno.[NoteId] = [no].[ID]
    LEFT JOIN [Officer] [off] ON [off].[ID] = [no].[CreatedBy]
	LEFT JOIN [NotesAttachments] noa on [noa].[NotesID] = [no].[ID]
	LEFT JOIN [Attachment] att on att.[ID] = [noa].[AttachmentID]
	LEFT JOIN [PaymentLogs] plog ON plog.[PaymentID] = p.[ID]
	LEFT JOIN [Log] [log] on plog.[LogId] = [log].[ID]
    LEFT JOIN [Person] per ON per.[ID] = p.[ContactPersonID]
    LEFT JOIN [PersonContacts] pc ON pc.[PersonID] = per.[ID]
    LEFT JOIn [ContactInfo] ci ON ci.[ID] = pc.[ContactID]
    LEFT JOIn [ContactInfoTypeLookup] cil ON cil.[ID] = ci.[Type]

    WHERE p.[ID] = @ID
    END
GO
/****** Object:  StoredProcedure [dbo].[InsertPayment]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 4/11/2020
-- Description:	Insert Payment
-- =============================================
CREATE PROCEDURE [dbo].[InsertPayment]
		@AltID VARCHAR(36),
		@RefNo VARCHAR(36),
		@Status SMALLINT,
		@Mode SMALLINT,
		@Method SMALLINT,
		@AccountID UNIQUEIDENTIFIER,
		@Name NVARCHAR(150), 
		@TransactionNo VARCHAR(36),
		@ReceiptNo VARCHAR(36), 
		@Amount DECIMAL(12,2), 
		@GSTAmount DECIMAL(12,2), 
		@GST DECIMAL(12,2), 
		@PaidOn DATETIME2(0), 
		@ProcessedBy UNIQUEIDENTIFIER, 
		@ProcessedOn DATETIME2(0),
    @ContactPersonID UNIQUEIDENTIFIER NULL,
    @ID [Bigint] OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	  INSERT INTO [dbo].[Payment] ([AltID],
        [RefNo],
        [Status],
        [Mode],
        [Method],
        [AccountID],
        [Name],
        [TransactionNo],
        [ReceiptNo],
		    [Amount],
        [GSTAmount],
        [GST],
        [PaidOn],
        [ProcessedBy],
        [ProcessedOn],
        [ContactPersonID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@AltID,
        @RefNo,
        @Status,
        @Mode,
        @Method,
        @AccountID,
        @Name,
        @TransactionNo,
        @ReceiptNo,
        @Amount,
        @GSTAmount,
        @GST,
        @PaidOn,
        @ProcessedBy,
        @ProcessedOn,
        @ContactPersonID,
        GETUTCDATE(),
        GETUTCDATE(), 
		    0)

	  SET @ID = SCOPE_IDENTITY()

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[PaymentAction]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve payment info.
-- =============================================
CREATE PROCEDURE [dbo].[PaymentAction]
	@ID BIGINT,
    @Status SMALLINT,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;

	EXEC InsertOrReplaceOfficer @UserID, @UserName

    UPDATE [Payment]
    SET [Status] = @Status,
        [ProcessedBy] = @UserID,
        [ProcessedOn] = GETUTCDATE(),
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[SelectPayment]    Script Date: 13/3/2021 1:49:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve transaction code base on specified code.
-- =============================================
-- Modification History
-- 13/03/2021 Ramesh
-- Included Account ID in filter options
-- =============================================
ALTER PROCEDURE [dbo].[SelectPayment]
	@ID BIGINT NULL,
	@AccountID UNIQUEIDENTIFIER NULL,
	@Name NVARCHAR(150) NULL,
  @TransactionNo NVARCHAR(36) NULL,
  @Status SMALLINT NULL,
  @Mode SMALLINT NULL,
  @Method SMALLINT NULL,
  @From DATETIME2(0) NULL,
  @To DATETIME2(0) NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*, psl.[Text] AS [StatusText], pml.[Text] AS [ModeText], pmtl.[Text] AS [MethodText]
    FROM [Payment] p
    INNER JOIN [PaymentStatusLookup] psl ON psl.[ID] = p.[Status]
    INNER JOIN [PaymentModeLookup] pml ON pml.[ID] = p.[Mode]
    INNER JOIN [PaymentMethodLookup] pmtl ON pmtl.[ID] = p.[Method]
    WHERE (@ID IS NULL OR p.[ID] = @ID)
	  AND (@AccountID IS NULL OR p.[AccountID] = @AccountID)
    AND (@Name IS NULL OR p.[Name] LIKE CONCAT('%', @Name, '%'))
    AND (@TransactionNo IS NULL OR p.[TransactionNo] = @TransactionNo)
    AND (@Status IS NULL OR p.[Status] = @Status)
    AND (@Mode IS NULL OR p.[Mode] = @Mode)
    AND (@Method IS NULL OR p.[Method] = @Method)
    AND (@From IS NULL OR p.[PaidOn] >= CAST(@From AS DATE))
    AND (@To IS NULL OR p.[PaidOn] < DATEADD(DD, 1, CAST(@To AS DATE)))
END
GO
/****** Object:  StoredProcedure [dbo].[MapNotesAttachments]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 08-Jan-2021
-- Description:	Stored procedure to map attachment to notes
-- =============================================
CREATE PROCEDURE [dbo].[MapPaymentsNotes]
	@PaymentID BIGINT,
	@NoteID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [PaymentNotes]
    ([PayID],[NoteID])
	values (@PaymentID,@NoteID)

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapPaymentLog]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 05-M-2021
-- Description:	Stored procedure to map log to payment
-- =============================================
CREATE PROCEDURE [dbo].[MapPaymentsLog]
	@PaymentID BIGINT,
	@LogID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [PaymentLogs]
        ([PaymentID],[LogID])
	VALUES (@PaymentID,@LogID)
END
GO
/****** Object:  StoredProcedure [dbo].[MapPaymentBill]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Ang, Keyfe
-- Create date: 05-Mar-2021
-- Description:	Stored procedure to map bill to payment
-- =============================================
CREATE PROCEDURE [dbo].[MapPaymentBill]
	@PaymentID BIGINT,
	@BillID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [PaymentBills]
	VALUES (@PaymentID,
      @BillID)
END
GO

/****** Object:  StoredProcedure [dbo].[MapPaymentBank]    Script Date: 12-03-2021 09:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Vasu
-- Create date: qq-Mar-2021
-- Description:	Stored procedure to map bank to payment
-- =============================================
CREATE PROCEDURE [dbo].[MapPaymentBank]
	@PaymentID BIGINT,
	@BankID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [PaymentBanks]
	VALUES (@PaymentID,@BankID)

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[GetCustomerRecentPayment]    Script Date: 11/3/2021 11:23:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 10-Mar-2021
-- Description:	Stored procedure to retrieve recent payment for specified customer
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerRecentPayment]	
	@ID AS UNIQUEIDENTIFIER,
	@RowFrom AS BIGINT,
	@RowCount AS BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT p.*, psl.[Text] as [StatusText], pml.[Text] as [ModeText], pmel.[Text] as [MethodText]
    FROM [Payment] p
		INNER JOIN [PaymentStatusLookup] psl ON psl.[ID] = p.[Status]
		LEFT JOIN [PaymentModeLookup] pml ON pml.[ID] = p.[Mode]
		INNER JOIN [PaymentMethodLookup] pmel ON pmel.[ID] = p.[Method] 
	WHERE p.[AccountID] = @ID
	ORDER BY p.[PaidOn] DESC 
	OFFSET @RowFrom ROWS 
	FETCH FIRST @RowCount ROWS ONLY;
END
GO
