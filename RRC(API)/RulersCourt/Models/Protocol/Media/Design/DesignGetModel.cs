using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Design
{
    [DataContract]
    public class DesignGetModel
    {
        [DataMember(Name = "DesignId")]
        public int? DesignId { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "InitiativeProjectActivity")]
        public string InitiativeProjectActivity { get; set; }

        [DataMember(Name = "DiwansRole")]
        public string DiwansRole { get; set; }

        [DataMember(Name = "OtherParties")]
        public string OtherParties { get; set; }

        [DataMember(Name = "TargetGroup")]
        public string TargetGroup { get; set; }

        [DataMember(Name = "DeliverableDate")]
        public DateTime? DeliverableDate { get; set; }

        [DataMember(Name = "TypeofDesignRequired")]
        public string TypeofDesignRequired { get; set; }

        [DataMember(Name = "Languages")]
        public int? Languages { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "GeneralObjective")]
        public string GeneralObjective { get; set; }

        [DataMember(Name = "MainObjective")]
        public string MainObjective { get; set; }

        [DataMember(Name = "StrategicObjective")]
        public string StrategicObjective { get; set; }

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

        [DataMember(Name = "AssigneeID")]
        public List<CurrentMediaAssigneeModel> AssigneeID { get; set; }

        [DataMember(Name = "MediaHeadUsedId")]
        public List<CurrentMediaHeadModel> MediaHeadUsedId { get; set; }

        [DataMember(Name = "Attachments")]
        public List<DesignAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "DesignCommunicationHistory")]
        public List<DesignCommunicationHistory> CommunicationHistory { get; set; }
    }
}
