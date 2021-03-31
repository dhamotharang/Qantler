GO
/****** Object:  StoredProcedure [dbo].[DeleteFile]    Script Date: 2020-11-19 10:34:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Naqi, Imam
-- Create date: 22-June-2020
-- Description:	Stored procedure to delete notifications for a user
-- =============================================
CREATE PROCEDURE [dbo].[DeleteFile]
	@ID as uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	Update [File] set IsDeleted = 1, ModifiedOn = GETUTCDATE() Where ID = @ID
	
END

GO
/****** Object:  StoredProcedure [dbo].[GetFileByID]    Script Date: 2020-11-19 10:34:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 15-June-2020
-- Description:	Stored procedure to get File by an ID
-- =============================================
CREATE PROCEDURE [dbo].[GetFileByID]
	@ID uniqueidentifier	
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM [File] WHERE ID = @ID)
	BEGIN
		RETURN 0;
	END

	SELECT ID,
		   Directory,
		   FileName,
		   Extension,
		   Size,
		   CreatedOn
		   FROM [File] 
		   WHERE ID = @ID AND isDeleted = 0
	
END

GO
/****** Object:  StoredProcedure [dbo].[InsertFile]    Script Date: 2020-11-19 10:34:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 15-June-2020
-- Description:	Stored procedure to Insert File
-- =============================================
CREATE PROCEDURE [dbo].[InsertFile]
	@ID uniqueidentifier,
	@FileName nvarchar(100),
	@Directory nvarchar(255),
	@Extension varchar(10),
	@Size bigint

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [File]([ID], [Directory], [FileName], [Extension], [Size], [CreatedOn])
	VALUES(@ID, @Directory, @FileName, @Extension, @Size, GETUTCDATE())
 
END

GO
