
ALTER PROCEDURE [dbo].[Get_MeetingPreview] -- [Get_MeetingPreview] 44,2,'EN'
	-- Add the parameters for the stored procedure here
	@P_MeetingID int = null,
	@P_UserID int = null,
	@P_Language nvarchar(10)= null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select [ReferenceNumber],
	(select IIF(@P_Language = 'EN', DepartmentName, ArDepartmentName) from M_Department where DepartmentID= OrganizerDepartmentID ) as OrganizerDepartmentID,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId=OrganizerUserID ) as OrganizerUserID,
	subject,Location,
	(select STUFF((

select (
    select ','+ContactPerson AS [text()]  from (
        select ContactPerson 
        from MeetingExternalInvitees where MeetingID=c.MeetingID
        UNION
        select CAST(IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].[UserProfile] where [UserProfileId] in 
	(select UserID from [dbo].MeetingInternalInvitees where MeetingID =c.MeetingID)
    ) A
    FOR XML PATH ('')
) ), 1, 1, NULL) )as Attendees,

 

    StartDateTime,EndDateTime,(select PointsDiscussed from MOM as a where a.MeetingID=c.MeetingID) as PointsDiscussed,
    (select PendingPoints from MOM as a where a.MeetingID=c.MeetingID) as PendingPoints,
    (select Suggestion from MOM as a where a.MeetingID=c.MeetingID) as Suggestion,CreatedBy  from [dbo].Meeting as c where c.MeetingID = @P_MeetingID and @P_UserID=c.CreatedBy 
    or @P_UserID in (select UserID from MeetingInternalInvitees as d where d.MeetingID=@P_MeetingID ) 
END