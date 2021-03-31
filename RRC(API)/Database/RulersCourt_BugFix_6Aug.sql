
/****** Object:  StoredProcedure [dbo].[Save_DutyTask]    ******/
ALTER PROCEDURE [dbo].[Save_DutyTask]
	@P_TaskID int = null,
	@P_SourceOU nvarchar(255) = null,
	@P_SourceName nvarchar(255) = null,
	@P_Title nvarchar(255) = null,
	@P_StartDate datetime  = null,
	@P_EndDate datetime  = null,
	@P_TaskDetails nvarchar(max)= null,
	@P_LinkToMemo nvarchar(max)= null,
	@P_LinkToLetter nvarchar(max)= null,
	@P_Priority nvarchar(255) = null,
	@P_RemindMeAt datetime = null,
	@P_CreatedBy int = null,
	@P_UpdatedBy  int = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_DeleteFlag int = 0,
	@P_Action nvarchar(100)= null,
	@P_Comment nvarchar(Max) = null,
	@P_AttachmentGuid nvarchar(max) =null,
	@P_AttachmentName nvarchar(max) = null,
	@P_Status int = null,
	@P_AssigneeID int = null,
	@P_DelegateUser int=null,
	@P_DelegateAssignee int=null,
	@P_AssigneeDepartmentId int=null,
	@P_Country int=null,
	@P_Emirates int=null,
	@P_City nvarchar(255)=null
	

AS
BEGIN
	
	SET NOCOUNT ON;

	 declare @P_Referencenumber nvarchar(255) = null,@currentApprover int = null, @DelegateUser int=null
	declare @temp int = null
	set @temp = (SELECT IDENT_CURRENT('DutyTask'))+1
	select  @P_Referencenumber =concat((Right(concat('0','0',@temp),3)),'-',(SELECT YEAR((select GETDATE()))),'-','DT');

	if(@P_Action ='Update')
	set @P_Status = (select [Status] from [DutyTask] where TaskID=@P_TaskID)
	if(@P_Action = 'Submit' or @P_Action='Assign')
	set @P_Status = 30
	if(@P_Action = 'Completed')
	set @P_Status = 31
	if(@P_Action = 'Close')
	set @P_Status = 32


	declare @RequestList table(	
		ReferenceNumber nvarchar(max),
		WorkflowProcess nvarchar(max),
		FromName nvarchar(250),
		ToName nvarchar(250),
		Status nvarchar(250),
		FromEmail nvarchar(255),
		ToEmail nvarchar(255))

		declare @Result table(
		id INT IDENTITY(1, 1) primary key,
		DOANameID int,
		StartDate datetime,
		EndDate datetime,
		Creator int )

		insert into @RequestList
		select  [ReferenceNumber],[WorkflowProcess],[FromName],[ToName],[Status],[FromEmail],[ToEmail]
		from [dbo].[Workflow] where CreatedDateTime in  (select max(CreatedDateTime)
		from [dbo].[Workflow] group by ReferenceNumber) 

		insert into @Result
		SELECT DOANameID,StartDate,EndDate,CreatedBy
		from [dbo].[Leave] l join @RequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
		and (w.Status = 11)

		--select * from @Result

		select @P_DelegateAssignee = [DOANameID] from @Result where Creator = @P_AssigneeID and (select GETDATE()) >= [StartDate] 
		and (select GETDATE()) <= EndDate 

	if(@P_TaskID is null or @P_TaskID =0)
	begin
	insert into [dbo].[DutyTask] 
	([ReferenceNumber],[SourceOU],[SourceName],[Title],[StartDate],[EndDate],[TaskDetails],[Priority],[LinkToMemo],[LinkToLetter],[RemindMeAt],[CreatedBy],[CreatedDateTime],[Status],[AssigneeID],DelegateAssignee,AssigneeDepartment,Country,City,Emirates)
	select @P_Referencenumber,@P_SourceOU,@P_SourceName,@P_Title,(SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_StartDate), 0)))),(SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_EndDate), 0)))),@P_TaskDetails,@P_Priority,@P_LinkToMemo,@P_LinkToLetter, (SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_RemindMeAt), 0)))),@P_CreatedBy,@P_CreatedDateTime,@P_Status,@P_AssigneeID,@P_DelegateAssignee,@P_AssigneeDepartmentId,@P_Country,@P_City,@P_Emirates
	set @P_TaskID = (SELECT IDENT_CURRENT('DutyTask'))

	set @temp = (SELECT IDENT_CURRENT('DutyTask'))
	select  @P_Referencenumber =concat((Right(concat('0','0',@temp),3)),'-',(SELECT YEAR((select GETDATE()))),'-','DT');

	update DutyTask set [ReferenceNumber] =  @P_Referencenumber where TaskID=@P_TaskID


	insert into [dbo].[DutyTaskCommunicationHistory]([TaskID],[Message],[Action],[AttachmentGuid],
	[AttachmentName],[CreatedBy],[CreatedDateTime])
    select @P_TaskID,@P_Comment,@P_Action,@P_AttachmentGuid,@P_AttachmentName,@P_CreatedBy,@P_CreatedDateTime
	end

	

	
	Else
	if(@P_TaskID is not NUll)
	begin
	update [dbo].[DutyTask]  set  Title=@P_Title,StartDate=(SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_StartDate), 0)))),EndDate=(SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_EndDate), 0)))),TaskDetails=@P_TaskDetails
	,Priority=@P_Priority,RemindMeAt= (SELECT DATEADD(MI,1,(select dateadd(D, datediff(D, 0, @P_RemindMeAt), 0)))),DeleteFlag=@P_DeleteFlag,UpdatedBy=@P_UpdatedBy,UpdatedDateTime=@P_UpdatedDateTime,[Status]=@P_Status,[AssigneeID]=@P_AssigneeID
	,[LinkToMemo]=@P_LinkToMemo,[LinkToLetter]=@P_LinkToLetter,DelegateAssignee=@P_DelegateAssignee,AssigneeDepartment=@P_AssigneeDepartmentId,Country=@P_Country,City=@P_City,Emirates=@P_Emirates
	where TaskID=@P_TaskID

		if(@P_UpdatedBy = @P_DelegateAssignee and @P_Action = 'Completed')
			insert into [dbo].[DutyTaskCommunicationHistory]
			 select @P_TaskID,@P_Comment,@P_Action,@P_AttachmentGuid,@P_AttachmentName,@P_AssigneeID,@P_UpdatedDateTime,@P_UpdatedBy
		 else 
			insert into [dbo].[DutyTaskCommunicationHistory]
			 select @P_TaskID,@P_Comment,@P_Action,@P_AttachmentGuid,@P_AttachmentName,@P_UpdatedBy,@P_UpdatedDateTime,null


	end
	select TaskID,ReferenceNumber as ReferenceNumber ,CreatedBy as CreatorID, (case when (UpdatedBy is null) then CreatedBy else UpdatedBy end) as FromID from  [dbo].[DutyTask] where TaskID=@P_TaskID
	

end
