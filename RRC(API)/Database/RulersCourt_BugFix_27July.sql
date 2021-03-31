ALTER PROCEDURE [dbo].[Get_VehiclePreview] --  [Get_VehiclePreview] 321
	-- Add the parameters for the stored procedure here select * from  VehicleRequest
	@P_VehicleRequestID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
		id int identity(1,1),
		PlateNumber nvarchar(max),
		PlateCode nvarchar(max),
		VehicleMake nvarchar(max),
		VehicleModel nvarchar(max),
		DriverName nvarchar(max),
		ReleaseDate nvarchar(max),
		ReleaseTime nvarchar(max),
		ReleaseMeridiem nvarchar(max),
		ReleaseLocation nvarchar(max),
		LastMileageOnRelease nvarchar(max),
		ReturnDate nvarchar(max),
		ReturnTime nvarchar(max),
		ReturnMeridiem nvarchar(max),
		ReturnLocation nvarchar(max),
		LastMileageOnReturn nvarchar(max),
		YearOfManufacture nvarchar(max),
		VehicleID int,
		ReleasedBy nvarchar(max),
		ReturnedBy nvarchar(max),
		Note nvarchar(max),
		PersonalBelonging nvarchar(max),
		ReferenceNumber nvarchar(max))


	insert into @result
    select V.PlateNumber,V.PlateColor,V.VehicleMake,V.VehicleModel,(select UP.EmployeeName from UserProfile as UP where UP.UserProfileId= (case when VR.DriverID is null then VR.TobeDrivenbyDriverID else VR.DriverID end)) 
	,(select (SELECT  CONVERT(nvarchar(120), cast((SELECT CONVERT(datetime,  SWITCHOFFSET(CONVERT(datetimeoffset,  VR.ReleaseDateTime),   DATENAME(TzOffset, SYSDATETIMEOFFSET()))))  as Date), 103)))
	,(select (SELECT  CONVERT(nvarchar(120), cast((SELECT CONVERT(datetime,  SWITCHOFFSET(CONVERT(datetimeoffset,  VR.ReleaseDateTime),   DATENAME(TzOffset, SYSDATETIMEOFFSET()))))  as Time), 100)))
	,null,(select ArDisplayName from M_Lookups as M where M.Module='VehicleLocation' and M.Category=VR.ReleaseLocationID)
	,VR.LastMileageReading
	,(select (SELECT  CONVERT(nvarchar(120), cast((SELECT CONVERT(datetime,  SWITCHOFFSET(CONVERT(datetimeoffset,  VR.ReturnDateTime),   DATENAME(TzOffset, SYSDATETIMEOFFSET()))))  as Date), 103)))
	,(select (SELECT  CONVERT(nvarchar(120), cast((SELECT CONVERT(datetime,  SWITCHOFFSET(CONVERT(datetimeoffset,  VR.ReturnDateTime),   DATENAME(TzOffset, SYSDATETIMEOFFSET()))))  as Time), 100)))
	,null,(select ArDisplayName from M_Lookups as M where M.Module='VehicleLocation' and M.Category=VR.ReturnLocationID)
	,VR.CurrentMileageReading
	,VR.VehicleID
	,YearofManufacture,(select Up.EmployeeName from UserProfile as UP where Up.UserProfileId = (select  VH.ActionBy from VehicleHistory as VH where VH.Action = 'Release' and VH.VehiclReqID=@P_VehicleRequestID))
	,(select Up.EmployeeName from UserProfile as UP where Up.UserProfileId = (select VH.ActionBy from VehicleHistory as VH where VH.Action = 'Return' and VH.VehiclReqID=@P_VehicleRequestID))
	,VR.Notes,VR.PersonalBelongingsText,VR.ReferenceNumber from VehicleRequest as VR left join Vehicles as V on Vr.VehicleID= V.VehicleID where VR.VehicleReqID=@P_VehicleRequestID
	
	Update @Result set ReturnMeridiem = (select ArDisplayName from M_Lookups where DisplayName='AM') where ReleaseTime like '%AM%'
	Update @Result set ReturnTime = REPLACE(ReturnTime,'AM','') where ReleaseTime like '%AM%'
	
	Update @Result set ReturnMeridiem = (select ArDisplayName from M_Lookups where DisplayName='PM') where ReleaseTime like '%PM%'
	Update @Result set ReturnTime = REPLACE(ReturnTime,'PM','') where ReleaseTime like '%PM%'

	
	Update @Result set ReleaseMeridiem = (select ArDisplayName from M_Lookups where DisplayName='AM') where ReleaseTime like '%AM%'
	Update @Result set ReleaseTime = REPLACE(ReleaseTime,'AM','') where ReleaseTime like '%AM%'
	
	Update @Result set ReleaseMeridiem = (select ArDisplayName from M_Lookups where DisplayName='PM') where ReleaseTime like '%PM%'
	Update @Result set ReleaseTime = REPLACE(ReleaseTime,'PM','') where ReleaseTime like '%PM%'


	select * from @result

END

