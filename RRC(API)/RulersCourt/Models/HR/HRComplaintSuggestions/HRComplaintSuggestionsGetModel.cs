using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HRComplaintSuggestions
{
    [DataContract]
    public class HRComplaintSuggestionsGetModel
    {
        [DataMember(Name = "HRComplaintSuggestionsID")]
        public int? HRComplaintSuggestionsID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [DataMember(Name = "RequestCreatedBy")]
        public string RequestCreatedBy { get; set; }

        [DataMember(Name = "MailID")]
        public string MailID { get; set; }

        [DataMember(Name = "ActionTaken")]
        public string ActionTaken { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

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

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<HRComplaintSuggestionsHistoryLogModel> HistoryLog { get; set; }
    }
}
