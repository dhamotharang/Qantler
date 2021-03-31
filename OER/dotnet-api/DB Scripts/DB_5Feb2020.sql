Alter table [WebPageContent] Add VideoLink nvarchar(max) null
GO

ALTER PROCEDURE [dbo].[spu_WebPageContent]    
 (    
 @PageID INT,     
 @PageContent NVARCHAR(MAX),    
 @PageContent_Ar NVARCHAR(MAX),    
 @UpdatedBy INT,
 @VideoLink NVARCHAR(MAX)
 )    
AS    
BEGIN    
IF EXISTS(SELECT TOP 1 1 FROM WebPageContent  WHERE PageId =@PageID)    
BEGIN    
 UPDATE WebPageContent    
    
 SET PageContent = @PageContent,    
  PageContent_Ar = @PageContent_Ar,  
  UpdatedBy = @UpdatedBy,    
  UpdatedOn = GETDATE(),  
  VideoLink = @VideoLink  
 WHERE  PageId =@PageID    
    
RETURN 101;    
END    
    
ELSE    
    
BEGIN    
RETURN 105    
END;    
    
END 
GO

ALTER PROCEDURE [dbo].[spi_WebPageContent]    
 (    
 @PageID INT,     
 @PageContent NVARCHAR(MAX),    
 @PageContent_Ar NVARCHAR(MAX),   
 @CreatedBy INT,
 @VideoLink NVARCHAR(MAX)    
 )    
AS    
BEGIN    
    
IF NOT EXISTS(SELECT TOP 1 1 FROM WebPageContent WHERE PageId =@PageID )    
BEGIN    
 INSERT INTO WebPageContent(PageID, PageContent,PageContent_Ar,CreatedBy,CreatedOn,VideoLink)    
VALUES(    
@PageID,    
@PageContent,    
@PageContent_Ar,    
@CreatedBy    
,GETDATE(),
@VideoLink
)    
RETURN 100    
END    
    
ELSE    
    
BEGIN    
RETURN 105    
END    
    
    
END 
GO

ALTER PROCEDURE [dbo].[sps_PageContentByPageId]    
 (    
 @PageId INT    
 )    
AS    
BEGIN    
SELECT     
Id,    
PageID, PageContent,PageContent_Ar,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,VideoLink   
    
 FROM WebPageContent WHERE PageID = @PageId    
    
 RETURN 105    
END 
GO

ALTER PROCEDURE [dbo].[sps_PageContents]      
AS    
BEGIN    
select WP.Id, WP.PageID, WP.PageContent,WP.PageContent_Ar,WP.CreatedBy,WP.CreatedOn,WP.UpdatedBy,WP.UpdatedOn,WC.PageName,WP.VideoLink
from WebPageContent as WP inner join WebContentPages as WC on Wp.PageID=WC.Id 
    
 RETURN 105    
END 


