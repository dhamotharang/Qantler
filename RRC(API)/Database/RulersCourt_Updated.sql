USE [RulersCourt]
GO

ALTER PROCEDURE [dbo].[AccessControl] --  [AccessControl] 1,1,'Contact','ContactID','Contact','Delete'
	-- Add the parameters for the stored procedure here
	@P_UserID int =0,
	@P_ServiceID nvarchar(max) = 0,
	@P_Type nvarchar(max) = null,
	@P_ServiceIDName nvarchar(max) = null,
	@P_TableName nvarchar(max) = null,
	@P_Method nvarchar(max) = null
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Declare @ReferenceNumber nvarchar(max) =null,@Result nvarchar(100) = 'Fail',@UserEmail nvarchar(max) = null, @RefCommand nvarchar(max)= null
	,@creator int = null,@creatorOrgUnitID int = null, @CalendarBulkEvent int  =0, @ParentReferenceNumber nvarchar(100) = null;
	select @UserEmail = OfficialMailId from UserProfile where UserProfileId = @P_UserID

	set @RefCommand = 'SELECT @ReferenceNumber = ReferenceNumber FROM '+ @P_TableName+' WHERE '+ @P_ServiceIDName +' = '+ @P_ServiceID

	if(@P_Type = 'Inbound Letter' or @P_Type = 'Outbound Letter')
	set @RefCommand = 'SELECT @ReferenceNumber = LetterReferenceID FROM '+ @P_TableName+' WHERE '+ @P_ServiceIDName +' = '+ @P_ServiceID
	ELSE IF(@P_Type ='Training')
	set @RefCommand = 'SELECT @ReferenceNumber = TrainingReferenceID FROM '+ @P_TableName+' WHERE '+ @P_ServiceIDName +' = '+ @P_ServiceID
	ELSE IF(@P_Type ='Compensation')
	set @RefCommand = 'SELECT @ReferenceNumber = ReferenceNumber FROM '+ @P_TableName+' WHERE [CompensationID] = '+ @P_ServiceID

	exec sp_executesql @RefCommand, N'@ReferenceNumber NVARCHAR(MAX) out', @ReferenceNumber out

	if(@P_Type='Calendar' and (select count(*)from Calendar where CalendarID= @P_ServiceID and ParentReferenceNumber is not null)>0)
	begin
		set @CalendarBulkEvent =1;
		select @ParentReferenceNumber= ParentReferenceNumber from Calendar where CalendarID= @P_ServiceID
	end

	select @creator = UserProfileId,@creatorOrgUnitID = OrgUnitID from UserProfile where OfficialMailId =( select top(1)FromEmail from Workflow where WorkflowProcess='SubmissionWorkflow' and ReferenceNumber = @ReferenceNumber order by WorkflowID asc)

	--select @creator , @creatorOrgUnitID

	
	if(@P_ServiceID =0)
	set @Result = 'Success'

	else if(@P_ServiceID > 0)
	begin
		if(@UserEmail in (select FromEmail from Workflow where Service = @P_Type and [ReferenceNumber] =@ReferenceNumber and FromEmail is not null))
		set @Result = 'Success'

		else if(@UserEmail in (select ToEmail from Workflow where WorkflowProcess !='DraftWorkflow' and Service = @P_Type and [ReferenceNumber] =@ReferenceNumber and ToEmail is not null))
		set @Result = 'Success'

		else if(@UserEmail in (select DelegateFromEmail from Workflow where  Service = @P_Type and [ReferenceNumber] =@ReferenceNumber  and DelegateFromEmail is not null))
		set @Result = 'Success'

		else if(@UserEmail in (select DelegateToEmail from Workflow where  Service = @P_Type and [ReferenceNumber] =@ReferenceNumber and DelegateToEmail is not null))
		set @Result = 'Success'

		else if(@P_Type = 'Memo' and  (select top 1 (W.Status) from Workflow W where W.ReferenceNumber = @ReferenceNumber order by W.WorkflowID desc)!=1)
		begin
			if(@UserEmail in (select OfficialMailId from UserProfile where UserProfileId in 
			(select D.UserID from [dbo].[MemoDestinationUsers] as D where D.[MemoID] = @P_ServiceID and( D.DeleteFlag = 0 or D.DeleteFlag is null)) and OfficialMailId is not null))
			set @Result = 'Success'

			else  if(@P_UserID in (select userID from ShareparticipationUsers where ServiceID=@P_ServiceID))
				set @Result = 'Success'

			else if((select Private from Memo where MemoID = @P_ServiceID and (DeleteFlag =0 or DeleteFlag is null))='0')
				begin
					if(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID = (select OrgUnitID from UserProfile where UserProfileID= (select CreatedBy from Memo where MemoID=@P_ServiceID and (DeleteFlag = 0 or DeleteFlag is null)))
						))
						set @Result = 'Success'
				end
		end

		else if(@P_Type = 'Outbound Letter' and (select top 1 [Status] from [dbo].[Workflow] where [ReferenceNumber] = @ReferenceNumber order by workflowID desc)<>18)
		begin
			if(@UserEmail in (select OfficialMailId from UserProfile where [OrgUnitID]=14 and (DeleteFlag = 0 or DeleteFlag is null)))
			set @Result ='Success'
		end

		else if(@P_Type = 'Inbound Letter'and (select  top 1 [Status] from [dbo].[Workflow] where [ReferenceNumber] = @ReferenceNumber  order by workflowID desc)<>24)
		begin
			if(@UserEmail in (select OfficialMailId from UserProfile where UserProfileId in 
			(select D.UserID from [dbo].[LettersInboundDestinationUsers] as D where D.[LetterID] = @P_ServiceID and( D.DeleteFlag = 0 or D.DeleteFlag is null)) and OfficialMailId is not null))
			set @Result ='Success'
		end

		else if(@P_Type = 'ITSupport')
		begin
			if(((select CreatedBy from ITSupport where ITSupportID = @P_ServiceID)= @P_UserID) or( @P_UserID in (select UserProfileId from UserProfile where OrgUnitID=11 and IsOrgHead = 1)))
			set @Result ='Success'
		end

		else if(@P_Type = 'Circular'and (select top 1 [Status] from [dbo].[Workflow] where [ReferenceNumber] = @ReferenceNumber order by WorkflowID desc)<>12)
		begin
			if(@UserEmail in (select OfficialMailId from UserProfile where DepartmentID in 
			(select D.[DepartmentID] from [dbo].[CircularDestinationDepartment] as D where D.CircularID=@P_ServiceID and 
			(D.DeleteFlag = 0 or D.DeleteFlag is null)) and (DeleteFlag = 0 or DeleteFlag is null)))
			set @Result ='Success'
		end

		else if(@P_Type = 'DutyTask')
		begin
			if((@P_UserID in (select [AssigneeID] from DutyTask where TaskID = @P_ServiceID ))or
		    	(@P_UserID in (select DelegateAssignee from DutyTask where TaskID = @P_ServiceID ))or
				(@P_UserID in (select CreatedBy from DutyTask where TaskID = @P_ServiceID )) or
				(@P_UserID in (select [ResponsibleUsersID] from [dbo].[DutyTaskResponsibleUsers] where TaskID = @P_ServiceID )))
				set @Result ='Success'
		end

		--else if(@P_Type = 'Citizen Affair')
		--begin
		--	if(@P_UserID in (select [InternalRequestorID] from [dbo].[CitizenAffair] where [CitizenAffairID] = @P_ServiceID and (DeleteFlag is null or DeleteFlag =0)))
		--		set @Result ='Success'
		--end

		else if(@P_Type = 'CAComplaintSuggestions')
		begin
			if((((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow')) and
				(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID in (5,6,7,8) and (DeleteFlag = 0 or DeleteFlag is null))))
				set @Result ='Success'

				if((((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='AssignToMeWorkflow')) and
				(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID in (5,6,7,8)  and (DeleteFlag = 0 or DeleteFlag is null))))
				set @Result ='Success'

				if((((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='AssignWorkflow')) and
				(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID in (5,6,7,8) and (DeleteFlag = 0 or DeleteFlag is null))))
				set @Result ='Success'

		end

		else if(@P_Type = 'Maintenance')
		begin
			if((((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)in('ApprovalWorkflow','AssignWorkflow','AssignToMeWorkflow'))) and
				((select top(1) [IsMaintenanceHeadApproved] from [dbo].[Maintenance]  where [MaintenanceID] =@P_ServiceID )=1) and
				 (@P_UserID in(select UserProfileId from UserProfile where OrgUnitID = 12)))
				set @Result ='Success'

			if((((select top(1) WorkflowID from Workflow  where [ReferenceNumber] =@ReferenceNumber and WorkflowProcess ='SubmissionWorkflow' order by WorkflowID desc)>0)) and
				(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID = 12 and IsOrgHead = 1 and (DeleteFlag = 0 or DeleteFlag is null))))
				set @Result ='Success'
		
		end

		else if(@P_Type = 'Legal')
		begin
			if((((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)in('SubmissionWorkflow','ReopenWorkflow','AssignWorkflow','AssignToMeWorkflow'))) and
				(@P_UserID in (select UserProfileId from UserProfile where OrgUnitID = 16 and (DeleteFlag = 0 or DeleteFlag is null))))
				set @Result ='Success'

	
		
		end


		else if((@P_Type = 'MediaDesignRequest' or @P_Type = 'DiwanIdentity' or @P_Type = 'MediaPhotoRequest' or
				@P_Type = 'MediaNewPhotoGrapherRequest' or @P_Type = 'MediaNewCampaignRequest' or @P_Type = 'MediaNewPressReleaseRequest') and 
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in (4,17) and IsOrgHead =1 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and IsOrgHead =1 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID  = 17 and IsOrgHead =1 and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				begin
					set @Result ='Success'	
				end

		else if((@P_Type = 'MediaDesignRequest' or @P_Type = 'DiwanIdentity' or @P_Type = 'MediaPhotoRequest' or
			@P_Type = 'MediaNewPhotoGrapherRequest' or @P_Type = 'MediaNewCampaignRequest' or @P_Type = 'MediaNewPressReleaseRequest') 
			and ((select top (1) WorkflowProcess from Workflow where ReferenceNumber=@ReferenceNumber order by WorkflowID desc) in ('AssignToMeWorkflow','AssignWorkflow')) and 
			((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in (4,17) and (DeleteFlag = 0 or DeleteFlag is null))) or
			(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4  and (DeleteFlag = 0 or DeleteFlag is null))) or
			(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID  = 17 and (DeleteFlag = 0 or DeleteFlag is null))))
			)
			begin
				set @Result ='Success'	
			end

		else if(@P_Type = 'MediaNewCampaignRequest')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=82) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
			  )
				set @Result ='Success'	
		end

		

		else if(@P_Type = 'MediaDesignRequest')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=64) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				set @Result ='Success'	
		end

		else if(@P_Type = 'DiwanIdentity')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=96) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				set @Result ='Success'	
		end

		else if(@P_Type = 'MediaPhotoRequest')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=88) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				set @Result ='Success'	
		end

		else if(@P_Type = 'MediaNewPhotoGrapherRequest')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=70) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				set @Result ='Success'	
		end

		else if(@P_Type = 'MediaNewPressReleaseRequest')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow') or
				((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select top(1) Status from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)=76) and
				((@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID = 4 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17) and @creatorOrgUnitID = 17 and (DeleteFlag = 0 or DeleteFlag is null))) or
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 17 or OrgUnitID = 4) and @creatorOrgUnitID not in(4,17) and (DeleteFlag = 0 or DeleteFlag is null))))
				)
				set @Result ='Success'	
		end

		else if((@P_Type = 'Leave' or @P_Type='CVBank' or @P_Type='UserProfile'  or @P_Type='Training' or @P_Type='Compensation') and 
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and IsOrgHead =1 and (DeleteFlag = 0 or DeleteFlag is null))))
				begin
					set @Result ='Success'	
				end

		else if((@P_Type = 'Leave'   or @P_Type='Training' or @P_Type='Compensation' or
				@P_Type = 'Announcement' or @P_Type= 'BabyAddition' or @P_Type ='Certificate' or @P_Type ='HRComplaintSuggestions') and 
				(select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)in ('AssignToMeWorkflow','AssignWorkflow')and
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9)  and (DeleteFlag = 0 or DeleteFlag is null))))
				begin
					set @Result ='Success'	
				end

		else if(@P_Type = 'Leave' )
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select IsHrHeadApproved from Leave where LeaveID = @P_ServiceID and (DeleteFlag = 0 or DeleteFlag is null))=1) and
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and (DeleteFlag = 0 or DeleteFlag is null))) )
				set @Result ='Success'	
		end

		else if(@P_Type='Training')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				((select IsHrHeadApproved from Training where TrainingID = @P_ServiceID and (DeleteFlag = 0 or DeleteFlag is null))=1) and
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and (DeleteFlag = 0 or DeleteFlag is null))) )
				set @Result ='Success'
			else if((select count(*) from Training where TraineeName=@P_UserID and TrainingReferenceID =@ReferenceNumber)>0)
				set @Result='Success'
		end

		else if(@P_Type = 'Announcement' or @P_Type= 'BabyAddition' or @P_Type ='Certificate' or @P_Type ='HRComplaintSuggestions')
		begin
			if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='SubmissionWorkflow')) and
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and (DeleteFlag = 0 or DeleteFlag is null))) )
				set @Result ='Success'	
		end

		else if(@P_Type = 'Compensation')
		begin
				--if( (((select top(1) WorkflowProcess from Workflow  where [ReferenceNumber] =@ReferenceNumber order by WorkflowID desc)='ApprovalWorkflow')) and
				--((select [IsHrHeadApproved] from [dbo].[Compensation] where CompensationID = @P_ServiceID and (DeleteFlag = 0 or DeleteFlag is null))=1) and
				--(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and (DeleteFlag = 0 or DeleteFlag is null))) )
				set @Result ='Success'		
		end

		else if(( @P_Type= 'Compensation') and 
				(@P_UserID in (select UserProfileId from UserProfile where (OrgUnitID = 9) and IsOrgHead =1 and (DeleteFlag = 0 or DeleteFlag is null))))
				begin
					set @Result ='Success'	
				end

		else if(@P_Type = 'Gift')
		begin
			if((@P_UserID in (select CreatedBy from Gift where GiftID = @P_ServiceID and (DeleteFlag =0 or DeleteFlag is null)))  )
				set @Result ='Success'	
		end

		else if(@P_Type = 'Meeting')
		begin
			if( 
				(@P_UserID in (select CreatedBy from Meeting where MeetingID = @P_ServiceID and (DeleteFlag =0 or DeleteFlag is null))) or 
				(@P_UserID in (select [UserID] from [dbo].[MeetingInternalInvitees] where MeetingID = @P_ServiceID )) 
			  )
				set @Result ='Success'	
		end

		

		else if(@P_Type = 'UserProfile')
		begin
			if( 
				(@P_UserID in (select OrgUnitID from UserProfile where @P_Method='GET' and((OrgUnitID = 4 and IsOrgHead =1) or (CanRaiseOfficalRequest = 1)) and 
				(DeleteFlag =0 or DeleteFlag is null))))
				set @Result ='Success'	

				if( 
				(@P_UserID in (select OrgUnitID from UserProfile where (@P_Method='GET' or @P_Method='POST' or @P_Method='PUT' or @P_Method='DELETE') and((OrgUnitID = 4 and IsOrgHead =1) ) and 
				(DeleteFlag =0 or DeleteFlag is null))))
				set @Result ='Success'	

				--if(@P_UserID = @P_ServiceID)
				set @Result ='Success'
		end

	end

	--if(( @P_Type= 'Compensation') and (@P_UserID != (select CreatedBy from OfficialTask where OfficialTaskID = @P_ServiceID)) and 
	--	 (select count(*) from [dbo].[Compensation] where [OfficialTaskID] = @P_ServiceID)=0)
	--			begin
	--				set @Result ='Fail'	
	--			end
	--else 
	if(( @P_Type= 'OfficialTask') and @P_ServiceID =0 and (select CanRaiseOfficalRequest from UserProfile where UserProfileId=@P_UserID) = 0)
				begin
					set @Result ='Fail'	
				end
	else if(@P_Type = 'Contact'  and (select CanEditContact
	from [dbo].[UserProfile] where UserProfileId=@P_UserID and (DeleteFlag = 0 or DeleteFlag is null)) = 0 and @P_Method in ('POST','PUT','DELETE'))
			begin
				set @Result ='Fail'	
			end
			else if(@P_Type = 'Contact'  and (select CanEditContact
	from [dbo].[UserProfile] where UserProfileId=@P_UserID and (DeleteFlag = 0 or DeleteFlag is null)) != 0 )
			begin
				set @Result ='Success'	
			end

			--if(@P_Type ='calendar' and ((@UserEmail in(select ToEmail from Worflow where ReferenceNumber in(select ParentReferenceNumber from Calendar where ReferenceNumber = @ReferenceNumber)))
			--	or (@UserEmail in(select FromEmail from Worflow where ReferenceNumber in(select ParentReferenceNumber from Calendar where ReferenceNumber = @ReferenceNumber)))
			--	or (@UserEmail in(select [DelegateFromEmail] from Worflow where ReferenceNumber in(select ParentReferenceNumber from Calendar where ReferenceNumber = @ReferenceNumber)))
			--	or (@UserEmail in(select [DelegateToEmail] from Worflow where ReferenceNumber in(select ParentReferenceNumber from Calendar where ReferenceNumber = @ReferenceNumber)))
			--))
			--set @Result ='Success'	
			if(@P_Type='Calendar' and @UserEmail in 
			
			(select OfficialMailID from UserProfile where OrgUnitID=4))
			set @Result ='Success'

			else if((@P_Type='Gift' or @P_Type='Calendar') and ((select [OrgUnitID] from [dbo].[UserProfile] where [UserProfileId]= @P_UserID)<>4)	)
				set @Result ='Fail'	
	select @Result
	end
	GO

	ALTER PROCEDURE [dbo].[Get_CompensationPreview]  --   Get_CompensationPreview 69
	-- Add the parameters for the stored procedure here
	@P_CompensationID int = 0
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
NoOfDays int
)

