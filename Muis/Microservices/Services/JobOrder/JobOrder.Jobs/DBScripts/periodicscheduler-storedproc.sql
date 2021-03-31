/****** Object:  StoredProcedure [dbo].[Job_GetSchedulers]    Script Date: 01-02-2021 13:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 29-Jan-2021
-- Description:	Get eligible scheduler for periodic inspection
-- =============================================
CREATE PROCEDURE [dbo].[Job_GetSchedulers]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM PeriodicScheduler 
	WHERE ([LastJobID] is null 
	or [LastJobID] in (select [ID] from JobOrder where [Status]>=400 and [IsDeleted]=0))
	and [NextTargetInspection] <= DATEADD(M,1,CONVERT(Date, GETUTCDATE())) 
	and [Status]=100
END
GO


