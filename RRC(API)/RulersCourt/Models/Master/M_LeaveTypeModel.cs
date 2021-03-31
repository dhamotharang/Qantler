using System.Runtime.Serialization;

namespace RulersCourt.Models.Master
{
    [DataContract]
    public class M_LeaveTypeModel
    {
        [DataMember(Name = "LeaveID")]
        public int? LeaveID { get; set; }

        [DataMember(Name = "LeaveType")]
        public string LeaveType { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
