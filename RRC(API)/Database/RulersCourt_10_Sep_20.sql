GO
/****** Object:  StoredProcedure [dbo].[Get_MaintenanceList]   ******/
ALTER PROCEDURE [dbo].[Get_MaintenanceList] -- [Get_MaintenanceList] 1,100,12,5,0
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int =0,
	@P_Type nvarchar(250) = null,
	@P_Method int =0,
	@P_UserName nvarchar(250) = null,
	@P_SourceOU nvarchar(250) = null,
	@P_Subject nvarchar(250) = null,
	@P_Priority nvarchar(250) = null,
	@P_AttendedBy nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null	,
	@P_Language nvarchar(max) = null





AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;
	--declare @UserID int = 2
	declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)

	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)
	 set @P_SourceOU =(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from [dbo].[M_Department] where DepartmentID=@P_SourceOU)
	 set @P_Status =(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where [LookupsID]=@P_Status)
	 declare @MaintenanceManagers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  declare @MaintenanceUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @MaintenanceManagers
	  select [UserProfileId],OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 12 and ([IsOrgHead] = 1 or HOD = 1 or HOS = 1)
	  --select * from @HRManagers
	  --GET HR users
	  insert into @MaintenanceUsers
	  select [UserProfileId],OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 12 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	  	declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromEmail nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	MaintenanceID int,
	ReferenceNumber nvarchar(max),
	Source nvarchar(max),
	Subject nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	AttendedBy nvarchar(250),
	Priority nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	Details nvarchar(max),
	Requestor nvarchar(max),
	RequestorDepartment nvarchar(max),
	AssignedTo nvarchar(max))

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromEmail],[ToEmail],[Status],DelegateToEmail
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] where Service = 'Maintenance' group by ReferenceNumber) 

	--select * from @RequestList

	if(@P_Type = 1) --New requests
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],
			   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,	m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
				else  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2)  end,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and (((r.Status = 53 and @UserEmail = r.ToEmail)or(( @UserEmail = r.ToEmail
	       or @UserEmail = r.DelegateTOEmail)and r.Status=53)) or (@P_UserID = m.CreatedBy and r.WorkflowProcess='SubmissionWorkflow')
		   or (r.Status =55 and m.IsMaintenanceHeadApproved =1 and @P_UserID in (select UserProfileId from UserProfile where OrgUnitID= 12))
		   or (r.Status =55 and m.IsMaintenanceHeadApproved is null and @P_UserID in (select UserProfileId from UserProfile where OrgUnitID= 12 and ([IsOrgHead] = 1 or HOD = 1 or HOS = 1)))
		   ) order by m.[CreatedDateTime] desc
			
	end
	else if(@P_Type = 2) --Need more info
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
			 else (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end,
			 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
		(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and ((r.WorkflowProcess='ReturnWorkflow' and @UserEmail = r.ToEmail and r.Status = 54) or
				(@UserEmail in(select FromEmail from Workflow as W where W.WorkflowProcess='ReturnWorkflow' and W.ReferenceNumber=r.ReferenceNumber )))
			order by m.[CreatedDateTime] desc
	end
	else if(@P_Type = 3) --closed requests
	begin

	insert into @Result
			SELECT m.MaintenanceID,  m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0)  
			else(select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
		(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and  ((@UserEmail = r.FromEmail or @P_UserID = m.CreatedBy)  and( r.Status = 57 or r.Status=56))
			order by m.[CreatedDateTime] desc
	end
	else if(@P_Type = 4) -- My pending Actions for RRC employees
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
			 else (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end,
			 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
		(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and ((( @UserEmail = r.ToEmail or @UserEmail = r.DelegateTOEmail)and (r.Status=53 or r.Status=54))
			)
			order by m.[CreatedDateTime] desc
		--	select * from M_Loockups
	end
	
	else if(@P_Type = 5) --My own personal requests
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
			 else (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end,
			 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
	(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and m.CreatedBy = @P_UserID and r.WorkflowProcess!='ReturnWorkflow'
			order by m.[CreatedDateTime] desc
	end
	else if(@P_Type = 6	) --My Processed Request
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
			 else (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end
			 ,
			 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
				( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and @UserEmail in (select W.FromEmail from Workflow as W where m.ReferenceNumber = w.ReferenceNumber and W.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow' ))
			order by m.[CreatedDateTime] desc
	end


	else if(@P_Type = 7	) --InProgressRequest
	begin

	insert into @Result
			SELECT  m.MaintenanceID, m.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.SourceOU) as SourceOU ,m.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			case when (m.[Priority] = 1) then  (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=0) 
			 else (select (case when @P_Language ='EN' then DisplayName else ArDisplayname end) from M_lookups where Category='priority' and Module=2) end
			 ,
			 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.SourceName)  as sourceName,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and Status = 53 order by [WorkflowID] desc) 
			 ))
			as ApproverName,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.ApproverDepartment) as ApproverDepartment,
			m.Details,(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=m.RequestorID) as Requestor,
				( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = m.RequestorDepartmentID) as RequestorDepartment,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=m.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Maintenance] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and ((@P_UserID= m.CreatedBy and r.WorkflowProcess not in ('SubmissionWorkflow' ,'ReturnWorkflow')))
			--or( @UserEmail in (select W.FromEmail from Workflow as W where m.ReferenceNumber = w.ReferenceNumber and W.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow' ))))
			order by m.[CreatedDateTime] desc
	end
	--select * from @Result

	 if(@P_SourceOU != '')
	 delete from @result where Source not like '%'+@P_SourceOU+'%'

	 if(@P_Subject != '')
	 delete from @result where Subject not like '%'+@P_Subject+'%'

	 if(@P_Priority != '')
	 delete from @result where Priority not like '%'+@P_Priority+'%'

	 if(@P_AttendedBy!= '')
	 delete from @result where AttendedBy not like '%'+@P_AttendedBy+'%'

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
		select * from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or
		(Source  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Subject like '%'+@P_SmartSearch+'%')
		or (AttendedBy like '%'+@P_SmartSearch+'%')or (AssignedTo like '%'+@P_SmartSearch+'%')
		or ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  
		or (Priority  like '%'+@P_SmartSearch+'%') or (SourceName like '%'+@P_SmartSearch+'%')or (ApproverName like '%'+@P_SmartSearch+'%')or (ApproverDepartment like '%'+@P_SmartSearch+'%')
		or (Details like '%'+@P_SmartSearch+'%')or (Requestor like '%'+@P_SmartSearch+'%')or (RequestorDepartment like '%'+@P_SmartSearch+'%')) and id between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or
		(Source  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Subject like '%'+@P_SmartSearch+'%')
		or (AttendedBy like '%'+@P_SmartSearch+'%')or (AssignedTo like '%'+@P_SmartSearch+'%')
		or ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  
		or (Priority  like '%'+@P_SmartSearch+'%')or (SourceName like '%'+@P_SmartSearch+'%')or (ApproverName like '%'+@P_SmartSearch+'%')or (ApproverDepartment like '%'+@P_SmartSearch+'%')
		or (Details like '%'+@P_SmartSearch+'%')or (Requestor like '%'+@P_SmartSearch+'%')or (RequestorDepartment like '%'+@P_SmartSearch+'%')) 

		end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from @result where id between @StartNo and @EndNo 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result

END


GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
    ALTER PROCEDURE [dbo].[Get_CalendarHistoryByID]  -- [Get_CalendarHistoryByID] 122
    -- Add the parameters for the stored procedure here
    @P_CalendarID int = null,
	@P_Language nvarchar(10)= 'EN'
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
        SELECT CalendarID,[Action],[CommunicationId],[ParentCommunicationID],
    case when (Action != 'Reject' or Action is null)
        then (case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.CreatedBy)
             else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), ' on behalf of ',
             (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end)
    when (Action = 'Reject' and (select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID ) = 1 )
        then concat((case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.CreatedBy)
             else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), ' on behalf of ',
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),' With Apologies Letter')
    when (Action = 'Reject' and ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID ) is null) or ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID )= 0) )
        then concat((case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.CreatedBy)
             else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), ' on behalf of ',
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),' Without Apologies Letter') end   
    as
     CreatedBy,
        [CreatedDateTime],Comment from [dbo].CalendarCommunicationHistory as a where a.CalendarID = @P_CalendarID
END