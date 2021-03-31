using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    [DataContract]
    public class AnnouncementSaveResponseModel
    {
        [DataMember(Name = "AnnouncementID")]
        public int? AnnouncementID { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