declare @Assignee nvarchar(max) = null;

select @Assignee = (case when q.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.ToEmail) 
					when q.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b 
					where b.ReferenceNumber=(select ReferenceNumber from Compensation where CompensationID=@P_CompensationID) and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as q

insert into @result
select O.ReferenceNumber,C.ReferenceNumber,O.OfficialTaskDescription,C.StartDate,(select Up.EmployeeName from UserProfile as UP where UP.UserProfileID = O.CreatedBy)
,(select Up.EmployeeName from UserProfile as UP where UP.UserProfileID = O.CreatedBy) ,(@Assignee)
,(@Assignee),C.NumberofDays from Compensation as C inner join OfficialTask as O on C.OfficialTaskID=O.OfficialTaskID 
 where C.CompensationID = @P_CompensationID

select * from @result

END
GO

ALTER PROCEDURE [dbo].[Get_CompensationPreview]  --   Get_CompensationPreview 69
	-- Add the parameters for the stored procedure here
	@P_CompensationID int = 0
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
NoOfDays int
)

declare @Assignee nvarchar(max) = null;

select @Assignee = (case when q.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.ToEmail) 
					when q.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b 
					where b.ReferenceNumber=(select ReferenceNumber from Compensation where CompensationID=@P_CompensationID) and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as q

insert into @result
select O.ReferenceNumber,C.ReferenceNumber,O.OfficialTaskDescription,C.StartDate,(select Up.EmployeeName from UserProfile as UP where UP.UserProfileID = O.CreatedBy)
,(select Up.EmployeePosition from UserProfile as UP where UP.UserProfileID = O.CreatedBy) ,(@Assignee)
,(select Up.EmployeeCode from UserProfile as UP where UP.EmployeeName = @Assignee),C.NumberofDays from Compensation as C inner join OfficialTask as O on C.OfficialTaskID=O.OfficialTaskID 
 where C.CompensationID = @P_CompensationID

select * from @result

END

GO

ALTER PROCEDURE [dbo].[Get_MyPendingListInHR]  --[Get_MyPendingListInHR] 1,20,2,0
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int = 0,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	  SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --select @UserName
	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1
	  --select * from @HRManagers
	  --GET HR users
	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int,
	AssignedTo nvarchar(max),
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	IsCompensationRequest bit
    )

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] 
	group by ReferenceNumber) 
	--select * from @RequestList
	
		 declare @RequestList1 table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)
	insert into @RequestList1
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where Service='OfficialTask' and ((WorkflowProcess='SubmissionWorkflow' and ToName=@UserName)or(WorkflowProcess='CloseWorkflow' and FromName=@UserName))
	--select * from @RequestList1
	-- For Training Request
	insert into @Result
			SELECT  t.TrainingID,t.[TrainingReferenceID],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           		 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 
	   (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= t.ApproverDepartmentID) as ApproverDepartment

            		,0 from [dbo].[Training] t join @RequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( (((w.Status = 42 or w.Status = 43 or w.Status = 44) and w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))
			or (( @UserEmail = w.ToEmail or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=42)))
		
--	select * from @Result
	-- For Announcements Request

			insert into @Result
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
		
			,0 from [dbo].[Announcement] a join @RequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and ((w.Status = 36 or w.Status = 37))

	-- For HRComplaintSuggestions Request

		insert into @Result
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
	
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c),
			null,l.Source as SourceName ,null,null
			,0 from [dbo].[HRComplaintSuggestions] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ((w.Status = 47 or w.Status = 48))

	-- For Baby Addition Request

		insert into @Result
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			
			,0 from [dbo].[BabyAddition] b join @RequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and( (w.Status = 39 or w.Status = 40))

	-- For Experience Certificate Request

		insert into @Result
			SELECT  e.CertificateID,e.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null

			,0 from [dbo].[Certificate] e join @RequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and ((w.Status = 33 or w.Status = 34))


	-- For Salary Certificate Request

			insert into @Result
			SELECT  s.CertificateID,s.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			
			,0 from [dbo].[Certificate] s join @RequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and ((w.Status = 33 or w.Status = 34))
			

	-- For Leave Request

		insert into @Result
			SELECT  l.LeaveID,l.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 
          		  ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c),
				( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 
	 (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= l.ApproverDepartmentID) as ApproverDepartment

            
			,0 from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ( ((( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=7) or ((w.Status = 7 or w.Status = 8 or w.Status =10)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))) )
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @Result
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
then 107 else 113 end) as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0,null,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
			,0 from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
				((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
				and 
				(113=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @Result
			SELECT  C.CompensationID,C.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            		,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
				,(select (case when t.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.ToEmail) 
					when t.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=c.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as t) ,
				
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 
	   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ( select OrgUnitID from [UserProfile] where OfficialMailId=(
	  (select top 1 ToEmail from @RequestList
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108)) 
	 )))
	  as ApproverDepartment
          		 ,1 from [dbo].[Compensation] C join @RequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]  
			and( (( ( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	  and w.Status =108)or ((w.Status = 108 or w.Status = 109 or w.Status =110)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))))
			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @Result where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @Result where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @Result where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end 

	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
			select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%')
				or a.AssignedTo like '%'+@P_SmartSearch+'%' 
			or(SourceName  like '%'+@P_SmartSearch+'%')or(ApproverName  like '%'+@P_SmartSearch+'%')or(ApproverDepartment  like '%'+@P_SmartSearch+'%')
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
or	((SELECT  CONVERT(nvarchar(10), ( cast( DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()), l.Birthday) as date)), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 				or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
	
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')
) ) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
				or a.AssignedTo like '%'+@P_SmartSearch+'%' 
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%')
			or(SourceName  like '%'+@P_SmartSearch+'%')or(ApproverName  like '%'+@P_SmartSearch+'%')or(ApproverDepartment  like '%'+@P_SmartSearch+'%')
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
or	((SELECT  CONVERT(nvarchar(10), ( cast( DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()), l.Birthday) as date)), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 				or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests'and IsCompensationRequest = 'false' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID= (select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID) and  a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests'and IsCompensationRequest = 'true' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
	
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')
)
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m  
	 where slno between @StartNo and @EndNo
	 Order By UpdatedDateTime desc 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END

