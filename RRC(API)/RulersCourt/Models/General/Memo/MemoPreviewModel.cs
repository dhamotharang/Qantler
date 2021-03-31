using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Memo
{
    [DataContract]
    public class MemoPreviewModel
    {
        [DataMember(Name = "MemoID")]
        public int? MemoID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "DestinationTitle")]
        public string DestinationTitle { get; set; }

        [DataMember(Name = "ApproverNameID")]
        public string ApproverNameID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public string ApproverDepartmentID { get; set; }

        [DataMember(Name = "ApproverDesignation")]
        public string ApproverDesignation { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [DataMember(Name = "Private")]
        public string Private { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "SignaturePhotoApprover")]
        public string SignaturePhotoApprover { get; set; }

        [DataMember(Name = "RedirectNameID")]
        public string RedirectNameID { get; set; }

        [DataMember(Name = "RedirectID")]
        public int? RedirectID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "RedirectReferenceNumber")]
        public string RedirectReferenceNumber { get; set; }

        [DataMember(Name = "SignaturePhotoRedirector")]
        public string SignaturePhotoRedirector { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DestinationUsernameID")]
        public List<MemoDestinationUsersGetModel> DestinationUsernameID { get; set; }

        [DataMember(Name = "DestinationDepartmentID")]
        public List<MemoDestinationDepartmentGetModel> DestinationDepartmentID { get; set; }

        [DataMember(Name = "Keywords")]
        public List<MemoKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "Attachments")]
        public List<AttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "M_KeywordsList")]
        public List<MemoKeywordsModel> M_KeywordsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<MemoHistoryLogModel> HistoryLog { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }
    }
}