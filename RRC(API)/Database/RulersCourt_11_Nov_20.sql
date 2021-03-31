ALTER PROCEDURE [dbo].[Get_CalendarHistoryByID]  -- [Get_CalendarHistoryByID] 122,'AR'
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
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),IIF(@P_Language = 'EN', ' With Apologies Letter', N' رفض مع رسالة اعتذار'))
    when (Action = 'Reject' and ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID ) is null) or ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID )= 0) )
        then concat((case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.CreatedBy)
             else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), ' on behalf of ',
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),IIF(@P_Language = 'EN', ' Without Apologies Letter',N' رفض بدون رسالة اعتذار')) end   
    as
     CreatedBy,
        [CreatedDateTime],Comment from [dbo].CalendarCommunicationHistory as a where a.CalendarID = @P_CalendarID
END

GO

ALTER PROCEDURE [dbo].[Get_VehicleListCount]  --[Get_VehicleListCount] 27
	-- Add the parameters for the stored procedure here
	
	@P_UserID int= 0,
	@P_Language nvarchar(10)= null
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	
	 

	  declare @VehicleUsers table(
	  UserID int,
	  UserEmail nvarchar(250)) 

	  --GET HR users
	  insert into @VehicleUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 3
	  --select * from @HRUsers
 declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		FromEmail nvarchar(max),
		Status int,
		WorkflowProcess nvarchar(max),
		DelegateTOEmail nvarchar(max)
		);
		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,FromEmail, Status,WorkflowProcess,DelegateToEmail from [dbo].[Workflow] 

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1
		declare @VehicleRequest int=0,@Fine int=0,@DriverManagement int =0, @OwnRequest int=0, @RentedCar int =0, @MyProcessedRequest int =0,
				@VehicleOnTrip int =0, @VehicleOfTrip int=0,@DriversOnTrip int=0,@DriversOfTrip int=0;

 set @VehicleRequest =(Select Count(*) from VehicleRequest as V join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber]  where (  ( @P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail and r.Status!=217  and r.Status != 224 and r.Status!=213  and r.Status!=212 and r.Status!= 211 ))
		or (@P_UserID in (select UserProfileId from dbo.UserProfile where OrgUnitID=13) and (r.Status!=218 and r.Status !=214 and r.Status!=215 and r.Status!=216 and r.Status!=217 and r.Status != 224 and r.Status!=213) )) )

set @OwnRequest =(Select Count(*) from VehicleRequest as V join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber] where V.[DeleteFlag] !=1
				and V.CreatedBy=@P_UserID )


set @Fine = (select count(*) from VehicleFines where DeleteFlag is null or DeleteFlag =0)
Set @DriverManagement =(select count(*) from Drivers where DeleteFlag is null or DeleteFlag =0)
Set @RentedCar =(select count(*) from CarCompany where DeleteFlag is null or DeleteFlag =0)
Set @MyProcessedRequest =(select Count(distinct  v.ReferenceNumber)from VehicleRequest as V join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber]  where
					  ( @P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId in(select w.FromEmail 
					  from Workflow as w where w.ReferenceNumber= v.ReferenceNumber and w.WorkflowProcess !='SubmissionWorkflow') ))) 

select @DriversOnTrip = count(*) from VehicleRequest  as V join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber] where GETUTCDATE() between V.TripPeriodFrom and V.TripPeriodTo and V.DriverID is not null and r.Status<216
select @DriversOfTrip = Count(*) from UserProfile where IsDriver = 1 and UserProfileId not in (select DriverID from VehicleRequest as V  join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber]where GETUTCDATE() between V.TripPeriodFrom and V.TripPeriodTo and V.DriverID is not null and  r.Status<216)
select @VehicleOnTrip = count(*) from VehicleRequest as VR join @Workflow r on r.[ReferenceNumber] =VR.[ReferenceNumber] where GETUTCDATE() between VR.TripPeriodFrom and VR.TripPeriodTo and VR.VehicleID is not null and  r.Status<216
select @VehicleOfTrip = Count(*) from Vehicles as V where (V.DeleteFlag= 0 or V.DeleteFlag is null) or V.VehicleID not in 
	(select VR.VehicleID from VehicleRequest as VR join @Workflow r on r.[ReferenceNumber] =V.[ReferenceNumber] where GETUTCDATE() between VR.TripPeriodFrom and VR.TripPeriodTo and VR.VehicleID is not null and  r.Status<216)
select @VehicleRequest as Vehicle,@Fine as Fine,@DriverManagement as Driver , @OwnRequest as OwnRequest
		,@MyProcessedRequest as MyProcessedRequest, @RentedCar as RentedCar,@DriversOnTrip as DriversOnTrip,
		@DriversOfTrip as DriversOffTrip,@VehicleOnTrip as VehicleOnTrip,@VehicleOfTrip as VehicleOffTrip

  
  end 

GO
  
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
			from UserProfile u where DepartmentID=(select DepartmentID from M_Department d where d.GroupMailID=@P_ToEmail and @P_ToEmail is not null)
			
		end
		if(@P_Service = 'Vehicle' and (@P_WorkflowProcess = 'ApprovalWorkflow' or @P_WorkflowProcess='AssignWorkflow'  or @P_WorkflowProcess='VehicleReleaseConfirmationRejectWorkflow' or ((@P_WorkflowProcess='VehicleReleaseWorkflow' or @P_WorkflowProcess='VehicleReturnWorkflow') 
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
ALTER PROCEDURE [dbo].[Get_HomeModulesCount] -- Get_HomeModulesCount 2
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


	set  @Circular_Count = (select count(*) FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and w.Status = 14 
   and (
   (@P_UserID in (select UserProfileId from UserProfile where DepartmentID in 
	(select [DepartmentID] from [dbo].[CircularDestinationDepartment] where [CircularID]=a.CircularID and DeleteFlag =0)))))
	
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

