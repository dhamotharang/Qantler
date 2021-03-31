/****** Object:  StoredProcedure [dbo].[GetSettingsByType]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve settings based on specified type.
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