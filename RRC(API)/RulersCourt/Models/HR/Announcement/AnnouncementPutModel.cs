using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    [DataContract]
    public class AnnouncementPutModel
    {
        [DataMember(Name = "AnnouncementID")]
        public int? AnnouncementID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "AnnouncementType")]
        public string AnnouncementType { get; set; }

        [DataMember(Name = "AnnouncementDescription")]
        public string AnnouncementDescription { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }
    }
}