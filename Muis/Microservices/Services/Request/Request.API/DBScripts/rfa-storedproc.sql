/****** Object:  StoredProcedure [dbo].[CleanRFA]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to clear RFA items
-- =============================================
CREATE PROCEDURE [dbo].[CleanRFA]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [RFALineItemAttachments]
	WHERE [LineItemID] IN (SELECT [ID] FROM [RFALineItem] WHERE [RFAID] = @ID)

	DELETE FROM [RFALineItem]
	WHERE [RFAID] = @ID

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[DeleteRFA]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to delete RFA
-- =============================================
CREATE PROCEDURE [dbo].[DeleteRFA]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [RFA]
	SET [IsDeleted] = 1
	WHERE [ID] = @ID

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[ExtendRFADueDate]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ExtendRFADueDate]
	@ID BIGINT,
	@Notes NVARCHAR(4000),
	@ToDate DATETIME2(0),
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

	Update RFA
	SET [DueOn] = @ToDate,
		ModifiedOn = GETUTCDATE()
	WHERE ID = @ID

	/* Insert logs */
	DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogID BIGINT
	DECLARE @LogParam NVARCHAR(2000) = CONCAT('[''', CONVERT(VARCHAR, @ToDate, 120), ''']')

	EXEC GetTranslation 0, 'RFAExtendDueDate', @Text = @LogText OUTPUT

	EXEC InsertLog 0, NULL, @LogText, @LogParam, @Notes, @UserID, @UserName, @ID = @LogID OUTPUT

	INSERT INTO [RFALog]
	VALUES (@ID, @LogID)

	RETURN 0
END



GO
/****** Object:  StoredProcedure [dbo].[GetRFAByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to get RFA by an ID
-- =============================================
CREATE PROCEDURE [dbo].[GetRFAByID]
	@ID bigint	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.*, rsl.[Text] as [StatusText], o.[Name] as [RaisedByName],
		li.[ID] as [LineItemID], li.*, sl.[Text] AS [SchemeText],
		rliaa.[ID] as [AttachmentID], rliaa.*,
		rep.ID as [ReplyID], rep.*,
		repaa.[ID] as [AttachmentID], repaa.*,
		log.[ID] as [LogID], log.*
	FROM [RFA] r
	LEFT JOIN [Officer] o ON o.[ID] = r.[RaisedBy]
	LEFT JOIN [RFAStatusLookup] rsl ON rsl.[ID] = r.[status]
	LEFT JOIN [RFALineItem] li ON li.[RFAID] = r.[ID]
	LEFT JOIN [SchemeLookup] sl ON sl.[ID] = li.[Scheme]
	LEFT JOIN [RFALineItemAttachments] lia ON lia.[LineItemID] = li.[ID]
	LEFT JOIN [Attachment] rliaa ON rliaa.[ID] = lia.[AttachmentID]
	LEFT JOIN [RFAReply] rep ON rep.[LineItemID] = li.[ID]
	LEFT JOIN [RFAReplyAttachments] repa ON repa.[ReplyID] = rep.[ID]
	LEFT JOIN [Attachment] repaa ON repaa.[ID] = repa.[AttachmentID]
	LEFT JOIN [RFALog] rlog ON rlog.[RFAID] = r.[ID]
	LEFT JOIN [Log] log ON rlog.[LogID] = log.[ID]
	WHERE r.[ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[InsertRFA]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 11-June-2020
-- Description:	Stored procedure to Insert RFA
-- =============================================
CREATE PROCEDURE [dbo].[InsertRFA]
	@Status SMALLINT,
	@RaisedBy UNIQUEIDENTIFIER,
	@RaisedByName NVARCHAR(150),
	@DueOn DATETIME2(0),
	@RequestID BIGINT,
	@ID BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;

	EXEC InsertOrReplaceOfficer @RaisedBy, @RaisedByName
	
	INSERT INTO [RFA] ([Status],
		[DueOn],
		[RaisedBy],
		[RequestID],
		[CreatedOn],
		[ModifiedOn])
	VALUES (@Status,
		@DueOn,
		@RaisedBy,
		@RequestID,
		GETUTCDATE(),
		GETUTCDATE())

	SET @ID = SCOPE_IDENTITY()

	/* Insert Request Log */
	IF @Status <> 1
	BEGIN
		DECLARE @LogText NVARCHAR(2000);
		DECLARE @LogID BIGINT;

		EXEC GetTranslation 0, 'RequestRaisedRFA', @Text = @LogText OUTPUT

		EXEC InsertLog 2, @ID, @LogText, NULL, NULL, @RaisedBy, @RaisedByName, @ID = @LogID OUTPUT

		INSERT INTO [RequestLog] VALUES (@RequestID, @LogID)
	END

	/* Insert RFA Log */

	EXEC GetTranslation 0, 'RFACreated', @Text = @LogText OUTPUT

	EXEC InsertLog 0, @ID, @LogText, NULL, NULL, @RaisedBy, @RaisedByName, @ID = @LogID OUTPUT

	INSERT INTO [RFALog] VALUES (@ID, @LogID)

	RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[InsertRFALineItem]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to Insert RFA line item
-- =============================================
CREATE PROCEDURE [dbo].[InsertRFALineItem]
	@Scheme SMALLINT,
	@Index SMALLINT,
	@ChecklistCategoryID BIGINT,
	@ChecklistCategoryText NVARCHAR(80),
	@ChecklistID BIGINT,
	@ChecklistText NVARCHAR(4000),
	@Remarks NVARCHAR(2000),
	@RFAID BIGINT,
	@ID BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [RFALineItem] ([Scheme],
		[Index],
		[Remarks],
		[ChecklistCategoryID],
		[ChecklistCategoryText],
		[ChecklistID],
		[ChecklistText],
		[RFAID],
		[CreatedOn],
		[ModifiedOn])
	VALUES (@Scheme,
		@Index,
		@Remarks,
		@ChecklistCategoryID,
		@ChecklistCategoryText,
		@ChecklistID,
		@ChecklistText,
		@RFAID,
		GETUTCDATE(),
		GETUTCDATE())

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertRFALineItemAttachments]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to Insert RFA lineitem attachments
-- =============================================
CREATE PROCEDURE [dbo].[InsertRFALineItemAttachments]
	@IDMappingType IDMappingType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [RFALineItemAttachments] ([LineItemID], [AttachmentID])
	SELECT [A], [B] FROM @IDMappingType


	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertRFAResponse]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		TEBS
-- Create date: 21-8-2020
-- Description:	Insert RFA Response
-- =============================================
CREATE PROCEDURE [dbo].[InsertRFAResponse]
	@RFAID BIGINT ,
	@RFA RFAType READONLY,
	@RFALineItemReply RFALineItemReplyType READONLY,
	@LineItemReplyAttachments LineItemReplyAttachmentsType READONLY	

AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @attachmentID bigint;
	DECLARE @templineitem bigint;
	DECLARE @tempindex int;
	DECLARE @tempattachmentID bigint;
	DECLARE @logID bigint;
	DECLARE @actiontext nvarchar(2000);

	UPDATE RFA SET [ModifiedOn] = GETUTCDATE(), [Status] = (SELECT [Status] FROM @RFA)	

	IF EXISTS (SELECT 1 FROM @RFALineItemReply)
	BEGIN
		MERGE [RFAReply] AS TARGET
		USING @RFALineItemReply AS SOURCE 
		ON (TARGET.[Text] = SOURCE.[Text] AND TARGET.[LineItemID] = SOURCE.[LineItemID])

		WHEN NOT MATCHED BY TARGET 
		THEN INSERT ([Text],[LineItemID],[CreatedOn],[ModifiedOn],[IsDeleted])
			VALUES (
			SOURCE.[Text], SOURCE.[LineItemID], (SELECT GETUTCDATE()),(SELECT GETUTCDATE()),0);

		--INSERT INTO [dbo].[RFAReply]
		--([Text],[LineItemID],[CreatedOn],[ModifiedOn],[IsDeleted])

		--SELECT [Text],[LineItemID], (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),0		
		--FROM @RFALineItemReply

		IF EXISTS (SELECT 1 FROM @LineItemReplyAttachments)
		BEGIN
			SET rowcount 0
			SELECT NULL mykey, * into #mytemp from @LineItemReplyAttachments

			SET rowcount 1
			UPDATE #mytemp set mykey = 1

			WHILE @@rowcount > 0
				BEGIN
					SET rowcount 0
					INSERT INTO Attachment([FileID], [FileName], [Extension],
					[Size], [CreatedOn], [ModifiedOn])

					SELECT [FileID], [FileName], [Extension], [Size],
					(SELECT GETUTCDATE()), (SELECT GETUTCDATE())
					FROM #mytemp where mykey = 1

					SET @tempattachmentID = SCOPE_IDENTITY();

					Set @tempindex = (Select [LineItemID] from #mytemp where mykey = 1);
					Set @templineitem = (Select ID from RFAReply where [LineItemID] = @tempindex)
					
					INSERT INTO RFAReplyAttachments values(@templineitem, @tempattachmentID) ;

					DELETE #mytemp where mykey = 1
					SET rowcount 1
					UPDATE #mytemp set mykey = 1
				END
		END
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[SelectRFA]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 01-Sept-2020
-- Description:	Stored procedure to select RFA base on specified options
-- =============================================
CREATE PROCEDURE [dbo].[SelectRFA]
	@ID BIGINT NULL,
	@RequestID BIGINT NULL,
	@Customer NVARCHAR(150) NULL,
	@RaisedBy UNIQUEIDENTIFIER NULL,
	@CreatedOn DATETIME2(0) NULL,
	@DueOn DATETIME2(0) NULL,
	@Status SMALLINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.*, rsl.[Text] as [StatusText], o.[Name] as [RaisedByName],
		req.ID as [Request], req.*
	FROM [RFA] r
	INNER JOIN [RFAStatusLookup] rsl ON rsl.[ID] = r.[Status]
	INNER JOIN [Officer] o ON r.[RaisedBy] = o.[ID]
	INNER JOIN [Request] req ON req.[ID] = r.[RequestID]
	WHERE (@RequestID IS NULL OR r.[RequestID] = @RequestID)
	AND (@ID IS NULL OR r.[ID] = @ID)
	AND (@Customer IS NULL OR req.[CustomerName] LIKE CONCAT('%', @Customer, '%'))
	AND (@RaisedBy IS NULL OR r.[RaisedBy] = @RaisedBy)
	AND (@CreatedOn IS NULL OR (r.[CreatedOn] >= @CreatedOn AND r.[CreatedOn] < DATEADD(DD, 1, @CreatedOn)))
	AND (@DueOn IS NULL OR (r.[DueOn] >= @DueOn AND r.[DueOn] < DATEADD(DD, 1, @DueOn)))
	AND (NOT EXISTS (SELECT 1 FROM @Status) OR r.[Status] IN (SELECT [Val] FROM @Status))
	AND r.[IsDeleted] = 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateRFA]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 11-June-2020
-- Description:	Stored procedure to update RFA
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRFA]
	@ID BIGINT,
	@Status SMALLINT,
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150),
	@DueOn DATETIME2(0)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC InsertOrReplaceOfficer @userID, @UserName
	
	UPDATE [RFA]
	SET [Status] = @Status,
		[DueOn] = @DueOn,
		[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

	/* Insert Request Log */
	IF @Status <> 1
	BEGIN
		DECLARE @LogText NVARCHAR(2000)
		DECLARE @LogID BIGINT
		DECLARE @RequestID BIGINT

		SELECT @RequestID = [RequestID]
		FROM [RFA]
		WHERE [ID] = @ID

		EXEC GetTranslation 0, 'RequestRaisedRFA', @Text = @LogText OUTPUT

		EXEC InsertLog 2, @ID, @LogText, NULL, NULL, @UserID, @UserName, @ID = @LogID OUTPUT

		INSERT INTO [RequestLog] VALUES (@RequestID, @LogID)
	END

	/* Insert RFA Log */

	
	EXEC GetTranslation 0, 'RFAUpdated', @Text = @LogText OUTPUT

	EXEC InsertLog 0, @ID, @LogText, NULL, NULL, @UserID, @UserName, @ID = @LogID OUTPUT

	INSERT INTO [RFALog] VALUES (@ID, @LogID)

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateRFAStatus]    Script Date: 2/12/2020 6:26:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- 2/12/2020 Sathiya Priya
-- included log enrty for RFA expired status
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRFAStatus]
	@ID BIGINT,
	@Status SMALLINT,
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
		
	Update RFA
	SET [Status] = @Status,
		ModifiedOn = GETUTCDATE()
	WHERE ID = @ID

	/* Insert logs */
	DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogID BIGINT

	IF @Status = 300
	BEGIN
		EXEC GetTranslation 0, 'RFAClosed', @Text = @LogText OUTPUT

		EXEC InsertLog 0, NULL, @LogText, NULL, NULL, @UserID, @UserName, @ID = @LogID OUTPUT

		INSERT INTO [RFALog]
		VALUES (@ID, @LogID)
	END
	IF @Status = 400
	BEGIN
		EXEC GetTranslation 0, 'RFALapsedBody', @Text = @LogText OUTPUT

		EXEC InsertLog 0, NULL, @LogText, NULL, NULL, @UserID, @UserName, @ID = @LogID OUTPUT

		INSERT INTO [RFALog]
		VALUES (@ID, @LogID)
	END

	RETURN 0
END



GO