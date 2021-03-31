ALTER PROCEDURE [dbo].[CreateCourse]         
(        
     @Title NVARCHAR(250),        
           @CategoryId INT,        
           @SubCategoryId INT=NULL,        
           @Thumbnail  NVARCHAR(MAX)=NULL,        
           @CourseDescription  NVARCHAR(2000)=NULL,        
           @Keywords  NVARCHAR(1500)=NULL,        
           @CourseContent NTEXT=NULL,                  
           @CopyRightId INT=NULL,        
           @IsDraft BIT=NULL,        
     @CreatedBy INT=NULL,        
     @EducationId int= NULL,        
           @ProfessionId int=NULL,          
   @References NVARCHAR(MAX)=null,          
   @CourseFiles NVARCHAR(MAX)=null,      
   @ReadingTime INT = NULL,        
   @UT_Sections [UT_Sections] Readonly,        
   @UT_Resource [UT_Resource] Readonly,      
   @LevelId INT = NULL,      
   @EducationalStandardId INT = NULL,      
   @EducationalUseId INT = NULL      
     )        
AS        
BEGIN        
        
DECLARE @Id INT        
Declare @return INT        
DECLARE @SectionsCount INT;    
DECLARE @OutCourseId INT   
DECLARE @MessageTypeId Int;   
        
INSERT INTO [dbo].[CourseMaster]        
           (Title        
           ,CategoryId        
           ,SubCategoryId        
           ,Thumbnail        
           ,CourseDescription        
           ,Keywords        
           ,CourseContent        
           ,CopyRightId        
           ,IsDraft        
           ,CreatedBy         
           ,EducationId        
           ,ProfessionId        
   ,CreatedOn,        
   ReadingTime,      
   LevelId,      
   EducationalStandardId,      
   EducationalUseId,      
   IsApproved
   )        
     VALUES        
           (@Title,        
           @CategoryId,        
           @SubCategoryId,        
           @Thumbnail,         
           @CourseDescription,         
           @Keywords,         
           @CourseContent,                
           @CopyRightId,        
           @IsDraft,         
     @CreatedBy,        
           @EducationId,        
           @ProfessionId,        
     GETDATE(),        
     @ReadingTime,      
     @LevelId ,      
     @EducationalStandardId ,      
     @EducationalUseId,
	 NULL
     )        
        
 SET @Id=SCOPE_IDENTITY();    
  SET @OutCourseId =      @Id  
  
  
        
 IF @Id>0        
 BEGIN      
   
 SET @return = 100 -- creation success        
         
IF(@IsDraft = 0)  
BEGIN         
DECLARE @TotalCount INT;        
DECLARE @QrcID INT;        
DECLARE @RecordId INT;        
DECLARE @SectionId INT;        
DECLARE @i INT; 

SET @i = 1;        
   
   Declare @Date Datetime
   SET @Date = getdate() 
   SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Approval'
   --SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
--exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@CreatedBy,NULL,NULL,NULL
--exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
        
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 and QRCId in(select id from QRCMaster where active=1)--order by CreatedOn asc        
        
IF(@TotalCount>0)        
BEGIN        
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 and QRCId in(select id from QRCMaster where active=1)  
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
   order by CreatedOn asc        
        
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId        
        
   IF EXISTS(        
   SELECT         
     TOP 1 1        
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)        
     BEGIN        
       INSERT INTO ContentApproval(ContentId,        
       ContentType,        
       CreatedBy,        
       CreatedOn,        
       AssignedTo,        
       AssignedDate,  
    QrcId)        
        
       SELECT         
 @Id,        
       1, -- Course        
       @CreatedBy,        
       GETDATE(),        
       userid,        
       GETDATE() ,  
    @QrcID  
       from QRCusermapping  where QRCID =@QrcID and active = 1   
    and QRCusermapping.CategoryId = @CategoryId     
    UPDATE CourseMaster SET IsDraft = 0 WHERE Id = @Id      
      END        
