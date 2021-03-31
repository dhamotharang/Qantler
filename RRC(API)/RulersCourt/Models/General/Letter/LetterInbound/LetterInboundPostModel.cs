using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundPostModel
    {
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "ApproverId")]
        public int ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int ApproverDepartmentId { get; set; }

        [DataMember(Name = "ReceivingDate")]
        public DateTime? ReceivingDate { get; set; }

        [DataMember(Name = "ReceivedFromGovernmentEntity")]
        public string ReceivedFromGovernmentEntity { get; set; }

        [DataMember(Name = "ReceivedFromName")]
        public string ReceivedFromName { get; set; }

        [DataMember(Name = "ReceivedFromEntityID")]
        public int? ReceivedFromEntityID { get; set; }

        [DataMember(Name = "IsGovernmentEntity")]
        public bool? IsGovernmentEntity { get; set; }

        [DataMember(Name = "RelatedToIncomingLetter")]
        public string RelatedToIncomingLetter { get; set; }

        [DataMember(Name = "LetterDetails")]
        public string LetterDetails { get; set; }

        [DataMember(Name = "Notes")]
        public string Notes { get; set; }

        [DataMember(Name = "DocumentClassification")]
        public string DocumentClassification { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "NeedReply")]
        public bool? NeedReply { get; set; }

        [DataMember(Name = "LetterPhysicallySend")]
        public bool? LetterPhysicallySend { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DestinationUserId")]
        public List<LetterInboundDestinationUsersModel> DestinationUserId { get; set; }

        [DataMember(Name = "DestinationDepartmentId")]
        public List<LetterInboundDestinationDepartmentModel> DestinationDepartmentId { get; set; }

        [DataMember(Name = "RelatedOutgoingLetters")]
        public List<LetterInboundRelatedOutgoingModel> RelatedOutgoingLetters { get; set; }

        [DataMember(Name = "RelatedIncomingLetters")]
        public List<LetterInboundRelatedOutgoingModel> RelatedIncomingLetters { get; set; }

        [DataMember(Name = "Keywords")]
        public List<LetterInboundKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "Attachments")]
        public List<LetterAttachmentGetModel> Attachments { get; set; }
    }
}
