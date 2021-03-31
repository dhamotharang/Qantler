/****** Object:  StoredProcedure [dbo].[GetChecklistHistoryByID]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to retrieve checklist history by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetChecklistHistoryByID]
	@ID bigint	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT hist.*, scheme.[Text] as [SchemeText],
        cat.[ID] as [CategoryID], cat.*,
        item.[ID] as [ItemID], item.*
    FROM [ChecklistHistory] hist
    LEFT JOIN [SchemeLookup] scheme ON scheme.[ID] = hist.[Scheme]
    LEFT JOIN [ChecklistCategory] cat ON cat.[HistoryID] = hist.[ID]
    LEFT JOIN [ChecklistItem] item ON item.[CategoryID] = cat.[ID]
    WHERE hist.[ID] = @ID
END
GO
/****** Object:  StoredProcedure [dbo].[GetChecklistHistoryByScheme]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 21-Sep-2020
-- Description:	Stored procedure to get list of checklist by scheme
-- =============================================
CREATE PROCEDURE [dbo].[GetChecklistHistoryByScheme]
	@ID SMALLINT	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT hist.*, scheme.[Text] as [SchemeText], users.Name as [CreatedByName],
        cat.[ID] as [CategoryID], cat.*,
        item.[ID] as [ItemID], item.*
    FROM [ChecklistHistory] hist
    LEFT JOIN  [SchemeLookup] scheme ON scheme.[ID] = hist.[Scheme]
    LEFT JOIN [ChecklistCategory] cat ON cat.[HistoryID] = hist.[ID]
    LEFT JOIN [ChecklistItem] item ON item.[CategoryID] = cat.[ID]
	LEFT JOIN [Identity] users  ON users.ID = hist.CreatedBy
    WHERE hist.Scheme = @ID and hist.IsDeleted = 0 order by hist.Version desc
END

GO
/****** Object:  StoredProcedure [dbo].[GetLatestChecklist]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to retrieve the latest checklist for the specified scheme
-- =============================================
CREATE PROCEDURE [dbo].[GetLatestChecklist]
	@Scheme	SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT hist.*, scheme.[Text] as [SchemeText],
        cat.[ID] as [CategoryID], cat.*,
        item.[ID] as [ItemID], item.*
    FROM [ChecklistHistory] hist
    LEFT JOIN [SchemeLookup] scheme ON scheme.[ID] = hist.[Scheme]
    LEFT JOIN [ChecklistCategory] cat ON cat.[HistoryID] = hist.[ID]
    LEFT JOIN [ChecklistItem] item ON item.[CategoryID] = cat.[ID]
    WHERE hist.[Scheme] = @Scheme
    AND hist.[IsDeleted] = 0
    AND hist.[Version] = (SELECT MAX(Version) FROM [ChecklistHistory] WHERE [Scheme] = @Scheme)
END


GO
/****** Object:  StoredProcedure [dbo].[InsertChecklistCategory]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 21-Sep-2020
-- Description:	Stored procedure to insert checklist category
-- =============================================
CREATE PROCEDURE [dbo].[InsertChecklistCategory] 
	-- Add the parameters for the stored procedure here
	@Index SMALLINT,
	@Text NVARCHAR(4000),
	@HistoryID BIGINT,
	@ID BIGINT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ChecklistCategory]([Index], [Text], [HistoryID])
	SELECT @Index, @Text, @HistoryID

	SET @ID = SCOPE_IDENTITY()

	RETURN 0;
END

GO
/****** Object:  StoredProcedure [dbo].[InsertChecklistHistory]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 21-Sep-2020
-- Description:	Stored procedure to insert checklist history
-- =============================================
CREATE PROCEDURE [dbo].[InsertChecklistHistory] 
	-- Add the parameters for the stored procedure here
	@Scheme SMALLINT,
	@Version SMALLINT,
	@CreatedBy UNIQUEIDENTIFIER,
	@EffectiveFrom DATETIME2(0),
	@ID BIGINT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ChecklistHistory]([Scheme], [Version], [CreatedBy], [EffectiveFrom], [CreatedOn], [IsDeleted], [ModifiedOn])
	SELECT @Scheme, @Version, @CreatedBy, @EffectiveFrom, GETUTCDATE(), 0, GETUTCDATE()

	SET @ID = SCOPE_IDENTITY()

	RETURN 0;
END

GO
/****** Object:  StoredProcedure [dbo].[InsertChecklistItem]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 21-Sep-2020
-- Description:	Stored procedure to insert checklist items
-- =============================================
CREATE PROCEDURE [dbo].[InsertChecklistItem] 
	-- Add the parameters for the stored procedure here
	@Index SMALLINT,
	@Text NVARCHAR(4000),
	@CategoryID BIGINT,
	@Notes NVARCHAR(4000) = NULL,
	@ID BIGINT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ChecklistItem]([Index], [Text], [CategoryID], [Notes])
	SELECT @Index, @Text, @CategoryID, @Notes

	SET @ID = SCOPE_IDENTITY()

	RETURN 0;
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateChecklistHistory]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 21-Sep-2020
-- Description:	Stored procedure to update checklist history
-- =============================================
CREATE PROCEDURE [dbo].[UpdateChecklistHistory] 
	-- Add the parameters for the stored procedure here
	@ID BIGINT,
	@Scheme SMALLINT,
	@Version SMALLINT,
	@EffectiveFrom DATETIME2(0)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ChecklistHistory]
	SET
	[Scheme] = @Scheme,
	[Version] = @Version,
	[EffectiveFrom] = @EffectiveFrom,
	[ModifiedOn] = GETUTCDATE()
	WHERE ID = @ID
	SELECT @Scheme, @Version, @EffectiveFrom, GETUTCDATE()

	--Delete old checklist category and list items
	DELETE FROM [dbo].[ChecklistItem] 
	WHERE [CategoryID] IN (SELECT [ID] FROM [dbo].[ChecklistCategory] WHERE [HistoryID] = @ID)

	DELETE FROM [dbo].[ChecklistCategory] WHERE [HistoryID] = @ID;

	RETURN 0;
END

GO