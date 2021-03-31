/****** Object:  StoredProcedure [dbo].[InsertAttendeeAttachments]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi
-- Create date: 17-Sep-2020
-- Description:	Stored procedure to Insert Attendee attachments
-- =============================================
CREATE PROCEDURE [dbo].[InsertAttendeeAttachments]
	@IDMappingType IDMappingType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [AttendeeSignature] ([AttendeeID], [AttachmentID])
	SELECT [A], [B] FROM @IDMappingType


	RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[InsertAttendees]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Naqi, Imam
-- Create date: 7-Sep-2020
-- Description:	Stored procedure to Insert Attendees
-- =============================================
CREATE PROCEDURE [dbo].[InsertAttendees]
	@Name NVARCHAR(150),
    @Designation NVARCHAR(150),
    @Start BIT, 
    @End BIT, 
    @JobID BIGINT,
    @ID BIGINT OUTPUT
	

AS
BEGIN
	SET NOCOUNT ON;
	

	INSERT INTO Attendee([Name],[Designation], [Start], 
	[End], [JobID], [CreatedOn], [ModifiedOn],[IsDeleted])
	 
	SELECT @Name, @Designation, @Start, @End, @JobID, (SELECT GETUTCDATE()), (SELECT GETUTCDATE()),0

	SET @ID = SCOPE_IDENTITY()

	RETURN 0;
	
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateAttendees]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 22-June-2020
-- Description:	Stored procedure to Update Attendees
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAttendees]
	@ID as bigint,
	@Attendees as dbo.AttendeeType readonly
	
AS
BEGIN
	SET NOCOUNT ON;
		
	MERGE Attendee AS att
    USING @Attendees AS src
      ON src.ID = att.ID
     WHEN MATCHED AND att.JobID = @ID  AND att.IsDeleted = 0 THEN
       UPDATE SET
	          att.Start = (SELECT ISNULL(src.Start, att.Start)),
			  att.[End] = (SELECT ISNULL(src.[End], att.[End] )),
			  att.Name = (SELECT ISNULL(src.Name,att.Name)),  
			  att.Designation = (SELECT ISNULL(src.Designation,att.Designation)) ,
			  att.ModifiedON = (SELECT GETUTCDATE());
             
	
END

GO