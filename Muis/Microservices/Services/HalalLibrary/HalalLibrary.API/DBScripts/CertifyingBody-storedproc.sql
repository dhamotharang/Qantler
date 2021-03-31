/****** Object:  StoredProcedure [dbo].[InsertCertifyingBody]    Script Date: 4/1/2021 12:24:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 29-Dec-2020
-- Description:	Stored procedure to insert CertifyingBody
-- =============================================
CREATE PROCEDURE [dbo].[InsertCertifyingBody] 
	-- Add the parameters for the stored procedure here
	@Name NVARCHAR(255),
	@Status SMALLINT,
	@ID BIGINT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM [CertifyingBody] WHERE LOWER([Name]) = LOWER(@Name) AND [IsDeleted] = 0)
	BEGIN
		SET @ID = 0
	END
	ELSE
	BEGIN
		INSERT INTO [CertifyingBody]([Name], [Status], [CreatedOn], [IsDeleted], [ModifiedOn])
		SELECT @Name, @Status, GETUTCDATE(), 0, GETUTCDATE()

		SET @ID = SCOPE_IDENTITY()
	END
	
	RETURN 0;
END
GO

/****** Object:  StoredProcedure [dbo].[SelectCertifyingBody]    Script Date: 2/3/2021 6:57:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel Ravi
-- Create date: 28-Sep-2020
-- Description:	Stored procedure to get list of halal library
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertifyingBody] -- [SelectCertifyingBody] 

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * FROM CertifyingBody c WHERE c.[IsDeleted] = 0
END
GO