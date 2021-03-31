using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingReport
    {
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "MeetingType")]
        public string MeetingType { get; set; }

        [DataMember(Name = "Invitees")]
        public string Invitees { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }
    }
}
