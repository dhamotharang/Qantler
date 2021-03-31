/****** Object:  StoredProcedure [dbo].[DeleteMaster]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Oct-2020
-- Description:	Stored procedure to delete master
-- =============================================
CREATE PROCEDURE [dbo].[DeleteMaster]
	@ID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master] SET [IsDeleted]=1 WHERE [ID] = @ID

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[GetMaster]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Oct-2020
-- Description:	Stored procedure to delete master
-- =============================================
CREATE PROCEDURE [dbo].[GetMaster]
	@Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Type], [ID], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted] FROM [dbo].[Master]
	WHERE [IsDeleted] = 0 AND [Type] = @Type ORDER BY [CreatedOn] DESC

END
GO
/****** Object:  StoredProcedure [dbo].[InsertMaster]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Oct-2020
-- Description:	Stored procedure to insert master
-- =============================================
CREATE PROCEDURE [dbo].[InsertMaster]
	@Type SMALLINT NULL,
	@ID UNIQUEIDENTIFIER,
	@Value NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master] ([Type],
		[ID],
		[Value],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@Type,
		@ID,
		@Value,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateMaster]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 19-Oct-2020
-- Description:	Stored procedure to update master
-- =============================================
CREATE PROCEDURE [dbo].[UpdateMaster]
	@Type SMALLINT NULL,
	@ID UNIQUEIDENTIFIER,
	@Value NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master] SET [Type]=@Type, [Value]=@Value, [ModifiedOn]=GETUTCDATE() 
					WHERE [ID] = @ID

	RETURN 0
END
GO