GO

ALTER PROCEDURE [dbo].[Get_MyPendingListInHRCount]  --[Get_MyPendingListInHR] 1,20,3,1
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int = 0,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --select @UserName
	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1
	  --select * from @HRManagers
	  --GET HR users
	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int
    )

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] group by ReferenceNumber) 
	 declare @RequestList1 table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)
	insert into @RequestList1
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where Service='OfficialTask' and ((WorkflowProcess='SubmissionWorkflow' and ToName=@UserName)or(WorkflowProcess='CloseWorkflow' and FromName=@UserName))
	
	-- For Training Request
		
		insert into @Result
			SELECT  t.TrainingID,t.[TrainingReferenceID],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           		 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
            		from [dbo].[Training] t join @RequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( ((w.Status = 42 or w.Status = 43 or w.Status = 44) and w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))
			or (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=42))
		
--	select * from @Result
	-- For Announcements Request

			insert into @Result
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Announcement] a join @RequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (w.Status = 36 or w.Status = 37)

	-- For HRComplaintSuggestions Request

		insert into @Result
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
	
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			from [dbo].[HRComplaintSuggestions] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (w.Status = 47 or w.Status = 48)

	-- For Baby Addition Request

		insert into @Result
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			from [dbo].[BabyAddition] b join @RequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (w.Status = 39 or w.Status = 40)

	-- For Experience Certificate Request

		insert into @Result
			SELECT  e.CertificateID,e.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0
			from [dbo].[Certificate] e join @RequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (w.Status = 33 or w.Status = 34)


	-- For Salary Certificate Request

			insert into @Result
			SELECT  s.CertificateID,s.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Certificate] s join @RequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (w.Status = 33 or w.Status = 34)

			

	-- For Leave Request

		insert into @Result
			SELECT  l.LeaveID,l.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 
          		  ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            
            
			from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ( (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=7) or ((w.Status = 7 or w.Status = 8  or w.Status =10)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID)))
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @Result
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			, 
			0,0,0
			from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
				((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
				and(113=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @Result
			SELECT  C.CompensationID,C.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            		,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
          		 from [dbo].[Compensation] C join @RequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   and (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and( (( ( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	  and w.Status =108)or ((w.Status = 108 or w.Status = 109 or w.Status =110)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))))

			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @Result where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @Result where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @Result where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end 

	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
			select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		 (Status  like '%'+@P_SmartSearch+'%')) ) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or  (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		  (Status  like '%'+@P_SmartSearch+'%') ) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m  
	 where slno between @StartNo and @EndNo
	 Order By UpdatedDateTime desc 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END

Go

ALTER PROCEDURE [dbo].[Get_MyOwnRequestsInHR] -- Get_MyOwnRequestsInHR 1,27,11,0
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =1,
	@P_PageSize int =10,
	@P_UserID int = 1,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = '',
	@P_Language nvarchar(10)= null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

	declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)


	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250))

	 declare @Result table(
	id INT IDENTITY(1, 1) primary key,
		RequestID int,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	UpdatedDateTime datetime,
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	IsCompensationRequest bit,
	AssignedTo nvarchar(255)
	)

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status]
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] group by ReferenceNumber) 


	-- For Training Request

		insert into @Result
	select distinct t.TrainingID,t.[TrainingReferenceID],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = t.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end)  from [dbo].[M_Lookups] where Module = 'Training' AND Category = 'Status' AND LookupsID = w.Status)as Status,
	t.[CreatedDateTime],
	(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 42 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	
	   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.ApproverDepartmentID)
	  as ApproverDepartment
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.[TrainingReferenceID] and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[Training] t join @RequestList w on t.TrainingReferenceID = w.ReferenceNumber and t.DeleteFlag is not null and ((t.CreatedBy = @P_UserID and w.Status!=44)
				 or(t.TraineeName=@P_UserID and t.IsHrHeadApproved=1))
	

	--select * from @Result

	-- For Leave Request

	insert into @Result
	select distinct l.LeaveID,l.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = l.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Leave' AND Category = 'Status' AND LookupsID = w.Status ) as Status,
	l.[CreatedDateTime],
	(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
		( select EmployeeName from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 
	   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID=l.ApproverDepartmentID
	   )
	  as ApproverDepartment
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.ReferenceNumber and l.DeleteFlag is not null and l.CreatedBy = @P_UserID and w.Status != 10
	

	-- For Baby Addition Request

	insert into @Result
	select distinct b.BabyAdditionID,b.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = b.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'BabyAddition' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	b.[CreatedDateTime],
	(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  t where t.ReferenceNumber=b.ReferenceNumber and t.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by t.WorkflowID desc) as c)
				 from [dbo].[BabyAddition] b join @RequestList w on b.[ReferenceNumber] = w.ReferenceNumber and b.DeleteFlag is not null and b.CreatedBy = @P_UserID


	-- For Salary Certificate Request

	insert into @Result
	select distinct s.CertificateID,s.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = s.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	s.[CreatedDateTime],
	(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=s.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[Certificate] s join @RequestList w on s.[ReferenceNumber] = w.ReferenceNumber and s.DeleteFlag is not null 
	and s.CreatedBy = @P_UserID and s.CertificateType = 'Salary'
	

	-- For Experience Certificate Request

	insert into @Result
	select distinct e.CertificateID, e.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = e.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	e.[CreatedDateTime],
	(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null

	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=e.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[Certificate] e join @RequestList w on e.[ReferenceNumber] = w.ReferenceNumber and e.DeleteFlag is not null 
	and e.CreatedBy = @P_UserID and e.CertificateType = 'Experience'
	

	-- For HRComplaintSuggestions Request

	insert into @Result
	select distinct h.HRComplaintSuggestionsID,h.[ReferenceNumber],
	case when h.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = h.[CreatedBy]) end as Creator,
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'HRComplaintSuggestions' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	h.[CreatedDateTime],
	(case when (h.UpdatedDateTime is null) then h.[CreatedDateTime] else h.UpdatedDateTime end) as UpdatedDateTime,
			null,h.Source as SourceName ,null,null
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=h.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[HRComplaintSuggestions] h join @RequestList w on h.[ReferenceNumber] = w.ReferenceNumber and h.DeleteFlag is not null 
	and h.CreatedBy = @P_UserID 
	

	-- For Announcements Request

	insert into @Result
	select distinct a.AnnouncementID,a.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = a.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Announcement' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	a.[CreatedDateTime],
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
		
	,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
				 from [dbo].[Announcement] a join @RequestList w on a.[ReferenceNumber] = w.ReferenceNumber and a.DeleteFlag is not null 
	and a.CreatedBy = @P_UserID 
	--select * from @Result
	--For OfficialTask Request
		insert into @Result
	select distinct OT.OfficialTaskID,OT.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = OT.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'OfficialTask' AND Category = 'Status' AND LookupsID = 	(select case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber))
then 107 else 113 end) ) as Status,
	OT.[CreatedDateTime],
	(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
	,0,null from [dbo].[OfficialTask] as OT join @RequestList w on OT.[ReferenceNumber] = w.ReferenceNumber and OT.DeleteFlag is not null 
	and OT.CreatedBy = @P_UserID 

	--For Compensation Request
	insert into @Result
	select distinct C.CompensationID,C.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = C.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Compensation' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	C.[CreatedDateTime],
	(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime ,
				
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
				(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 
	   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ( select OrgUnitID from [UserProfile] where OfficialMailId=(
	  (select top 1 ToEmail from @RequestList
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108)) 
	 )))
	  as ApproverDepartment
	,1,(select (case when q.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.ToEmail) 
					when q.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=c.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as q)
		 from [dbo].[Compensation] as C join @RequestList w on C.[ReferenceNumber] = w.ReferenceNumber --and C.DeleteFlag is not null 
	and C.CreatedBy = @P_UserID and w.WorkflowProcess !='ReturnWorkflow'

	--select * from @Result

	/* if(@P_Method = 0 or @P_Method is null)
	begin
	select * from @Result
	end
	else if(@P_Method = 1)
	begin
	select count(*) from @Result
	end */

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 --if(@P_Creator !='')
	 --begin
		--delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 --end 

	
	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
		select * from (select  row_number() over (Order By RequestDate desc) as slno,* from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%')
			or(SourceName  like '%'+@P_SmartSearch+'%')or(ApproverName  like '%'+@P_SmartSearch+'%')or(ApproverDepartment  like '%'+@P_SmartSearch+'%')
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
or	((SELECT  CONVERT(nvarchar(10), ( cast( DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()), l.Birthday) as date)), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			
			or ((select (K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and 
					a.RequestType='New Baby Addition'  )like '%'+@P_SmartSearch+'%')or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests'and IsCompensationRequest = 'false' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID= (select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID) and  a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests'and IsCompensationRequest = 'true' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or a.AssignedTo like '%'+@P_SmartSearch+'%' 
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
		
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')
)) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
	((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%')
			or(SourceName  like '%'+@P_SmartSearch+'%')or(ApproverName  like '%'+@P_SmartSearch+'%')or(ApproverDepartment  like '%'+@P_SmartSearch+'%')
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or a.AssignedTo like '%'+@P_SmartSearch+'%' 
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), ( cast( DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()), l.Birthday) as date)), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select (K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and 
					a.RequestType='New Baby Addition'  )like '%'+@P_SmartSearch+'%')
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests' and IsCompensationRequest = 'false' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=a.RequestID and a.RequestType='Official Requests'and IsCompensationRequest = 'false' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID= (select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID) and  a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests' and IsCompensationRequest = 'true' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.OfficialTaskID=(select c.OfficialTaskID from Compensation as c where c.CompensationID=a.RequestID)  and a.RequestType='Official Requests'and IsCompensationRequest = 'true' and (select EmployeeCode from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
			or((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
	or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')
)
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m 
	 where slno between @StartNo and @EndNo  
	 Order By UpdatedDateTime desc

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END

GO

ALTER PROCEDURE [dbo].[Get_MyProcessedListInHR]  --[Get_MyProcessedListInHR] 1,20,5,1
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =1,
	@P_PageSize int =10,
	@P_UserID int = 4,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	  SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --select @UserName
	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1
	  --select * from @HRManagers
	  --GET HR users
	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int,
	AssignedTo nvarchar(max),
	IsCompensationRequest bit
    )

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] group by ReferenceNumber) 


	 declare @LeaveRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @CertificateRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @BabyAdditionRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @TrainingRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @AnnouncementRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @OfficialRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @ComplaientRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	
		;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Leave'
		)
	insert into @LeaveRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Certificate'
		)
	insert into @CertificateRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='BabyAddition'
		)
	insert into @BabyAdditionRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Training'
		)
	insert into @TrainingRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Announcement'
		)
	insert into @AnnouncementRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='HRComplaintSuggestions'
		)
	insert into @ComplaientRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	
	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service in ('Compensation','OfficialTask')
		)
	insert into @OfficialRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	-- For Training Request
		
		insert into @Result
			SELECT  t.TrainingID,t.[TrainingReferenceID],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           		 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					 from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
            	,0	from [dbo].[Training] t join @TrainingRequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( (	 @P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and 
			P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
--	select * from @Result
	-- For Announcements Request

			insert into @Result
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			,0 from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

	-- For HRComplaintSuggestions Request

		insert into @Result
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
	
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			,0 from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

	-- For Baby Addition Request

		insert into @Result
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c)
			,0 from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

	-- For Experience Certificate Request

		insert into @Result
			SELECT  e.CertificateID,e.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c)
			,0 from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))


	-- For Salary Certificate Request

			insert into @Result
			SELECT  s.CertificateID,s.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c)
			,0 from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

			

	-- For Leave Request

		insert into @Result
			SELECT  l.LeaveID,l.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 
          		  ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as c)
            
			,0 from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and
	  (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber 
	  and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @Result
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			,(case when 
			(((select top 1[FromEmail] from Workflow where [ReferenceNumber] = OT.[ReferenceNumber] and status = 107) = @UserEmail
			and DATEADD(DAY, 5, OT.CreatedDateTime) > (select GETDATE()) ) and ((select count(*) from Compensation where OfficialTaskID=OT.OfficialTaskID)=0))
			then 1 else 0 end),0,0,null
			,0 from [dbo].[OfficialTask] OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
				((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
				and (w.Status <> 107 and w.WorkflowProcess!='SubmissionWorkflow')) or (w.Status= 107  and W.FromEmail= (select OfficialMailId from UserProfile where UserProfileId=@P_UserID)))
			--and OT.OfficialTaskID not in (select OfficialTaskID from Compensation where DeleteFlag!=1 ) 
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @Result
			SELECT  C.CompensationID,C.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            		,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
				,(select (case when t.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.ToEmail) 
					when t.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=c.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by q.WorkflowID desc) as t)
          		,1  from [dbo].[Compensation] C join @OfficialRequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   and (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @Result where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @Result where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @Result where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end  

	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
			select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		 (Status  like '%'+@P_SmartSearch+'%')) ) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or  (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		  (Status  like '%'+@P_SmartSearch+'%') ) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m  
	 where slno between @StartNo and @EndNo
	 Order By UpdatedDateTime desc 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result

 END
 
 GO
 