END        
IF(@TotalCount=1)          
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END    
  
--IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 1)  
-- BEGIN  
-- UPDATE CourseMaster SET IsDraft = 1 WHERE Id = @Id  
-- END    
END        
SET @SectionsCount = (SELECT Count(*) FROM @UT_Sections)        
        
WHILE @i <= @SectionsCount        
        
BEGIN        
        
INSERT INTO CourseSections        
(        
Name,        
CourseId        
)        
        
SELECT Name,@Id FROM @UT_Sections WHERE Id = @i        
        
SET @SectionId = SCOPE_IDENTITY();        
        
INSERT INTO [dbo].[SectionResource](ResourceId,        
SectionId)        
SELECT         
ResourceId,        
@SectionId        
FROM @UT_Resource WHERE SectionId in (SELECT distinct  ID FROM @UT_Sections WHERE Id = @i)        
        
SET @i= @i+1;        
END;        
        
--IF (@ResourcesIds <> NULL)        
--BEGIN        
--CREATE TABLE #temp        
--(        
--CourseId int,        
--ResourceId int        
--)        
--INSERT INTO #temp (        
--CourseId,        
--ResourceId        
--)        
--SELECT         
--@Id,        
--value      
--FROM StringSplit(@ResourcesIds, ',')          
        
--DECLARE @FirstResourceId INT;        
--SET @FirstResourceId =(SELECT top 1 ResourceID FROM #temp)        
--IF(@FirstResourceId>0)        
--BEGIN        
        
--IF NOT EXISTS(SELECT TOP 1 1 FROM CourseResourceMapping WHERE CourseID =@Id )        
--   INSERT INTO CourseResourceMapping(CourseId,ResourcesId)        
--   SELECT CourseId,ResourceId FROM #temp        
        
--END;        
--END;        
 IF @References IS NOT NULL        
 BEGIN        
 -- INSERT Resource URL References FROM JSON        
         
  INSERT INTO CourseURLReferences        
            
  SELECT @Id,URLReferenceId,GETDATE() FROM          
   OPENJSON ( @References )          
  WITH (           
URLReferenceId   int '$.URLReferenceId'         
  )         
        
 END        
        
 IF @CourseFiles IS NOT NULL        
 BEGIN        
 -- INSERT Resource Associated Files FROM JSON        
         
  INSERT INTO CourseAssociatedFiles        
            
  SELECT @Id,AssociatedFile,GETDATE(),FileName,1 FROM          
   OPENJSON ( @CourseFiles )          
  WITH (           
              AssociatedFile   varchar(200) '$.AssociatedFile' ,    
     FileName   nvarchar(200) '$.FileName'     
  )         
        
 END        
  
        
        
  SELECT Id,        
TitleId,        
FirstName +' ' +LastName as UserName,        
Email FROM UserMaster WHERE ID in (        
 SELECT         
userid        
from QRCusermapping  where QRCID = @QrcID and active = 1) AND         
IsEmailNotification = 1 

DECLARE @QRCUsers TABLE(ID INT IDENTITY(1,1),UserId INT)
INSERT INTO @QRCUsers  
SELECT Id FROM UserMaster WHERE ID in (SELECT userid from QRCusermapping  where QRCID = @QrcID and active = 1) AND IsEmailNotification = 1 
DECLARE @TotalRows INT
DECLARE @ict INT = 1
SELECT @TotalRows = COUNT(*) FROM @QRCUsers
WHILE @ict <= @TotalRows
BEGIN
	SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'QRC Review'
	DECLARE @UserIdToNotify INT 
	SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
	exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
SET @ict = @ict + 1
END             
        
--SELECT * from Coursesections WHERE CourseId = @Id        
        
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr        
--INNER JOIN ResourceMaster rm        
--ON sr.resourceid = rm.id        
        
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)        
        
        
 DECLARE @SharedCount INT         
DECLARE @DownloadCount INT       
DECLARE @VisitCount INT    
          
