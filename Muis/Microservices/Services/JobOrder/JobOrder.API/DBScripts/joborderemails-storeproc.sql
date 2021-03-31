SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 12 Feb 2021
-- Description:	Insert records to job order email
-- =============================================
CREATE PROCEDURE InsertJobOrderEmail
	-- Add the parameters for the stored procedure here
	@JobId bigint,
	@EmailId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into JobOrderEmails([JobId],[EmailId])
	values (@JobId,@EmailId)

	Return 0
END
GO
