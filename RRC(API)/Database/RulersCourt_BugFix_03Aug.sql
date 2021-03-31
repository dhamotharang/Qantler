
ALTER PROCEDURE [dbo].[Get_UserProfileList] -- [Get_UserProfileList] 1,100,0
-- Add the parameters for the stored procedure here
	@P_PageNumber int =0,
	@P_PageSize int =0,
	@P_Method int = 0,
	@P_UserName nvarchar(max) = null,
	@P_OrgDepartmentID int= 0,
	@P_SmartSearch nvarchar(max) = '',
	@P_JobTitle nvarchar(max) = null,
	@P_Type int =0,
	@P_Language nvarchar(10)= 'EN'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
	id int,
	ReferenceNumber nvarchar(255),
	UserProfileId int,
	DepartmentName nvarchar(255),
	EmployeeName nvarchar(max),
	JobTitle nvarchar(max),
	PassportExpiryDate datetime,
	InsurenceExpiryDate datetime,
	EmiratesExpiryDate datetime,
	LabourcontactExpiryDate datetime,
	VisaExpiryDate datetime,
	CreatedDateTime datetime
	);

    -- Insert statements for procedure here
	declare  @RefCout int =0, @StartNo int =0, @EndNo int =0,@Count int =0;

	select @RefCout=(@P_PageNumber - 1) * @P_PageSize;
	select @StartNo =(@RefCout + 1);
	select @EndNo=@RefCout + @P_PageSize;

	insert into @result  
	SELECT row_number() over (Order By  UP.[UserProfileId]) as slno,up.ReferenceNumber,
	UP.[UserProfileId],
	(select ( case when @P_Language='AR' then ArDepartmentName else DepartmentName end) 
		from [dbo].M_Department as OU where DepartmentID = UP.OrgUnitID ) as DepartmentName,
	UP.[EmployeeName],
	 EmployeePosition as JobTitle	
	, PassportExpiryDate,InsuranceExpiryDate,EmiratesIdExpiryDate,LaborContractExpiryDate,VisaExpiryDate,CreatedDatetime
	from [dbo].[UserProfile] as UP 
	--select * from @result --where id between 1 and 3 

	if(@P_UserName != '')
	 delete from @result where EmployeeName not like '%'+@P_UserName+'%'  or EmployeeName is null

	 if(@P_OrgDepartmentID != '') 
	 delete from @result where DepartmentName !=  (select ( case when @P_Language='AR' then ArDepartmentName else DepartmentName end)
		 from M_Department where DepartmentID = @P_OrgDepartmentID)or DepartmentName is null

	if(@P_JobTitle != '') 
	begin

	 delete from @result where JobTitle not like '%'+@P_JobTitle+'%' or JobTitle is null

	 end

	
	if(@P_Type = 1)
	 begin
		delete from @result where PassportExpiryDate > (select GETDATE())
	 end

	 if(@P_Type = 2)
	 begin
		delete from @result where InsurenceExpiryDate > (select GETDATE())
	 end

	 if(@P_Type = 3)
	 begin
		delete from @result where EmiratesExpiryDate > (select GETDATE())
	 end

	 if(@P_Type = 4)
	 begin
		delete from @result where LabourcontactExpiryDate > (select GETDATE())
	 end

	  if(@P_Type = 5)
	 begin
		delete from @result where VisaExpiryDate > (select GETDATE())
	 end

	if(@P_SmartSearch is not null and @P_Method = 0 )
	  begin
	 select * from (SELECT row_number() over (Order By  CreatedDateTime desc) as slno,
		 * from @result as m  where ((ReferenceNumber  like '%'+@P_SmartSearch+'%')or(EmployeeName  like '%'+@P_SmartSearch+'%') or (JobTitle like '%'+@P_SmartSearch+'%') or
		  (DepartmentName  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(PassportExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(InsurenceExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(VisaExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(LabourcontactExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(EmiratesExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.PassportIssueDate as date), 103) from UserProfile as a where a.UserProfileId=m.UserProfileId )  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast(a.VisaIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId )  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.EmiratesIdIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast(a.LabourContractIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.InsuranceIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  (a.PassportNumber) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT  a.PassportIssuePlace  from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')	     or	((SELECT  a.LabourContractNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT  a.InsuranceNumber  from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')    or	((SELECT  a.VisaNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT a.EmiratesIdNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') or			
		(((select count(CONVERT(nvarchar(10), cast(K.StartDate as date), 103)) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and CONVERT(nvarchar(10), cast(k.StartDate as date), 103) like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.TrainingName) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.TrainingName like '%'+@P_SmartSearch+'%')>0))
		or  (((select count(K.StartDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (  CONVERT(nvarchar(10), cast(K.StartDate as date), 103) )  like '%'+@P_SmartSearch+'%')>0)) 
		or
			  (((select count(K.EndDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (CONVERT(nvarchar(10), cast(K.EndDate as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		     or (((select count(K.EndDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (CONVERT(nvarchar(10), cast(K.EndDate as date), 103) )  like '%'+@P_SmartSearch+'%')>0))
		 		     or (((select count(K.TimePeriodFrom) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and ( CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), K.TimePeriodFrom)) as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		   		     or (((select count(K.TimePeriodTo) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), K.TimePeriodFrom)) as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.City) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.City like '%'+@P_SmartSearch+'%')>0))
			  or  (((select count(K.Company) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.Company like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.JobTitle) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.JobTitle like '%'+@P_SmartSearch+'%')>0))
			  or  (((select count(K.SchoolCollege) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.SchoolCollege like '%'+@P_SmartSearch+'%')>0))
			 or  (((select count(K.FieldStudy) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
		and k.FieldStudy like '%'+@P_SmartSearch+'%')>0))
			 or  (((select count(K.Degree) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and  (select case when @P_Language = 'AR' then ArEducationName else EducationName end from M_Education where EducationID = k.Degree)  = @P_SmartSearch)>0))
 		or( (select a.OfficialMailId from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')
		or( (select a.PersonalMailId from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	
		or( (select a.EmployeeCode from UserProfile as a  where a.UserProfileId=m.UserProfileId) = @P_SmartSearch)	
		or( (select a.age from UserProfile as a  where a.UserProfileId=m.UserProfileId)like '%' +@P_SmartSearch+ '%')	
		or( (select a.MobileNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	or( (select a.EmployeePhoneNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	or( (select a.EmployeePosition from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')
		or( (select a.BalanceLeave from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	  or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.ResignationDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
		 or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.JoinDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.BirthDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
		   	or ((select count(K.Religion) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then ReligionName else ArReligionName end)from M_Religion as c where c.ReligionID=k.Religion) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.CountryofResidence) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then CountryName else ArCountryName end)from M_Country as c where c.CountryID=k.CountryofResidence) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.Nationality) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then NationalityName else ArNationalityName end)from M_Nationality as c where c.NationalityID=k.Nationality) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.PreviousNationality) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then NationalityName else ArNationalityName end)from M_Nationality as c where c.NationalityID=k.PreviousNationality) like '%'+@P_SmartSearch+'%')>0)
		or( (select (case when a.NotificationPreferencesEmail=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=163))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId  ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.NotificationPreferencesSMS=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=164))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Title =0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=165))
			when a.Title=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=166))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Gender=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=167))
			when a.Gender=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=168))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Resigned=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=169))
			when a.Resigned=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=170))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.EmploymentStatus=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=182))
			when a.EmploymentStatus=2 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=183))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.CreatedDatetime)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
			or  (((select count(CONVERT(nvarchar(10), cast(K.TimePeriodFrom as date), 103)) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and CONVERT(nvarchar(10), cast(k.TimePeriodFrom as date), 103) like '%'+@P_SmartSearch+'%')>0))
		  
		 ) ) as m where  slno between @StartNo and @EndNo 
	 end


		 if(@P_SmartSearch is not null and @P_Method = 1 )
	 begin
		select count(*) from @result as m where ((ReferenceNumber  like '%'+@P_SmartSearch+'%')or(EmployeeName  like '%'+@P_SmartSearch+'%') or (JobTitle like '%'+@P_SmartSearch+'%') or
		  (DepartmentName  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(PassportExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(InsurenceExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(VisaExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(LabourcontactExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')or	((SELECT  CONVERT(nvarchar(10), cast(EmiratesExpiryDate as date), 103))  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.PassportIssueDate as date), 103) from UserProfile as a where a.UserProfileId=m.UserProfileId )  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast(a.VisaIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId )  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.EmiratesIdIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast(a.LabourContractIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast(a.InsuranceIssueDate as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  (a.PassportNumber) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT  a.PassportIssuePlace  from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')	     or	((SELECT  a.LabourContractNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT  a.InsuranceNumber  from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')    or	((SELECT  a.VisaNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')
		 or	((SELECT a.EmiratesIdNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') or			
		(((select count(CONVERT(nvarchar(10), cast(K.StartDate as date), 103)) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and CONVERT(nvarchar(10), cast(k.StartDate as date), 103) like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.TrainingName) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.TrainingName like '%'+@P_SmartSearch+'%')>0))
		or  (((select count(K.StartDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (  CONVERT(nvarchar(10), cast(K.StartDate as date), 103) )  like '%'+@P_SmartSearch+'%')>0)) 
		or
			  (((select count(K.EndDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (CONVERT(nvarchar(10), cast(K.EndDate as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		     or (((select count(K.EndDate) from UserProfileTrainingCertifications as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (CONVERT(nvarchar(10), cast(K.EndDate as date), 103) )  like '%'+@P_SmartSearch+'%')>0))
		 		     or (((select count(K.TimePeriodFrom) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and ( CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), K.TimePeriodFrom)) as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		   		     or (((select count(K.TimePeriodTo) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), K.TimePeriodFrom)) as date), 103))  like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.City) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.City like '%'+@P_SmartSearch+'%')>0))
			  or  (((select count(K.Company) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.Company like '%'+@P_SmartSearch+'%')>0))
		  or  (((select count(K.JobTitle) from UserProfileWorkExperience as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.JobTitle like '%'+@P_SmartSearch+'%')>0))
			  or  (((select count(K.SchoolCollege) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and k.SchoolCollege like '%'+@P_SmartSearch+'%')>0))
			 or  (((select count(K.FieldStudy) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
		and k.FieldStudy like '%'+@P_SmartSearch+'%')>0))
			 or  (((select count(K.Degree) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and (select case when @P_Language = 'AR' then ArEducationName else EducationName end from M_Education where EducationID = k.Degree) = @P_SmartSearch)>0))
 		or( (select a.OfficialMailId from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')or( (select a.PersonalMailId from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	
		or( (select a.EmployeeCode from UserProfile as a  where a.UserProfileId=m.UserProfileId) = @P_SmartSearch)	or( (select a.age from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	
		or( (select a.MobileNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	or( (select a.EmployeePhoneNumber from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	or( (select a.EmployeePosition from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')
		or( (select a.BalanceLeave from UserProfile as a  where a.UserProfileId=m.UserProfileId) like '%' +@P_SmartSearch+ '%')	  or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.ResignationDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
		 or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.JoinDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%')   or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.BirthDate)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
		   	or ((select count(K.Religion) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then ReligionName else ArReligionName end)from M_Religion as c where c.ReligionID=k.Religion) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.CountryofResidence) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then CountryName else ArCountryName end)from M_Country as c where c.CountryID=k.CountryofResidence) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.Nationality) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then NationalityName else ArNationalityName end)from M_Nationality as c where c.NationalityID=k.Nationality) like '%'+@P_SmartSearch+'%')>0)
		  	or ((select count(K.PreviousNationality) from UserProfile as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or 
		K.DeleteFlag is null) and (SELECT  (case when @P_Language = 'EN' then NationalityName else ArNationalityName end)from M_Nationality as c where c.NationalityID=k.PreviousNationality) like '%'+@P_SmartSearch+'%')>0)
		or( (select (case when a.NotificationPreferencesEmail=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=163))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId  ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.NotificationPreferencesSMS=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=164))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Title =0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=165))
			when a.Title=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=166))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Gender=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=167))
			when a.Gender=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=168))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.Resigned=0 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=169))
			when a.Resigned=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=170))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
			or( (select (case when a.EmploymentStatus=1 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=182))
			when a.EmploymentStatus=2 then((select (case when @P_Language='EN' then DisplayName else ArDisplayName end) from M_Lookups where lookupsID=183))
			end) from UserProfile as a where a.UserProfileId=m.UserProfileId   ) like  '%'+@P_SmartSearch+'%')
		  or	((SELECT  CONVERT(nvarchar(10), cast((SELECT DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), a.CreatedDatetime)) as date), 103) from UserProfile as a  where a.UserProfileId=m.UserProfileId)  like '%'+@P_SmartSearch+'%') 
			or  (((select count(CONVERT(nvarchar(10), cast(K.TimePeriodFrom as date), 103)) from UserProfileEducation as K where K.UserProfileId=m.UserProfileId and (K.DeleteFlag=0 or K.DeleteFlag is null)
			and CONVERT(nvarchar(10), cast(k.TimePeriodFrom as date), 103) like '%'+@P_SmartSearch+'%')>0))
		  
		 )
		
	 end

	 if(@P_Method = 0 and @P_SmartSearch is null)
	 begin
	 select * from (SELECT row_number() over (Order By  [UserProfileId] desc) as slno,
		 * from @result) as m where slno between @StartNo and @EndNo 
	 end

	 if(@P_Method = 1 and @P_SmartSearch is null)
	 select count(*) from @result

 END 
 GO
 
 /****** Object:  StoredProcedure [dbo].[Save_UserProfile]  ******/
