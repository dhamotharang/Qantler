SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Request statistics after insert trigger
-- =============================================
CREATE TRIGGER [dbo].[TR_Request_AI]
   ON  [dbo].[Request]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @ID [bigint]
	DECLARE @Type [smallint]
	DECLARE @Status [smallint]
	DECLARE @AssignedTo [uniqueidentifier]

	-- Statically set timezone to +8 (SG Time)
	DECLARE @DateTime [datetime2](0) = CAST(DATEADD(hour, 8, GETUTCDATE()) AS DATE)
	DECLARE @MonthYear [date] = CAST(DATEADD(day, 1 - DATEPART(day, @DateTime), @DateTime) AS DATE);
	DECLARE @Year [smallint] = YEAR(@DateTime);

    DECLARE ins_cursor CURSOR FOR
	SELECT [ID], [Status], [Type], [AssignedTo] FROM [inserted]
	WHERE [Status] = 200

	OPEN ins_cursor

	FETCH NEXT FROM ins_cursor
	INTO @ID, @Status, @Type, @AssignedTo

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
		VALUES (NULL, 'New', @ID, @Type, @DateTime, @MonthYear, @Year)

		IF @AssignedTo IS NOT NULL
		BEGIN
			IF @Status = 300
				SET @Status = 200

			INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
			VALUES (@AssignedTo, CONCAT(@Status, '_Start'), @ID, @Type, @DateTime, @MonthYear, @Year)
		END

		FETCH NEXT FROM ins_cursor
		INTO @ID, @Status, @Type, @AssignedTo
	END

	CLOSE ins_cursor
	DEALLOCATE ins_cursor
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Request statistics after update trigger
-- =============================================
CREATE TRIGGER [dbo].[TR_Request_AU]
   ON  [dbo].[Request]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @ID [bigint]
	DECLARE @Type [smallint]
	DECLARE @Status [smallint]
	DECLARE @AssignedTo [uniqueidentifier]

	DECLARE @OldStatus [smallint]
	DECLARE @OldAssignedTo [uniqueidentifier]

	DECLARE @Key [varchar](36)
	DECLARE @Event [varchar](30)

	-- Statically set timezone to +8 (SG Time)
	DECLARE @DateTime [datetime2](0) = CAST(DATEADD(hour, 8, GETUTCDATE()) AS DATE)
	DECLARE @MonthYear [date] = CAST(DATEADD(day, 1 - DATEPART(day, @DateTime), @DateTime) AS DATE);
	DECLARE @Year [smallint] = YEAR(@DateTime);

	DECLARE ins_cursor CURSOR FOR
	SELECT [ID], [Status], [Type], [AssignedTo] FROM [inserted]

	OPEN ins_cursor

	FETCH NEXT FROM ins_cursor
	INTO @ID, @Status, @Type, @AssignedTo

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @OldStatus = del.[Status],
			@OldAssignedTo = del.[AssignedTo]
		FROM [deleted] del
		WHERE del.[ID] = @ID

		IF @OldStatus <> @Status
		BEGIN
			-- 300 not considered. For the reason it's part of 200

			-- New/Closed status
			IF @Status = 200
			BEGIN
				INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
				VALUES (NULL, 'New', @ID, @Type, @DateTime, @MonthYear, @Year)
			END
			ELSE IF @Status = 900
			BEGIN
				INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
				VALUES (NULL, 'Closed', @ID, @Type, @DateTime, @MonthYear, @Year)
			END
			ELSE IF @Status = 1000
			BEGIN
				INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
				VALUES (NULL, 'Rejected', @ID, @Type, @DateTime, @MonthYear, @Year)
			END

			-- For new status. 
			IF @Status >= 200 AND @Status <> 300 AND @Status < 900
			BEGIN
				IF @Status = 200
					SET @Key = @AssignedTo
				ELSE
					SET @Key = [dbo].GetRequestStatusKey(@Status)

				INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
				VALUES (@Key, CONCAT(@Status, '_Start'), @ID, @Type, @DateTime, @MonthYear, @Year)
			END

			-- For old status.
			IF @OldStatus >= 200 AND @OldStatus <> 300 AND @OldStatus < 900
			BEGIN
				IF @OldStatus = 200
					SET @Key = @OldAssignedTo
				ELSE
					SET @Key = [dbo].GetRequestStatusKey(@OldStatus)

				INSERT INTO [Statistics] ([Key], [Event], [RequestID], [Type], [CreatedOn], [MonthYear], [Year])
				VALUES (@Key, CONCAT(@OldStatus, '_End'), @ID, @Type, @DateTime, @MonthYear, @Year)
			END

		END

		FETCH NEXT FROM ins_cursor
		INTO @ID, @Status, @Type, @AssignedTo
	END

	CLOSE ins_cursor
	DEALLOCATE ins_cursor

END
GO