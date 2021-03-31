/****** Object:  StoredProcedure [dbo].[InsertOrReplacePremise]    Script Date: 26-01-2021 18:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Editor:		Vasu
-- Create date: 31-Aug-2020
-- Updated date: 22-Jan-2021
-- Description:	Stored procedure to insert or replace premise
-- Update Description : Longtitude, Latitude, Grade and High Priority columns added
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrReplacePremise]
    @ID BIGINT,
    @IsLocal BIT,
    @Name NVARCHAR(150) NULL,
    @Type SMALLINT,
    @Area VARCHAR(15) NULL,
    @Schedule VARCHAR(50) NULL,
    @BlockNo VARCHAR(15) NULL,
    @UnitNo VARCHAR(5) NULL,
    @FloorNo VARCHAR(5) NULL,
    @BuildingName NVARCHAR(100) NULL,
    @Address1 NVARCHAR(150) NULL,
    @Address2 NVARCHAR(150) NULL,
    @City NVARCHAR(100) NULL,
    @Province NVARCHAR(100) NULL,
    @Country NVARCHAR(100) NULL,
    @Postal VARCHAR(20) NULL,
	  @Longtitude DECIMAL(9, 6),
	  @Latitude DECIMAL(9, 6),
	  @Grade SMALLINT NULL,
	  @IsHighPriority BIT NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [Premise] WHERE [ID] = @ID)
    BEGIN
        UPDATE [Premise]
        SET [IsLocal] = @IsLocal,
            [Name] = @Name,
            [Type] = @Type,
            [Area] = @Area,
            [Schedule] = @Schedule,
            [BlockNo] = @BlockNo,
            [UnitNo] = @UnitNo,
            [FloorNo] = @FloorNo,
            [BuildingName] = @BuildingName,
            [Address1] = @Address1,
            [Address2] = @Address2,
            [City] = @City,
            [Province] = @Province,
            [Country] = @Country,
            [Postal] = @Postal,
			      [Longitude]=@Longtitude,
			      [Latitude]=@Latitude,
			      [Grade]=@Grade,
			      [IsHighPriority]=@IsHighPriority,
            [ModifiedOn] = GETUTCDATE()
        WHERE [ID] = @ID
    END
    ELSE
    BEGIN
        INSERT INTO [Premise] ([ID],
            [IsLocal],
            [Name],
            [Type],
            [Area],
            [Schedule],
            [BlockNo],
            [UnitNo],
            [FloorNo],
            [BuildingName],
            [Address1],
            [Address2],
            [City],
            [Province],
            [Country],
            [Postal],
			      [Longitude],
			      [Latitude],
			      [Grade],
			      [IsHighPriority],
            [CreatedOn],
            [ModifiedOn],
            [IsDeleted])
        VALUES (@ID,
            @IsLocal,
            @Name,
            @Type,
            @Area,
            @Schedule,
            @BlockNo,
            @UnitNo,
            @FloorNo,
            @BuildingName,
            @Address1,
            @Address2,
            @City,
            @Province,
            @Country,		      
            @Postal,
            @Longtitude,
			      @Latitude,
			      @Grade,
			      @IsHighPriority,
            GETUTCDATE(),
            GETUTCDATE(),
            0);
    END
    
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[GetPremiseByID]    Script Date: 22-01-2021 13:57:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 22-Jan-2021
-- Description:	Stored procedure to retrieve premise with specified ID
-- =============================================
CREATE PROCEDURE [dbo].[GetPremiseByID]
    @ID bigint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Premise]
    WHERE [ID] = @ID
END

GO