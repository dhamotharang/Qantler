ALTER PROCEDURE [dbo].[Get_LetterOutboundPreview] --  [Get_LetterOutboundPreview] 3
    @P_LetterID int = 0,
    @P_UserID int = 0,
    @P_Language nvarchar(10)
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
    select @Title = U.Title, @ApproverID=UserProfileId,@ApproverName= case when @P_Language='EN' then EmployeeName else AREmployeeName end,@ApproverDesignation=case when @P_Language='EN' then EmployeePosition else AREmployeePosition end,@ApproverDepartment=
    (select ArDepartmentName from M_Department as M where M.DepartmentID= U.DepartmentID) from UserProfile as U  where OfficialMailId = @ApproverEmail

 

    if((select count(*) from Workflow where ReferenceNumber = @ReferenceNumber and Status in (20,23) )>0)
    begin
    select @SignatureName = U.SignaturePhoto, @signatureID=U.SignaturePhotoID from UserProfile as U  where OfficialMailId = @ApproverEmail
    end
    

 

    SELECT [LetterID],[LetterReferenceID],[LetterTitle],[SourceOU],(select case when @P_Language='EN' then EmployeeName else AREmployeeName end from [UserProfile] where UserProfileId=SourceName)as SourceName,[LetterDetails],[Priority],[LetterDetails],[DocumentClassification],
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
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),IIF(@P_Language = 'EN', ' With Apologies Letter', N' راذتعا ةلاسر عم ضفر'))
    when (Action = 'Reject' and ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID ) is null) or ((select [IsApologiesSent] from Calendar where CalendarID = a.CalendarID )= 0) )
        then concat((case when [DelegateUser] is null then (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]= a.CreatedBy)
             else concat((select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where [UserProfileId]=[DelegateUser]), ' on behalf of ',
            (select IIF(@P_Language = 'EN', EmployeeName, AREmployeeName) from [dbo].[UserProfile] where UserProfileId = a.CreatedBy  )) end),' Without Apologies Letter') end   
    as
     CreatedBy,
        [CreatedDateTime],Comment from [dbo].CalendarCommunicationHistory as a where a.CalendarID = @P_CalendarID
END
