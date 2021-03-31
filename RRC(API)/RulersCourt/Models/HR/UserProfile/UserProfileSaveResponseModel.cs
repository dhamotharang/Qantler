using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfileSaveResponseModel
    {
        [DataMember(Name = "UserProfileId")]
        public int? UserProfileId { get; set; }

        [DataMember(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
