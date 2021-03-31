GO
/****** Object:  StoredProcedure [dbo].[DeleteAllNotification]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to delete all notification for specified user.
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAllNotification]
    @UserID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [UserNotification]
    SET [State] = 2,
        [ModifiedOn] = GETUTCDATE()
    WHERE [UserID] = @UserID
        AND [State] <> 2

END


GO
/****** Object:  StoredProcedure [dbo].[GetNotificationByID]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to retrieve notification base on specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetNotificationByID]
    @ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Notification]
    WHERE [ID] = @ID

END


GO
/****** Object:  StoredProcedure [dbo].[InsertNotification]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to insert notification
-- =============================================
CREATE PROCEDURE [dbo].[InsertNotification]
    @Title NVARCHAR(100),
    @Preview NVARCHAR(1024) NULL,
    @Body NVARCHAR(2000),
    @RefID NVARCHAR(36) NULL,
    @Module NVARCHAR(36),
    @Category SMALLINT,
    @Level SMALLINT,
    @ContentType SMALLINT,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Notification] ([Title],
        [Preview],
        [Body],
        [RefID],
        [Module],
        [Category],
        [Level],
        [ContentType],
        [CreatedOn])
    VALUES (@Title,
        @Preview,
        @Body,
        @RefID,
        @Module,
        @Category,
        @Level,
        @ContentType,
        GETUTCDATE())
    
    SET @ID = SCOPE_IDENTITY()
    

    RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[MapUserToNofication]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to map users to notification
-- =============================================
CREATE PROCEDURE [dbo].[MapUserToNofication]
    @NotificationID BIGINT,
    @UserIDs UNIQUEIDENTIFIERTYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [UserNotification] ([UserID],
        [NotificationID],
        [State],
        [ModifiedOn])
    SELECT [Val], @NotificationID, 0, GETUTCDATE()
    FROM @UserIDs

    RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[SelectNotification]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to select notification base on specified criteria
-- =============================================
CREATE PROCEDURE [dbo].[SelectNotification]
    @UserID UNIQUEIDENTIFIER NULL,
    @From DATETIME2(0) NULL,
    @LastModified DATETIME2(0) NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Notification] n
    INNER JOIN [UserNotification] un ON un.[NotificationID] = n.[ID]
    WHERE (@UserID IS NULL OR un.[UserID] = @UserID)
        AND (@From IS NULL OR n.[CreatedOn] >= @From)
        AND (@LastModified IS NULL OR un.[ModifiedOn] > @LastModified)

END


GO
/****** Object:  StoredProcedure [dbo].[UpdateNotificationState]    Script Date: 2020-11-19 11:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 14-Sep-2020 
-- Description:	Stored procedure to update notification state
-- =============================================
CREATE PROCEDURE [dbo].[UpdateNotificationState]
    @NotificationID BIGINT,
    @UserID UNIQUEIDENTIFIER,
    @State SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [UserNotification]
    SET [State] = @State
    WHERE [NotificationID] = @NotificationID
    AND [UserID] = @UserID

    RETURN 0
END

GO
