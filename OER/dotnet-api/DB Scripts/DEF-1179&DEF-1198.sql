alter table [dbo].[CopyrightMaster] alter column [Description] text null
GO

ALTER PROCEDURE [dbo].[UpdateCopyright]       
 @Id INT,      
 @Title NVARCHAR(100),      
 @Description Text=null,      
 @Title_Ar NVARCHAR(200),      
 @Description_Ar NVARCHAR(MAX)=null,      
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

ALTER PROCEDURE [dbo].[CreateCopyright]     
 @Title NVARCHAR(100),    
 @Description Text=null,    
 @Title_Ar NVARCHAR(200),    
 @Description_Ar NVARCHAR(MAX)=null,    
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
