ALTER PROCEDURE [dbo].[Get_LetterInboundHistoryByID]-- 49
	@P_LetterID int = null,
	@P_Language nvarchar(10)= null
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT [HistoryID],[LetterID],[Action],
	case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.ActionBy)
	else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), 
	case when @P_Language = 'AR' then (select ' '+ArDisplayName+' ' from [dbo].[M_Lookups] where DisplayName='On behalf of') else ' on behalf of ' end,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.ActionBy  )) end as 
	 ActionBy,
			[ActionDateTime],[Comments],
			(select IIF(@P_Language = 'AR', AREmployeeName, EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=Escalate_RedirectUser) as Escalate_RedirectUser
			from [dbo].[LettersInboundHistory] as a where [LetterID] = @P_LetterID
END

GO

ALTER PROCEDURE [dbo].[Get_LetterOutboundHistoryByID]-- 49
	@P_LetterID int = null,
	@P_Language nvarchar(10) = 'EN'
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT [HistoryID],[LetterID],[Action], 
	case when [DelegateUser] is null or DelegateUser =0 then (select IIF(@P_Language = 'EN',EmployeeName,AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.ActionBy)
	else concat((select IIF(@P_Language = 'EN',EmployeeName,AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]),
	case when @P_Language = 'AR' then (select ' '+ArDisplayName+' ' from [dbo].[M_Lookups] where DisplayName='On behalf of') else ' on behalf of ' end,
	(select IIF(@P_Language = 'EN',EmployeeName,AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.ActionBy  )) end as 
	 ActionBy,
			[ActionDateTime],[Comments],
			(select IIF(@P_Language = 'AR', AREmployeeName, EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=Escalate_RedirectUser) as Escalate_RedirectUser
			from [dbo].[LettersOutboundHistory]  as a where [LetterID] = @P_LetterID
END

GO

ALTER PROCEDURE [dbo].[Get_LetterOutboundPreview] --  [Get_LetterOutboundPreview] 3
    @P_LetterID int = 0,
    @P_UserID int = 0,
    @P_lang nvarchar(10)
AS
BEGIN
    
    SET NOCOUNT ON;

 

    Declare @ReferenceNumber nvarchar(max) = null, @ApproverEmail nvarchar(max) = null, @ApproverName nvarchar(max),@ApproverDesignation nvarchar(max),
    @ApproverID int,@ApproverDepartment  nvarchar(max) = null,@signatureID nvarchar(max)=null, @SignatureName nvarchar(max)=null,@Title nvarchar(max)=null; 

 

    select @ReferenceNumber=LetterReferenceID from LettersOutbound where LetterID= @P_LetterID

 


    set @ApproverEmail = (select top 1 case when DelegateFromEmail is null then FromEmail else DelegateFromEmail end from Workflow where ReferenceNumber = @ReferenceNumber 
                            and Status in (20,23) order by WorkflowID desc)

 

    if(@ApproverEmail is null)
    set @ApproverEmail = (select top 1 ToEmail from Workflow where ReferenceNumber = @ReferenceNumber 
                            and Status in (18,19) order by WorkflowID desc)
    
    if(@ApproverEmail is not null)
    select @Title = U.Title, @ApproverID=UserProfileId,@ApproverName= case when @P_lang='EN' then EmployeeName else AREmployeeName end,@ApproverDesignation=case when @P_lang='EN' then EmployeePosition else AREmployeePosition end,@ApproverDepartment=
    (select ArDepartmentName from M_Department as M where M.DepartmentID= U.DepartmentID) from UserProfile as U  where OfficialMailId = @ApproverEmail

 

    if((select count(*) from Workflow where ReferenceNumber = @ReferenceNumber and Status in (20,23) )>0)
    begin
    select @SignatureName = U.SignaturePhoto, @signatureID=U.SignaturePhotoID from UserProfile as U  where OfficialMailId = @ApproverEmail
    end
    

 

    SELECT [LetterID],[LetterReferenceID],[LetterTitle],[SourceOU],(select case when @P_lang='EN' then EmployeeName else AREmployeeName end from [UserProfile] where UserProfileId=SourceName)as SourceName,[LetterDetails],[Priority],[LetterDetails],[DocumentClassification],
    [RelatedToIncomingLetter],[CreatedBy] ,[UpdatedBy],[CreatedDateTime],[UpdatedDateTime],NeedReply,
     (select top 1 Status from [Workflow]  where [ReferenceNumber] =l.[LetterReferenceID] order by [WorkflowID] desc) as Status,
    (select STUFF((select (select ','+concat(SenderName,'.',EntityName) AS [text()]  from (
        select  SenderName,EntityName from Contact as c inner join LettersOutboundDestinationEntity as d on c.ContactID=d.EntityID where d.LetterID= @P_LetterID and (d.DeleteFlag is null or d.DeleteFlag =0)) A
        FOR XML PATH ('')) ), 1, 1, NULL) )as DestinationTitle,
      @ApproverName as ApproverNameID, @ApproverID as ApproverID,@ApproverDesignation as ApproverDesignation, l.[InitialApproverDepartment]
       as ApproverDepartmentID, 'Outbound Letter' as LetterType, @signatureID as SignaturePhotoID, @SignatureName as SignaturePhotoName,@Title as ApproverTitle
      from [dbo].[LettersOutbound] as l  WHERE [LetterID] = @P_LetterID
END
GO

ALTER PROCEDURE [dbo].[Get_LetterReportList] 
	-- Add the parameters for the stored procedure here
	
	@P_UserID int = 0,
	@P_Status nvarchar(255) = '',
        @P_Username nvarchar(250) ='',
	@P_SourceOU nvarchar(255) = null,
	@P_DestinationOU nvarchar(255) = null,
	@P_SenderName nvarchar(250) = null,
	@P_Priority nvarchar(255) = null,
	@P_DateFrom datetime = null,
	@P_DateTo datetime = null,
	@P_SmartSearch nvarchar(Max) = '',
	@P_Language nvarchar(10)= 'EN'	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
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
	LetterType nvarchar(255)
	);

    -- Insert statements for procedure here
	declare  @P_UserEmail nvarchar(max)=null;	

	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 --SET @P_Username = (Select UserID from [dbo].[User] where LoginUser = @P_Username)
	
	 declare @LinkToOtherLetter nvarchar(250) = (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131);

	 --create a workflow temp table and inserted latest row depending group by reference number
		 declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		FromEmail nvarchar(max),
		Status int,
		DelegateTOEmail nvarchar(max));

		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,FromEmail, Status,DelegateToEmail from [dbo].[Workflow]  

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1

	 insert into @result
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST(EntityName AS VARCHAR(max)) AS [text()]
    FROM  [dbo].Contact where ContactID in 
		(select EntityID from [dbo].[LettersOutboundDestinationEntity] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
	 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) as UserName,
  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'OutboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
   (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Outbound Letter' as LetterType
    FROM dbo.[LettersOutbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber
    where a.DeleteFlag = 0 and ( @P_UserEmail = w.ToEmail or @P_UserEmail = W.DelegateTOEmail)and( w.Status=19 or w.Status=22)) as m  

	insert into @result
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST(EntityName AS VARCHAR(max)) AS [text()]
    FROM  [dbo].Contact where ContactID in 
		(select EntityID from [dbo].[LettersOutboundDestinationEntity] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
	 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) as UserName,
  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'OutboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
   (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Outbound Letter' as LetterType
    FROM dbo.[LettersOutbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber
where a.DeleteFlag = 0 and (@P_UserEmail != w.ToEmail or @P_UserID = a.CreatedBy) AND(( (a.CreatedBy = @P_UserID and w.Status<>18) or  (w.FromEmail=@P_UserEmail and w.Status<>18 ) or
@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber = a.LetterReferenceID))and w.Status!=22))as m

	insert into @result
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST(EntityName AS VARCHAR(max)) AS [text()]
    FROM  [dbo].Contact where ContactID in 
		(select EntityID from [dbo].[LettersOutboundDestinationEntity] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
	 (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.CreatedBy) as UserName,
  (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
	 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'OutboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
   (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Outbound Letter' as LetterType
    FROM dbo.[LettersOutbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber where a.DeleteFlag = 0 
   and (w.Status = 20 or w.Status = 23) and ((@P_UserID in (select UserProfileId from [dbo].[UserProfile] where [OrgUnitID]=14 and DeleteFlag =0 )))) as m 

	insert into @result 
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST((case when @P_Language = 'EN' then DepartmentName else ArDepartmentName end) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].[LettersInboundDestinationDepartment] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
 (select STUFF((SELECT ',' + 
CAST(IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].UserProfile where UserProfileId in 
	(select UserID from [dbo].LettersInboundDestinationUsers where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) )AS UsereName,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130) 
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
		 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'InboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
    (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Inbound Letter' as LetterType
	FROM dbo.[LettersInbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber
    where a.DeleteFlag = 0 and( @P_UserEmail = w.ToEmail
	or  @P_UserEmail = W.DelegateTOEmail) and (w.Status=25 or w.Status=28))as m 


	insert into @result 
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST((case when @P_Language = 'EN' then DepartmentName else ArDepartmentName end) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].[LettersInboundDestinationDepartment] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
 (select STUFF((SELECT ',' + 
CAST(IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].UserProfile where UserProfileId in 
	(select UserID from [dbo].LettersInboundDestinationUsers where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) )AS UsereName,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130) 
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
		 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'InboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
    (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Inbound Letter' as LetterType
	FROM dbo.[LettersInbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber
where a.DeleteFlag = 0 and (@P_UserEmail != w.ToEmail or @P_UserID = a.CreatedBy) AND (((a.CreatedBy = @P_UserID and w.Status<>24) or  (w.FromEmail=@P_UserEmail ) or
@P_UserEmail in (select ToEmail from Workflow where ReferenceNumber = a.LetterReferenceID)) and w.Status!=28))as m 


	insert into @result 
	Select  * from (SELECT row_number() over (Order By  [LetterID]) as slno,
	 a.[LetterID],
	 a.[LetterReferenceID],
	 a.[LetterTitle],
	 ( select (case when @P_Language ='EN' then DepartmentName else ArDepartmentName end) from M_Department  where DepartmentID = a.SourceOU)as SourceOU  ,
	(select STUFF((SELECT ',' + 
CAST((case when @P_Language = 'EN' then DepartmentName else ArDepartmentName end) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].[M_Department] where DepartmentID in 
	(select DepartmentID from [dbo].[LettersInboundDestinationDepartment] where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) ) AS Destination,
 (select STUFF((SELECT ',' + 
CAST(IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) AS NVARCHAR(max)) AS [text()]
    FROM  [dbo].UserProfile where UserProfileId in 
	(select UserID from [dbo].LettersInboundDestinationUsers where [LetterID] = a.[LetterID]and (DeleteFlag is null or DeleteFlag=0))
FOR XML PATH('')), 1, 1, NULL) )AS UsereName,
	(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName)  from [dbo].[UserProfile] where UserProfileId=a.SourceName) as SourceName,
	(select case 
	when (((select count(*) from [LettersInbound_RelatedOutgoing] where [LetterID] = a.[LetterID])>0)or(a.RelatedToIncomingLetter is not null))
	then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130) 
	else (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131)
end ) as LinkToOtherLetter,
(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [UserProfile] where UserProfileId=a.[CreatedBy])  as SenderName,
		 (select (case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from [dbo].[M_Lookups] where LookupsID = w.status AND Module = 'InboundLetter' AND Category = 'Status') as Status,
   a.CreatedDateTime,
    (case when(a.[NeedReply]=1) then (select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=130)  when(a.[NeedReply]=0) then (select(case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=131) else null end) as NeedReply,
   (select(case when @P_Language = 'EN' then DisplayName else ArDisplayName end) from M_Lookups where Category='Priority' and Module= a.Priority)as Priority,
   'Inbound Letter' as LetterType
	 FROM dbo.[LettersInbound] a inner join @Workflow w on a.[LetterReferenceID] = w.ReferenceNumber
   where a.DeleteFlag = 0 and (w.Status = 26 or w.Status = 29)and (
   (@P_UserID in (select [UserID] from [dbo].[LettersInboundDestinationUsers] where [LetterID]=a.LetterID and DeleteFlag =0 )) ) ) as m 


	 if(@P_SourceOU != '')
	 delete from @result where SourceOU not like '%'+@P_SourceOU+'%'
	  if(@P_Username != '')
	 delete from @result where UserName not like '%'+@P_Username+'%'

	 if(@P_SenderName != '')
	 delete from @result where SenderName not like '%'+@P_SenderName+'%'

	 if(@P_DestinationOU != '')
	 delete from @result where @P_DestinationOU not in (select value from string_split(Destination,','))

	 if(@P_Priority != '')
	 delete from @result where Priority not like '%'+@P_Priority+'%' or Priority is null

	 if(@P_DateFrom is not null)
	 delete from @result where cast( CreatedDateTime as date) < cast(@P_DateFrom as date)

	 if(@P_DateTo is not null)
	 delete from @result where cast(CreatedDateTime as date) > cast(@P_DateTo as date)

	 if(@P_Status !='' or @P_Status!=null )
	 begin
		delete from @result where Status !=  @P_Status
	 end

	 if(@P_SmartSearch is not null )
	 
	  With CTE_Duplicates as
   (select  row_number() over(partition by ReferenceNumber order by ReferenceNumber ) rownumber 
   from @result  )
   delete from CTE_Duplicates where rownumber!=1


select id,LetterID,ReferenceNumber ,Title,SourceOU,Destination,UserName,
SourceName,LinkToOtherLetter,SenderName,Status,CreatedDateTime,
Replied,Priority,LetterType from (SELECT row_number() over (Order By ReferenceNumber desc) as slno,
		 * from @result as R where 
		((ReferenceNumber  like '%'+@P_SmartSearch+'%') or (Title  like '%'+@P_SmartSearch+'%') or
		(SourceOU  like '%'+@P_SmartSearch+'%') or (Status  like '%'+@P_SmartSearch+'%') 
		or(Destination  like '%'+@P_SmartSearch+'%') or(SenderName  like '%'+@P_SmartSearch+'%') or (UserName  like '%'+@P_SmartSearch+'%') or
		((SELECT  CONVERT(nvarchar(10), cast(CreatedDateTime as date), 103))  like '%'+@P_SmartSearch+'%')  or
		(Replied  like '%'+@P_SmartSearch+'%') or (Priority  like '%'+@P_SmartSearch+'%') or
		(((select count(K.Keywords) from LettersInboundKeywords as K where K.LetterID=R.LetterID 
			and (K.DeleteFlag=0 or K.DeleteFlag is null)and K.Keywords like '%'+@P_SmartSearch+'%')>0)  and R.LetterType='Inbound Letter') or
		 (((select count(K.Keywords) from LettersOutboundKeywords as K where K.LetterID=R.LetterID 
			and (K.DeleteFlag=0 or K.DeleteFlag is null)and K.Keywords like '%'+@P_SmartSearch+'%')>0)  and R.LetterType='Outbound Letter')or
	     (((select count(K.EntityID) from LettersOutboundDestinationEntity as K where K.LetterID=R.LetterID and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.EntityID like '%'+@P_SmartSearch+'%')>0) ) or  
		 (((select count(K.UserID) from LettersInboundDestinationUsers as K where K.LetterID=R.LetterID and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and(select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from UserProfile where UserProfileId= K.UserID ) like '%'+@P_SmartSearch+'%')>0))or
		((select count(c.EntityName) from Contact as c join LettersOutboundDestinationEntity as a on c.ContactID=a.EntityID where a.LetterID=r.LetterID  and c.EntityName like '%'+@P_SmartSearch+'%')>0)
		or((select count(c.EntityName) from Contact as c join LettersOutboundDestinationEntity as a on c.ContactID=a.EntityID where a.LetterID=r.LetterID and c.EntityName like '%'+@P_SmartSearch+'%')>0)
		or((select count(c.UserName) from Contact as c join LettersOutboundDestinationEntity as a on c.ContactID=a.EntityID where a.LetterID=r.LetterID and r.LetterType='Inbound Letter'and c.UserName like '%'+@P_SmartSearch+'%')>0)
		or((select count(c.UserName) from Contact as c join LettersOutboundDestinationEntity as a on c.ContactID=a.EntityID where a.LetterID=r.LetterID and r.LetterType='Outbound Letter'and c.UserName like '%'+@P_SmartSearch+'%')>0)
		 or(( select Count(*) from (select (case when a.IsGovernmentEntity=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=184))
			when a.IsGovernmentEntity=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=185))
			end)as e from LettersOutboundDestinationEntity as a join LettersOutbound as m on a.LetterID=m.LetterID 
				where m.LetterReferenceID=r.ReferenceNumber   ) as za where za.e like  '%'+@P_SmartSearch+'%')>0)
		or( select count(*) from(select (case when a.IsGovernmentEntity=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=184))
			when a.IsGovernmentEntity=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=185))
			end)as Q from LettersOutboundDestinationEntity as a join LettersInbound as m on a.LetterID=m.LetterID 
				where m.LetterReferenceID=r.ReferenceNumber   )as DR where Dr.Q like  '%'+@P_SmartSearch+'%')>0)) as m 
	
	 if( @P_SmartSearch is null)
	select* from @result 
END

GO

GO
ALTER PROCEDURE [dbo].[Get_M_User]-- 'TestUser15'
    
    @P_search nvarchar(max) = null,
    @P_DepartmentID int =0,
    @P_Language nvarchar(10)= 'EN'
    -- Add the parameters for the stored procedure here
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

 

    select UserProfileId as UserProfileID,IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) as EmployeeName from UserProfile where (DeleteFlag = 0 or DeleteFlag is null) and
     IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) like case when @P_search is null then IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) else @P_search+'%' end and
     UserProfileId not in ( select ApproverID from M_ApproverConfiguration as A where A.DepartmentID = @P_DepartmentID and DeleteFlag =0)

 

