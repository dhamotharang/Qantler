/****** Object:  StoredProcedure [dbo].[GetLetterTemplate]    Script Date: 18-02-2021 14:07:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 18-Feb-2020
-- Description:	Stored procedure to retrieve letter template
-- =============================================
CREATE PROCEDURE [dbo].[GetLetterTemplate]
    @Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [LetterTemplate]
    WHERE [Type] = @Type
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateLetterTemplate]    Script Date: 18-02-2021 14:08:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateLetterTemplate]
	@ID BIGINT,
	@Body VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
		
	Update [LetterTemplate]
	SET [Body] = @Body,
	ModifiedOn = GETUTCDATE()
	WHERE ID = @ID

	RETURN 0
END
GO