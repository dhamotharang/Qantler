SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Request status key name
-- =============================================
CREATE FUNCTION GetRequestStatusKey
(	
	@Value [smallint]
)
RETURNS [varchar](30) 
AS
BEGIN
	DECLARE @Result [varchar](30) = ''

	IF @Value = 250
		SET @Result = 'SU'
	ELSE IF @Value = 400
		SET @Result = 'AO'
	ELSE IF @Value = 550
		SET @Result = 'Mufti'
	ELSE IF @Value = 600 OR @Value = 800
		SET @Result = 'IO'
	ELSE IF @Value = 700
		SET @Result = 'Finance'

	RETURN @Result
END
GO