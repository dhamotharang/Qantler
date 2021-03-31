ALTER PROCEDURE [dbo].[sps_GetSensoryCheckList] --3    
(      
  @UserId INT,      
  @PageNo int = 1,            
  @PageSize int = 5 ,
  @CategoryId INT = NULL
)      
AS      
BEGIN        
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT           
DECLARE @start INT, @end INT            
SET @start = (@PageNo - 1) * @PageSize + 1            
SET @end = @PageNo * @PageSize      
CREATE table #tempData            
(            
ContentId INT,             
ContentType INT,    
Title NVARCHAR(MAX),    
Category NVARCHAR(400)    
)   

if(@CategoryId =0)
begin
INSERT INTO #tempData      
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
FROM CourseMaster c WHERE  c.IsDraft = 0   
AND c.Id  IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 1 AND Userid = @UserId AND Status=1)  
UNION       
SELECT Id, 2, Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
 FROM ResourceMaster c WHERE  IsDraft = 0      
 AND c.Id IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 2 AND Userid = @UserId AND Status=1)  
end
else
begin
INSERT INTO #tempData      
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
FROM CourseMaster c WHERE c.IsApproved IS NULL AND c.IsDraft = 0   
AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 1 AND Userid = @UserId AND IsCurrent = 1)  
AND c.CreatedBy <> @UserId  AND c.CategoryId= @CategoryId
--AND CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))      
UNION       
SELECT Id, 2, Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
 FROM ResourceMaster c WHERE c.IsApproved IS NULL AND IsDraft = 0      
 AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 2 AND Userid = @UserId AND IsCurrent = 1)  
 AND c.CreatedBy <> @UserId    AND c.CategoryId= @CategoryId    
--AND CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))      
end     
    
        
      
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
