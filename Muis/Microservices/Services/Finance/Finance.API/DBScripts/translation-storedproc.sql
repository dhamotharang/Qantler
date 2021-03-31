/****** Object:  StoredProcedure [dbo].[GetTranslation]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-June-2020
-- Description:	Stored procedure to get translation
-- =============================================
CREATE PROCEDURE [dbo].[GetTranslation]
	@Locale int,
	@Key varchar(60),
	@Text nvarchar(2000) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @Text = [Text]
	FROM Translations
	WHERE [Locale] = @Locale
	AND [Key] = @Key
END

GO