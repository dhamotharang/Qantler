
ALTER PROCEDURE [dbo].[sps_GetMoEcategories]  
(    
  @UserId INT        
)    
AS    
BEGIN    
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)    
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT         
 
CREATE table #tempData          
(          
ContentId INT,           
ContentType INT,    
Title NVARCHAR(MAX),    
CategoryId INT   
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1  And MoEBadge<>1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id from  CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1  And MoEBadge<>1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
    
    
IF EXISTS(SELECT TOP 1 1 FROM #tempData)    
BEGIN    
SELECT distinct cm.id,cm.Name,cm.Name_Ar from #tempData t INNER JOIN CategoryMaster cm
on t.CategoryId = cm.Id and cm.Active = 1
   RETURN 105;      
   drop table #tempData     
 END    
 ELSE    
 BEGIN    
  RETURN 113    
 END
END