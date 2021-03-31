/****** Object:  StoredProcedure [dbo].[GetRequestActionHistories]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve request action histories.
-- =============================================
CREATE PROCEDURE [dbo].[GetRequestActionHistories]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT h.*,
        o.[ID] as [OfficerID], o.*
    FROM [RequestActionHistory] h
    INNER JOIN [Officer] o ON o.[ID] = h.[OfficerID]
    WHERE h.[RequestID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[GetRequestByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020 
-- Description:	Stored procedure to get request by an ID
-- =============================================
CREATE PROCEDURE [dbo].[GetRequestByID]
	@ID bigint	
AS
BEGIN
	  SET NOCOUNT ON;

	  SELECT r.*, o.[Name] as [AssignedToName], rsl.[Text] as [StatusText], rtl.[Text] as [TypeText],
      cc.[ID] as [CCID], cc.*,
      gc.[ID] as [GCID], gc.*,
        ht.[ID] as [HalalTeamID], ht.*,
        p.[ID] as [PremiseID], p.*, rp.[IsPrimary], ptl.[Text] as [TypeText],
        c.[ID] as [CharID], c.*, cctl.Text as [TypeText],
        rfa.[ID] as [RFAID], rfa.*, rfasl.[Text] as [StatusText], orfa.[Name] as [RaisedByName],
        l.[ID] as [LogID], l.*, lo.[Name] as [UserName],
        ra.[ID] as [AttID], ra.*,
        rli.[ID] as [LineItemID], rli.*, rlisl.[Text] as [SchemeText], rlissl.[Text] as [SubSchemeText],
        rev.[ID] as [ReviewID], rev.*, revo.[Name] as [ReviewerName],
        revli.[ID] as [ReviewLineItemID], revli.*, revlisl.[Text] as [SchemeText], revlissl.[Text] as [SubSchemeText],
        rlicc.[ID] as [LiCharID], rlicc.*, rliccctl.[Text] as [TypeText]
    FROM [Request] r
    LEFT JOIN [Code] cc ON cc.[ID] = r.[CodeID]
    LEFT JOIN [Code] gc ON gc.[ID] = r.[GroupCodeID]
    INNER JOIN [RequestStatusLookup] rsl ON rsl.[ID] = r.[status]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = r.[Type]
    LEFT JOIN [Officer] o ON o.[ID] = r.[AssignedTo]
    LEFT JOIN [HalalTeam] ht ON ht.[RequestID] = r.[ID] AND ht.[IsDeleted] = 0
    LEFT JOIN [RequestPremise] rp ON rp.[RequestID] = r.[ID]
    LEFT JOIN [Premise] p ON p.[ID] = rp.[PremiseID]
    LEFT JOIN [PremiseTypeLookup] ptl ON ptl.[ID] = p.[Type]
    LEFT JOIN [RequestCharacteristics] rc ON rc.[RequestID] = r.[ID]
    LEFT JOIN [Characteristic] c ON  c.[ID] = rc.[CharID]
    LEFT JOIN [CharTypeLookup] cctl ON cctl.[ID] = c.[Type]
    LEFT JOIN [RFA] rfa ON rfa.[RequestID] = r.[ID] AND rfa.[IsDeleted] = 0
    LEFT JOIN [Officer] orfa ON orfa.[ID] = rfa.[RaisedBy]
    LEFT JOIN [RFAStatusLookup] rfasl ON rfasl.[ID] = rfa.[Status]
    LEFT JOIN [RequestLog] rl ON rl.[RequestID] = r.[ID]
    LEFT JOIN [Log] l ON l.[ID] = rl.[LogID]
    LEFT JOIN [Officer] lo on lo.[ID] = l.[UserID]
    LEFT JOIN [RequestAttachments] rra ON rra.[RequestID] = r.[ID]
    LEFT JOIN [Attachment] ra ON ra.[ID] = rra.[AttachmentID]
    LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
    LEFT JOIN [SchemeLookup] rlisl ON rlisl.[ID] = rli.[Scheme]
    LEFT JOIN [SubSchemeLookup] rlissl ON rlissl.[ID] = rli.[SubScheme]
    LEFT JOIN [Review] rev ON rev.[RequestID] = r.[ID]
    LEFT JOIN [Officer] revo ON revo.[ID] = rev.[ReviewerID]
    LEFT JOIN [ReviewLineItem] revli ON revli.[ReviewID] = rev.[ID]
    LEFT JOIN [SchemeLookup] revlisl ON revlisl.[ID] = revli.[Scheme]
    LEFT JOIN [SubSchemeLookup] revlissl ON revlissl.[ID]= revli.[SubScheme] 
    LEFT JOIN [RequestLineItemCharacteristics] rlic ON rlic.[LineItemID] = rli.[ID]
    LEFT JOIN [Characteristic] rlicc ON rlicc.[ID] = rlic.[CharID]
    LEFT JOIN [CharTypeLookup] rliccctl ON rliccctl.[ID] = rlicc.[Type]
    WHERE r.[ID] = @ID AND r.[IsDeleted] = 0;
END


GO
/****** Object:  StoredProcedure [dbo].[GetRequestByIDBasic]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to get request basic details by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetRequestByIDBasic]
	@ID bigint	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT r.*,
        rli.[ID] AS [LineItemID], rli.*,
        rlc.[ID] AS [LineItemCharID], rlc.*
    FROM [Request] r
    LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
    LEFT JOIN [RequestLineItemCharacteristics] rlic ON rlic.[LineItemID] = rli.[ID]
    LEFT JOIN [Characteristic] rlc ON rlc.[ID] = rlic.[CharID]
    WHERE r.[ID] = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[GetRequestByRefID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rameshkumar
-- Create date: 05-Oct-2020 
-- Description:	Stored procedure to get request by an Ref ID
-- =============================================
CREATE PROCEDURE [dbo].[GetRequestByRefID]
	@RefID VARCHAR(36)	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.*, cust.[Name] as [CustomerName], 
		o.[Name] as [AssignedToName], rsl.[Text] as [StatusText], rtl.[Text] as [TypeText],
        ht.[ID] as [HalalTeamID], ht.*,
        p.[ID] as [PremiseID], p.*, rp.[IsPrimary], ptl.[Text] as [TypeText],
        c.[ID] as [CharID], c.*, cctl.Text as [TypeText],
        rfa.[ID] as [RFAID], rfa.*, rfasl.[Text] as [StatusText],
        l.[ID] as [LogID], l.*,
        ra.[ID] as [AttID], ra.*,
        rli.[ID] as [LineItemID], rli.*, rlisl.[Text] as [SchemeText], rlissl.[Text] as [SubSchemeText],
        rev.[ID] as [ReviewID], rev.*, revo.[Name] as [ReviewerName],
        revli.[ID] as [ReviewLineItemID], revli.*, revlisl.[Text] as [SchemeText], revlissl.[Text] as [SubSchemeText],
        rlicc.[ID] as [LiCharID], rlicc.*, rliccctl.[Text] as [TypeText]
    FROM [Request] r
    INNER JOIN [Customer] cust ON cust.[ID] = r.[CustomerID]
    INNER JOIN [RequestStatusLookup] rsl ON rsl.[ID] = r.[status]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = r.[Type]
    LEFT JOIN [Officer] o ON o.[ID] = r.[AssignedTo]
    LEFT JOIN [HalalTeam] ht ON ht.[RequestID] = r.[ID] AND ht.[IsDeleted] = 0
    LEFT JOIN [RequestPremise] rp ON rp.[RequestID] = r.[ID]
    LEFT JOIN [Premise] p ON p.[ID] = rp.[PremiseID]
    LEFT JOIN [PremiseTypeLookup] ptl ON ptl.[ID] = p.[Type]
    LEFT JOIN [RequestCharacteristics] rc ON rc.[RequestID] = r.[ID]
    LEFT JOIN [Characteristic] c ON  c.[ID] = rc.[CharID]
    LEFT JOIN [CharTypeLookup] cctl ON cctl.[ID] = c.[Type]
    LEFT JOIN [RFA] rfa ON rfa.[RequestID] = r.[ID] AND rfa.[IsDeleted] = 0
    LEFT JOIN [RFAStatusLookup] rfasl ON rfasl.[ID] = rfa.[Status]
    LEFT JOIN [RequestLog] rl ON rl.[RequestID] = r.[ID]
    LEFT JOIN [Log] l ON l.[ID] = rl.[LogID]
    LEFT JOIN [RequestAttachments] rra ON rra.[RequestID] = r.[ID]
    LEFT JOIN [Attachment] ra ON ra.[ID] = rra.[AttachmentID]
    LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
    LEFT JOIN [SchemeLookup] rlisl ON rlisl.[ID] = rli.[Scheme]
    LEFT JOIN [SubSchemeLookup] rlissl ON rlissl.[ID] = rli.[SubScheme]
    LEFT JOIN [Review] rev ON rev.[RequestID] = r.[ID]
    LEFT JOIN [Officer] revo ON revo.[ID] = rev.[ReviewerID]
    LEFT JOIN [ReviewLineItem] revli ON revli.[ReviewID] = rev.[ID]
    LEFT JOIN [SchemeLookup] revlisl ON revlisl.[ID] = revli.[Scheme]
    LEFT JOIN [SubSchemeLookup] revlissl ON revlissl.[ID]= revli.[SubScheme] 
    LEFT JOIN [RequestLineItemCharacteristics] rlic ON rlic.[LineItemID] = rli.[ID]
    LEFT JOIN [Characteristic] rlicc ON rlicc.[ID] = rlic.[CharID]
    LEFT JOIN [CharTypeLookup] rliccctl ON rliccctl.[ID] = rlicc.[Type]
    WHERE r.[RefID] = @RefID AND r.[IsDeleted] = 0;
END

GO
/****** Object:  StoredProcedure [dbo].[GetRequestIDFromCharacteristic]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 12-June-2020
-- Description:	Stored procedure to get RequestID From Characteristic
-- =============================================
CREATE PROCEDURE [dbo].[GetRequestIDFromCharacteristic]
	@Characteristics CharacteristicsType READONLY,
	@ID bigint OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;
	

	SET @ID = (select rc.RequestID from RequestCharacteristics rc
	INNER JOIN Characteristic c on c.ID = rc.CharID
	INNER JOIN Request r on r.ID = rc.RequestID AND r.Status = 900
	WHERE c.Value = (SELECT Value FROM @Characteristics) AND c.Type = 2 AND c.IsDeleted = 0)
	
END

GO
/****** Object:  StoredProcedure [dbo].[InsertReqAttachments]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 15/11/2020
-- Description:	Inset request attachments
-- =============================================
CREATE PROCEDURE [dbo].[InsertReqAttachments]
	-- Add the parameters for the stored procedure here
	@ReqID bigint,
	@FileID UNIQUEIDENTIFIER,
	@FileName NVARCHAR(150),
	@Extension NVARCHAR(30),
	@Size BIGINT,
	@ID BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		
		INSERT INTO Attachment([FileID], [FileName], [Extension],
		[Size], [CreatedOn], [ModifiedOn])		
		VALUES (@FileID, @FileName, @Extension, @Size,
		(SELECT GETUTCDATE()), (SELECT GETUTCDATE()))
		
		set @ID = SCOPE_IDENTITY()
		INSERT iNTO RequestAttachments([RequestID], [AttachmentID])
		values(@ReqID, @ID )
END
GO
/****** Object:  StoredProcedure [dbo].[InsertRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Naqi, Imam
-- Create date: 11-June-2020
-- Description:	Stored procedure to Insert Request
-- Modified
-- Author:		Ramesh
-- Create date: 02-Oct-2020
-- Description:	Remove CustomerCode, CustomerName and AssignedTo in Request table
-- Author: Sathiya Priya
-- Create date: 01-NOV-2020
-- Description:	Added request Premise
-- =============================================
CREATE PROCEDURE [dbo].[InsertRequest]
	@Request ApplicationRequestType READONLY,
	@HalalTeam HalalTeamType READONLY,
	@Menu MenuType READONLY,
	@Ingredient IngredientType READONLY,
	@Premise PremiseType READONLY,
	@Characteristics CharacteristicsType READONLY,
	@Attachments AttachmentsType READONLY,
	@RequestLineItems RequestLineItemType READONLY,
	@RequestLineItemCharacteristics RequestLineItemCharacteristicsType READONLY,
	@ID bigint OUT

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @attachmentID bigint;

	INSERT INTO Request([Step],
	[CustomerID],[CustomerName],[RequestorID],[RequestorName],
	[AgentID],[AgentName], [Type],
	[Status], [StatusMinor], [Expedite], 
	[RefID], [AssignedTo],
	[CreatedOn], [ModifiedOn], [DueOn],[ParentID], [SubmittedOn])
	 
	SELECT [Step],  [CustomerID], 
	[CustomerName],[RequestorID],[RequestorName],
	[AgentID],[AgentName], 
	[Type], [Status], [StatusMinor],
	[Expedite], [RefID], [AssignedTo], 
	(SELECT GETUTCDATE()), (SELECT GETUTCDATE()),
	[DueOn],[ParentID], [SubmittedOn]
	FROM @Request;

	SET @ID = SCOPE_IDENTITY();

	IF EXISTS (SELECT 1 FROM @HalalTeam)
	BEGIN
		INSERT INTO HalalTeam([AltID], [Name], [Designation],
		[Role], [IsCertified], [JoinedOn], [ChangeType], [RequestID],
		[CreatedOn], [ModifiedOn])

		SELECT [AltID], [Name], [Designation], [Role], [IsCertified],
		[JoinedOn],[ChangeType], @ID, (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
		FROM @HalalTeam
	END

	IF EXISTS (SELECT 1 FROM @Menu)
	BEGIN
		INSERT INTO Menu([Scheme], [Text], [SubText], [Approved], [Remarks],
		[ReviewedBy], [ChangeType],
		[RequestID], [CreatedOn], [ModifiedOn])

		SELECT [Scheme], [Text], [Subtext], [Approved], [Remarks], [ReviewedBy],
		[ChangeType], @ID, (SELECT GETUTCDATE()),
		(SELECT GETUTCDATE())
		FROM @Menu
	END
	
	IF EXISTS (SELECT 1 FROM @Ingredient)
	BEGIN
		INSERT INTO Ingredient([RiskCategory], [Text], [SubText], [Approved],
		[Remarks], [ReviewedBy], [ChangeType], [RequestID],
		[CreatedOn], [ModifiedOn])

		SELECT 999, [Text], [SubText], [Approved], [Remarks],
		[ReviewedBy], [ChangeType], @ID, (SELECT GETUTCDATE()),
		(SELECT GETUTCDATE())
		FROM @Ingredient
	END

	IF EXISTS (SELECT 1 FROM @Premise)
	BEGIN
		CREATE TABLE #REQPREMISE(ROWNUM INT,[IsLocal] [bit] ,[Name] [nvarchar](150) ,[Type] [smallint] ,
						[Area] [float] ,[Schedule] [varchar](50) ,[BlockNo] [varchar](15) ,[UnitNo] [varchar](5) ,[FloorNo] [varchar](5) ,
						[BuildingName] [nvarchar](100) ,[Address1] [nvarchar](150) ,[Address2] [nvarchar](150) ,[City] [nvarchar](100) ,[Province] [nvarchar](100) ,
						[Country] [nvarchar](100) ,[Postal] [varchar](20) ,[Longitude] [decimal](9, 6) ,[Latitude] [decimal](9, 6) ,
						[CreatedOn] [datetime2](0)  ,[ModifiedOn] [datetime2](0) ,[IsDeleted] [bit], [IsPrimary] bit  )
		INSERT INTO #REQPREMISE
				SELECT ROW_NUMBER() OVER (ORDER BY [Type]) AS ROWNUM,[IsLocal], [Name], [Type], [Area], [Schedule],
				[BlockNo], [UnitNo], [FloorNo],[BuildingName], [Address1], [Address2],
				[City], [Province], [Country], [Postal], [Longitude], [Latitude], (SELECT GETUTCDATE()), 
				(SELECT GETUTCDATE()), 0, IsPrimary FROM @Premise

		
		DECLARE @TOTREQPREM INT
		DECLARE @REQPREMID BIGINT
		DECLARE @REQPRMEINDEX INT
		SET @REQPRMEINDEX = 1;
		SET @TOTREQPREM = (SELECT COUNT(*) FROM #REQPREMISE)	
		WHILE(@REQPRMEINDEX <= @TOTREQPREM)
			BEGIN
				INSERT INTO Premise([IsLocal], [Name], [Type], [Area], [Schedule],
				[BlockNo], [UnitNo], [FloorNo],[BuildingName], [Address1], [Address2],
				[City], [Province], [Country], [Postal], [Longitude], [Latitude], [CreatedOn],	[ModifiedOn])

				SELECT [IsLocal], [Name], [Type], [Area], [Schedule], [BlockNo], [UnitNo],
				[FloorNo], [BuildingName],  [Address1],	[Address2], [City], [Province],
				[Country], [Postal], [Longitude], [Latitude], (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
				FROM #REQPREMISE
				WHERE ROWNUM = @REQPRMEINDEX 

				SET @REQPREMID = SCOPE_IDENTITY();

				INSERT INTO [dbo].RequestPremise
				(RequestID  ,PremiseID,IsPrimary,ChangeType)
				VALUES(@ID,@REQPREMID,(SELECT IsPrimary FROM #REQPREMISE
				WHERE ROWNUM = @REQPRMEINDEX   ),0)
				SET @REQPRMEINDEX = @REQPRMEINDEX + 1
			END

	END

	IF EXISTS (SELECT 1 FROM @Characteristics)
	BEGIN
		CREATE TABLE #REQCHAR(ROWNUM INT, [Type] SMALLINT, [Value] NVARCHAR(2000), [CreatedOn] DATETIME2(0),
		[ModifiedOn] DATETIME2(0))
		INSERT INTO #REQCHAR
		SELECT ROW_NUMBER() OVER (ORDER BY [Type]) AS ROWNUM,[Type], [Value], [CreatedOn],
		[ModifiedOn] FROM @Characteristics

		
		DECLARE @TOTREQCHARS INT
		DECLARE @REQCHARID BIGINT
		DECLARE @REQCHARINDEX INT
		SET @REQCHARINDEX = 1;
		SET @TOTREQCHARS = (SELECT COUNT(*) FROM #REQCHAR)	
		WHILE(@REQCHARINDEX <= @TOTREQCHARS)
			BEGIN
				INSERT INTO Characteristic([Type], [Value],  [CreatedOn],
				[ModifiedOn])

				SELECT [Type], [Value],  (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
				FROM #REQCHAR
				WHERE ROWNUM = @REQCHARINDEX 

				SET @REQCHARID = SCOPE_IDENTITY();

				INSERT INTO [dbo].RequestCharacteristics
				(RequestID  ,[CharID])
				VALUES(@ID,@REQCHARID)
				SET @REQCHARINDEX = @REQCHARINDEX + 1
			END

	END

	IF EXISTS (SELECT 1 FROM @Attachments)
	BEGIN
		declare @inserted table (attachmentID bigint)
		
		INSERT INTO Attachment([FileID], [FileName], [Extension],
		[Size], [CreatedOn], [ModifiedOn])
		output inserted.Id into @inserted (attachmentID)
		SELECT [FileID], [FileName], [Extension], [Size],
		(SELECT GETUTCDATE()), (SELECT GETUTCDATE())
		FROM @Attachments
				
		INSERT iNTO RequestAttachments([RequestID], [AttachmentID])
		SELECT @ID, attachmentID FROM @inserted 
	END
	

	IF EXISTS (SELECT 1 FROM @RequestLineItems)
		BEGIN
			DECLARE @TOTREQLINEITEMS INT
			DECLARE @REQLINEITEMID BIGINT
			DECLARE @REQLINEITEMINDEX INT

			DECLARE @TOTREQLINEITEMCHARS INT
			DECLARE @REQLINEITEMCHARID BIGINT
			DECLARE @REQLINEITEMCHARINDEX INT

			SET @REQLINEITEMINDEX = 1;
			SET @TOTREQLINEITEMS =(SELECT COUNT(*) FROM @RequestLineItems)
			WHILE(@REQLINEITEMINDEX <= @TOTREQLINEITEMS)
				BEGIN
						INSERT INTO RequestLineItem([Scheme], [SubScheme], [ChecklistHistoryID],
						[RequestID], [CreatedOn], [ModifiedOn], [IsDeleted])

						SELECT [Scheme], [SubScheme], [ComplianceHistoryID], @ID,
						(SELECT GETUTCDATE()), (SELECT GETUTCDATE()), 0
						FROM @RequestLineItems WHERE [INDEX] = @REQLINEITEMINDEX

						SET @REQLINEITEMID = SCOPE_IDENTITY();

						IF EXISTS (SELECT 1 FROM @RequestLineItemCharacteristics)
							BEGIN
								SET @REQLINEITEMCHARINDEX = 1;
								SET @TOTREQLINEITEMCHARS = (SELECT COUNT(*) FROM @RequestLineItemCharacteristics 
															WHERE REFINDEX = @REQLINEITEMINDEX)	
								WHILE(@REQLINEITEMCHARINDEX <= @TOTREQLINEITEMCHARS)
									BEGIN
										INSERT INTO Characteristic([Type], [Value],  [CreatedOn],
										[ModifiedOn])

										SELECT [Type], [Value],  (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
										FROM @RequestLineItemCharacteristics
										WHERE RefIndex = @REQLINEITEMINDEX AND [Index] = @REQLINEITEMCHARINDEX

										SET @REQLINEITEMCHARID = SCOPE_IDENTITY();

										INSERT INTO [dbo].[RequestLineItemCharacteristics]
										([LineItemID]  ,[CharID])
										VALUES(@REQLINEITEMID,@REQLINEITEMCHARID)
										SET @REQLINEITEMCHARINDEX = @REQLINEITEMCHARINDEX + 1
									END 
							END
						
						SET @REQLINEITEMINDEX = @REQLINEITEMINDEX + 1
				END
		END

END

GO
/****** Object:  StoredProcedure [dbo].[InsertRequestActionHistory]    Script Date: 28/1/2021 5:25:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert request action history
-- =============================================
-- Modification History
-- 27/01/2021 SathiyaPriya
-- Included RefID and Remarks new columns
-- =============================================
CREATE PROCEDURE [dbo].[InsertRequestActionHistory]
	@Action SMALLINT,
    @OfficerID UNIQUEIDENTIFIER,
    @OfficerName NVARCHAR(150),
    @RequestID BIGINT,
	@RefID VARCHAR(36),
	@Remarks NVARCHAR(500),
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName

    INSERT INTO [RequestActionHistory]([Action],
        [OfficerID],
        [RequestID],
		[RefID],
		[Remarks],
        [CreatedOn])
    VALUES (@Action,
        @OfficerID,
        @RequestID,
		@RefID,
		@Remarks,
        GETUTCDATE())

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[KIVRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KIVRequest]
	@ID BIGINT,
    @RemindOn DATETIME2(0),
    @Notes NVARCHAR(2000) NULL,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @LogText NVARCHAR(2000)
    DECLARE @LogID BIGINT

    UPDATE [Request]
    SET [OldStatus] = [Status],
        [Status] = 1300,
        [RemindOn] = @RemindOn,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    /* Insert Log */

    EXEC GetTranslation 0, 'RequestKIV', @Text = @LogText OUTPUT

    EXEC InsertLog 1, NULL, @LogText, NULL, @Notes, @UserID, @UserName, @ID = @LogID OUTPUT

    INSERT INTO [RequestLog] VALUES (@ID, @LogID)
