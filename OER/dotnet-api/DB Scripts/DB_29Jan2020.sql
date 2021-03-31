
Alter table CategoryMaster Add Weight int
Alter table SubCategoryMaster Add Weight int
Alter table EducationMaster Add Weight int
Alter table ProfessionMaster Add Weight int
Alter table CopyrightMaster Add Weight int
Alter table MaterialTypeMaster Add Weight int
Alter table lu_Educational_Standard Add Weight int
Alter table lu_Educational_Use Add Weight int
Alter table lu_Level Add Weight int
GO

ALTER PROCEDURE [dbo].[sps_ResourceMasterData]          
          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
    -- Insert statements for procedure here          
 select Id,[Standard],Standard_Ar from [dbo].[lu_Educational_Standard] where Active = 1 and Status = 1
 order by Weight Desc
          
select Id,          
EducationalUse,EducationalUse_Ar from [dbo].[lu_Educational_Use] where Active = 1    and Status = 1  
order by Weight Desc
          
select Id,          
[Level],Level_Ar from [dbo].[lu_Level] where Active = 1  and Status = 1    
order by Weight Desc
          
        
SELECT cm.Id,          
    cm.Name,          
    cm.Name_Ar FROM CategoryMaster cm where cm.Active = 1   and cm.Status = 1     
	order by Weight Desc
        
        
    SELECT sm.Id,      
    sm.Name,      
    sm.Name_Ar,      
    cm.Name as CategoryName,       
    cm.Id as CategoryId    
   from SubCategoryMaster sm       
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id  
   where sm.Active = 1 and cm.Active = 1   and cm.Status = 1  and sm.Status = 1
   order by sm.Weight Desc
        
        
  SELECT cm.Id,          
    cm.Name,          
    cm.Name_Ar        
 from MaterialTypeMaster cm where cm.Active = 1  and cm.Status = 1   
 order by Weight Desc
        
        
  SELECT em.Id,          
    em.Name ,Name_Ar from EducationMaster em where em.Active = 1   and em.Status = 1    
	order by Weight Desc
        
        
   SELECT cm.Id,          
    cm.Name,Name_Ar        
 from ProfessionMaster cm where cm.Active = 1    and cm.Status = 1 order by Weight Desc     
        
  SELECT cr.Id,          
    cr.Title,          
    Cr.Description,          
    cr.Title_Ar,          
    Cr.Description_Ar  ,  
 Media  
 from CopyrightMaster cr where cr.Active = 1  and cr.Status = 1 order by Weight Desc    
RETURN 105;          
END 
GO

ALTER PROCEDURE [dbo].[GetCategory]    
AS  
BEGIN    
 IF Exists(select * from CategoryMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active ,cm.Weight 
   from CategoryMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status = 1 --and c.Active = 1 and l.Active = 1 -- added Active check
   order by cm.Weight desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO

ALTER PROCEDURE [dbo].[GetSubCategory]    
AS  
BEGIN    
 IF Exists(select * from SubCategoryMaster where Status=1)   
 BEGIN   
  SELECT sm.Id,  
    sm.Name,  
    sm.Name_Ar,  
    cm.Name as CategoryName,   
    cm.Id as CategoryId,  
    sm.CreatedOn,      
       CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    sm.UpdatedOn,   
    sm.Active , sm.Weight
   from SubCategoryMaster sm   
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id  
   inner join UserMaster c  on sm.CreatedBy= c.Id  
   inner join UserMaster l on sm.UpdatedBy =l.Id  
   where sm.Status = 1 --and cm.Active = 1 and l.Active = 1 and c.Active = 1  
   order by sm.Weight desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO

ALTER PROCEDURE [dbo].[GetEducation]    
AS  
BEGIN    
 IF Exists(select * from EducationMaster)   
 BEGIN   
  SELECT em.Id,  
    em.Name,  
    em.CreatedOn,      
    c.FirstName,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    em.UpdatedOn, em.Active,  
    em.Name_Ar, em.Weight
   from EducationMaster em   
   inner join UserMaster c  on em.CreatedBy= c.Id  
   inner join UserMaster l on em.UpdatedBy =l.Id where em.Status=1
   Order by em.Weight desc   
   
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO

ALTER PROCEDURE [dbo].[GetProfession]    
AS  
BEGIN    
 IF Exists(select * from ProfessionMaster where Status=1)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active ,cm.Weight 
   from ProfessionMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id   
   where cm.Status=1   
   order by cm.Weight desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO

ALTER PROCEDURE [dbo].[GetCopyright]        
AS      
BEGIN        
  IF Exists(select * from CopyrightMaster)      
  BEGIN       
  SELECT cr.Id,      
    cr.Title,      
    Cr.Description,      
    cr.Title_Ar,      
    Cr.Description_Ar,      
    cr.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn, cr.Media ,cr.Protected ,  
 cr.IsResourceProtect, cr.Weight 
   from CopyrightMaster cr       
   inner join UserMaster c  on cr.CreatedBy= c.Id      
   inner join UserMaster l on cr.UpdatedBy =l.Id        
   where cr.Status = 1 --and c.Active = 1 and l.Active = 1 -- added active check 
   order by cr.Weight desc      
      
  RETURN 105 -- record exists      
  END      
  ELSE      
  RETURN 102 -- record does not exists      
END 
GO

ALTER PROCEDURE [dbo].[GetMaterialType]    
AS  
BEGIN    
 IF Exists(select top 1 1 from MaterialTypeMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active,
	cm.Weight
   from MaterialTypeMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status =1 --and l.Active = 1 --Added active check  
   order by cm.Weight desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO

ALTER PROCEDURE [dbo].[sps_lu_Educational_Standard]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Educational_Standard WHERE Active = 1)  
 BEGIN  
   SELECT es.Id,  
es.Standard,  
es.Standard_Ar,  
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
es.CreatedOn,  
es.UpdatedOn,  
es.Active,
es.Weight FROM lu_Educational_Standard  es  
   INNER join UserMaster c  on es.CreatedBy= c.Id    
   INNER join UserMaster l on es.UpdatedBy =l.Id   
   WHERE es.Status = 1 --and c.Active = 1 and l. Active = 1 -- added active check 
   order by es.Weight desc
  
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO

ALTER PROCEDURE [dbo].[sps_EducationalUse]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Educational_Use WHERE Status = 1)  
 BEGIN  
   SELECT eu.Id,  
