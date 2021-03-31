ALTER PROCEDURE [dbo].[sps_MoEApproved]
(
  @PageNo int = 1,      
  @PageSize int = 5,
  @UserID int
)
AS
BEGIN
	DECLARE @return INT, @query NVARCHAR(MAX) 
	DECLARE @start INT, @end INT      
	SET @start = (@PageNo - 1) * @PageSize + 1      
	SET @end = @PageNo * @PageSize
	CREATE table #tempData      
	(      
	ContentId INT,       
	ContentType INT,
	Title NVARCHAR(MAX),
	ApprovedBy NVARCHAR(MAX)
	)    
	INSERT INTO #tempData
	SELECT c.ContentId, c.ContentType,(CASE WHEN c.ContentType = 1 THEN (SELECT Title FROM CourseMaster WHERE Id = c.ContentId) 
	ELSE ((SELECT Title FROM ResourceMaster WHERE Id = c.ContentId))END),(SELECT FirstName + ' ' + LastName FROM UserMaster WHERE Id = c.UserId) 
	FROM MoECheckMaster c WHERE [Status] = 1 and UserId = @UserID
IF EXISTS(SELECT TOP 1 1 FROM #tempData)
BEGIN
SET @query =      
	';with sqlpaging as (      
	SELECT       
	Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,      
	* FROM #tempData)      
      
	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(rownumber) from sqlpaging) as         
	 Totalrows    from sqlpaging        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
	END
	ELSE
	BEGIN
		RETURN 113
	END
END
GO

ALTER PROCEDURE [dbo].[sps_CommunityApprovedByUser]
(
  @UserId INT,
  @PageNo int = 1,      
  @PageSize int = 5    
)
AS
BEGIN
	DECLARE @return INT, @query NVARCHAR(MAX) 
	DECLARE @start INT, @end INT      
	SET @start = (@PageNo - 1) * @PageSize + 1      
	SET @end = @PageNo * @PageSize
	CREATE table #tempData      
	(      
	ContentId INT,       
	ContentType INT,
	Title NVARCHAR(MAX) 
	)    
	INSERT INTO #tempData
	SELECT c.ContentId, c.ContentType,(CASE WHEN c.ContentType = 1 THEN (SELECT Title FROM CourseMaster WHERE Id = c.ContentId) 
	ELSE ((SELECT Title FROM ResourceMaster WHERE Id = c.ContentId))END) FROM CommunityCheckMaster c WHERE [Status] = 1 And UserId = @UserId 
IF EXISTS(SELECT TOP 1 1 FROM #tempData)
BEGIN
SET @query =      
	';with sqlpaging as (      
	SELECT       
	Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,      
	* FROM #tempData)      
      
	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(rownumber) from sqlpaging) as         
	 Totalrows    from sqlpaging        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
	END
	ELSE
	BEGIN
		RETURN 113
	END
END