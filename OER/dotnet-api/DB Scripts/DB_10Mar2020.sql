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
 --  OR qmc.id in (
 --   SELECT DISTINCT cm.CategoryId FROM CourseMaster cm
 --INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserID AND ca.ContentType = 1
 --AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
 --UNION  
 --SELECT DISTINCT cm.CategoryId  FROM ResourceMaster cm
 --INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserID AND ca.ContentType = 2
 --AND  ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
 --  )
 RETURN 105
END
