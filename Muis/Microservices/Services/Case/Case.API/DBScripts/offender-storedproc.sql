/****** Object:  StoredProcedure [dbo].[InsertOffender]    Script Date: 10/2/2021 9:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 4-Feb-2021
-- Description:	Stored procedure to insert Offender
-- =============================================
CREATE PROCEDURE [dbo].[InsertOffender]
	@OffenderName NVARCHAR(150) NULL,
	@OffenderID UNIQUEIDENTIFIER NULL
AS
BEGIN
	SET NOCOUNT ON;

		INSERT INTO [dbo].[Offender] ([ID], [Name])
		VALUES (@OffenderID, @OffenderName)

	RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[GetOffenderByID]    Script Date: 5/3/2021 12:06:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 5-Mar-2021
-- Description:	Stored procedure to retrieve offender with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetOffenderByID]
    @ID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM [Offender]
    WHERE [ID] = @ID

END