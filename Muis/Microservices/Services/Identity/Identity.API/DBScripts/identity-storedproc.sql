/****** Object:  StoredProcedure [dbo].[GetIdentityByID]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve identity by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetIdentityByID]
    @ID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    SELECT i.*, isl.[Text] as [StatusText],
        irt.[RequestType],
        c.[ID] as [ClusterID], c.*,
        n.[Node],
        l.[ID] as [LogID], l.*
    FROM [Identity] i
    INNER JOIN [IdentityStatusLookup] isl ON isl.[ID] = i.[Status]
    LEFT JOIN [IdentityRequestTypes] irt ON irt.[IdentityID] = i.[ID]
    LEFT JOIN [IdentityClusters] ic ON ic.[IdentityID] = i.[ID]
    LEFT JOIN [Cluster] c ON c.[ID] = ic.[ClusterID]
    LEFT JOIN [Nodes] n ON n.[ClusterID] = c.[ID]
    LEFT JOIN [IdentityLogs] il ON il.[IdentityID] = i.[ID]
    LEFT JOIN [Log] l ON l.[ID] = il.[LogID]
    WHERE i.[ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[InsertIdentity]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert identity
-- =============================================
CREATE PROCEDURE [dbo].[InsertIdentity]
    @ID UNIQUEIDENTIFIER,
	@Name NVARCHAR (150),
    @Designation NVARCHAR(150),
    @Role SMALLINT,
    @Permissions VARCHAR(255),
    @Email VARCHAR(100),
    @Status SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Identity] ([ID],
        [Name],
        [Designation],
        [Role],
        [Permissions],
        [Email],
        [Status],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@ID,
        @Name,
        @Designation,
        @Role,
        @Permissions,
        @Email,
        @Status,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertIdentityIfNotExists]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to insert identity if does not exists
-- =============================================
CREATE PROCEDURE [dbo].[InsertIdentityIfNotExists]
    @ID UNIQUEIDENTIFIER,
    @Name NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [Identity] WHERE [ID] = @ID)
    BEGIN
        INSERT INTO [Identity] ([ID], [Name], [IsDeleted])
        VALUES (@ID, @Name, 0)
    END

    RETURN 0;
END


GO
/****** Object:  StoredProcedure [dbo].[MapIdentityToClusters]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to map identity to clusters
-- =============================================
CREATE PROCEDURE [dbo].[MapIdentityToClusters]
    @ID UNIQUEIDENTIFIER,
	@Clusters [BigIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [IdentityClusters]
    WHERE [IdentityID] = @ID

    INSERT INTO [IdentityClusters]
    SELECT @ID, [VAL]
    FROM @Clusters

    UPDATE [Identity]
    SET [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[MapIdentityToLog]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to map identity to logs.
-- =============================================
CREATE PROCEDURE [dbo].[MapIdentityToLog]
    @IdentityID UNIQUEIDENTIFIER,
    @LogID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [IdentityLogs]
    VALUES (@IdentityID, @LogID)
END


GO
/****** Object:  StoredProcedure [dbo].[MapIdentityToRequestTypes]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to map identity to request types
-- =============================================
CREATE PROCEDURE [dbo].[MapIdentityToRequestTypes]
    @ID UNIQUEIDENTIFIER,
	@RequestTypes [SmallIntType] READONLY
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [IdentityRequestTypes]
    WHERE [IdentityID] = @ID

    INSERT INTO [IdentityRequestTypes]
    SELECT @ID, [VAL]
    FROM @RequestTypes

    UPDATE [Identity]
    SET [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[SelectIdentity]    Script Date: 10/2/2021 11:20:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to select identity based on specified filters
-- =============================================
-- Modified by:		Ramesh
-- Modified date: 10-Feb-2021
-- Description:	Added new column for nodes
-- =============================================
CREATE PROCEDURE [dbo].[SelectIdentity]
    @Name [nvarchar](150) NULL,
    @Email [nvarchar](100) NULL,
    @Permissions [SmallIntType] READONLY,
    @IDs [UniqueIdentifierType] READONLY,
    @RequestTypes [SmallIntType] READONLY,
    @Clusters [BigIntType] READONLY,
	@Nodes [NvarcharType] READONLY,
	@Status [SmallInt] = null
AS
BEGIN
	SET NOCOUNT ON;

    SELECT i.*, isl.[Text] AS [StatusText],
        irt.[RequestType],
        c.[ID] as [ClusterID], c.*,
        n.[Node]
    FROM [Identity] i
    INNER JOIN [IdentityStatusLookup] isl ON isl.[ID] = i.[Status]
    LEFT JOIN [IdentityRequestTypes] irt ON irt.[IdentityID] = i.[ID]
    LEFT JOIN [IdentityClusters] ic ON ic.[IdentityID] = i.[ID]
    LEFT JOIN [Cluster] c ON c.[ID] = ic.[ClusterID]
    LEFT JOIN [Nodes] n ON n.[ClusterID] = c.[ID]
    WHERE (NOT EXISTS (SELECT 1 FROM @IDs) OR i.[ID] IN (SELECT [Val] FROM @IDs))
    AND (NOT EXISTS (SELECT 1 FROM @Permissions) OR EXISTS (SELECT 1 FROM @Permissions
                                                            WHERE SUBSTRING(i.[Permissions], [Val] + 1, 1) = '1'))
    AND (NOT EXISTS (SELECT 1 FROM @RequestTypes) OR irt.[RequestType] IN (SELECT [Val] FROM @RequestTypes))
    AND (@Name IS NULL OR i.[Name] LIKE CONCAT('%', @Name, '%'))
    AND (@Email IS NULL OR i.[Email] LIKE CONCAT('%', @Email, '%'))
	AND (@Status IS NULL OR  i.Status = @Status)
    AND (NOT EXISTS (SELECT 1 FROM @Clusters) OR c.[ID] IN (SELECT [Val] FROM @Clusters))
	 AND (NOT EXISTS (SELECT 1 FROM @Nodes) OR n.[Node] IN (SELECT [Val] FROM @Nodes))
    AND i.[ID] <> '00000000-0000-0000-0000-000000000000'
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateIdentity]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to update identity
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIdentity]
    @ID UNIQUEIDENTIFIER,
	@Name NVARCHAR (150),
    @Designation NVARCHAR(150),
    @Role SMALLINT,
    @Permissions VARCHAR(255),
    @Email VARCHAR(100),
    @Status SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Identity]
    SET [Name] = @Name,
        [Designation] = @Designation,
        [Role] = @Role,
        [Permissions] = @Permissions,
        [Email] = @Email,
        [Status] = @Status,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateIdentitySequence]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		SathiyaPriya
-- Create date: 9-11-2020
-- Description:	Update Identity Sequence
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIdentitySequence] 
	-- Add the parameters for the stored procedure here
	  @ID UNIQUEIDENTIFIER,
	  @Sequence SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [Identity]
    SET [Sequence] = @Sequence,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END
GO