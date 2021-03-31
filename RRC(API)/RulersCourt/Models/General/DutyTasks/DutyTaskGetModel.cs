using RulersCourt.Models.DutyTasks;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskGetModel
    {
        [DataMember(Name = "TaskID")]
        public int? TaskID { get; set; }

        [DataMember(Name = "TaskReferenceNumber")]
        public string TaskReferenceNumber { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "TaskDetails")]
        public string TaskDetails { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "RemindMeAt")]
        public DateTime? RemindMeAt { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "AssigneeUserId")]
        public int? AssigneeUserId { get; set; }

        [DataMember(Name = "AssigneeDepartmentId")]
        public int? AssigneeDepartmentId { get; set; }

        [DataMember(Name = "DelegateAssignee")]
        public int? DelegateAssignee { get; set; }

        [DataMember(Name = "Country")]
        public int? Country { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "Emirates")]
        public int? Emirates { get; set; }

        [DataMember(Name = "LinkToMemo")]
        public List<DutyTaskMemoReferenceNumberModel> LinkToMemo { get; set; }

        [DataMember(Name = "LinkToLetter")]
        public List<DutyTaskLetterReferenceNumberModel> LinkToLetter { get; set; }

        [DataMember(Name = "LinkToMeeting")]
        public List<DutyTaskMeetingReferenceNumberModel> LinkToMeeting { get; set; }

        [DataMember(Name = "Labels")]
        public List<DutyTaskLablesModel> Labels { get; set; }

        [DataMember(Name = "ResponsibleUserId")]
        public List<DutyTaskResponsibleUsersModel> ResponsibleUserId { get; set; }

        [DataMember(Name = "TagList")]
        public List<DutyTaskResponsibleUsersModel> TagList { get; set; }

        [DataMember(Name = "ResponsibleDepartmentId")]
        public List<DutyTaskResponsibleDepartmentModel> ResponsibleDepartmentId { get; set; }

        [DataMember(Name = "Attachments")]
        public List<DutyTaskAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "M_OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "communiationHistory")]
        public List<DutyTaskCommunicationHistoryGetModel> CommuniationHistory { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
