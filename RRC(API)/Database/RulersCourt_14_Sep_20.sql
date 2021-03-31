GO

/****** Object:  StoredProcedure [dbo].[CitizenAffairReport]     ******/
ALTER PROCEDURE [dbo].[CitizenAffairReport]    --[CitizenAffairReport] 0,0,0,1,null,null,null,null,null,null,null,'AR - Test User 2',null,'AR'   -- [CitizenAffairReport] 0,0,0,1,null,null,null,null,null,null,null,'TestUser2',null,'EN'
	-- Add the parameters for the stored procedure here
	 @P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Method int =0,
	@P_UserID int = 0,
	@P_Status nvarchar(255) = '',
	@P_RequestType nvarchar(255) = '',
	@P_PersonalLocationName nvarchar(255) = '',
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_PhoneNumber nvarchar(255) = '',
	@P_ReferenceNumber nvarchar(255) = '',
	@P_SmartSearch nvarchar(max)= null,
	@P_SourceName nvarchar(max)= null,
	@P_Language nvarchar(max)= 'EN'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0, @UserEmail nvarchar(max) = null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;
	SET @P_SourceName = (Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = @P_SourceName)
	select @UserEmail = OfficialMailId from UserProfile where UserProfileId = @P_UserID

	declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		Status int
		);

		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,Status from [dbo].[Workflow] 

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1


		declare @RequestComplientList table(
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	FromEmail nvarchar(255),
	Status nvarchar(250))

	insert into @RequestComplientList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],ToEmail,FromEmail,[Status]
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] where Service = 'CAComplaintSuggestions'  group by ReferenceNumber ) 

	declare @RequestList table(
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	FromEmail nvarchar(255),
	Status nvarchar(250))

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],ToEmail,FromEmail,[Status]
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] where Service = 'Citizen Affair'  group by ReferenceNumber ) 



    declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	CitizenAffairID int,
	ReferenceNumber nvarchar(max),
	RequestType nvarchar(max),
	PersonalLocationName nvarchar(max),
	PhoneNumber nvarchar(max),
	Status nvarchar(max),
	RequestedDateTime datetime,
	AttendedBy nvarchar(250),
	AssignedTo nvarchar(255),
	SourceOU nvarchar(max),
	Creator nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	Subject nvarchar(max))


	-- New requst
	insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select (case when @P_Language='AR' then ArLocationName else [LocationName] end) from [dbo].[M_Location] where [LocationID]= (select F.Location from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )) 
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
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			(select (case when a.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.ToEmail) 
					when a.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  W where W.ReferenceNumber=m.ReferenceNumber and W.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by W.WorkflowID desc) as a),null ,
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,null,Subject
			from [dbo].[CAComplaintSuggestions] m join @RequestComplientList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  where  m.[DeleteFlag] !=1
			and 
			@P_UserID in (select [UserProfileId] from [dbo].[UserProfile] U where U.OrgUnitID in (5,6,7,8))  and r.Status = 51 --and r.WorkflowProcess='SubmissionWorkflow'
			
