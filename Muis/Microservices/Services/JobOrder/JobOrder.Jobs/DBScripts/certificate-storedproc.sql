/****** Object:  StoredProcedure [dbo].[SelectCertificateFull]    Script Date: 02-02-2021 17:15:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Vasu
-- Create date: 29-Jan-2021
-- Description:	Get the certificate, premise and customer details for periodic inspection scheduler
-- =============================================
CREATE PROCEDURE [dbo].[SelectCertificateFull] 
	@Number VARCHAR(60) NULL,
	@Status SMALLINT NULL,
	@Scheme SMALLINT NULL,
	@SubScheme SMALLINT NULL,
	@CustomerID UNIQUEIDENTIFIER NULL,
	@PremiseID BIGINT NULL
AS
BEGIN
	SET NOCOUNT ON;

	select ce.*,
	pr.[ID] as [PremiseID], pr.*,
	cu.[ID] as [CustomerID], cu.*
	from [Certificate] ce
	inner join [Premise] pr on ce.[PremiseID] = pr.[ID]
	inner join [Customer] cu on ce.[CustomerID] = cu.[ID]
	where (@Number IS NULL OR ce.[NUMBER] = @Number)
    AND (@Status IS NULL OR ce.[Status] = @Status)
	AND (@Scheme IS NULL OR ce.[Scheme] = @Scheme)
	AND	(@SubScheme IS NULL OR ce.[SubScheme] = @SubScheme)
    AND (@CustomerID IS NULL OR ce.[CustomerID] = @CustomerID)
	AND (@PremiseID IS NULL OR ce.[PremiseID] = @PremiseID)
	AND (ce.[IsDeleted] = 0)
END
GO