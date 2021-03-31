using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MOMPostModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "PointsDiscussed")]
        public string PointsDiscussed { get; set; }

        [DataMember(Name = "PendingPoints")]
        public string PendingPoints { get; set; }

        [DataMember(Name = "Suggestion")]
        public string Suggestion { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
