ALTER PROCEDURE [dbo].[UpdateResource]      
     @Title  NVARCHAR(250),  
           @CategoryId INT,  
           @SubCategoryId INT= NULL,  
           @Thumbnail  NVARCHAR(400) = NULL,  
           @ResourceDescription  NVARCHAR(2000) = NULL,  
           @Keywords  NVARCHAR(1500) = NULL,  
           @ResourceContent ntext = NULL,  
           @MaterialTypeId INT= NULL,  
           @CopyRightId INT = NULL,  
           @IsDraft BIT = 0,  
     @References NVARCHAR(MAX)=null,    
     @ResourceFiles NVARCHAR(MAX)=null,    
     @Id INT,  
     @ReadingTime INT = NULL,  
     @LevelId INT = NULL,  
     @EducationalStandardId INT = NULL,  
     @EducationalUseId INT = NULL,  
     @Format NVARCHAR(100) = NULL,  
     @Objective NVARCHAR(4000) = NULL  
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM ResourceMaster WHERE @Id=@Id)  
BEGIN   
UPDATE ResourceMaster SET Title=@Title,  
           CategoryId=@CategoryId,  
           SubCategoryId=@SubCategoryId,  
           Thumbnail=@Thumbnail,  
           ResourceDescription=@ResourceDescription,  
           Keywords=@Keywords,  
           ResourceContent=@ResourceContent,  
           MaterialTypeId=@MaterialTypeId,  
           CopyRightId=@CopyRightId,  
           IsDraft=@IsDraft,  
           ReadingTime = @ReadingTime,  
           LevelId = @LevelId,  
           EducationalStandardId = @EducationalStandardId,  
           EducationalUseId = @EducationalUseId,  
           [Format] = @Format,  
           Objective = @Objective  ,
		   IsApproved = NULL
           WHERE Id=@Id  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    BEGIN
    SET @return= 106 -- update failed   
  

END  
 ELSE  
  BEGIN
  SET @return= 101 -- update success  
IF(@IsDraft = 0)
BEGIN 
  DECLARE @TotalCount INT;      
DECLARE @QrcID INT;      
DECLARE @RecordId INT;      
DECLARE @SectionId INT;      
DECLARE @i INT; 
DECLARE @CreatedBy INT     
SET @i = 1;      
SELECT @CreatedBy = CreatedBy FROM ResourceMaster WHERE Id = @Id
    
      
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc      
      
IF(@TotalCount>0)      
BEGIN      
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))
   order by CreatedOn asc      
      
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId      
      
   IF EXISTS(      
   SELECT       
     TOP 1 1      
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)      
     BEGIN   
		 IF NOT EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @Id AND ContentType = 2)
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
			   GETDATE() ,
			   @QrcID
			   from QRCusermapping  where QRCID =@QrcID and active = 1 
			   and QRCusermapping.CategoryId = @CategoryId  
		 END
		 ELSE
		 BEGIN
			UPDATE ContentApproval
			SET [Status] = NULL,
			ApprovedBy = NULL,
			ApprovedDate = NULL,
			UpdatedOn = NULL,
			UpdatedBy = NULL,
			Comment = NULL
			WHERE Id = (SELECT TOP(1) Id FROM ContentApproval WHERE ContentId = @Id AND ContentType = 2)
		 END
	   
	   UPDATE ResourceMaster SET IsDraft = 0 WHERE Id = @Id     
      END   
   
END      
IF(@TotalCount=1)        
BEGIN        
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID         
END 
 END
  END
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 2)
 --BEGIN
	--UPDATE ResourceMaster SET IsDraft = 1 WHERE Id = @Id
 --END 
IF @ResourceFiles IS NOT NULL  
 BEGIN  
 -- Update Resource Associated Files FROM JSON  
   
