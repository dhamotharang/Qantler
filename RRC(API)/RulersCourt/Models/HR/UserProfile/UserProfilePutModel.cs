using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfilePutModel
    {
        [DataMember(Name = "UserProfileId")]
        public int? UserProfileId { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [DataMember(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        [DataMember(Name = "OfficialMailId")]
        public string OfficialMailId { get; set; }

        [DataMember(Name = "PersonalMailId")]
        public string PersonalMailId { get; set; }

        [DataMember(Name = "Gender")]
        public string Gender { get; set; }

        [DataMember(Name = "BirthDate")]
        public DateTime? BirthDate { get; set; }

        [DataMember(Name = "Age")]
        public int? Age { get; set; }

        [DataMember(Name = "CountryofResidence")]
        public int? CountryofResidence { get; set; }

        [DataMember(Name = "MobileNumber")]
        public string MobileNumber { get; set; }

        [DataMember(Name = "EmployeePhoneNumber")]
        public string EmployeePhoneNumber { get; set; }

        [DataMember(Name = "Religion")]
        public int? Religion { get; set; }

        [DataMember(Name = "Nationality")]
        public int? Nationality { get; set; }

        [DataMember(Name = "PreviousNationality")]
        public string PreviousNationality { get; set; }

        [DataMember(Name = "JoinDate")]
        public DateTime? JoinDate { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Grade")]
        public string Grade { get; set; }

        [DataMember(Name = "EmployeePosition")]
        public string EmployeePosition { get; set; }

        [DataMember(Name = "EmploymentStatus")]
        public string EmploymentStatus { get; set; }

        [DataMember(Name = "Resigned")]
        public string Resigned { get; set; }

        [DataMember(Name = "ResignationDate")]
        public DateTime? ResignationDate { get; set; }

        [DataMember(Name = "BalanceLeave")]
        public int? BalanceLeave { get; set; }

        [DataMember(Name = "NotificationPreferencesEmail")]
        public string NotificationPreferencesEmail { get; set; }

        [DataMember(Name = "NotificationPreferencesSMS")]
        public string NotificationPreferencesSMS { get; set; }

        [DataMember(Name = "Education")]
        public List<UserProfileEducationModel> Education { get; set; }

        [DataMember(Name = "WorkExperience")]
        public List<UserProfileWorkExperienceModel> WorkExperience { get; set; }

        [DataMember(Name = "TrainingCertifications")]
        public List<UserProfileTrainingCertificationsModel> TrainingCertifications { get; set; }

        [DataMember(Name = "ProfilePhotoID")]
        public string ProfilePhotoID { get; set; }

        [DataMember(Name = "ProfilePhotoName")]
        public string ProfilePhotoName { get; set; }

        [DataMember(Name = "PassportNumber")]
        public string PassportNumber { get; set; }

        [DataMember(Name = "PassportIssuePlace")]
        public string PassportIssuePlace { get; set; }

        [DataMember(Name = "PassportIssueDate")]
        public DateTime? PassportIssueDate { get; set; }

        [DataMember(Name = "PassportExpiryDate")]
        public DateTime? PassportExpiryDate { get; set; }

        [DataMember(Name = "VisaNumber")]
        public string VisaNumber { get; set; }

        [DataMember(Name = "VisaIssueDate")]
        public DateTime? VisaIssueDate { get; set; }

        [DataMember(Name = "VisaExpiryDate")]
        public DateTime? VisaExpiryDate { get; set; }

        [DataMember(Name = "EmiratesIdNumber")]
        public string EmiratesIdNumber { get; set; }

        [DataMember(Name = "EmiratesIdIssueDate")]
        public DateTime? EmiratesIdIssueDate { get; set; }

        [DataMember(Name = "EmiratesIdExpiryDate")]
        public DateTime? EmiratesIdExpiryDate { get; set; }

        [DataMember(Name = "InsuranceNumber")]
        public string InsuranceNumber { get; set; }

        [DataMember(Name = "InsuranceIssueDate")]
        public DateTime? InsuranceIssueDate { get; set; }

        [DataMember(Name = "InsuranceExpiryDate")]
        public DateTime? InsuranceExpiryDate { get; set; }

        [DataMember(Name = "LabourContractNumber")]
        public string LabourContractNumber { get; set; }

        [DataMember(Name = "LabourContractIssueDate")]
        public DateTime? LabourContractIssueDate { get; set; }

        [DataMember(Name = "LaborContractExpiryDate")]
        public DateTime? LaborContractExpiryDate { get; set; }

        [DataMember(Name = "RoleId")]
        public string RoleId { get; set; }

        [DataMember(Name = "LoginUser")]
        public string LoginUser { get; set; }

        [DataMember(Name = "OrgUnitID")]
        public int? OrgUnitID { get; set; }

        [DataMember(Name = "IsOrgHead")]
        public bool? IsOrgHead { get; set; }

        [DataMember(Name = "EmployeePhoto")]
        public string EmployeePhoto { get; set; }

        [DataMember(Name = "ResidenceNumber")]
        public string ResidenceNumber { get; set; }

        [DataMember(Name = "ResidenceIssuePlace")]
        public string ResidenceIssuePlace { get; set; }

        [DataMember(Name = "ResidenceIssueDate")]
        public DateTime? ResidenceIssueDate { get; set; }

        [DataMember(Name = "EnteringDate")]
        public DateTime? EnteringDate { get; set; }

        [DataMember(Name = "WorkAddress")]
        public string WorkAddress { get; set; }

        [DataMember(Name = "GraduationDate")]
        public DateTime? GraduationDate { get; set; }

        [DataMember(Name = "BirthPlace")]
        public string BirthPlace { get; set; }

        [DataMember(Name = "Salary")]
        public string Salary { get; set; }

        [DataMember(Name = "ContractTypes")]
        public string ContractTypes { get; set; }

        [DataMember(Name = "Attachment")]
        public List<UserProfileAttachmentGetModel> Attachment { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "SignaturePhoto")]
        public string SignaturePhoto { get; set; }

        [DataMember(Name = "SignaturePhotoID")]
        public string SignaturePhotoID { get; set; }
    }
}
