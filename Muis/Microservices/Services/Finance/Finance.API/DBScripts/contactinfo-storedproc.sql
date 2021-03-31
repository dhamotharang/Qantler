/****** Object:  StoredProcedure [dbo].[InsertContactInfo]    Script Date: 26-02-2021 10:51:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25-Feb-2021
-- Description:	Stored procedure to insert contact info
-- =============================================
CREATE PROCEDURE [dbo].[InsertContactInfo] 
@Type smallint,
@Value nvarchar(100),
@IsPrimary bit,
@ID bigint out
AS
BEGIN
	SET NOCOUNT ON;

	Insert into ContactInfo([Type],
		[Value],
		[IsPrimary],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	values(@Type,
		@Value,
		@IsPrimary,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

	set @ID=SCOPE_IDENTITY()

END
GO
