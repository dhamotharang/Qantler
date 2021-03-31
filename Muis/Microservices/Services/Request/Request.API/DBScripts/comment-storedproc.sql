/****** Object:  StoredProcedure [dbo].[GetCommentByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve comment by id
-- =============================================
CREATE PROCEDURE [dbo].[GetCommentByID]
    @ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Comment]
    WHERE [ID] = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[InsertComment]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to insert comment
-- =============================================
CREATE PROCEDURE [dbo].[InsertComment]
    @BatchID BIGINT,
    @Text NVARCHAR(500),
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150),
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @UserID, @UserName

    INSERT INTO [Comment]
    VALUES (@Text,
        @UserID,
        @BatchID,
        GETUTCDATE(),
        GETUTCDATE(),
        0)
    
    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[SelectComments]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to retrieve comment specified by parameters.
-- =============================================
CREATE PROCEDURE [dbo].[SelectComments]
    @BatchID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Comment]
    WHERE [BatchID] = @BatchID
END


GO