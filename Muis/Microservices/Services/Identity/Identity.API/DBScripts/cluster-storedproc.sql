/****** Object:  UserDefinedTableType [dbo].[NodesType]    Script Date: 8/12/2020 2:32:44 PM ******/
CREATE TYPE [dbo].[NodesType] AS TABLE(
	[Node] [varchar](2) NOT NULL
)
GO

/****** Object:  StoredProcedure [dbo].[DeleteCluster]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Nov-2020
-- Description:	Stored procedure to delete cluster
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCluster]
    @id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

	 IF EXISTS (SELECT 1 FROM [Cluster] WHERE [ID] = @id)
		BEGIN
			UPDATE [dbo].[Cluster]SET IsDeleted = 1, ModifiedOn = GETDATE()
				WHERE [ID] = @id

			DELETE FROM [Nodes] WHERE [ClusterID] = @id

			RETURN 0;			
		END
END
GO
/****** Object:  StoredProcedure [dbo].[GetCluster]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetCluster]
AS
BEGIN

	SELECT c.*, c.[District] as [District], c.[Locations] AS [Locations],
	n.[Node] AS [Nodes],
	l.[ID] AS [LogID], l.*,
	'' AS AllNodes

	FROM [dbo].[Cluster] c
	 LEFT JOIN [dbo].[Nodes] n ON n.[ClusterID] = c.[ID]
	 LEFT JOIN [dbo].[ClusterLog] cl ON cl.[ClusterID] = c.[ID]
	 LEFT JOIN [Log] l ON l.[ID] = cl.[LogID]
	WHERE c.IsDeleted=0 AND c.District != ''

 END
GO
/****** Object:  StoredProcedure [dbo].[GetClusterByID]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetClusterByID]
@ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @NodeList varchar(100)

	SELECT @NodeList = COALESCE(@NodeList + ', ', '') + 
	   CAST(Node AS varchar(5))
	FROM Nodes n
	JOIN Cluster c on c.ID = n.ClusterID
	WHERE c.IsDeleted = 0 and c.ID != @ID

	if(@NodeList != '')
	set @NodeList = substring(@NodeList, 1, (len(@NodeList) - 1))

	SELECT c.*, c.[District] as [District], c.[Locations] AS [Locations],
	n.[Node] AS [Nodes],
	l.[ID] AS [LogID], l.*,i.[Name] as [UserName],
	@NodeList AS AllNodes

	FROM [dbo].[Cluster] c
	 LEFT JOIN [dbo].[Nodes] n ON n.[ClusterID] = @ID
	 LEFT JOIN [dbo].[ClusterLog] cl ON cl.[ClusterID] = c.[ID]
	 LEFT JOIN [Log] l ON l.[ID] = cl.[LogID]
	 LEFT JOIN [Identity] i ON i.[ID] = l.[UserId]
	WHERE c.ID = @ID and c.IsDeleted=0
	ORDER BY l.CreatedOn DESC
 END
GO
/****** Object:  StoredProcedure [dbo].[GetClusterByNode]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		SathiyaPriya
-- Create date: 9-11-2020
-- Description:	Get Cluster By Node
-- =============================================
CREATE PROCEDURE [dbo].[GetClusterByNode]
	-- Add the parameters for the stored procedure here
	@Node varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT c.*, c.[District] as [District], c.[Locations] AS [Locations],
	n.[Node] AS [Nodes]	

	FROM [dbo].[Cluster] c
	 LEFT JOIN [dbo].[Nodes] n ON n.[ClusterID] = c.[ID]	
	WHERE c.IsDeleted=0 AND c.District != '' AND n.[Node] = @Node

END
GO
/****** Object:  StoredProcedure [dbo].[InsertCluster]    Script Date: 1/2/2021 11:39:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Israel Ravi
-- Create date: 4-Nov-2020
-- Description:	Stored procedure to insert Cluster
-- =============================================
-- Modification History
-- 29/01/2021 SathiyaPriya
-- Included new column color of the node
-- =============================================
CREATE PROCEDURE [dbo].[InsertCluster] 
    @district NVARCHAR(15),
    @locations NVARCHAR(500),
	@color nvarchar(50),
		@cNode NodesType READONLY,
		@outID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @id BIGINT

	SET @outID = -1

	INSERT INTO [dbo].[Cluster](District,Locations,Color,ModifiedOn,CreatedOn)
	SELECT @district, @locations, @color, GETDATE(),GETDATE()

	SET @id = SCOPE_IDENTITY()

	INSERT INTO [dbo].[Nodes]
	SELECT @id, Node FROM @cNode;

	SET @outID = @id

	RETURN 0;			
	
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateCluster]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCluster]
    @district NVARCHAR(15),
    @locations NVARCHAR(500),
		@cNode NodesType READONLY,
		@cID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM [Cluster] WHERE [ID] = @cID AND [IsDeleted] = 0 AND [Locations] like ISNULL(@locations,''))			
	BEGIN
		UPDATE [dbo].[Cluster] SET Locations = @locations, ModifiedOn = GETDATE(), [District] = @district WHERE [ID] = @cID
	END

	DELETE FROM [Nodes] WHERE ClusterID = @cID

	INSERT INTO [dbo].[Nodes]
	SELECT @cID, Node FROM @cNode

END
GO
