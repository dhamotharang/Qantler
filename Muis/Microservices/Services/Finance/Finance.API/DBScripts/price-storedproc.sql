/****** Object:  StoredProcedure [dbo].[GetLatestPrice]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Oct-2020
-- Description:	Stored procedure to retrieve latest price for specified code and reference date
-- =============================================
CREATE PROCEDURE [dbo].[GetLatestPrice]
	@Code varchar(15),
	@RefDate datetime2(0)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM [Price] p
	INNER JOIN [TransactionCode] t ON t.[ID] = p.[TransactionCodeID]
	WHERE t.[Code] = @Code
	AND p.[EffectiveFrom] <= @RefDate
	ORDER BY p.[EffectiveFrom] DESC
END

GO
/****** Object:  StoredProcedure [dbo].[InsertPrice]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertPrice]
    @Amount DECIMAL(12,2),
    @EffectiveFrom DATETIME2(0),
	@TransactionCodeID BIGINT,
    @UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogParam NVARCHAR(2000)

	 IF NOT EXISTS (SELECT 1 FROM [Price] WHERE [TransactionCodeID] = @TransactionCodeID and [EffectiveFrom] = @EffectiveFrom)
		BEGIN
			INSERT INTO [dbo].[Price]
			   ([Amount]
			   ,[EffectiveFrom]
			   ,[TransactionCodeID]
			   ,[CreatedOn]
			   ,[ModifiedOn]
			   ,[IsDeleted])
			VALUES
			   (@Amount
			   ,@EffectiveFrom
			   ,@TransactionCodeID
			   ,GETUTCDATE()
			   ,GETUTCDATE()
			   ,0)

			EXEC GetTranslationText 0, 'AddNewPrice', @ActionText = @LogText OUTPUT

			SET @LogParam = CONCAT('[''', @Amount, ''', ''', @EffectiveFrom, ''']')
		
			EXEC InsertLog 0, NULL, @LogText, @LogParam, NULL, @UserID, @UserName, @ID = @LogID OUTPUT
             
			INSERT INTO [dbo].[TransactionCodeLog] VALUES (@TransactionCodeID, @LogID)

			RETURN 0;			
		END
END

GO
/****** Object:  StoredProcedure [dbo].[UpdatePrice]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[UpdatePrice]
	@ID as BIGINT,
	@Amount as DECIMAL(12,2) NULL,
	@EffectiveFrom as DATETIME2(0) NULL,
	@UpdateType AS BIGINT,
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogParam NVARCHAR(2000)
	DECLARE @OldAmount DECIMAL(12,2)
	DECLARE @OldEffectiveForm DATETIME2(0)
	DECLARE @Flag NVARCHAR(200)
	DECLARE @Flags BIT
	DECLARE @TransactionCodeID BIGINT

	IF(@ID > 0)
	BEGIN
	IF EXISTS (SELECT * FROM Price WHERE ID = @ID)
	BEGIN
		SELECT @OldAmount = Amount, @OldEffectiveForm = EffectiveFrom, @TransactionCodeID = TransactionCodeID
		FROM Price WHERE ID = @ID
		IF(@Amount != @OldAmount OR @OldEffectiveForm != @EffectiveFrom)
		BEGIN
			SET @Flag =	CASE
			WHEN @UpdateType = 1 THEN 'EditPriceOnly'		
			WHEN @UpdateType = 2 THEN 'EditDateOnly'
			ELSE 'EditPriseDate'
			END

			SET @LogParam =	CASE
			WHEN @UpdateType = 1 THEN CONCAT('[''', @OldAmount, ''', ''', @EffectiveFrom, ''', ''', @Amount, ''']')
			WHEN @UpdateType = 2 THEN CONCAT('[''', @OldAmount, ''', ''', @OldEffectiveForm, ''', ''', @EffectiveFrom, ''']')
			ELSE CONCAT('[''', @OldAmount, ''', ''', @Amount, ''',''', @OldEffectiveForm, ''', ''', @EffectiveFrom, ''']')
			END

			UPDATE Price SET Amount = @Amount, EffectiveFrom = @EffectiveFrom WHERE ID = @ID

			EXEC GetTranslationText 0, @Flag, @ActionText = @LogText OUTPUT
		
			EXEC InsertLog 0, NULL, @LogText, @LogParam, NULL, @UserID, @UserName, @ID = @LogID OUTPUT
             
			INSERT INTO [dbo].[TransactionCodeLog] VALUES (@TransactionCodeID, @LogID)
		END
	END
	END
	ELSE
	BEGIN
		EXEC [InsertPrice] @Amount, @EffectiveFrom, @UpdateType, @UserID, @UserName
	END
	RETURN 0;
	
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePriceHistory]    Script Date: 2020-11-19 10:55:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePriceHistory]
	@ID as BIGINT,
	@Amount as DECIMAL(12,2) NULL,
	@EffectiveFrom as DATETIME2(0) NULL,
	@UserID UNIQUEIDENTIFIER,
	@UserName NVARCHAR(150),
	@TransactionCodeID BIGINT NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @LogID BIGINT
    DECLARE @LogText NVARCHAR(2000)
	DECLARE @LogParam NVARCHAR(2000)
	DECLARE @OldAmount DECIMAL(12,2)
	DECLARE @OldEffectiveForm DATETIME2(0)
	DECLARE @Flag NVARCHAR(200)
	DECLARE @Flags BIT
	DECLARE @UpdateType BIGINT

	IF(@ID > 0)
	BEGIN
	IF EXISTS (SELECT * FROM Price WHERE ID = @ID)
	BEGIN
		SELECT @OldAmount = Amount, @OldEffectiveForm = EffectiveFrom
		FROM Price WHERE ID = @ID
		IF(@Amount != @OldAmount OR @OldEffectiveForm != @EffectiveFrom)
		BEGIN
			
			IF(@Amount != @OldAmount AND @OldEffectiveForm != @EffectiveFrom)
			BEGIN
				SET @Flag ='EditPriseDate';
				SET @UpdateType = 3;
			END
			ELSE IF(@Amount != @OldAmount)
			BEGIN
				SET @Flag ='EditPriceOnly';
				SET @UpdateType = 1;
			END
			ELSE
			BEGIN
				SET @Flag ='EditDateOnly';
				SET @UpdateType = 2;
			END

			SET @LogParam =	CASE
			WHEN @UpdateType = 1 THEN CONCAT('[''', @OldAmount, ''', ''', @EffectiveFrom, ''', ''', @Amount, ''']')
			WHEN @UpdateType = 2 THEN CONCAT('[''', @OldAmount, ''', ''', @OldEffectiveForm, ''', ''', @EffectiveFrom, ''']')
			ELSE CONCAT('[''', @OldAmount, ''', ''', @Amount, ''',''', @OldEffectiveForm, ''', ''', @EffectiveFrom, ''']')
			END

			UPDATE Price SET Amount = @Amount, EffectiveFrom = @EffectiveFrom WHERE ID = @ID

			EXEC GetTranslationText 0, @Flag, @ActionText = @LogText OUTPUT
		
			EXEC InsertLog 0, NULL, @LogText, @LogParam, NULL, @UserID, @UserName, @ID = @LogID OUTPUT
             
			INSERT INTO [dbo].[TransactionCodeLog] VALUES (@TransactionCodeID, @LogID)
		END
	END
	END
	ELSE
	BEGIN
		EXEC [InsertPrice] @Amount, @EffectiveFrom, @TransactionCodeID, @UserID, @UserName
	END
	RETURN 0;
	
END
GO