--  Need more info requests


	insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]) ,
			(select case when RequestType=0 then 
			(select (case when @P_Language='AR' then ArLocationName else [LocationName] end) from [dbo].[M_Location] where [LocationID]= (select F.Location from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )) 
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
			
--closed

insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select (case when @P_Language='AR' then ArLocationName else [LocationName] end) from [dbo].[M_Location] where [LocationID]= (select F.Location from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )) 
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
			and ((@UserEmail IN (select W.FromEmail from Workflow as W where W.ReferenceNumber = r.ReferenceNumber and (W.Status = 60 or w.Status = 62))) or(@P_UserID=m.CreatedBy and (r.Status=60 or r.Status= 62)))

	 insert into @Result
		SELECT   m.CAComplaintSuggestionsID,m.ReferenceNumber,(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = 2),
			null as LocationName,
			PhoneNumber as PhoneNumber,
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end)  from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			m.[CreatedDateTime],
			(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId] = m.[CreatedBy]) as Creator,
			(select (case when a.WorkflowProcess='AssignWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.ToEmail) 
					when a.WorkflowProcess='AssignToMeWorkflow' then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where OfficialMailId = a.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  W where W.ReferenceNumber=m.ReferenceNumber and W.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by W.WorkflowID desc) as a),null ,
			 (select (case when @P_Language ='EN' then OrganizationUnits else ArOrganizationUnits end) from Organization as Org where Org.OrganizationID= (select UP.OrgUnitID from UserProfile UP where UserProfileId = m.CreatedBy) )  as SourceOU ,
			 (select IIF(@P_Language = 'EN', UP.EmployeeName, UP.AREmployeeName) from UserProfile UP where UserProfileId = m.CreatedBy) as SourceName ,null,Subject
			from [dbo].[CAComplaintSuggestions] m join @RequestComplientList r   on r.[ReferenceNumber] = m.[ReferenceNumber]  Where  m.[DeleteFlag] !=1
			and
			((@UserEmail IN (select W.FromEmail from Workflow as W where W.ReferenceNumber = r.ReferenceNumber and W.Status = 52)) OR (@P_UserID=M.CreatedBy AND R.Status=52))


