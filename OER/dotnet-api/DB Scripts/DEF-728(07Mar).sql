ALTER PROCEDURE [dbo].[GetCategory]
(
@IsActive    INT = 0
)
AS
BEGIN    
 if(@IsActive = 0 and Exists(select * from CategoryMaster where Status = 1 ))
    begin
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
        where cm.Status = 1 --and cm.Active = 1 --and l.Active = 1 -- added Active check
        order by cm.Weight desc
    end
  else if(@IsActive != 0 and Exists(select * from CategoryMaster where Status = 1 and Active = 1  ))
    begin
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
        where cm.Status = 1 and cm.Active = 1
        order by cm.Weight desc
    end
    else
    RETURN 102 -- reconrd does not exists
  RETURN 105 -- record exists  
END
GO

USE [oerdevdb]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 07-Mar-20 12:12:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
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
   (select count(X.id) from(SELECT id ,value FROM UserMaster       CROSS APPLY STRING_SPLIT(subjectsinterested, ','))X      where X.value=(select Name from CategoryMaster where Id=@Id));  
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

USE [oerdevdb]
GO
/****** Object:  StoredProcedure [dbo].[sps_QRCByUserID]    Script Date: 07-Mar-20 6:40:46 PM ******/
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
ALTER PROCEDURE [dbo].[sps_QRCByUserID]  
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
  ON qmc.Id = qc.CategoryId  and qmc.Active=1
   WHERE qm.ID in ( SELECT QRCId FROM QRCUserMapping WHERE UserId = @UserID AND Active = 1)
   AND qm.Active =1 AND qmc.Status = 1 AND qmc.Active = 1
   OR qmc.id in (
    SELECT DISTINCT cm.CategoryId FROM CourseMaster cm
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserID AND ca.ContentType = 1
 AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
 UNION  
 SELECT DISTINCT cm.CategoryId  FROM ResourceMaster cm
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserID AND ca.ContentType = 2
 AND  ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
   )
 RETURN 105
END
 