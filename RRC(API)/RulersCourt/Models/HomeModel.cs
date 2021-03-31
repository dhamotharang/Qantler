using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class HomeModel
    {
        [DataMember(Name = "Circular")]
        public int? Circular { get; set; }

        [DataMember(Name = "Letters")]
        public int? Letters { get; set; }

        [DataMember(Name = "Memo")]
        public int? Memo { get; set; }

        [DataMember(Name = "DutyTask")]
        public int? DutyTask { get; set; }

        [DataMember(Name = "Meeting")]
        public int? Meeting { get; set; }

        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "NextMeetingDateTime")]
        public DateTime? NextMeetingDateTime { get; set; }
    }
}
