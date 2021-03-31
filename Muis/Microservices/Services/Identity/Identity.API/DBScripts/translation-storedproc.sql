/****** Object:  StoredProcedure [dbo].[GetTranslationText]    Script Date: 2020-11-19 11:08:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetTranslationText]
	@Locale int,
	@Key varchar(60),
	@Actiontext nvarchar(2000) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SET @Actiontext = (SELECT [TEXT] FROM Translations  WHERE [Locale] = @Locale and [Key] = @Key)
	
END
GO