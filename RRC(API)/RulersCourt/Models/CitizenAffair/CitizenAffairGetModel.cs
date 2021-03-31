using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenAffairGetModel
    {
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CurrentApproverID")]
        public int? CurrentApproverID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int? ApproverDepartmentId { get; set; }

        [DataMember(Name = "NotifyUpon")]
        public string NotifyUpon { get; set; }

        [DataMember(Name = "InternalRequestorID")]
        public int? InternalRequestorID { get; set; }

        [DataMember(Name = "InternalRequestorDepartmentID")]
        public int? InternalRequestorDepartmentID { get; set; }

        [DataMember(Name = "ExternalRequestEmailID")]
        public string ExternalRequestEmailID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Documents")]
        public List<CitizenAffairAttachmentGetModel> Documents { get; set; }

        [DataMember(Name = "Photos")]
        public List<CitizenAffairAttachmentGetModel> Photos { get; set; }

        [DataMember(Name = "PersonalReport")]
        public CitizenAffairPersonalReportModel PersonalReport { get; set; }

        [DataMember(Name = "FieldVisit")]
        public CitizenAffairFieldVisitModel FieldVisit { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<CitizenAffairHistoryLogModel> HistoryLog { get; set; }
    }
}
