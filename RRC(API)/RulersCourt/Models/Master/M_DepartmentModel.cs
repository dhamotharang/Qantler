using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_DepartmentModel
    {
        [DataMember(Name = "DepartmentID")]
        public int? DepartmentID { get; set; }

        [DataMember(Name = "DepartmentName")]
        public string DepartmentName { get; set; }
    }
}