ALTER PROCEDURE [dbo].[Get_MyPendingListInHRCount]  --[Get_MyPendingListInHR] 1,20,3,1
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int = 0,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --select @UserName
	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1
	  --select * from @HRManagers
	  --GET HR users
	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int
    )

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] group by ReferenceNumber) 
	 declare @RequestList1 table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)
	insert into @RequestList1
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where Service='OfficialTask' and ((WorkflowProcess='SubmissionWorkflow' and ToName=@UserName)or(WorkflowProcess='CloseWorkflow' and FromName=@UserName))
	
	-- For Training Request
		
		insert into @Result
			SELECT  t.TrainingID,t.[TrainingReferenceID],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           		 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
            		from [dbo].[Training] t join @RequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( ((w.Status = 42 or w.Status = 43 or w.Status = 44) and w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))
			or (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=42))
		
--	select * from @Result
	-- For Announcements Request

			insert into @Result
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Announcement] a join @RequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (w.Status = 36 or w.Status = 37)

	-- For HRComplaintSuggestions Request

		insert into @Result
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
	
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			from [dbo].[HRComplaintSuggestions] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (w.Status = 47 or w.Status = 48)

	-- For Baby Addition Request

		insert into @Result
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			from [dbo].[BabyAddition] b join @RequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (w.Status = 39 or w.Status = 40)

	-- For Experience Certificate Request

		insert into @Result
			SELECT  e.CertificateID,e.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0
			from [dbo].[Certificate] e join @RequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (w.Status = 33 or w.Status = 34)


	-- For Salary Certificate Request

			insert into @Result
			SELECT  s.CertificateID,s.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Certificate] s join @RequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (w.Status = 33 or w.Status = 34)

			

	-- For Leave Request

		insert into @Result
			SELECT  l.LeaveID,l.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 
          		  ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            
            
			from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ( (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=7) or ((w.Status = 7 or w.Status = 8  or w.Status =10)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID)))
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @Result
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			, 
			0,0,0
			from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
				((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
				and(w.Status=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @Result
			SELECT  C.CompensationID,C.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            		,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
          		 from [dbo].[Compensation] C join @RequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   and (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and( (( ( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	  and w.Status =108)or ((w.Status = 108 or w.Status = 109 or w.Status =110)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))))

			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @Result where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @Result where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @Result where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end 

	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
			select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		 (Status  like '%'+@P_SmartSearch+'%')) ) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or  (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		  (Status  like '%'+@P_SmartSearch+'%') ) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m  
	 where slno between @StartNo and @EndNo
	 Order By UpdatedDateTime desc 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END

GO

Update M_Lookups set ArDisplayName=N'جديد' where Module='Legal' and DisplayName = 'New'

GO

ALTER PROCEDURE [dbo].[Get_MemoPreview] 
	@P_MemoID int = 0
AS
BEGIN
	
	SET NOCOUNT ON;

	
	Declare @ReferenceNumber nvarchar(max) = null, @ApproverEmail nvarchar(max) = null, @ApproverName nvarchar(max),@ApproverDesignation nvarchar(max),
	@ApproverID int,@ApproverDepartment  nvarchar(max) = null,@signatureID nvarchar(max)=null, @SignatureName nvarchar(max)=null,@Title nvarchar(max)=null
	,@RedirectName nvarchar(max),@RedirectID int=0, @RedirectPhoto nvarchar(max); 

	select @ReferenceNumber=ReferenceNumber from Memo where MemoID= @P_MemoID

	set @ApproverEmail = (select top 1 case when DelegateFromEmail is null then FromEmail else DelegateFromEmail end from Workflow where ReferenceNumber = @ReferenceNumber 
							and Status in (3,4) order by WorkflowID desc)

	select @RedirectName=EmployeeName,@RedirectID = UserProfileId,@RedirectPhoto = SignaturePhoto from UserProfile where OfficialMailId = (select top 1 FromEmail from Workflow where ReferenceNumber= @ReferenceNumber and status =5 order by WorkflowID desc)

	if(@ApproverEmail is null)
	set @ApproverEmail = (select top 1 ToEmail from Workflow where ReferenceNumber = @ReferenceNumber 
							and Status in (1,2) order by WorkflowID desc)
	
	if(@ApproverEmail is not null)
	select @Title = U.Title, @ApproverID=UserProfileId,@ApproverName=EmployeeName,@ApproverDesignation=EmployeePosition,@ApproverDepartment=
	(select ArDepartmentName from M_Department as M where M.DepartmentID= U.DepartmentID) from UserProfile as U  where OfficialMailId = @ApproverEmail

	if((select count(*) from Workflow where ReferenceNumber = @ReferenceNumber and Status in (3,4) )>0)
	begin
	select @SignatureName = U.SignaturePhoto, @signatureID=U.SignaturePhotoID from UserProfile as U  where OfficialMailId = @ApproverEmail
	end

	SELECT [MemoID],[ReferenceNumber],[Title] ,[SourceOU],[SourceName],[Details],[Private],[Priority],[CreatedBy] 
	 ,[UpdatedBy],[CreatedDateTime],[UpdatedDateTime],
	 (select top 1 Status from [Workflow] 
	 where ReferenceNumber =[Memo].ReferenceNumber order by WorkflowID desc) as Status,@ApproverDesignation as ApproverDesignation,
	  (select STUFF((select (select ','+EmployeeName AS [text()]  from (
        select EmployeeName from UserProfile as c join MemoDestinationUsers as a on c.UserProfileId=a.UserID where a.MemoID=memo.MemoID
       ) A FOR XML PATH ('') ) ), 1, 1, NULL) )as DestinationTitle, (select us.EmployeeName from UserProfile us where us.UserProfileId=[Memo].[CreatedBy]) as ApproverNameID,@ApproverID  as ApproverID,
	@SignatureName as SignaturePhotoApprover,  @RedirectPhoto as SignaturePhotoRedirector, @RedirectName as RedirectNameID,
	@RedirectID as RedirectID,(  select top 1 m.Comments from MemoHistory as m where Action='Redirect' and m.MemoID=memo.MemoID ) as Comments
	from [dbo].[Memo]  WHERE MemoID = @P_MemoID
END

GO

ALTER PROCEDURE [dbo].[Get_LegalList] -- [Get_LegalList] 1,50,5,12,0
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Type nvarchar(10) =null,
	@P_UserID int = 0,
	@P_Method int = 0,
	@P_Status nvarchar(250) = null,
	@P_UserName nvarchar(250) = null,
	@P_SourceOU nvarchar(250) = null,
	@P_Subject nvarchar(250) = null,
	@P_RequestDateFrom  datetime = null,
	@P_RequestDateTo datetime = null,
	@P_Label nvarchar(250) = null,
	@P_AttendedBy nvarchar(250) = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_language nvarchar(10) = 'EN'

	AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
declare @RefCout int =0, @StartNo int =0, @EndNo int =0;

select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
select @StartNo =(@RefCout + 1);
select @EndNo=@RefCout + @P_PageSize;
declare @UserID int = @P_UserID
declare @UserName nvarchar(250), @UserEmail nvarchar(250)
SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
set @P_SourceOU = (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end)  from [dbo].M_Department where DepartmentID = @P_SourceOU)
SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)

declare @LegalManagers table(
UserID int,
UserEmail nvarchar(250))

declare @LegalUsers table(
UserID int,
UserEmail nvarchar(250))

--GET HR managers
insert into @LegalManagers
select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 16 and ([IsOrgHead] = 1 or HOD = 1 or HOS = 1)
--select * from @HRManagers
--GET HR users
insert into @LegalUsers
select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 16 and ([IsOrgHead] is null or [IsOrgHead] =0)
--select * from @HRUsers

declare @RequestList table(

ReferenceNumber nvarchar(max),
--id int,
--Creator nvarchar(max),
WorkflowProcess nvarchar(max),
	FromEmail nvarchar(250),
	ToEmail nvarchar(250),
Status nvarchar(250),DelagateToEmail nvarchar(max))



declare @Result table(
id INT IDENTITY(1, 1) primary key,
LegalID int ,
ReferenceNumber nvarchar(max),
SourceOU nvarchar(max),
Subject nvarchar(max),
Status nvarchar(max),
RequestDate datetime,
	--Label nvarchar(max),
AttendedBy nvarchar(250),
SourceName  nvarchar(max),
Details nvarchar(max),
AssignedTo nvarchar(max)
)



;With CTE_Duplicates as
   (select [ReferenceNumber],[WorkflowProcess],[FromEmail],[ToEmail],[Status],[DelegateToEmail], 
			row_number() over(partition by [ReferenceNumber] order by WorkflowID desc ) rownumber 
   from [Workflow] where Service = 'Legal' )
   insert into @RequestList
   select  [ReferenceNumber],[WorkflowProcess],[FromEmail],[ToEmail],[Status],[DelegateToEmail] from CTE_Duplicates where rownumber=1

   


-- insert into @requestlist
-- select  [referencenumber],[workflowprocess],[fromemail],[toemail],[status]
	-- from [dbo].[workflow] where createddatetime in  (select max(createddatetime)
	-- from [dbo].[workflow] where service = 'legal' group by referencenumber)  

