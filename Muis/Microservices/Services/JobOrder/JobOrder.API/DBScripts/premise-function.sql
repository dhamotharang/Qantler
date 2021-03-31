/****** Object:  UserDefinedFunction [dbo].[FormatPremise]    Script Date: 2020-11-19 11:49:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FormatPremise](
    @Name NVARCHAR(150) = NULL,
    @BlockNo VARCHAR(15) = NULL,
    @Address1 NVARCHAR(150) = NULL,
    @Address2 NVARCHAR(150) = NULL,
    @FloorNo VARCHAR(5) = NULL,
    @UnitNo VARCHAR(5) = NULL,
    @BuildingName NVARCHAR(100) = NULL,
    @Province NVARCHAR(100) = NULL,
    @City NVARCHAR(100) = NULL,
    @Country NVARCHAR(100) = NULL,
    @Postal VARCHAR(20) = NULL)
RETURNS NVARCHAR(700)
BEGIN
    RETURN CONCAT(@BlockNo, ' ', @Address1, ' ', @Address2, CASE WHEN @FloorNo IS NOT NULL THEN ' #' ELSE ' ' END, @FloorNo, CASE WHEN @FloorNo IS NOT NULL THEN '-' ELSE ' ' END, @UnitNo, ' ', @BuildingName, ' ', @Province, ' ', @City, ' ', @Country, ' ', @Postal)
END
GO
