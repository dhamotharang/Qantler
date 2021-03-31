using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Legal
{
    [DataContract]
    public class LegalGetModel
    {
        [DataMember(Name = "LegalID")]
        public int? LegalID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "RequestDetails")]
        public string RequestDetails { get; set; }

        [DataMember(Name = "Keywords")]
        public List<LegalKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "AssigneeID")]
        public List<CurrentLegalAssigneeModel> AssigneeID { get; set; }

        [DataMember(Name = "LegalHeadUserID")]
        public List<CurrentLegalHeadModel> LegalHeadUserID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "Attachments")]
        public List<LegalAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "LegalCommunicationHistory")]
        public List<LegalCommunicationHistory> CommunicationHistory { get; set; }
    }
}