IF Exists(select TOP 1 1 from CourseMaster WHERE Id=@Id)               
 BEGIN              
           
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 1)          
 SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 1)          
    
    
 SET @VisitCount = (SELECT ViewCount FROM CourseMaster WHERE ID = @Id)    
     
 IF(@VisitCount IS NULL)    
 BEGIN    
 Update CourseMaster SET ViewCount =  1 WHERE ID = @Id     
    
 END    
    
 ELSE     
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
      , CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById              
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
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar]        
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
              
              
              
  SELECT cur.Id              
      ,CourseId              
      ,cur.URLReferenceId,uwl.URL as URLReference              
      ,cur.CreatedOn              
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId              
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId AND cm.Id=@Id              
              
              
SELECT cc.Id              
      ,[CourseId]              
      ,[Comments]              
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage              
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
      ,CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById            
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
  END       
        
         
 END        
        
 ELSE SET @return= 107         
        
    RETURN @return        
END        

GO

ALTER PROCEDURE [dbo].[CreateResource]           
(          
     @Title NVARCHAR(250),          
           @CategoryId INT,          
           @SubCategoryId INT=NULL,          
           @Thumbnail  NVARCHAR(400) = NULL,          
           @ResourceDescription  NVARCHAR(2000)=NULL,          
           @Keywords  NVARCHAR(1500)= NULL,          
           @ResourceContent NTEXT=NULL,          
           @MaterialTypeId INT=NULL,          
           @CopyRightId INT=NULL,          
           @IsDraft BIT,          
     @CreatedBy INT,          
     @References NVARCHAR(MAX)=null,            
     @ResourceFiles NVARCHAR(MAX)=null,          
     @ReadingTime INT = NULL,          
     @ResourceSourceId INT = NULL,          
     @LevelId INT = NULL,          
     @EducationalStandardId INT = NULL,          
     @EducationalUseId INT = NULL,          
     @Format NVARCHAR(100) = NULL,          
     @Objective NVARCHAR(MAX) = NULL          
               
     )          
AS          
BEGIN          
          
DECLARE @Id INT          
Declare @return INT   
DECLARE @MessageTypeId INT       
          
INSERT INTO [dbo].[ResourceMaster]          
           ([Title]          
           ,[CategoryId]          
           ,[SubCategoryId]          
           ,[Thumbnail]          
           ,[ResourceDescription]          
           ,[Keywords]          
           ,[ResourceContent]          
           ,[MaterialTypeId]          
           ,[CopyRightId]          
           ,[IsDraft]          
           ,[CreatedBy]          
           ,[CreatedOn],          
     [ReadingTime],          
     [LevelId],          
     [EducationalStandardId],          
     [EducationalUseId],          
     [Format],          
     [Objective],
	 [IsApproved]
   )          
     VALUES          
           (@Title,          
           @CategoryId,          
           @SubCategoryId,          
           @Thumbnail,           
           @ResourceDescription,           
           @Keywords,           
           @ResourceContent,          
           @MaterialTypeId,          
           @CopyRightId,          
           @IsDraft,           
     @CreatedBy,          
     GETDATE(),          
     @ReadingTime,          
     @LevelId,          
     @EducationalStandardId,          
     @EducationalUseId,          
     @Format,          
     @Objective,
	 NULL
     )          
          
 SET @Id=SCOPE_IDENTITY();          
          
IF(@IsDraft = 0)  
BEGIN           
DECLARE @TotalCount INT;          
DECLARE @QrcID INT;          
DECLARE @RecordId INT;          
          
--select top 10 * from QRCCategory where CategoryId =@CategoryID          
--select * from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 order by CreatedOn asc          
Declare @Date Datetime
SET @Date = getdate() 
SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Approval'

--exec spi_Notifications @Id,1,@Title,@ResourceDescription,@MessageTypeId,0,@Date,0,0,@CreatedBy,NULL,NULL,NULL
        
       
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 and QRCId in(select id from QRCMaster where active=1) --order by CreatedOn asc          
          
