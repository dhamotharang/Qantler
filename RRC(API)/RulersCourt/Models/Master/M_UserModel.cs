using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_UserModel
    {
        [DataMember(Name = "UserProfileID")]
        public int? UserProfileID { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }
    }
}
