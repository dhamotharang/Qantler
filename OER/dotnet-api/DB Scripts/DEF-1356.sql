
ALTER PROCEDURE [dbo].[DeleteCategory]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT  
  IF EXISTS (SELECT * FROM CategoryMaster  WHERE Id=@Id and Active = 1 and Status = 1)  
  BEGIN  
   SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM ResourceMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM SubCategoryMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM QRCCategory  WHERE CategoryId=@Id) +  
   (SELECT COUNT(*) FROM QRCMasterCategory  WHERE CategoryId=@Id)+  
   (select count(X.id) from(SELECT id ,value FROM UserMaster       CROSS APPLY STRING_SPLIT(subjectsinterested, ','))X      where X.value=@Id); --(select id from CategoryMaster where Id=@Id));  
   IF(@catCount > 0)  
    SET @return = 121  
   ELSE 
   BEGIN  
    --DELETE FROM CategoryMaster  WHERE Id=@Id and Active=0;  
    update CategoryMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
  END  

  ELSE IF EXISTS (SELECT * FROM CategoryMaster  WHERE Id=@Id and Active = 0 and Status = 1)  
  BEGIN 
  update CategoryMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END    
  ELSE  
  BEGIN  
   SET @return = 102 -- reconrd does not exists  
  END  
  SELECT cm.Id,  
    cm.Name,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from CategoryMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status = 1  
    order by cm.Id desc    
    RETURN @return  
    
END  

GO
ALTER PROCEDURE [dbo].[sps_GetCommunityCategories]   
(    
  @UserId INT,    
  @PageNo int = 1,          
  @PageSize int = 5         
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
CategoryId INT
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,','))and Active = 1)    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,','))and Active = 1)    
AND c.CreatedBy <> @UserId       
    
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
SELECT distinct cm.id,cm.Name,cm.Name_Ar from #tempData t INNER JOIN CategoryMaster cm
on t.CategoryId = cm.Id and cm.Active=1

          
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
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId
UNION   
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1 And MoEBadge<>1 and CommunityBadge<>1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
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
AND c.CategoryId IN (SELECT Id from  CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1  And MoEBadge<>1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
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

Go

    
ALTER PROCEDURE [dbo].[sps_GetMoECheckList]    
(    
  @UserId INT,    
  @PageNo int = 1,          
  @PageSize int = 5 ,
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
Category NVARCHAR(400)    
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND (IsApprovedSensory = 1 or CommunityBadge = 1)and c.MoEBadge <> 1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId    AND c.CategoryId =  @CategoryId   
UNION     
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND (IsApprovedSensory = 1 or CommunityBadge = 1) and c.MoEBadge <> 1)) AND  c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Id] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))			   
AND c.CreatedBy <> @UserId  AND  c.CategoryId =  @CategoryId      
    
    
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

ALTER PROCEDURE [dbo].[sps_GetUserNotInQRC]  
(  
@QrcId int = NULL,  
@Category Int = NULL,  
@PageNo int = 1,  
@PageSize int = 5  ,
@FilterCategoryId INT = NULL
)  
  
AS    
BEGIN    
SET ANSI_NULLS ON
Declare @return INT  
--select top 30 ID, FirstName + ' ' + LastName as UserName  
--,10 as ResourceContributed  
--,0 as CourseCreated  
--,5 as CurrentQRCS  
--,9 as NoOfReviews from UserMaster where  IsContributor = 1 AND FirstName <> '' AND LastName <>''  

  DECLARE @query NVARCHAR(MAX)  
  
  
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  


--IF (@FilterCategoryId IS NOT NULL)
--BEGIN
--  --Select 'test'
--END

--ELSE

--BEGIN
---- Select 'test2'
--END
IF (@FilterCategoryId IS  NULL  OR @FilterCategoryId = 0 )
BEGIN
SET @query = 
';with sqlpaging as (  
SELECT   
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,  
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,  
u.Email,  
u.Photo  
,COUNT(rm.Id)  as ResourceContributed,  
COUNT(c.Id) as CourseCreated  
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS  
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
,SubjectsInterested 
 from   
 UserMaster u 
