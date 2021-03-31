/****** Object:  StoredProcedure [dbo].[AddInvitee]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to add invitee
-- =============================================
CREATE PROCEDURE [dbo].[AddInvitee]
    @ID BIGINT,
    @OfficerID UNIQUEIDENTIFIER,
    @OfficerName NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    EXEC InsertOrReplaceOfficer @OfficerID, @OfficerName

    INSERT INTO [Invitees]
    VALUES(@ID, @OfficerID)

    RETURN 0
END


GO
/****** Object:  StoredProcedure [dbo].[GetAttendeesByJobID]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Naqi, Imam
-- Create date: 22-June-2020
-- Description:	Stored procedure to Get Attendees By Job ID
-- =============================================
CREATE PROCEDURE [dbo].[GetAttendeesByJobID]
	@ID bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Attendee Where JobID = @ID AND IsDeleted = 0
	
END

GO
/****** Object:  StoredProcedure [dbo].[RemoveInvitee]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 31-Aug-2020
-- Description:	Stored procedure to remove invitee
-- =============================================
CREATE PROCEDURE [dbo].[RemoveInvitee]
    @ID BIGINT,
    @OfficerID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Invitees]
    WHERE [JobID] = @ID
    AND [OfficerID] = @OfficerID

    RETURN 0
END


GO