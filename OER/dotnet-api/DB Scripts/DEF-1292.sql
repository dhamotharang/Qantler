ALTER PROCEDURE [dbo].[sps_UserById]   
(  
@Id INT 
)  
AS  
BEGIN   
  
IF Exists(SELECT TOP 1 1 from UserMaster WHERE Id=@Id)   
 BEGIN   
  

  
 SELECT um.Id  
      ,um.TitleId, tm.[Name] AS Title  
      ,um.FirstName  
      ,um.MiddleName  
      ,um.LastName  
      ,um.CountryId, cm.[Name] AS Country,cm.[Name_Ar] AS Country_Ar  
      ,um.StateId, sm.[Name] AS [State] ,sm.[Name_Ar] AS [State_Ar]  
      ,um.Gender -- Enumeration  
      ,um.Email  
      ,um.PortalLanguageId -- Enumeration  
      --,um.DepartmentId, dm.Name as Department  
     -- ,um.DesignationId, dsm.Name as Designation  
      ,um.DateOfBirth  
      ,um.Photo  
      ,um.ProfileDescription  
      ,um.SubjectsInterested  
      ,um.ApprovalStatus  
      ,um.CreatedOn  
      ,um.UpdatedOn  
      ,um.Active  
      ,um.IsContributor, um.IsAdmin, um.IsEmailNotification  
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id  
      left JOIN CountryMaster cm on um.CountryId=cm.Id  
      left JOIN StateMaster sm on um.StateId=sm.Id  
     -- INNER JOIN DepartmentMaster dm on um.DepartmentId=dm.Id  
     -- INNER JOIN DesignationMaster dsm on um.DesignationId=dsm.Id  
  
     where um.Id=@Id  
 
 
 SELECT uc.Id
      ,UserId
      ,CertificationName
      ,[Year]
      ,uc.CreatedOn
  FROM UserCertification uc INNER JOIN UserMaster um on um.Id=uc.UserId WHERE um.Id=@Id

SELECT ue.Id
      ,UserId
      ,UniversitySchool
      ,Major
      ,Grade
      ,FromDate
      ,ToDate
      ,ue.CreatedOn
  FROM UserEducation ue INNER JOIN UserMaster um on um.Id=ue.UserId WHERE um.Id=@Id

  SELECT ul.Id
      ,UserId
      ,LanguageId, lm.[Name] as [Language]
      ,IsSpeak
      ,IsRead
      ,IsWrite
      ,ul.CreatedOn
  FROM UserLanguages ul INNER JOIN LanguageMaster lm on lm.Id=ul.LanguageId  
						INNER JOIN UserMaster um on um.Id=ul.UserId WHERE um.Id=@Id

  SELECT uexp.Id
      ,UserId
      ,OrganizationName
      ,Designation
      ,FromDate
      ,ToDate
      ,uexp.CreatedOn
  FROM dbo.UserExperiences uexp INNER JOIN UserMaster um on um.Id=uexp.UserId WHERE um.Id=@Id
  
SELECT sm.Id
      ,UserId
      ,SocialMediaId,smm.Name as SocialMedia
	  ,smm.Name_Ar
      ,[URL]
      ,sm.CreatedOn
  FROM dbo.UserSocialMedia sm INNER JOIN SocialMediaMaster smm on sm.SocialMediaId=smm.Id
  
  
  INNER JOIN UserMaster um on um.Id=sm.UserId WHERE um.Id=@Id

RETURN 105 -- record exists  
  
END  
  
ELSE RETURN 102 -- No record exist  
  
END  
