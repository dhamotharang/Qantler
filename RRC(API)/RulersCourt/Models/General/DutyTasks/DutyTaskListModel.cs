using RulersCourt.Models.DutyTasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskListModel
    {
        [DataMember(Name = "Collection")]
        public List<DutyTaskDashboardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "Creator")]
        public List<DutyTaskAssigneeAndCreatorModel> Creator { get; set; }

        [DataMember(Name = "Assignee")]
        public List<DutyTaskAssigneeAndCreatorModel> Assignee { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }
    }
}
