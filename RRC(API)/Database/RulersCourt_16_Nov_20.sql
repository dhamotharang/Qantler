
ALTER PROCEDURE [dbo].[Save_Notification]

	@P_ReferenceNumber nvarchar(250) = null,	
	@P_Service nvarchar(250) = null,	
	@P_ServiceID int = 0,	
	@P_WorkflowProcess nvarchar(250) = null,
	@P_FromName nvarchar(250) = null,
	@P_FromEmail nvarchar(250) = null,
	@P_ToName nvarchar(250) = null,
	@P_ToEmail nvarchar(250) = null,
	@P_Status int = null	,
	@P_DelegateFromEmail nvarchar(255) = null,
	@P_DelegateFromName nvarchar(255) = null,
	@P_DelegateToEmail nvarchar(255) = null,
	@P_DelegateToName nvarchar(255) = null,
	@P_IsAnonymous bit = 0

AS
BEGIN

		IF(@P_WorkflowProcess='HRComplaintSuggestions' AND @P_WorkflowProcess ='SubmissionWorkflow')
		begin
			if((SELECT RequestCreatedBy from HRComplaintSuggestions where HRComplaintSuggestionsID=@P_ServiceID )= 'true')
			SET @P_IsAnonymous = 1;
		end
		if(@P_Service = 'Circular' and @P_WorkflowProcess = 'ApprovalWorkflow')
		begin
			insert into Notification (ServiceID,[ReferenceNumber],Service,Process,LastUpdateDate,IsRead,FromEmail,ToEmail,DelegateToEmail,DelegateFromEmail,IsAnonymous)
			select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,u.OfficialMailId,@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous			
			from UserProfile u where DepartmentID = (select DepartmentID from M_Department d where d.GroupMailID=@P_ToEmail and @P_ToEmail is not null)
			
		end
		if(@P_Service = 'Vehicle' and (@P_WorkflowProcess = 'ApprovalWorkflow' or @P_WorkflowProcess='VehicleReleaseConfirmWorkflow' or @P_WorkflowProcess='VehicleReturnConfirmWorkflow' or (@P_WorkflowProcess='AssignWorkflow' and (select count(*) from [dbo].[Organization] where GroupMailID = @P_ToEmail and @P_ToEmail is not null) > 0) or @P_WorkflowProcess='VehicleReleaseConfirmationRejectWorkflow' or ((@P_WorkflowProcess='VehicleReleaseWorkflow' or @P_WorkflowProcess='VehicleReturnWorkflow') 
		and ((select count(distinct ReferenceNumber) from [VehicleRequest] where ReferenceNumber=@P_ReferenceNumber and RequestType in (2,3)) <= 0))))
		begin
			insert into Notification (ServiceID,[ReferenceNumber],Service,Process,LastUpdateDate,IsRead,FromEmail,ToEmail,DelegateToEmail,DelegateFromEmail,IsAnonymous)
			select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,u.OfficialMailId,@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous			
			from UserProfile u where OrgUnitID=(select OrganizationID from [dbo].[Organization] where GroupMailID = @P_ToEmail and @P_ToEmail is not null)
			
		end
		else
		begin
			insert into Notification (ServiceID,[ReferenceNumber],Service,Process,LastUpdateDate,IsRead,FromEmail,ToEmail,DelegateToEmail,DelegateFromEmail,IsAnonymous)
			--select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,(select OfficialMailId from UserProfile where DepartmentID=(select DepartmentID from M_Department where OfficialMailId=@P_ToEmail and @P_ToEmail is not null)),@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous				
			
			select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,@P_ToEmail,@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous				
			
		end
END

GO

/****** Object:  StoredProcedure [dbo].[Get_HomeModulesCount]   ******/
ALTER PROCEDURE [dbo].[Get_HomeModulesCount] -- Get_HomeModulesCount 4
	-- Add the parameters for the stored procedure here
	@P_UserID int = 0,
	@P_Language nvarchar(10)= null
	