eu.EducationalUse,  
eu.EducationalUse_Ar,  
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
eu.CreatedOn,  
eu.UpdatedOn,  
eu.Active,
eu.Weight FROM lu_Educational_Use  eu   
   INNER join UserMaster c  on eu.CreatedBy= c.Id    
   INNER join UserMaster l on eu.UpdatedBy =l.Id   
   WHERE eu.Status = 1 --and c.Active = 1 and l.Active = 1 --added Active check 
   order by eu.Weight desc
  
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO

ALTER PROCEDURE [dbo].[sps_lu_Level]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Level WHERE Status = 1)  
 BEGIN  
    
   SELECT el.Id,  
     el.[Level],  
     el.Level_Ar,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
     CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
     el.CreatedOn,  
     el.UpdatedOn,  
     el.Active,
	 el.Weight FROM lu_Level  el   
      INNER join UserMaster c  on el.CreatedBy= c.Id    
      INNER join UserMaster l on el.UpdatedBy =l.Id   
      WHERE el.Status = 1 --and c.Active = 1 and l.Active = 1 -- added active check 
	  order by el.Weight desc
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO

ALTER PROCEDURE [dbo].[CreateCategory] 
	@Name NVARCHAR(150),
	@Name_Ar NVARCHAR(350),
	@CreatedBy INT,
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM CategoryMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO CategoryMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status,Weight)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,1,1,@Weight)
		
		exec CreateLogEntry @LogModuleId=1,@UserId=@CreatedBy,@ActionId=1,@ActionDetail=''

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Name_Ar
		 from CategoryMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id and cm.Status = 1 
		 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[UpdateCategory] 
	@Id INT,
	@Name NVARCHAR(100),
	@Name_Ar NVARCHAR(300),
	@UpdatedBy INT,
	@Active BIT,
	@Weight INT
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT TOP 1 1 FROM CategoryMaster WHERE Id = @Id)
BEGIN	
		UPDATE CategoryMaster  SET Name=@Name,
		Name_Ar = @Name_Ar,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active
									  ,Weight = @Weight
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Weight
		 from CategoryMaster cm 		 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 where cm.Status = 1
		 order by cm.Id desc 

		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[CreateSubCategory] 
	@Name NVARCHAR(200),
	@Name_Ar NVARCHAR(200),
	@CategoryId INT,
	@CreatedBy INT,
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name AND CategoryId=@CategoryId and Status = 1)
BEGIN	
		INSERT INTO SubCategoryMaster (Name,Name_Ar,CategoryId, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active,Status,Weight)
		VALUES (@Name,@Name_Ar,@CategoryId,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1,1,@Weight)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active,
				sm.Weight
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id 
		 where cm.Status = 1 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[CreateSubCategory] 
	@Name NVARCHAR(200),
	@Name_Ar NVARCHAR(200),
	@CategoryId INT,
	@CreatedBy INT,
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name AND CategoryId=@CategoryId and Status = 1)
BEGIN	
		INSERT INTO SubCategoryMaster (Name,Name_Ar,CategoryId, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active,Status,Weight)
		VALUES (@Name,@Name_Ar,@CategoryId,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1,1,@Weight)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active,
				sm.Weight
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id 
		 where cm.Status = 1 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[UpdateSubCategory] 
	@Id INT,
	@CategoryId INT,
	@Name NVARCHAR(255),
	@Name_Ar NVARCHAR(255),
	@UpdatedBy INT,
	@Active INT,
	@Weight INT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE SubCategoryMaster  SET Name=@Name,
									 Name_Ar=@Name_Ar,
									  CategoryId=@CategoryId,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(),
									  Active=@Active,
									  Weight=@Weight
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active,
				sm.Weight
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id 
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id where sm.Status=1 order by cm.Id desc 	

		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[CreateEducation] 
	@Name NVARCHAR(150),
	@CreatedBy INT,
	@Name_Ar NVARCHAR(MAX),
	@Active BIT,
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO EducationMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status,Weight)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,@Active,1,@Weight)
		
		-- do log entry here

	SET	@return= 100 -- creation success

