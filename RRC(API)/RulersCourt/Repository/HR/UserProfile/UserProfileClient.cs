using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.HR.UserProfile;
using RulersCourt.Models.UserProfile;
using RulersCourt.Translators;
using RulersCourt.Translators.UserProfile;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.UserProfile
{
    public class UserProfileClient
    {
        public UserProfileListModel GetUserProfile(string connString, int pageNumber, int pageSize, string userName, string orgDepartmentID, string smartSearch, string jobTitle, string type, string lang)
        {
            UserProfileListModel list = new UserProfileListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_UserName", userName),
                new SqlParameter("@P_OrgDepartmentID", orgDepartmentID),
                new SqlParameter("@P_JobTitle", jobTitle),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<UserProfileDashBoardListModel>>(connString, "Get_UserProfileList", r => r.TranslateAsUserProfileDashBoardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_UserName", userName),
                new SqlParameter("@P_JobTitle", jobTitle),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_OrgDepartmentID", orgDepartmentID),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_UserProfileList", parama);

            Parallel.Invoke(
              () => list.OrganizationList = GetM_Organisation(connString, lang));

            return list;
        }

        public UserProfileHomeModel GetUserProfileHomeCount(string connString, int userID)
        {
            SqlParameter[] getHomeCountParam = {
                    new SqlParameter("@P_UserID", userID) };

            return SqlHelper.ExecuteProcedureReturnData<UserProfileHomeModel>(connString, "Get_UserProfileDashboardCount", r => r.TranslateAsUserProfileHomeCount(), getHomeCountParam);
        }

        public UserProfileGetModel GetUserProfileByID(string connString, int userProfileId, int userID, string lang)
        {
            UserProfileGetModel userProfileDetails = new UserProfileGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_UserProfileId", userProfileId),
                new SqlParameter("@P_Language", lang)
            };
            SqlParameter[] getEducationparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            SqlParameter[] getWorkExperienceparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            SqlParameter[] getTrainingCertificationsparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            if (userProfileId != 0)
            {
                userProfileDetails = SqlHelper.ExecuteProcedureReturnData<List<UserProfileGetModel>>(connString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam).FirstOrDefault();

                if (userProfileDetails != null)
                {
                    userProfileDetails.Education = SqlHelper.ExecuteProcedureReturnData<List<UserProfileEducationModel>>(connString, "Get_UserProfileEducation", r => r.UserProfileTranslateAsEducationList(), getEducationparam);
                    userProfileDetails.WorkExperience = SqlHelper.ExecuteProcedureReturnData<List<UserProfileWorkExperienceModel>>(connString, "Get_UserProfileWorkExperience", r => r.UserProfileTranslateAsWorkExperienceList(), getWorkExperienceparam);
                    userProfileDetails.TrainingCertifications = SqlHelper.ExecuteProcedureReturnData<List<UserProfileTrainingCertificationsModel>>(connString, "Get_UserProfileTrainingCertifications", r => r.UserProfileTranslateAsTrainingCertificationsList(), getTrainingCertificationsparam);
                    if (userProfileDetails.TrainingCertifications != null)
                    {
                        int i = 0;
                        foreach (UserProfileTrainingCertificationsModel training in userProfileDetails.TrainingCertifications)
                        {
                            userProfileDetails.TrainingCertifications[i].Attachment = new UserProfileAttachmentClient().GetUserProfileAttachmentById(connString, training.TrainingID, "UserProfileTraining");
                            i++;
                        }
                    }

                    userProfileDetails.Attachment = new UserProfileAttachmentClient().GetUserProfileAttachmentById(connString, userProfileDetails.UserProfileId, "UserProfile");
                    userID = userProfileDetails.CreatedBy.GetValueOrDefault();
                }
            }

            if (userProfileDetails != null)
                Parallel.Invoke(() => userProfileDetails.M_OrganizationList = GetM_Organisation(connString, lang));

            return userProfileDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public UserProfileSaveResponseModel PostUserProfile(string connString, UserProfilePostModel userProfile)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_EmployeeName", userProfile.EmployeeName),
                new SqlParameter("@P_EmployeeCode", userProfile.EmployeeCode),
                new SqlParameter("@P_OfficialMailId", userProfile.OfficialMailId),
                new SqlParameter("@P_PersonalMailId", userProfile.PersonalMailId),
                new SqlParameter("@P_Gender", userProfile.Gender),
                new SqlParameter("@P_BirthDate", userProfile.BirthDate),
                new SqlParameter("@P_Age", userProfile.Age),
                new SqlParameter("@P_CountryofResidence", userProfile.CountryofResidence),
                new SqlParameter("@P_MobileNumber", userProfile.MobileNumber),
                new SqlParameter("@P_EmployeePhoneNumber", userProfile.EmployeePhoneNumber),
                new SqlParameter("@P_Religion", userProfile.Religion),
                new SqlParameter("@P_Nationality", userProfile.Nationality),
                new SqlParameter("@P_PreviousNationality", userProfile.PreviousNationality),
                new SqlParameter("@P_JoinDate", userProfile.JoinDate),
                new SqlParameter("@P_Title", userProfile.Title),
                new SqlParameter("@P_Grade", userProfile.Grade),
                new SqlParameter("@P_EmployeePosition", userProfile.EmployeePosition),
                new SqlParameter("@P_EmploymentStatus", userProfile.EmploymentStatus),
                new SqlParameter("@P_Resigned", userProfile.Resigned),
                new SqlParameter("@P_ResignationDate", userProfile.ResignationDate),
                new SqlParameter("@P_BalanceLeave", userProfile.BalanceLeave),
                new SqlParameter("@P_NotificationPreferencesSMS", userProfile.NotificationPreferencesSMS),
                new SqlParameter("@P_NotificationPreferencesEmail", userProfile.NotificationPreferencesEmail),
                new SqlParameter("@P_PassportNumber", userProfile.PassportNumber),
                new SqlParameter("@P_PassportIssuePlace", userProfile.PassportIssuePlace),
                new SqlParameter("@P_PassportIssueDate", userProfile.PassportIssueDate),
                new SqlParameter("@P_PassportExpiryDate", userProfile.PassportExpiryDate),
                new SqlParameter("@P_VisaNumber", userProfile.VisaNumber),
                new SqlParameter("@P_VisaIssueDate", userProfile.VisaIssueDate),
                new SqlParameter("@P_VisaExpiryDate", userProfile.VisaExpiryDate),
                new SqlParameter("@P_EmiratesIdNumber", userProfile.EmiratesIdNumber),
                new SqlParameter("@P_EmiratesIdIssueDate", userProfile.EmiratesIdIssueDate),
                new SqlParameter("@P_EmiratesIdExpiryDate", userProfile.EmiratesIdExpiryDate),
                new SqlParameter("@P_InsuranceNumber", userProfile.InsuranceNumber),
                new SqlParameter("@P_InsuranceIssueDate", userProfile.InsuranceIssueDate),
                new SqlParameter("@P_InsuranceExpiryDate", userProfile.InsuranceExpiryDate),
                new SqlParameter("@P_LabourContractNumber", userProfile.LabourContractNumber),
                new SqlParameter("@P_LabourContractIssueDate", userProfile.LabourContractIssueDate),
                new SqlParameter("@P_LabourContractExpiryDate", userProfile.LaborContractExpiryDate),
                new SqlParameter("@P_LoginUser", userProfile.LoginUser),
                new SqlParameter("@P_RoleId", userProfile.RoleId),
                new SqlParameter("@P_IsOrgHead", userProfile.IsOrgHead),
                new SqlParameter("@P_OrgUnitID", userProfile.OrgUnitID),
                new SqlParameter("@P_EmployeePhoto", userProfile.EmployeePhoto),
                new SqlParameter("@P_ResidenceNumber", userProfile.ResidenceNumber),
                new SqlParameter("@P_ResidenceIssuePlace", userProfile.ResidenceIssuePlace),
                new SqlParameter("@P_ResidenceIssueDate", userProfile.ResidenceIssueDate),
                new SqlParameter("@P_EnteringDate", userProfile.EnteringDate),
                new SqlParameter("@P_WorkAddress", userProfile.WorkAddress),
                new SqlParameter("@P_GraduationDate", userProfile.GraduationDate),
                new SqlParameter("@P_BirthPlace", userProfile.BirthPlace),
                new SqlParameter("@P_Salary", userProfile.Salary),
                new SqlParameter("@P_ContractTypes", userProfile.ContractTypes),
                new SqlParameter("@P_CreatedBy", userProfile.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", userProfile.CreatedDatetime),
                new SqlParameter("@P_ProfilePhotoID", userProfile.ProfilePhotoID),
                new SqlParameter("@P_ProfilePhotoName", userProfile.ProfilePhotoName),
                new SqlParameter("@P_SignaturePhoto", userProfile.SignaturePhoto),
                new SqlParameter("@P_SignaturePhotoID", userProfile.SignaturePhotoID)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<UserProfileSaveResponseModel>(connString, "Save_UserProfile", r => r.TranslateAsUserProfileSaveResponseList(), param);

            if (userProfile.Education != null)
                new UserProfileEducationClient().SaveEducation(connString, userProfile.Education, result.UserProfileId);

            if (userProfile.WorkExperience != null)
                new UserProfileWorkExperienceClient().SaveWorkExperience(connString, userProfile.WorkExperience, result.UserProfileId);

            if (userProfile.TrainingCertifications != null)
                new UserProfileTrainingCertificationsClient().SaveTrainingCertifications(connString, userProfile.TrainingCertifications, result.UserProfileId);

            if (userProfile.Attachment != null)
                new UserProfileAttachmentClient().PostUserProfileAttachments(connString, "UserProfile", userProfile.Attachment, result.UserProfileId, result.ReferenceNumber);
            return result;
        }

        public UserProfileSaveResponseModel PutUserProfile(string connString, UserProfilePutModel userProfile)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_UserProfileId", userProfile.UserProfileId),
                new SqlParameter("@P_EmployeeCode", userProfile.EmployeeCode),
                new SqlParameter("@P_EmployeeName", userProfile.EmployeeName),
                new SqlParameter("@P_OfficialMailId", userProfile.OfficialMailId),
                new SqlParameter("@P_PersonalMailId", userProfile.PersonalMailId),
                new SqlParameter("@P_Gender", userProfile.Gender),
                new SqlParameter("@P_BirthDate", userProfile.BirthDate),
                new SqlParameter("@P_Age", userProfile.Age),
                new SqlParameter("@P_CountryofResidence", userProfile.CountryofResidence),
                new SqlParameter("@P_MobileNumber", userProfile.MobileNumber),
                new SqlParameter("@P_EmployeePhoneNumber", userProfile.EmployeePhoneNumber),
                new SqlParameter("@P_Religion", userProfile.Religion),
                new SqlParameter("@P_Nationality", userProfile.Nationality),
                new SqlParameter("@P_PreviousNationality", userProfile.PreviousNationality),
                new SqlParameter("@P_JoinDate", userProfile.JoinDate),
                new SqlParameter("@P_Title", userProfile.Title),
                new SqlParameter("@P_Grade", userProfile.Grade),
                new SqlParameter("@P_EmployeePosition", userProfile.EmployeePosition),
                new SqlParameter("@P_EmploymentStatus", userProfile.EmploymentStatus),
                new SqlParameter("@P_Resigned", userProfile.Resigned),
                new SqlParameter("@P_ResignationDate", userProfile.ResignationDate),
                new SqlParameter("@P_BalanceLeave", userProfile.BalanceLeave),
                new SqlParameter("@P_NotificationPreferencesSMS", userProfile.NotificationPreferencesSMS),
                new SqlParameter("@P_NotificationPreferencesEmail", userProfile.NotificationPreferencesEmail),
                new SqlParameter("@P_PassportNumber", userProfile.PassportNumber),
                new SqlParameter("@P_PassportIssuePlace", userProfile.PassportIssuePlace),
                new SqlParameter("@P_PassportIssueDate", userProfile.PassportIssueDate),
                new SqlParameter("@P_PassportExpiryDate", userProfile.PassportExpiryDate),
                new SqlParameter("@P_VisaNumber", userProfile.VisaNumber),
                new SqlParameter("@P_VisaIssueDate", userProfile.VisaIssueDate),
                new SqlParameter("@P_VisaExpiryDate", userProfile.VisaExpiryDate),
                new SqlParameter("@P_EmiratesIdNumber", userProfile.EmiratesIdNumber),
                new SqlParameter("@P_EmiratesIdIssueDate", userProfile.EmiratesIdIssueDate),
                new SqlParameter("@P_EmiratesIdExpiryDate", userProfile.EmiratesIdExpiryDate),
                new SqlParameter("@P_InsuranceNumber", userProfile.InsuranceNumber),
                new SqlParameter("@P_InsuranceIssueDate", userProfile.InsuranceIssueDate),
                new SqlParameter("@P_InsuranceExpiryDate", userProfile.InsuranceExpiryDate),
                new SqlParameter("@P_LabourContractNumber", userProfile.LabourContractNumber),
                new SqlParameter("@P_LabourContractIssueDate", userProfile.LabourContractIssueDate),
                new SqlParameter("@P_LabourContractExpiryDate", userProfile.LaborContractExpiryDate),
                new SqlParameter("@P_LoginUser", userProfile.LoginUser),
                new SqlParameter("@P_RoleId", userProfile.RoleId),
                new SqlParameter("@P_IsOrgHead", userProfile.IsOrgHead),
                new SqlParameter("@P_OrgUnitID", userProfile.OrgUnitID),
                new SqlParameter("@P_EmployeePhoto", userProfile.EmployeePhoto),
                new SqlParameter("@P_ResidenceNumber", userProfile.ResidenceNumber),
                new SqlParameter("@P_ResidenceIssuePlace", userProfile.ResidenceIssuePlace),
                new SqlParameter("@P_ResidenceIssueDate", userProfile.ResidenceIssueDate),
                new SqlParameter("@P_EnteringDate", userProfile.EnteringDate),
                new SqlParameter("@P_WorkAddress", userProfile.WorkAddress),
                new SqlParameter("@P_GraduationDate", userProfile.GraduationDate),
                new SqlParameter("@P_BirthPlace", userProfile.BirthPlace),
                new SqlParameter("@P_Salary", userProfile.Salary),
                new SqlParameter("@P_ContractTypes", userProfile.ContractTypes),
                new SqlParameter("@P_UpdatedBy", userProfile.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", userProfile.UpdatedDateTime),
                new SqlParameter("@P_DeleteFlag", userProfile.DeleteFlag),
                new SqlParameter("@P_ProfilePhotoID", userProfile.ProfilePhotoID),
                new SqlParameter("@P_ProfilePhotoName", userProfile.ProfilePhotoName),
                new SqlParameter("@P_SignaturePhoto", userProfile.SignaturePhoto),
                new SqlParameter("@P_SignaturePhotoID", userProfile.SignaturePhotoID)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<UserProfileSaveResponseModel>(connString, "Save_UserProfile", r => r.TranslateAsUserProfileSaveResponseList(), param);

            if (userProfile.Education != null)
                new UserProfileEducationClient().SaveEducation(connString, userProfile.Education, result.UserProfileId);

            if (userProfile.WorkExperience != null)
                new UserProfileWorkExperienceClient().SaveWorkExperience(connString, userProfile.WorkExperience, result.UserProfileId);

            if (userProfile.TrainingCertifications != null)
                new UserProfileTrainingCertificationsClient().SaveTrainingCertifications(connString, userProfile.TrainingCertifications, result.UserProfileId);

            if (userProfile.Attachment != null)
                new UserProfileAttachmentClient().PostUserProfileAttachments(connString, "UserProfile", userProfile.Attachment, result.UserProfileId, result.ReferenceNumber);
            return result;
        }

        public string DeleteUserProfile(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_UserProfileId", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_UserProfileByID", param);
        }

        public UserProfilePutModel GetPatchUserProfileByID(string connString, int userProfileId, string lang)
        {
            UserProfilePutModel userProfileDetails = new UserProfilePutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_UserProfileId", userProfileId),
                new SqlParameter("@P_Language", lang)
            };
            SqlParameter[] getEducationparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            SqlParameter[] getWorkExperienceparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            SqlParameter[] getTrainingCertificationsparam = {
                new SqlParameter("@P_UserProfileId", userProfileId) };
            if (userProfileId != 0)
            {
                userProfileDetails = SqlHelper.ExecuteProcedureReturnData<List<UserProfilePutModel>>(connString, "Get_UserProfileByID", r => r.TranslateAsPutUserProfileList(), getparam).FirstOrDefault();

                userProfileDetails.Education = SqlHelper.ExecuteProcedureReturnData<List<UserProfileEducationModel>>(connString, "Get_UserProfileEducation", r => r.UserProfileTranslateAsEducationList(), getEducationparam);

                userProfileDetails.WorkExperience = SqlHelper.ExecuteProcedureReturnData<List<UserProfileWorkExperienceModel>>(connString, "Get_UserProfileWorkExperience", r => r.UserProfileTranslateAsWorkExperienceList(), getWorkExperienceparam);

                userProfileDetails.TrainingCertifications = SqlHelper.ExecuteProcedureReturnData<List<UserProfileTrainingCertificationsModel>>(connString, "Get_UserProfileTrainingCertifications", r => r.UserProfileTranslateAsTrainingCertificationsList(), getTrainingCertificationsparam);

                userProfileDetails.Attachment = new UserProfileAttachmentClient().GetUserProfileAttachmentById(connString, userProfileDetails.UserProfileId, "UserProfile");
            }

            return userProfileDetails;
        }

        internal UserProfileSaveResponseModel PatchUserProfile(string connString, int id, JsonPatchDocument<UserProfilePutModel> value, string lang)
        {
            var result = GetPatchUserProfileByID(connString, id, lang);
            value.ApplyTo(result);
            var res = PutUserProfile(connString, result);
            return res;
        }
    }
}