using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.UserProfile
{
    [DataContract]
    public class UserProfileDashBoardListModel
    {
        [DataMember(Name = "referenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "UserProfileId")]
        public int? UserProfileId { get; set; }

        [DataMember(Name = "DepartmentName")]
        public string DepartmentName { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [DataMember(Name = "JobTitle")]
        public string JobTitle { get; set; }
    }
}
