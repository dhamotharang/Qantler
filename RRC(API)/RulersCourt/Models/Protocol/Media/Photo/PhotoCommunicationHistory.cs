using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Photo
{
    [DataContract]
    public class PhotoCommunicationHistory
    {
        [DataMember(Name = "CommunicationID")]
        public int? CommunicationID { get; set; }

        [DataMember(Name = "PhotoID")]
        public int? PhotoID { get; set; }

        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "ParentCommunicationID")]
        public int? ParentCommunicationID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "Photo")]
        public string Photo { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
