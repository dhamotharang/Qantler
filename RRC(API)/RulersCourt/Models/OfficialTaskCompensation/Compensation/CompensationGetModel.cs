using RulersCourt.Models.OfficialTaskCompensation.Compensation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficalTaskCompensation.Compensation
{
    [DataContract]
    public class CompensationGetModel
    {
        [DataMember(Name = "CompensationID")]
        public int? CompensationID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "OfficialTaskID")]
        public int? OfficialTaskID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public string ApproverDepartmentID { get; set; }

        [DataMember(Name = "ApproverID")]
        public string ApproverID { get; set; }

        [DataMember(Name = "OfficialTaskType")]
        public string OfficialTaskType { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "NumberofDays")]
        public int? NumberofDays { get; set; }

        [DataMember(Name = "CompensationDescription")]
        public string CompensationDescription { get; set; }

        [DataMember(Name = "NeedCompensation")]
        public bool? NeedCompensation { get; set; }

        [DataMember(Name = "EmployeeNameID")]
        public List<CompensationEmployeeGetModel> EmployeeNameID { get; set; }

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "CompensationCommunicationHistory")]
        public List<CompensationCommunicationHistoryModel> CommunicationHistory { get; set; }
    }
}