END

GO
/****** Object:  StoredProcedure [dbo].[MapJobOrderToRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to map an job order to a request
-- =============================================
CREATE PROCEDURE [dbo].[MapJobOrderToRequest]
	@RequestID BIGINT,
    @JobID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Request]
    SET [JobID] = @JobID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @RequestID
END


GO
/****** Object:  StoredProcedure [dbo].[MapLogToRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to map log to request
-- =============================================
CREATE PROCEDURE [dbo].[MapLogToRequest]
	@RequestID BIGINT,
    @LogID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [RequestLog]
    VALUES (@RequestID, @LogID)

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[RequestEscalateAction]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to execute escalation action
-- =============================================
CREATE PROCEDURE [dbo].[RequestEscalateAction]
	@RequestID BIGINT,
    @Status SMALLINT,
    @Remarks NVARCHAR(4000),
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)
    
    IF @Status = 200
    BEGIN
        EXEC GetTranslation 0, 'EscalateClosed', @Text = @LogText OUTPUT
    END
    ELSE
    BEGIN
        DECLARE @CurrStatus SMALLINT

        SELECT @CurrStatus = [EscalateStatus]
        FROM [Request]
        WHERE [ID] = @RequestID

        IF @CurrStatus = 200
            EXEC GetTranslation 0, 'ReEscalateRequest', @Text = @LogText OUTPUT
        ELSE IF @CurrStatus = 100
            EXEC GetTranslation 0, 'EscalateNotes', @Text = @LogText OUTPUT
        ELSE
            EXEC GetTranslation 0, 'EscalateRequest', @Text = @LogText OUTPUT
    END

    UPDATE [Request]
    SET [EscalateStatus] = @Status,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @RequestID

    EXEC InsertLog 0, NULL, @LogText, NULL, @Remarks, @UserID, @UserName, @ID = @LogID OUTPUT

    INSERT INTO [RequestLog]
    VALUES (@RequestID, @LogID)

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[RevertKIV]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RevertKIV]
	@ID BIGINT,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @LogText NVARCHAR(2000)
    DECLARE @LogID BIGINT

    UPDATE [Request]
    SET [Status] = [OldStatus],
        [OldStatus] = NULL,
        [RemindOn] = NULL,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    /* Insert Log */

    EXEC GetTranslation 0, 'RevertKIV', @Text = @LogText OUTPUT

    EXEC InsertLog 1, NULL, @LogText, NULL, NULL, @UserID, @UserName, @ID = @LogID OUTPUT

    INSERT INTO [RequestLog] VALUES (@ID, @LogID)
