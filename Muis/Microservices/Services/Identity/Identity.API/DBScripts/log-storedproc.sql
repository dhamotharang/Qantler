/****** Object:  StoredProcedure [dbo].[InsertLog]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to insert Log
-- =============================================
CREATE PROCEDURE [dbo].[InsertLog]
    @Type SMALLINT NULL,
    @RefID VARCHAR(36) NULL,
    @Action NVARCHAR(2000),
    @Raw NVARCHAR(2000) NULL,
    @Notes NVARCHAR(4000) NULL,
    @UserID UNIQUEIDENTIFIER,
    @UserName NVARCHAR(150),
	@ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    IF @Raw IS NULL
    BEGIN
        SET @Raw = '[]'
    END

	INSERT INTO [Log]
    VALUES (@Type,
        @REfID,
        @Action,
        @Raw,
        @Notes,
        @UserID,
        GETUTCDATE(),
        0);
    
    SET @ID = SCOPE_IDENTITY();

    RETURN 0;
END
GO

/****** Object:  StoredProcedure [dbo].[MapClusterToLog]    Script Date: 7/12/2020 7:37:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 07-Dec-2020 
-- Description:	Stored procedure to map cluster to logs.
-- =============================================
CREATE PROCEDURE [dbo].[MapClusterToLog]
    @ClusterID BIGINT,
    @LogID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [ClusterLog]
    VALUES (@ClusterID, @LogID)
END