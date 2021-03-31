using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskReportRequestModel
    {
        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "Assignee")]
        public string Assignee { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "Lable")]
        public string Lable { get; set; }

        [DataMember(Name = "LinkTo")]
        public string LinkTo { get; set; }

        [DataMember(Name = "DueDateFrom")]
        public DateTime? DueDateFrom { get; set; }

        [DataMember(Name = "DueDateTo")]
        public DateTime? DueDateTo { get; set; }

        [DataMember(Name = "CreationDateFrom")]
        public DateTime? CreationDateFrom { get; set; }

        [DataMember(Name = "CreationDateTo")]
        public DateTime? CreationDateTo { get; set; }

        [DataMember(Name = "Participants")]
        public string Participants { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }
    }
}
