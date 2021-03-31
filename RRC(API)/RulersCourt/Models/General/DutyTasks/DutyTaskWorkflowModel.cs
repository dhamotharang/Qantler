using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskWorkflowModel
    {
        [DataMember(Name = "TaskID")]
        public int? TaskID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "AssigneeUserId")]
        public int? AssigneeUserId { get; set; }

        [DataMember(Name = "ResponsibleUserId")]
        public List<DutyTaskResponsibleUsersModel> ResponsibleUserId { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousAssigneeID")]
        public int? PreviousAssigneeID { get; set; }
    }
}
