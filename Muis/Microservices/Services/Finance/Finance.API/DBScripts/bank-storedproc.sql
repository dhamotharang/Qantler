/****** Object:  StoredProcedure [dbo].[InsertBank]    Script Date: 15-03-2021 11:33:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 11-Mar-2021
-- Description:	Stored procedure to insert bank
-- =============================================
CREATE PROCEDURE [dbo].[InsertBank]
	@AccountNo [nvarchar](60) NULL,
	@AccountName [nvarchar](150) NULL,
	@BankName [nvarchar](150) NULL,
	@BranchCode [nvarchar](15) NULL,
	@DDAStatus smallint,
	@ID bigint out
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Bank] ([AccountNo],
		[AccountName],
		[BankName],
		[BranchCode],
		[DDAStatus],
		[CreatedOn],
		[ModifiedOn],
		[IsDeleted])
	VALUES (@AccountNo,
		@AccountName,
		@BankName,
		@BranchCode,
		@DDAStatus,
		GETUTCDATE(),
		GETUTCDATE(),
		0)

		set  @ID=SCOPE_IDENTITY()
END

/****** Object:  StoredProcedure [dbo].[SelectBank]    Script Date: 17-03-2021 09:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 11-Mar-2021
-- Description:	Get bank details by account name
-- =============================================
CREATE PROCEDURE [dbo].[SelectBank]
	@AccountNo NVARCHAR(60),
	@AccountName NVARCHAR(150),
	@BankName  NVARCHAR(150),
	@BranchCode  NVARCHAR(15),
	@DDAStatus SMALLINT NULL = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Bank  
	WHERE (@AccountName is Null or AccountName like '%'+ @AccountName +'%')
	AND (@AccountNo is Null or AccountNo like '%'+ @AccountNo +'%')
	AND (@BankName is Null or BankName like '%'+ @BankName +'%')
	AND (@BranchCode is null or BranchCode like '%'+ @BranchCode +'%')
	AND (@DDAStatus is null or DDAStatus=@DDAStatus)
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateBank]    Script Date: 11-03-2021 15:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 11-Mar-2021
-- Description:	Stored procedure to update bank
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBank]
	@ID bigint,
	@AccountNo [nvarchar](60) NULL,
	@AccountName [nvarchar](150) NULL,
	@BankName [nvarchar](150) NULL,
	@BranchCode [nvarchar](15) NULL,
	@DDAStatus smallint
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Bank]
	SET [AccountNo]=@AccountNo,
		[AccountName]=@AccountName,
		[BankName]=@BankName,
		[BranchCode]=@BranchCode,
		[DDAStatus]=@DDAStatus,
			[ModifiedOn] = GETUTCDATE()
	WHERE [ID] = @ID

		RETURN 0
END
GO