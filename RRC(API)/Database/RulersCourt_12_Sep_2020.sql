
ALTER PROCEDURE [dbo].[Get_MemoList] -- [Get_MemoList]1,50,4,1,0,null,null,null,null,'Medium',null,null,null,'EN'
	-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Type nvarchar (250) = null,
	@P_Username nvarchar(250) = null,
	@P_Method int =0,
	@P_Status nvarchar(255) = '',
	@P_SourceOU nvarchar(255) = '',
	@P_DestinationOU nvarchar(255) = '',
	@P_Private nvarchar(255) = '',
	@P_Priority nvarchar(255) = '',
	@P_DateFrom datetime = null,
	@P_DateTo datetime = null,
	@P_SmartSearch nvarchar(Max) = null,
	@P_Language nvarchar(10)= ''

	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
	id int,
	MemoID int,
	ReferenceNumber nvarchar(255),
	Title nvarchar(255),
	SourceOU nvarchar(max),
	SourceName nvarchar(max),
	Details nvarchar(max),
	Destination nvarchar(max),
	Status nvarchar(255),
	StatusCode int,
	CreatedDateTime datetime,
	Private nvarchar(255),
	Priority nvarchar(255),
	ApproverDepartment nvarchar(255),
	ApproverName nvarchar(255),	
	UpdatedDateTime datetime
	);


    -- Insert statements for procedure here
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_Username)
	 --SET @P_Username = (Select UserProfileId from [dbo].[UserProfile] where EmployeeName = @P_Username)
	
	 

	 --create a workflow temp table and inserted latest row depending group by reference number
		 declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		FromMail nvarchar(max),
		ToEmail nvarchar(max),
		Status int,
		DelegateTOEmail nvarchar(max), 
		WorkflowProcess nvarchar(max)
		);

		insert into @Workflow
		select WorkflowID,ReferenceNumber,FromEmail,ToEmail,Status,DelegateToEmail,WorkflowProcess from [dbo].[Workflow] order by WorkflowID desc

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1

	--	select * from @Workflow order by WorkflowID asc

	
		if (@P_Type = '1' or @P_Type ='0')
	begin
	
	--SET @P_Status = (Select LookupsID from [dbo].[M_Lookups] where DisplayName = 'Waiting for Approval' AND Module = 'Memo' AND Category = 'Status')	  
     
	 insert into @result 
    Select  * from (SELECT row_number() over (Order By  [MemoID] ) as slno, 
    a.MemoID,
    a.ReferenceNumber,
    a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName,
   a.Details,
       (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
   (select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
    a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
		(select top 1 [UserProfileId] from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime 
    FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
    where a.DeleteFlag = 0 and 
	(( @P_UserEmail = w.ToEmail or  @P_UserEmail = W.DelegateTOEmail)  and ( w.Status=2 or w.Status=5)) or
	((w.Status = 3 )  and 
	((@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber=a.ReferenceNumber and 
	DeleteFlag =0 and Status=3 and IsRedirect =0 ))
	))) as m 
   	 end

	
	
	

	--Outgoing Memos
	if (@P_Type = '2' or @P_Type ='0')
	begin
	 insert into @result
	 Select  * from (SELECT row_number() over (Order By  [MemoID]) as slno, 
	 a.MemoID,
	 a.ReferenceNumber,
	 a.Title,

   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName,
   a.Details,
	   (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
     (select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
	a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	  (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
	FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
	where a.DeleteFlag = 0 and 
	--(@P_UserEmail != w.ToEmail or (@P_Username = a.CreatedBy and status !=5)) AND
	((a.CreatedBy = @P_Username and w.Status not in (1,5))  or
	@P_UserEmail in (select FromEmail from Workflow where ReferenceNumber = a.ReferenceNumber and WorkflowProcess!='SubmissionWorkflow' and Status NOT IN (1,6)))) as m 
	end
	
	
	
	--Incoming Memos
	if (@P_Type = '3' or @P_Type ='0')
	begin
	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [MemoID]) as slno, 
	a.MemoID,
	a.ReferenceNumber,
	a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName,
	a.Details,
	   (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
     (select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
	a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
  (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
		(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	  (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
   FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and ((w.Status = 6 and W.FromMail = @P_UserEmail)
   OR (w.Status=3 and  @P_Username in (select [UserID] from [dbo].[ShareparticipationUsers]   where [ServiceID]=a.MemoID and [Type] ='Memo' )))
   --or (@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber=a.ReferenceNumber and  DeleteFlag =0 and Status=3 and IsRedirect =0 ))
	) as m 
   end
		

	--Draft Memos
	if (@P_Type = '4')
	begin
	--SET @P_Status=(Select LookupsID from [dbo].[M_Lookups] where DisplayName = 'Draft' AND Module = 'Memo' AND Category = 'Status')
	 
	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [MemoID] desc) as slno,
	 a.MemoID,
	 a.ReferenceNumber,
	 a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
    (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName,a.Details,
	   (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
     (select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
	a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   --(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Priority,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
 (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
	 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime 
   FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
    where a.DeleteFlag = 0 AND a.CreatedBy = @P_Username and w.Status=1) as m 
	 end


	 ;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY ReferenceNumber desc
		) RowNumber
		FROM  @result
		)
		DELETE FROM CTE WHERE RowNumber > 1

	 if(@P_SourceOU != '')
	 delete from @result where SourceOU not like '%'+Rtrim(@P_SourceOU)+'%'

	 if(@P_DestinationOU != '')
	 delete from @result where Rtrim(@P_DestinationOU) not in (select value from string_split(Destination,','))

	 if(@P_Private != '')
	 delete from @result where cast(Private as nvarchar(max)) != RTRIM(@P_Private)

	 if(@P_Priority != '')
	 delete from @result where Priority!= RTrim(@P_Priority) or Priority is null

	 if(@P_DateFrom is not null)
	 delete from @result where cast( CreatedDateTime as date) < cast(@P_DateFrom as date)

	 if(@P_DateTo is not null)
	 delete from @result where cast(CreatedDateTime as date) > cast(@P_DateTo as date)

	 if(@P_Status !='' and @P_Status !='All' and (@P_Type = 2 or @P_Type = 3 or @P_Type =1))
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin	 
		select * from (SELECT row_number() over (Order By ReferenceNumber desc) as slno, * from   @result as R where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceOU  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(100), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		(Private  like '%'+@P_SmartSearch+'%')   or (Priority  like '%'+@P_SmartSearch+'%') or
		((select count(K.MemoKeywordsID) from MemoKeywords as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and K.Keywords like '%'+@P_SmartSearch+'%')>0) or (ApproverDepartment like  '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%')  or (Details like '%'+@P_SmartSearch+'%') or(SourceName like '%'+@P_SmartSearch+'%')
		or ((select count(K.UserID) from MemoDestinationUsers as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
		or ((select count(K.DepartmentID) from MemoDestinationDepartment as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		 )
		)

		as m where slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as R where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceOU  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(100), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		(Private  like '%'+@P_SmartSearch+'%')  or (Priority  like '%'+@P_SmartSearch+'%') or
		((select count(K.MemoKeywordsID) from MemoKeywords as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and K.Keywords like '%'+@P_SmartSearch+'%')>0) or (ApproverDepartment like  '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%') 
		or (Details like '%'+@P_SmartSearch+'%')or(SourceName like '%'+@P_SmartSearch+'%')or ((select count(K.UserID) from MemoDestinationUsers as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
	or ((select count(K.DepartmentID) from MemoDestinationDepartment as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		
	) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 select * from (SELECT row_number() over (Order By ReferenceNumber desc ) as slno, * from @result) as  m where slno between @StartNo and @EndNo 

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result

	 End  


	 GO

	 ALTER PROCEDURE [dbo].[Get_CircularList] --[Get_CircularList]1,10,1,TestUser18
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Type nvarchar (250) = null,
	@P_Username nvarchar(250) = null,
	@P_Method int =0,
	@P_Status nvarchar(255) = '',
	@P_SourceOU nvarchar(255) = '',
	@P_DestinationOU nvarchar(255) = '',
	@P_Priority nvarchar(255) = '',
	@P_DateFrom datetime = null,
	@P_DateTo datetime = null,
	@P_SmartSearch nvarchar(Max) = null,
	@P_Language nvarchar(10) =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
	id int,
	CircularID int,
	ReferenceNumber nvarchar(255),
	Title nvarchar(255),
	SourceOU nvarchar(max),
	Destination nvarchar(max),
	Status nvarchar(255),
    StatusCode int,
	CreatedDateTime datetime,
	Priority nvarchar(255),
	UpdatedDateTime datetime,
	Details nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	SourceName nvarchar(max));

    -- Insert statements for procedure here
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0,@P_UserEmail nvarchar(max)=null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_Username)
	 --SET @P_Username = (Select UserProfileId from [dbo].[UserProfile] where EmployeeName = @P_Username)
	
	 

	 --create a workflow temp table and inserted latest row depending group by reference number
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

		--select * from @Workflow

	
	--My Pending Actions
	if (@P_Type = '1')
	begin
	
	--SET @P_Status = (Select LookupsID from [dbo].[M_Lookups] where DisplayName = 'Waiting for Approval' AND Module = 'Memo' AND Category = 'Status')	  
     
	 insert into @result 
    Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
    a.CircularID,
    a.ReferenceNumber,
    a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
     (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
(select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
    a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime ,
	a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	  	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from [M_Department] where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
		  (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
    FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
    where a.DeleteFlag = 0  
	and( @P_UserEmail = w.ToEmail
	or 
	  @P_UserEmail = W.DelegateTOEmail)
	 and( w.Status=13 or w.Status=16))
    as m 
   	 end
	

	--Outgoing Circular
	if (@P_Type = '2')
	begin
	 insert into @result
	 Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
	 a.CircularID,
	 a.ReferenceNumber,
	 a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
	     (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
(select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
	 a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	 (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime ,
	a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
		 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
	FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
	where a.DeleteFlag = 0 AND (((a.CreatedBy = @P_Username and w.Status<>12 and w.Status!=16) or (w.FromMail=@P_UserEmail and w.Status<>12) or 
	@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber = a.ReferenceNumber)) ) and  (@P_UserEmail != w.ToEmail or @P_Username = a.CreatedBy)) as m 
	end
	
	--Incoming Circular
	if (@P_Type = '3')
	begin
	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
	a.CircularID,
	a.ReferenceNumber,
	a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
	  (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
(select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
   a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime ,
	a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
			  (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
    
   FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and w.Status = 14 
   and (
   (@P_Username in (select UserProfileId from UserProfile where DepartmentID in 
	(select [DepartmentID] from [dbo].[CircularDestinationDepartment] where [CircularID]=a.CircularID and DeleteFlag =0))))
   ) as m 
   end
		

	--Draft Circular
	if (@P_Type = '4')
	begin
	--SET @P_Status=(Select LookupsID from [dbo].[M_Lookups] where DisplayName = 'Draft' AND Module = 'Memo' AND Category = 'Status')
	 
	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno,
	 a.CircularID,
	 a.ReferenceNumber,
	 a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
	  (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
 (select top 1 Status from [Workflow] 
	 where ReferenceNumber =a.ReferenceNumber order by WorkflowID desc) as StatusCode,
   a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime ,
	a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
		  (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
    
   FROM [dbo].[Circular] a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
    where a.DeleteFlag = 0 AND a.CreatedBy = @P_Username and w.Status=12
	) as m 
	 end


	 if(@P_SourceOU != '')
	 delete from @result where SourceOU not like '%'+@P_SourceOU+'%'

	 if(@P_DestinationOU != '')
	 delete from @result where @P_DestinationOU not in (select value from string_split(Destination,','))

	 if(@P_Priority != '')
	 delete from @result where Priority not like @P_Priority  or Priority is null

	 if(@P_DateFrom is not null)
	 delete from @result where cast( CreatedDateTime as date) < cast(@P_DateFrom as date)

	 if(@P_DateTo is not null)
	 delete from @result where cast(CreatedDateTime as date) > cast(@P_DateTo as date)

	 if(@P_Status !='' )
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 if(@P_SmartSearch is not null and @P_Method = 0 )
	 begin
	 select * from (SELECT row_number() over (Order By  [CircularID] desc) as slno,
		 * from @result) as m where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceOU  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		(Details  like '%'+@P_SmartSearch+'%') or(SourceName  like '%'+@P_SmartSearch+'%') or
			(ApproverName  like '%'+@P_SmartSearch+'%') or(ApproverDepartment  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(10), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		((select count(K.DepartmentID) from CircularDestinationDepartment as K where K.CircularID=m.CircularID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		or
		(Priority  like '%'+@P_SmartSearch+'%'))  and slno between @StartNo and @EndNo 
	 end

	  if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as a where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceOU  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		(Details  like '%'+@P_SmartSearch+'%') or(SourceName  like '%'+@P_SmartSearch+'%') or
			(ApproverName  like '%'+@P_SmartSearch+'%') or(ApproverDepartment  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(10), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		((select count(K.DepartmentID) from CircularDestinationDepartment as K where K.CircularID=a.CircularID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		or
		(Priority  like '%'+@P_SmartSearch+'%')) 
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 begin
	 select * from (SELECT row_number() over (Order By  [CircularID] desc) as slno,
		 * from @result) as m where slno between @StartNo and @EndNo 
	 end

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result 

	 End  


	 GO

	 ALTER PROCEDURE [dbo].[CircularReport] --[CircularReport] 1,10,1,16,null,null,null,null,null,null,'TestUser16','EN'
    @P_PageNumber int =1,
	@P_PageSize int =10,
	@P_Method int =2,
	@P_UserID int = 1,
	@P_Status nvarchar(255) = null,
	@P_SourceOU nvarchar(255) = null,
	@P_DestinationOU nvarchar(255) = null,
	@P_Priority nvarchar(255) = null,
	@P_DateFrom datetime = null,
	@P_DateTo datetime = null,
	@P_SmartSearch nvarchar(max)= null, 
	@P_Language nvarchar(10) = 'EN'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		declare  @RefCout int =0, @StartNo int =0, @EndNo int =0, @P_UserEmail nvarchar(255),@P_Username nvarchar(255) ;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;
	 SET @P_UserEmail = (Select [OfficialMailId] from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)
	 SET @P_Username = (Select [UserProfileId] from [dbo].[UserProfile] where [UserProfileId] = @P_UserID)

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

   declare @result table(
	id int,
	CircularID int,
	ReferenceNumber nvarchar(255),
	Title nvarchar(255),
	Source nvarchar(max),
	Destination nvarchar(max),
	Status nvarchar(255),
	CreatedDateTime datetime,
	Priority nvarchar(255),
	UpdatedDateTime datetime,
	Details nvarchar(max),
	ApproverName nvarchar(max),
	ApproverDepartment nvarchar(max),
	SourceName nvarchar(max)
	);
	
 Insert into @result
 Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
    a.CircularID,
    a.ReferenceNumber,
    a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
    (select STUFF(replace((SELECT ',' + 
CAST((case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) AS nVARCHAR(max)) AS [text()]
    FROM  [dbo].M_Department where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID  and DeleteFlag=0)
FOR XML PATH('')),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
    a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime, 
   	a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	  	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from [M_Department] where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
		  (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
    FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
    where a.DeleteFlag = 0  and( @P_UserEmail = w.ToEmail or  @P_UserEmail = W.DelegateTOEmail) and( w.Status=13 or w.Status=16))as m 
   	 
	--Outgoing Circular
	 insert into @result
	 Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
	 a.CircularID,
	 a.ReferenceNumber,
	 a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	 (select STUFF(replace((SELECT ',' + 
CAST((case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) AS nVARCHAR(max)) AS [text()]
    FROM  [dbo].M_Department where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID  and DeleteFlag=0)
FOR XML PATH('')),'&amp;','&'), 1, 1, NULL) )AS Destination,
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
	 a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	 (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime,
	 a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	 (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
		 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName	
	FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
	where a.DeleteFlag = 0 AND (((a.CreatedBy = @P_Username and w.Status<>12 and w.Status!=16) or (w.FromMail=@P_UserEmail and w.Status<>12) or 
	@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber = a.ReferenceNumber)) ) and  (@P_UserEmail != w.ToEmail or @P_Username = a.CreatedBy)) as m  	
	
	--Incoming Circular	
	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [CircularID]) as slno, 
	a.CircularID,
	a.ReferenceNumber,
	a.Title,
   ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF(replace((SELECT ',' + 
CAST((case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) AS nVARCHAR(max)) AS [text()]
    FROM  [dbo].M_Department where DepartmentID in 
	(select DepartmentID from [dbo].CircularDestinationDepartment where CircularID = a.CircularID  and DeleteFlag=0)
FOR XML PATH('')),'&amp;','&'), 1, 1, NULL) )AS Destination,
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Circular' AND Category = 'Status') as Status,
   a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   (case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime,
   a.Details,
	 (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 12 or Status = 13) order by WorkflowID desc) 
	 ))
	  as ApproverName,
	(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,
			  (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from UserProfile where UserProfileId=a.SourceName) as SourceName
    
	FROM dbo.Circular a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and w.Status = 14 
   and (
   (@P_Username in (select UserProfileId from [dbo].[UserProfile] where DepartmentID in (
    (select [DepartmentID] from [dbo].[CircularDestinationDepartment] where [CircularID]=a.CircularID and DeleteFlag =0 )))))
   ) as m 
   
   ;With CTE_Duplicates as
   (select   row_number() over(partition by ReferenceNumber order by ReferenceNumber ) rownumber 
   from @result  )
   delete from CTE_Duplicates where rownumber!=1
   	 
	if(@P_SourceOU != '')
	 delete from @result where Source not like '%'+@P_SourceOU+'%'

	 if(@P_DestinationOU != '')
	 delete from @result where @P_DestinationOU not in (select value from string_split(Destination,','))

	 if(@P_Priority != '')
	 delete from @result where Priority not like @P_Priority

	 if(@P_DateFrom is not null)
	 delete from @result where cast( CreatedDateTime as date) < cast(@P_DateFrom as date)

	 if(@P_DateTo is not null)
	 delete from @result where cast(CreatedDateTime as date) > cast(@P_DateTo as date)

	 if(@P_Status != '' )
	 begin
		delete from @result where Status != @P_Status
     end

	 if(@P_SmartSearch is not null)
	 begin
		 select id,CircularID,ReferenceNumber,Title ,Source,Destination,Status,CreatedDateTime,Priority,UpdatedDateTime from (SELECT row_number() over (Order By  [CircularID] desc) as slno,
		 * from @result) as m where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(Source  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		(Details  like '%'+@P_SmartSearch+'%') or(SourceName  like '%'+@P_SmartSearch+'%') or
			(ApproverName  like '%'+@P_SmartSearch+'%') or(ApproverDepartment  like '%'+@P_SmartSearch+'%') or		
		((SELECT  CONVERT(nvarchar(10), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		((select count(K.DepartmentID) from CircularDestinationDepartment as K where K.CircularID=m.CircularID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		or
		(Priority  like '%'+@P_SmartSearch+'%'))  and slno between @StartNo and @EndNo 
	 end

	 if( @P_SmartSearch is  null)
	 begin
		select * from @result
	 end
END

GO

ALTER  procedure [dbo].[MemoReport] --  [MemoReport]0,0,2,1


    @P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Method int =2,
	@P_UserID int = 0,
	@P_Status nvarchar(255) = '',
	@P_SourceOU nvarchar(255) = '',
	@P_DestinationOU nvarchar(255) = '',
	@P_Private nvarchar(255) = '',
	@P_Priority nvarchar(255) = '',
	@P_DateFrom datetime = null,
	@P_DateTo datetime = null,
	@P_SmartSearch nvarchar(max)= null,
	@P_Language nvarchar(10)= 'EN'
	
	
	as
begin
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@P_UserEmail nvarchar(max) = null;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;
	SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)

	if(@P_Private='All')
	set @P_Private='';

	declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		Status int,
		DelegateTOEmail nvarchar(max),
		FromMail nvarchar(max)
		);

		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,Status,DelegateToEmail,FromEmail from [dbo].[Workflow] 

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1

    declare @result table(
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

-- MyPending Action
insert into @result 
    Select   
    a.MemoID,
    a.ReferenceNumber,
    a.Title,
   ( select (case when @P_Language ='EN' then [DepartmentName] else [ArDepartmentName] end) from M_Department  where [DepartmentID] = a.SourceOU) as SourceOU ,
   (select case when @P_Language='EN' then EmployeeName else AREmployeeName end from UserProfile where UserProfileId=a.SourceName) as SourceName,a.Details,
   (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
    a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where  UserProfileId =a.[CreatedBy]),
	(select STUFF((SELECT ',' + 
	CAST(case when @P_Language='EN' then EmployeeName else AREmployeeName end AS VARCHAR(max)) AS [text()]
    FROM  [dbo].[UserProfile] where UserProfileId in 
	(select [UserID] from [dbo].[MemoDestinationUsers] where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
	FOR XML PATH('')), 1, 1, NULL) ),
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =w.ReferenceNumber and (Status =2) order by WorkflowID desc) )),
	 (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
		(select [UserProfileId] from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName
    FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and 
	(( @P_UserEmail = w.ToEmail or  @P_UserEmail = W.DelegateTOEmail)  and ( w.Status=2 or w.Status=5)) or
	((w.Status = 3 )  and 
	((@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber=a.ReferenceNumber and 
	DeleteFlag =0 and Status=3 and IsRedirect =0 ))
	))

   	 

----Incomming Memos

 insert into @result
	Select  
	a.MemoID,
	a.ReferenceNumber,
	a.Title,
   ( select (case when @P_Language ='EN' then [DepartmentName] else [ArDepartmentName] end) from M_Department  where [DepartmentID] = a.SourceOU) as SourceOU ,
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from UserProfile where UserProfileId=a.SourceName) as SourceName,a.Details,
	(select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
   a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where  UserProfileId =a.[CreatedBy]),
	(select STUFF((SELECT ',' + 
	CAST(case when @P_Language='EN' then EmployeeName else AREmployeeName end AS VARCHAR(max)) AS [text()]
    FROM  [dbo].[UserProfile] where UserProfileId in 
	(select [UserID] from [dbo].[MemoDestinationUsers] where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
	FOR XML PATH('')), 1, 1, NULL) ),
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =w.ReferenceNumber and (Status = 1 or Status =2) order by WorkflowID desc) )),
	 (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
		(select [UserProfileId] from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName
  FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
		where a.DeleteFlag = 0 and 
	--(@P_UserEmail != w.ToEmail or (@P_Username = a.CreatedBy and status !=5)) AND
	((a.CreatedBy = @P_UserID and w.Status not in (1,5))  or
	@P_UserEmail in (select FromEmail from Workflow where ReferenceNumber = a.ReferenceNumber and WorkflowProcess!='SubmissionWorkflow' and Status NOT IN (1,6))) 
	
   
   


--Outgoing memos
 Insert into @result
 Select  
    a.MemoID,
    a.ReferenceNumber,
    a.Title,
   ( select (case when @P_Language ='EN' then [DepartmentName] else [ArDepartmentName] end) from M_Department  where [DepartmentID] = a.SourceOU) as SourceOU ,
   (select case when @P_Language='EN' then EmployeeName else AREmployeeName end from UserProfile where UserProfileId=a.SourceName) as SourceName,a.Details,
  (select STUFF(replace(((SELECT ',' + 
CAST( (case when @P_Language = 'EN' then [DepartmentName] else [ArDepartmentName] end)  AS nvarchar(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].MemoDestinationDepartment where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH(''))),'&amp;','&'), 1, 1, NULL) )AS Destination,
	(select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'Memo' AND Category = 'Status') as Status,
    a.CreatedDateTime,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Private' and Module= a.Private)as Private,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where  UserProfileId =a.[CreatedBy]),
	(select STUFF((SELECT ',' + 
	CAST(case when @P_Language='EN' then EmployeeName else AREmployeeName end AS VARCHAR(max)) AS [text()]
    FROM  [dbo].[UserProfile] where UserProfileId in 
	(select [UserID] from [dbo].[MemoDestinationUsers] where MemoID = a.MemoID and (DeleteFlag is null or DeleteFlag=0))
	FOR XML PATH('')), 1, 1, NULL) ),
	(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =w.ReferenceNumber and (Status = 1 or Status =2) order by WorkflowID desc) )),
	 (select (case when @P_Language ='EN' then DepartmentName else [ArDepartmentName] end) from M_Department where DepartmentID= a.InitalApproverDepartment) as ApproverDepartment,	
		(select [UserProfileId] from [dbo].[UserProfile] where [OfficialMailId]  = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =a.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) and (Status = 1 or Status =2) order by WorkflowID desc) 
	 ))
	  as ApproverName
  FROM dbo.memo a inner join @Workflow w on a.ReferenceNumber = w.ReferenceNumber
   where a.DeleteFlag = 0 and ((w.Status = 6 and W.FromMail = @P_UserEmail)
   OR (w.Status=3 and  @P_UserID in (select [UserID] from [dbo].[ShareparticipationUsers]   where [ServiceID]=a.MemoID and [Type] ='Memo' )))



	if(@P_SourceOU != '' and @P_SourceOU is not null)
	 delete from @result where (SourceName not like '%'+Rtrim(@P_SourceOU)+'%' or SourceName is null)

	 if(@P_DestinationOU != '' and @P_DestinationOU is not null)
	 delete from @result where @P_DestinationOU not in (select value from string_split(Destination,','))

	if(@P_Private != '')
	 delete from @result where cast(Private as nvarchar(max)) != RTRIM(@P_Private)

	 if(@P_Priority != '')
	 delete from @result where Priority!= RTrim(@P_Priority)

	 if(@P_DateFrom is not null)
	 delete from @result where cast( CreationDate as date) < cast(@P_DateFrom as date)

	 if(@P_DateTo is not null)
	 delete from @result where cast(CreationDate as date) > cast(@P_DateTo as date)

	 if(@P_Status != '' )
	 begin
		delete from @result where Status != @P_Status

     end

	 if(@P_Method = 0)
	 begin
		select * from @result-- where id between @StartNo and @EndNo
	 end

	 if(@P_Method = 1)
	 begin
		select count(*) from @result
	 end


	 if(@P_Method = 2 and  (@P_SmartSearch is not null and @P_SmartSearch != ''))
	 begin
		select * from @result as R where  
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceName  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(100), cast(CreationDate as date), 103))  like '%'+@P_SmartSearch+'%')  or
		(Private  like '%'+@P_SmartSearch+'%')   or (Priority  like '%'+@P_SmartSearch+'%') or
		((select count(K.MemoKeywordsID) from MemoKeywords as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and K.Keywords like '%'+@P_SmartSearch+'%')>0) or (ApproverDepartment like  '%'+@P_SmartSearch+'%')
		or(ApproverName like '%'+@P_SmartSearch+'%')  or (Details like '%'+@P_SmartSearch+'%') or(SourceName like '%'+@P_SmartSearch+'%')
		or ((select count(K.UserID) from MemoDestinationUsers as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
		or ((select count(K.DepartmentID) from MemoDestinationDepartment as K where K.MemoID=R.MemoID and (K.DeleteFlag=0 or K.DeleteFlag is null) and (select (case when @P_Language ='EN' then [DepartmentName] else [ArDepartmentName] end) from M_Department where DepartmentID= K.DepartmentID )like '%'+@P_SmartSearch+'%')>0)
		 )
		
	 end

	 if(@P_Method = 2 and  (@P_SmartSearch is  null or @P_SmartSearch =''))
	 begin
		select * from @result
	 end

end


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
	 SET @UserName = (Select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	  SET @P_OrgUnit = (Select OrgUnitID from [dbo].[UserProfile] where UserProfileId = @P_UserID)

	  if(@P_Status='All' or @P_Status='????')
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
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = t.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 148),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end)  from [dbo].[M_Lookups] where Module = 'Training' AND Category = 'Status' AND LookupsID = w.Status)as Status,
	t.[CreatedDateTime],
	(case when (t.UpdatedDateTime is null) then t.[CreatedDateTime] else t.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =t.TrainingReferenceID and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=t.CreatedBy) and (Status = 42 ) order by WorkflowID desc) 
	 ))
	  as ApproverName,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.ApproverDepartmentID)
	  as ApproverDepartment
		from [dbo].[Training] t join @TrainingRequestList w on t.TrainingReferenceID = w.ReferenceNumber and t.DeleteFlag is not null and t.CreatedBy = @P_UserID and w.Status!=44

	insert into @MyOwnResult
	select distinct l.LeaveID,l.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = l.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Leave' AND Category = 'Status' AND LookupsID = w.Status ) as Status,
	l.[CreatedDateTime],
	(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
	 (select top 1 ToEmail from [Workflow]
	 where ReferenceNumber =l.ReferenceNumber and FromEmail = (select OfficialMailId from [dbo].[UserProfile] where UserProfileId=l.CreatedBy) and (Status = 7 or Status = 8) order by WorkflowID desc) 
	 ))as ApproverName,
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID=l.ApproverDepartmentID ) as ApproverDepartment
	from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.ReferenceNumber and l.DeleteFlag is not null and l.CreatedBy = @P_UserID and w.Status != 10

	insert into @MyOwnResult
	select distinct b.BabyAdditionID,b.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = b.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'BabyAddition' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	b.[CreatedDateTime],
	(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
	from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.ReferenceNumber and b.DeleteFlag is not null and b.CreatedBy = @P_UserID

	insert into @MyOwnResult
	select distinct s.CertificateID,s.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = s.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	s.[CreatedDateTime],
	(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
	from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.ReferenceNumber and s.DeleteFlag is not null 
	and s.CreatedBy = @P_UserID and s.CertificateType = 'Salary'
	
	insert into @MyOwnResult
	select distinct e.CertificateID, e.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = e.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Certificate' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	e.[CreatedDateTime],
	(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
	from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.ReferenceNumber and e.DeleteFlag is not null 
	and e.CreatedBy = @P_UserID and e.CertificateType = 'Experience'
	
	insert into @MyOwnResult
	select distinct h.HRComplaintSuggestionsID,h.[ReferenceNumber],
	case when h.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = h.[CreatedBy]) end as Creator,
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'HRComplaintSuggestions' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	h.[CreatedDateTime],
	(case when (h.UpdatedDateTime is null) then h.[CreatedDateTime] else h.UpdatedDateTime end) as UpdatedDateTime,
	null,h.Source as SourceName ,null,null
	from [dbo].[HRComplaintSuggestions] h join @ComplaientRequestList w on h.[ReferenceNumber] = w.ReferenceNumber and h.DeleteFlag is not null 
	and h.CreatedBy = @P_UserID 

	insert into @MyOwnResult
	select distinct a.AnnouncementID,a.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = a.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Announcement' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	a.[CreatedDateTime],
	(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime,
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null	
	from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.ReferenceNumber and a.DeleteFlag is not null 
	and a.CreatedBy = @P_UserID 

		insert into @MyOwnResult
	select distinct OT.OfficialTaskID,OT.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = OT.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'OfficialTask' AND Category = 'Status' AND LookupsID = 	(select case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber))
	then 107 else 113 end) ) as Status,
	OT.[CreatedDateTime],
	(case when (OT.UpdatedDateTime is null) then OT.[CreatedDateTime] else OT.UpdatedDateTime end) as UpdatedDateTime,	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = ot.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
	from [dbo].[OfficialTask] as OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.ReferenceNumber and OT.DeleteFlag is not null 
	and OT.CreatedBy = @P_UserID 

	insert into @MyOwnResult
	select distinct C.CompensationID,C.[ReferenceNumber],
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where [UserProfileId] = C.[CreatedBy]),
	(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 149),
	(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where Module = 'Compensation' AND Category = 'Status' AND LookupsID = w.Status) as Status,
	C.[CreatedDateTime],
	(case when (C.UpdatedDateTime is null) then C.[CreatedDateTime] else C.UpdatedDateTime end) as UpdatedDateTime ,			
	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
	( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
	(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
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
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
            from [dbo].[Training] t join @TrainingRequestList w on t.[TrainingReferenceID] = w.[ReferenceNumber]   and (t.[DeleteFlag] is not null or t.[DeleteFlag] !=0) 
			and( (	 @P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))

		
			insert into @MyProcessedResult
			SELECT  a.AnnouncementID,a.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c)
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

			insert into @MyProcessedResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))

		insert into @MyProcessedResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
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
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c)
            from [dbo].[Leave] l join @LeaveRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and(@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber 
			and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow')))
		
			insert into @MyProcessedResult
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
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
			SELECT  C.CompensationID,C.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
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
			,(select (case when t.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = t.ToEmail) 
			when t.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
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
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
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
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.TrainingReferenceID and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			SELECT  a.AnnouncementID,a.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 147) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			a.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (a.UpdatedDateTime is null) then a.[CreatedDateTime] else a.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and ((w.Status = 36 or w.Status = 37))

	-- For HRComplaintSuggestions Request

		insert into @MyPengdingResult
			SELECT  l.HRComplaintSuggestionsID,l.[ReferenceNumber],
			case when l.RequestCreatedBy=0 then (select( case when @P_Language='EN' then M.DisplayName else M.ArDisplayName end)  from M_Lookups as M where M.Category='CreatedBy' and M.Module=1)else 
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 150) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime		
			,0,0,0 
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			null,l.Source as SourceName ,null,null
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and ((w.Status = 47 or w.Status = 48))

	-- For Baby Addition Request

			insert into @MyPengdingResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 146) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			b.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (b.UpdatedDateTime is null) then b.[CreatedDateTime] else b.UpdatedDateTime end) as UpdatedDateTime
			,0,0 ,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=b.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and( (w.Status = 39 or w.Status = 40))

	-- For Experience Certificate Request

		insert into @MyPengdingResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 145) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			e.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (e.UpdatedDateTime is null) then e.[CreatedDateTime] else e.UpdatedDateTime end) as UpdatedDateTime
			 ,0,0,0,
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=e.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' and ((w.Status = 33 or w.Status = 34))


	-- For Salary Certificate Request

			insert into @MyPengdingResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 144) as RequestType,
			(select (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			s.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when (s.UpdatedDateTime is null) then s.[CreatedDateTime] else s.UpdatedDateTime end) as UpdatedDateTime
			,0,0,0
			,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=s.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' and ((w.Status = 33 or w.Status = 34))
			

	-- For Leave Request

		insert into @MyPengdingResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
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
            ,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=l.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
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
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
			from [dbo].[OfficialTask] OT join @RequestList1 w on OT.[ReferenceNumber] = w.[ReferenceNumber] where 
			((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
			and 
			(w.Status=(case when ((select count(*) from Workflow as d where d.status=113 and d.ReferenceNumber=OT.ReferenceNumber and ToName=@UserName)=(select count(*) from Workflow as d where d.status=107 and d.ReferenceNumber=OT.ReferenceNumber and FromName=@UserName))
			then 107 else 113 end))))
			 and (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)
		    
		-- For Compensation Request
		    insert into @MyPengdingResult
			SELECT  C.CompensationID,C.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
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
			,(select (case when t.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = t.ToEmail) 
			when t.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = t.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  q where q.ReferenceNumber=c.ReferenceNumber and q.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by q.WorkflowID desc) as t) ,( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = a.[CreatedBy]) as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=a.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = a.SourceName) as SourceName ,null,null
			from [dbo].[Announcement] a join @AnnouncementRequestList w on a.[ReferenceNumber] = w.[ReferenceNumber]   and (a.[DeleteFlag] is not null or a.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
	
			insert into @HRResult
			SELECT  b.BabyAdditionID,b.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = b.[CreatedBy]) as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  t where t.ReferenceNumber=b.ReferenceNumber and t.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by t.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = b.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = b.SourceName) as SourceName ,null,null
			from [dbo].[BabyAddition] b join @BabyAdditionRequestList w on b.[ReferenceNumber] = w.[ReferenceNumber]   and (b.[DeleteFlag] is not null or b.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
	 
			insert into @HRResult
			SELECT  s.CertificateID,s.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = s.[CreatedBy]) as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=s.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = s.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = s.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] s join @CertificateRequestList w on s.[ReferenceNumber] = w.[ReferenceNumber]   and (s.[DeleteFlag] is not null or s.[DeleteFlag] !=0) 
			and s.[CertificateType] = 'Salary' 	
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  e.CertificateID,e.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = e.[CreatedBy]) as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=e.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			(select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = e.SourceOU) as SourceOU ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = e.SourceName) as SourceName ,null,null
			from [dbo].[Certificate] e join @CertificateRequestList w on e.[ReferenceNumber] = w.[ReferenceNumber]   and (e.[DeleteFlag] is not null or e.[DeleteFlag] !=0) 
			and e.[CertificateType] = 'Experience' 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9 )))
			)or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  l.LeaveID,l.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) as Creator,
			(select (case when @P_Language= 'AR' then ArDisplayName else DisplayName end)  from M_Lookups where LookupsID = 143) as RequestType,
			(select  (case when @P_Language='AR' then ArDisplayName else DisplayName end) from [dbo].[M_Lookups] where LookupsID = w.Status) as Status,
			l.[CreatedDateTime] as RequestDate,
			w.WorkflowProcess as WorkflowProcess,
			w.FromName as FromName,
			w.ToName as ToName,
			w.Status as StatusCode,
			(case when w.WorkflowProcess='AssignToMeWorkflow' then (select w.FromEmail)  when w.WorkflowProcess ='AssignWorkflow' then (select w.ToEmail) else null end),
			(case when (l.UpdatedDateTime is null) then l.[CreatedDateTime] else l.UpdatedDateTime end) as UpdatedDateTime
			,0,(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = l.SourceName) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = l.SourceOU) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			SELECT  t.TrainingID,t.[TrainingReferenceID],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = t.[CreatedBy]) as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=t.[TrainingReferenceID] and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),	( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = t.SourceOU) as SourceOU ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = t.SourceName) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = l.[CreatedBy]) end as Creator,
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
			(select (case when c.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.ToEmail) 
			when c.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = c.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=l.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as c),
			null,l.Source as SourceName ,null,null
			from [dbo].[HRComplaintSuggestions] l join @ComplaientRequestList w on l.[ReferenceNumber] = w.[ReferenceNumber]   and (l.[DeleteFlag] is not null or l.[DeleteFlag] !=0) 
			and (((w.WorkflowProcess not in ('AssignToMeWorkflow','AssignWorkflow','CloseWorkflow') and  (@P_UserID in (select U.UserProfileId from UserProfile as U where U.OrgUnitID = 9))) 
			or(w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@UserEmail = W.ToEmail))
			or (w.WorkflowProcess  in ('AssignToMeWorkflow','AssignWorkflow') and (@P_UserID in (select US.UserProfileId from UserProfile as US where US.OrgUnitID = 9)))
			) or (@P_UserEmail in (select P.FromEmail from Workflow as P where P.ReferenceNumber = w.ReferenceNumber and P.WorkflowProcess not in ('SubmissionWorkflow','AssignToMeWorkflow','AssignWorkflow'))))
		
			insert into @HRResult
			SELECT  OT.OfficialTaskID,OT.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = OT.[CreatedBy]) as Creator,
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
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = ot.SourceName) as SourceName ,null,null
			from [dbo].[OfficialTask] OT join @OfficialRequestList w on OT.[ReferenceNumber] = w.[ReferenceNumber]  
			 where (OT.[DeleteFlag] is not null or OT.[DeleteFlag] !=0)  and
			 ((@P_UserID in (select UserID from OfficialTaskEmployeeName where OfficialTaskID = OT.OfficialTaskID) 
			and (w.Status <> 107)) or (w.Status= 107  and W.FromEmail= (select OfficialMailId from UserProfile where UserProfileId=@P_UserID)))
			
			insert into @HRResult
			SELECT  C.CompensationID,C.[ReferenceNumber],(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = C.[CreatedBy]) as Creator,
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
			(select (case when q.WorkflowProcess='AssignWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = q.ToEmail) 
			when q.WorkflowProcess='AssignToMeWorkflow' then (select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile where OfficialMailId = q.FromEmail)  else null end) 
			from (select top 1 FromEmail,ToEmail,WorkflowProcess from Workflow as  b where b.ReferenceNumber=c.ReferenceNumber and b.WorkflowProcess in('AssignWorkflow','AssignToMeWorkflow') 
			order by b.WorkflowID desc) as q) ,
			( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = c.SourceOU) as SourceOU ,
			( select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile  where UserProfileId = c.SourceName) as SourceName ,
			(select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where OfficialMailId = (
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

	 if(@P_Status !='' and @P_Status is not null and @P_Status not like '????')
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
			or((select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
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
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '147') and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.Birthday as date), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '146') and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '146') and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select IIF(@P_Language = 'EN', d.EmployeeName, d.AREmployeeName) from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select IIF(@P_Language = 'EN', d.EmployeePosition, d.AREmployeePosition) from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
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
			or((select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from UserProfile join Leave as l on UserProfileId=l.DOANameID where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
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
			or ((select count(K.AnnouncementType) from Announcement as K where K.AnnouncementID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '147') and (select (case when @P_Language ='EN' then AnnouncementTypeName  end) from AnnouncementTypeAndDescription as d where d.AnnouncementTypeID= K.AnnouncementType )like '%'+@P_SmartSearch+'%')>0)
			or((select l.AnnouncementDescription from  Announcement as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.BabyName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select l.HospitalName from  BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or	((SELECT  CONVERT(nvarchar(10), cast(l.Birthday as date), 103)from BabyAddition as l where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%') 	
			or ((select count(K.CityofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '146') and (select (case when @P_Language ='EN' then CityName else ArCityName  end) from M_City as d where cast(d.CityID as Nvarchar(max))= K.CityofBirth )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.CountryofBirth) from BabyAddition as K where K.BabyAdditionID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '146') and (select (case when @P_Language ='EN' then CountryName else ArCountryName  end) from M_Country as d where d.CountryID= K.CountryofBirth )like '%'+@P_SmartSearch+'%')>0)
			or((select (case when l.Gender=0 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=167) 
			when l.Gender=1 then (select  (case when @P_Language='EN' then DisplayName else ArDisplayName end)from M_Lookups where LookupsID=168)end) from BabyAddition as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select l.Reason from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')or((select(l.CertificateType) from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or((select [To] from  Certificate as l  where l.ReferenceNumber=a.ReferenceNumber)  like '%'+@P_SmartSearch+'%')
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select Grade from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
			or ((select count(K.UserID) from OfficialTaskEmployeeName as K where K.UserID=a.RequestID and a.RequestType=(select IIF(@P_Language = 'EN',DisplayName, ArDisplayName) from M_Lookups where LookupsID = '149') and (select IIF(@P_Language = 'EN', EmployeePosition, AREmployeePosition) from UserProfile as d where d.UserProfileId= K.UserID )like '%'+@P_SmartSearch+'%')>0)
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

/****** Object:  StoredProcedure [dbo].[GlobalSearch]    ******/
ALTER  PROCEDURE [dbo].[GlobalSearch] -- 26,'061-2019-M',1    [GlobalSearch] 1,'1',7,'AR',0,1,10
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
		Exec [CitizenAffairReport] @P_UserID = @P_UserID,@P_SmartSearch=@P_Search

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