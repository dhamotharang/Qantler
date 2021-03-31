using RulersCourt.Models.Circular;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Circular
{
    [DataContract]
    public class CircularPreviewModel
    {
        [DataMember(Name = "CircularID")]
        public int? CircularID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "ApproverNameID")]
        public string ApproverName { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int? ApproverDepartmentId { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

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

        [DataMember(Name = "ApproverDesignation")]
        public string ApproverDesignation { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "DestinationDepartmentID")]
        public List<CircularDestinationDepartmentGetModel> DestinationDepartmentID { get; set; }

        [DataMember(Name = "Attachments")]
        public List<CircularAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<CircularHistoryLogModel> HistoryLog { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }
    }
}