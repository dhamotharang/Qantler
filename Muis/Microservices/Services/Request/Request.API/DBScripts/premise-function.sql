/****** Object:  UserDefinedFunction [dbo].[FormatPremise]    Script Date: 2020-11-19 2:31:11 PM ******/
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


/****** Object:  UserDefinedFunction [dbo].[FormatPremiseWOPostal]    Script Date: 14/1/2021 3:51:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FormatPremiseWOPostal](
    @Name NVARCHAR(150) = NULL,
    @BlockNo VARCHAR(15) = NULL,
    @Address1 NVARCHAR(150) = NULL,
    @Address2 NVARCHAR(150) = NULL,
    @FloorNo VARCHAR(5) = NULL,
    @UnitNo VARCHAR(5) = NULL,
    @BuildingName NVARCHAR(100) = NULL,
    @Province NVARCHAR(100) = NULL,
    @City NVARCHAR(100) = NULL,
    @Country NVARCHAR(100) = NULL)
RETURNS NVARCHAR(700)
BEGIN
    RETURN CONCAT(@BlockNo, ' ', @Address1, ' ', @Address2, CASE WHEN @FloorNo IS NOT NULL THEN ' #' ELSE ' ' END, @FloorNo, CASE WHEN @FloorNo IS NOT NULL THEN '-' ELSE ' ' END, @UnitNo, ' ', @BuildingName, ' ', @Province, ' ', @City, ' ', @Country)
END
GO
