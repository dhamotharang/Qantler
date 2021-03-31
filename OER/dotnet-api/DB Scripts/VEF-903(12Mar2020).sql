ALTER PROCEDURE [dbo].[UrlIsWhitelisted] 
@URL nvarchar(2000)
AS
BEGIN        
        
    SELECT wlu.Id,    
       CONCAT(c.FirstName, '', c.LastName) as RequestedBy,
       CONCAT(l.FirstName, '', l.LastName) as VerifiedBy,
       VerifiedBy AS [VerifiedById]
      ,URL
      ,IsApproved
      ,RequestedOn
      ,VerifiedOn
      ,RejectedReason
  FROM WhiteListingURLs wlu 
   inner join UserMaster c  on wlu.RequestedBy= c.Id
         left join UserMaster l on wlu.VerifiedBy =l.Id  where IsApproved=1

 

  IF EXISTS(select wlu.Id from  WhiteListingURLs wlu  inner join UserMaster c  on wlu.RequestedBy= c.Id left join UserMaster l on wlu.VerifiedBy =l.Id  where IsApproved=1 and wlu.URL=@URL)
        return 105 -- record exists
        
        ELSE
        return 102 -- record does not exists
END