END
GO

/****** Object:  StoredProcedure [dbo].[SelectRequest]    Script Date: 13/3/2021 1:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 08-Sep-2020
-- Description:	Stored procedure to filter Request based on specified parameters
-- =============================================
-- Modification History
-- 26/01/2021 SathiyaPriya
-- Included status Minor in filter options
-- =============================================
-- Modification History
-- 13/03/2021 Ramesh
-- Included Premise ID, Customer ID in filter options
-- =============================================
CREATE PROCEDURE [dbo].[SelectRequest]
	@ID BIGINT NULL,
	@CustomerCode VARCHAR(30) NULL,
	@CustomerName NVARCHAR(150) NULL,
	@CustomerID UNIQUEIDENTIFIER NULL,
  @RFAStatus SMALLINT NULL,
  @EscalateStatus SMALLINT NULL,
  @From DATETIME2(0) NULL,
  @To DATETIME2(0) NULL,
  @Premise NVARCHAR(150) NULL,
	@PremiseID BIGINT NULL,
  @Type [SmallIntType] READONLY,
  @Status [SmallIntType] READONLY,
	@AssignedTo [UniqueIdentifierType] READONLY,
	@StatusMinor [SmallIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT r.*, rsl.[Text] as [StatusText], rtl.[Text] as [TypeText], o.[Name] as [AssignedToName],
        cc.[ID] as [CCID], cc.*,
        rli.[ID] as [LineItemID], rli.*, sl.[Text] as [SchemeText], [ssl].[Text] as [SubSchemeText],
        p.[ID] as [PremiseID], p.*, ptl.[Text] as [TypeText],
        rfa.[ID] as [RFAID], rfa.*, rfasl.[Text] as [StatusText]
    FROM [Request] r
    LEFT JOIN [Code] cc ON cc.[ID] = r.[CodeID]
    INNER JOIN [RequestStatusLookup] rsl ON rsl.[ID] = r.[Status]
		LEFT JOIN [RequestStatusMinorLookup] rsml ON rsml.[ID] = r.[StatusMinor]
    INNER JOIN [RequestTypeLookup] rtl ON rtl.[ID] = r.[Type]
    LEFT JOIN [Officer] o ON o.[ID] = r.[AssignedTo]
    LEFT JOIN (SELECT ip.*,
                irp.[RequestID],
                irp.[ChangeType],
                irp.[IsPrimary],
                dbo.FormatPremise([Name], [BlockNo], [Address1], [Address2], [FloorNo], [UnitNo], [BuildingName], [Province], [City], [Country], [Postal]) AS [Text]
                FROM [Premise] ip,
                    [RequestPremise] irp
                WHERE irp.[IsPrimary] = 1
                AND [IsDeleted] = 0
                AND ip.[ID] = irp.[PremiseID]) p ON p.[RequestID] = r.[ID]
    LEFT JOIN [PremiseTypeLookup] ptl ON ptl.[ID] = p.[Type]
    LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
    LEFT JOIN [SchemeLookup] sl ON sl.[ID] = rli.[Scheme]
    LEFT JOIN [SubSchemeLookup] [ssl] ON [ssl].[ID] = rli.[SubScheme]
    LEFT JOIN [RFA] rfa ON rfa.[RequestID] = r.[ID] AND rfa.[ID] = (SELECT MAX([ID]) FROM [RFA] irfa WHERE irfa.[RequestID] = r.[ID] AND [IsDeleted] = 0)
    LEFT JOIN [RFAStatusLookup] rfasl ON rfasl.[ID] = rfa.[Status]
    WHERE (NOT EXISTS (SELECT 1 FROM @AssignedTo) OR r.[AssignedTo] IN (SELECT [Val] FROM @AssignedTo))
    AND (NOT EXISTS (SELECT 1 FROM @Type) OR r.[Type] IN (SELECT [Val] FROM @Type))
    AND (NOT EXISTS (SELECT 1 FROM @Status) OR r.[Status] IN (SELECT [Val] FROM @Status))
		AND (NOT EXISTS (SELECT 1 FROM @StatusMinor) OR r.[StatusMinor] IN (SELECT [Val] FROM @StatusMinor))
    AND (@CustomerCode IS NULL OR cc.[Value] = @CustomerCode)
    AND (@CustomerName IS NULL OR r.[CustomerName] LIKE '%' + @CustomerName + '%')
    AND (@RFAStatus IS NULL OR rfa.[Status] = @RFAStatus)
    AND (@Premise IS NULL OR p.[Text] LIKE '%' + @Premise + '%')
		AND (@PremiseID IS NULL OR p.[ID] = @PremiseID)
    AND (@ID IS NULL OR r.[ID] = @ID)
		AND (@CustomerID IS NULL OR r.[CustomerID] = @CustomerID)
    AND (@EscalateStatus IS NULL OR r.[EscalateStatus] = @EscalateStatus)
    AND (@From IS NULL OR r.[SubmittedOn] >= CAST(@From AS DATE))
    AND (@To IS NULL OR r.[SubmittedOn] < DATEADD(DD, 1, CAST(@To AS DATE)))
	
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateReqChars]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 16/11/2020
-- Description:	Update request characteristics
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqChars] 
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@Type smallint,
	@Value nvarchar(2000),
	@IsDeleted bit,
	@RequestID bigint
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			IF EXISTS (SELECT ID FROM [dbo].Characteristic WHERE [ID]= @ID	AND	[Type] = @Type )
				BEGIN
					UPDATE Characteristic
					SET [Value] = @Value, [IsDeleted] = @IsDeleted, [ModifiedOn] = (SELECT GETUTCDATE())
					WHERE [ID]= @ID	AND	[Type] = @Type 
				END
		END
	ELSE
		BEGIN
			DECLARE @PID BIGINT
			INSERT INTO Characteristic([Type], [Value],  [CreatedOn],[ModifiedOn], [IsDeleted])
			VALUES(@Type, @Value, (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),0)
			
			SET @PID = SCOPE_IDENTITY();

			INSERT INTO [dbo].[RequestCharacteristics]
			(RequestID  ,[CharID])
			VALUES(@RequestID,@PID)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqHalalTeam]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 16/11/2020
-- Description:	Update request halal team
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqHalalTeam]
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@AltID   nvarchar(36),
	@Name   nvarchar(150),
	@Designation   nvarchar(100),
	@Role   nvarchar(50),
	@IsCertified   bit,
	@JoinedOn   datetime2(0),
	@ChangeType   smallint,
	@RequestID   bigint,
	@CreatedOn   datetime2(0),
	@ModifiedOn   datetime2(0),
	@IsDeleted   bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			UPDATE [dbo].[HalalTeam]
		   SET [AltID] = @AltID,
			  [Name] = @Name, 
			  [Designation] = @Designation, 
			  [Role] = @Role, 
			  [IsCertified] = @IsCertified,
			  [JoinedOn] = @JoinedOn, 
			  [ChangeType] = @ChangeType,
			  [RequestID] = @RequestID, 			  
			  [ModifiedOn] = (SELECT GETUTCDATE()), 
			  [IsDeleted] = @IsDeleted		 
			WHERE ID = @ID

		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[HalalTeam]
           ([AltID]
           ,[Name]
           ,[Designation]
           ,[Role]
           ,[IsCertified]
           ,[JoinedOn]
           ,[ChangeType]
           ,[RequestID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[IsDeleted])
			VALUES(@AltID ,
					@Name  ,
					@Designation   ,
					@Role  ,
					@IsCertified  ,
					@JoinedOn  ,
					@ChangeType   ,
					@RequestID   ,
					(SELECT GETUTCDATE()), (SELECT GETUTCDATE()),
					@IsDeleted   )
		END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqIngredient]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 16/11/2020
-- Description:	Update request ingredient
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqIngredient]
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@RiskCategory smallint,
	@Text nvarchar(500),
	@SubText nvarchar(2000),
	@Approved bit,
	@Remarks nvarchar(2000),
	@ReviewedOn datetime2(0),
	@ChangeType smallint,
	@RequestID bigint,
	@CreatedOn datetime2(0),
	@ModifiedOn datetime2(0),
	@IsDeleted bit,
	@ReviewedBy uniqueidentifier,
	@Undeclared bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			UPDATE [dbo].[Ingredient]
			   SET [RiskCategory] = @RiskCategory
				  ,[Text] = @Text
				  ,[SubText] = @SubText
				  ,[Approved] = @Approved
				  ,[Remarks] = @Remarks
				  ,[ReviewedOn] = @ReviewedOn
				  ,[ChangeType] = @ChangeType
				  ,[RequestID] = @RequestID     
				  ,[ModifiedOn] = (SELECT GETUTCDATE())
				  ,[IsDeleted] = @IsDeleted
				  ,[ReviewedBy] = @ReviewedBy
				  ,[Undeclared] = @Undeclared
			WHERE ID = @ID

		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Ingredient]
           ([RiskCategory]
           ,[Text]
           ,[SubText]
           ,[Approved]
           ,[Remarks]
           ,[ReviewedOn]
           ,[ChangeType]
           ,[RequestID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[IsDeleted]
           ,[ReviewedBy]
           ,[Undeclared])
     VALUES
           (@RiskCategory, 
           @Text, 
           @SubText, 
           @Approved, 
           @Remarks,
           @ReviewedOn, 
           @ChangeType, 
           @RequestID,
          (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),
           @IsDeleted, 
           @ReviewedBy, 
           @Undeclared)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqLineItem]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 15/11/2020
-- Description:	Update request line item and its characteristics
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqLineItem]
	-- Add the parameters for the stored procedure here
	@Scheme SMALLINT,
	@SubScheme SMALLINT,
	@RequestID BIGINT,
	@IsDeleted BIT,
	@ID INT OUTPUT
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF NOT EXISTS (SELECT ID FROM [dbo].[RequestLineItem] WHERE 
	[Scheme] = @Scheme	AND	[SubScheme] = @SubScheme AND [RequestID] = @RequestID)

		BEGIN
			INSERT INTO RequestLineItem([Scheme], [SubScheme],		
			[RequestID], [CreatedOn], [ModifiedOn], [IsDeleted])

			VALUES( @Scheme, @SubScheme, @RequestID,		
			(SELECT GETUTCDATE()), (SELECT GETUTCDATE()), 0)

			SET @ID = SCOPE_IDENTITY();			
		END
	ELSE
		BEGIN
			UPDATE [RequestLineItem] SET IsDeleted = @IsDeleted
			WHERE [Scheme] = @Scheme	AND	[SubScheme] = @SubScheme AND [RequestID] = @RequestID

			SET @ID = (SELECT ID FROM [RequestLineItem] 
			WHERE [Scheme] = @Scheme	AND	[SubScheme] = @SubScheme AND [RequestID] = @RequestID)
		END

						
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqLineItemChars]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 15/11/2020
-- Description:	Update request lineitem characteristics
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqLineItemChars]
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@Type smallint,
	@Value nvarchar(2000),
	@IsDeleted bit,
	@LineItemID bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			IF EXISTS (SELECT ID FROM [dbo].Characteristic WHERE [ID]= @ID	AND	[Type] = @Type )
				BEGIN
					UPDATE Characteristic
					SET [Value] = @Value, [IsDeleted] = @IsDeleted, [ModifiedOn] = (SELECT GETUTCDATE())
					WHERE [ID]= @ID	AND	[Type] = @Type 
				END
		END
	ELSE
		BEGIN
			DECLARE @PID BIGINT
			INSERT INTO Characteristic([Type], [Value],  [CreatedOn],[ModifiedOn], [IsDeleted])
			VALUES(@Type, @Value, (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),0)
			
			SET @PID = SCOPE_IDENTITY();

			INSERT INTO [dbo].[RequestLineItemCharacteristics]
			([LineItemID]  ,[CharID])
			VALUES(@LineItemID,@PID)
		END

		
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqMenu]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 16/11/2020
-- Description:	Update Request Menu
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqMenu] 
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@Scheme smallint,
	@Text nvarchar(500),
	@SubText nvarchar(2000),
	@Approved bit,
	@Remarks nvarchar(2000),
	@ReviewedOn datetime2(0),
	@ChangeType smallint,
	@RequestID bigint,
	@CreatedOn datetime2(0),
	@ModifiedOn datetime2(0),
	@IsDeleted bit,
	@ReviewedBy uniqueidentifier,
	@Group smallint,
	@Index smallint,
	@Undeclared bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			UPDATE [dbo].[Menu] 
			SET [Scheme] = @Scheme,
			[Text] = @Text,
			[SubText] = @SubText,
			[Approved] = @Approved,
			[Remarks] = @Remarks,
			[ReviewedOn] = @ReviewedOn,
			[ChangeType] = @ChangeType,
			[RequestID] = @RequestID	,		
			[ModifiedOn] = (SELECT GETUTCDATE()),
			[IsDeleted] = @IsDeleted,
			[ReviewedBy] = @ReviewedBy,
			[Group] = @Group,
			[Index] = @Index,
			[Undeclared] = @Undeclared
			WHERE ID = @ID

		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Menu]
           ([Scheme]
           ,[Text]
           ,[SubText]
           ,[Approved]
           ,[Remarks]
           ,[ReviewedOn]
           ,[ChangeType]
           ,[RequestID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[IsDeleted]
           ,[ReviewedBy]
           ,[Group]
           ,[Index]
           ,[Undeclared])
		   VALUES(@Scheme,@Text,@SubText,@Approved,@Remarks,@ReviewedOn,@ChangeType,
		   @RequestID, (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),@IsDeleted,
		   @ReviewedBy,@Group,@Index,@Undeclared)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateReqPremise]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 15/11/2020
-- Description:	Update request premises
-- =============================================
CREATE PROCEDURE [dbo].[UpdateReqPremise]
	-- Add the parameters for the stored procedure here
	@ID bigint,
	@IsLocal bit,
	@Name nvarchar(150),
	@Type smallint,
	@Area float,
	@Schedule varchar(50),
	@BlockNo varchar(15),
	@UnitNo varchar(5),
	@FloorNo varchar(5),
	@BuildingName nvarchar(100),
	@Address1 nvarchar(150),
	@Address2 nvarchar(150),
	@City nvarchar(100),
	@Province nvarchar(100),
	@Country nvarchar(100),
	@Postal varchar(20),
	@Longitude decimal(9,6),
	@Latitude decimal(9,6),
	@CreatedOn datetime2(0),
	@ModifiedOn datetime2(0),
	@IsDeleted bit,
	@ReqID bigint,
	@IsPrimary bit,
	@ChangeType smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@ID > 0)
		BEGIN
			UPDATE [dbo].[Premise] 
			SET [IsLocal] = @IsLocal,
			[Name] = @Name,
			[Type] = @Type,
			[Area] = @Area,
			[Schedule] = @Schedule,
			[BlockNo] = @BlockNo,
			[UnitNo] = @UnitNo,
			[FloorNo] = @FloorNo,
			[BuildingName] = @BuildingName,
			[Address1] = @Address1,
			[Address2] = @Address2,
			[City] = @City,
			[Province] = @Province,
			[Country] = @Country,
			[Postal] = @Postal,
			[Longitude] = @Longitude,
			[Latitude] = @Latitude,
			[ModifiedOn] = (SELECT GETUTCDATE()),
			[IsDeleted] = @IsDeleted
			WHERE ID = @ID

			UPDATE [dbo].[RequestPremise]
			SET [IsPrimary]  = @IsPrimary, [ChangeType] = @ChangeType
			WHERE [RequestID] = @ReqID AND [PremiseID] = @ID
		END
	ELSE
		BEGIN
			declare @premiseID bigint

			INSERT INTO Premise([IsLocal], [Name], [Type], [Area], [Schedule],
				[BlockNo], [UnitNo], [FloorNo],[BuildingName], [Address1], [Address2],
				[City], [Province], [Country], [Postal], [Longitude], [Latitude], 				
				[CreatedOn],	[ModifiedOn], [IsDeleted])
			VALUES(@IsLocal,@Name,@Type,@Area,@Schedule,@BlockNo,@UnitNo,@FloorNo,@BuildingName,@Address1,
			@Address2,@City,@Province,@Country,@Postal,@Longitude,@Latitude,(SELECT GETUTCDATE()),
			(SELECT GETUTCDATE()),0)

			set @premiseID = SCOPE_IDENTITY();

			INSERT INTO [dbo].RequestPremise
				(RequestID  ,PremiseID,IsPrimary,ChangeType)
				VALUES(@ReqID, @premiseID,@IsPrimary, @ChangeType)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TEBS
-- Create date: 21-Aug-2020
-- Description:	Stored procedure to Update Request for HC02 with RFA response
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRequest]
	@ID bigint,
	--@Request ApplicationRequestType READONLY,	
	@Menu MenuType READONLY,
	@Ingredient IngredientType READONLY,
	@Premise PremiseType READONLY,
	@Characteristics CharacteristicsType READONLY,
	@Attachments AttachmentsType READONLY,
	@RequestLineItems RequestLineItemType READONLY,
	@RequestLineItemCharacteristics RequestLineItemCharacteristicsType READONLY	

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @attachmentID bigint;

	DECLARE @TEMPMENU TABLE
	(Scheme smallint not null,
	[Text] nvarchar(500) not null,
	[SubText] nvarchar(2000) not null,
	[ChangeType] smallint not null,
	[IsDeleted] bit not null)
	
	INSERT INTO @TEMPMENU(Scheme,[Text],[SubText],[ChangeType],[IsDeleted])
	SELECT Scheme,[Text],[SubText],[ChangeType],[IsDeleted] FROM Menu WHERE RequestID = @ID

	IF EXISTS (SELECT 1 FROM @Menu)
	BEGIN
		INSERT INTO Menu([Scheme], [Text], [SubText], [Approved], [Remarks],
		--[ReviewdBy], [ReviewdByName],
		[ChangeType],
		[RequestID], [CreatedOn], [ModifiedOn])

		SELECT [Scheme], [Text], [Subtext], [Approved], [Remarks], 
		--[ReviewedBy],
		--[ReviewedByName], 
		[ChangeType], @ID, (SELECT GETUTCDATE()),
		(SELECT GETUTCDATE())
		FROM @Menu
	END
	
	IF EXISTS (SELECT 1 FROM @Ingredient)
	BEGIN
		INSERT INTO Ingredient([RiskCategory], [Text], [SubText], [Approved],
		[Remarks], 
		--[ReviewdBy], [ReviewdByName], 
		[ChangeType], [RequestID],
		[CreatedOn], [ModifiedOn])

		SELECT 999, [Text], [SubText], [Approved], [Remarks],
		--[ReviewedBy], [ReviewedByName], 
		[ChangeType], @ID, (SELECT GETUTCDATE()),
		(SELECT GETUTCDATE())
		FROM @Ingredient
	END

	IF EXISTS (SELECT 1 FROM @Premise)
	BEGIN
		INSERT INTO Premise([IsLocal], [Name], [Type], [Area], [Schedule],
		[BlockNo], [UnitNo], [FloorNo],[BuildingName], [Address1], [Address2],
		[City], [Province], [Country], [Postal], [Longitude], [Latitude], 
		--[IsPrimary],		[ChangeType], [RequestID],
		[CreatedOn],	[ModifiedOn])

		SELECT [IsLocal], [Name], [Type], [Area], [Schedule], [BlockNo], [UnitNo],
		[FloorNo], [BuildingName],  [Address1],	[Address2], [City], [Province],
		[Country], [Postal], [Longitude], [Latitude], 
		--[IsPrimary],		[ChangeType], @ID,
		(SELECT GETUTCDATE()), (SELECT GETUTCDATE())
		FROM @Premise
	END

	IF EXISTS (SELECT 1 FROM @Characteristics)
	BEGIN
		CREATE TABLE #REQCHAR(ROWNUM INT, [Type] SMALLINT, [Value] NVARCHAR(2000), [CreatedOn] DATETIME2(0),
		[ModifiedOn] DATETIME2(0))
		INSERT INTO #REQCHAR
		SELECT ROW_NUMBER() OVER (ORDER BY [Type]) AS ROWNUM,[Type], [Value], [CreatedOn],
		[ModifiedOn] FROM @Characteristics

		
		DECLARE @TOTREQCHARS INT
		DECLARE @REQCHARID BIGINT
		DECLARE @REQCHARINDEX INT
		SET @REQCHARINDEX = 1;
		SET @TOTREQCHARS = (SELECT COUNT(*) FROM #REQCHAR)	
		WHILE(@REQCHARINDEX <= @TOTREQCHARS)
			BEGIN
				INSERT INTO Characteristic([Type], [Value],  [CreatedOn],
				[ModifiedOn])

				SELECT [Type], [Value],  (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
				FROM #REQCHAR
				WHERE ROWNUM = @REQCHARINDEX 

				SET @REQCHARID = SCOPE_IDENTITY();

				INSERT INTO [dbo].RequestCharacteristics
				(RequestID  ,[CharID])
				VALUES(@ID,@REQCHARID)
				SET @REQCHARINDEX = @REQCHARINDEX + 1
			END

	END

	IF EXISTS (SELECT 1 FROM @Attachments)
	BEGIN
		declare @inserted table (attachmentID bigint)
		
		INSERT INTO Attachment([FileID], [FileName], [Extension],
		[Size], [CreatedOn], [ModifiedOn])
		output inserted.Id into @inserted (attachmentID)
		SELECT [FileID], [FileName], [Extension], [Size],
		(SELECT GETUTCDATE()), (SELECT GETUTCDATE())
		FROM @Attachments
				
		INSERT iNTO RequestAttachments([RequestID], [AttachmentID])
		SELECT @ID, attachmentID FROM @inserted 
	END
	

	IF EXISTS (SELECT 1 FROM @RequestLineItems)
		BEGIN
			DECLARE @TOTREQLINEITEMS INT
			DECLARE @REQLINEITEMID BIGINT
			DECLARE @REQLINEITEMINDEX INT

			DECLARE @TOTREQLINEITEMCHARS INT
			DECLARE @REQLINEITEMCHARID BIGINT
			DECLARE @REQLINEITEMCHARINDEX INT

			SET @REQLINEITEMINDEX = 1;
			SET @TOTREQLINEITEMS =(SELECT COUNT(*) FROM @RequestLineItems)
			WHILE(@REQLINEITEMINDEX <= @TOTREQLINEITEMS)
				BEGIN
						INSERT INTO RequestLineItem([Scheme], [SubScheme], 
						--[ComplianceHistoryID],
						[RequestID], [CreatedOn], [ModifiedOn], [IsDeleted])

						SELECT [Scheme], [SubScheme], 
						--[ComplianceHistoryID], 
						@ID,
						(SELECT GETUTCDATE()), (SELECT GETUTCDATE()), 0
						FROM @RequestLineItems WHERE [INDEX] = @REQLINEITEMINDEX

						SET @REQLINEITEMID = SCOPE_IDENTITY();

						IF EXISTS (SELECT 1 FROM @RequestLineItemCharacteristics)
							BEGIN
								SET @REQLINEITEMCHARINDEX = 1;
								SET @TOTREQLINEITEMCHARS = (SELECT COUNT(*) FROM @RequestLineItemCharacteristics 
															WHERE REFINDEX = @REQLINEITEMINDEX)	
								WHILE(@REQLINEITEMCHARINDEX <= @TOTREQLINEITEMCHARS)
									BEGIN
										INSERT INTO Characteristic([Type], [Value],  [CreatedOn],
										[ModifiedOn])

										SELECT [Type], [Value],  (SELECT GETUTCDATE()), (SELECT GETUTCDATE())
										FROM @RequestLineItemCharacteristics
										WHERE RefIndex = @REQLINEITEMINDEX AND [Index] = @REQLINEITEMCHARINDEX

										SET @REQLINEITEMCHARID = SCOPE_IDENTITY();

										INSERT INTO [dbo].[RequestLineItemCharacteristics]
										([LineItemID]  ,[CharID])
										VALUES(@REQLINEITEMID,@REQLINEITEMCHARID)
										SET @REQLINEITEMCHARINDEX = @REQLINEITEMCHARINDEX + 1
									END 
							END
						
						SET @REQLINEITEMINDEX = @REQLINEITEMINDEX + 1
				END
		END

END

GO
/****** Object:  StoredProcedure [dbo].[UpdateRequests]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TEBS
-- Create date: 21-Aug-2020
-- Description:	Stored procedure to Update Request for HC02 with RFA response
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRequests]
	@ID bigint,
	--@Request ApplicationRequestType READONLY,
	@HalalTeam HalalTeamType READONLY,
	@Menu MenuType READONLY,
	@Ingredient IngredientType READONLY,
	@Premise PremiseType READONLY,
	@Characteristics CharacteristicsType READONLY,
	@Attachments AttachmentsType READONLY,
	@RequestLineItems RequestLineItemType READONLY,
	@RequestLineItemCharacteristics RequestLineItemCharacteristicsType READONLY	

AS
BEGIN
	SET NOCOUNT ON;
	--DECLARE @TEMPMENU TABLE
	--(Scheme smallint not null,
	--[Text] nvarchar(500) not null,
	--[SubText] nvarchar(2000) not null,
	--[ReviewedBy] uniqueidentifier,
	--[Approved] bit not null,
	--[ChangeType] smallint not null,
	--[Remarks] nvarchar(2000) not null,
	--[IsDeleted] bit not null)

	--set @ID=1
	
	--INSERT INTO @TEMPMENU(Scheme,[Text],[SubText],[ChangeType],[IsDeleted])
	--SELECT Scheme,[Text],[SubText],[ChangeType],[IsDeleted] FROM Menu WHERE RequestID = @ID
	IF EXISTS (SELECT 1 FROM @MENU)
	BEGIN
		MERGE Menu AS TARGET
		--USING @TEMPMENU AS SOURCE 
		USING @MENU AS SOURCE 
		ON (TARGET.Scheme = SOURCE.Scheme AND TARGET.[Text] = SOURCE.[Text]
		AND  TARGET.[SubText] = SOURCE.[SubText] AND TARGET.[RequestID] = @ID) 
	
		--When records are matched, update the records if there is any change
		WHEN MATCHED
		THEN UPDATE SET TARGET.[Approved] = SOURCE.[Approved], TARGET.[Remarks] = SOURCE.[Remarks],
		TARGET.[ReviewedBy] = SOURCE.[ReviewedBy],TARGET.[ChangeType] = SOURCE.[ChangeType],
		TARGET.[CreatedOn] = GETUTCDATE(), TARGET.[ModifiedOn] = GETUTCDATE()
	
		--When no records are matched, insert the incoming records from source table to target table
		WHEN NOT MATCHED BY TARGET 
		THEN INSERT ([Scheme], [Text], [SubText], [Approved], [Remarks], [ReviewedBy], [ChangeType],
			[RequestID], [CreatedOn], [ModifiedOn])
			VALUES (
			SOURCE.[Scheme], SOURCE.[Text], SOURCE.[Subtext], SOURCE.[Approved], SOURCE.[Remarks], SOURCE.[ReviewedBy],
			SOURCE.[ChangeType], @ID, (SELECT GETUTCDATE()),(SELECT GETUTCDATE()));
	END

	IF EXISTS (SELECT 1 FROM @Ingredient)
	BEGIN
		MERGE Ingredient AS TARGET
		USING @Ingredient AS SOURCE 
		ON (TARGET.[Text] = SOURCE.[Text] AND  TARGET.[SubText] = SOURCE.[SubText] AND TARGET.[RequestID] = @ID) 
	
		WHEN MATCHED
		THEN UPDATE SET TARGET.[RiskCategory] = SOURCE.[RiskCategory], 
		TARGET.[Approved] = SOURCE.[Approved], TARGET.[Remarks] = SOURCE.[Remarks],
		TARGET.[ReviewedBy] = SOURCE.[ReviewedBy],TARGET.[ChangeType] = SOURCE.[ChangeType],
		TARGET.[CreatedOn] = GETUTCDATE(), TARGET.[ModifiedOn] = GETUTCDATE()
	
		WHEN NOT MATCHED BY TARGET 
		THEN INSERT ([RiskCategory], [Text], [SubText], [Approved],
			[Remarks], [ReviewedBy], [ChangeType], [RequestID],
			[CreatedOn], [ModifiedOn])
			VALUES (
			999, SOURCE.[Text], SOURCE.[Subtext], SOURCE.[Approved], SOURCE.[Remarks], SOURCE.[ReviewedBy],
			SOURCE.[ChangeType], @ID, (SELECT GETUTCDATE()),(SELECT GETUTCDATE()));
	END

	IF EXISTS (SELECT 1 FROM @HalalTeam)
	BEGIN
		MERGE HalalTeam AS TARGET
		USING @HalalTeam AS SOURCE 
		ON (TARGET.[AltID] = SOURCE.[AltID] AND TARGET.[Name] = SOURCE.[Name])

		WHEN MATCHED
		THEN UPDATE SET TARGET.[Designation] = SOURCE.[Designation], TARGET.[Role] = SOURCE.[Role],
		TARGET.[IsCertified] = SOURCE.[IsCertified],TARGET.[JoinedOn] = SOURCE.[JoinedOn],
		TARGET.[ChangeType] = SOURCE.[ChangeType],
		TARGET.[CreatedOn] = GETUTCDATE(), TARGET.[ModifiedOn] = GETUTCDATE()

		WHEN NOT MATCHED BY TARGET 
		THEN INSERT ([AltID], [Name], [Designation],
			[Role], [IsCertified], [JoinedOn], [ChangeType], [RequestID],
			[CreatedOn], [ModifiedOn])
			VALUES (
			SOURCE.[AltID], SOURCE.[Name], SOURCE.[Designation], SOURCE.[Role],
			SOURCE.[IsCertified], SOURCE.[JoinedOn],
			SOURCE.[ChangeType], @ID, (SELECT GETUTCDATE()),(SELECT GETUTCDATE()));
	END

	IF EXISTS (SELECT 1 FROM @Premise)
	BEGIN
		MERGE Premise AS TARGET
		USING @Premise AS SOURCE 
		ON (TARGET.[Name] = SOURCE.[Name])

		WHEN MATCHED
		THEN UPDATE SET TARGET.[IsLocal] = SOURCE.[IsLocal], TARGET.[Type] = SOURCE.[Type],
		TARGET.[Area] = SOURCE.[Area],TARGET.[Schedule] = SOURCE.[Schedule],
		TARGET.[BlockNo] = SOURCE.[BlockNo],TARGET.[UnitNo] = SOURCE.[UnitNo],
		TARGET.[FloorNo] = SOURCE.[FloorNo],TARGET.[BuildingName] = SOURCE.[BuildingName],
		TARGET.[Address1] = SOURCE.[Address1],TARGET.[Address2] = SOURCE.[Address2],
		TARGET.[City] = SOURCE.[City],TARGET.[Province] = SOURCE.[Province],
		TARGET.[Country] = SOURCE.[Country],TARGET.[Postal] = SOURCE.[Postal],
		TARGET.[Longitude] = SOURCE.[Longitude],TARGET.[Latitude] = SOURCE.[Latitude],
		TARGET.[CreatedOn] = GETUTCDATE(), TARGET.[ModifiedOn] = GETUTCDATE()

		WHEN NOT MATCHED BY TARGET 
		THEN INSERT ([IsLocal], [Name], [Type], [Area], [Schedule],
			[BlockNo], [UnitNo], [FloorNo],[BuildingName], [Address1], [Address2],
			[City], [Province], [Country], [Postal], [Longitude], [Latitude], [CreatedOn],	[ModifiedOn])
			VALUES (
			SOURCE.[IsLocal], SOURCE.[Name], SOURCE.[Type], SOURCE.[Area], SOURCE.[Schedule], SOURCE.[BlockNo],
			SOURCE.[UnitNo], SOURCE.[FloorNo], SOURCE.[BuildingName], SOURCE.[Address1], SOURCE.[Address2], SOURCE.[City],
			SOURCE.[Province], SOURCE.[Country], SOURCE.[Postal], SOURCE.[Longitude], SOURCE.[Latitude],
			(SELECT GETUTCDATE()),(SELECT GETUTCDATE()));
	END

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRequestStatus]    Script Date: 28/1/2021 3:07:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to update request status
-- =============================================
-- Modification History
--
-- 27/11/2020 SathiyaPriya
-- Included old status in the update statement
-- =============================================
-- Modification History
--
-- 28/01/2021 SathiyaPriya
-- Included old status as another param from SP
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRequestStatus]
	@ID BIGINT,
    @Status SMALLINT,
    @StatusMinor SMALLINT NULL,
	@OldStatus SMALLINT NULL
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @lastAction DATETIME2(0)

    SELECT @lastAction = [LastAction]
    FROM [Request]
    WHERE [ID] = @ID

    IF @StatusMinor IS NULL
    BEGIN
        SET @lastAction = GETUTCDATE()
    END

    UPDATE [Request]
    SET [Status] = @Status,
        [StatusMinor] = @StatusMinor,
        [LastAction] = @lastAction,
		[OldStatus] = @OldStatus,
        [ModifiedOn] = GETUTCDATE()		
    WHERE [ID] = @ID

    RETURN 0
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 26/11/2020
-- Description:	Validate application request based on scheme and premise
-- =============================================
CREATE PROCEDURE [dbo].[ValidateRequest]
	-- Add the parameters for the stored procedure here
	@Scheme smallint,
	@SubScheme smallint null,	
	@Premise PremiseType READONLY

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 R.*,RL.[ID] as [LineItemID], RL.* , P.[ID] as [PremiseID], P.* 
	FROM [dbo].[Request] R 
	INNER JOIN [dbo].[RequestLineItem] RL ON R.ID = RL.RequestID
	INNER JOIN [dbo].[RequestPremise] RP ON R.ID = RP.RequestID
	INNER JOIN [dbo].[Premise] P ON RP.PremiseID = P.ID AND RP.IsPrimary = 1 AND P.[IsDeleted] = 0
	WHERE RL.Scheme = @Scheme AND isnull(RL.SubScheme,'') = isnull(@SubScheme,'')
	AND ISNULL(P.BlockNo,'') = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(BlockNo)),'') FROM @Premise)
	AND ISNULL(P.UnitNo ,'') = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(UnitNo)),'')  FROM @Premise)
	AND ISNULL(P.FloorNo,'')  = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(FloorNo)),'')  FROM @Premise)
	AND ISNULL(P.BuildingName,'')  = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(BuildingName)),'')  FROM @Premise)
	AND ISNULL(P.Address1,'')  = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(Address1)),'')  FROM @Premise)
	AND ISNULL(P.Postal,'')  = (SELECT TOP 1 ISNULL(LTRIM(RTRIM(Postal)),'')  FROM @Premise)
	AND R.[Status] < 900
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sathiya Priya
-- Create date: 01/12/2020
-- Description: Get Parent Request
-- =============================================
CREATE PROCEDURE GetParentRequest
	-- Add the parameters for the stored procedure here
	@Scheme smallint,
	@SubScheme smallint null,	
	@BlockNo varchar(15),
	@UnitNo varchar(5),
	@FloorNo varchar(5),
	@BuildingName nvarchar(100),
	@Address1 nvarchar(150),
	@Postal varchar(20),
	@Status [SmallIntType] READONLY,
	@ReqTypes [SmallIntType] READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 R.*,RL.[ID] as [LineItemID], RL.* , P.[ID] as [PremiseID], P.* 
	FROM [dbo].[Request] R 
	INNER JOIN [dbo].[RequestLineItem] RL ON R.ID = RL.RequestID
	INNER JOIN [dbo].[RequestPremise] RP ON R.ID = RP.RequestID
	INNER JOIN [dbo].[Premise] P ON RP.PremiseID = P.ID AND RP.IsPrimary = 1 AND P.[IsDeleted] = 0
	WHERE RL.Scheme = @Scheme AND isnull(RL.SubScheme,'') = isnull(@SubScheme,'')
	AND ISNULL(P.BlockNo,'') =isnull(@BlockNo,'')
	AND ISNULL(P.UnitNo ,'') = isnull(@UnitNo,'')
	AND ISNULL(P.FloorNo,'')  = isnull(@FloorNo,'')
	AND ISNULL(P.BuildingName,'')  = isnull(@BuildingName,'')
	AND ISNULL(P.Address1,'')  = isnull(@Address1,'')
	AND ISNULL(P.Postal,'')  = isnull(@Postal,'')
	AND R.[Status] in (SELECT [Val] FROM @Status) 
	AND R.[Type]  in (SELECT [Val] FROM @ReqTypes)  ORDER BY R.ModifiedOn DESC
END
GO

/****** Object:  StoredProcedure [dbo].[SelectRelatedRequest]    Script Date: 17/2/2021 2:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 17-Feb-2021
-- Description:	Stored procedure to Related request based on request Id
-- =============================================
CREATE PROCEDURE [dbo].[SelectRelatedRequest]
	@ID BIGINT 
AS
BEGIN

SET NOCOUNT ON;

;WITH Cte_Request AS
(
	SELECT	Rank() Over(PARTITION BY p.[ID] ORDER BY r.[SubmittedOn] DESC) AS [RowNo],
			r.[ID], r.[RefID], r.[Expedite], r.[SubmittedOn], 
			r.[Status], rsl.[Text] as [StatusText], 
			r.[Type], rtl.[Text] as [TypeText], 
			r.[AssignedTo], o.[Name] as [AssignedToName],
			rli.[ID] as [LineItemID], rli.[Scheme], sl.[Text] as [SchemeText],
			p.[ID] AS [PremiseID], p.[Area], p.[BlockNo], p.[UnitNo], 
			p.[FloorNo], p.[BuildingName], p.[Address1], p.[Address2],
			p.[City], p.[Province], p.[Country], p.[Postal], rp.[IsPrimary], rp.[RequestID], rp.[ChangeType]
    FROM	[dbo].[Request] r
			LEFT JOIN [dbo].[Code] cc ON cc.[ID] = r.[CodeID]
			INNER JOIN [dbo].[RequestStatusLookup] rsl ON rsl.[ID] = r.[Status]
			INNER JOIN [dbo].[RequestTypeLookup] rtl ON rtl.[ID] = r.[Type]
			INNER JOIN (
				SELECT rq.[CodeID] FROM [dbo].[Request] rq WHERE rq.[ID] = @ID
			)rq ON r.[CodeID] = rq.[CodeID] AND r.[ID] NOT IN (@ID)
			LEFT JOIN [dbo].[Officer] o ON o.[ID] = r.[AssignedTo]
			LEFT JOIN [RequestPremise] rp ON rp.[RequestID] = r.[ID] AND rp.[IsPrimary] = 1
			LEFT JOIN [Premise] p ON p.[ID] = rp.[PremiseID]			
			LEFT JOIN [RequestLineItem] rli ON rli.[RequestID] = r.[ID]
			LEFT JOIN [SchemeLookup] sl ON sl.[ID] = rli.[Scheme]
)
SELECT		cr.[ID], cr.[RefID], cr.[Expedite], cr.[SubmittedOn],
			cr.[Status], cr.[StatusText], cr.[Type], cr.[TypeText],
			cr.[AssignedTo], cr.[AssignedToName],
			cr.[LineItemID], cr.[LineItemID] AS [ID], cr.[Scheme], cr.[SchemeText],
			cr.[PremiseID], cr.[PremiseID] AS [ID], cr.[Area], cr.[BlockNo], cr.[UnitNo], 
			cr.[FloorNo], cr.[BuildingName], cr.[Address1], cr.[Address2], cr.[City], cr.[Province], 
			cr.[Country], cr.[Postal], cr.[IsPrimary], cr.[RequestID], cr.[ChangeType]
FROM		Cte_Request cr 
WHERE		cr.[RowNo] = 1
ORDER BY	cr.[SubmittedOn] DESC	
END
GO