using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.BabyAddition
{
    [DataContract]
    public class BabyAdditionGetModel
    {
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "BabyAdditionID")]
        public int? BabyAdditionID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "BabyName")]
        public string BabyName { get; set; }

        [DataMember(Name = "Gender")]
        public string Gender { get; set; }

        [DataMember(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [DataMember(Name = "HospitalName")]
        public string HospitalName { get; set; }

        [DataMember(Name = "CountryOfBirth")]
        public string CountryOfBirth { get; set; }

        [DataMember(Name = "CityOfBirth")]
        public string CityOfBirth { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

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

        [DataMember(Name = "AssigneeId")]
        public List<CurrentAssigneeModel> AssigneeId { get; set; }

        [DataMember(Name = "HRHeadUsedId")]
        public List<CurrentHRHeadModel> HRHeadUsedId { get; set; }

        [DataMember(Name = "Attachments")]
        public List<BabyAdditionAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<BabyAdditionHistoryLogModel> HistoryLog { get; set; }
    }
}