--select * from @RequestList;
	if(@P_Type = 1)           --New Requests
	begin
	insert into @Result
	       select L.[LegalID],L.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department 
	       where DepartmentID = L.SourceOU) as SourceOU,L.[Subject],
          (select  (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
          L.[CreatedDateTime],   
	  (select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = L.[CreatedBy]) as Creator,
        (select EmployeeName from UserProfile where UserProfileId=L.SourceName) as SourceName,L.RequestDetails,
		(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow','ReopenWorkflow') 
				order by b.WorkflowID desc) as c)
         from [dbo].[Legal] L join @RequestList r on r.[ReferenceNumber] = L.[ReferenceNumber] where L.[DeleteFlag] !=1
     and
          (( ((@P_UserID in (select UserProfileId from [dbo].[UserProfile] where  OrgUnitID=16)  and  ((r.Status=102)  or (r.Status=103 and WorkflowProcess='ReopenWorkflow') )))

	--	    or
	--      ((@P_UserID in (select  UserProfileId from [dbo].[UserProfile] where [OrgUnitID]=16 )  and  r.Status = 103 and( r.WorkflowProcess != 'AssignWorkflow' 
	--and    r.WorkflowProcess != 'AssignToMeWorkflow')))
 --      or  (( @P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail or [OrgUnitID]=16 )  and r.Status = 103 
 --  AND   ( r.WorkflowProcess='AssignWorkflow' or  r.WorkflowProcess = 'AssignToMeWorkflow')))
   or    (@P_UserID = L.CreatedBy and r.WorkflowProcess='SubmissionWorkflow')))

   end

   else if(@P_Type = 2)  --My needmoreinfo records
   begin

     insert into @Result
            SELECT L.[LegalID],L.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = L.SourceOU) as SourceOU,
			L.[Subject],(select  (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
            L.[CreatedDateTime],
            (select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = L.[CreatedBy]) as Creator,
            (select EmployeeName from UserProfile where UserProfileId=L.SourceName) as SourceName, L.RequestDetails,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
            from [dbo].[Legal] L join @RequestList r on r.[ReferenceNumber] = L.[ReferenceNumber] where L.[DeleteFlag] !=1
       and 
			(((r.WorkflowProcess='ReturnWorkflow' and  @P_UserID = (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail) and  r.Status!=103 ) 
	   and 
	        (r.Status = 104 )) or (@UserEmail in (select W.FromEmail from Workflow as W where W.[ReferenceNumber]= r.ReferenceNumber and W. WorkflowProcess='ReturnWorkflow'))and (r.Status!=105 and r.Status!=103))
	        order by L.[LegalID] desc
   end

	else if(@P_Type = 3) --My closed Records
    begin

      insert into @Result
            SELECT L.[LegalID],L.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = L.SourceOU) as SourceOU,
			L.[Subject],(select  (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
            L.[CreatedDateTime],
            (select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = L.[CreatedBy]) as Creator,
	    (select EmployeeName from UserProfile where UserProfileId=L.SourceName) as SourceName, L.RequestDetails,
		(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
            from [dbo].[Legal] L join @RequestList r on r.[ReferenceNumber] = L.[ReferenceNumber] where  L.[DeleteFlag] !=1
            and  (((@UserEmail = r.FromEmail or @P_UserID = L.CreatedBy) and (r.Status = 105))
			or(@UserEmail in (select W.FromEmail from Workflow as W where W.[ReferenceNumber]= L.ReferenceNumber and W. WorkflowProcess='CloseWorkflow') )) order by L.[LegalID] desc

end

     else if(@P_Type = 5 or @P_Type is null ) --My Pending Request
     begin

      insert into @Result
           SELECT L.[LegalID],L.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = L.[SourceOU]) as SourceOU,
		   L.[Subject],(select  (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
           L.[CreatedDateTime],
           (select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = L.[CreatedBy]) as Creator,
           (select [EmployeeName] from UserProfile where UserProfileId=L.[SourceName]) as SourceName, L.RequestDetails,
		   (select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
           from [dbo].[Legal] L join @RequestList r on r.[ReferenceNumber] = L.[ReferenceNumber] where L.[DeleteFlag] !=1
      and 
	     
        --  ( @P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail )  and r.Status = 104 ) order by L.[LegalID] desc

			 (( @UserEmail = r.ToEmail or @UserEmail = r.DelagateToEmail)and (r.Status=102 or r.WorkflowProcess = 'ReturnWorkflow')and (r.Status!=105))

    end

    else if(@P_Type = 4) --My own request
    begin

    insert into @Result
          SELECT L.[LegalID],L.[ReferenceNumber],( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = L.SourceOU) as SourceOU,
		  L.[Subject],(select  (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
          L.[CreatedDateTime],
          (select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = L.[CreatedBy]) as Creator,
	  (select EmployeeName from UserProfile where UserProfileId=L.SourceName) as SourceName, L.RequestDetails,
	  (select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
          from [dbo].[Legal] L join @RequestList r on r.[ReferenceNumber] = L.[ReferenceNumber] where L.[DeleteFlag] !=1 
	 and
		  L.CreatedBy = @P_UserID  and r.WorkflowProcess !='ReturnWorkflow'order by L.[LegalID] desc
    end

	else if(@P_Type = 6	) --InProgressRequest
	begin

	insert into @Result
			SELECT  L.LegalID, L.[ReferenceNumber],
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = L.SourceOU) as SourceOU
			 ,L.[Subject],
			(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = r.Status) as Status,
			L.[CreatedDateTime],
			(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = L.[CreatedBy]) as Creator,
                        (select EmployeeName from UserProfile where UserProfileId=L.SourceName) as SourceName, L.RequestDetails,
						(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
					when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=L.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as c)
			from [dbo].[Legal] L join @RequestList r   on r.[ReferenceNumber] = L.[ReferenceNumber]  where  L.[DeleteFlag] !=1
			and ((@P_UserID= L.CreatedBy and r.WorkflowProcess not in ( 'SubmissionWorkflow','ReturnWorkflow','CloseWorkflow'))
			   or  (( @P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail or [OrgUnitID]=16 )  and r.Status = 103 
   AND   ( r.WorkflowProcess='AssignWorkflow' or  r.WorkflowProcess = 'AssignToMeWorkflow')
   )) 
		--	or (@P_UserID in (select UserProfileId from [dbo].[UserProfile] where OfficialMailId = r.ToEmail) and r.Status=103 and r.WorkflowProcess='SubmissionWorkflow')
 )
			
			order by L.[CreatedDateTime] desc
			
	end

--select * from @Result

     if(@P_SourceOU != '')
	 delete from @Result where SourceOU not like @P_SourceOU
	 
	 if(@P_Subject != '')
	 delete from @Result where Subject not like '%'+@P_Subject+'%' or Subject  is null

	 if(@P_AttendedBy != '')
	 delete from @Result where AttendedBy not like '%'+@P_AttendedBy+'%'

	 
	if(@P_Label != '')
	delete from @Result where LegalID not in (select  LegalID from @Result as a where(select count(*) from  [dbo].[LegalKeywords] as L 
	where L.[Keywords]  like '%'+@P_Label+'%'  and L.[LegalID] = a.LegalID and  [DeleteFlag] != 1)>0)

	 if(@P_RequestDateFrom is not null)
	 delete from @Result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @Result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='' )
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
		select * from (SELECT row_number() over (Order By ReferenceNumber desc) as slno,
		 * from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%') or (AssignedTo like '%'+@P_SmartSearch+'%') or
		(Subject  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		 ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')   or
		(AttendedBy like '%'+@P_SmartSearch+'%') or (SourceName  like '%'+@P_SmartSearch+'%')or (Details  like '%'+@P_SmartSearch+'%')or
        ((select count(K.Keywords) from LegalKeywords as K where K.LegalID=a.LegalID and (K.DeleteFlag=0 or K.DeleteFlag is null) and k.Keywords like '%'+@P_SmartSearch+'%')>0)
))
	as m	where slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (SourceOU  like '%'+@P_SmartSearch+'%') or (AssignedTo like '%'+@P_SmartSearch+'%') or
		 ((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')   or
		(Subject  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		(AttendedBy  like '%'+@P_SmartSearch+'%') or (SourceName  like '%'+@P_SmartSearch+'%')or (Details  like '%'+@P_SmartSearch+'%')or
                ((select count(K.Keywords) from LegalKeywords as K where K.LegalID=a.LegalID and (K.DeleteFlag=0 or K.DeleteFlag is null) and k.Keywords like '%'+@P_SmartSearch+'%')>0)) 
     end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select  * from (SELECT row_number() over (Order By ReferenceNumber desc ) as slno,
		 * from @result) as m where slno between @StartNo and @EndNo 
	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @Result

END

GO

update M_Lookups set ArDisplayName=N'طلب دورة تدريبية' where LookupsID = 148

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
		else
		begin
			insert into Notification (ServiceID,[ReferenceNumber],Service,Process,LastUpdateDate,IsRead,FromEmail,ToEmail,DelegateToEmail,DelegateFromEmail,IsAnonymous)
			--select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,(select OfficialMailId from UserProfile where DepartmentID=(select DepartmentID from M_Department where OfficialMailId=@P_ToEmail and @P_ToEmail is not null)),@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous				
			
			select @P_ServiceID,@P_ReferenceNumber,@P_Service,@P_WorkflowProcess,GETUTCDATE(),0,@P_FromEmail,@P_ToEmail,@P_DelegateToEmail,@P_DelegateFromEmail,@P_IsAnonymous				
			
		end
END

GO

ALTER PROCEDURE [dbo].[CalendarReport] --[CalendarReport] 1,10 
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int =15,
	@P_CalendarID int=0,
	@P_Method int=2,
	@P_ReferenceNumber nvarchar(250)='',
	@P_EventType nvarchar(250)=null,
	@P_Location nvarchar(250)='',
	@P_DateFrom datetime ='',
	@P_DateTo datetime ='',
	@P_EventRequestor nvarchar(255)='',
	@P_Status nvarchar(250)='',
	@P_SmartSearch nvarchar(250)='',
	@P_Language nvarchar(10)= null
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0, @UserEmail nvarchar(max)=null
	select @UserEmail= OfficialMailId from UserProfile where UserProfileId= @P_UserID

	if(@P_EventType is not null)
	begin
		Select @P_EventType = EventTypeName from M_EventType where EventTypeID=@P_EventType
	end

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;
	declare @result table(

	CalendarID int ,
	ReferenceNumber nvarchar(255),
	EventType nvarchar(255),
	EventRequestor nvarchar(max),
	Datefrom datetime,
	DateTo dateTime,
	Location nvarchar(255),
	UserName nvarchar(255),
	Status nvarchar(255) ,
	ApproverName  nvarchar(max),
	City  nvarchar(max)
	);

		declare @RequestList table(	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	Status nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	DelegateTOEmail nvarchar(max))

	declare @RequestList1 table(	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	Status nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	DelegateTOEmail nvarchar(max),
	ParentReferenceNumber1 nvarchar(max))

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[Status],[FromEmail],[ToEmail],DelegateToEmail
	from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] group by ReferenceNumber) 


	insert into @RequestList1  
	select C.ReferenceNumber,R.WorkflowProcess,R.FromEmail,R.ToEmail,R.Status,R.FromEmail,R.ToEmail,R.DelegateTOEmail,C.ParentReferenceNumber
	 from @RequestList as R right join Calendar c on c.ReferenceNumber = R.ReferenceNumber



	 update a set a.WorkflowProcess= b.WorkflowProcess,a.FromName=b.FromName,a.ToName=b.ToName,a.Status=b.Status
	 ,a.FromEmail=b.FromEmail,a.ToEmail=b.ToEmail,a.DelegateTOEmail=b.DelegateToEmail from @RequestList1 as a  
	 inner join (select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[Status],[FromEmail],[ToEmail],DelegateToEmail
	from [dbo].[Workflow] where  CreatedDateTime in  (select max(CreatedDateTime)
	from [dbo].[Workflow] where WorkflowProcess='SubmissionWorkflow'  group by ReferenceNumber)) as b on a.ParentReferenceNumber1= b.ReferenceNumber where a.WorkflowProcess is null


	insert into @result
	select c.CalendarID,(case when (ParentReferenceNumber is null or ParentReferenceNumber='' ) then c.ReferenceNumber when (ParentReferenceNumber !=null or ParentReferenceNumber!='') then ParentReferenceNumber end) as ReferenceNumber,
	(select (case when @P_Language='AR' then E.[ArEventTypeName] else E.EventTypeName  end) from M_EVentType as E where E.[EventTypeID]=c.EventType)as EventType,
	c.EventRequestor,c.DateFrom,c.DateTo,
	(select(case when @P_Language='AR' then  ArLocationName else LocationName end) from M_Location where LocationID=Location),
	(select EmployeeName from UserProfile where UserProfileId=c.CreatedBy ) as UserName,
    (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where LookupsID = w.Status) as Status
     ,(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =(case when( ParentReferenceNumber is null or ParentReferenceNumber='') then C.ReferenceNumber 
	 when (ParentReferenceNumber !=''or ParentReferenceNumber!=null) then ParentReferenceNumber end)and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=C.CreatedBy)

	 and (Status = 120 ) order by WorkflowID desc) 
	 )),(Select(case when @P_Language='EN' then CityName else ArCityName end)  from M_City where CityID=c.city) as City
	 from [dbo].Calendar c join @RequestList1 w on c.[ReferenceNumber] = w.[ReferenceNumber]   
	where (c.[DeleteFlag] is  null or c.[DeleteFlag] =0) 
     and (c.CreatedBy=@P_UserID and (w.Status=120 or w.Status=123 or w.Status=124 ) )

	insert into @result
	select c.CalendarID,(case when (ParentReferenceNumber is null or ParentReferenceNumber='' ) then c.ReferenceNumber when (ParentReferenceNumber !=null or ParentReferenceNumber!='') then ParentReferenceNumber end) as ReferenceNumber,
	(select (case when @P_Language='AR' then E.[ArEventTypeName] else E.EventTypeName  end) from M_EVentType as E where E.[EventTypeID]=c.EventType)as EventType,
	c.EventRequestor,c.DateFrom,c.DateTo,
	(select(case when @P_Language='AR' then  ArLocationName else LocationName end) from M_Location where LocationID=Location),
	(select EmployeeName from UserProfile where UserProfileId=c.CreatedBy ) as UserName,
    (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where LookupsID = w.Status) as Status
     ,(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =(case when( ParentReferenceNumber is null or ParentReferenceNumber='') then C.ReferenceNumber 
	 when (ParentReferenceNumber !=''or ParentReferenceNumber!=null) then ParentReferenceNumber end)and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=C.CreatedBy)

	 and (Status = 120 ) order by WorkflowID desc) 
	 )),(Select(case when @P_Language='EN' then CityName else ArCityName end)  from M_City where CityID=c.city) as City
	 from [dbo].Calendar c join @RequestList1 w on c.[ReferenceNumber] = w.[ReferenceNumber]   where (c.[DeleteFlag] is  null or c.[DeleteFlag] =0) 
    and ((w.Status =120  and @UserEmail=w.ToEmail) or (w.Status=122  and c.CreatedBy = @P_UserID) 
	or(( @UserEmail = w.ToEmail or @UserEmail = w.DelegateTOEmail)and w.Status=120))

	insert into @result
	select c.CalendarID,(case when (ParentReferenceNumber is null or ParentReferenceNumber='' ) then c.ReferenceNumber when (ParentReferenceNumber !=null or ParentReferenceNumber!='') then ParentReferenceNumber end) as ReferenceNumber,
	(select (case when @P_Language='AR' then E.[ArEventTypeName] else E.EventTypeName  end) from M_EVentType as E where E.[EventTypeID]=c.EventType)as EventType,
	c.EventRequestor,c.DateFrom,c.DateTo,
	(select(case when @P_Language='AR' then  ArLocationName else LocationName end) from M_Location where LocationID=Location),
	(select EmployeeName from UserProfile where UserProfileId=c.CreatedBy ) as UserName,
    (select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where LookupsID = w.Status) as Status
     ,(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =(case when( ParentReferenceNumber is null or ParentReferenceNumber='') then C.ReferenceNumber 
	 when (ParentReferenceNumber !=''or ParentReferenceNumber!=null) then ParentReferenceNumber end)and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=C.CreatedBy)

	 and (Status = 120 ) order by WorkflowID desc) 
	 )),(Select(case when @P_Language='EN' then CityName else ArCityName end)  from M_City where CityID=c.city) as City
	 from [dbo].Calendar c join @RequestList1 w on c.[ReferenceNumber] = w.[ReferenceNumber]   where (c.[DeleteFlag] is  null or c.[DeleteFlag] =0) 
     and(c.CreatedBy=@P_UserID and (w.Status=121 ) )
	
	;WITH CTE AS(
   SELECT 
    RN = ROW_NUMBER()OVER(PARTITION BY CalendarID ORDER BY CalendarID)
   FROM @result
	)
	DELETE FROM CTE WHERE RN > 1
	
	--select * from @result

	-- update a set a.EventRequestor = (select  EventRequestor from Calendar where ReferenceNumber=a.ReferenceNumber )
	-- , a.EventType = (select (select (case when @P_Language='AR' then E.[ArEventTypeName] else E.EventTypeName  end) from M_EventType as E where E.EventTypeID= EventType) from Calendar where ReferenceNumber=a.ReferenceNumber ) from @result as a

	
	if(@P_ReferenceNumber != '')
	 delete from @result where ReferenceNumber not like '%'+@P_ReferenceNumber+'%'
	 
	 if(@P_EventRequestor != '')
	 delete from @result where EventRequestor not like '%'+@P_EventRequestor+'%'

	 if(@P_Location != '')
	 delete from @result where Location not like '%'+@P_Location+'%'

	 if(@P_Status!='')
	 delete from @result where Status not like '%'+@P_Status+'%'

	 if(@P_EventType is not null or @P_EventType !='')
	 delete from @result where EventType  not like '%' +@P_EventType+ '%'

	 if( @P_DateFrom!='' )
     delete from @result where cast( DateFrom as date) < cast(@P_DateFrom as date)

    if( @P_DateTo!='')
     delete from @result where cast(DateTo as date) > cast(@P_DateTo as date)
	--SELECT * FROM @result
     if(@P_SmartSearch != '')
     begin
     select CalendarID ,ReferenceNumber,EventType,EventRequestor,Datefrom,
	DateTo,Location,UserName,Status,ApproverName,City from (SELECT row_number() over (Order By  ReferenceNumber desc) as slno,
         * from @result where
        ((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (EventRequestor  like '%'+@P_SmartSearch+'%') or
        (Location  like '%'+@P_SmartSearch+'%') or  (Status  like '%'+@P_SmartSearch+'%') or       
        ((SELECT  CONVERT(nvarchar(10), cast(DateFrom as date), 103))  like '%'+@P_SmartSearch+'%')  or
        ((SELECT  CONVERT(nvarchar(10), cast(DateTo as date), 103))  like '%'+@P_SmartSearch+'%') or
        (EventType like '%'+@P_SmartSearch+'%')or  (City like '%'+@P_SmartSearch+'%')
		or  (ApproverName like '%'+@P_SmartSearch+'%')))as m 		
	 end

	 if( @P_SmartSearch = '')
	 select * from @result 
  
END	

GO

ALTER PROCEDURE [dbo].[Get_CompensationPreview]  --   Get_CompensationPreview 91
	-- Add the parameters for the stored procedure here
	@P_CompensationID int = 0
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

select @Assignee = (case when q.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.ToEmail) 
					when q.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
					from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b 
					where b.ReferenceNumber=(select ReferenceNumber from Compensation where CompensationID=1) and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
				order by b.WorkflowID desc) as q

