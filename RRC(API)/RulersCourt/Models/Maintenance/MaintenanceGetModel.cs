using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Maintenance
{
    [DataContract]
    public class MaintenanceGetModel
    {
        [DataMember(Name = "MaintenanceID")]
        public int? MaintenanceID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "RequestorID")]
        public int? RequestorID { get; set; }

        [DataMember(Name = "RequestorDepartmentID")]
        public int? RequestorDepartmentID { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "RequestDetails")]
        public string RequestDetails { get; set; }

        [DataMember(Name = "Priority")]
        public bool? Priority { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "MaintenanceManagerUserID")]
        public int? MaintenanceManagerUserID { get; set; }

        [DataMember(Name = "Attachments")]
        public List<MaintenanceAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "MaintenanceCommunicationHistory")]
        public List<MaintenanceCommunicationHistory> CommunicationHistory { get; set; }
    }
}
