using RulersCourt.Models.HR.Training;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Training
{
    [DataContract]
    public class TrainingGetModel
    {
        [DataMember(Name = "TrainingID")]
        public int? TrainingID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "TrainingFor")]
        public bool? TrainingFor { get; set; }

        [DataMember(Name = "TraineeName")]
        public string TraineeName { get; set; }

        [DataMember(Name = "TraineeDepartmentID")]
        public int? TraineeDepartmentID { get; set; }

        [DataMember(Name = "TrainingName")]
        public string TrainingName { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }

        [DataMember(Name = "HRTeamUserID")]
        public int? HRTeamUserID { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "TrainingCommunicationHistory")]
        public List<TrainingCommunicationHistory> CommunicationHistory { get; set; }

        [DataMember(Name = "IsNotificationReceived")]
        public bool? IsNotificationReceived { get; set; }

        [DataMember(Name = "Attachments")]
        public List<TrainingAttachmentModel> Attachments { get; set; }
    }
}