END

ELSE
	BEGIN
		SET	@return=105 -- Record exists
	END

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active,em.Name_Ar,em.Weight
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id	
		 where em.Status = 1 --and c.Active = 1 and l.Active = 1
		 Order by em.Id desc 	
		 
		 RETURN @return	
END
GO

ALTER  PROCEDURE [dbo].[UpdateEducation] 
	@Id INT,
	@Name NVARCHAR(150),
	@UpdatedBy INT,
	@Active BIT,
	@Name_Ar NVARCHAR(MAX),
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name AND Id<> @Id)
BEGIN	
		UPDATE EducationMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active,
									  Name_Ar = @Name_Ar,
									  Weight = @Weight
									  WHERE Id=@Id
		-- do log entry here	

	IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active,
				em.Name_Ar,em.Weight
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id where em.Status=1 Order by em.Id desc 

	RETURN @return
END
GO

ALTER PROCEDURE [dbo].[DeleteEducation]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT 
Declare @catCount INT 
  IF EXISTS (SELECT * FROM EducationMaster  WHERE Id=@Id)  
    BEGIN  
 
    SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE EducationId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update EducationMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    
    
  SELECT em.Id,  
    em.Name,  
    em.CreatedOn,      
    c.FirstName,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    em.UpdatedOn, em.Active , em.Weight 
   from EducationMaster em   
   inner join UserMaster c  on em.CreatedBy= c.Id  
   inner join UserMaster l on em.UpdatedBy =l.Id where Status=1 Order by em.Id desc   
  RETURN @return  
END  
GO

ALTER PROCEDURE [dbo].[CreateProfession] 
	@Name NVARCHAR(200),
	@CreatedBy INT,
	@Name_Ar NVARCHAR(200),
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name and Status=1)
BEGIN	
		INSERT INTO ProfessionMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status,Weight)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,1,1,@Weight)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Weight
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Weight desc 	
		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[UpdateProfession] 
	@Id INT,
	@Name NVARCHAR(200),
	@UpdatedBy INT,
	@Active BIT,
	@Name_Ar NVARCHAR(200),
	@Weight INT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE ProfessionMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active,
									  Name_Ar = @Name_Ar,
									  Weight = @Weight
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Weight
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Weight desc 	

		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[CreateCopyright]     
 @Title NVARCHAR(100),    
 @Description Text,    
 @Title_Ar NVARCHAR(100),    
 @Description_Ar NVARCHAR(MAX),    
 @CreatedBy INT,    
 @Media NVARCHAR(100)=NULL ,  
 @Protected BIT = 0,
 @IsResourceProtect BIT = 0,
 @Weight INT = 0
AS    
BEGIN    
Declare @return INT    
  
IF NOT EXISTS (SELECT * FROM CopyrightMaster WHERE Title=@Title and Status = 1)    
BEGIN     
    
  INSERT INTO CopyrightMaster (Title,Description,Title_Ar,Description_Ar, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Media,Protected,IsResourceProtect,Active,Status,Weight)    
  VALUES (@Title, @Description,@Title_Ar,@Description_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Media,@Protected,@IsResourceProtect,1,1,@Weight)    
      
  -- do log entry here     
    
  SET @return = 100 -- creation success    
END    
    
