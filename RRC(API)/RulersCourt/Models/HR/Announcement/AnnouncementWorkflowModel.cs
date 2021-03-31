using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    [DataContract]
    public class AnnouncementWorkflowModel
    {
        [DataMember(Name = "AnnouncementID")]
        public int? AnnouncementID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "HRHeadUsedID")]
        public List<UserModel> HRHeadUsedID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }
    }
}