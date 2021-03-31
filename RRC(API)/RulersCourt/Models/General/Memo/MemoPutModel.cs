using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class MemoPutModel
    {
        [DataMember(Name = "MemoID")]
        public int? MemoID { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "DestinationUserId")]
        public List<MemoDestinationUsersGetModel> DestinationUserId { get; set; }

        [DataMember(Name = "DestinationDepartmentId")]
        public List<MemoDestinationDepartmentGetModel> DestinationDepartmentId { get; set; }

        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int? ApproverDepartmentId { get; set; }

        [DataMember(Name = "DestinationRedirectedBy")]
        public int? DestinatoionRedirectedBy { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [DataMember(Name = "Private")]
        public string Private { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "Keywords")]
        public List<MemoKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "Attachments")]
        public List<AttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }
    }
}
