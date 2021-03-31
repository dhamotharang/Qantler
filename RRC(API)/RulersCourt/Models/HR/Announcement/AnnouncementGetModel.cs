using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    [DataContract]
    public class AnnouncementGetModel
    {
        [DataMember(Name = "AnnouncementID")]
        public int? AnnouncementID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "AnnouncementType")]
        public string AnnouncementType { get; set; }

        [DataMember(Name = "AnnouncementDescription")]
        public string AnnouncementDescription { get; set; }

        [DataMember(Name = "AssigneeId")]
        public List<CurrentAssigneeModel> AssigneeId { get; set; }

        [DataMember(Name = "HRHeadUsedId")]
        public List<CurrentHRHeadModel> HRHeadUsedId { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<AnnouncementHistoryLogModel> HistoryLog { get; set; }

        [DataMember(Name = "AnnouncementTypeAndDescription")]
        public List<AnnouncementTypeAndDescriptionModel> AnnouncementTypeAndDescription { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }
    }
}
