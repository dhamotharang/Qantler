/****** Object:  StoredProcedure [dbo].[InsertOrReplaceCustomer]    Script Date: 2020-11-19 11:27:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Ang, Keyfe
-- Editor: Vasu
-- Create date: 31-Aug-2020
-- Updated date: 26-Jan-2021
-- Description:	Stored procedure to insert customer if does not exists, otherwise update info
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrReplaceCustomer]
    @ID UNIQUEIDENTIFIER,
    @Name NVARCHAR(150),
    @Code NVARCHAR(30),
    @GroupCode NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [Customer] WHERE [ID] = @ID)
    BEGIN
        INSERT INTO [Customer] ([ID],
            [Name],
            [Code],
            [IsDeleted])
        VALUES (@ID,
            @Name,
            @Code,
            0)
    END
    ELSE
    BEGIN
        UPDATE [Customer]
        SET [Name] = @Name,
            [Code] = @Code,
			      [GroupCode]=@GroupCode
        WHERE [ID] = @ID
    END

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerByID]    Script Date: 26-01-2021 17:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vasu
-- Create date: 26-Jan-2021
-- Description:	Stored procedure to retrieve customer with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerByID]
    @ID uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Customer]
    WHERE [ID] = @ID
END

GO