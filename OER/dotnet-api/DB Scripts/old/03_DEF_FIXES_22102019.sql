
    ALTER TABLE WhiteListingURLs
    ADD IsActive BIT
    DEFAULT 1 NOT NULL;
GO
/****** Object:  StoredProcedure [dbo].[CreateQRC]    Script Date: 10/22/2019 3:56:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
      
ALTER PROCEDURE [dbo].[CreateQRC]       
 @Name NVARCHAR(150),      
 @Description NVARCHAR(2000),      
 @CreatedBy INT,      
 @CategoryIds NVARCHAR(450)      
AS      
BEGIN      
Declare @return INT      
IF NOT EXISTS (SELECT * FROM QRCMaster WHERE Name=@Name)      
BEGIN       
      
CREATE TABLE #temp      
(      
Name nvarchar(50),      
Description nvarchar(1000),      
CreatedBy int,      
CatId int      
)      
      
INSERT INTO #temp (      
Name,      
Description,      
CreatedBy,      
CatId      
)      
SELECT       
@Name,      
@Description,      
@CreatedBy,      
value        
FROM StringSplit(@CategoryIds, ',')        
      
      
DECLARE @QRCID INT;      
  INSERT INTO QRCMaster (Name,Description,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active)      
  VALUES (@Name,@Description,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1)      
      
      
  SET @QRCID = SCOPE_IDENTITY();      
      
  print @QRCID      
  INSERT INTO QRCCategory(QRCId,CategoryId,CreatedOn)      
  SELECT @QRCID,CatId,GETDATE() FROM #temp;      
        
  -- do log entry here      
      
  SET @return =100 -- creation success      
END      
      
ELSE      
 BEGIN       
  SET @return = 105 -- Record exists      
 END      
      
 SELECT cm.Id,      
    cm.Name,Description,      
    cm.CreatedOn,          
    CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,      
    cm.UpdatedOn, cm.Active      
   from QRCMaster cm       
   inner join UserMaster c  on cm.CreatedBy= c.Id      
   inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc        
   RETURN @return      
END      
      
GO
/****** Object:  StoredProcedure [dbo].[GetCourseById]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
       
ALTER PROCEDURE [dbo].[GetCourseById]                   
(                  
@Id INT                  
)                  
AS                  
BEGIN                
              
DECLARE @SharedCount INT             
DECLARE @DownloadCount INT           
DECLARE @VisitCount INT  
DECLARE @IsApproved BIT  
              
IF Exists(select TOP 1 1 from CourseMaster WHERE Id=@Id)                   
 BEGIN                  
               
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 1)              
 SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 1)              
        
        
  SELECT @VisitCount = ViewCount,@IsApproved = IsApproved FROM CourseMaster WHERE ID = @Id        
         
 IF(@VisitCount IS NULL AND @IsApproved = 1)        
 BEGIN        
 Update CourseMaster SET ViewCount =  1 WHERE ID = @Id         
        
 END        
 ELSE IF(@IsApproved = 1)        
 BEGIN        
  Update CourseMaster SET ViewCount = @VisitCount+ 1 WHERE ID = @Id         
 END;        
        
        
 SELECT r.Id                  
      ,r.Title                  
      ,r.CategoryId, c.Name as CategoryName                  
      ,SubCategoryId, sc.Name as SubCategoryName                  
      ,Thumbnail                  
      ,CourseDescription                  
      ,Keywords                  
      ,CourseContent                       
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription                  
      ,IsDraft                  
   , EducationId, edm.Name as EducationName                  
   , ProfessionId, pm.Name as ProfessionName                  
      , um.FirstName+ ' '+ um.LastName as CreatedBy, um.Id as CreatedById                  
      ,r.CreatedOn                  
      ,IsApproved                  
      ,Rating                       
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,ReadingTime  ,              
   @SharedCount as SharedCount,            
   LastView,            
   r.LevelId,         
   ViewCount,        
   (SELECT [Level] FROM lu_Level WHERE Id = r.LevelId) AS [LevelName],            
   (SELECT Level_Ar FROM lu_Level WHERE Id = r.LevelId) AS [Level_Ar],            
   r.EducationalStandardId,            
   (SELECT [Standard] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard],            
   (SELECT [Standard_Ar] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard_Ar],            
   r.EducationalUseId,            
    (SELECT [EducationalUse] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUseName],            
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar] ,  
   CommunityBadge,  
   MoEBadge           
  FROM CourseMaster r                  
   inner join CategoryMaster c on r.CategoryId = c.Id                     
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                  
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                  
   left join EducationMaster edm on edm.Id= r.EducationId                  
   left join ProfessionMaster pm on pm.Id= r.ProfessionId                  
   inner join UserMaster um on r.CreatedBy =um.Id where r.Id=@Id Order by r.Id desc                   
                  
                  
   Update CourseMaster SET LastView = GETDATE() WHERE ID = @Id          
                  
            
        
        
SELECT caf.Id                  
      ,CourseId                  
      ,AssociatedFile                  
      ,caf.CreatedOn  ,      
  caf.FileName      
  FROM CourseAssociatedFiles caf INNER JOIN CourseMaster cm ON cm.Id=caf.CourseId  AND cm.Id=@Id             
  WHERE IsInclude = 1    
                  
                  
                  
  SELECT cur.Id                  
      ,CourseId                  
      ,cur.URLReferenceId,uwl.URL as URLReference         
   ,cur.CreatedOn,
   uwl.IsActive
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId                  
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId AND cm.Id=@Id    
     AND uwl.IsApproved = 1                   
         
                  
SELECT cc.Id                  
      ,[CourseId]                  
      ,[Comments]                  
      ,um.FirstName+' '+ um.LastName as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage                  
      ,[CommentDate]                  
      ,[ReportAbuseCount]                  
  FROM [dbo].[CourseComments] cc inner join UserMaster um on cc.UserId=um.Id AND cc.CourseId=@Id where cc.IsHidden=0                  
                    
                
 SELECT * from Coursesections WHERE CourseId = @Id                  
                  
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr                  
--INNER JOIN ResourceMaster rm                  
--ON sr.resourceid = rm.id                  
                  
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)             
          
             
  SELECT r.Id                  
      ,r.Title   ,          
   sr.SectionId           
      ,r.CategoryId, c.Name as CategoryName                  
      ,SubCategoryId, sc.Name as SubCategoryName                  
      ,Thumbnail                  
      ,ResourceDescription                  
      ,Keywords                  
      ,ResourceContent                  
      ,MaterialTypeId, mt.Name as MaterialTypeName                  
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription                  
      ,IsDraft                  
      ,um.FirstName+ ' ' + um.LastName as CreatedBy, um.Id as CreatedById                  
      ,r.CreatedOn                  
      ,IsApproved                  
      ,Rating                  
      ,AlignmentRating                  
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,                
   es.Standard,                
 eu.EducationalUse,                
 el.Level,                
 Objective,               
 @SharedCount as SharedCount,              
 LastView,              
 [Format]                    
  FROM           
  SectionResource sr                  
INNER JOIN ResourceMaster r                 
ON sr.resourceid = r.id                     
   inner join CategoryMaster c on r.CategoryId = c.Id                  
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id                  
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                  
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                  
    LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id                
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id                
   LEFT JOIN lu_Level el ON r.LevelId = el.Id                
                
   inner join UserMaster um on r.CreatedBy =um.Id           
  where sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)             
     
 SELECT Rating,COUNT(RatedBy) As NoOfUsers    
from [CourseRating] where courseid = @Id    
GROUP BY Rating    
  
SELECT r.Rating,COUNT(r.RatedBy) As NoOfUsers  ,r.ResourceId  
from [resourcerating] r where r.resourceid in( select resourceid from SectionResource where sectionid in (SELECT ID from Coursesections WHERE CourseId = @Id))  
GROUP BY Rating  ,r.ResourceId  
--select * from resourcerating   
        
 return 105 -- record exists                  
                  
 end                  
  ELSE                  
 return 102 -- reconrd does not exists                  
END             
GO
/****** Object:  StoredProcedure [dbo].[GetQRC]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
ALTER PROCEDURE [dbo].[GetQRC]    
AS  
BEGIN    
 IF Exists(select * from QRCMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,Description,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from QRCMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id  
   WHERE cm.Active = 1
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetQRCbyCategoryId]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- exec GetQRCbyCategoryId 1    
CREATE PROCEDURE [dbo].[GetQRCbyCategoryId]      
(    
@Id INT    
)    
AS      
BEGIN        
 IF Exists(select top 1 1 from QRCMaster where ID in (SELECT QRCId from QRCCategory WHERE CategoryID = @Id))       
 BEGIN       
  SELECT cm.Id,      
    cm.Name,Description,      
    cm.CreatedOn,          
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,      
    cm.UpdatedOn, cm.Active      
   from QRCMaster cm       
   inner join UserMaster c  on cm.CreatedBy= c.Id      
   inner join UserMaster l on cm.UpdatedBy =l.Id      
   WHERE cm.Active = 1    
   AND cm.Id in (SELECT QRCId from QRCCategory WHERE CategoryID = @Id)    
   order by cm.Id desc       
         
  RETURN 105 -- record exists      
 end      
  ELSE      
  RETURN 102 -- reconrd does not exists      
END      
GO
/****** Object:  StoredProcedure [dbo].[GetResourceById]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
        
  --exec GetResourceById 67            
ALTER PROCEDURE [dbo].[GetResourceById]                       
(                      
@Id INT                      
)                      
AS                      
BEGIN                       
                    
DECLARE @SharedCount INT                  
DECLARE @DownloadCount INT                  
DECLARE @IsRemix BIT                   
DECLARE @VisitCount INT      
DECLARE @IsApproved BIT  
SET @IsRemix = 0                      
                      
IF EXISTS(SELECT TOP 1 1 from ResourceRemixHistory WHERE ResourceRemixedID =@Id)                      
BEGIN                      
SET @IsRemix= 1                      
END                      
IF Exists(select TOP 1 1 from ResourceMaster WHERE Id=@Id)                       
 BEGIN                       
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 2)                   
  SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 2)                
                
  SELECT @VisitCount = ViewCount,@IsApproved =IsApproved FROM ResourceMaster WHERE ID = @Id           
               
 IF(@VisitCount IS NULL AND @IsApproved = 1)              
 BEGIN              
 Update ResourceMaster SET ViewCount =  1 WHERE ID = @Id               
              
 END              
              
 ELSE IF(@IsApproved = 1)        
 BEGIN                               
  Update ResourceMaster SET ViewCount = @VisitCount+ 1 WHERE ID = @Id               
 END;              
              
              
  SELECT r.Id                      
      ,r.Title                      
      ,r.CategoryId, c.Name as CategoryName                      
      ,SubCategoryId, sc.Name as SubCategoryName                      
      ,Thumbnail                      
      ,ResourceDescription                      
      ,Keywords                      
      ,ResourceContent                      
      ,MaterialTypeId, mt.Name as MaterialTypeName                      
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription,  
   cm.Media,  
   cm.Protected  
      ,IsDraft                      
      ,um.FirstName+' '+ um.LastName as CreatedBy, um.Id as CreatedById                      
      ,r.CreatedOn                      
      ,IsApproved                      
      ,Rating                      
      ,AlignmentRating                      
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,                    
   es.Standard,                          
 Objective,                   
 @SharedCount as SharedCount,                  
 CAST(LastView as DATETIME) as LastView,                  
 [Format]                    
   ,@IsRemix AS IsRemix ,            
   LevelId ,            
      (SELECT [Level] FROM lu_Level WHERE Id = r.LevelId) AS [LevelName],                  
   (SELECT Level_Ar FROM lu_Level WHERE Id = r.LevelId) AS [Level_Ar],                  
   r.EducationalStandardId,                  
   (SELECT [Standard] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard],                  
   (SELECT [Standard_Ar] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard_Ar],                  
   r.EducationalUseId,                  
    (SELECT [EducationalUse] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUseName],                  
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar],  
   CommunityBadge,  
   MoEBadge             
  FROM ResourceMaster r                      
   inner join CategoryMaster c on r.CategoryId = c.Id                      
   LEFT join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id                      
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                      
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                      
    LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id                    
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id                    
   LEFT JOIN lu_Level el ON r.LevelId = el.Id                    
                    
   inner join UserMaster um on r.CreatedBy =um.Id where r.Id=@Id Order by r.Id desc                       
    Update ResourceMaster SET LastView = GETDATE() WHERE ID = @Id                  
                      
SELECT raf.Id                      
      ,ResourceId                      
      ,AssociatedFile                      
      ,UploadedDate,          
   FileName          
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId AND r.Id=@Id     
  WHERE IsInclude = 1    
                      
                      
                      
   SELECT rur.Id                      
      ,ResourceId                      
      ,URLReferenceId, uwl.URL as URLReference                      
      ,rur.CreatedOn,
	  uwl.IsActive
  FROM dbo.ResourceURLReferences rur                       
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId                 
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId AND r.Id=@Id   
  AND uwl.IsApproved = 1                     
                      
   SELECT rc.Id                      
      ,[ResourceId]                      
      ,[Comments]                      
      ,um.FirstName+ ' '+ um.LastName as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage                      
      ,[CommentDate]                      
      ,[ReportAbuseCount]                      
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id AND rc.ResourceId=@Id where rc.IsHidden=0                    
      
SELECT Rating,COUNT(RatedBy) As NoOfUsers    
from [ResourceRating] where resourceid = @Id    
GROUP BY Rating                   
 return 105 -- record exists                      
                      
 end                      
  ELSE                      
  return 102 -- reconrd does not exists                      
END     
GO
/****** Object:  StoredProcedure [dbo].[GetWhitelistingRequests]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


    
ALTER PROCEDURE [dbo].[GetWhitelistingRequests]     
  (  
  @IsApproved BIT  
  )  
AS    
BEGIN      
      
 SELECT wlu.Id,        
    CONCAT(c.FirstName, '', c.LastName) as RequestedBy,    
    CONCAT(l.FirstName, '', l.LastName) as VerifiedBy    
      ,URL    
      ,IsApproved    
      ,RequestedOn    
      ,VerifiedOn    
      ,RejectedReason    
  FROM WhiteListingURLs wlu     
   inner join UserMaster c  on wlu.RequestedBy= c.Id    
   left join UserMaster l on wlu.VerifiedBy =l.Id where IsApproved=@IsApproved AND IsActive =1 order by wlu.Id desc      
      
    
  IF @@ROWCOUNT >0    
  return 105 -- record exists    
      
  ELSE    
  return 102 -- record does not exists    
END    
GO
/****** Object:  StoredProcedure [dbo].[spd_UnAssignedQRC]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--spd_UnAssignedQRC 1
CREATE PROCEDURE [dbo].[spd_UnAssignedQRC]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  



if exists(select top 1 1 from QRCUserMapping where qrcid = @Id and Active = 1)
begin 
 SET @return =104 -- record DELETED   
end

else
begin 
Update QRCMaster SET Active = 0 WHERE Id = @Id
 SET @return =103 -- record DELETED   

end

RETURN @return  
    
END  
GO
/****** Object:  StoredProcedure [dbo].[spd_WhiteListingURLs]    Script Date: 10/22/2019 3:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  exec spd_WhiteListingURLs 1
CREATE PROCEDURE [dbo].[spd_WhiteListingURLs]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
  IF EXISTS (SELECT TOP 1 1 FROM WhiteListingURLs  WHERE Id=@Id)  
  BEGIN  
    
   Update WhiteListingURLs SET IsActive = 0 WHERE Id = @Id    
     SET @return =103 -- record DELETED       
  END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    RETURN @return  
    
END  
GO