AS
BEGIN

	--Circular = 1, Meeting = 2, Letter = 3, Duty Task = 4, Memo = 5
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)

declare @Memo_Count int = 0, @Letters_Count int = 0, @DutyTask_Count int = 0, @Circular_Count int = 0, @Meeting_Count int = 0, 
@NextMeetingCount datetime = null, @MeetingID int =0

 

      declare @RequestList table(
    
    ReferenceNumber nvarchar(max),
    --id int,
    --Creator nvarchar(max),
    WorkflowProcess nvarchar(max),
    FromName nvarchar(250),
    ToName nvarchar(250),
    FromEmail nvarchar(250),
    ToEmail nvarchar(250),
    Status nvarchar(250),
    DelegateToEmail nvarchar(250))

 

    insert into @RequestList
    select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[FromEmail],[ToEmail],[Status],DelegateToEmail
    from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
    from [dbo].[Workflow] group by ReferenceNumber) 

	--select * from @RequestList

	declare @Output table(
	id INT IDENTITY(1,1) primary key,
	Modules nvarchar(250),
	Memo_Count int,
	Letters_Count int,
	DutyTask_Count int,
	Circular_Count int,
	Meeting_Count int)

			 declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		Status int,
		DelegateTOEmail nvarchar(max),
		FromMail nvarchar(max)
		);

		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,Status,DelegateTOEmail,FromEmail from [dbo].[Workflow] 

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1


	set  @Circular_Count = 
	(select count(*) FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where ( a.DeleteFlag = 0 and w.Status = 14 
   and (   (@P_UserID in (select UserProfileId from UserProfile where DepartmentID in 
	(select [DepartmentID] from [dbo].[CircularDestinationDepartment] where [CircularID]=a.CircularID and DeleteFlag =0))))) 
	or ((a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) and 
                 ( w.Status = 13 or w.Status=16) and (w.ToEmail = @UserEmail
    or 
      @UserEmail = w.DelegateTOEmail) ) )
	
	--(select count(*) from [Circular] where ReferenceNumber in(
 --                 SELECT c.ReferenceNumber from [dbo].[Circular] as  c inner join @RequestList as r on c.ReferenceNumber = r.ReferenceNumber  where (c.[DeleteFlag] is not null or c.[DeleteFlag] !=0) and 
 --                ( r.Status = 13 or r.Status=16) and (r.ToEmail = @UserEmail
 --   or 
 --     @UserEmail = r.DelegateTOEmail) ))

    set  @Letters_Count = (select count(*) from [LettersInbound] where [LetterReferenceID] in(
                  SELECT LI.[LetterReferenceID] from [dbo].[LettersInbound] as  LI inner join @RequestList as r on LI.[LetterReferenceID] = r.ReferenceNumber  where (LI.[DeleteFlag] is not null or LI.[DeleteFlag] !=0) and 
                  r.Status = 25 and (r.ToEmail = @UserEmail
    or 
      @UserEmail = r.DelegateTOEmail) ))  + 

						(select count(*) from [LettersOutbound] where [LetterReferenceID] in(
                  SELECT LO.[LetterReferenceID] from [dbo].[LettersOutbound] as  LO inner join @RequestList as r on LO.[LetterReferenceID] = r.ReferenceNumber  where (LO.[DeleteFlag] is not null or LO.[DeleteFlag] !=0) and 
                  r.Status = 19 and (r.ToEmail = @UserEmail
    or 
      @UserEmail = r.DelegateTOEmail) ))

 

    set  @DutyTask_Count = (select count(*) from [DutyTask] where Status = 30 and AssigneeID= @P_UserID or DelegateAssignee=@P_UserID)

	set  @Memo_Count = (select count(*) from [Memo] where ReferenceNumber in(
                  SELECT m.ReferenceNumber from [dbo].[Memo] as m inner join @RequestList as r on m.ReferenceNumber = r.ReferenceNumber  where (m.[DeleteFlag] is not null or m.[DeleteFlag] !=0) and 
                 (r.Status = 2 or r.Status=5)and (r.ToEmail = @UserEmail
    or 
      @UserEmail = r.DelegateTOEmail)
     ))

		set @Meeting_Count = (select count(*)  from [dbo].[Meeting] as m where  DeleteFlag=0 and ( m.Status <>115 or m.Status is null) and  StartDateTime > (Select GETUTCDATE()) AND DeleteFlag=0 and ( (@P_UserID = m.CreatedBy) or 
						@P_UserID in (select UserID from MeetingInternalInvitees where MeetingID=m.MeetingID )))

		set @NextMeetingCount = (select top (1)StartDateTime from [dbo].[Meeting] as m where  DeleteFlag=0 and ( (@P_UserID = m.CreatedBy) or 
						@P_UserID in (select UserID from MeetingInternalInvitees where MeetingID=m.MeetingID )) and( m.Status <>115 or m.Status is null)
						and StartDateTime > (Select GETUTCDATE()) order by StartDateTime asc  ) 

		set @MeetingID = (select top (1)MeetingID from [dbo].[Meeting] as m where  DeleteFlag=0 and ( (@P_UserID = m.CreatedBy) or 
						@P_UserID in (select UserID from MeetingInternalInvitees where MeetingID=m.MeetingID )) and ( m.Status <>115 or m.Status is null)
						and StartDateTime > (Select GETUTCDATE()) order by StartDateTime asc  ) 

	select @Circular_Count as Circular, @Letters_Count as Letters, @Memo_Count as Memo, @Meeting_Count as Meeting, @DutyTask_Count as DutyTask,
		@NextMeetingCount as NextMeetingDateTime, @MeetingID as MeetingID

