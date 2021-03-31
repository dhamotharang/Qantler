/****** Object:  StoredProcedure [dbo].[InsertSanctionInfo]    Script Date: 24/2/2021 10:43:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 24-Feb-2021
-- Description:	Stored procedure to insert sanction info
-- =============================================
CREATE PROCEDURE [dbo].[InsertSanctionInfo]
	@Type SMALLINT NULL,
	@Sanction SMALLINT NULL,
	@Value VARCHAR(36),
	@CaseID BIGINT,
	@Out BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[SanctionInfo] ([Type],
		[Sanction],
		[Value],
		[CaseID],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@Type,
		@Sanction,
		@Value,
		@CaseID,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	SET @Out = SCOPE_IDENTITY()

	RETURN 0
END

/****** Object:  StoredProcedure [dbo].[GetSanctionInfo]    Script Date: 5/3/2021 12:06:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 5-Mar-2021
-- Description:	Stored procedure to sanction info offender with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetSanctionInfo]
    @CaseID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM [dbo].[SanctionInfo]
    WHERE [IsDeleted] = 0 
	AND (@CaseID IS NULL OR [CaseID] = @CaseID)	

END