declare @EmployeeDetail table(
id int identity(1,1),
name nvarchar(max));

insert into @EmployeeDetail
select concat(b.EmployeeCode,'/',b.EmployeeName) from [dbo].[UserProfile] as b where b.UserProfileId in (select a.UserID from [dbo].[OfficialTaskEmployeeName] as a where a.OfficialTaskID=(select OfficialTaskID from Compensation where CompensationID=@P_CompensationID))

DECLARE @EmployeeDetailList NVARCHAR(max);

SELECT @EmployeeDetailList = COALESCE(@EmployeeDetailList + ',', '') + CAST(name AS VARCHAR)
FROM @EmployeeDetail

insert into @result
select O.ReferenceNumber,C.ReferenceNumber,O.OfficialTaskDescription,C.StartDate,(select Up.EmployeeName from UserProfile as UP where UP.UserProfileID = O.CreatedBy)
,(select Up.EmployeePosition from UserProfile as UP where UP.UserProfileID = O.CreatedBy) ,(@Assignee)
,(select Up.EmployeeCode from UserProfile as UP where UP.EmployeeName = @Assignee),C.NumberofDays,@EmployeeDetailList from Compensation as C inner join OfficialTask as O on C.OfficialTaskID=O.OfficialTaskID 
 where C.CompensationID = @P_CompensationID

select * from @result

END

GO

ALTER PROCEDURE [dbo].[Get_MyPendingListInHRCount]  --[Get_MyPendingListInHR] 1,20,3,1
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_UserID int = 0,
	@P_Method int = 0,
	@P_Username nvarchar(250) = null,
	@P_Creator nvarchar(250) = null,
	@P_Status nvarchar(250) = null,
	@P_RequestType nvarchar(250) = null,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) = null 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 
	--declare @P_UserID int = 2, @MinCount int, @MaxCount int , @P_RequestType nvarchar(250) = 'Training'

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --select @UserName
	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  --GET HR managers
	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1
	  --select * from @HRManagers
	  --GET HR users
	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)
	  --select * from @HRUsers

	 declare @RequestList table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)

	declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int
    )

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] group by ReferenceNumber) 
	 declare @RequestList1 table(
	
	ReferenceNumber nvarchar(max),
	--id int,
	--Creator nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255)
	)
	insert into @RequestList1
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where Service='OfficialTask' and ((WorkflowProcess='SubmissionWorkflow' and ToName=@UserName)or(WorkflowProcess='CloseWorkflow' and FromName=@UserName))
	
	-- For Training Request
		
		insert into @Result
			SELECT  t.TrainingID,t.[TrainingReferenceID],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           		 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
            		from [dbo].[Training] t join @RequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( ((w.Status = 42 or w.Status = 43 or w.Status = 44) and w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))
			or (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=42))
		
--	select * from @Result
	-- For Announcements Request

			insert into @Result
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Announcement] a join @RequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (w.Status = 36 or w.Status = 37)

	-- For HRComplaintSuggestions Request

		insert into @Result
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
	
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			from [dbo].[HRComplaintSuggestions] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (w.Status = 47 or w.Status = 48)

	-- For Baby Addition Request

		insert into @Result
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			from [dbo].[BabyAddition] b join @RequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (w.Status = 39 or w.Status = 40)

	-- For Experience Certificate Request

		insert into @Result
			SELECT  e.CertificateID,e.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0
			from [dbo].[Certificate] e join @RequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (w.Status = 33 or w.Status = 34)


	-- For Salary Certificate Request

			insert into @Result
			SELECT  s.CertificateID,s.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			from [dbo].[Certificate] s join @RequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (w.Status = 33 or w.Status = 34)

			

	-- For Leave Request

		insert into @Result
			SELECT  l.LeaveID,l.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 
          		  ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            
            
			from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ( (( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	 and w.Status=7) or ((w.Status = 7 or w.Status = 8  or w.Status =10)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID)))
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @Result
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			, 
			0,0,0
			from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
				((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
				and(113=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @Result
			SELECT  C.CompensationID,C.[ReferenceNumber],
	
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
				(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            		,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
          		 from [dbo].[Compensation] C join @RequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   and (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and( (( ( @UserEmail = w.ToEmail
	or 
	  @UserEmail = W.DelegateTOEmail)
	  and w.Status =108)or ((w.Status = 108 or w.Status = 109 or w.Status =110)and
	 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))))

			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @Result where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @Result where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @Result where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end

	 if(@P_RequestDateFrom is not null)
	 delete from @result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_RequestDateTo is not null)
	 delete from @result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_Status !='')
	 begin
		delete from @result where Status !=  @P_Status
	 end

	if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end 

	 if(@P_RequestType !='')
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
			select * from (SELECT row_number() over (Order By RequestDate desc) as slno, * from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		 (Status  like '%'+@P_SmartSearch+'%')) ) as m where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or  (RequestType  like '%'+@P_SmartSearch+'%') or (Creator  like '%'+@P_SmartSearch+'%') 
		or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		  (Status  like '%'+@P_SmartSearch+'%') ) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (select ROW_NUMBER() over (order by RequestDate desc) as slno, * from @Result)as m  
	 where slno between @StartNo and @EndNo
	 Order By UpdatedDateTime desc 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
END

GO