MERGE ResourceAssociatedFiles AS TARGET  
  USING  
    (  
    SELECT DISTINCT AssociatedFile,FileName
      FROM  
      OPENJSON(@ResourceFiles)  
      WITH (AssociatedFile nvarchar(200) '$.AssociatedFile',FileName nvarchar(200) '$.FileName')  
    ) AS source (AssociatedFile,FileName)  
  ON TARGET .AssociatedFile = source.AssociatedFile AND TARGET.FileName = source.FileName
  WHEN NOT MATCHED   
  THEN   
    INSERT (ResourceId,AssociatedFile,FileName, UploadedDate)  
      VALUES (@Id,source.AssociatedFile,source.FileName,GETDATE())
	WHEN NOT MATCHED BY SOURCE AND TARGET.ResourceId = @Id
    THEN DELETE;  
  
 END  
  
 IF @References IS NOT NULL  
 BEGIN  
MERGE ResourceURLReferences AS TARGET  
  USING  
    (  
    SELECT DISTINCT URLReferenceId  
      FROM  
      OPENJSON(@References)  
      WITH (URLReferenceId Int '$.URLReferenceId')  
    ) AS source (URLReferenceId)  
  ON TARGET.id = source.URLReferenceId
  AND TARGET.ResourceId = @Id
  WHEN NOT MATCHED   
  THEN   
    INSERT (ResourceId,URLReferenceId, CreatedOn)  
      VALUES (@Id,source.URLReferenceId,GETDATE())
  WHEN NOT MATCHED BY SOURCE AND TARGET.ResourceId = @Id
    THEN DELETE;
 END  
END  
  
ELSE  
 BEGIN  
  SET @return= 102 -- Record does not exists  
 END  
  
exec GetResourceById  @Id  
RETURN @return  
END  

GO

ALTER PROCEDURE [dbo].[UpdateCourse]          
           @Title NVARCHAR(250) = NULL,      
           @CategoryId INT,      
           @SubCategoryId INT=NULL,      
           @Thumbnail  NVARCHAR(400)  = NULL,      
           @CourseDescription  NVARCHAR(2000)  = NULL,      
           @Keywords  NVARCHAR(1500)  = NULL,      
           @CourseContent NTEXT  = NULL,                
           @CopyRightId INT  = NULL,      
           @IsDraft BIT,         
           @EducationId int = NULL,      
           @ProfessionId int = NULL,        
           @References NVARCHAR(MAX)=null,        
           @CourseFiles NVARCHAR(MAX)=null,       
           @UT_Sections [UT_Sections] Readonly,        
           @UT_Resource [UT_Resource] Readonly,     
           @Id INT,      
           @ReadingTime INT = NULL,      
           @LevelId INT = NULL,      
           @EducationalStandardId INT = NULL,      
           @EducationalUseId INT = NULL              
AS      
BEGIN      
DECLARE @i INT;      
DECLARE @SectionId INT;    
SET @i = 1;      
Declare @return INT      
DECLARE @SectionsCount INT;        
IF EXISTS (SELECT * FROM CourseMaster WHERE @Id=@Id)      
BEGIN       
UPDATE CourseMaster SET Title=@Title,      
           CategoryId=@CategoryId,      
           SubCategoryId=@SubCategoryId,      
           Thumbnail=@Thumbnail,      
           CourseDescription=@CourseDescription,      
           Keywords=@Keywords,      
           CourseContent=@CourseContent,                
           CopyRightId=@CopyRightId,      
           IsDraft=@IsDraft,      
           EducationId=@EducationId,      
           ProfessionId=@ProfessionId,      
           ReadingTime=@ReadingTime,      
           LevelId= @LevelId,      
           EducationalStandardId = @EducationalStandardId,      
           EducationalUseId = @EducationalUseId ,
		   IsApproved = NULL     
           WHERE Id=@Id      
  -- do log entry here      
    
