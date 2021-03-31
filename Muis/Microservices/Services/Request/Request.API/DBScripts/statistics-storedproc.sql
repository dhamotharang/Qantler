/****** Object:  StoredProcedure [dbo].[StatsPerformance]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Stored procedure to return performance statistics
-- =============================================
CREATE PROCEDURE [dbo].[StatsPerformance]
	@Keys  [varchartype] READONLY,
	@From  [datetime2](0) NULL,
	@To    [datetime2](2) NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [MonthYear],
		SUM(CASE WHEN [Event] LIKE '%_Start' THEN 1 ELSE 0 END) as [Assigned],
		SUM(CASE WHEN [Event] LIKE '%_END' THEN 1 ELSE 0 END) as [Processed],
		SUM(CASE WHEN [Event] = 'New' THEN 1 ELSE 0 END) as [New],
		SUM(CASE WHEN [Event] = 'Closed' THEN 1 ELSE 0 END) as [Closed]
	FROM [Statistics]
	WHERE (NOT EXISTS (SELECT 1 FROM @Keys) OR [Event] IN ('New', 'Closed') OR [Key] IN (SELECT [val] FROM @Keys))
	AND (@From IS NULL OR [CreatedOn] >= @From)
	AND (@To IS NULL OR [CreatedOn] <= @To)
	GROUP BY [MonthYear]
	ORDER BY [MonthYear]

END 
GO
/****** Object:  StoredProcedure [dbo].[StatsOverview]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Stored procedure to return overview statistics
-- =============================================
CREATE PROCEDURE [dbo].[StatsOverview]
	@From  [datetime2](0) NULL,
	@To    [datetime2](2) NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [MonthYear],
		SUM(CASE WHEN [Type] = 0 THEN 1 ELSE 0 END) as [New],
		SUM(CASE WHEN [Type] = 1 THEN 1 ELSE 0 END) as [Renewal],
		SUM(CASE WHEN [Event] NOT IN (0, 1) THEN 1 ELSE 0 END) as [Amend]
	FROM [Statistics]
	WHERE (@From IS NULL OR [CreatedOn] >= @From)
	AND (@To IS NULL OR [CreatedOn] <= @To)
	AND [Event] = 'New'
	GROUP BY [MonthYear]
	ORDER BY [MonthYear]

END 
GO
/****** Object:  StoredProcedure [dbo].[StatsStatus]    Script Date: 2020-11-19 11:59:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ang, Keyfe
-- Create date: 06-Mar-2021
-- Description:	Stored procedure to return status statistics
-- =============================================
CREATE PROCEDURE [dbo].[StatsStatus]
	@From  [datetime2](0) NULL,
	@To    [datetime2](2) NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [MonthYear],
		SUM(CASE WHEN [Event] = 'New' THEN 1 ELSE 0 END) as [New],
		SUM(CASE WHEN [Event] = 'Closed' THEN 1 ELSE 0 END) as [Closed],
		SUM(CASE WHEN [Event] = 'Rejected' THEN 1 ELSE 0 END) as [Rejected]
	FROM [Statistics]
	WHERE (@From IS NULL OR [CreatedOn] >= @From)
	AND (@To IS NULL OR [CreatedOn] <= @To)
	AND [Event] IN ('New', 'Closed', 'Rejected')
	GROUP BY [MonthYear]
	ORDER BY [MonthYear]

END 
GO
