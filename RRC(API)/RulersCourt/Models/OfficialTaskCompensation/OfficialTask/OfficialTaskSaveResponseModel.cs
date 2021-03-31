using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficialTask
{
    [DataContract]
    public class OfficialTaskSaveResponseModel
    {
        [DataMember(Name = "OfficialTaskID")]
        public int? OfficialTaskID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}