IF(@TotalCount>0)          
BEGIN          
select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 and QRCId in(select id from QRCMaster where active=1) 
AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
order by CreatedOn asc          
          
Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId          
          
IF EXISTS(          
SELECT           
TOP 1 1          
from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)          
BEGIN          
INSERT INTO ContentApproval(ContentId,          
ContentType,          
CreatedBy,          
CreatedOn,          
AssignedTo,          
AssignedDate,    
QrcId)          
          
SELECT           
@Id,          
2, -- Resource          
@CreatedBy,          
GETDATE(),          
userid,          
GETDATE(),    
@QrcID    
from QRCusermapping  where QRCID =@QrcID and active = 1  
and QRCusermapping.CategoryId = @CategoryId          
      UPDATE ResourceMaster SET IsDraft = 0 WHERE Id = @Id       
END          
          
          
          
END          
    
IF(@TotalCount=1)         
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END          
          
          
--select top 10 * from QRCCategory where CategoryId =@CategoryID          
            
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 2)  
 --BEGIN  
 --UPDATE ResourceMaster SET IsDraft = 1 WHERE Id = @Id  
 --END   
END   
 IF @Id>0   
 BEGIN          
 SET @return =100 -- creation success          
          
 IF @References IS NOT NULL          
 BEGIN          
 -- INSERT Resource URL References FROM JSON          
           
  INSERT INTO ResourceURLReferences          
              
  SELECT @Id,URLReferenceId,GETDATE() FROM            
   OPENJSON ( @References )            
  WITH (             
              URLReferenceId   int '$.URLReferenceId'           
  )           
          
 END          
          
 IF @ResourceFiles IS NOT NULL          
 BEGIN          
 -- INSERT Resource Associated Files FROM JSON          
           
  INSERT INTO ResourceAssociatedFiles          
              
  SELECT @Id,AssociatedFile,GETDATE(),FileName,1 FROM            
   OPENJSON ( @ResourceFiles )            
  WITH (             
              AssociatedFile   nvarchar(MAX) '$.AssociatedFile',      
     FileName   nvarchar(MAX) '$.FileName'       
  )           
          
 END          
          
 DECLARE @Version INT;          
 IF(@ResourceSourceId<>'')          
 BEGIN          
          
 SELECT TOP 1 @Version=Version FROM ResourceRemixHistory WHERE ResourceSourceId=@ResourceSourceId order by CreatedOn desc          
 IF(@Version IS NULL)          
 BEGIN          
 SET @Version=1          
 END          
          
 ELSE          
          
 BEGIN          
SET @Version=@Version+1          
 END          
 INSERT INTO ResourceRemixHistory(ResourceSourceID,          
ResourceRemixedID,          
Version,          
CreatedOn)          
VALUES(          
@ResourceSourceId,          
@Id,          
@Version,          
GETDATE()          
)          
          
END          
 SELECT Id,          
TitleId,          
FirstName +' ' +LastName as UserName,          
Email FROM UserMaster WHERE ID in (          
 SELECT           
userid          
from QRCusermapping  where QRCID = @QrcID and active = 1)          
AND           
IsEmailNotification = 1 
 
DECLARE @QRCUsers TABLE(ID INT IDENTITY(1,1),UserId INT)
INSERT INTO @QRCUsers  
SELECT Id FROM UserMaster WHERE ID in (SELECT userid from QRCusermapping  where QRCID = @QrcID and active = 1) AND IsEmailNotification = 1 
DECLARE @TotalRows INT
DECLARE @ict INT = 1
SELECT @TotalRows = COUNT(*) FROM @QRCUsers
WHILE @ict <= @TotalRows
BEGIN
	SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'QRC Review'
	DECLARE @UserIdToNotify INT 
	SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
	exec spi_Notifications @Id,1,@Title,@ResourceDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
SET @ict = @ict + 1
END
        
 exec GetResourceById  @Id          
 END          
          
 ELSE SET @return= 107           
          
    RETURN @return          
END          
