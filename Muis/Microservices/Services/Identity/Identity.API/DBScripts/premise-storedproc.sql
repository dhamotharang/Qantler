/****** Object:  StoredProcedure [dbo].[GetPremise]    Script Date: 10/2/2021 10:20:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Israel
-- Create date: 08-Feb-2021 
-- Description:	Stored procedure to retrieve premise.
-- =============================================
CREATE PROCEDURE [dbo].[GetPremise]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT P.*, PTL.[Text] AS TypeText,
			C.[ID] AS 'PremiseCustomerID', C.*,IDT.[Text] AS 'IDTypeText',CS.[Text] AS 'StatusText'
    FROM 
	[dbo].[Premise] AS P
	INNER JOIN [dbo].[PremiseTypeLookup] AS PTL ON PTL.[ID] = P.[Type]
	LEFT JOIN [dbo].[Customer] AS C ON P.[CustomerID] = C.[ID]
	LEFT JOIN [dbo].[IDTypeLookup] AS IDT ON C.[IDType] = IDT.[ID]
	LEFT JOIN [dbo].[CustomerStatusLookup] AS CS ON C.[Status] = cs.[ID]
    RETURN 0
END