END

GO

ALTER PROCEDURE [dbo].[Get_CompensationPreview]  --   Get_CompensationPreview 91
	-- Add the parameters for the stored procedure here
	@P_CompensationID int = 0,
	@P_Language nvarchar(10)= null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   declare @result table(
id int identity(1,1),
OfficialTaskReferenceNo nvarchar(255),
CompensationReferenceNo nvarchar(255),
OfficialTaskDescription nvarchar(max),
StartDate datetime,
OfficialTaskCreatorName nvarchar(255),
OfficialTaskCreatorDesignation nvarchar(255),
AssigneeName nvarchar(255),
AssigneeEmployeeID nvarchar(255),
NoOfDays int,
EmployeeDetails nvarchar(max)
)

declare @Assignee nvarchar(max) = null;

select @Assignee = (case when q.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = q.ToEmail) 
					when q.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b 
					where b.ReferenceNumber=(select ReferenceNumber from Compensation where CompensationID=1) and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as q

declare @EmployeeDetail table(
id int identity(1,1),
name nvarchar(max));

insert into @EmployeeDetail
select concat(b.EmployeeCode,'/',IIF(@P_Language = 'EN', b.EmployeeName, b.AREmployeeName)) from [dbo].[UserProfile] as b where b.UserProfileId in (select a.UserID from [dbo].[OfficialTaskEmployeeName] as a where a.OfficialTaskID=(select OfficialTaskID from Compensation where CompensationID=@P_CompensationID))

DECLARE @EmployeeDetailList NVARCHAR(max);

SELECT @EmployeeDetailList = COALESCE(@EmployeeDetailList + ',', '') + CAST(name AS NVARCHAR)
FROM @EmployeeDetail

insert into @result
select O.ReferenceNumber,C.ReferenceNumber,O.OfficialTaskDescription,C.StartDate,(select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile as UP where UP.UserProfileID = O.CreatedBy)
,(select IIF(@P_Language = 'EN', UP.EmployeePosition, UP.AREmployeePosition) from UserProfile as UP where UP.UserProfileID = O.CreatedBy) ,(@Assignee)
,(select Up.EmployeeCode from UserProfile as UP where IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) = @Assignee),C.NumberofDays,@EmployeeDetailList from Compensation as C inner join OfficialTask as O on C.OfficialTaskID=O.OfficialTaskID 
 where C.CompensationID = @P_CompensationID

select * from @result

END


