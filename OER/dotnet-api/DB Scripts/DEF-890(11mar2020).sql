ALTER PROCEDURE [dbo].[sps_GetUserNotInQRC]
(
@QrcId int = NULL,
@Category Int = NULL,
@PageNo int = 1,
@PageSize int = 5 ,
@FilterCategoryId INT = NULL
)

AS
BEGIN
SET ANSI_NULLS ON
Declare @return INT
--select top 30 ID, FirstName + ' ' + LastName as UserName
--,10 as ResourceContributed
--,0 as CourseCreated
--,5 as CurrentQRCS
--,9 as NoOfReviews from UserMaster where IsContributor = 1 AND FirstName <> '' AND LastName <>''

DECLARE @query NVARCHAR(MAX)


declare @start int, @end int
set @start = (@PageNo - 1) * @PageSize + 1
set @end = @PageNo * @PageSize


--IF (@FilterCategoryId IS NOT NULL)
--BEGIN
-- --Select 'test'
--END

--ELSE

--BEGIN
---- Select 'test2'
--END
IF (@FilterCategoryId IS NULL OR @FilterCategoryId = 0 )
BEGIN
SET @query =
';with sqlpaging as (
SELECT
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,
u.Email,
u.Photo
,COUNT(rm.Id) as ResourceContributed,
COUNT(c.Id) as CourseCreated
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS
,(SELECT count(ID) from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews
,SubjectsInterested
from
UserMaster u
LEFT JOIN ResourceMaster rm
ON u.id = rm.CreatedBy
LEFT JOIN CourseMaster c
ON u.id = c.CreatedBy
WHERE
u.id not in (select userid from qrcusermapping where active=1 and CategoryId='+CAST(@Category AS VARCHAR(50))+') AND u.IsContributor = 1
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)


select
top ('+CAST(@PageSize AS VARCHAR(50))+') *,
(select max(rownumber) from sqlpaging) as
Totalrows
from sqlpaging
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''
END
ELSE
BEGIN
SET @query =
';with sqlpaging as (
SELECT
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,
u.Email,
u.Photo
,COUNT(rm.Id) as ResourceContributed,
COUNT(c.Id) as CourseCreated
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS
,(SELECT count(ID) from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews
,SubjectsInterested
from
UserMaster u LEFT JOIN ResourceMaster rm
ON u.id = rm.CreatedBy
LEFT JOIN CourseMaster c
ON u.id = c.CreatedBy
WHERE
u.id not in (select userid from qrcusermapping where active=1 AND CategoryId='+CAST(@Category AS VARCHAR(50))+') and u.IsContributor = 1
AND CHARINDEX((SELECT distinct Value FROM STRING_SPLIT(SubjectsInterested,'','') WHERE Value IN
(SELECT Name from CAtegoryMaster WHERE Status=1 and Active=1 and Id = '+CAST(@FilterCategoryId AS VARCHAR(50))+')),SubjectsInterested) > 0
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)

select
top ('+CAST(@PageSize AS VARCHAR(50))+') *,
(select max(rownumber) from sqlpaging) as
Totalrows
from sqlpaging
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))

END

EXEC sp_executesql @query

SET @return = 105 -- reconrd does not exists
RETURN @return
END