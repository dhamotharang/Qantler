/****** Object:  StoredProcedure [dbo].[GetTranslation]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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