ALTER PROCEDURE [dbo].[sps_ReportAbuseContent]           
AS          
BEGIN          
          
SET NOCOUNT ON;          
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;          
          
          
DECLARE @Return INT;          
          
    
Create table #tempReport    
(    
Id decimal ,    
Title NVARCHAR(500),    
ReportAbuseCount INT,    
Description NVARCHAR(500),    
ContentType INT,    
IsHidden BIT,    
Reason NVARCHAR(500), 
ReportReasons NVARCHAR(100), 
IsDeleted BIT,    
UpdateDate DATETIME ,
ContentId INT
    
)    
    
INSERT INTO #tempReport(    
Id,    
Title,    
ReportAbuseCount,    
Description,    
ContentType,    
IsHidden,    
Reason,
ReportReasons,
IsDeleted,    
UpdateDate,
ContentId
)    
SELECT ca.ID,cm.Title,cm.ReportAbuseCount,        
ca.comments as Description,        
1 as ContentType,ca.IsHidden,ca.Reason,ca.ReportReasons as ReportReasons,ca.IsHidden as IsDeleted,ca.UpdateDate,cm.id as ContentId  FROM        
CourseAbuseReports ca inner join CourseMaster cm on ca.courseid = cm.id   
and cm.ReportAbuseCount > 0      
-- WHERE ca.IsHidden <> 1         
union all          
SELECT rc.ID,rm.Title,rm.ReportAbuseCount,        
rc.Comments as Description,        
2 as ContentType,rc.IsHidden,rc.Reason,rc.ReportReasons as ReportReasons,rc.IsHidden as IsDeleted,rc.UpdateDate,rm.id as ContentId    FROM        
ResourceAbuseReports rc INNER join ResourceMaster rm on rc.ResourceId = rm.Id  
and rm.ReportAbuseCount > 0
--where        
--rc.IsHidden <>1         
        
          
union all         
        
select rc.Id as ID,        
rm.title,        
rc.ReportAbuseCount,        
rc.comments as Description,        
3 as ContentType,        
rc.IsHidden,rc.Reason,(select ReportReasons from ResourceCommentsAbuseReports where ResourceCommentId=rc.Id) as ReportReasons,rc.IsHidden as IsDeleted,rc.UpdateDate,rm.id as ContentId   from [dbo].[ResourceComments] rc        
 INNER join resourcemaster rm on rc.ResourceId = rm.id
 and rc.ReportAbuseCount > 0        
 --WHERE rc.IsHidden <> 1         
        
Union all        
        
        
select rc.Id as ID,        
rm.title,rc.ReportAbuseCount,rc.comments as Description,4 as ContentType,rc.IsHidden ,rc.Reason,(select ReportReasons from CourseCommentAbuseReports where CourseCommentId = rc.Id ) as ReportReasons,      
rc.IsHidden as IsDeleted  ,rc.UpdateDate ,rm.id as ContentId     
from [dbo].[CourseComments] rc INNER join coursemaster rm on rc.CourseID = rm.id        
 --WHERE --rc.IsHidden <> 1 AND      
AND rc.ReportAbuseCount > 0       
     
    
    
 SELECT * FROM #tempReport order by UpdateDate desc    
    
DROP table #tempReport    
 SET @Return = 105;          
          
 RETURN @Return;          
          
END;   