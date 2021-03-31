ALTER PROCEDURE [dbo].[Save_VehicleRequest]

	@P_VehicleReqID int = null,
    @P_RequestType int = null,
	@P_Requestor int = null,
	@P_RequestDateTime datetime = null,
	@P_DriverID int = null,	
	@P_TobeDrivenbyDepartmentID int = null,
	@P_TobeDrivenbyDriverID int=null,
	@P_TripTypeID int=null,
	@P_TripTypeOthers nvarchar(max) = null,
	@P_Emirates int = null,
	@P_City int = null,
	@P_Destination int = null,
	@P_DestinationOthers nvarchar(max) = null,
	@P_TripPeriodFrom datetime = null,
	@P_TripPeriodTo datetime = null,
	@P_VehicleModelID int = null,
	@P_ApproverDepartment int = null,
	@P_ApproverName int = null,
	@P_ReleaseDateTime datetime = null,
	@P_LastMileageReading int = null,
	@P_ReleaseLocationID int = null,
	@P_ReturnDateTime datetime = null,
	@P_CurrentMileageReading int = null,
	@P_HavePersonalBelongings bit = null,
	@P_PersonalBelongingsText nvarchar(max)= null,	
	@P_VehicleID int = 0,
	@P_DeleteFlag bit = 0,
	@P_CreatedBy nvarchar(250) = null,
	@P_UpdatedBy  nvarchar(250) = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_Notes nvarchar(max) = 0,
	@P_ReturnLocationID int = null,
	@P_Action nvarchar(max) = null,
	@P_Reason nvarchar(max) = null
	

AS
BEGIN
	
	SET NOCOUNT ON;
	 declare @Referencenumber nvarchar(255) = null
	declare @temp int = null
	if((select count(*) from Vehicles) = 0)
       set @temp = (SELECT IDENT_CURRENT('VehicleRequest'))
	
	else
	set @temp = (SELECT IDENT_CURRENT('VehicleRequest'))+1
	--select  @Referencenumber =concat((Right(concat('0','0',@temp),LEN(@temp))),'-',(SELECT YEAR((select GETDATE()))),'-','VR');
	select  @Referencenumber =concat(FORMAT(@temp,'000'),'-',(SELECT YEAR((select GETDATE()))),'-','VR');
        if(@P_VehicleReqID is null or @P_VehicleReqID =0)
	begin
	insert into [dbo].[VehicleRequest]
	select @ReferenceNumber,@P_RequestType,@P_Requestor,@P_RequestDateTime,@P_DriverID,@P_TobeDrivenbyDepartmentID,
	@P_TobeDrivenbyDriverID,@P_TripTypeID,@P_TripTypeOthers,@P_Emirates,@P_City,
	@P_Destination,@P_DestinationOthers,@P_TripPeriodFrom,@P_TripPeriodTo,@P_VehicleModelID,
	@P_ApproverDepartment,@P_ApproverName,@P_ReleaseDateTime,@P_LastMileageReading,@P_ReleaseLocationID,
	@P_ReturnDateTime,@P_CurrentMileageReading,@P_HavePersonalBelongings,@P_PersonalBelongingsText,	
	@P_DeleteFlag,@P_CreatedBy,@P_UpdatedBy,@P_CreatedDateTime,@P_UpdatedDateTime,@P_VehicleID,@P_Notes,@P_ReturnLocationID
	,@P_Reason

	set @P_VehicleReqID = (SELECT IDENT_CURRENT('VehicleRequest'))   

  	--select * from VehicleDetails
	end
	Else
	begin
	update [dbo].[VehicleRequest] set RequestType = @P_RequestType,Requestor = @P_Requestor ,RequestDateTime=@P_RequestDateTime,DriverID=@P_DriverID,TobeDrivenbyDepartmentID=@P_TobeDrivenbyDepartmentID,TobeDrivenbyDriverID=@P_TobeDrivenbyDriverID,TripTypeID=@P_TripTypeID,
	TripTypeOthers=@P_TripTypeOthers,Emirates=@P_Emirates,City=@P_City,Destination=@P_Destination,DestinationOthers=@P_DestinationOthers,
	TripPeriodFrom=@P_TripPeriodFrom,TripPeriodTo=@P_TripPeriodTo,VehicleModelID =@P_VehicleModelID,
	ApproverDepartment = @P_ApproverDepartment,ApproverName=@P_ApproverName,
	ReleaseDateTime = @P_ReleaseDateTime,LastMileageReading=@P_LastMileageReading,ReleaseLocationID=@P_ReleaseLocationID,
	ReturnDateTime = @P_ReturnDateTime,CurrentMileageReading=@P_CurrentMileageReading,
	HavePersonalBelongings = @P_HavePersonalBelongings,PersonalBelongingsText = @P_PersonalBelongingsText,
	VehicleID=@P_VehicleID,ReturnLocationID=@P_ReturnLocationID,
	Notes=@P_Notes,DeleteFlag=@P_DeleteFlag,UpdatedBy=@P_UpdatedBy,UpdatedDateTime=@P_UpdatedDateTime,Reason=@P_Reason
	where VehicleReqID=@P_VehicleReqID 
	end

	If(@P_Action = 'Return')
	update Vehicles set CurrentMileage = @P_CurrentMileageReading where VehicleID = @P_VehicleID

	if(@P_Action ='Release')
	Update Vehicles set RequestorID = @P_Requestor,CurrentMileage = @P_LastMileageReading where VehicleID = @P_VehicleID

	insert into VehicleHistory([VehiclReqID], [Action], [ActionBy], [ActionDateTime])
	SELECT VehicleReqID,@P_Action,CreatedBy,GETUTCDATE() from [dbo].[VehicleRequest]
	where VehicleReqID=@P_VehicleReqID 

	SELECT VehicleReqID,ReferenceNumber,CreatedBy as CreatorID, (case when (UpdatedBy is null) then CreatedBy else UpdatedBy end) as   FromID from [dbo].[VehicleRequest]
	where VehicleReqID=@P_VehicleReqID
END