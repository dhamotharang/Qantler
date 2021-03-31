using System.Runtime.Serialization;

namespace RulersCourt.Models.General
{
    [DataContract]
    public class HomeCountModel
    {
        [DataMember(Name = "DutyTask")]
        public int? DutyTask { get; set; }

        [DataMember(Name = "Memos")]
        public int? Memos { get; set; }

        [DataMember(Name = "Letter")]
        public int? Letter { get; set; }

        [DataMember(Name = "Meeting")]
        public int? Meeting { get; set; }

        [DataMember(Name = "Circular")]
        public int? Circular { get; set; }
    }
}
