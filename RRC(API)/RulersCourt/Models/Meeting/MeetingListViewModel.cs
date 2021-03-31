using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingListViewModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "StartDateTime")]
        public DateTime? StartDateTime { get; set; }

        [DataMember(Name = "EndDateTime")]
        public DateTime? EndDateTime { get; set; }

        [DataMember(Name = "MeetingType")]
        public string MeetingType { get; set; }

        [DataMember(Name = "Invitees")]
        public string Invitees { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }
    }
}