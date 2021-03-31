ALTER PROCEDURE [dbo].[sps_Announcements]   
    
AS   
BEGIN   
 -- SET NOCOUNT ON added to prevent extra result sets from   
 -- interfering with SELECT statements.   
 SET NOCOUNT ON;   
   
 IF EXISTS(SELECT TOP 1 1 FROM Announcements)   
 BEGIN   
   SELECT es.Id,   
es.Text,
es.Text_Ar,   
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,     
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,    
es.CreatedOn,   
es.UpdatedOn,   
es.Active FROM Announcements  es   
   INNER join UserMaster c  on es.CreatedBy= c.Id     
   INNER join UserMaster l on es.UpdatedBy =l.Id
ORDER BY es.CreatedOn DESC   
     return 105    
   
  END   
   
  ELSE   
  BEGIN   
    RETURN 102 -- reconrd does not exists     
  END;   
END   
 