ALTER PROCEDURE [dbo].[HRReport] --HRReport 1,10,0,1,'','',
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Method int = 0,
	@P_UserID int = 0,
	@P_Username nvarchar(max) = null ,
	@P_Creator nvarchar(max) = null,
	@P_Status nvarchar(max) = null,
	@P_RequestType nvarchar(250) = 0,
	@P_RequestDateFrom datetime = null,
	@P_RequestDateTo datetime = null,
	@P_SmartSearch nvarchar(max) = null,
	@P_Language nvarchar(10) ='AR'
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


    -- Insert statements for procedure here

	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null, @P_OrgUnit int = 0;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;


	declare @UserName nvarchar(250), @UserEmail nvarchar(250)
	 SET @UserName = (Select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	  SET @P_OrgUnit = (Select OrgUnitID from [dbo].[UserProfile] where UserProfileId = @P_UserID)

	  if(@P_Status='All' or @P_Status='الكل')
	  set @P_Status= null

	  declare @HRManagers table(
	  ManagerUserID int,
	  UserEmail nvarchar(250))

	  declare @HRUsers table(
	  UserID int,
	  UserEmail nvarchar(250))

	  insert into @HRManagers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and [IsOrgHead] = 1

	  insert into @HRUsers
	  select UserProfileId,OfficialMailId from [dbo].[UserProfile] where [OrgUnitID] = 9 and ([IsOrgHead] is null or [IsOrgHead] =0)

	 declare @Result table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime)

	 declare @RequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	insert into @RequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from [dbo].[Workflow] where WorkflowID in  (select max(WorkflowID)
	from [dbo].[Workflow] group by ReferenceNumber)


		 declare @LeaveRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @CertificateRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @BabyAdditionRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @TrainingRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @AnnouncementRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @OfficialRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	declare @ComplaientRequestList table(	
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	FromEmail nvarchar(255),
	ToEmail nvarchar(255),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max))

	
		;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Leave'
		)
	insert into @LeaveRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Certificate'
		)
	insert into @CertificateRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='BabyAddition'
		)
	insert into @BabyAdditionRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Training'
		)
	insert into @TrainingRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='Announcement'
		)
	insert into @AnnouncementRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service='HRComplaintSuggestions'
		)
	insert into @ComplaientRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1


	
	;WITH CTE AS 
		(
		SELECT *, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  Workflow where Service in ('Compensation','OfficialTask')
		)
	insert into @OfficialRequestList
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],FromEmail,ToEmail,[Status],DelegateTOEmail
	from CTE where RowNumber=1

	

	 declare @MyOwnResult table(
	RequestID int,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	UpdatedDateTime datetime,
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max)
	)

	--Get own request 
		insert into @MyOwnResult
	select distinct t.TrainingID,t.[TrainingReferenceID],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = t.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end)  from [dbo].[M_Lookups] where Module = 'Training' AND Category = 'Status' AND LookupsID = w.Status)as Status,
	t.[CreatedDateTime],
	(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
	(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 42 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.ApproverDepartmentID)
	  as ApproverDepartment
		from [dbo].[Training] t join @TrainingRequestList w on t.TrainingReferenceID = w.ReferenceNumber and t.DeleteFlag is not null and t.CreatedBy = @P_UserID and w.Status!=44

	insert into @MyOwnResult
	select distinct l.LeaveID,l.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = l.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Leave' AND Category = 'Status' AND LookupsID = w.Status ) as Status,
	l.[CreatedDateTime],
	(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
	(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	 (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
	 ))as ApproverName,
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID=l.ApproverDepartmentID ) as ApproverDepartment
	from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.ReferenceNumber and l.DeleteFlag is not null and l.CreatedBy = @P_UserID and w.Status != 10

	insert into @MyOwnResult
	select distinct b.BabyAdditionID,b.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = b.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'BabyAddition' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	b.[CreatedDateTime],
	(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
	from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.ReferenceNumber and b.DeleteFlag is not null and b.CreatedBy = @P_UserID

	insert into @MyOwnResult
	select distinct s.CertificateID,s.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = s.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	s.[CreatedDateTime],
	(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
	from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.ReferenceNumber and s.DeleteFlag is not null 
	and s.CreatedBy = @P_UserID and s.CertificateType = 'Salary'
	
	insert into @MyOwnResult
	select distinct e.CertificateID, e.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = e.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	e.[CreatedDateTime],
	(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
	from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.ReferenceNumber and e.DeleteFlag is not null 
	and e.CreatedBy = @P_UserID and e.CertificateType = 'Experience'
	
	insert into @MyOwnResult
	select distinct h.HRComplaintSuggestionsID,h.[ReferenceNumber],
	case when h.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = h.[CreatedBy]) end as Creator,
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'HRComplaintSuggestions' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	h.[CreatedDateTime],
	(case when (h.UpdatedDateTime is null) then h.[CreatedDateTime] else h.UpdatedDateTime end) as UpdatedDateTime,
	null,h.Source as SourceName ,null,null
	from [dbo].[HRComplaintSuggestions] h join @ComplaientRequestList w on h.[ReferenceNumber] = w.ReferenceNumber and h.DeleteFlag is not null 
	and h.CreatedBy = @P_UserID 

	insert into @MyOwnResult
	select distinct a.AnnouncementID,a.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = a.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Announcement' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	a.[CreatedDateTime],
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null	
	from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.ReferenceNumber and a.DeleteFlag is not null 
	and a.CreatedBy = @P_UserID 

		insert into @MyOwnResult
	select distinct OT.OfficialTaskID,OT.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = OT.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'OfficialTask' AND Category = 'Status' AND LookupsID = 	(select case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber))
	then 107 else 113 end) ) as Status,
	OT.[CreatedDateTime],
	(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
	from [dbo].[OfficialTask] as OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.ReferenceNumber and OT.DeleteFlag is not null 
	and OT.CreatedBy = @P_UserID 

	insert into @MyOwnResult
	select distinct C.CompensationID,C.[ReferenceNumber],
	(select [EmployeeName] from [dbo].[UserProfile] where [UserProfileId] = C.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Compensation' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	C.[CreatedDateTime],
	(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime ,			
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
	( select EmployeeName from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
	(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
	 ))as ApproverName,
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ( select OrgUnitID from [UserProfile] where OfficialMailId=(
	  (select top 1 ToEmail from @OfficialRequestList
	 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108)) 
	 ))) as ApproverDepartment
	from [dbo].[Compensation] as C join @OfficialRequestList w on C.[ReferenceNumber] = w.ReferenceNumber --and C.DeleteFlag is not null 
	and C.CreatedBy = @P_UserID and w.WorkflowProcess !='ReturnWorkflow'


	insert into @Result
	select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @MyOwnResult	
	
	
	 declare @MyProcessedResult table(
		RequestID int ,
		ReferenceNumber nvarchar(max),
		Creator nvarchar(max),
		RequestType nvarchar(max),
		Status nvarchar(max),
		RequestDate datetime,
		WorkflowProcess nvarchar(max),
		FromName nvarchar(250),
		ToName nvarchar(250),
		StatusCode int,
		UpdatedDateTime datetime,
		IsCompensationAvaliable bit ,
		DelegateUser int,
    	Approver int,
		AssignedTo nvarchar(max))

	insert into @MyProcessedResult
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           	,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
            from [dbo].[Training] t join @TrainingRequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( (	 @P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))

		
			insert into @MyProcessedResult
			SELECT  a.AnnouncementID,a.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

			insert into @MyProcessedResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 			,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
            from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and(@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber 
			and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))
		
			insert into @MyProcessedResult
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			,(case when 
			(((select top 1[FromEmail] from Workflow where [ReferenceNumber] = OT.[ReferenceNumber] and status = 107) = @UserEmail
			and DATEADD(DAY, 5, OT.CreatedDateTime) > (select GETDATE()) ) and ((select count(*) from Compensation where OfficialTaskID=OT.OfficialTaskID)=0))
			then 1 else 0 end),0,0,null
			from [dbo].[OfficialTask] OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
			((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
			and (w.Status <> 107 and w.WorkflowProcess!='SubmissionWorkflow')) or (w.Status= 107  and W.FromEmail= (select OfficialMailId from UserProfile where UserProfileId=@P_UserID)))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		    insert into @MyProcessedResult
			SELECT  C.CompensationID,C.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
			,(select (case when t.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.ToEmail) 
			when t.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=c.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as t)
          	from [dbo].[Compensation] C join @OfficialRequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   and (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

			-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @MyProcessedResult where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @MyProcessedResult where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @MyProcessedResult where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end	
	
	insert into @Result
	select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @MyProcessedResult



	declare @MyPengdingResult table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int ,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime,
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	StatusCode int,
	UpdatedDateTime datetime,
	IsCompensationAvaliable bit ,
	DelegateUser int,
    	Approver int,
	AssignedTo nvarchar(max),
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max)
    )


	 declare @RequestList1 table(
	ReferenceNumber nvarchar(max),
	WorkflowProcess nvarchar(max),
	FromName nvarchar(250),
	ToName nvarchar(250),
	ToEmail nvarchar(250),
	Status nvarchar(250),
	DelegateTOEmail nvarchar(max),
	FromEmail nvarchar(255))

	insert into @RequestList1
	select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[ToEmail],[Status],DelegateTOEmail,FromEmail
	from [dbo].[Workflow] 
	where Service='OfficialTask' and ((WorkflowProcess='SubmissionWorkflow' and ToName=@UserName)or(WorkflowProcess='CloseWorkflow' and FromName=@UserName))
	--select * from @RequestList1
	-- For Training Request
	insert into @MyPengdingResult
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID)) as DelegateUser
           	 ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=t.TrainingReferenceID and Status=42) ) as Approver
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			(select top 1 ToEmail from [Workflow]
			 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
			)) as ApproverName,
			(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= t.ApproverDepartmentID) as ApproverDepartment
			from [dbo].[Training] t join @TrainingRequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( (((w.Status = 42 or w.Status = 43 or w.Status = 44) and w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))
			or (( @UserEmail = w.ToEmail or 
		   @UserEmail = W.DelegateTOEmail)
			and w.Status=42)))
		
