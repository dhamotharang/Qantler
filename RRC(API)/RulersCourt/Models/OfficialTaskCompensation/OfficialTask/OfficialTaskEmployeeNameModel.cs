using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficialTaskCompensation.OfficialTask
{
    [DataContract]
    public class OfficialTaskEmployeeNameModel
    {
        [DataMember(Name = "OfficialTaskEmployeeID")]
        public int? OfficialTaskEmployeeID { get; set; }

        [DataMember(Name = "OfficialTaskEmployeeName")]
        public string OfficialTaskEmployeeName { get; set; }
    }
}
