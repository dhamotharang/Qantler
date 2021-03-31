using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskDashboardListModel
    {
        [DataMember(Name = "TaskID")]
        public int? TaskID { get; set; }

        [DataMember(Name = "TaskReferenceNumber")]
        public string TaskReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "Assignee")]
        public string Assignee { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "CreationDate")]
        public DateTime? CreationDate { get; set; }

        [DataMember(Name = "DueDate")]
        public DateTime? DueDate { get; set; }

        [DataMember(Name = "LastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
