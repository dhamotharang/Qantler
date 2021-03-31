/****** Object:  StoredProcedure [dbo].[GetCaseByID]    Script Date: 5/2/2021 3:39:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 26-Jan-2021
-- Description:	Stored procedure to get case by an ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCaseByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.*, StatusMinor AS 'MinorStatus', SL.Text AS 'SourceText', CS.Text AS 'StatusText',
	CSS.Text AS 'OldStatusText',CSM.Text AS 'StatusMinorText',SAL.Text AS 'SanctionText', C.Type,
	CTL.Text as 'TypeText',
	OFFE.ID AS 'offenderID', OFFE.*, 
	CI.ID AS 'offenderContactID', CI.*, CITL.Text AS 'TypeText',
	MOFFI.ID AS 'managedByID', MOFFI.*,
	AOFFI.ID AS 'assignedToID', AOFFI.*,
	RPER.ID AS 'reporterID', RPER.*,
	PCIO.ID AS 'contactID', PCIO.*, PCITL.Text AS 'TypeText',
	BM.ID AS 'breachID', BM.*,
	CM.ID AS 'offences', CM.*,
	ATT.ID AS 'attachmentID', ATT.*,
	CER.ID AS 'certificateID',CER.*,SCL.Text as 'SchemeText', SSL.Text as 'SubSchemeText',
	PRE.ID AS 'premiseID', PRE.*,
	SAN.ID AS 'sanctionInfoID', SAN.*, SALO.Text AS 'SanctionText', SIT.Text AS 'SanctionInfoTypeText'
	FROM [dbo].[Case] AS C
	LEFT JOIN [dbo].[SourceLookup] AS SL ON C.Source = SL.ID AND C.ID = @ID
	LEFT JOIN [dbo].[CaseStatusLookup] AS CS ON C.Status = CS.ID AND C.ID = @ID
	LEFT JOIN [dbo].[CaseStatusMinorLookup] AS CSM ON C.StatusMinor = CSM.ID AND C.ID = @ID
	LEFT JOIN [dbo].[CaseTypeLookup] AS CTL ON C.Type = CTL.ID
	LEFT JOIN [dbo].[SanctionLookup] AS SAL ON C.Sanction = SAL.ID AND C.ID = @ID 
	LEFT JOIN [dbo].[Offender] AS OFFE ON C.OffenderID = OFFE.ID AND C.ID = @ID
	LEFT JOIN [dbo].[OffenderContactInfos] AS OCI ON OFFE.ID = OCI.OffenderID
	LEFT JOIN [dbo].[ContactInfo] AS CI ON OCI.ContactInfoID = CI.ID
	LEFT JOIN [dbo].[ContactInfoTypeLookup] AS CITL ON CI.Type = CITL.ID
	LEFT JOIN [dbo].[Officer] AS MOFFI ON C.ManagedByID = MOFFI.ID AND C.ID = @ID
	LEFT JOIN [dbo].[Officer] AS AOFFI ON C.AssignedToID = AOFFI.ID AND C.ID = @ID
	LEFT JOIN [dbo].[Person] AS RPer ON C.ReportedByID = RPer.ID AND C.ID = @ID
	LEFT JOIN [dbo].[PersonContactInfos] AS PCI ON RPer.ID = PCI.PersonID
	LEFT JOIN [dbo].[ContactInfo] AS PCIO ON PCI.ContactInfoID = PCIO.ID
	LEFT JOIN [dbo].[ContactInfoTypeLookup] AS PCITL ON PCIO.Type = PCITL.ID
	LEFT JOIN [dbo].[CaseBreachCategories] AS CBC ON @ID = CBC.CaseID 
	LEFT JOIN [dbo].[Master] AS BM ON CBC.MasterID = BM.ID
	LEFT JOIN [dbo].[CaseOffences] AS CO ON @ID = CO.CaseID
	LEFT JOIN [dbo].[Master] AS CM ON CO.MasterID = CM.ID
	LEFT JOIN [dbo].[CaseAttachments] AS CATT ON C.ID = CATT.CaseID AND C.ID = @ID
	LEFT JOIN [dbo].[Attachment] AS Att ON CATT.AttachmentID = Att.ID
	LEFT JOIN [dbo].[Certificate] AS CER ON C.ID = CER.CaseID AND C.ID = @ID
	LEFT JOIN [dbo].[SchemeLookup] AS SCL ON CER.Scheme = SCL.ID
	LEFT JOIN [dbo].[SubSchemeLookup] AS SSL ON CER.SubScheme = SSL.ID
	LEFT JOIN [dbo].[CasePremises] AS CP ON C.ID = CP.CaseID AND C.ID = @ID
	LEFT JOIN [dbo].[Premise] AS PRE ON CP.PremiseID = PRE.ID
	LEFT JOIN [dbo].[SanctionInfo] AS SAN ON C.ID = SAN.CaseID AND C.ID = @ID
	LEFT JOIN [dbo].[SanctionLookup] AS SALO ON SAN.Sanction = SALO.ID
	LEFT JOIN [dbo].[SanctionInfoTypeLookup] AS SIT ON SAN.Type = SIT.ID
	WHERE  C.IsDeleted = 0 AND C.ID = @ID

END
GO
/****** Object:  StoredProcedure [dbo].[GetActivityByCaseID]    Script Date: 5/2/2021 3:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 26-Jan-2021
-- Description:	Stored procedure to get Activity by case ID
-- =============================================
CREATE PROCEDURE [dbo].[GetActivityByCaseID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ACT.*,
	ACTOFFI.ID as 'userID', ACTOFFI.*,
	AATT.ID AS 'activityAttachmentID', AATT.*,
	L.ID AS 'letterID', L.ID,L.EmailID,L.Status,L.Type,L.IsDeleted,L.ModifiedOn,L.CreatedOn,
	EM.ID AS 'emailID', EM.*
	FROM [dbo].[Activity] ACT
	LEFT JOIN [dbo].[Officer] AS ACTOFFI ON ACT.UserId = ACTOFFI.ID
	LEFT JOIN [dbo].[ActivityAttachments] AS AAT ON ACT.ID = AAT.ActivityID
	LEFT JOIN [dbo].[Attachment] AS AATT ON AAT.AttachmentID = AATT.ID
	LEFT JOIN [dbo].[ActivityLetters] AS AL ON ACT.ID = AL.ActivityID 
	LEFT JOIN [dbo].[Letter] AS L ON AL.LetterID = L.ID
	LEFT JOIN [dbo].[Email] AS EM ON L.EmailID = EM.ID
	WHERE  ACT.IsDeleted = 0 AND ACT.CaseID = @ID
	ORDER BY ACT.[CreatedOn] DESC

END
GO
/****** Object:  StoredProcedure [dbo].[SelectCase]    Script Date: 2/2/2021 9:50:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 26-Jan-2021
-- Description:	Stored procedure to filter Case based on specified parameters
-- =============================================
CREATE PROCEDURE [dbo].[SelectCase]
	@ID BIGINT = NULL,
	@Source BIGINT = NULL,
	@OffenceType [UniqueIdentifierType] READONLY,
	@ManagedBy [UniqueIdentifierType] READONLY,
	@AssignedTO [UniqueIdentifierType] READONLY,
	@Status [BigIntType] READONLY,
	@From DATETIME = NULL,
	@To DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.*, StatusMinor AS 'MinorStatus', SL.Text AS 'SourceText', CS.Text AS 'StatusText',
		CSM.Text AS 'StatusMinorText', SAL.Text AS 'SanctionText', C.Type, CTL.Text as 'TypeText',
		MOFFI.ID AS 'managedByID', MOFFI.*,
		AOFFI.ID AS 'assignedToID', AOFFI.*,
		RPER.ID AS 'reporterID', RPER.*,
		BC.ID AS 'breachID', BC.*,
		OFFC.ID AS 'offences', OFFC.*
	FROM [dbo].[Case] AS C
	LEFT JOIN [dbo].[SourceLookup] AS SL ON C.Source = SL.ID 
	LEFT JOIN [dbo].[CaseStatusLookup] AS CS ON C.Status = CS.ID 
	LEFT JOIN [dbo].[CaseStatusMinorLookup] AS CSM ON C.StatusMinor = CSM.ID
	LEFT JOIN [dbo].[CaseTypeLookup] AS CTL ON C.Type = CTL.ID
	LEFT JOIN [dbo].[SanctionLookup] AS SAL ON C.Sanction = SAL.ID 
	LEFT JOIN [dbo].[Officer] AS MOFFI ON C.ManagedByID = MOFFI.ID
	LEFT JOIN [dbo].[Officer] AS AOFFI ON C.AssignedToID = AOFFI.ID
	LEFT JOIN [dbo].[Person] AS RPer ON C.ReportedByID = RPer.ID
	LEFT JOIN [dbo].[Offender] AS OFFE ON C.OffenderID = OFFE.ID
	LEFT JOIN (SELECT M.*,CBC.CaseID as 'CaseID' FROM [dbo].[CaseBreachCategories] 
				AS CBC INNER JOIN [dbo].[Master] AS M ON M.ID = CBC.MasterID) 
				AS BC ON C.ID = BC.CaseID
	LEFT JOIN (SELECT M.*,CO.CaseID as 'CaseID' FROM [dbo].[CaseOffences] AS CO INNER JOIN
				[dbo].[Master] AS M ON M.ID = CO.MasterID) 
				AS OFFC ON C.ID = OFFC.CaseID
	WHERE C.IsDeleted = 0 
	AND	(@ID IS NULL OR C.ID = @ID)
	AND (@Source IS NULL OR SL.ID = @Source)
	AND (NOT EXISTS (SELECT 1 FROM @AssignedTo) OR AOFFI.ID IN (SELECT [Val] FROM @AssignedTo))
	AND (NOT EXISTS (SELECT 1 FROM @ManagedBy) OR MOFFI.ID IN (SELECT [Val] FROM @ManagedBy))
	AND (NOT EXISTS (SELECT 1 FROM @OffenceType) OR OFFC.ID IN (SELECT [Val] FROM @OffenceType))
	AND (NOT EXISTS (SELECT 1 FROM @Status) OR C.Status IN (SELECT [Val] FROM @Status))
  AND (@From IS NULL OR C.CreatedOn >= CAST(@From AS DATE))
  AND (@To IS NULL OR C.CreatedOn < DATEADD(DD, 1, CAST(@To AS DATE)))

END
GO
/****** Object:  StoredProcedure [dbo].[InsertCase]    Script Date: 5/2/2021 12:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to create case
-- =============================================
CREATE PROCEDURE [dbo].[InsertCase]
	@Ref NVARCHAR(36) NULL,
	@Source SMALLINT NULL,
	@Type SMALLINT,
	@Background NVARCHAR(4000),
	@OffenderID uniqueidentifier NULL,
	@ReportedByID uniqueidentifier,
	@ManagedByID UNIQUEIDENTIFIER,
	@CreatedByID UNIQUEIDENTIFIER,
	@Title NVARCHAR(255),
	@Out BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Case] ([RefID],
		[Source],
		[Type],
		[Background],
		[OffenderID],
		[ReportedByID],
		[ReportedOn],
		[ManagedByID],
		[CreatedByID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted],
		[Title],
		[Status])
	VALUES (@Ref,
		@Source,
		@Type,
		@Background,
		@OffenderID,
		@ReportedByID,
		GETUTCDATE(),
		@ManagedByID,
		@CreatedByID,
		GETUTCDATE(),
		GETUTCDATE(),
		0,
		@Title,
		100)

	SET @Out = SCOPE_IDENTITY()

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapCaseBreachByOffence]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to breach
-- =============================================
CREATE PROCEDURE [dbo].[MapCaseBreachByOffence]
	@CaseID BIGINT,
	@Offences UniqueIdentifierType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CaseBreachCategories] ([MasterID],[CaseID])
	SELECT Distinct([ParentID]), @CaseID FROM Master WHERE [ID] in (SELECT [val] FROM @Offences)

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapCaseOffence]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to offence
-- =============================================
CREATE PROCEDURE [dbo].[MapCaseOffence]
	@CaseID BIGINT,
	@Offences UniqueIdentifierType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CaseOffences] ([CaseID],[MasterID])
	SELECT @CaseID, [val] FROM @Offences

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapCasePremises]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 04-Feb-2021
-- Description:	Stored procedure to map attachment to case
-- =============================================
CREATE PROCEDURE [dbo].[MapCasePremises]
	@CaseID BIGINT,
    @PremisesIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CasePremises]
    SELECT @CaseID, [Val]
    FROM @PremisesIDs

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCaseStatus]    Script Date: 18/2/2021 11:45:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 15-Feb-2021
-- Description:	Stored procedure to update case status
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCaseStatus]
	@ID BIGINT,
    @Status SMALLINT,
	@StatusMinor SMALLINT NULL
AS
BEGIN
	SET NOCOUNT ON;
 
    UPDATE [dbo].[Case]
    SET [Status] = @Status,
	 [StatusMinor] = @StatusMinor,
	 [ModifiedOn] = GETUTCDATE()		
    WHERE [ID] = @ID

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[MapCasePremises]    Script Date: 24/2/2021 10:52:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 04-Feb-2021
-- Description:	Stored procedure to map case to sanction info
-- =============================================
CREATE PROCEDURE [dbo].[MapCaseSanctionInfo]
	@CaseID BIGINT,
  @SanctionInfoID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CaseSanctionInfos]
    SELECT @CaseID, @SanctionInfoID

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCaseInfo]    Script Date: 24/2/2021 7:12:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 04-Feb-2021
-- Description:	Stored procedure to update case info
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCaseInfo]
	@Ref NVARCHAR(36) NULL,
	@Source SMALLINT,
	@Status SMALLINT,
	@OldStatus SMALLINT NULL,
	@StatusMinor SMALLINT NULL,
	@OtherStatus SMALLINT NULL,
	@OtherStatusMinor SMALLINT NULL,
	@Type SMALLINT NULL,
	@Occurrence SMALLINT NULL,
	@Background NVARCHAR(4000),
	@OffenderID UNIQUEIDENTIFIER NULL,
	@ManagedByID UNIQUEIDENTIFIER NULL,
	@AssignedToID UNIQUEIDENTIFIER NULL,
	@ReportedByID UNIQUEIDENTIFIER NULL,
	@SuspendedFrom DATETIME2(0) NULL,
	@Title NVARCHAR(255),
	@Sanction SMALLINT NULL,
	@ID BIGINT
AS
BEGIN

	SET NOCOUNT ON;

    UPDATE [Case] 
		SET [RefID] = @Ref,
		[Source] = @Source,
		[Status] = @Status, 
		[OldStatus] = @OldStatus, 
		[StatusMinor] = @StatusMinor, 
		[Type] = @Type, 
		[Occurrence] = @Occurrence, 
		[Background] = @Background, 
		[OffenderID] = @OffenderID, 
		[ManagedByID] = @ManagedByID, 
		[AssignedToID] = @AssignedToID,
		[ReportedByID] = @ReportedByID,
		[Title] = @Title, 
		[Sanction] = @Sanction,
		[SuspendedFrom] = @SuspendedFrom,
		[OtherStatus] = @OtherStatus,
		[OtherStatusMinor] = @OtherStatusMinor
	WHERE [ID] = @ID

	RETURN 0
END