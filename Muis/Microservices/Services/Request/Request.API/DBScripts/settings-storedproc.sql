/****** Object:  StoredProcedure [dbo].[GetRequestSettings]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetRequestSettings]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SettingsLog_table TABLE (
    SettingsID BIGINT NOT NULL,
    LogID BIGINT NOT NULL
);

INSERT INTO @SettingsLog_table
SELECT SettingsID, max(LogID) AS LogID
 FROM [SettingsLog] 
 GROUP BY SettingsID

	SELECT  s.*, 
	ISNULL(l.[ID], 0) AS [LogID],l.*,o.[Name] as [UserName]
	FROM [Settings] s
	LEFT JOIN @SettingsLog_table sl ON sl.[SettingsID] = s.[ID]
	LEFT JOIN [Log] l ON l.[ID] = sl.[LogID]
	LEFT JOIN [Officer] o ON o.[ID] = l.[UserId]
	WHERE s.IsDeleted=0
	ORDER BY l.CreatedOn DESC	
 END
 
GO
/****** Object:  StoredProcedure [dbo].[GetSettingsByType]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve settings by type.
-- =============================================
CREATE PROCEDURE [dbo].[GetSettingsByType]
    @Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Settings]
    WHERE [Type] = @Type
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateRequestSettings]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateRequestSettings]
	@Type as smallint,
	@Value as nvarchar(400),
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ID BIGINT
	DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogParam NVARCHAR(2000)
	DECLARE @OldValue NVARCHAR(2000)
	DECLARE @SettingsType NVARCHAR(2000)

	IF EXISTS (SELECT * FROM Settings WHERE Type = @Type)
	BEGIN
		SET @OldValue = (SELECT Value FROM Settings WHERE Type = @Type )

		SET @SettingsType = (SELECT Text FROM SettingsLookup WHERE ID = @Type)

		SET @LogParam = CONCAT('[''', @SettingsType, ''', ''', @OldValue, ''', ''', @Value, ''']')

		UPDATE Settings SET Value = @Value WHERE Type = @Type

		EXEC GetTranslationText 0, 'UpdatedRequestSettings', @ActionText = @LogText OUTPUT
		
		EXEC InsertLog 0, NULL, @LogText, @LogParam, NULL, @UserID, @UserName, @ID = @LogID OUTPUT
             
		SET @ID = (SELECT id from Settings WHERE Type = @Type)
	
		INSERT INTO [SettingsLog] VALUES (@ID, @LogID)

		RETURN 0;
	END
END
GO