ALTER PROCEDURE [dbo].[Get_CitizenAffairList] --   [Get_CitizenAffairList] 1,10,4,5,0,null,null,null,null,null,null,'Test1'
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int =0,
	@P_Type nvarchar(250) = 0,
	@P_Method int =0,
	@P_RefNo nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_PersonalLocationName nvarchar(250) = null,
	@P_PhoneNo nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) =null,
	@P_RequestType nvarchar(max)= null,
	@P_SourceName nvarchar(max)= null,
	@P_Language nvarchar(10)= 'EN'
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
	declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	-- SET @P_SourceName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_SourceName)
	 SET @UserEmail = (Select [OfficialMailId] from [dbo].[UserProfile] where UserProfileId = @P_UserID)


	

	declare @RequestList table(
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	FromEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @RequestComplientList table(
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	FromEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	insert into @RequestComplientList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],ToEmail,FromEmail,[Status],DelegateToEmail
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] where Service = 'CAComplaintSuggestions'  group by ReferenceNumber ) 

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	CitizenAffairID int,
	ReferenceNumber nvarchar(max),
	RequestType nvarchar(max),
	LocationName nvarchar(max),
	PhoneNumber nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	AttendedBy nvarchar(250),
	AssignedTo nvarchar(255),
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	Reporter nvarchar(max))

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],ToEmail,FromEmail,[Status],DelegateToEmail
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] where Service = 'Citizen Affair'  group by ReferenceNumber ) 

	--select * from @RequestComplientList

	
	if(@P_Type = 1) -- new requests
	begin

	
	insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select F.LocationName from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )
			when RequestType=1 then
			(select P.Name from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end ),
			(select case when RequestType=0 then 
			(Select [PhoneNumber] from [dbo].[CitizenAffarFieldVisit] where [CitizenAffairID] = m.[CitizenAffairID])
			when RequestType=1 then
			(select P.PhoneNumber from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end),
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			null,
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			  (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and (Status = 58 or Status = 59) order by [WorkflowID] desc) 
			))
			as ApproverName,(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= m.InitalApproverDepartmentID) as ApproverDepartment
			,null
			from [dbo].[CitizenAffair] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and(( @UserEmail = r.ToEmail and r.WorkflowProcess !='DraftWorkflow' ) or
				(@UserEmail = r.FromEmail and 
				((r.WorkflowProcess='SubmissionWorkflow' 
				and ((select count(*) from Workflow a where a.ReferenceNumber=m.ReferenceNumber and a.WorkflowProcess='SubmissionWorkflow')=1)) 
					or r.WorkflowProcess='DraftWorkflow')))
				and (r.Status = 59 or r.Status=58)
			
	insert into @Result		 
			SELECT   m.CAComplaintSuggestionsID,m.ReferenceNumber,(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = 2),
			null as LocationName,
			PhoneNumber as PhoneNumber,
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end)  from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			case when m.CreatedBy =0 then (select top 1 case when @P_Language ='EN' then DisplayName else ArDisplayName end from M_Lookups where Category='System') else (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) end as Creator,
			(select (case when a.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.ToEmail) 
					when a.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  W where W.ReferenceNumber=m.ReferenceNumber and W.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by W.WorkflowID desc) as a),null ,
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,null,Source
			from [dbo].[CAComplaintSuggestions] m join @RequestComplientList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and 
			@P_UserID in (select [UserProfileId] from [dbo].[UserProfile] U where U.OrgUnitID in (5,6,7,8))  and r.Status = 51 --and r.WorkflowProcess='SubmissionWorkflow'
			
	end
	else if(@P_Type = 2) -- Need more info requests
	begin

	insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]) ,
			(select case when RequestType=0 then 
			(select F.LocationName from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )
			when RequestType=1 then
			(select P.Name from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end ),
			(select case when RequestType=0 then 
			(Select [PhoneNumber] from [dbo].[CitizenAffarFieldVisit] where [CitizenAffairID] = m.[CitizenAffairID])
			when RequestType=1 then
			(select P.PhoneNumber from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end),
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator, null,
			(select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			  (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and (Status = 58 or Status = 59) order by [WorkflowID] desc) 
			))
			as ApproverName,(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= m.InitalApproverDepartmentID) as ApproverDepartment
		,null
			from [dbo].[CitizenAffair] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and (@UserEmail =r.ToEmail and r.Status = 61) or (@UserEmail IN (select r.FromEmail from Workflow as r where r.ReferenceNumber = m.ReferenceNumber and r.Status = 61))
			
	end
	else if(@P_Type = 3) --Closed requests
	begin

		insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select F.LocationName from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )
			when RequestType=1 then
			(select P.Name from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end ),
			(select case when RequestType=0 then 
			(Select [PhoneNumber] from [dbo].[CitizenAffarFieldVisit] where [CitizenAffairID] = m.[CitizenAffairID])
			when RequestType=1 then
			(select P.PhoneNumber from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end),
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator, null,	
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			  (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and (Status = 58 or Status = 59) order by [WorkflowID] desc) 
			))
			as ApproverName,(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= m.InitalApproverDepartmentID) as ApproverDepartment
			,null
			from [dbo].[CitizenAffair] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and ((@UserEmail IN (select W.FromEmail from Workflow as W where W.ReferenceNumber = r.ReferenceNumber and ((W.Status = 60 or w.Status = 62) or w.WorkflowProcess='EscalateWorkflow'))) 
			or(@P_UserID=m.CreatedBy)) and (r.Status=60 or r.Status= 62)

	 insert into @Result
		SELECT   m.CAComplaintSuggestionsID,m.ReferenceNumber,(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = 2),
			null as LocationName,
			PhoneNumber as PhoneNumber,
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end)  from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			case when m.CreatedBy =0 then (select case when @P_Language ='EN' then DisplayName else ArDisplayName end from M_Lookups where Category='System') else (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) end as Creator,
			(select (case when a.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.ToEmail) 
					when a.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  W where W.ReferenceNumber=m.ReferenceNumber and W.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by W.WorkflowID desc) as a),null ,
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,null,Source
			from [dbo].[CAComplaintSuggestions] m join @RequestComplientList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  Where  m.[DeleteFlag] !=1
			and
			((@UserEmail IN (select W.FromEmail from Workflow as W where W.ReferenceNumber = r.ReferenceNumber and W.Status = 52)) OR (@P_UserID=M.CreatedBy AND R.Status=52))

			
	end
	
	else if(@P_Type = 5) --My Own Personal Requests
	begin	

	

	insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select F.LocationName from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )
			when RequestType=1 then
			(select P.Name from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end ),
			(select case when RequestType=0 then 
			(Select [PhoneNumber] from [dbo].[CitizenAffarFieldVisit] where [CitizenAffairID] = m.[CitizenAffairID])
			when RequestType=1 then
			(select P.PhoneNumber from [CitizenAffairPersonalReport] as P where P.CitizenAffairID=m.CitizenAffairID)
			end),
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			null,	
			(select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
			  (select top 1 ToEmail from [Workflow]
			 where [ReferenceNumber] = m.[ReferenceNumber] and [FromEmail] = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=m.CreatedBy) and (Status = 58 or Status = 59) order by [WorkflowID] desc) 
			))
			as ApproverName,(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= m.InitalApproverDepartmentID) as ApproverDepartment
			,null
			from [dbo].[CitizenAffair] m join @RequestList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
				and 
			--(@UserEmail in (select FromEmail from Workflow as W where W.ReferenceNumber=m.ReferenceNumber and W.WorkflowProcess='EscalateWorkflow') or
				((r.WorkflowProcess ='EscalateWorkflow'  and r.Status = 59 and @P_UserID=m.CreatedBy)
				or (@UserEmail = r.FromEmail and  (r.WorkflowProcess in('SubmissionWorkflow','EscalateWorkflow') and 
				((select count(*) from Workflow a where a.ReferenceNumber=m.ReferenceNumber and a.WorkflowProcess in('SubmissionWorkflow','EscalateWorkflow'))>1))))
				and (r.Status = 59 )

	end

	

	if(@P_RequestType is not null and @P_RequestType != 'All')
	delete  from @Result where RequestType != @P_RequestType or RequestType is null

	if(@P_RefNo != '')
	 delete from @result where ReferenceNumber not like '%'+@P_RefNo+'%' or ReferenceNumber is null

	 if(@P_PersonalLocationName != '')
	 delete from @result where LocationName not like '%'+@P_PersonalLocationName+'%' or LocationName is null

	 if(@P_PhoneNo != '')
	 delete from @result where PhoneNumber not like '%'+@P_PhoneNo+'%' or PhoneNumber is null

	  if(@P_SourceName != '' and @P_SourceName is not null)
	 delete from @result where SourceName not like '%'+@P_SourceName+'%' or SourceName is null

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='' and @P_Status != 'All')
	 begin
		delete from @result where Status !=  @P_Status or Status is null
	 end

	 --select * from @Result

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	begin
		select  row_number() over (Order By RequestDate desc) as slno,* from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or(RequestType  like '%'+@P_SmartSearch+'%') or 
		 (Status  like '%'+@P_SmartSearch+'%') or (LocationName like '%'+@P_SmartSearch+'%') or
		  ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')   
		or (PhoneNumber  like '%'+@P_SmartSearch+'%')or(SourceName like '%'+@P_SmartSearch+'%')or(AssignedTo like '%'+@P_SmartSearch+'%')
		or(SourceOU like '%'+@P_SmartSearch+'%')or(ApproverDepartment like '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%') or((Select Subject from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		 or((Select c.Source from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')or ((Select Details from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		or( (select (case when m.Type=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=172))
			when m.Type=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=173))
			end) from CAComplaintSuggestions as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
		or( (select (case when m.RequestCreatedBy=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=171))
			else (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=226) end) from CAComplaintSuggestions as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
		 or((select case when @P_Language='EN' then L.DisplayName else L.ArDisplayName end from M_Lookups as L where L.Module='CitizenAffair' and  L.Category = (Select c.NotifyUpon from CitizenAffair as c where c.ReferenceNumber=a.ReferenceNumber)) like '%'+@P_SmartSearch+'%')
		 or ((Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile join CitizenAffair as c on UserProfileId=c.InternalRequestorID where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
			or((Select  (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department join CitizenAffair as c on DepartmentID=c.InternalRequestorDepartmentID where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		or((Select  (case when c.RequestType=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=152))
			when c.RequestType=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=153))
			end )from CitizenAffair as c  where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')  or((Select c.ExternalRequestEmailID from CitizenAffair as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')
		 or((Select c.EmiratesID from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		 or((Select c.FindingNotes from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.RequetsedBy from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.Name from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.PhoneNumber from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	   or((Select c.VisitObjective from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	   or((Select CONVERT(nvarchar(10), cast(c.Date as date), 103) from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	  or ((select count(K.Location) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit' and (select (case when @P_Language ='EN' then LocationName else ArLocationName end) from M_Location as d where d.LocationID= K.Location )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.City) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit' and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.LocationID) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit'  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.LocationID )like '%'+@P_SmartSearch+'%')>0)
     or ((select count(K.LocationName) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID   and a.RequestType='FieldVisit'and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where CONVERT(nvarchar,e.EmiratesID)= CONVERT(nvarchar,K.LocationName) )like '%'+@P_SmartSearch+'%')>0)
	 or((select (case when @P_Language ='EN' then L.DisplayName else L.ArDisplayName end) from M_Lookups as L where L.Category = (Select c.ForWhom from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit')) like '%'+@P_SmartSearch+'%')		   
    or((Select c.Name from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
    or((Select c.Age from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
   or((Select c.FindingNotes from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.MaritalStatus from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.MonthlySalary from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.NoOfChildrens from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.Recommendation from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.PhoneNumber from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.Destination from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.ReportObjectives from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or ((select count(K.City) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report'  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	or ((select count(K.Emirates) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report' and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where e.EmiratesID= K.Emirates )like '%'+@P_SmartSearch+'%')>0)
	 or ((Select c.Employer from CitizenAffairPersonalReport as c  where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%') 
	or((Select c.EmiratesID from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
		))as m where  m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or(RequestType  like '%'+@P_SmartSearch+'%') or 
		 (Status  like '%'+@P_SmartSearch+'%') or (LocationName like '%'+@P_SmartSearch+'%') or
		  ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')   
		or (PhoneNumber  like '%'+@P_SmartSearch+'%')or(SourceName like '%'+@P_SmartSearch+'%')or(AssignedTo like '%'+@P_SmartSearch+'%')
		or(SourceOU like '%'+@P_SmartSearch+'%')or(ApproverDepartment like '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%') or((Select Subject from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		 or((Select c.Source from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')or ((Select Details from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		or( (select (case when m.Type=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=172))
			when m.Type=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=173))
			end) from CAComplaintSuggestions as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
		or( (select (case when m.RequestCreatedBy=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=171))
			else (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=226) end) from CAComplaintSuggestions as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
		or((select case when @P_Language='EN' then L.DisplayName else L.ArDisplayName end from M_Lookups as L where L.Module='CitizenAffair' and  L.Category = (Select c.NotifyUpon from CitizenAffair as c where c.ReferenceNumber=a.ReferenceNumber)) like '%'+@P_SmartSearch+'%')
		or ((Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile join CitizenAffair as c on UserProfileId=c.InternalRequestorID where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
			or((Select  (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department join CitizenAffair as c on DepartmentID=c.InternalRequestorDepartmentID where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		or((Select  (case when c.RequestType=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=152))
			when c.RequestType=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=153))
			end )from CitizenAffair as c  where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')  or((Select c.ExternalRequestEmailID from CitizenAffair as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')
		 or((Select c.EmiratesID from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		 or((Select c.FindingNotes from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.RequetsedBy from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.Name from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
		or((Select c.PhoneNumber from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	   or((Select c.VisitObjective from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	   or((Select CONVERT(nvarchar(10), cast(c.Date as date), 103) from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		
	  or ((select count(K.Location) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit' and (select (case when @P_Language ='EN' then LocationName else ArLocationName end) from M_Location as d where d.LocationID= K.Location )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.City) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit' and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.LocationID) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit'  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.LocationID )like '%'+@P_SmartSearch+'%')>0)
     or ((select count(K.LocationName) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID   and a.RequestType='FieldVisit'and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where CONVERT(nvarchar,e.EmiratesID)= CONVERT(nvarchar,K.LocationName) )like '%'+@P_SmartSearch+'%')>0)
	 or((Select c.ForWhom from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='FieldVisit') like '%'+@P_SmartSearch+'%')		   
    or((Select c.Name from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
    or((Select c.Age from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
   or((Select c.FindingNotes from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.MaritalStatus from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.MonthlySalary from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	 or((Select c.NoOfChildrens from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.Recommendation from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.PhoneNumber from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.Destination from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or((Select c.ReportObjectives from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
	or ((select count(K.City) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report'  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	or ((select count(K.Emirates) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report' and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where e.EmiratesID= K.Emirates )like '%'+@P_SmartSearch+'%')>0)
	 or ((Select c.Employer from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%') 
	or((Select c.EmiratesID from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType='Personal Report') like '%'+@P_SmartSearch+'%')		
		)
	 end

	if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result) as m
	  where slno between @StartNo and @EndNo 


	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END