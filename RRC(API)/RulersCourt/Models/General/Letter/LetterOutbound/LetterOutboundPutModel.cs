using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundPutModel
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

        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int? ApproverDepartmentId { get; set; }

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

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "LetterType")]
        public string LetterType { get; set; }

        [DataMember(Name = "DestinationRedirectedBy")]
        public int? DestinatoionRedirectedBy { get; set; }

        [DataMember(Name = "DestinationEntity")]
        public List<LetterOutboundDestinationDepartmentModel> DestinationEntity { get; set; }

        [DataMember(Name = "RelatedOutgoingLetters")]
        public List<LetterOutboundRelatedOutgoingModel> RelatedOutgoingLetters { get; set; }

        [DataMember(Name = "RelatedIncomingLetters")]
        public List<LetterOutboundRelatedOutgoingModel> RelatedIncomingLetters { get; set; }

        [DataMember(Name = "Keywords")]
        public List<LetterOutboundKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "Attachments")]
        public List<LetterAttachmentGetModel> Attachments { get; set; }
    }
}
