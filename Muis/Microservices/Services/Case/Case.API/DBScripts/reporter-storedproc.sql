/****** Object:  StoredProcedure [dbo].[InsertReporter]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to Insert Reporter
-- =============================================
CREATE PROCEDURE [dbo].[InsertReporter]
	@ReportedByName NVARCHAR(150),
	@Out UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ReportedByID uniqueidentifier = NEWID();

	INSERT INTO [dbo].[Person] ([ID], [Name], [CreatedOn], [ModifiedOn], [IsDeleted])
	VALUES (@ReportedByID, @ReportedByName, GETUTCDATE(), GETUTCDATE(), 0)
	
	SET @Out = @ReportedByID

	select @ReportedByID

	RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[InsertReporterContactInfo]    Script Date: 5/2/2021 12:04:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to inser reporter contact info
-- =============================================
CREATE PROCEDURE [dbo].[InsertReporterContactInfo]
	@CaseID NVARCHAR(36) NULL,
	@Type SMALLINT,
	@Value NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ContactID BIGINT,
	@ReporterID  UNIQUEIDENTIFIER

	SELECT @ReporterID = ReportedByID FROM [dbo].[Case] WHERE ID = @CaseID

	INSERT INTO [dbo].[ContactInfo]([Type],
		[Value],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@Type,
		@Value,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @ContactID = SCOPE_IDENTITY()

	INSERT INTO [dbo].[PersonContactInfos]([PersonID],
		[ContactInfoID])
	VALUES (@ReporterID,
		@ContactID)

	RETURN 0
END
GO