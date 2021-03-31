/****** Object:  StoredProcedure [dbo].[GetJobOrderByAssignedToID]    Script Date: 26-02-2021 10:54:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Naqi,,Naqi Imam>
-- Create date: <25-08-2020>
-- Description:	<Get updated Job order assigned to specific auditor>
-- =============================================
CREATE PROCEDURE [dbo].[GetJobOrderByAssignedToID] 
	@AssignedTo uniqueidentifier,
	@LastUpdatedOn datetimeoffset = null
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT j.*,jol.[Text] as [TypeText],jos.[Text] as [StatusText],
	at.[ID] as [AttendeeID], at.*,
	o.[ID] as [OfficerID], o.*,
	f.[ID] AS [FindingsID],f.*, o.[Name] AS [OfficerName],
	fl.[ID] as [FindingsLineItemID]  , fl.*, sl.[Text] AS [SchemeText], su.[Text] AS [SubSchemeText],
	p.[ID] as [PremiseID], p.*,
	a.[ID] as [AttachmentID], a.*,
	fsatt.[ID] as [FindingsAttachmentID], fsatt.*,
	c.[ID] as [CustomerID], c.*,
	jli.[ID] as [LineItemID], jli.*, jlisl.[Text] as [SchemeText], jlissl.[Text] as [SubSchemeText],
	per.[ID] as [PersonID], per.*,
	coninfo.[ID] as [ContactInfoID], coninfo.*

	FROM JobOrder j 
	LEFT JOIN [Customer] c ON c.ID = j.CustomerID
	LEFT JOIN [Attendee] at ON at.[JobID] = j.[ID]
	LeFT JOIN [Invitees] inv ON inv.JobID = j.ID 
	LEFT JOIN [Officer] o ON o.ID = inv.OfficerID
	LEFT JOIN [JobOrderPremises] jp ON jp.JobID = j.id
	LEFT JOIN [Premise] p ON p.ID = jp.PremiseID
	LEFT JOIN [JobOrderTypeLookup] jol ON jol.[ID] =j.[Type]
	LEFT JOIN [JobOrderStatusLookup] jos ON jos.[ID] = j.[Status]
	LEFT JOIN [JobOrderLineItem] jli ON jli.[JobID] = j.[ID]
    LEFT JOIN [SchemeLookup] jlisl ON jlisl.[ID] = jli.[Scheme]
    LEFT JOIN [SubSchemeLookup] jlissl ON jlissl.[ID] = jli.[SubScheme]
	LEFT JOIN [Findings] f on f.JobID = j.ID AND f.IsDeleted = 0
	LEFT JOIN [FindingsSignature] fs ON fs.FindingsID = f.ID
	LEFT JOIN [Attachment] fsatt ON fsatt.ID = fs.AttachmentID
	LEFT JOIN [FindingsLineItem] fl ON fl.FindingsID = f.ID AND fl.IsDeleted = 0
	LEFT JOIN [SchemeLookup] sl ON sl.[ID] = fl.[Scheme]
	LEFT JOIN [SubSchemeLookup] su ON sl.[ID] = fl.[SubScheme]
	LEFT JOIN [FindingsLineItemAttachments] fa ON fa.LineItemID = fl.ID
	LEFT JOIN [Attachment] a ON a.ID = fa.AttachmentID
	LEFT JOIN [Person] per on per.[ID] = j.[ContactPersonID]
	LEFT JOIN [PersonContacts] percon on percon.[PersonID] = per.[ID]
	LEFT JOIN [ContactInfo] coninfo on coninfo.[ID] = percon.[ContactID]
	WHERE j.AssignedTo = @AssignedTo OR O.ID = @AssignedTo
	AND j.[CreatedOn] >= DATEADD(month, -1, GETUTCDATE())
    AND (@LastUpdatedOn IS NULL OR j.[ModifiedOn] > @LastUpdatedOn)
	ORDER BY ScheduledOn
    
END
GO
/****** Object:  StoredProcedure [dbo].[GetJobOrderByID]    Script Date: 26-02-2021 10:54:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to retrieve job order by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetJobOrderByID]
    @ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT jo.*, josl.[Text] as [StatusText], jotl.[Text] as [TypeText],
		    li.[ID] as [LineItemID], li.*, sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
		    c.[ID] as [CustomerID], c.*,
		    o.[ID] as [OfficerID], o.*,
		    p.[ID] as [PremiseID], p.*,
        lia.[ID] as [LineItemAttachment], lia.*,
        ae.[ID] as [Attendee], ae.*,
        invo.[ID] as [Invitee], invo.*,
        l.[ID] as [LogID], l.*, lo.[Name] as [UserName],
        f.[ID] as [Findings], f.*,
        fo.[ID] as [FindingsOfficer], fo.*,
        fli.[ID] as [FindingsLineItem], fli.*,
        flia.[ID] as [FLIAttachment], flia.*,
        fsa.[ID] as [Signature], fsa.*,
		    per.[ID] as [PersonID], per.*,
		    coninfo.[ID] as [ContactInfoID], coninfo.*, [cil].[Text] as [TypeText]
	  FROM [JobOrder] jo
	  INNER JOIN [JobOrderStatusLookup] josl ON josl.[ID] = jo.[Status]
	  INNER JOIN [JobOrderTypeLookup] jotl ON jotl.[ID] = jo.[Type]
	  INNER JOIN [Customer] c ON c.[ID] = jo.[CustomerID]
	  LEFT JOIN [JobOrderLineItem] li ON li.[JobID] = jo.[ID]
	  LEFT JOIN [SchemeLookup] sl ON sl.[ID] = li.[Scheme]
	  LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = li.[SubScheme]
	  LEFT JOIN [Officer] o ON o.[ID] = jo.[AssignedTo]
    LEFT JOIN [JobOrderLineItemAttachments] mlia ON mlia.[LineItemID] = li.[ID]
    LEFT JOIN [Attachment] lia ON lia.[ID] = mlia.[AttachmentID]
    LEFT JOIN [Attendee] ae ON ae.[JobID] = jo.[ID]
    LEFT JOIN [Invitees] inv ON inv.[JobID] = jo.[ID]
    LEFT JOIN [Officer] invo ON invo.[ID] = inv.[OfficerID]
    LEFT JOIN [JobOrderLog] jol  ON jol.[JobID] = jo.[ID]
    LEFT JOIN [Log] l ON l.[ID] = jol.[LogID]
    LEFT JOIN [Officer] lo ON lo.[ID] = l.[UserId]
    LEFT JOIN [Findings] f ON f.[JobID] = jo.[ID]
    LEFT JOIN [Officer] fo ON fo.[ID] = f.[OfficerID]
    LEFT JOIN [FindingsLineItem] fli ON fli.[FindingsID] = f.[ID]
    LEFT JOIN [FindingsLineItemAttachments] mflia ON mflia.[LineItemID] = fli.[ID]
    LEFT JOIN [Attachment] flia ON flia.[ID] = mflia.[AttachmentID]
    LEFT JOIN [FindingsSignature ] fs ON fs.[FindingsID] = f.[ID]
    LEFT JOIN [Attachment] fsa ON fsa.[ID] = fs.[AttachmentID]
	  LEFT JOIN [Person] per ON per.[ID] = jo.[ContactPersonID]
	  LEFT JOIN [PersonContacts] percon on percon.[PersonID] = per.[ID]
	  LEFT JOIN [ContactInfo] coninfo on coninfo.[ID] = percon.[ContactID]
	  LEFT JOIn [ContactInfoTypeLookup] cil ON cil.[ID] = coninfo.[Type]
	  LEFT JOIN (SELECT ip.*,
                ijop.[JobID],
                dbo.FormatPremise([Name], [BlockNo], [Address1], [Address2], [FloorNo], [UnitNo], [BuildingName], [Province], [City], [Country], [Postal]) AS [Text]
                FROM [Premise] ip,
                    [JobOrderPremises] ijop
                WHERE [IsDeleted] = 0
                AND ip.[ID] = ijop.[PremiseID]) p ON p.[JobID] = jo.[ID]
	WHERE jo.[ID] = @ID
END
GO
/****** Object:  StoredProcedure [dbo].[GetJobOrderByIDBasic]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi Imam
-- Create date: 03-Sep-2020
-- Description:	Stored procedure to get job order basic details by ID
-- =============================================
Create PROCEDURE [dbo].[GetJobOrderByIDBasic]
	@ID bigint	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [JobOrder]
    WHERE [ID] = @ID
END
GO
/****** Object:  StoredProcedure [dbo].[InsertJobOrder]    Script Date: 02-02-2021 17:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertJobOrder]
    @RefID BIGINT NULL,
    @Type SMALLINT,
    @Status SMALLINT,
    @Notes NVARCHAR(4000),
    @CustomerID UNIQUEIDENTIFIER,
    @CustomerName NVARCHAR(150),
    @CustomerCode VARCHAR(30),
    @OfficerID UNIQUEIDENTIFIER NULL,
    @OfficerName NVARCHAR(150) NULL,
    @TargetDate DATETIME2(0) NULL,
    @ScheduledOn DATETIME2(0) NULL,
	  @ScheduledOnTo DATETIME2(0) NULL,
    @ID BIGINT OUTPUT,
	  @ContactPersonID UNIQUEIDENTIFIER NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)

    EXEC InsertOrReplaceCustomer @CustomerID, @CustomerName, @CustomerCode, NULL

    IF @OfficerID IS NOT NULL
    BEGIN
        EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName
    END

    INSERT INTO [JobOrder] ([RefID],
        [Type],
        [Status],
        [Notes],
        [CustomerID],
        [AssignedTo],
        [TargetDate],
        [ScheduledOn],
		    [ScheduledOnTo],
		    [ContactPersonID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@RefID,
        @Type,
        @Status,
        @Notes,
        @CustomerID,
        @OfficerID,
        @TargetDate,
        @ScheduledOn,
		    @ScheduledOnTo,
		    @ContactPersonID,
        GETUTCDATE(),
        GETUTCDATE(),
        0);

    SET @ID = SCOPE_IDENTITY()        

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[InsertJobOrderLineItem]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to insert job order lineitem
-- =============================================
CREATE PROCEDURE [dbo].[InsertJobOrderLineItem]
    @Scheme SMALLINT,
    @SubScheme SMALLINT,
    @ChecklistHistoryID BIGINT,
    @JobID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [JobOrderLineItem] ([Scheme],
        [SubScheme],
        [ChecklistHistoryID],
        [JobID],
        [CreatedOn],
        [ModifiedOn],
        [IsDEleted])
    VALUES (@Scheme,
        @SubScheme,
        @ChecklistHistoryID,
        @JobID,
        GETUTCDATE(),
        GETUTCDATE(),
        0);

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertJobOrderPremises]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to insert job order lineitem
-- =============================================
CREATE PROCEDURE [dbo].[InsertJobOrderPremises]
    @IDMappingType IDMappingType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [JobOrderPremises]
    SELECT [A], [B] FROM @IDMappingType

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertRescheduleHistory]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 21-Oct-2020
-- Description:	Stored procedure to insert JobOrderRescheduleHistory
-- =============================================
CREATE PROCEDURE [dbo].[InsertRescheduleHistory]
	@JobID BIGINT,
	@MasterID UNIQUEIDENTIFIER,
	@Notes NVARCHAR(4000) NULL,
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[RescheduleHistory] ([JobID],
		[MasterID],
		[Notes],
		[CreatedOn])
	VALUES (@JobID,
		@MasterID,
		@Notes,
		GETUTCDATE())

	SET @ID = SCOPE_IDENTITY()

	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapLogToJobRequest]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 08-Oct-2020 
-- Description:	Stored procedure to map log to job request
-- =============================================
CREATE PROCEDURE [dbo].[MapLogToJobRequest]
	@JobOrderID BIGINT,
    @LogID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[JobOrderLog]
    VALUES (@JobOrderID, @LogID)

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[SelectJobOrder]    Script Date: 26-02-2021 10:54:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectJobOrder]
	@ID BIGINT NULL,
	@CustomerName NVARCHAR(150) NULL,
    @Premise NVARCHAR(150) NULL,
	@From DATETIME2(0) NULL,
	@To DATETIME2(0) NULL,
    @Type [SmallIntType] READONLY,
    @Status [SmallIntType] READONLY,
	@AssignedTo [UniqueIdentifierType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT jo.*, josl.[Text] as [StatusText], jotl.[Text] as [TypeText],
		li.[ID] as [LineItemID], li.*, sl.[Text] as [SchemeText], ssl.[Text] as [SubSchemeText],
		c.[ID] as [CustomerID], c.*,
		o.[ID] as [OfficerID], o.*,
		p.[ID] as [PremiseID], p.*,
		per.[ID] as [PersonID], per.*,
		coninfo.[ID] as [ContactInfoID], coninfo.*, [cil].[Text] as [TypeText]
	FROM [JobOrder] jo
	INNER JOIN [JobOrderStatusLookup] josl ON josl.[ID] = jo.[Status]
	INNER JOIN [JobOrderTypeLookup] jotl ON jotl.[ID] = jo.[Type]
	INNER JOIN [Customer] c ON c.[ID] = jo.[CustomerID]
	LEFT JOIN [JobOrderLineItem] li ON li.[JobID] = jo.[ID]
	LEFT JOIN [SchemeLookup] sl ON sl.[ID] = li.[Scheme]
	LEFT JOIN [SubSchemeLookup] ssl ON ssl.[ID] = li.[SubScheme]
	LEFT JOIN [Officer] o ON o.[ID] = jo.[AssignedTo]
	LEFT JOIN [Person] per on per.[ID] = jo.[ContactPersonID]
	LEFT JOIN [PersonContacts] percon on percon.[PersonID] = per.[ID]
	LEFT JOIN [ContactInfo] coninfo on coninfo.[ID] = percon.[ContactID]
	LEFT JOIn [ContactInfoTypeLookup] cil ON cil.[ID] = coninfo.[Type]
	LEFT JOIN (SELECT ip.*,
                ijop.[JobID],
                dbo.FormatPremise([Name], [BlockNo], [Address1], [Address2], [FloorNo], [UnitNo], [BuildingName], [Province], [City], [Country], [Postal]) AS [Text]
                FROM [Premise] ip,
                    [JobOrderPremises] ijop
                WHERE [IsDeleted] = 0
                AND ip.[ID] = ijop.[PremiseID]) p ON p.[JobID] = jo.[ID]
	WHERE (@ID IS NULL OR jo.[ID] = @ID)
	AND (NOT EXISTS (SELECT 1 FROM @Type) OR jo.[Type] IN (SELECT [Val] FROM @Type))
  AND (NOT EXISTS (SELECT 1 FROM @Status) OR jo.[Status] IN (SELECT [Val] FROM @Status))
	AND (@Premise IS NULL OR p.[Text] LIKE '%' + @Premise + '%')
	AND (NOT EXISTS (SELECT 1 FROM @AssignedTo) OR jo.[AssignedTo] IN (SELECT [Val] FROM @AssignedTo))
	AND	(@From IS NULL OR jo.[ScheduledOn] >= @From)
  AND (@To IS NULL OR jo.[ScheduledOn] < DATEADD(SS, 1, @To))
	AND (@CustomerName IS NULL OR c.[Name] LIKE '%' + @CustomerName + '%')
 END
GO
/****** Object:  StoredProcedure [dbo].[UpdateJobOrder]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateJobOrder]
    @ID BIGINT,
    @RefID BIGINT NULL,
    @Type SMALLINT,
    @Status SMALLINT,
    @Notes NVARCHAR(4000),
    @CustomerID UNIQUEIDENTIFIER,
    @OfficerID UNIQUEIDENTIFIER NULL,
    @OfficerName NVARCHAR(150) NULL,
    @TargetDate DATETIME2(0) NULL,
    @ScheduledOn DATETIME2(0) NULL,
	  @ScheduledOnTo DATETIME2(0) NULL,
	  @ContactPersonID UNIQUEIDENTIFIER NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @OfficerID IS NOT NULL
    BEGIN
        EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName
    END

    UPDATE [JobOrder]
    SET [RefID] = @RefID,
        [Type] = @Type,
        [Status] = @Status,
        [Notes] = @Notes,
        [CustomerID] = @CustomerID,
        [AssignedTo] = @OfficerID,
        [TargetDate] = @TargetDate,
        [ScheduledOn] = @ScheduledOn,
		    [ScheduledOnTo] = @ScheduledOnTo,
		    [ContactPersonID] = @ContactPersonID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateJobOrderState]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateJobOrderState]
	@ID as bigint,
	@Status as int
AS
BEGIN
	SET NOCOUNT ON;

	IF(@Status <> 400)
		BEGIN
			Update JobOrder set Status = @Status, ModifiedON = (SELECT GETUTCDATE()) WHERE ID = @ID;
		END

	ELSE
		BEGIN
			Update JobOrder set Status = @Status, ModifiedON = (SELECT GETUTCDATE()),
			CompletedOn = (SELECT GETUTCDATE()) WHERE ID = @ID;
		END
END
		
GO
/****** Object:  StoredProcedure [dbo].[MapEmailToJobOrder]    Script Date: 16-02-2021 14:04:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vasu
-- Create date: 12 Feb 2021
-- Description:	Insert records to job order email
-- =============================================
CREATE PROCEDURE [dbo].[MapEmailToJobOrder]
	-- Add the parameters for the stored procedure here
	@JobId bigint,
	@EmailId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into JobOrderEmails([JobId],[EmailId])
	values (@JobId,@EmailId)

	Return 0
END
GO
/****** Object:  StoredProcedure [dbo].[SelectJobOrderEmails]    Script Date: 16-02-2021 09:50:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 16-Feb-2020
-- Description:	Stored procedure to retrieve email ids for job order
-- =============================================
CREATE PROCEDURE [dbo].[SelectJobOrderEmails]
	@JobID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT EmailID
    FROM [JobOrderEmails]
    WHERE [JobID] = @JobID
END