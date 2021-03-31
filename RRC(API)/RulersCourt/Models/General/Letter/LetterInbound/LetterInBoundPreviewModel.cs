using RulersCourt.Models.Letter.LetterInbound;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Letter.LetterInbound
{
    [DataContract]
    public class LetterInBoundPreviewModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "LetterReferenceNumber")]
        public string LetterReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "LetterDetails")]
        public string LetterDetails { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "DestinationTitle")]
        public string DestinationTitle { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "ApproverName")]
        public string ApproverName { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public string ApproverDepartmentId { get; set; }

        [DataMember(Name = "ApproverDesignation")]
        public string ApproverDesignation { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "SignaturePhotoID")]
        public string SignaturePhotoID { get; set; }

        [DataMember(Name = "SignaturePhotoName")]
        public string SignaturePhotoName { get; set; }

        [DataMember(Name = "ApproverTitle")]
        public string ApproverTitle { get; set; }

        [DataMember(Name = "LetterType")]
        public string LetterType { get; set; }

        [DataMember(Name = "DestinationUserId")]
        public List<LetterInboundDestinationUsersModel> DestinationUserId { get; set; }

        [DataMember(Name = "DestinationDepartmentId")]
        public List<LetterInboundDestinationDepartmentModel> DestinationDepartmentId { get; set; }
    }
}