-- My Own Personal Requests
			insert into @Result
			SELECT   m.CitizenAffairID,m.[ReferenceNumber], (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) 
			from M_Lookups where Category='CARequestType' and Module = m.[RequestType]),
			(select case when RequestType=0 then 
			(select (case when @P_Language='AR' then ArLocationName else [LocationName] end) from [dbo].[M_Location] where [LocationID]= (select F.Location from [CitizenAffarFieldVisit] as F where F.CitizenAffairID=m.CitizenAffairID )) 
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
				(r.WorkflowProcess ='EscalateWorkflow'  and r.Status = 59 and @P_UserID=m.CreatedBy)
				or (@UserEmail = r.FromEmail and  (r.WorkflowProcess='SubmissionWorkflow' and 
				((select count(*) from Workflow a where a.ReferenceNumber=m.ReferenceNumber and a.WorkflowProcess='SubmissionWorkflow')>1)))
				and (r.Status = 59 )

	if(@P_RequestType  !='' and @P_RequestType is not null)
	delete  from @Result where RequestType != @P_RequestType or RequestType is null

	if(@P_ReferenceNumber != '' and @P_ReferenceNumber is not null)
	 delete from @result where ReferenceNumber not like '%'+@P_ReferenceNumber+'%' or ReferenceNumber is null

	 if(@P_PersonalLocationName != '' and @P_PersonalLocationName is not null)
	 delete from @result where PersonalLocationName not like '%'+@P_PersonalLocationName+'%' or PersonalLocationName is null
 
	  if(@P_SourceName != '' and @P_SourceName is not null)
	 delete from @result where Creator not like '%'+@P_SourceName+'%' or Creator is null

	 if(@P_PhoneNumber != '' and @P_PhoneNumber is not null)
	 delete from @result where PhoneNumber not like '%'+@P_PhoneNumber+'%' or PhoneNumber is null

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestedDateTime as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestedDateTime as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='' and @P_Status is not null)
	 begin
		delete from @result where Status !=  @P_Status or Status is null
	 end


	 if(@P_SmartSearch is not null  )
	begin
		select * from (SELECT row_number() over (Order By [RequestedDateTime] desc) as slno,* from @result as a where 
	((ReferenceNumber  like '%'+@P_SmartSearch+'%') or(RequestType  like '%'+@P_SmartSearch+'%') or 
		 (Status  like '%'+@P_SmartSearch+'%') or (PersonalLocationName like '%'+@P_SmartSearch+'%') or
		  ((SELECT  CONVERT(nvarchar(10), cast(RequestedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')   
		or (PhoneNumber  like '%'+@P_SmartSearch+'%')or(Creator like '%'+@P_SmartSearch+'%')or(AssignedTo like '%'+@P_SmartSearch+'%')
		or(SourceOU like '%'+@P_SmartSearch+'%')or(ApproverDepartment like '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%') 
		or((Select Subject from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
		 or((Select c.Source from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%')
		 or ((Select Details from CAComplaintSuggestions as c where c.ReferenceNumber=a.ReferenceNumber) like '%'+@P_SmartSearch+'%') 
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
		 or((Select c.EmiratesID from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
		 or((Select c.FindingNotes from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
		or((Select c.RequetsedBy from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
		or((Select c.Name from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
		or((Select c.PhoneNumber from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
	   or((Select c.VisitObjective from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
	   or((Select CONVERT(nvarchar(10), cast(c.Date as date), 103) from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')) like '%'+@P_SmartSearch+'%')		
	  or ((select count(K.Location) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152') and (select (case when @P_Language ='EN' then LocationName else ArLocationName end) from M_Location as d where d.LocationID= K.Location )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.City) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152') and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	  or ((select count(K.LocationID) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.LocationID )like '%'+@P_SmartSearch+'%')>0)
     --or ((select count(K.LocationName) from CitizenAffarFieldVisit as K where K.CitizenAffairID=a.CitizenAffairID   and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152')and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where e.EmiratesID= K.LocationName )like '%'+@P_SmartSearch+'%')>0)
	 or((select (case when @P_Language ='EN' then L.DisplayName else L.ArDisplayName end) from M_Lookups as L where L.Category = (Select c.ForWhom from CitizenAffarFieldVisit as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '152'))) like '%'+@P_SmartSearch+'%')		   
    or((Select c.Name from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
    or((Select c.Age from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
   or((Select c.FindingNotes from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	 or((Select c.MaritalStatus from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	 or((Select c.MonthlySalary from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	 or((Select c.NoOfChildrens from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	or((Select c.Recommendation from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	or((Select c.PhoneNumber from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	or((Select c.Destination from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	or((Select c.ReportObjectives from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
	or ((select count(K.City) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')  and (select (case when @P_Language ='EN' then CityName else ArCityName end) from M_City as d where d.CityID= K.City )like '%'+@P_SmartSearch+'%')>0)
	or ((select count(K.Emirates) from CitizenAffairPersonalReport as K where K.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153') and (select (case when @P_Language ='EN' then EmiratesName else ArEmiratesName end) from M_Emirates as e where e.EmiratesID= K.Emirates )like '%'+@P_SmartSearch+'%')>0)
	 or ((Select c.Employer from CitizenAffairPersonalReport as c  where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%') 
	or((Select c.EmiratesID from CitizenAffairPersonalReport as c where c.CitizenAffairID=a.CitizenAffairID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '153')) like '%'+@P_SmartSearch+'%')		
		))m
	 end

	 if(@P_SmartSearch is null)
	 select * from (SELECT row_number() over (Order By [RequestedDateTime] desc) as slno, * from @result) as m
	
END

GO

/****** Object:  StoredProcedure [dbo].[GlobalSearch]    ******/
ALTER  PROCEDURE [dbo].[GlobalSearch] -- 26,'061-2019-M',1    [GlobalSearch] 1,'TestUser2',9,'EN',0,1,1000    -- [GlobalSearch] 1,'AR - Test User 2',9,'AR',0,1,1000
	-- Add the parameters for the stored procedure here
	@P_UserID int = 1, 
	@P_Search nvarchar(max) = null,
	@P_Type int =0,
	@P_Language nvarchar(10)= 'EN',
	@P_Method int= 0,
	@P_PageNumber int =1,
	@P_PageSize int =10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0;

	declare @Memos int =0, @Letters int =0,@DutyTask int =0,@Meetings int =0,@Circulars int =0
	,@Legal int =0,@Protocol int =0,@HR int =0,@CitizenAffair int =0,@Maintenance int =0,@IT int =0
	,@UserEmail nvarchar(max)=null

	select @UserEmail= OfficialMailId from UserProfile where UserProfileId=@P_UserID

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

	declare @Workflow table(
		ID int,
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		FromEmail nvarchar(max),
		Status int,
		DelegateTOEmail nvarchar(max));


    Declare @result table(
	ID int ,
	ReferenceNumber nvarchar(max),
	Title nvarchar(max),
	Type int,
	CanEdit bit default(0)
	);

	--Memo List
	if(@P_Type = 1 or @P_Type = 0)
	begin
		declare @Memoresult table(
		[MemoID] int,
		[ReferenceNumber] [nvarchar](250) NULL,
		[Title] [nvarchar](max) NULL,
		[SourceName] [nvarchar](250) NULL,
		SourceUserName nvarchar(max),
		Details nvarchar(max),
		[Destination] [nvarchar](max) NULL,
		[Status] [nvarchar](250) NULL,
		[CreationDate] datetime null,
		[Private] [nvarchar](250) NULL,
		[Priority] [nvarchar](250) NULL,
		[Creator] nvarchar(max) null,
		[Destinator]  nvarchar(max) null,
		[Receiver] nvarchar(max) null,
		ApproverDepartment nvarchar(255),
		ApproverName nvarchar(255));

		insert into @Memoresult 
		Exec [MemoReport] @P_Language= @P_Language,@P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @Memos = (Select count(*) from @Memoresult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select MemoID, ReferenceNumber,Title,1 from @Memoresult

	end
	

	--Letters
	 if(@P_Type = 2 or @P_Type = 0)
	begin
		declare @LetterResult table(
		id int,
		LetterID int,
		ReferenceNumber nvarchar(255),
		Title nvarchar(255),
		SourceOU nvarchar(max),
		Destination nvarchar(max),
		UserName nvarchar(max),
        SourceName nvarchar(max),
		LinkToOtherLetter nvarchar(250) ,
		SenderName nvarchar(250),
		Status nvarchar(255),
		CreatedDateTime datetime,
		Replied nvarchar(255),
		Priority nvarchar(255),
		LetterType nvarchar(255));

		insert into @LetterResult
		Exec [Get_LetterReportList] @P_Language= @P_Language,@P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @Letters = (Select count(*) from @LetterResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select LetterID, ReferenceNumber,Title,case when LetterType='OutboundLetter' then 3 else 2 end from @LetterResult
	end


	--DutyTask
	 if(@P_Type = 3 or @P_Type = 0)
	begin
		declare @DutyTaskResult table(
		id int,
		TaskID int,
		TaskReferenceNumber nvarchar(255),
		Title nvarchar(255),
		Creator nvarchar(max),
		SourceOU nvarchar(max),
		Assignee nvarchar(max),
		AssigneeDepartment nvarchar(max),
		Details nvarchar(max),
		Status nvarchar(255),
		Priority nvarchar(255),
		CreationDate datetime,
		DueDate datetime,
		LastUpdate datetime,
		Participants nvarchar(100),
	  RemindMeAt datetime,
	  ParticipantUsers nvarchar(100));

		insert into @DutyTaskResult
		Exec [Get_DutyTaskReport] @P_Language= @P_Language,@P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @DutyTask = (Select count(*) from @DutyTaskResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select TaskID, TaskReferenceNumber,Title,4 from @DutyTaskResult
	end

			


	--Meetings
	 if(@P_Type = 4 or @P_Type = 0)
	begin
		declare @MeetingsResult table(
		MeetingID int,
		ReferenceNumber nvarchar(255),
		Subject nvarchar(255),
		Location nvarchar(max),
		MeetingType nvarchar(255),
		Invitees nvarchar(Max),
		UserName nvarchar(255),
		InternalInvitees int,
		ExternalInvitees int,
		startDateTime datetime,
		EndDateTime datetime,
		OrganizerDepartment nvarchar(max),
		OrganizerName nvarchar(max) );

		insert into @MeetingsResult
		Exec [MeetingReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language=@P_Language

		set @Meetings = (Select count(*) from @MeetingsResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select  MeetingID, ReferenceNumber,Subject,5 from @MeetingsResult
	end


	

	--Circular
	 if(@P_Type = 5 or @P_Type = 0)
	begin
		declare @CircularResult table(
		id int,
		CircularID int,
		ReferenceNumber nvarchar(255),
		Title nvarchar(255),
		Source nvarchar(max),
		Destination nvarchar(max),
		Status nvarchar(255),
		CreatedDateTime datetime,
		Priority nvarchar(255),
		UpdatedDateTime datetime);

		insert into @CircularResult
		Exec [CircularReport] @P_Language= @P_Language,@P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @Circulars = (Select count(*) from @CircularResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select  CircularID, ReferenceNumber,Title, 6 from @CircularResult
	end


	--Legal
	 if(@P_Type = 6 or @P_Type = 0)
	begin
		declare @LegalResult table(
		[LegalID] int,
		[ReferenceNumber] [nvarchar](250) NULL,
		[SourceOU] [nvarchar](250) NULL,
		[Subject] [nvarchar](max) NULL,
		[Status] [nvarchar](250) NULL,
		[RequestDate] datetime null,
		[AttendedBy] [nvarchar](250) NULL,
		[CreatedBy] [nvarchar](250) NULL,
		[ApprovedBy] [nvarchar](250) NULL);

		insert into @LegalResult
		Exec [LegalReport] @P_Language= @P_Language,@P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @Legal = (Select count(*) from @LegalResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select LegalID, ReferenceNumber,Subject,7 from @LegalResult
	end


	--CA
	 if( @P_Type = 9 or @P_Type = 0)
	begin
	declare @CitizenAffairResult table(
	slno int,
	id INT ,
	CitizenAffairID int,
	ReferenceNumber nvarchar(max),
	RequestType nvarchar(max),
	PersonalLocationName nvarchar(max),
	PhoneNumber nvarchar(max),
	Status nvarchar(max),
	RequestedDateTime datetime,
	AttendedBy nvarchar(250),
	AssignedTo nvarchar(255),
	SourceOU nvarchar(max),
	Creator nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	Subject nvarchar(max))

		insert into @CitizenAffairResult
		Exec [CitizenAffairReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language=@P_Language

		set @CitizenAffair = (Select count(*) from @CitizenAffairResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select [CitizenAffairID], [ReferenceNumber],Subject,case when RequestType='Complaint' then 9 else 8 end from @CitizenAffairResult
	end


	--Maintenance
	 if(@P_Type = 10 or @P_Type = 0)
	begin
		declare @MaintenaceResult table(
		[MaintenanceID] int,
		[ReferenceNumber] [nvarchar](250) NULL,
		[SourceOU] [nvarchar](250) NULL,
		[Subject] [nvarchar](max) NULL,
		[Status] [nvarchar](250) NULL,
		[RequestedDateTime] datetime null,
		[AttendedBy] [nvarchar](250) NULL,
		[Priority] [nvarchar](250) NULL);

		insert into @MaintenaceResult
		Exec [MaintenanceReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language = @P_Language

		set @Maintenance = (Select count(*) from @MaintenaceResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select [MaintenanceID], [ReferenceNumber],[Subject],10 from @MaintenaceResult
	end


	--HR
	 if((@P_Type = 8) or @P_Type = 0)
	begin
		 declare @HRResult table(
			id INT ,
			ReferenceNumber nvarchar(max),
			Creator nvarchar(max),
			RequestType nvarchar(max),
			Status nvarchar(max),
			RequestDate datetime,
			RequestID int);

		insert into @HRResult
		Exec [HRReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language = @P_Language


		set @HR = (Select count(*) from @HRResult )

		if(@P_Language = 'EN')
	begin
		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select RequestID, [ReferenceNumber],null,(select ModuleID from M_Module where ModuleName=RequestType) from @HRResult
	end
	else
	begin
	insert into @result (ID,ReferenceNumber ,Title ,Type )
		select RequestID, [ReferenceNumber],null,(select ModuleID from M_Module where ModuleName=(select DisplayName from M_Lookups where ArDisplayName = RequestType)) from @HRResult
	end
	end


	--Protocol
	 if((@P_Type = 7) or @P_Type = 0)
	begin
		 declare @MediaResult table(
			[RefID] int ,
			[ReferenceNumber] [nvarchar](250) NULL,
			[SourceOU] [nvarchar](250) Null,
			[SourceName] [nvarchar](250) Null,
			[Status] [nvarchar](250) NULL,
			RequestDate datetime,
			RequestType nvarchar(max),
			MediaDate datetime,
			EventName nvarchar(max),
			Languages nvarchar(max),
			Location nvarchar(max),
			Assignee nvarchar(255),
			Subject nvarchar(max))


		insert into @MediaResult
		Exec [MediaReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language = @P_Language

		
		if(@P_Language = 'EN')
	    begin
		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select [RefID], [ReferenceNumber],Subject,(select ModuleID from M_Module where ModuleName=RequestType) from @MediaResult
		end
		else
	    begin
	    insert into @result (ID,ReferenceNumber ,Title ,Type )
	    select [RefID], [ReferenceNumber],Subject,(select ModuleID from M_Module where ModuleName=(select DisplayName from M_Lookups where ArDisplayName = RequestType)) from @MediaResult
	    end

		declare @CalendarResult table(
			CalendarID int ,
			ReferenceNumber nvarchar(255),
			EventType nvarchar(255),
			EventRequestor nvarchar(max),
			Datefrom datetime,
			DateTo dateTime,
			Location nvarchar(255),
			UserName nvarchar(255),
			Status nvarchar(255),
			ApproverName nvarchar(max),
			City nvarchar(max))


		insert into @CalendarResult
		Exec [CalendarReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language =@P_Language

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select CalendarID, [ReferenceNumber],EventType,12 from @CalendarResult

		declare @GiftResult table(
			GiftID int,
			ReferenceNumber nvarchar(255),
			GiftType nvarchar(255),
			PurchasedBy nvarchar(255),
			Status nvarchar(255),
			CreatedBy nvarchar(255) )


		insert into @GiftResult
		Exec [Get_GiftReportList] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search,@P_Language = @P_Language

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select GiftID, [ReferenceNumber],GiftType,11 from @GiftResult

		set @Protocol = ((Select count(*) from @MediaResult )+(Select count(*) from @GiftResult)+(Select count(*) from @CalendarResult))
	end

	if((@P_Type = 11) or @P_Type = 0)
	begin
		 declare @ITResult table(
			id int,
			RequestID int,
			ReferenceNumber nvarchar(max),
			RequestType nvarchar(max),
			SourceOU nvarchar(max),
			Subject nvarchar(max),
			Priority nvarchar(max),
			Status nvarchar(max),
			RequestDate datetime,
			StatusCode int,
			Creator nvarchar(255))

		insert into @ITResult
		Exec [ITReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search

		set @IT = (Select count(*) from @ITResult )

		insert into @result (ID,ReferenceNumber ,Title ,Type )
		select RequestID, [ReferenceNumber],Subject,27 from @ITResult
	end

	;WITH CTE AS 
		(
		SELECT  ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		)as RowNumber,WorkflowID,ReferenceNumber,ToEmail,FromEmail, Status,DelegateToEmail 
		FROM  Workflow where ReferenceNumber In (select R.ReferenceNumber from @result as R)
		)
		insert into @Workflow
		select * FROM CTE WHERE RowNumber = 1

	--all modules expect Dutytask
	Update a set a.CanEdit=1 from @result as a inner join @Workflow as b on a.ReferenceNumber=b.ReferenceNumber
	and b.ToEmail=@UserEmail and b.Status in (select M.LookupsID from M_Lookups as M where M.DisplayName='Pending for Resubmission')

	-- for Duty Task update in insert into @result
	 

	if(@P_Method =0 and @P_Type != 0)
	select * from (select *,row_number() over (Order By  id desc) as slno from @result) as a where a.slno between @StartNo and @EndNo 
	else if(@P_Type != 0)
	select count(*) from @result
	else if(@P_Type =0)
	select  @Memos as Memos , @Letters as Letters ,@DutyTask as DutyTask ,@Meetings as Meetings ,@Circulars  as Circulars
	,@Legal as Legal ,@Protocol as Protocol ,@HR as HR ,@CitizenAffair as CitizenAffair ,@Maintenance  as Maintenance, @IT as IT 

END
