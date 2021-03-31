using RulersCourt.Models.HR.UserProfile;
using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileTranslator
    {
        public static UserProfileGetModel TranslateAsUserProfile(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileModel = new UserProfileGetModel();

            if (reader.IsColumnExists("UserProfileId"))
                userProfileModel.UserProfileId = SqlHelper.GetNullableInt32(reader, "UserProfileId");

            if (reader.IsColumnExists("ProfilePhotoID"))
                userProfileModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");

            if (reader.IsColumnExists("ProfilePhotoName"))
                userProfileModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");

            if (reader.IsColumnExists("ReferenceNumber"))
                userProfileModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("EmployeeName"))
                userProfileModel.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("EmployeeCode"))
                userProfileModel.EmployeeCode = SqlHelper.GetNullableString(reader, "EmployeeCode");

            if (reader.IsColumnExists("OfficialMailId"))
                userProfileModel.OfficialMailId = SqlHelper.GetNullableString(reader, "OfficialMailId");

            if (reader.IsColumnExists("PersonalMailId"))
                userProfileModel.PersonalMailId = SqlHelper.GetNullableString(reader, "PersonalMailId");

            if (reader.IsColumnExists("Gender"))
                userProfileModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("BirthDate"))
                userProfileModel.BirthDate = SqlHelper.GetDateTime(reader, "BirthDate");

            if (reader.IsColumnExists("Age"))
                userProfileModel.Age = SqlHelper.GetNullableInt32(reader, "Age");

            if (reader.IsColumnExists("CountryofResidence"))
                userProfileModel.CountryofResidence = SqlHelper.GetNullableInt32(reader, "CountryofResidence");

            if (reader.IsColumnExists("MobileNumber"))
                userProfileModel.MobileNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            if (reader.IsColumnExists("EmployeePhoneNumber"))
                userProfileModel.EmployeePhoneNumber = SqlHelper.GetNullableString(reader, "EmployeePhoneNumber");

            if (reader.IsColumnExists("Religion"))
                userProfileModel.Religion = SqlHelper.GetNullableInt32(reader, "Religion");

            if (reader.IsColumnExists("Nationality"))
                userProfileModel.Nationality = SqlHelper.GetNullableInt32(reader, "Nationality");

            if (reader.IsColumnExists("PreviousNationality"))
                userProfileModel.PreviousNationality = SqlHelper.GetNullableString(reader, "PreviousNationality");

            if (reader.IsColumnExists("JoinDate"))
                userProfileModel.JoinDate = SqlHelper.GetDateTime(reader, "JoinDate");

            if (reader.IsColumnExists("Title"))
                userProfileModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Grade"))
                userProfileModel.Grade = SqlHelper.GetNullableString(reader, "Grade");

            if (reader.IsColumnExists("EmployeePosition"))
                userProfileModel.EmployeePosition = SqlHelper.GetNullableString(reader, "EmployeePosition");

            if (reader.IsColumnExists("EmploymentStatus"))
                userProfileModel.EmploymentStatus = SqlHelper.GetNullableString(reader, "EmploymentStatus");

            if (reader.IsColumnExists("Resigned"))
                userProfileModel.Resigned = SqlHelper.GetNullableString(reader, "Resigned");

            if (reader.IsColumnExists("ResignationDate"))
                userProfileModel.ResignationDate = SqlHelper.GetDateTime(reader, "ResignationDate");

            if (reader.IsColumnExists("BalanceLeave"))
                userProfileModel.BalanceLeave = SqlHelper.GetNullableInt32(reader, "BalanceLeave");

            if (reader.IsColumnExists("NotificationPreferencesEmail"))
                userProfileModel.NotificationPreferencesEmail = SqlHelper.GetNullableString(reader, "NotificationPreferencesEmail");
            if (reader.IsColumnExists("NotificationPreferencesSMS"))
                userProfileModel.NotificationPreferencesSMS = SqlHelper.GetNullableString(reader, "NotificationPreferencesSMS");

            if (reader.IsColumnExists("PassportNumber"))
                userProfileModel.PassportNumber = SqlHelper.GetNullableString(reader, "PassportNumber");

            if (reader.IsColumnExists("PassportIssuePlace"))
                userProfileModel.PassportIssuePlace = SqlHelper.GetNullableString(reader, "PassportIssuePlace");

            if (reader.IsColumnExists("PassportIssueDate"))
                userProfileModel.PassportIssueDate = SqlHelper.GetDateTime(reader, "PassportIssueDate");

            if (reader.IsColumnExists("PassportExpiryDate"))
                userProfileModel.PassportExpiryDate = SqlHelper.GetDateTime(reader, "PassportExpiryDate");

            if (reader.IsColumnExists("VisaNumber"))
                userProfileModel.VisaNumber = SqlHelper.GetNullableString(reader, "VisaNumber");

            if (reader.IsColumnExists("VisaIssueDate"))
                userProfileModel.VisaIssueDate = SqlHelper.GetDateTime(reader, "VisaIssueDate");

            if (reader.IsColumnExists("VisaExpiryDate"))
                userProfileModel.VisaExpiryDate = SqlHelper.GetDateTime(reader, "VisaExpiryDate");

            if (reader.IsColumnExists("EmiratesIdNumber"))
                userProfileModel.EmiratesIdNumber = SqlHelper.GetNullableString(reader, "EmiratesIdNumber");

            if (reader.IsColumnExists("EmiratesIdIssueDate"))
                userProfileModel.EmiratesIdIssueDate = SqlHelper.GetDateTime(reader, "EmiratesIdIssueDate");

            if (reader.IsColumnExists("EmiratesIdExpiryDate"))
                userProfileModel.EmiratesIdExpiryDate = SqlHelper.GetDateTime(reader, "EmiratesIdExpiryDate");

            if (reader.IsColumnExists("InsuranceNumber"))
                userProfileModel.InsuranceNumber = SqlHelper.GetNullableString(reader, "InsuranceNumber");

            if (reader.IsColumnExists("InsuranceIssueDate"))
                userProfileModel.InsuranceIssueDate = SqlHelper.GetDateTime(reader, "InsuranceIssueDate");

            if (reader.IsColumnExists("InsuranceExpiryDate"))
                userProfileModel.InsuranceExpiryDate = SqlHelper.GetDateTime(reader, "InsuranceExpiryDate");

            if (reader.IsColumnExists("LabourContractNumber"))
                userProfileModel.LabourContractNumber = SqlHelper.GetNullableString(reader, "LabourContractNumber");

            if (reader.IsColumnExists("LabourContractIssueDate"))
                userProfileModel.LabourContractIssueDate = SqlHelper.GetDateTime(reader, "LabourContractIssueDate");

            if (reader.IsColumnExists("LaborContractExpiryDate"))
                userProfileModel.LaborContractExpiryDate = SqlHelper.GetDateTime(reader, "LaborContractExpiryDate");

            if (reader.IsColumnExists("RoleId"))
                userProfileModel.RoleId = SqlHelper.GetNullableString(reader, "RoleId");

            if (reader.IsColumnExists("LoginUser"))
                userProfileModel.LoginUser = SqlHelper.GetNullableString(reader, "LoginUser");

            if (reader.IsColumnExists("OrgUnitID"))
                userProfileModel.OrgUnitID = SqlHelper.GetNullableInt32(reader, "OrgUnitID");

            if (reader.IsColumnExists("IsOrgHead"))
                userProfileModel.IsOrgHead = SqlHelper.GetBoolean(reader, "IsOrgHead");

            if (reader.IsColumnExists("EmployeePhoto"))
                userProfileModel.EmployeePhoto = SqlHelper.GetNullableString(reader, "EmployeePhoto");

            if (reader.IsColumnExists("ResidenceNumber"))
                userProfileModel.ResidenceNumber = SqlHelper.GetNullableString(reader, "ResidenceNumber");

            if (reader.IsColumnExists("ResidenceIssuePlace"))
                userProfileModel.ResidenceIssuePlace = SqlHelper.GetNullableString(reader, "ResidenceIssuePlace");

            if (reader.IsColumnExists("ResidenceIssueDate"))
                userProfileModel.ResidenceIssueDate = SqlHelper.GetDateTime(reader, "ResidenceIssueDate");

            if (reader.IsColumnExists("EnteringDate"))
                userProfileModel.EnteringDate = SqlHelper.GetDateTime(reader, "EnteringDate");

            if (reader.IsColumnExists("WorkAddress"))
                userProfileModel.WorkAddress = SqlHelper.GetNullableString(reader, "WorkAddress");

            if (reader.IsColumnExists("GraduationDate"))
                userProfileModel.GraduationDate = SqlHelper.GetDateTime(reader, "GraduationDate");

            if (reader.IsColumnExists("BirthPlace"))
                userProfileModel.BirthPlace = SqlHelper.GetNullableString(reader, "BirthPlace");

            if (reader.IsColumnExists("Salary"))
                userProfileModel.Salary = SqlHelper.GetNullableString(reader, "Salary");

            if (reader.IsColumnExists("ContractTypes"))
                userProfileModel.ContractTypes = SqlHelper.GetNullableString(reader, "ContractTypes");

            if (reader.IsColumnExists("CreatedBy"))
                userProfileModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                userProfileModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                userProfileModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                userProfileModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("SignaturePhoto"))
                userProfileModel.SignaturePhoto = SqlHelper.GetNullableString(reader, "SignaturePhoto");

            if (reader.IsColumnExists("SignaturePhotoID"))
                userProfileModel.SignaturePhotoID = SqlHelper.GetNullableString(reader, "SignaturePhotoID");

            return userProfileModel;
        }

        public static List<UserProfileGetModel> TranslateAsUserProfileList(this SqlDataReader reader)
        {
            var userProfileList = new List<UserProfileGetModel>();
            while (reader.Read())
            {
                userProfileList.Add(TranslateAsUserProfile(reader, true));
            }

            return userProfileList;
        }

        public static UserProfileDashBoardListModel UserProfileDashBoardListSet(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileModel = new UserProfileDashBoardListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                userProfileModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("UserProfileId"))
                userProfileModel.UserProfileId = SqlHelper.GetNullableInt32(reader, "UserProfileId");

            if (reader.IsColumnExists("EmployeeName"))
                userProfileModel.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("DepartmentName"))
                userProfileModel.DepartmentName = SqlHelper.GetNullableString(reader, "DepartmentName");

            if (reader.IsColumnExists("JobTitle"))
                userProfileModel.JobTitle = SqlHelper.GetNullableString(reader, "JobTitle");

            return userProfileModel;
        }

        public static List<UserProfileDashBoardListModel> TranslateAsUserProfileDashBoardList(this SqlDataReader reader)
        {
            var list = new List<UserProfileDashBoardListModel>();
            while (reader.Read())
            {
                list.Add(UserProfileDashBoardListSet(reader, true));
            }

            return list;
        }

        public static UserProfileHomeModel TranslateAsUserProfileHomeCounts(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var label = new UserProfileHomeModel();

            if (reader.IsColumnExists("EmployeesRegistered"))
                label.EmployeesRegistered = SqlHelper.GetNullableInt32(reader, "EmployeesRegistered");

            if (reader.IsColumnExists("ExpiredPassportNumber"))
                label.ExpiredPassportNumber = SqlHelper.GetNullableInt32(reader, "ExpiredPassportNumber");

            if (reader.IsColumnExists("ExpiredInsuranceNumber"))
                label.ExpiredInsuranceNumber = SqlHelper.GetNullableInt32(reader, "ExpiredInsuranceNumber");

            if (reader.IsColumnExists("ExpiredEmiratesID"))
                label.ExpiredEmiratesID = SqlHelper.GetNullableInt32(reader, "ExpiredEmiratesID");

            if (reader.IsColumnExists("ExpiredLabourContract"))
                label.ExpiredLabourContract = SqlHelper.GetNullableInt32(reader, "ExpiredLabourContract");

            if (reader.IsColumnExists("ExpiredVisa"))
                label.ExpiredVisa = SqlHelper.GetNullableInt32(reader, "ExpiredVisa");

            return label;
        }

        public static UserProfileHomeModel TranslateAsUserProfileHomeCount(this SqlDataReader reader)
        {
            var homeModel = new UserProfileHomeModel();
            while (reader.Read())
            {
                homeModel = TranslateAsUserProfileHomeCounts(reader, true);
            }

            return homeModel;
        }

        public static UserProfilePutModel TranslateAsPutUserProfile(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileModel = new UserProfilePutModel();

            if (reader.IsColumnExists("UserProfileId"))
                userProfileModel.UserProfileId = SqlHelper.GetNullableInt32(reader, "UserProfileId");

            if (reader.IsColumnExists("EmployeeName"))
                userProfileModel.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("ProfilePhotoID"))
                userProfileModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");

            if (reader.IsColumnExists("ProfilePhotoName"))
                userProfileModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");

            if (reader.IsColumnExists("OfficialMailId"))
                userProfileModel.OfficialMailId = SqlHelper.GetNullableString(reader, "OfficialMailId");

            if (reader.IsColumnExists("PersonalMailId"))
                userProfileModel.PersonalMailId = SqlHelper.GetNullableString(reader, "PersonalMailId");

            if (reader.IsColumnExists("Gender"))
                userProfileModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("BirthDate"))
                userProfileModel.BirthDate = SqlHelper.GetDateTime(reader, "BirthDate");

            if (reader.IsColumnExists("Age"))
                userProfileModel.Age = SqlHelper.GetNullableInt32(reader, "Age");

            if (reader.IsColumnExists("CountryofResidence"))
                userProfileModel.CountryofResidence = SqlHelper.GetNullableInt32(reader, "CountryofResidence");

            if (reader.IsColumnExists("MobileNumber"))
                userProfileModel.MobileNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            if (reader.IsColumnExists("EmployeePhoneNumber"))
                userProfileModel.EmployeePhoneNumber = SqlHelper.GetNullableString(reader, "EmployeePhoneNumber");

            if (reader.IsColumnExists("Religion"))
                userProfileModel.Religion = SqlHelper.GetNullableInt32(reader, "Religion");

            if (reader.IsColumnExists("Nationality"))
                userProfileModel.Nationality = SqlHelper.GetNullableInt32(reader, "Nationality");

            if (reader.IsColumnExists("PreviousNationality"))
                userProfileModel.PreviousNationality = SqlHelper.GetNullableString(reader, "PreviousNationality");

            if (reader.IsColumnExists("JoinDate"))
                userProfileModel.JoinDate = SqlHelper.GetDateTime(reader, "JoinDate");

            if (reader.IsColumnExists("Title"))
                userProfileModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Grade"))
                userProfileModel.Grade = SqlHelper.GetNullableString(reader, "Grade");

            if (reader.IsColumnExists("EmployeePosition"))
                userProfileModel.EmployeePosition = SqlHelper.GetNullableString(reader, "EmployeePosition");

            if (reader.IsColumnExists("EmploymentStatus"))
                userProfileModel.EmploymentStatus = SqlHelper.GetNullableString(reader, "EmploymentStatus");

            if (reader.IsColumnExists("Resigned"))
                userProfileModel.Resigned = SqlHelper.GetNullableString(reader, "Resigned");

            if (reader.IsColumnExists("ResignationDate"))
                userProfileModel.ResignationDate = SqlHelper.GetDateTime(reader, "ResignationDate");

            if (reader.IsColumnExists("BalanceLeave"))
                userProfileModel.BalanceLeave = SqlHelper.GetNullableInt32(reader, "BalanceLeave");

            if (reader.IsColumnExists("NotificationPreferencesEmail"))
                userProfileModel.NotificationPreferencesEmail = SqlHelper.GetNullableString(reader, "NotificationPreferencesEmail");

            if (reader.IsColumnExists("NotificationPreferencesSMS"))
                userProfileModel.NotificationPreferencesSMS = SqlHelper.GetNullableString(reader, "NotificationPreferencesSMS");

            if (reader.IsColumnExists("PassportNumber"))
                userProfileModel.PassportNumber = SqlHelper.GetNullableString(reader, "PassportNumber");

            if (reader.IsColumnExists("PassportIssuePlace"))
                userProfileModel.PassportIssuePlace = SqlHelper.GetNullableString(reader, "PassportIssuePlace");

            if (reader.IsColumnExists("PassportIssueDate"))
                userProfileModel.PassportIssueDate = SqlHelper.GetDateTime(reader, "PassportIssueDate");

            if (reader.IsColumnExists("PassportExpiryDate"))
                userProfileModel.PassportExpiryDate = SqlHelper.GetDateTime(reader, "PassportExpiryDate");

            if (reader.IsColumnExists("VisaNumber"))
                userProfileModel.VisaNumber = SqlHelper.GetNullableString(reader, "VisaNumber");

            if (reader.IsColumnExists("VisaIssueDate"))
                userProfileModel.VisaIssueDate = SqlHelper.GetDateTime(reader, "VisaIssueDate");

            if (reader.IsColumnExists("VisaExpiryDate"))
                userProfileModel.VisaExpiryDate = SqlHelper.GetDateTime(reader, "VisaExpiryDate");

            if (reader.IsColumnExists("EmiratesIdNumber"))
                userProfileModel.EmiratesIdNumber = SqlHelper.GetNullableString(reader, "EmiratesIdNumber");

            if (reader.IsColumnExists("EmiratesIdIssueDate"))
                userProfileModel.EmiratesIdIssueDate = SqlHelper.GetDateTime(reader, "EmiratesIdIssueDate");

            if (reader.IsColumnExists("EmiratesIdExpiryDate"))
                userProfileModel.EmiratesIdExpiryDate = SqlHelper.GetDateTime(reader, "EmiratesIdExpiryDate");

            if (reader.IsColumnExists("InsuranceNumber"))
                userProfileModel.InsuranceNumber = SqlHelper.GetNullableString(reader, "InsuranceNumber");

            if (reader.IsColumnExists("InsuranceIssueDate"))
                userProfileModel.InsuranceIssueDate = SqlHelper.GetDateTime(reader, "InsuranceIssueDate");

            if (reader.IsColumnExists("InsuranceExpiryDate"))
                userProfileModel.InsuranceExpiryDate = SqlHelper.GetDateTime(reader, "InsuranceExpiryDate");

            if (reader.IsColumnExists("LabourContractNumber"))
                userProfileModel.LabourContractNumber = SqlHelper.GetNullableString(reader, "LabourContractNumber");

            if (reader.IsColumnExists("LabourContractIssueDate"))
                userProfileModel.LabourContractIssueDate = SqlHelper.GetDateTime(reader, "LabourContractIssueDate");

            if (reader.IsColumnExists("LaborContractExpiryDate"))
                userProfileModel.LaborContractExpiryDate = SqlHelper.GetDateTime(reader, "LaborContractExpiryDate");

            if (reader.IsColumnExists("RoleId"))
                userProfileModel.RoleId = SqlHelper.GetNullableString(reader, "RoleId");

            if (reader.IsColumnExists("LoginUser"))
                userProfileModel.LoginUser = SqlHelper.GetNullableString(reader, "LoginUser");

            if (reader.IsColumnExists("OrgUnitID"))
                userProfileModel.OrgUnitID = SqlHelper.GetNullableInt32(reader, "OrgUnitID");

            if (reader.IsColumnExists("IsOrgHead"))
                userProfileModel.IsOrgHead = SqlHelper.GetBoolean(reader, "IsOrgHead");

            if (reader.IsColumnExists("EmployeePhoto"))
                userProfileModel.EmployeePhoto = SqlHelper.GetNullableString(reader, "EmployeePhoto");

            if (reader.IsColumnExists("SignaturePhoto"))
                userProfileModel.SignaturePhoto = SqlHelper.GetNullableString(reader, "SignaturePhoto");

            if (reader.IsColumnExists("SignaturePhotoID"))
                userProfileModel.SignaturePhotoID = SqlHelper.GetNullableString(reader, "SignaturePhotoID");

            if (reader.IsColumnExists("ResidenceNumber"))
                userProfileModel.ResidenceNumber = SqlHelper.GetNullableString(reader, "ResidenceNumber");

            if (reader.IsColumnExists("ResidenceIssuePlace"))
                userProfileModel.ResidenceIssuePlace = SqlHelper.GetNullableString(reader, "ResidenceIssuePlace");

            if (reader.IsColumnExists("ResidenceIssueDate"))
                userProfileModel.ResidenceIssueDate = SqlHelper.GetDateTime(reader, "ResidenceIssueDate");

            if (reader.IsColumnExists("EnteringDate"))
                userProfileModel.EnteringDate = SqlHelper.GetDateTime(reader, "EnteringDate");

            if (reader.IsColumnExists("WorkAddress"))
                userProfileModel.WorkAddress = SqlHelper.GetNullableString(reader, "WorkAddress");

            if (reader.IsColumnExists("GraduationDate"))
                userProfileModel.GraduationDate = SqlHelper.GetDateTime(reader, "GraduationDate");

            if (reader.IsColumnExists("BirthPlace"))
                userProfileModel.BirthPlace = SqlHelper.GetNullableString(reader, "BirthPlace");

            if (reader.IsColumnExists("Salary"))
                userProfileModel.Salary = SqlHelper.GetNullableString(reader, "Salary");

            if (reader.IsColumnExists("ContractTypes"))
                userProfileModel.ContractTypes = SqlHelper.GetNullableString(reader, "ContractTypes");

            if (reader.IsColumnExists("UpdatedBy"))
                userProfileModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                userProfileModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return userProfileModel;
        }

        public static List<UserProfilePutModel> TranslateAsPutUserProfileList(this SqlDataReader reader)
        {
            var userProfileList = new List<UserProfilePutModel>();
            while (reader.Read())
            {
                userProfileList.Add(TranslateAsPutUserProfile(reader, true));
            }

            return userProfileList;
        }
    }
}
