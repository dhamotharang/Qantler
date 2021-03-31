using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_OfficialTaskRequestModel
    {
        [DataMember(Name = "OfficialTaskRequestID")]
        public int? OfficialTaskRequestID { get; set; }

        [DataMember(Name = "OfficialTaskRequestName")]
        public string OfficialTaskRequestName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
