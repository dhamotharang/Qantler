
/****** Object:  StoredProcedure [dbo].[UpdateCopyright]    Script Date: 13-01-2020 12.46.39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
-- exec UpdateCopyright @Id=1,@Title='testsdfdf',@Description='dfsfds',@Title_Ar='test',@Description_Ar='fw', @UpdatedBy=9,@Active=1,@Media='x3d.png'      
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
 @IsResourceProtect BIT = 0
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
  IsResourceProtect = @IsResourceProtect
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
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media  ,cr.Protected, cr.IsResourceProtect    
   from CopyrightMaster cr       
   inner join UserMaster c  on cr.CreatedBy= c.Id      
   inner join UserMaster l on cr.UpdatedBy =l.Id where cr.Status=1 order by cr.Id desc       
      
   RETURN @return      
END   

GO

ALTER TABLE [dbo].[CopyrightMaster]
ALTER COLUMN [Description_Ar] TEXT;   
