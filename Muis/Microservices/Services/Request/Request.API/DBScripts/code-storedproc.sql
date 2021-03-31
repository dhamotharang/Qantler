/****** Object:  StoredProcedure [dbo].[GenerateCodeSeries]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 12-Oct-2020
-- Description:	Stored procedure to generate code series
-- =============================================
CREATE PROCEDURE [dbo].[GenerateCodeSeries]
    @Type SMALLINT,
	@Result INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    IF @Type = 0
    BEGIN
        SET @Result = NEXT VALUE FOR [CustomerCodeSeries]
    END
    ELSE IF @Type = 1
    BEGIN
        SET @Result = NEXT VALUE FOR [CustomerGroupCodeSeries]
    END

END


GO
/****** Object:  StoredProcedure [dbo].[GetCodeByID]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to retrieve code instance by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCodeByID]
    @ID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Code]
    WHERE [ID] = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[InsertCode]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to insert code
-- =============================================
CREATE PROCEDURE [dbo].[InsertCode]
	@Type SMALLINT,
    @Value VARCHAR(36),
    @Text NVARCHAR(255),
    @BillingCycle VARCHAR(4),
    @CertificateExpiry DATETIME2(0) NULL,
    @ID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [Code] ([Type],
        [Value],
        [Text],
        [BillingCycle],
        [CertificateExpiry])
    VALUES (@Type,
        @Value,
        @Text,
        @BillingCycle,
        @CertificateExpiry)
    
    SET @ID = SCOPE_IDENTITY()

    RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[SelectCode]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to select code with specified parameters
-- =============================================
CREATE PROCEDURE [dbo].[SelectCode]
    @Type SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM [Code]
    WHERE [Type] = @Type
END


GO
/****** Object:  StoredProcedure [dbo].[SyncCodeToRequest]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to set code and group code for that aren't yet been approved for the specified customer
-- =============================================
CREATE PROCEDURE [dbo].[SyncCodeToRequest]
	@CustomerID UNIQUEIDENTIFIER,
    @CodeID BIGINT NULL,
    @GroupCodeID BIGINT NULL
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Request]
    SET [CodeID] = @CodeID,
        [GroupCodeID] = @GroupCodeID,
        [ModifiedOn] = GETUTCDATE()
    WHERE [CustomerID] = @CustomerID
    AND [Status] < 500
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateCode]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 04-Sep-2020 
-- Description:	Stored procedure to update code
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCode]
    @ID BIGINT,
	@Type SMALLINT,
    @Value VARCHAR(36),
    @Text NVARCHAR(255),
    @BillingCycle VARCHAR(4),
    @CertificateExpiry DATETIME2(0) NULL
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Code]
    SET [Type] = @Type,
        [Value] = @Value,
        [Text] = @Text,
        [BillingCycle] = @BillingCycle,
        [CertificateExpiry] = @CertificateExpiry
    WHERE [ID] = @ID

    RETURN 0
END

GO

/****** Object:  StoredProcedure [dbo].[SyncOfficerToRequest]    Script Date: 19/3/2021 12:55:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ramesh
-- Create date: 19-Mar-2021
-- Description:	Stored procedure to set officer in-charge for that aren't yet been approved for the specified customer
-- =============================================
CREATE PROCEDURE [dbo].[SyncOfficerToRequest]
	@CustomerID UNIQUEIDENTIFIER,
    @OfficerInCharge UNIQUEIDENTIFIER NULL
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [Request]
    SET [OfficerInCharge] = @OfficerInCharge,
        [ModifiedOn] = GETUTCDATE()
    WHERE [CustomerID] = @CustomerID
    AND [Status] < 500
END
GO