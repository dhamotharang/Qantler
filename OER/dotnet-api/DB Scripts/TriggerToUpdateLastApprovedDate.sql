alter table ResourceMaster add LastActionDate datetime null
alter table CourseMaster add LastActionDate datetime null
GO

Update CourseMaster set LastActionDate = getdate() where IsApproved  is not null
Update ResourceMaster set LastActionDate = getdate() where IsApproved is not null

Go

Create TRIGGER [dbo].[UpdateCourseLastActionDate] 
   ON  [dbo].[CourseMaster] 
   after UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CourseId int =0;


	select  @CourseId=Id from inserted;

	IF update(IsApproved)  
	BEGIN
    UPDATE CourseMaster
    SET [LastActionDate] = GETDATE()
    where id = @CourseId
	END;
END
Go

Create TRIGGER [dbo].[UpdateResourceLastActionDate] 
   ON  [dbo].[ResourceMaster] 
   after UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @ResourseId int =0;
	declare @OldIsApproved bit = null;
	declare @newIsApproved bit = null;

	select @OldIsApproved=IsApproved from deleted;
	select  @ResourseId=Id,@newIsApproved=IsApproved from inserted;

	IF update(IsApproved) 
	BEGIN
    UPDATE ResourceMaster
    SET [LastActionDate] = GETDATE()
    where id = @ResourseId
	END;
END
GO

ALTER  VIEW [dbo].[VCourseMaster]  
AS  
select cm.Id,
cm.Title,
cm.CategoryId,
cat.Name as Category,
cm.SubCategoryId,
cm.Thumbnail,
cm.CourseDescription,
cm.Keywords,
cm.CourseContent,
cm.CopyRightId,
(SELECT Title FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightTitle,
(SELECT Title_Ar FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightTitle_Ar,
(SELECT Media FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightMedia,
(SELECT [Description] FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightDescription,
(SELECT Description_Ar FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightDescription_Ar,
cm.IsDraft,
u.FirstName +' '+ u.LastName as CreatedByName,
cm.CreatedBy,
cm.CreatedOn,
cm.IsApproved,
cm.Rating,
cm.ReportAbuseCount,
cm.EducationId,
cm.ProfessionId,
cm.ViewCount,
cm.AverageReadingTime,
cm.DownloadCount,
cm.SharedCount,
cm.ReadingTime,
cm.LastView,
cm.LevelId,
cat.Name_Ar,
cat.Name,
(SELECT Count(CourseId) from CourseEnrollment WHERE Courseid = cm.Id and Active = 1) As enrollmentcount
,(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As NoOfUsers
from [CourseRating] where courseid = cm.Id
GROUP BY Rating FOR JSON AUTO) AS AllRating,
cm.EducationalStandardId,
cm.EducationalUseId,
cm.CommunityBadge,
cm.MoEBadge,
cm.LastActionDate from CourseMaster cm
INNER JOIN UserMaster u ON cm.CreatedBy = u.id
INNER JOIN CategoryMaster cat ON cm.CategoryId = cat.Id
GO


ALTER VIEW [dbo].[VResourceMaster]  
AS  

SELECT 
rm.Id,
rm.Title,
rm.CategoryId,
rm.SubCategoryId,
rm.Thumbnail,
rm.ResourceDescription,
rm.Keywords,
rm.ResourceContent,
rm.MaterialTypeId,
rm.CopyRightId,
(SELECT Title FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightTitle,
(SELECT Title_Ar FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightTitle_Ar,
(SELECT Media FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightMedia,
(SELECT [Description] FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightDescription,
(SELECT Description_Ar FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightDescription_Ar,
rm.IsDraft,
rm.CreatedBy,
rm.CreatedOn,
rm.IsApproved,
rm.Rating,
rm.AlignmentRating,
rm.ReportAbuseCount,
rm.UrlSegment,
rm.ViewCount,
rm.AverageReadingTime,
rm.DownloadCount,
rm.SharedCount,
rm.ReadingTime,
rm.Objective,
rm.LevelId,
cat.Name_Ar,
cat.Name,
(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As NoOfUsers
from [ResourceRating] where resourceid = rm.Id
GROUP BY Rating FOR JSON AUTO) AS AllRating,
rm.EducationalStandardId,
rm.EducationalUseId,
rm.[Format],
rm.LastView,
cat.Name as Category,
u.FirstName +' '+ u.LastName as CreatedByName,
rm.MoEBadge,
rm.CommunityBadge,
rm.LastActionDate from ResourceMaster rm
INNER JOIN UserMaster u ON rm.CreatedBy = u.id
INNER JOIN CategoryMaster cat ON rm.CategoryId = cat.Id
GO