SET @SectionsCount = (SELECT Count(*) FROM @UT_Sections)        
      
      
DELETE FROM SectionResource WHERE SectionId in(SELECT ID FROM  CourseSections WHERE CourseId = @Id)    
DELETE FROM CourseSections WHERE CourseId = @Id    
    
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
         
    IF @@ERROR <> 0      
        
    SET @return= 106 -- update failed       
      
      
 ELSE      
    BEGIN  
  SET @return= 101 -- update success   
IF(@IsDraft = 0)  
BEGIN  
   DECLARE @TotalCount INT;        
DECLARE @QrcID INT;        
DECLARE @RecordId INT;              
DECLARE @CreatedBy INT        
SELECT @CreatedBy = CreatedBy FROM CourseMaster WHERE Id = @Id  
      
        
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc        
        
IF(@TotalCount>0)        
BEGIN        
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0   
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
   order by CreatedOn asc        
        
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId        
        
   IF EXISTS(        
   SELECT         
     TOP 1 1        
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)        
     BEGIN 
   IF NOT EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @Id AND ContentType = 1)  
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
  END  
  ELSE  
   BEGIN  
   UPDATE ContentApproval  
   SET [Status] = NULL,  
   ApprovedBy = NULL,  
   ApprovedDate = NULL,  
   UpdatedOn = NULL,  
   UpdatedBy = NULL,  
   Comment = NULL  
   WHERE Id = (SELECT TOP(1) Id FROM ContentApproval WHERE ContentId = @Id AND ContentType = 1)  
   END       
  
    UPDATE CourseMaster SET IsDraft = 0 WHERE Id = @Id  
      END        
    
   
END        
IF(@TotalCount=1)          
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END   
 END  
  END      
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 1)  
 --BEGIN  
 --UPDATE CourseMaster SET IsDraft = 1 WHERE Id = @Id  
 --END   
IF @CourseFiles IS NOT NULL      
 BEGIN      
 -- Update Resource Associated Files FROM JSON      
       
MERGE CourseAssociatedFiles AS TARGET      
  USING      
    (      
    SELECT DISTINCT AssociatedFile,FileName     
      FROM      
      OPENJSON(@CourseFiles)      
      WITH (AssociatedFile nvarchar(500) '$.AssociatedFile',FileName nvarchar(500) '$.FileName')      
    ) AS source (AssociatedFile,FileName)      
  ON TARGET.AssociatedFile = source.AssociatedFile AND TARGET.FileName = source.FileName
    AND TARGET.CourseId = @Id     
  WHEN NOT MATCHED       
  THEN       
    INSERT (CourseId,AssociatedFile,FileName, CreatedOn)      
      VALUES (@Id,source.AssociatedFile,source.FileName,GETDATE())
  WHEN NOT MATCHED BY SOURCE AND TARGET.CourseId = @Id
    THEN DELETE;       
      
 END      
      
 IF @References IS NOT NULL      
 BEGIN      
MERGE CourseURLReferences AS TARGET      
  USING      
    (      
    SELECT DISTINCT URLReferenceId      
      FROM      
      OPENJSON(@References)      
      WITH (URLReferenceId int '$.URLReferenceId')      
    ) AS source (URLReferenceId)      
  ON TARGET.id = source.URLReferenceId  
  AND TARGET.CourseId = @Id   
  -- WHEN MATCHED THEN     
    --    UPDATE SET CourseId = @Id,     
        --           id =source.URLReferenceId,     
      --             CreatedOn = GETDATE()     
  WHEN NOT MATCHED       
  THEN       
    INSERT (CourseId,URLReferenceId, CreatedOn)      
      VALUES (@Id,source.URLReferenceId,GETDATE())
  	WHEN NOT MATCHED BY SOURCE AND TARGET.CourseId = @Id
    THEN DELETE;     
 END      
END      
      
ELSE      
 BEGIN      
  SET @return= 102 -- Record does not exists      
 END      
      
exec GetCourseById  @Id      
RETURN @return      
END;      