ELSE    
 BEGIN    
  SET @return = 105 -- Record exists    
 END    
    
  SELECT cr.Id,    
    cr.Title,    
    Cr.Description,    
    cr.Title_Ar,    
    cr.Description_Ar,    
    cr.CreatedOn,    
    CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,    
    CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,Media,
	cr.IsResourceProtect,
	cr.Protected, cr.Weight
   from CopyrightMaster cr     
   inner join UserMaster c  on cr.CreatedBy= c.Id    
   inner join UserMaster l on cr.UpdatedBy =l.Id 
   where cr.Status = 1 --and c.Active = 1 and l.Active = 1
   order by cr.Weight desc     
    
   RETURN  @return    
    
END    
GO

ALTER PROCEDURE [dbo].[UpdateCopyright]       
 @Id INT,      
 @Title NVARCHAR(100),      
 @Description Text,      
 @Title_Ar NVARCHAR(100),      
 @Description_Ar NVARCHAR(MAX),      
 @UpdatedBy INT,       
 @Active BIT,      
 @Media NVARCHAR(200) = NULL,    
 @Protected BIT = 0,
 @IsResourceProtect BIT = 0,
 @Weight INT
AS      
BEGIN      
Declare @return INT      
IF EXISTS (SELECT TOP 1 1 FROM CopyrightMaster WHERE  Id = @Id)      
BEGIN       
  UPDATE CopyrightMaster  SET Title=@Title,       
  [Description]=@Description,      
  Title_Ar = @Title_Ar,      
  Description_Ar = @Description_Ar,      
  UpdatedBy = @UpdatedBy,        
  UpdatedOn=GETDATE(),       
  Active=@Active,       
  Media = @Media  ,    
  Protected = @Protected,
  IsResourceProtect = @IsResourceProtect,
  Weight = @Weight
  WHERE Id=@Id      
  -- do log entry here       
      
    IF @@ERROR <> 0      
          
   SET @return = 106 -- update failed       
    ELSE      
   SET @return = 101 -- update success       
END      
      
ELSE      
 BEGIN      
  SET @return =105 -- Record exists      
 END      
      
 SELECT cr.Id,      
    cr.Title,      
    Cr.[Description],      
    cr.Title_Ar,      
    Cr.Description_Ar,      
    cr.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media,
	cr.Protected, cr.IsResourceProtect, cr.Weight 
   from CopyrightMaster cr       
   inner join UserMaster c  on cr.CreatedBy= c.Id      
   inner join UserMaster l on cr.UpdatedBy =l.Id where cr.Status=1 order by cr.Id desc       
      
   RETURN @return      
END   
GO

ALTER PROCEDURE [dbo].[CreateMaterialType] 
	@Name NVARCHAR(150),
	@Name_Ar NVARCHAR(150),
	@CreatedBy INT,
	@Weight INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO MaterialTypeMaster (Name,Name_Ar,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active,Status,Weight)
		VALUES (@Name,@Name_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1,1,@Weight)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Weight
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id
		 where cm.Status = 1 --and c.Active = 1 and l.Active = 1
		 order by cm.Weight desc 	
		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[UpdateMaterialType] 
	@Id INT,
	@Name NVARCHAR(100),
	@Name_Ar NVARCHAR(100),
	@UpdatedBy INT,
	@Active BIT,
	@Weight INT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE MaterialTypeMaster  SET Name=@Name,
		Name_Ar=@Name_Ar,
									  UpdatedBy=@UpdatedBy,
									   UpdatedOn=GETDATE(),Active=@Active,
									   Weight=@Weight
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active,cm.Weight
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Weight desc 	

		 RETURN @return
END
GO

ALTER PROCEDURE [dbo].[spi_lu_Educational_Standard]
	(
	@Standard NVARCHAR(200),
	@Standard_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT *  FROM lu_Educational_Standard WHERE [Standard] = @Standard AND Status =1)
	BEGIN

INSERT INTO lu_Educational_Standard(
[Standard],
Active,
CreatedBy,
UpdatedBy,
CreatedOn,
UpdatedOn,
Standard_Ar,
Status,
Weight
)
VALUES
(
@Standard,
1,
@CreatedBy,
@CreatedBy,
GETDATE(),
GETDATE(),
@Standard_Ar,
1,
@Weight
)

SET @ID =(SELECT scope_identity())
--print @ID
	SELECT es.Id,
es.[Standard],
es.[Standard_Ar],
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
es.CreatedOn,
es.UpdatedOn,
es.Active, es.Weight FROM lu_Educational_Standard  es
   INNER join UserMaster c  on es.CreatedBy= c.Id  
   INNER join UserMaster l on es.UpdatedBy =l.Id 
   WHERE es.Status = 1 AND es.id = @ID
   Order by es.Weight

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO

