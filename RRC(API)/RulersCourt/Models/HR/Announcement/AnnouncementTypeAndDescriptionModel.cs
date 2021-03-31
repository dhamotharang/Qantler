using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    [DataContract]
    public class AnnouncementTypeAndDescriptionModel
    {
        [DataMember(Name = "AnnouncementTypeID")]
        public int? AnnouncementTypeID { get; set; }

        [DataMember(Name = "AnnouncementTypeName")]
        public string AnnouncementTypeName { get; set; }

        [DataMember(Name = "AnnouncementTypeNameAr")]
        public string AnnouncementTypeNameAr { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "DescriptionAr")]
        public string DescriptionAr { get; set; }
    }
}