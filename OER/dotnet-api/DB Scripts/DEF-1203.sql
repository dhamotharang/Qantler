ALTER PROCEDURE [dbo].[sps_ContentForApproval]
	(
	@QrcId INT = NULL,
	@CategoryID INT = NULL,
	@UserId INT,
	@PageNo int = 1,
    @PageSize int = 5
	)
AS
BEGIN
Declare @return INT, @query NVARCHAR(MAX)
declare @start int, @end int
set @start = (@PageNo - 1) * @PageSize + 1
set	@end = @PageNo * @PageSize

CREATE table #tempData
(
ContentId INT,
ContentApprovalID INT,
TITLE NVARCHAR(200),
CreatedOn DATETIME,
ContentType INT,
CategoryId INT
)


IF(@CategoryID =0 AND @QrcId =0)
BEGIN
--print 'both null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND  ca.ContentType = 1
	AND ca.ApprovedBy=@UserId AND ca.Status=1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND  ca.ContentType = 2
	AND ca.ApprovedBy=@UserId AND ca.Status=1
END;
else IF(@CategoryID IS NULL AND @QrcId IS NULL)
BEGIN
--print 'both null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL AND ca.ContentType = 1
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL AND ca.ContentType = 2
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

ELSE IF( @QrcId IS NOT NULL AND @CategoryID IS NULL )
BEGIN
--print 'one null one not'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in(SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL AND ca.ContentType = 1
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in (SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL AND ca.ContentType = 2
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

IF(@CategoryID IS NOT NULL AND @QrcId IS NOT NULL)
BEGIN

--print 'both not null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.ContentType = 1
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.ContentType = 2
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

SET @query =
';with sqlpaging as (
SELECT 
Rownumber = ROW_NUMBER() OVER(order by  ContentApprovalID desc) ,
* FROM #tempData)

select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''

EXEC sp_executesql @query

SET @return = 105 -- reconrd does not exists
	  RETURN @return
END
