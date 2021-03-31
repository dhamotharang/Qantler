using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.PressRelease
{
    [DataContract]
    public class PressReleaseGetModel
    {
        [DataMember(Name = "PressReleaseID")]
        public int? PressReleaseID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "EventName")]
        public string EventName { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "AttendedBy")]
        public string AttendedBy { get; set; }

        [DataMember(Name = "Partners")]
        public string Partners { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "AssigneeID")]
        public List<CurrentMediaAssigneeModel> AssigneeID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "MediaHeadUsedId")]
        public List<CurrentMediaHeadModel> MediaHeadUsedId { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "PressReleaseCommunicationHistory")]
        public List<PressReleaseCommunicationHistory> CommunicationHistory { get; set; }
    }
}
