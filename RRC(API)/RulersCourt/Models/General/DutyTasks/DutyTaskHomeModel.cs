using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    [DataContract]
    public class DutyTaskHomeModel
    {
        [DataMember(Name = "MyTask")]
        public int? MyTask { get; set; }

        [DataMember(Name = "AssignedTask")]
        public int? AssignedTask { get; set; }

        [DataMember(Name = "TaskParticipations")]
        public int? TaskParticipations { get; set; }

        [DataMember(Name = "NoOfNewTask")]
        public int? NoOfNewTask { get; set; }

        [DataMember(Name = "TaskBTStartReminderDate")]
        public int? TaskBTStartReminderDate { get; set; }

        [DataMember(Name = "TaskBTReminderEndDate")]
        public int? TaskBTReminderEndDate { get; set; }

        [DataMember(Name = "TaskEndDateGtActualdate")]
        public int? TaskEndDateGtActualdate { get; set; }

        [DataMember(Name = "TaskInprogress")]
        public int? TaskInprogress { get; set; }

        [DataMember(Name = "TaskCompleted")]
        public int? TaskCompleted { get; set; }

        [DataMember(Name = "TaskClosed")]
        public int? TaskClosed { get; set; }
    }
}
