using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundGetModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "LetterReferenceNumber")]
        public string LetterReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "StatusCode")]
        public int? StatusCode { get; set; }

        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public string ApproverDepartmentId { get; set; }

        [DataMember(Name = "RelatedToIncomingLetter")]
        public string RelatedToIncomingLetter { get; set; }

        [DataMember(Name = "LetterDetails")]
        public string LetterDetails { get; set; }

        [DataMember(Name = "DocumentClassification")]
        public string DocumentClassification { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "NeedReply")]
        public bool? NeedReply { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "IsRedirect")]
        public int? IsRedirect { get; set; }

        [DataMember(Name = "DestinationEntity")]
        public List<LetterOutboundDestinationDepartmentModel> DestinationEntity { get; set; }

        [DataMember(Name = "Keywords")]
        public List<LetterOutboundKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "Attachments")]
        public List<LetterAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "RelatedOutgoingLetters")]
        public List<LetterOutboundRelatedOutgoingModel> RelatedOutgoingLetters { get; set; }

        [DataMember(Name = "RelatedIncomingLetters")]
        public List<LetterOutboundRelatedOutgoingModel> RelatedIncomingLetters { get; set; }

        [DataMember(Name = "LetterType")]
        public string LetterType { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> L_OrganizationList { get; set; }

        [DataMember(Name = "OrganisationEntity")]
        public List<OrganisationEntityModel> OrganisationEntity { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "L_KeywordsList")]
        public List<LetterOutboundKeywordsModel> L_KeywordsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<LetterOutboundHistoryLogModel> HistoryLog { get; set; }
    }
}
