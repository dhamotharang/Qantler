using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_EmployeeStatusModel
    {
        [DataMember(Name = "EmployeeStatusID")]
        public int? EmployeeStatusID { get; set; }

        [DataMember(Name = "EmployeeStatusName")]
        public string EmployeeStatusName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
