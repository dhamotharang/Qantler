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
		from [dbo].M_Department as OU where DepartmentID = UP.DepartmentID ) as DepartmentName,
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
 
 ALTER PROCEDURE [dbo].[Save_M_VehicleModel]--1=>Specialization; 2=>Religion; 3=>OfficialTaskRequest; 4=>Nationality; 5=>MediaChannel;
												  --6=>Location; 7=>Language; 8=>Experience; 9=>EmployeeStatus; 10=>Emirates; 11=>Education; 
												  --12=>DesignType; 13=>Country; 14=>City	
	@P_LookupsID int = null,
	@P_DisplayName nvarchar(250) = null,
	@P_ARDisplayName nvarchar(250) = null,
	@P_DisplayOrder nvarchar(250) = null,
	@P_DeleteFlag bit = 0,
	@P_CreatedBy int = null,
	@P_UpdatedBy  int = null,
	@P_CreatedDateTime datetime = null,
	@P_UpdatedDateTime datetime = null,
	@P_Language nvarchar(10) = 'EN'
	
	

AS
BEGIN
	
	SET NOCOUNT ON;

	set @P_CreatedDateTime = (select GETUTCDATE());
	set @P_UpdatedDateTime = (select GETUTCDATE());

	if(@P_LookupsID is null or @P_LookupsID =0)
	begin 
		--if(@P_Language = 'EN')
		 insert into[dbo].[M_VehicleModel](VehicleModelARValue,VehicleModelENValue,DeleteFlag)
		select @P_ARDisplayName,@P_DisplayName,@P_DeleteFlag
		--else
		--insert into[dbo].[M_Location](ArLocationName,[DisplayOrder],[CreatedBy],[CreatedDateTime])
		--select @P_DisplayName,@P_DisplayOrder,@P_CreatedBy,@P_CreatedDateTime

		set @P_LookupsID = (SELECT IDENT_CURRENT('M_VehicleModel'))
		
	end
	Else if(@P_DeleteFlag != 1)
	begin
		--if(@P_Language = 'EN')
		update [dbo].[M_VehicleModel] set VehicleModelARValue=@P_ARDisplayName,VehicleModelENValue=@P_DisplayName where VehicleModelID=@P_LookupsID
		--else
		--update [dbo].[M_Location] set ArLocationName=@P_DisplayName,Displayorder=@P_DisplayOrder,UpdatedBy=@P_UpdatedBy,
		--UpdatedDateTime=@P_UpdatedDateTime where LocationID=@P_LookupsID
	end
	else
	begin
	update [dbo].[M_VehicleModel] set DeleteFlag=@P_DeleteFlag where VehicleModelID=@P_LookupsID
	end

	SELECT @P_LookupsID
END
GO

/****** Object:  StoredProcedure [dbo].[Get_MasterAdminManagement]   ******/
ALTER PROCEDURE [dbo].[Get_MasterAdminManagement]--1=>Specialization; 2=>Religion; 3=>OfficialTaskRequest; 4=>Nationality; 5=>MediaChannel;
												  --6=>Location; 7=>Language; 8=>Experience; 9=>EmployeeStatus; 10=>Emirates; 11=>Education; 
												  --12=>DesignType; 13=>Country; 14=>City	
    @P_Category nvarchar(250) = 17,
	@P_Search nvarchar(max) = null,
	@P_Language	 nvarchar(10)= 'EN'
AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @Result table(
	LookupsID int,
	DisplayName nvarchar(255),
	ArDisplayName nvarchar(255),
	CountryID  int,
	DisplayOrder int,
	CreatedBy int,
	UpdatedBy int,
	CreatedDateTime datetime,
	UpdatedDateTime datetime
	);

	if(@P_Category = 1)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select SpecializationID,SpecializationName,ArSpecializationName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Specialization   where DeleteFlag = 0
	end

	else if(@P_Category = 2)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select ReligionID,ReligionName,ArReligionName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Religion   where DeleteFlag = 0
	end

	else if(@P_Category = 3)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select OfficialTaskRequestID,OfficialTaskRequestName,ArOfficialTaskRequestName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_OfficialTaskRequest   where DeleteFlag = 0
	end

	else if(@P_Category = 4)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select NationalityID,NationalityName,ArNationalityName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Nationality   where DeleteFlag = 0
	end

	else if(@P_Category = 5)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select MediaChannelID,MediaChannelName,ArMediaChannelName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_MediaChannel   where DeleteFlag = 0
	end


	else if(@P_Category = 6)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select LocationID,LocationName,ArLocationName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Location   where DeleteFlag = 0
	end
	
	else if(@P_Category = 7)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select LanguageID,LanguageName,ArLanguageName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Language   where DeleteFlag = 0
	end

	else if(@P_Category = 8)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select ExperienceID,ExperienceName,ArExperienceName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Experience   where DeleteFlag = 0
	end

	else if(@P_Category = 9)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select EmployeeStatusID,EmployeeStatusName,ArEmployeeStatusName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_EmployeeStatus   where DeleteFlag = 0
	end

	else if(@P_Category = 10)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select EmiratesID,EmiratesName,ArEmiratesName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Emirates   where DeleteFlag = 0
	end

	else if(@P_Category = 11)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select EducationID,EducationName,ArEducationName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Education   where DeleteFlag = 0
	end

	else if(@P_Category = 12)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select DesignTypeID,DesignTypeName,ArDesignTypeName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_DesignType   where DeleteFlag = 0
	end

	else if(@P_Category = 13)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select CountryID,CountryName,ArCountryName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_Country   where DeleteFlag = 0
	end

	else if (@P_Category = 14)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,CountryID,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select CityID,CityName,ArCityName,CountryID,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_City   where DeleteFlag = 0
	end

	else if (@P_Category = 15)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select EventTypeID,EventTypeName,ArEventTypeName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_EventType   where DeleteFlag = 0
	end

	else if (@P_Category = 16)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select MeetingTypeID,MeetingTypeName,ArMeetingTypeName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_MeetingType   where DeleteFlag = 0
	end
	else if (@P_Category = 17)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName,DisplayOrder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime)
		select LeaveTypeID,LeaveTypeName,ArLeaveTypeName,Displayorder,CreatedBy,UpdatedBy,CreatedDateTime,UpdatedDateTime from M_LeaveType   where DeleteFlag = 0
	end

	else if (@P_Category = 18)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName)
		select TripTypeID,TripTypeENValue,TripTypeARValue from [M_TripType]   where DeleteFlag = 0
	end
	else if (@P_Category = 19)
	begin	
		insert into @result(LookupsID,DisplayName,ArDisplayName)
		select VehicleModelID,VehicleModelENValue,VehicleModelARValue from [M_VehicleModel] where DeleteFlag = 0
	end
	else if (@P_Category = 20)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName)
		select AnnouncementTypeID,AnnouncementTypeName,AnnouncementTypeNameAr from [AnnouncementTypeAndDescription] where DeleteFlag = 0
	end
	else if (@P_Category = 21)
	begin
		insert into @result(LookupsID,DisplayName,ArDisplayName)
		select AnnouncementTypeID,Description,DescriptionAr from [AnnouncementTypeAndDescription] where DeleteFlag = 0 and DescriptionAr is not null and Description is not null
	end

	if(@P_Search is not null)
	select * from @Result where DisplayName like  @P_Search+'%'  or ArDisplayName like @P_Search+'%'
	else
	select * from @Result 		
END