ALTER PROCEDURE [dbo].[Save_UserProfile]
	-- Add the parameters for the stored procedure here
	@P_EmployeeCode nvarchar(250)=null,
	@P_UserProfileId int = null,
	@P_EmployeeName nvarchar(250) = null,	
	@P_LoginUser nvarchar(250) = null,
	@P_OfficialMailId nvarchar(250) = null,
	@P_PersonalMailId nvarchar(250) = null,
	@P_IsOrgHead bit = null,
	@P_OrgUnitID int = null,
	@P_Gender nvarchar(250) = null,
	@P_BirthDate datetime = null,
	@P_Age int = 0,	
	@P_CountryofResidence int = null,
	@P_MobileNumber nvarchar(250) = null,
    @P_EmployeePhoneNumber nvarchar(250) = null,	
	@P_Religion int = null,
	@P_Nationality int = null,
	@P_PreviousNationality nvarchar(250) = null,
	@P_JoinDate datetime = null,	
	@P_Title nvarchar(250) = null,
	@P_Grade nvarchar(250) = null,
	@P_EmployeePosition nvarchar(250) = null,
    @P_EmploymentStatus nvarchar(250) = null,
	@P_Resigned nvarchar(250) = null,	
	@P_ResignationDate datetime = null,
	@P_BalanceLeave int = 0,
	@P_NotificationPreferencesEmail nvarchar(250) = null,
	@P_NotificationPreferencesSMS nvarchar(250) = null,
	@P_PassportNumber nvarchar(250) = null,
	@P_PassportIssuePlace nvarchar(250) = null,
	@P_PassportIssueDate datetime = null,
	@P_PassportExpiryDate datetime = null,
	@P_VisaNumber nvarchar(250) = null,
	@P_VisaIssueDate datetime = null,
	@P_VisaExpiryDate datetime = null,
	@P_EmiratesIdNumber nvarchar(250) = null,
	@P_EmiratesIdIssueDate datetime = null,
	@P_EmiratesIdExpiryDate datetime = null,
	@P_InsuranceNumber nvarchar(250) = null,
	@P_InsuranceIssueDate datetime = null,
	@P_InsuranceExpiryDate datetime = null,
	@P_LabourContractNumber nvarchar(250) = null,
	@P_LabourContractIssueDate datetime = null,
	@P_LabourContractExpiryDate datetime = null,
	@P_Action nvarchar(100)= null,
	@P_Comment nvarchar(Max) = null,
	@P_DeleteFlag bit = 0,
	@P_CreatedBy int = null,
	@P_UpdatedBy  int = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_RoleId nvarchar(250) = null,
	@P_EmployeePhoto nvarchar(250) = null,
	@P_ResidenceNumber nvarchar(250) = null,
	@P_ResidenceIssuePlace nvarchar(250) = null,
	@P_ResidenceIssueDate datetime = null,
	@P_EnteringDate datetime = null,
	@P_WorkAddress nvarchar(250) = null,
	@P_GraduationDate datetime = null,
	@P_BirthPlace nvarchar(250) = null,
	@P_Salary nvarchar(250) = null,
	@P_ContractTypes nvarchar(250) = null,
	@P_ProfilePhotoID nvarchar(max) = null,
	@P_ProfilePhotoName nvarchar(max) = null,
	@P_SignaturePhoto nvarchar(max) = null,
	@P_SignaturePhotoID nvarchar(max) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		

	set @P_NotificationPreferencesEmail = case when @P_NotificationPreferencesEmail = 'Email' then 1 else 0 end
	set @P_NotificationPreferencesSMS = case when @P_NotificationPreferencesSMS = 'SMS' then 1 else 0 end

	if(@P_UserProfileId is null or @P_UserProfileId =0)
	begin
	insert into [dbo].[UserProfile] ([EmployeeName],[EmployeeCode],[LoginUser],[OfficialMailId],[PersonalMailId],[IsOrgHead],[DepartmentID],[Gender],[BirthDate],[Age],[CountryofResidence],[MobileNumber],[EmployeePhoneNumber],[Religion],[Nationality],[PreviousNationality],[JoinDate],[Title],[Grade],[EmployeePosition],[EmploymentStatus],[Resigned],[ResignationDate],[BalanceLeave],[NotificationPreferencesEmail],[NotificationPreferencesSMS],[PassportNumber],[PassportIssuePlace],[PassportIssueDate],[PassportExpiryDate],[VisaNumber],[VisaIssueDate],[VisaExpiryDate],[EmiratesIdNumber],[EmiratesIdIssueDate],[EmiratesIdExpiryDate],[InsuranceNumber],[InsuranceIssueDate],[InsuranceExpiryDate],[LabourContractNumber],[LabourContractIssueDate],[LaborContractExpiryDate],[RoleId],[EmployeePhoto],[ResidenceNumber],[ResidenceIssuePlace],[ResidenceIssueDate],[EnteringDate],[WorkAddress],[GraduationDate],[BirthPlace],[Salary],[ContractTypes],[DeleteFlag],[CreatedBy],[CreatedDateTime],ProfilePhotoID,ProfilePhotoName,SignaturePhoto,SignaturePhotoID)
	select @P_EmployeeName,@P_EmployeeCode,@P_LoginUser,@P_OfficialMailId,@P_PersonalMailId,@P_IsOrgHead,@P_OrgUnitID,@P_Gender,@P_BirthDate,@P_Age,@P_CountryofResidence,@P_MobileNumber,@P_EmployeePhoneNumber,@P_Religion,@P_Nationality,@P_PreviousNationality,@P_JoinDate,@P_Title,@P_Grade,@P_EmployeePosition,@P_EmploymentStatus,@P_Resigned,@P_ResignationDate,@P_BalanceLeave,@P_NotificationPreferencesEmail,@P_NotificationPreferencesSMS,@P_PassportNumber,@P_PassportIssuePlace,@P_PassportIssueDate,@P_PassportExpiryDate,@P_VisaNumber,@P_VisaIssueDate,@P_VisaExpiryDate,@P_EmiratesIdNumber,@P_EmiratesIdIssueDate,@P_EmiratesIdExpiryDate,@P_InsuranceNumber,@P_InsuranceIssueDate,@P_InsuranceExpiryDate,@P_LabourContractNumber,@P_LabourContractIssueDate,@P_LabourContractExpiryDate,@P_RoleId,@P_EmployeePhoto,@P_ResidenceNumber,@P_ResidenceIssuePlace,@P_ResidenceIssueDate,@P_EnteringDate,@P_WorkAddress,@P_GraduationDate,@P_BirthPlace,@P_Salary,@P_ContractTypes,@P_DeleteFlag,@P_CreatedBy,@P_CreatedDateTime
	,@P_ProfilePhotoID,@P_ProfilePhotoName, @P_SignaturePhoto, @P_SignaturePhotoID
	set @P_UserProfileId = (SELECT IDENT_CURRENT('UserProfile'))
	end
	Else
	begin
	update [dbo].[UserProfile] set [EmployeeName]=@P_EmployeeName,EmployeeCode=@P_EmployeeCode,[LoginUser]=@P_LoginUser,[OfficialMailId]=@P_OfficialMailId,[PersonalMailId]=@P_PersonalMailId,[IsOrgHead]=@P_IsOrgHead,[DepartmentID]=@P_OrgUnitID,[Gender]=@P_Gender,[BirthDate]=@P_BirthDate,[Age]=@P_Age,[CountryofResidence]=@P_CountryofResidence,[MobileNumber]=@P_MobileNumber,[EmployeePhoneNumber]=@P_EmployeePhoneNumber,[Religion]=@P_Religion,[Nationality]=@P_Nationality,[PreviousNationality]=@P_PreviousNationality,[JoinDate]=@P_JoinDate,[Title]=@P_Title,[Grade]=@P_Grade,[EmployeePosition]=@P_EmployeePosition,[EmploymentStatus]=@P_EmploymentStatus,[Resigned]=@P_Resigned,[ResignationDate]=@P_ResignationDate,[BalanceLeave]=@P_BalanceLeave,[PassportNumber]=@P_PassportNumber,[PassportIssuePlace]=@P_PassportIssuePlace,[PassportIssueDate]=@P_PassportIssueDate,[PassportExpiryDate]=@P_PassportExpiryDate,[VisaNumber]=@P_VisaNumber,[VisaIssueDate]=@P_VisaIssueDate,
	[VisaExpiryDate]=@P_VisaExpiryDate,[EmiratesIdNumber]=@P_EmiratesIdNumber,[EmiratesIdIssueDate]=@P_EmiratesIdIssueDate,[EmiratesIdExpiryDate]=@P_EmiratesIdExpiryDate,[InsuranceNumber]=@P_InsuranceNumber,[InsuranceIssueDate]=@P_InsuranceIssueDate,[InsuranceExpiryDate]=@P_InsuranceExpiryDate,[LabourContractNumber]=@P_LabourContractNumber,[LabourContractIssueDate]=@P_LabourContractIssueDate,[LaborContractExpiryDate]=@P_LabourContractExpiryDate,[RoleId]=@P_RoleId,[EmployeePhoto]=@P_EmployeePhoto,[ResidenceNumber]=@P_ResidenceNumber,[ResidenceIssuePlace]=@P_ResidenceIssuePlace,[ResidenceIssueDate]=@P_ResidenceIssueDate,[EnteringDate]=@P_EnteringDate,[WorkAddress]=@P_WorkAddress,[GraduationDate]=@P_GraduationDate,[BirthPlace]=@P_BirthPlace,[Salary]=@P_Salary,[ContractTypes]=@P_ContractTypes,[DeleteFlag]=@P_DeleteFlag,[UpdatedBy]=@P_UpdatedBy,[UpdatedDateTime]=@P_UpdatedDateTime
	,[NotificationPreferencesSMS]=@P_NotificationPreferencesSMS,SignaturePhoto=@P_SignaturePhoto, [NotificationPreferencesEmail] = @P_NotificationPreferencesEmail,ProfilePhotoID=@P_ProfilePhotoID,ProfilePhotoName=@P_ProfilePhotoName, SignaturePhotoID=@P_SignaturePhotoID
	where UserProfileId=@P_UserProfileId
	end

	select UserProfileId,EmployeeCode,ReferenceNumber from [UserProfile] where UserProfileId=@P_UserProfileId
END