--	select * from @Result
	-- For Announcements Request

			insert into @MyPengdingResult
			SELECT  a.AnnouncementID,a.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and ((w.Status = 36 or w.Status = 37))

	-- For HRComplaintSuggestions Request

		insert into @MyPengdingResult
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			null,l.Source as SourceName ,null,null
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ((w.Status = 47 or w.Status = 48))

	-- For Baby Addition Request

			insert into @MyPengdingResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and( (w.Status = 39 or w.Status = 40))

	-- For Experience Certificate Request

		insert into @MyPengdingResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and ((w.Status = 33 or w.Status = 34))


	-- For Salary Certificate Request

			insert into @MyPengdingResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and ((w.Status = 33 or w.Status = 34))
			

	-- For Leave Request

		insert into @MyPengdingResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=l.ReferenceNumber)) as DelegateUser
 			,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=l.ReferenceNumber) and status=7) as Approver
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
			))as ApproverName,
			(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= l.ApproverDepartmentID) as ApproverDepartment
			from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ( ((( @UserEmail = w.ToEmail or @UserEmail = W.DelegateTOEmail)
			and w.Status=7) or ((w.Status = 7 or w.Status = 8 or w.Status =10)and
			 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))) )
		
		--select * from @Result 	
		-- For OfficialTask Request
			insert into @MyPengdingResult
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
			then 107 else 113 end) as StatusCode,
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			,(case when 
			(((select [FromEmail] from Workflow where [ReferenceNumber] = OT.[ReferenceNumber] and status = 107 and FromName=@UserName) = @UserEmail
			and DATEADD(DAY, 5, OT.CreatedDateTime) > (select GETDATE()) ) and ((select count(*) from Compensation where OfficialTaskID=OT.OfficialTaskID)=0))
			then 1 else 0 end),0,0,null,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
			from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
			((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
			and 
			(w.Status=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
			then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @MyPengdingResult
			SELECT  C.CompensationID,C.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,(select UserProfileId from UserProfile where OfficialMailId=(Select DelegateTOEmail from @RequestList where ReferenceNumber=c.ReferenceNumber)) as DelegateUser
            ,(select UserProfileId from UserProfile where OfficialMailId=(Select ToEmail from @RequestList where ReferenceNumber=c.ReferenceNumber) and Status=108) as Approver
			,(select (case when t.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.ToEmail) 
			when t.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=c.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as t) ,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			 (select top 1 ToEmail from [Workflow]
			where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
			 ))as ApproverName,
			 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ( select OrgUnitID from [UserProfile] where OfficialMailId=(
			 (select top 1 ToEmail from @RequestList
			 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108)) 
			 )))as ApproverDepartment
          	 from [dbo].[Compensation] C join @OfficialRequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]  
			and( (( ( @UserEmail = w.ToEmail or 
			@UserEmail = W.DelegateTOEmail)
			 and w.Status =108)or ((w.Status = 108 or w.Status = 109 or w.Status =110)and
			 w.ToEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId =@P_UserID))))

	-- Normal RRC Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID  not in (select UserID from @HRUsers))
	begin
	delete from @MyPengdingResult where ToName != @UserName and StatusCode <> 107 and(( @P_UserID  != DelegateUser) and (@P_UserID != Approver))
	end
	--select * from @Result
	-- HR Managers
	if(@P_UserID in (select ManagerUserID from @HRManagers))
	begin
	delete from @MyPengdingResult where (StatusCode = 7 or StatusCode = 42)
	end

	-- HR Users
	if(@P_UserID not in (select ManagerUserID from @HRManagers) and @P_UserID in (select UserID from @HRUsers))
	begin
	delete from @MyPengdingResult where (StatusCode = 7 and StatusCode = 42) and (ToName not in (select ManagerUserID from @HRManagers)) 
	and (ToName != @UserName)
	end


		insert into @Result
		select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @MyPengdingResult	


	if(@P_OrgUnit = 9)
	begin

		declare @HRResult table(
		RequestID int,
		ReferenceNumber nvarchar(max),
		Creator nvarchar(max),
		RequestType nvarchar(max),
		Status nvarchar(max),
		RequestDate datetime,
		WorkflowProcess nvarchar(max),
		FromName nvarchar(250),
		ToName nvarchar(250),
		StatusCode int,
		Assignee nvarchar(255),
		UpdatedDateTime datetime,
		IsCompensationAvaliable bit,
		AssignedTo nvarchar(255),
		SourceOU nvarchar(max),
		SourceName nvarchar(max),
		ApproverName nvarchar(max),
		ApproverDepartment nvarchar(max))

					insert into @HRResult
			SELECT  a.AnnouncementID,a.[ReferenceNumber],
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			( case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
	
			insert into @HRResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  t where t.ReferenceNumber=b.ReferenceNumber and t.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by t.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
	 
			insert into @HRResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=s.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' 	
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=e.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			(select EmployeeName from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			)or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			(select top 1 ToEmail from [Workflow]
			 where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
			 ))as ApproverName, (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= l.ApproverDepartmentID) as ApproverDepartment
			from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and( ((w.Status = 7 and w.ToEmail = @UserEmail) or
			(w.Status = 8 and (l.IsHrHeadApproved = 0 or l.IsHrHeadApproved is null) and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 and US.IsOrgHead =1))) or
			(w.Status = 8 and w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and l.IsHrHeadApproved = 1 and 
			(@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 ))) or
			(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow','ReturnWorkflow') and (@UserEmail = W.ToEmail)) or 
			(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in 
			(select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 ))) or (( @UserEmail = w.ToEmail or @UserEmail = w.DelegateTOEmail)and w.Status=7))
			or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			t.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.[TrainingReferenceID] and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			(select EmployeeName from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			(select top 1 ToEmail from [Workflow]
			 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 42 ) order by WorkflowID desc) 
			))as ApproverName,
			(select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= t.ApproverDepartmentID) as ApproverDepartment
			from [dbo].[Training] t join @TrainingRequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and (((w.Status = 42 and w.ToEmail = @UserEmail) or
			(w.Status = 43 and (t.IsHrHeadApproved = 0 or t.IsHrHeadApproved is null) and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 and US.IsOrgHead =1))) or
			(w.Status = 43 and w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and t.IsHrHeadApproved = 1 and 
			(@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 ))) or
			(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow','ReturnWorkflow') and (@UserEmail = W.ToEmail)) or 
			(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in 
			(select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9))) or
			(( @UserEmail = w.ToEmail or @UserEmail = w.DelegateTOEmail)and w.Status=42))
			or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
	
			insert into @HRResult
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			null,l.Source as SourceName ,null,null
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9)))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			OT.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='SubmissionWorkflow' then (select w.ToEmail) else null end),
			(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime
			,(case when 
			(((select top 1[FromEmail] from Workflow where [ReferenceNumber] = OT.[ReferenceNumber] and status = 107) = @UserEmail
			and DATEADD(DAY, 5, OT.CreatedDateTime) > (select GETDATE()) and ((select count(*) from Compensation where OfficialTaskID=OT.OfficialTaskID)=0)))
			then 1 else 0 end),null,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
			from [dbo].[OfficialTask] OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.[ReferenceNumber]  
			 where (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)  and
			 ((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
			and (w.Status <> 107)) or (w.Status= 107  and W.FromEmail= (select OfficialMailId from UserProfile where UserProfileId=@P_UserID)))
			
			insert into @HRResult
			SELECT  C.CompensationID,C.[ReferenceNumber],(select [EmployeeName] from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 186) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			C.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime
			,0,
			(select (case when q.WorkflowProcess='AssignWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.ToEmail) 
			when q.WorkflowProcess='AssignToMeWorkflow' then (select EmployeeName from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=c.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as q) ,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select EmployeeName from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
			(select EmployeeName from [dbo].[UserProfile] where OfficialMailId = (
			(select top 1 ToEmail from [Workflow]
			 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108 ) order by WorkflowID desc) 
			 )) as ApproverName,
			 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ( select OrgUnitID from [UserProfile] where OfficialMailId=(
				(select top 1 ToEmail from @RequestList
			 where ReferenceNumber =c.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=c.CreatedBy) and (Status = 108)) 
			 )))as ApproverDepartment
			from [dbo].[Compensation] C join @OfficialRequestList w on C.[ReferenceNumber] = w.[ReferenceNumber]   where (C.[DeleteFlag] is not null or C.[DeleteFlag] !=0) 
			and ( ((w.Status = 108 and w.ToEmail=@UserEmail )
			or (w.status =109 and( w.WorkflowProcess != 'AssignToMeWorkflow' or  w.WorkflowProcess !='AssignWorkflow'or w.WorkflowProcess !='CloseWorkflow')
			and @P_UserID in(select UserProfileId from [UserProfile] where OrgUnitID =9 ) and C.IsHrHeadApproved =1  )
			or (w.status =109 and( w.WorkflowProcess != 'AssignToMeWorkflow' or  w.WorkflowProcess !='AssignWorkflow'or w.WorkflowProcess !='CloseWorkflow')
			and @P_UserID in(select UserProfileId from [UserProfile] where OrgUnitID =9 and IsOrgHead = 1 ) and (C.IsHrHeadApproved <>1 or C.IsHrHeadApproved is null)  )
			or ((w.status =109 and w.WorkflowProcess = 'AssignToMeWorkflow' or  w.WorkflowProcess ='AssignWorkflow')or(w.Status =110 and w.WorkflowProcess = 'ReturnWorkflow')
			and ( (@P_UserID in(select UserProfileId from [UserProfile] where OrgUnitID =9)) or w.ToEmail = @UserEmail) ) or (( @UserEmail = w.ToEmail or @UserEmail = w.DelegateTOEmail)and w.Status=108) )
			or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
			--select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @HRResult
	
		insert into @Result
		select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @HRResult	
	end										  
		 --select * from @Result  where RequestType='Leave Requests'

	 ; With CTE_Duplicates as
	(select row_number() over(partition by ReferenceNumber order by ReferenceNumber ) rownumber 
	from @Result  )
	 delete from CTE_Duplicates where rownumber!=1

	 if(@P_RequestDateFrom != '')
	 delete from @Result where cast( RequestDate as date) < cast(@P_RequestDateFrom as date)

	 if(@P_Creator != '')
	 delete from @Result where Creator not like  '%'+@P_Creator+'%'

	 if(@P_RequestDateTo is not null)
	 delete from @Result where cast(RequestDate as date) > cast(@P_RequestDateTo as date)

	 if(@P_RequestType !='' and @P_RequestType is not null)
	 begin
		delete from @result where RequestType != (select( case when @P_Language='AR' then ArDisplayName else DisplayName end) from M_Lookups where Category='HrRequestType' and Module=@P_RequestType)
		
		-- (case when(@P_RequestType = 1) then 'Announcement Requests' when(@P_RequestType = 2) then 'New Baby Addition'
		--when(@P_RequestType = 3) then 'Salary Certificate' when(@P_RequestType = 4) then 'Experience Certificate' when(@P_RequestType = 6) then 'Leave Requests' 
		--when(@P_RequestType = 7) then 'OfficalTask Request' when(@P_RequestType = 8) then 'Training Requests'  when(@P_RequestType = 10) then 'Raise Complaints/Suggestions'
		--end)
	 end

	 if(@P_Status !='' and @P_Status is not null and @P_Status not like 'الكل')
	 begin
		delete from @Result where Status not like  '%'+@P_Status+'%'
	 end
	 
	 if(@P_Creator !='')
	 begin
		delete from @result where Creator !=  @P_Creator and Creator != (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)
	 end 

	

	 /* if(@P_RequestType !='')
	 begin
		delete from @result where RequestType !=  @P_RequestType
	 end */

		if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
		select id,ReferenceNumber,Creator,RequestType,Status,RequestDate,RequestID from (select row_number() over (Order By ReferenceNumber desc ) as slno, * from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%') 
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.Birthday as date), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')
			or ((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
)) as m
		 --where m.slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or (Creator like '%'+@P_SmartSearch+'%') or (RequestType like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(RequestDate as date), 103))  like '%'+@P_SmartSearch+'%')
			or((select (case when l.RequestCreatedBy=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177) 
			when l.RequestCreatedBy=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=171)end) from HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select EmployeeName from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then DepartmentName else ArDepartmentName end) from M_Department join Leave as l on DepartmentID=l.DOADepartmentID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.LeaveType=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=174) 
			when l.LeaveType=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=175)end) from Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Leave as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Leave as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.TraineeName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.TrainingName from  Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Training as l where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select (case when l.TrainingFor=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=176) 
			when l.TrainingFor=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=177)end) from Training as l  where l.TrainingReferenceID=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join OfficialTask as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from OfficialTask as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 
			or((select l.NumberofDays from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.OfficialTaskDescription from  OfficialTask as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType='Announcement Requests' and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.Birthday as date), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType='New Baby Addition' and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select EmployeeName from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType='Official Requests' and (select EmployeePosition from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			  or((select (case when @P_Language='EN' then OfficialTaskRequestName else ArOfficialTaskRequestName end) from M_OfficialTaskRequest join Compensation as l on OfficialTaskRequestID=l.OfficialTaskType where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
				or( (select (case when m.NeedCompensation=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=178))
			when m.NeedCompensation=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=179))
			end) from Compensation as m where m.ReferenceNumber=a.ReferenceNumber   ) like  '%'+@P_SmartSearch+'%')
				or	((SELECT  CONVERT(nvarchar(10), cast(l.StartDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or	((SELECT  CONVERT(nvarchar(10), cast(l.EndDate as date), 103)from Compensation as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
		or((select l.Subject from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		or((select l.Details from  HRComplaintSuggestions as l  where l.ReferenceNumber=a.ReferenceNumber and l.HRComplaintSuggestionsID=a.RequestID)like '%'+@P_SmartSearch+'%')
		
			or((select l.NumberofDays from  Compensation as l  where l.ReferenceNumber=a.ReferenceNumber)like '%'+@P_SmartSearch+'%')or 
			((select (case when l.Attention=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=203) 
			when l.Attention=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=204)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select (case when l.SalaryCertificateClassification=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=205) 
			when l.SalaryCertificateClassification=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=206)end) from Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
)
	 end

	 declare @LatestResult table(
	id INT IDENTITY(1, 1) primary key,
	RequestID int,
	ReferenceNumber nvarchar(max),
	Creator nvarchar(max),
	RequestType nvarchar(max),
	Status nvarchar(max),
	RequestDate datetime
	)

	insert into @LatestResult 
	select RequestID,ReferenceNumber,Creator,RequestType,Status,RequestDate from @Result order by RequestID desc



	 if(@P_Method = 0 and @P_SmartSearch is null)
	select id,ReferenceNumber,Creator,RequestType,Status,RequestDate,RequestID from ( select row_number() over (Order By ReferenceNumber desc ) as slno, id,ReferenceNumber,Creator,RequestType,Status,RequestDate,RequestID from @LatestResult ) as a 
	--where a.slno between @StartNo and @EndNo 
	

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result
	 
END

GO



