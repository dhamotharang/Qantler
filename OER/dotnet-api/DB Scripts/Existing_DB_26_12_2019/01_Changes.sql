/****** Object:  StoredProcedure [sps_GetCommunityCheckList]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  -- sps_GetCommunityCheckList   2,1,10,1
ALTER PROCEDURE [sps_GetCommunityCheckList]  
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
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId
UNION   
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0  
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
GO
/****** Object:  StoredProcedure [sps_GetSensoryCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
ALTER PROCEDURE [sps_GetSensoryCategory]  -- 2
 (  
 @UserId INT  
 )  
AS  
BEGIN  

SET NOCOUNT ON;
 SELECT cm.Id, cm.Name,cm.Name_Ar FROM CategoryMaster cm INNER JOIN
 CourseMaster c on cm.Id = c.CategoryId WHERE c.IsApproved IS NULL AND c.IsDraft = 0     
AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 1 AND Userid = @UserId AND IsCurrent = 1)    
AND c.CreatedBy <> @UserId AND cm.Active = 1 AND cm.Status = 1   
UNION         
SELECT cm.Id, cm.Name,cm.Name_Ar FROM CategoryMaster cm INNER JOIN    
  ResourceMaster c on cm.Id = c.CategoryId WHERE c.IsApproved IS NULL AND IsDraft = 0        
 AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 2 AND Userid = @UserId AND IsCurrent = 1)    
 AND c.CreatedBy <>@UserId AND cm.Active = 1 AND cm.Status = 1

  RETURN 105
  
END
GO
/****** Object:  StoredProcedure [sps_OerDashboardReport]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*___________________________________________________________________________________________________      
      
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved      
WorldWide.      
      
$Revision:  $1.0      
$Author:    $ Prince Kumar    
&Date       June 06, 2019    
    
Ticket: Ticket URL    
      
PURPOSE:       
This store procedure will get the reports of top contirbutors ,reviever, and other dashboard data    
    
EXEC sps_OerDashboardReport     
___________________________________________________________________________________________________*/    
ALTER PROCEDURE [sps_OerDashboardReport]     
AS    
BEGIN    
    
SET NOCOUNT ON;    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
DECLARE @Contributors INT,    
  @Courses INT,    
  @Resources INT,    
  @Return INT,  
  @TotalVisit INT;    
    
SET @Contributors =(SELECT count(*) from usermaster WHERE  IsContributor = 1)    
SET @Courses = (SELECT count(*)  from coursemaster WHERE IsApproved = 1 )    
SET @Resources =(SELECT count(*)  from resourcemaster WHERE IsApproved = 1)    
SET @TotalVisit= (SELECT  Count(ID) AS TotalVisit FROM Visiters)  
    
SELECT @Contributors as Contributors ,@Courses as Courses,@Resources as Resources ,@TotalVisit as  TotalVisit  
    
;WITH TopContributor_with(ID, UserName, CourseCount,Photo) AS    
(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u    
LEFT JOIN coursemaster c    
on u.id = c.createdby    
    
 WHERE  u.IsContributor = 1 AND c.isApproved =1     
    
 group by u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP 6 ID, UserName, CourseCount,Photo from  TopContributor_with where CourseCount>0  ORDER BY 3 DESC;    
    
    
     
WITH TopReviewer_with(ID, UserName, CourseCount,Photo) as(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u    
LEFT JOIN ContentApproval c    
on u.id = c.assignedto    
    
 GROUP BY u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP  6 ID, UserName, CourseCount,Photo from  TopReviewer_with where CourseCount>0  ORDER BY 3 DESC;    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END;    
GO
/****** Object:  StoredProcedure [sps_QRCByUserID]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
  
-- sps_QRCByUserID 2  
ALTER PROCEDURE [sps_QRCByUserID]   
 (  
 @UserID INT  
 )  
AS  
BEGIN 




   
 SELECT   
 qm.Id,  
qm.Name,  
Description,  
CAST(qm.CreatedBy AS nvarchar(50)) as CreatedBy,  
qm.CreatedOn,  
CAST(qm.UpdatedBy AS nvarchar(50)) as UpdatedBy,  
qm.UpdatedOn,  
qm.Active,  
qmc.Id As CategoryId,  
qmc.Name AS CategoryName,  
qmc.Name_Ar AS CategoryNameAr  
   
  FROM QRCMaster qm  
  INNER JOIN QRCCategory qc  
  ON qm.Id = qc.QRCId  
  INNER JOIN CategoryMaster qmc  
  ON qmc.Id = qc.CategoryId  
   WHERE qm.ID in ( SELECT QRCId FROM QRCUserMapping WHERE UserId = @UserID AND Active = 1) 
   AND qm.Active =1 AND qmc.Status = 1
   AND qmc.id in (
    SELECT DISTINCT cm.CategoryId FROM CourseMaster cm  
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = 2 AND ca.ContentType = 1  
 AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1  
 UNION   
 SELECT DISTINCT cm.CategoryId  FROM ResourceMaster cm  
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = 2 AND ca.ContentType = 2  
 AND  ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1 
   )
 RETURN 105  
END
GO
