using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class LinkToMeetingModel
    {
        [DataMember(Name = "MeetingReferenceNumber")]
        public string MeetingReferenceNumber { get; set; }

        [DataMember(Name = "MeetingID")]
        public string MeetingID { get; set; }
    }
}