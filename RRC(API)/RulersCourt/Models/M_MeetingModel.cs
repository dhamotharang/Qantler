using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MeetingModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingTypeID { get; set; }

        [DataMember(Name = "MeetingTypeName")]
        public string MeetingTypeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}