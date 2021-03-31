using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingSaveResponseModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "ReferenceNumber")]        public string ReferenceNumber { get; set; }        [DataMember(Name = "status")]        public int Status { get; set; }
    }
}
