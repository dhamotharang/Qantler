
GO
/****** Object:  StoredProcedure [dbo].[GetCourseById]    Script Date: 10/25/2019 12:06:11 AM ******/
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
      ,r.CategoryId, c.Name as CategoryName ,c.Name_Ar as CategoryName_Ar                 
      ,SubCategoryId, sc.Name as SubCategoryName  ,sc.Name_Ar as SubCategoryName_Ar                
      ,Thumbnail                  
      ,CourseDescription                  
      ,Keywords                  
      ,CourseContent                       
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Title_Ar as CopyrightTitle_Ar, cm.Description as CopyrightDescription                  
      ,IsDraft                  
   , EducationId, edm.Name as EducationName ,edm.Name_Ar as EducationName_Ar            
   , ProfessionId, pm.Name as ProfessionName ,pm.Name_Ar as ProfessionName_Ar                 
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
      ,cur.CreatedOn                  
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
      ,r.CategoryId, c.Name as CategoryName  ,c.Name_Ar as CategoryName_Ar                 
      ,SubCategoryId, sc.Name as SubCategoryName   ,sc.Name_Ar as SubCategoryName_Ar                 
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
/****** Object:  StoredProcedure [dbo].[GetResourceById]    Script Date: 10/25/2019 12:06:12 AM ******/
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
      ,SubCategoryId, sc.Name as SubCategoryName,
	   c.Name_Ar as CategoryName_Ar,
	   sc.Name_Ar as SubCategoryName_Ar
      ,Thumbnail                      
      ,ResourceDescription                      
      ,Keywords                      
      ,ResourceContent                      
      ,MaterialTypeId, mt.Name as MaterialTypeName ,
	  mt.Name_Ar as MaterialTypeName_Ar
      ,CopyRightId, cm.Title as CopyrightTitle, 
	  cm.Title_Ar as CopyrightTitle_Ar
	  ,cm.Description as CopyrightDescription,  
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
      ,rur.CreatedOn                      
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
/****** Object:  StoredProcedure [dbo].[sps_RemixPreviousVersion]    Script Date: 10/25/2019 12:06:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sps_RemixPreviousVersion]
	(
	@ResourceRemixedID INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Id INT;
	SELECT TOP 1 @Id = ResourceSourceId from ResourceRemixHistory WHERE ResourceRemixedID =@ResourceRemixedID order by [version]

	exec GetResourceById  @Id

	IF(@Id IS NOT NULL)
	RETURN 105
	ELSE
	RETURN 102
END

GO
