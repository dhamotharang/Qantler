/****** Object:  StoredProcedure [dbo].[GetFindingsByID]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Naqi Imam>
-- Create date: <25-08-2020>
-- Description:	<Get Findings by ID>
-- =============================================
CREATE PROCEDURE [dbo].[GetFindingsByID] 
	@ID BIGINT	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
	f.[ID] as [FindingsID], f.*,
	fsatt.[ID] as [FindingsAttachmentID], fsatt.*,
	fl.[ID] as [FindingsLineItemID]  , fl.*, sl.[Text] AS [SchemeText], su.[Text] AS [SubSchemeText],
	a.[ID] as [AttachmentID], a.*,
	o.[ID] as [OfficerID], o.*
	
	FROM Findings f 
	LEFT JOIN [FindingsSignature] fs ON fs.FindingsID = f.ID
	LEFT JOIN [Attachment] fsatt ON fsatt.ID = fs.AttachmentID
	LEFT JOIN [FindingsLineItem] fl ON fl.FindingsID = f.ID AND fl.IsDeleted = 0
	LEFT JOIN [SchemeLookup] sl ON sl.[ID] = fl.[Scheme]
	LEFT JOIN [SubSchemeLookup] su ON sl.[ID] = fl.[SubScheme]
	LEFT JOIN [FindingsLineItemAttachments] fa ON fa.LineItemID = fl.ID
	LEFT JOIN [Attachment] a ON a.ID = fa.AttachmentID
	LEFT JOIN [Officer] o ON o.ID = f.OfficerID AND o.IsDeleted = 0
	WHERE f.ID = @ID AND f.IsDeleted = 0
    
END
GO
/****** Object:  StoredProcedure [dbo].[InsertFindings]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertFindings]
    @Remarks nvarchar(4000) NULL,
	@OfficerID UNIQUEIDENTIFIER NULL,
	@OfficerName NVARCHAR(150) NULL,
	@JobID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	IF @OfficerID IS NOT NULL
    BEGIN
        EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName
		IF NOT EXISTS(SELECT * FROM Invitees WHERE JobID = @JobID AND OfficerID = @OfficerID)
			BEGIN
				INSERT INTO Invitees values(@JobID, @OfficerID)
			END
    END
	

	INSERT INTO [Findings] ([Remarks],
        [OfficerID],
        [JobID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@Remarks,
        @OfficerID,
        @JobID,
        GETUTCDATE(),
        GETUTCDATE(),
        0);

    SET @ID = SCOPE_IDENTITY()

	UPDATE JobOrder Set ModifiedOn = (SELECT GETUTCDATE()) WHERE ID = @JobID

    RETURN 0;
END


GO
/****** Object:  StoredProcedure [dbo].[InsertFindingsAttachments]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi
-- Create date: 17-Sep-2020
-- Description:	Stored procedure to Insert Findings attachments
-- =============================================
CREATE PROCEDURE [dbo].[InsertFindingsAttachments]
	@IDMappingType IDMappingType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [FindingsSignature] ([FindingsID], [AttachmentID])
	SELECT [A], [B] FROM @IDMappingType


	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertFindingsLineItem]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi Imam
-- Create date: 17-Sep-2020
-- Description:	Stored procedure to insert Findings lineitem
-- =============================================
CREATE PROCEDURE [dbo].[InsertFindingsLineItem]
    @Scheme SMALLINT,
    @SubScheme SMALLINT,
	@Index SMALLINT,
	@ChecklistCategoryID BIGINT,
	@ChecklistCategoryText nvarchar(80),
    @ChecklistItemID BIGINT,
	@ChecklistItemText nvarchar(4000),
	@Remarks nvarchar(4000),
	@Complied bit,
    @FindingsID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [FindingsLineItem] ([Scheme],
        [SubScheme],
		[Index],
        [ChecklistCategoryID],
		[ChecklistCategoryText],
		[ChecklistItemID],
		[ChecklistItemText],
		[Remarks],
        [Complied],
		[FindingsID],
        [CreatedOn],
        [ModifiedOn],
        [IsDEleted])
    VALUES (@Scheme,
        @SubScheme,
		@Index,
        @ChecklistCategoryID,
		@ChecklistCategoryText,
		@ChecklistItemID,
		@ChecklistItemText,
		@Remarks,
		@Complied,
        @FindingsID,
        GETUTCDATE(),
        GETUTCDATE(),
        0);

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertFindingsLineItemAttachments]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi
-- Create date: 17-Sep-2020
-- Description:	Stored procedure to Insert Findings lineitem attachments
-- =============================================
CREATE PROCEDURE [dbo].[InsertFindingsLineItemAttachments]
	@IDMappingType IDMappingType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [FindingsLineItemAttachments] ([LineItemID], [AttachmentID])
	SELECT [A], [B] FROM @IDMappingType


	RETURN 0
END


GO