LEFT JOIN ResourceMaster rm   
ON u.id = rm.CreatedBy    
LEFT JOIN CourseMaster c  
ON u.id = c.CreatedBy   
WHERE  
u.id not in (select userid from qrcusermapping where active=1 and CategoryId='+CAST(@Category AS VARCHAR(50))+') AND u.IsContributor = 1
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)  
  
  
select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''
END
ELSE
BEGIN
   SET @query = 
';with sqlpaging as (  
SELECT 
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,  
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,  
u.Email,  
u.Photo  
,COUNT(rm.Id)  as ResourceContributed,  
COUNT(c.Id) as CourseCreated  
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS   
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
,SubjectsInterested 
 from   
 UserMaster u  LEFT JOIN ResourceMaster rm   
ON u.id = rm.CreatedBy  
LEFT JOIN CourseMaster c  
ON u.id = c.CreatedBy   
WHERE  
u.id not in (select userid from qrcusermapping where active=1 AND CategoryId='+CAST(@Category AS VARCHAR(50))+') and u.IsContributor = 1
AND CHARINDEX((SELECT distinct Value FROM STRING_SPLIT(SubjectsInterested,'','') WHERE Value IN 
(SELECT Id from CAtegoryMaster WHERE Status=1 and Active=1  and Id =  '+CAST(@FilterCategoryId AS VARCHAR(50))+')),SubjectsInterested) > 0
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)  
  
select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))

END

EXEC sp_executesql @query
    
 SET @return = 105 -- reconrd does not exists  
   RETURN @return  
END   

GO
 
ALTER     PROCEDURE [dbo].[sps_RecommendedContent] --21 1 24      
(      
@UserId INT = NULL,    
@PageNo int = 1,    
@PageSize int = 5    
)      
AS      
BEGIN      
      
SET NOCOUNT ON;      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;      
      
      
  
      
DECLARE @Keyword NVARCHAR(100)      
DECLARE @Return INT      
DECLARE @totalRecords INT;    
DECLARE @query NVARCHAR(MAX)    
    
    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
     
 IF(@UserId IS NOT NULL)    
 BEGIN    
   DECLARE @TempTable TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX) )    
   INSERT INTO @TempTable    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id  
   LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    r.Keywords Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar  , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media  
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
    LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id   
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    c.Keywords Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%')) AND    
    c.IsApproved = 1    
  
    IF NOT EXISTS(SELECT value from  StringSplit((select subjectsinterested      
    from usermaster where id = @UserId and subjectsinterested <>''),','))    
   BEGIN    
      
     
    
   SET @QUery = '    
   DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
    on      
     r.CopyRightId = cpm.id      
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id     
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CreatedOn,CreatedOn, CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
    from @Table    
    )    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc '     
    
  
   END    
   ELSE    
   BEGIN    
   IF EXISTS(SELECT TOP 1 1 FROM @TempTable)  
   BEGIN  
    SET @QUery =     
    'DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id   
 
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    r.Keywords Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%'')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media       
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
 
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id          
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    c.Keywords Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%'')) AND    
    c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY ID desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_Ar ,  media     
    from @Table)    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc'    
    
    --print @QUery  
   END  
   ELSE  
   BEGIN  
     SET @QUery = '    
     DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
     INSERT INTO @Table    
     SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn    
      from resourcemaster r LEFT JOIN CategoryMaster cm      
     on      
     r.categoryid = cm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id     
     LEFT JOIN SubCategoryMaster sbm      
     ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
     UNION ALL    
     SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn     
      from coursemaster c LEFT JOIN CategoryMaster cm      
     on  
     c.categoryid = cm.id      
     LEFT JOIN SubCategoryMaster sbm      
     ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
     ;with sqlpaging as (    
     SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
      from @Table    
      )    
     select    
      top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
      (select max(rownumber) from sqlpaging) as     
      Totalrows    
     from sqlpaging    
     where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
     Order by Id desc '     
   END  
    --select count(*) as testingcount from @TABLE  
   END    
   END    
ELSE    
BEGIN    
  SET @QUery = '    
DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX), CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
INSERT INTO @Table    
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, r.CreatedOn,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media      
 from resourcemaster r LEFT JOIN CategoryMaster cm      
