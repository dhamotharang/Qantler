/****** Object:  StoredProcedure [dbo].[GetPeriodicSchedulerByPremise]    Script Date: 25-01-2021 16:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25-Jan-2021
-- Description:	Stored procedure to retrieve periodic scheduler with specified premise
-- =============================================
CREATE PROCEDURE [dbo].[GetPeriodicSchedulerByPremise]
    @PremiseId bigint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [PeriodicScheduler]
    WHERE [PremiseID] = @PremiseId
END
GO
/****** Object:  StoredProcedure [dbo].[InsertOrReplacePeriodicScheduler]    Script Date: 25-01-2021 16:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 25 Jan 2021
-- Description:	Stored procedure to insert or replace periodic scheduler
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrReplacePeriodicScheduler] 
	@PremiseId BIGINT,
	@LastJobID BIGINT,
	@LastScheduledOn DATETIME,
	@NextTargetInspection DATETIME,
	@Status SMALLINT
AS
BEGIN
		SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [PeriodicScheduler] WHERE [PremiseId] = @PremiseId)
    BEGIN
        UPDATE [PeriodicScheduler]
				SET [LastJobID]=@LastJobID,
						[LastScheduledOn]=@LastScheduledOn,
						[NextTargetInspection]=@NextTargetInspection,
						[Status]=@Status
				WHERE [PremiseId] = @PremiseId
    END
    ELSE
    BEGIN
        INSERT INTO [PeriodicScheduler] ([LastJobID],
					[PremiseId],
					[LastScheduledOn],
					[NextTargetInspection],
					[Status])
        VALUES (@LastJobID,
					@PremiseId,
					@LastScheduledOn,
					@NextTargetInspection,
					@Status);
    END
		
    RETURN 0
END
