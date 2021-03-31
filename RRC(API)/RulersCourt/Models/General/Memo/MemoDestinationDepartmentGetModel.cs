using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class MemoDestinationDepartmentGetModel
    {
        [DataMember(Name = "MemoDestinationDepartmentID")]
        public int? MemoDestinationDepartmentID { get; set; }

        [DataMember(Name = "MemoDestinationDepartmentName")]
        public string MemoDestinationDepartmentName { get; set; }
    }
}
