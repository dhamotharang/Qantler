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

/****** Object:  StoredProcedure [dbo].[UpdateContactInfo]    Script Date: 26-02-2021 10:51:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25-Feb-2021
-- Description:	Stored procedure to get all contact info of person
-- =============================================
CREATE PROCEDURE [dbo].[UpdateContactInfo]
@Id bigint,
@Type smallint,
@Value nvarchar(100),
@IsPrimary bit
AS
BEGIN
	SET NOCOUNT ON;

	update ContactInfo set Type=@Type,
		Value=@Value,
		IsPrimary=@IsPrimary,
		IsDeleted=0,
		ModifiedOn=GETUTCDATE()
		where ID=@Id

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[SelectContactInfo]    Script Date: 26-02-2021 10:51:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25-Feb-2021
-- Description:	Stored procedure to get all contact info of person
-- =============================================
CREATE PROCEDURE [dbo].[SelectContactInfo] 
	@PersonID uniqueidentifier,
	@Type smallint null
AS
BEGIN
	SET NOCOUNT ON;

	Select * from ContactInfo
	where ID in 
	(select ContactID from PersonContacts where PersonID=@PersonID)
	and (@Type is null or [Type]=@Type)
END
GO