ALTER PROCEDURE [dbo].[spi_lu_Educational_Use]
	(
	@EducationalUse NVARCHAR(200),
	@EducationalUse_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT * FROM lu_Educational_Use WHERE EducationalUse = @EducationalUse AND Status =1)
	BEGIN

INSERT INTO lu_Educational_Use(
EducationalUse,
EducationalUse_Ar,
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active,
Status,
Weight
)
VALUES
(
@EducationalUse,
@EducationalUse_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
1,
1,
@Weight
)

SET @ID =(SELECT scope_identity())
print @ID
	SELECT eu.Id,
eu.EducationalUse,
eu.EducationalUse_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
eu.CreatedOn,
eu.UpdatedOn,
eu.Active, eu.Weight FROM lu_Educational_Use  eu 
   INNER join UserMaster c  on eu.CreatedBy= c.Id  
   INNER join UserMaster l on eu.UpdatedBy =l.Id 
   WHERE eu.Status = 1 AND eu.id = @ID
   Order by eu.Weight

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO

ALTER PROCEDURE [dbo].[spi_lu_Level]
	(
	@Level NVARCHAR(200),
	@Level_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT * FROM lu_Level WHERE [Level] = @Level AND Status =1)
	BEGIN

INSERT INTO lu_Level(
[Level],
[Level_Ar],
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active,
Status,
Weight
)
VALUES
(
@Level,
@Level_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
1,
1,
@Weight
)

SET @ID =(SELECT scope_identity())
print @ID
	SELECT el.Id,
el.[Level],
el.Level_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
el.CreatedOn,
el.UpdatedOn,
el.Active, el.Weight FROM lu_Level  el 
   INNER join UserMaster c  on el.CreatedBy= c.Id  
   INNER join UserMaster l on el.UpdatedBy =l.Id 
   WHERE el.Status = 1 AND el.id = @ID
   order by el.Weight desc

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO

ALTER PROCEDURE [dbo].[spu_lu_Educational_Standard]
	(
	@Id INT,
	@Standard NVARCHAR(200),
	@Standard_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Standard WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Educational_Standard
				SET 
					[Standard] = @Standard,
					Standard_Ar = @Standard_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active,
					Weight =@Weight
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

		SELECT es.Id,
			es.[Standard],
			es.[Standard_Ar],
			CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
			CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
			es.CreatedOn,
			es.UpdatedOn,
			es.Active, es.Weight
			FROM lu_Educational_Standard  es
			   INNER join UserMaster c  on es.CreatedBy= c.Id  
			   INNER join UserMaster l on es.UpdatedBy =l.Id 
		WHERE es.Status = 1 AND es.id = @Id order by es.Weight desc
	 RETURN @return;  
END
GO

ALTER PROCEDURE [dbo].[spu_lu_Educational_Use]
	(
	@Id INT,
	@EducationalUse NVARCHAR(200),
	@EducationalUse_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Use WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Educational_Use
				SET 
					EducationalUse = @EducationalUse,
					EducationalUse_Ar = @EducationalUse_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active,
					Weight = @Weight
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

		SELECT eu.Id,
			eu.EducationalUse,
			eu.EducationalUse_Ar,
			CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
			CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
			eu.CreatedOn,
			eu.UpdatedOn,
			eu.Active, eu.Weight
			FROM lu_Educational_Use  eu 
			   INNER join UserMaster c  on eu.CreatedBy= c.Id  
			   INNER join UserMaster l on eu.UpdatedBy =l.Id 
		WHERE eu.Status = 1 AND eu.id = @Id order by eu.Weight desc;
	 RETURN @return;  
END
GO

ALTER PROCEDURE [dbo].[spu_lu_Level]
	(
	@Id INT,
	@Level NVARCHAR(200),
	@Level_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT,
	@Weight INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Level WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Level
				SET 
					[Level] = @Level,
					Level_Ar = @Level_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active,
					Weight =@Weight
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

			SELECT el.Id,
					el.[Level],
					el.Level_Ar,
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active,el.Weight FROM lu_Level  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Status = 1 AND el.id = @ID order by el.Weight desc
	 RETURN @return;  
END
GO

Update CategoryMaster set Weight = 1;
Update SubCategoryMaster set Weight = 1;
Update EducationMaster set Weight = 1;
Update ProfessionMaster set Weight = 1;
Update CopyrightMaster set Weight = 1;
Update MaterialTypeMaster set Weight = 1;
Update lu_Educational_Standard set Weight = 1;
Update lu_Educational_Use set Weight = 1;
Update lu_Level set Weight = 1;
