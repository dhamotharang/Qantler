/****** Object:  StoredProcedure [dbo].[GetReviewByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to query request review base on specified criteria.
-- =============================================
CREATE PROCEDURE [dbo].[GetReviewByID]
	@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT r.*, o.[Name] as [ReviewerName],
        li.[ID] as [LineItemID], li.*, s.[Text] as [SchemeText], ss.[Text] as [SubSchemeText],
        e.[ID] as [EmailID], e.*
    FROM [Review] r
    INNER JOIN [Officer] o ON o.[ID] = r.[ReviewerID]
    INNER JOIN [ReviewLineItem] li ON li.[ReviewID] = r.[ID]
    INNER JOIN [SchemeLookup] s ON s.[ID] = li.[Scheme]
    LEFT JOIN [SubSchemeLookup] ss ON s.[ID] = li.[SubScheme]
    LEFT JOIN [Email] e ON e.[ID] = r.[EmailID]
    WHERE r.[ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[InsertReview]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert review
-- =============================================
CREATE PROCEDURE [dbo].[InsertReview]
	@Step SMALLINT NULL,
    @ReviewerID UNIQUEIDENTIFIER,
    @ReviewerName NVARCHAR(150),
    @Grade SMALLINT NULL,
    @RefID BIGINT NULL,
    @RequestID BIGINT,
    @EmailID BIGINT NULL,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @ReviewerID, @ReviewerName
    
    INSERT INTO [Review] ([Step],
        [ReviewerID],
        [Grade],
        [RefID],
        [RequestID],
        [EmailID],
        [CreatedOn],
        [IsDeleted])
    VALUES (@Step,
        @ReviewerID,
        @Grade,
        @RefID,
        @RequestID,
        @EmailID,
        GETUTCDATE(),
        0);

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertReviewLineItem]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert review line item
-- =============================================
CREATE PROCEDURE [dbo].[InsertReviewLineItem]
	@Scheme SMALLINT,
    @SubScheme SMALLINT,
    @Approved BIT NULL,
    @Remarks NVARCHAR(2000) NULL,
    @ReviewID BIGINT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [ReviewLineItem]([Scheme],
        [SubScheme],
        [Approved],
        [Remarks],
        [ReviewID])
    VALUES (@Scheme,
        @SubScheme,
        @Approved,
        @Remarks,
        @ReviewID)

    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[QueryReview]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to query request review base on specified criteria.
-- =============================================
CREATE PROCEDURE [dbo].[QueryReview]
	@RequestIDs BIGINTTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    SELECT r.*, o.[Name] as [ReviewerName],
        li.[ID] as [LineItemID], li.*, s.[Text] as [SchemeText], ss.[Text] as [SubSchemeText]
    FROM [Review] r
    INNER JOIN [Officer] o ON o.[ID] = r.[ReviewerID]
    INNER JOIN [ReviewLineItem] li ON li.[ReviewID] = r.[ID]
    INNER JOIN [SchemeLookup] s ON s.[ID] = li.[Scheme]
    LEFT JOIN [SubSchemeLookup] ss ON s.[ID] = li.[SubScheme]
    WHERE (NOT EXISTS (SELECT 1 FROM @RequestIDs) OR r.[RequestID] IN (SELECT [Val] FROM @RequestIDs))
END


GO