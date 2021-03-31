using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_EventTypeModel
    {
        [DataMember(Name = "EventID")]
        public int? EventTypeID { get; set; }

        [DataMember(Name = "MeetingTypeName")]
        public string EventTypeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
