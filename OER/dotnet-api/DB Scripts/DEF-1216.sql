ALTER TRIGGER [dbo].[UpdateCourseLastActionDate] 
   ON  [dbo].[CourseMaster] 
   after UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CourseId int =0;


	select  @CourseId=Id from inserted;

	IF (update(IsApproved) or UPDATE(ViewCount) or UPDATE(SharedCount) or UPDATE(DownloadCount))
	BEGIN
    UPDATE CourseMaster
    SET [LastActionDate] = GETDATE()
    where id = @CourseId
	END;
END

GO

ALTER TRIGGER [dbo].[UpdateResourceLastActionDate] 
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

	IF (update(IsApproved) or UPDATE(ViewCount) or UPDATE(SharedCount) or UPDATE(DownloadCount))
	BEGIN
    UPDATE ResourceMaster
    SET [LastActionDate] = GETDATE()
    where id = @ResourseId
	END;
END

