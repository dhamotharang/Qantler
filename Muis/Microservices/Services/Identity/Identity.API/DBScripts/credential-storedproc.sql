/****** Object:  StoredProcedure [dbo].[GetCredential]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve credential.
-- =============================================
CREATE PROCEDURE [dbo].[GetCredential]
    @Provider SMALLINT,
    @Key NVARCHAR(255),
    @Secret NVARCHAR(2000)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Credential]
    WHERE [ProviderID] = @Provider
    AND [Key] = @Key
    AND [Secret] = @Secret

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[GetCredentialByKey]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve credential by key.
-- =============================================
CREATE PROCEDURE [dbo].[GetCredentialByKey]
    @Provider SMALLINT,
    @Key NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Credential]
    WHERE [ProviderID] = @Provider
    AND [Key] = @Key

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertCredential]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert credential.
-- =============================================
CREATE PROCEDURE [dbo].[InsertCredential]
    @Provider SMALLINT,
    @Key NVARCHAR(255),
    @Secret NVARCHAR(2000),
    @IsTemporary BIT,
    @ExpiresOn DATETIME2(0) NULL,
    @IdentityID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Credential] ([ProviderID],
        [Key],
        [Secret],
        [IsTemporary],
        [ExpiresOn],
        [IdentityID],
        [CreatedOn],
        [ModifiedOn],
        [IsDeleted])
    VALUES (@Provider,
        @Key,
        @Secret,
        @IsTemporary,
        @ExpiresOn,
        @IdentityID,
        GETUTCDATE(),
        GETUTCDATE(),
        0)

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateCredential]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to update credential.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCredential]
    @ID BIGINT,
    @Provider SMALLINT,
    @Key NVARCHAR(255),
    @Secret NVARCHAR(2000),
    @IsTemporary BIT,
    @ExpiresOn DATETIME2(0) NULL
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Credential]
    SET [ProviderID]  = @Provider,
        [Key] = @Key,
        [Secret] = @Secret,
        [IsTemporary] = @IsTemporary,
        [ExpiresOn] = @ExpiresOn,
        [ModifiedOn] = GETUTCDATE()
    WHERE [ID] = @ID

    RETURN 0
END


GO