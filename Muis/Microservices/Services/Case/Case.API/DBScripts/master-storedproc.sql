/****** Object:  StoredProcedure [dbo].[GetMaster]    Script Date: 2/2/2021 9:50:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 01-Feb-2021
-- Description:	Stored procedure to Get master
-- =============================================
CREATE PROCEDURE [dbo].[GetMaster]
	@Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Master]
	WHERE [IsDeleted] = 0 AND [Type] = @Type ORDER BY [CreatedOn] DESC

END
GO