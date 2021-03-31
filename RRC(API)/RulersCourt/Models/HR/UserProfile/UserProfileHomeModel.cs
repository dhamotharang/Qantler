using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.UserProfile
{
    [DataContract]
    public class UserProfileHomeModel
    {
        [DataMember(Name = "EmployeesRegistered")]
        public int? EmployeesRegistered { get; set; }

        [DataMember(Name = "ExpiredPassportNumber")]
        public int? ExpiredPassportNumber { get; set; }

        [DataMember(Name = "ExpiredInsuranceNumber")]
        public int? ExpiredInsuranceNumber { get; set; }

        [DataMember(Name = "ExpiredEmiratesID")]
        public int? ExpiredEmiratesID { get; set; }

        [DataMember(Name = "ExpiredLabourContract")]
        public int? ExpiredLabourContract { get; set; }

        [DataMember(Name = "ExpiredVisa")]
        public int? ExpiredVisa { get; set; }
    }
}
