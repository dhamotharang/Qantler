
ALTER PROCEDURE [dbo].[sps_GetCommunityCheckList]  
(  
  @UserId INT,  
  @PageNo int = 1,        
  @PageSize int = 5,
  @CategoryId INT = NULL
)  
AS  
BEGIN  
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)  
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT       
DECLARE @start INT, @end INT        
SET @start = (@PageNo - 1) * @PageSize + 1        
SET @end = @PageNo * @PageSize  
CREATE table #tempData        
(        
ContentId INT,         
ContentType INT,  
Title NVARCHAR(MAX),  
Category NVARCHAR(400),
CategoryId INT
)      
INSERT INTO #tempData  
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1 And MoEBadge<>1 and CommunityBadge<>1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId
UNION   
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1 And MoEBadge<>1 and CommunityBadge<>1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId   
  
DECLARE @CountsRejected INT = 0, @CountApproved INT = 0  
SELECT @CountsRejected = RejectCount, @CountApproved = ApproveCount FROM CommunityApproveRejectCount  
  
IF EXISTS(SELECT TOP 1 1 FROM #tempData)  
BEGIN  
DECLARE temp_cursor CURSOR FOR       
SELECT ContentId,ContentType      
FROM #tempData    
    
    
OPEN temp_cursor      
    
FETCH NEXT FROM temp_cursor       
INTO @ContentId,@ContentType      
    
    
WHILE @@FETCH_STATUS = 0      
BEGIN      
 IF EXISTS (SELECT TOP 1 1 FROM CommunityCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND UserId = @UserID )  
 BEGIN  
 DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 END     
 --IF EXISTS (SELECT TOP 1 1 FROM SensoryCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)  
 --BEGIN  
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 --END   
   
 --IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)  
 --BEGIN  
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 --END      
FETCH NEXT FROM temp_cursor       
INTO @ContentId,@ContentType          
     
END       
CLOSE temp_cursor;      
DEALLOCATE temp_cursor  
  
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
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+'' --and sqlpaging.CategoryId ='+CAST(@CategoryId AS INT)+''      
        
 EXEC sp_executesql @query        
   RETURN 105;    
   drop table #tempData   
 END  
 ELSE  
 BEGIN  
  RETURN 113  
 END  
END  
ELSE  
BEGIN  
 RETURN 113  
END  
END  
