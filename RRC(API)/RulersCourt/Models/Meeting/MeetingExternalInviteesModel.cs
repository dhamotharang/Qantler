using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingExternalInviteesModel
    {
        [DataMember(Name = "MeetingExternalInviteesID")]
        public int? MeetingExternalInviteesID { get; set; }

        [DataMember(Name = "Organization")]
        public string Organization { get; set; }

        [DataMember(Name = "ContactPerson")]
        public string ContactPerson { get; set; }

        [DataMember(Name = "EmailID")]
        public string EmailID { get; set; }
    }
}