on      
r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
LEFT JOIN SubCategoryMaster sbm      
ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
UNION ALL    
SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail, c.CreatedOn ,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
 from coursemaster c LEFT JOIN CategoryMaster cm      
on      
c.categoryid = cm.id
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id        
LEFT JOIN SubCategoryMaster sbm      
ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
;with sqlpaging as (    
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
 from @Table    
 )    
select    
 top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
Order by Id desc '      
END    

--return @Query;

EXEC sp_executesql @Query       
-- WHERE c.Keywords Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR cm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR sbm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      --drop table @TempTable
 SET @Return = 105;      
      
 RETURN @Return;      
      
END;     
    
    
GO
 
ALTER     PROCEDURE [dbo].[sps_RecommendedContent_test] --21 1 24      
(      
@UserId INT = NULL,    
@PageNo int = 1,    
@PageSize int = 5    
)      
AS      
BEGIN      
      
SET NOCOUNT ON;      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;      
      
      
  
      
DECLARE @Keyword NVARCHAR(100)      
DECLARE @Return INT      
DECLARE @totalRecords INT;    
DECLARE @query NVARCHAR(MAX)    
    
    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
     
 IF(@UserId IS NOT NULL)    
 BEGIN    
   DECLARE @TempTable TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX) )    
   INSERT INTO @TempTable    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id  
   LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    r.Keywords Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar  , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media  
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
    LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id   
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    c.Keywords Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+'%')) AND    
    c.IsApproved = 1    
  
    IF NOT EXISTS(SELECT value from  StringSplit((select subjectsinterested      
    from usermaster where id = @UserId and subjectsinterested <>''),','))    
   BEGIN    
      
     
    
   SET @QUery = '    
   DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
    on      
     r.CopyRightId = cpm.id      
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id     
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CreatedOn,CreatedOn, CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
    from @Table    
    )    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc '     
    
  
   END    
   ELSE    
   BEGIN    
   IF EXISTS(SELECT TOP 1 1 FROM @TempTable)  
   BEGIN  
    SET @QUery =     
    'DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id  
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    r.Keywords Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%'')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media       
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id 
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id          
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    c.Keywords Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST((select name from CategoryMaster where id=value) AS NVARCHAR(100))+''%'')) AND    
    c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY ID desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_Ar ,  media     
    from @Table)    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc'    
    
    --print @QUery  
   END  
   ELSE  
   BEGIN  
     SET @QUery = '    
     DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
     INSERT INTO @Table    
     SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn    
      from resourcemaster r LEFT JOIN CategoryMaster cm      
     on      
     r.categoryid = cm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id     
     LEFT JOIN SubCategoryMaster sbm      
     ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
     UNION ALL    
     SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn     
      from coursemaster c LEFT JOIN CategoryMaster cm      
     on  
     c.categoryid = cm.id      
     LEFT JOIN SubCategoryMaster sbm      
     ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
     ;with sqlpaging as (    
     SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
      from @Table    
      )    
     select    
      top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
      (select max(rownumber) from sqlpaging) as     
      Totalrows    
     from sqlpaging    
     where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
     Order by Id desc '     
   END  
    --select count(*) as testingcount from @TABLE  
   END    
   END    
ELSE    
BEGIN    
  SET @QUery = '    
DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX), CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
INSERT INTO @Table    
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, r.CreatedOn,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media      
 from resourcemaster r LEFT JOIN CategoryMaster cm      
on      
r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
LEFT JOIN SubCategoryMaster sbm      
ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
UNION ALL    
SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail, c.CreatedOn ,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
 from coursemaster c LEFT JOIN CategoryMaster cm      
on      
c.categoryid = cm.id
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id        
LEFT JOIN SubCategoryMaster sbm      
ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
;with sqlpaging as (    
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
 from @Table    
 )    
select    
 top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
Order by Id desc '      
END    

--return @Query;

EXEC sp_executesql @Query       
-- WHERE c.Keywords Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR cm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR sbm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      --drop table @TempTable
 SET @Return = 105;      
      
 RETURN @Return;      
      
END;     
    
    