END
GO

ALTER PROCEDURE [dbo].[Get_MemoHistoryByID]-- 49
	@P_MemoID int = null,
	@P_Language nvarchar(10)= null
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT [HistoryID],
		   [MemoID],
		   [Action], 
		   case when [DelegateUser] is null then (select IIF(@P_Language = 'AR',AREmployeeName,EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=ActionBy)
	       else concat((select IIF(@P_Language = 'AR', AREmployeeName, EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]),
		   case when @P_Language = 'AR' then (select ' '+ArDisplayName+' ' from [RulersCourt].[dbo].[M_Lookups] where DisplayName='On behalf of') else ' on behalf of ' end,
	       (select IIF(@P_Language = 'AR', AREmployeeName, EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=ActionBy)) end as ActionBy,
		   [ActionDateTime],
		   [Comments],
		   (select IIF(@P_Language = 'AR', AREmployeeName, EmployeeName) from [dbo].[UserProfile] where [UserProfileId]=Escalate_RedirectUser) as Escalate_RedirectUser
		   from [dbo].[MemoHistory] where MemoID = @P_MemoID
END

GO

GO
ALTER PROCEDURE [dbo].[Get_Notification] --   [Get_Notification] 4409,1

	@P_ID int = null,
	@P_UserID int = null,
	@P_ToReadAll bit = 0,
	@P_Language nvarchar(10)= null
AS
BEGIN	
	SET NOCOUNT ON;


	Declare @userEmail nvarchar(max) =  null

	select @userEmail= OfficialMailId from UserProfile where UserProfileId = @P_UserID

	if(@P_ToReadAll = 1)
	begin
		Update Notification set IsRead = 1 where ToEmail = @userEmail
	end
	else
	begin
		Update Notification set IsRead = 1 where ID = @P_ID
	end

		Select *,(select UF.EmployeeName from UserProfile as UF where UF.OfficialMailId = N.FromEmail) as EnFromName,
		(select UF.AREmployeeName from UserProfile as UF where UF.OfficialMailId = N.FromEmail) as ArFromName,
		(select UT.EmployeeName from UserProfile as UT where UT.OfficialMailId = N.ToEmail) as ToName,
		 (select UF.EmployeeName from UserProfile as UF where UF.OfficialMailId = N.DelegateFromEmail) as EnDelegateFromName,
		 (select UF.AREmployeeName from UserProfile as UF where UF.OfficialMailId = N.DelegateFromEmail) as ArDelegateFromName,
		(select IIF(@P_Language = 'EN', UT.EmployeeName, UT.AREmployeeName) from UserProfile as UT where UT.OfficialMailId = N.DelegateToEmail) as DelegateToName from Notification as N where ID = @P_ID

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
	 SET @UserName = (Select case when @P_Language='EN' then [EmployeeName] else [AREmployeeName] end from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	 SET @P_UserEmail = (Select OfficialMailId from [dbo].[UserProfile] where UserProfileId = @P_UserID)
	  SET @P_OrgUnit = (Select OrgUnitID from [dbo].[UserProfile] where UserProfileId = @P_UserID)

	  if(@P_Status='All' or @P_Status=N'الكل')
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

	 if(@P_Status !='' and @P_Status is not null and @P_Status not like N'الكل')
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

ALTER PROCEDURE [dbo].[Save_Memo]--[Save_Memo] 
	@P_MemoID int = null,
	@P_Title nvarchar(Max) = null,
	@P_SourceOU nvarchar(250) = null,
	@P_SourceName nvarchar(250) = null,
	@P_Details nvarchar(MAX) = null,
	@P_Private nvarchar (250) = null,
	@P_Priority nvarchar (250) = null,
	@P_Action nvarchar(100)= null,
	@P_Comment nvarchar(Max) = null,
	@P_DeleteFlag bit = 0,
	@P_CreatedBy int = null,
	@P_UpdatedBy  int = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_ApproverDepartmentId int = 0,
	@P_DelegateUser int=null,
	@P_DestinationRedirected_EscalatedUserID int=null
	
	

AS
BEGIN
	
	SET NOCOUNT ON;
	
	 declare @P_Referencenumber nvarchar(255) = null, @currentApprover int = null, @DelegateUser int
	 declare @UpdatedEmail nvarchar(250),@WorkflowRef nvarchar(250)
	 set @UpdatedEmail=(select officialMailID from UserProfile where UserProfileId=@P_UpdatedBy)
	declare @temp int = null
	set @temp = (SELECT IDENT_CURRENT('Memo'))
	--select  @P_Referencenumber =concat((Right(concat('0','0',@temp),3)),'-',(SELECT YEAR((select GETDATE()))),'-','M');		

	if(@P_MemoID is null or @P_MemoID =0)
	begin
		insert into [Memo] ([ReferenceNumber],[Title],[SourceOU],[SourceName],[Details],[Private],[Priority],[DeleteFlag], [CreatedBy] ,[CreatedDateTime])
		select @P_Referencenumber,@P_Title,@P_SourceOU,@P_SourceName,@P_Details,@P_Private,@P_Priority,@P_DeleteFlag,@P_CreatedBy,@P_CreatedDateTime
		set @temp = (select SCOPE_IDENTITY());
		set @P_MemoID = (select SCOPE_IDENTITY());
		select  @P_Referencenumber =concat((Right(concat('0','0',@temp),3)),'-',(SELECT YEAR((select GETDATE()))),'-','M');

		update [dbo].[Memo]  set  [ReferenceNumber]=@P_Referencenumber where MemoID=@temp

		insert into [dbo].[MemoHistory]([MemoID],[Action],[ActionBy],[ActionDateTime],[Comments],[Escalate_RedirectUser])
		select @temp,@P_Action,@P_CreatedBy,@P_CreatedDateTime,@P_Comment,@P_DestinationRedirected_EscalatedUserID

		select top 1 MemoId,ReferenceNumber as ReferenceNumber ,CreatedBy as CreatorID, (case when (UpdatedBy is null) then CreatedBy else UpdatedBy end) as FromID from [Memo] order by MemoID desc
	end

	Else if(@P_MemoID is not NUll)
	begin
		update [Memo] set  Title=@P_Title,SourceOU=@P_SourceOU,SourceName=@P_SourceName,Details=@P_Details,Private=@P_Private,Priority=@P_Priority,DeleteFlag=@P_DeleteFlag,UpdatedBy=@P_UpdatedBy,UpdatedDateTime=@P_UpdatedDateTime
		where MemoID=@P_MemoID

		set @WorkflowRef = (select ReferenceNumber from memo where MemoId=@P_MemoID)

		Update [Workflow]  set IsRedirect=1 where ToEmail=@UpdatedEmail AND
 ReferenceNumber =@WorkflowRef and (WorkflowProcess='RedirectWorkflow'or WorkflowProcess='ApprovalWorkflow') 
 and Status=3 and @P_Action='Redirect'

	declare @Workflow table(
		WorkflowID int,
		ReferenceNumber nvarchar(max),
		ToEmail nvarchar(max),
		Status int,
		delegateuser nvarchar(max)
		);

		insert into @Workflow
		select WorkflowID,ReferenceNumber,ToEmail,Status,DelegateToEmail from [dbo].[Workflow]where (Status = 1 or Status = 2) order by WorkflowID desc

		;WITH CTE AS 
		(
		SELECT ReferenceNumber, ROW_NUMBER() OVER 
		(
		    PARTITION BY ReferenceNumber ORDER BY WorkflowID desc
		) RowNumber
		FROM  @Workflow
		)
		DELETE FROM CTE WHERE RowNumber > 1
	
		set @currentApprover = ( select UserProfileId from [dbo].[UserProfile] where OfficialMailId = (
	  (select top 1 W.ToEmail from @Workflow W where W.ReferenceNumber =(select ReferenceNumber from Memo 
	  where MemoID = @P_MemoID) and (W.Status = 2 or W.Status = 1))))
	 

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

		declare @P_DeletgateUserID int =0;
		select @P_DeletgateUserID = [DOANameID] from @Result where Creator = @currentApprover and (select GETDATE()) >= [StartDate] 
		and (select GETDATE()) <= EndDate 

			if(@P_UpdatedBy = @P_DeletgateUserID and (select top 1 Status from @Workflow where ReferenceNumber=(select m.ReferenceNumber from Memo as m where m.MemoID=@P_MemoID)) =2
			and @P_Action != 'Close')
			begin
			set @P_UpdatedBy = @currentApprover
			set @P_DelegateUser= @P_DeletgateUserID
			end

		insert into [dbo].[MemoHistory]([MemoID],[Action],[ActionBy],[ActionDateTime],[Comments],[DelegateUser],[Escalate_RedirectUser])
		select @P_MemoID,@P_Action,@P_UpdatedBy,@P_UpdatedDateTime,@P_Comment,@P_DelegateUser,@P_DestinationRedirected_EscalatedUserID

		select MemoId,ReferenceNumber as ReferenceNumber ,CreatedBy as CreatorID, (case when (UpdatedBy is null) then CreatedBy else UpdatedBy end) as FromID from [Memo] where MemoID=@P_MemoID
	end

	if(@P_Action = 'Save' or @P_Action='Submit' or @P_Action ='Resubmit')
	update [Memo] set InitalApproverDepartment = @P_ApproverDepartmentId where MemoID= @P_MemoID
	
END


GO

ALTER PROCEDURE [dbo].[Save_OfficialTask]

	@P_OfficialTaskID int = null,
	@P_Date datetime = null,
    @P_SourceOU nvarchar(250) = null,
	@P_SourceName nvarchar(250) = null,
	@P_OfficialTaskType nvarchar(250) = null,
	@P_StartDate datetime = null,
	@P_EndDate datetime = null,
	@P_NumberofDays int = null,
	@P_OfficialTaskDescription nvarchar(Max) = null,
	@P_Action nvarchar(250) = null,
	@P_DeleteFlag bit = 0,
	@P_CreatedBy nvarchar(250) = null,
	@P_UpdatedBy  nvarchar(250) = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_Comments nvarchar(Max) = null
	
	

AS
BEGIN
	
	SET NOCOUNT ON;
	 declare @Referencenumber nvarchar(255) = null
	declare @temp int = null
	if((select count(*) from OfficialTask) = 0)
       set @temp = (SELECT IDENT_CURRENT('OfficialTask'))
	
	else
	set @temp = (SELECT IDENT_CURRENT('OfficialTask'))+1
	select  @Referencenumber =concat((Right(concat('0','0',@temp),3)),'-',(SELECT YEAR((select GETDATE()))),'-','OT');
        if(@P_OfficialTaskID is null or @P_OfficialTaskID =0)
	begin
	insert into [dbo].[OfficialTask]
	select @ReferenceNumber,@P_Date,@P_SourceOU,@P_SourceName,@P_OfficialTaskType,@P_StartDate,@P_EndDate,@P_NumberofDays,@P_OfficialTaskDescription,@P_Comments,@P_DeleteFlag,@P_CreatedBy,@P_UpdatedBy,@P_CreatedDateTime,@P_UpdatedDateTime,0
	set @P_OfficialTaskID = (SELECT IDENT_CURRENT('OfficialTask'))
     
	insert into [dbo].[OfficialTaskCommunicationHistory]
	select @P_OfficialTaskID,@P_Comments,0,@P_Action,@P_CreatedBy,@P_CreatedDateTime
  	
	end
	Else
	begin
	update [dbo].[OfficialTask] set Date = @P_Date,SourceOU = @P_SourceOU ,SourceName = @P_SourceName ,OfficialTaskType = @P_OfficialTaskType,StartDate = @P_StartDate,EndDate= @P_EndDate,NumberofDays = @P_NumberofDays,OfficialTaskDescription = @P_OfficialTaskDescription,Comments =@P_Comments,DeleteFlag=@P_DeleteFlag,UpdatedBy=@P_UpdatedBy,UpdatedDateTime=@P_UpdatedDateTime
	where OfficialTaskID=@P_OfficialTaskID

	if(@P_Action = 'MarkasComplete')
	begin
		if ((select Cast (DATEADD(DAY, 5, @P_EndDate) as Date)) < (select cast (GETDATE() as Date)))
		set @P_Action = 'Close'		
	end

   	insert into [dbo].[OfficialTaskCommunicationHistory]
	select @P_OfficialTaskID,@P_Comments,0,@P_Action,@P_UpdatedBy,@P_UpdatedDateTime

	end

	SELECT OfficialTaskID,ReferenceNumber,CreatedBy as CreatorID, (case when (UpdatedBy is null) then CreatedBy else UpdatedBy end) as   FromID from [dbo].[OfficialTask]
	where OfficialTaskID=@P_OfficialTaskID
END
GO

ALTER PROCEDURE [dbo].[Save_ShareparticipationUsers] --103,1,'Memo',null,0
	@P_ServiceId int = null,	
	@P_UserID int = null,
	@P_Type nvarchar(100)= null,
	@P_CreatorID int = null,
	@P_Comments nvarchar(max) = null
AS
BEGIN
	SET NOCOUNT ON;
	
	insert into [dbo].[ShareparticipationUsers]
	select @P_ServiceId,@P_UserID,@P_Type

	insert into MemoHistory
	select @P_ServiceId,'Share',@P_CreatorID,(select GETUTCDATE()),@P_Comments,null,null

	SELECT IDENT_CURRENT('ShareparticipationUsers')	
END
GO


