ALTER PROCEDURE [dbo].[sps_Notifications]   
 (  
 @UserID int ,  
 @PageNo int = 1,    
@PageSize int = 10    
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @Return INT, @Total INT,@RecordCount INT  
 declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
    -- Insert statements for procedure here  
  
   
 SELECT @RecordCount = Count(ID)  FROM Notifications    
    WHERE UserID = @UserID  AND IsDelete<>1  
 SELECT @Total = Count(ID)  FROM Notifications    
    WHERE UserID = @UserID AND IsRead = 0  AND messagetypeid <> null
;with sqlpaging as (    
SELECT     
Rownumber = ROW_NUMBER() OVER(ORDER BY n.Id desc) ,   
n.Id,  
 @Total as Total,  
n.ReferenceId,  
n.ReferenceTypeId,  
n.Subject,  
n.Content,  
n.MessageTypeId,  
m.type as MessageType,  
n.IsApproved,  
n.CreatedDate,  
n.ReadDate,  
n.DeletedDate,  
n.IsRead,  
n.IsDelete,  
n.Comment,  
n.Status,
um.FirstName + ' ' + um.LastName as Reviewer,  
n.[Url] As EmailUrl,  
n.UserId from Notifications n   
inner join MessageType m  
ON n.MessageTypeID = m.id  
LEFT join UserMaster um  
ON n.Reviewer = um.id  
WHERE UserID = @UserID AND IsDelete<>1)  
  
    
select    
 top (@PageSize) *, (SELECT COUNT(IsRead) from sqlpaging WHERE IsRead=0) as TotalUnRead,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between @start and @end    
ORDER by CreatedDate desc  
  
IF(@RecordCount>0)  
BEGIN  
SET @Return = 105  
END  
ELSE  
BEGIN  
SET @Return = 113  
END  
  
RETURN @Return  
END  