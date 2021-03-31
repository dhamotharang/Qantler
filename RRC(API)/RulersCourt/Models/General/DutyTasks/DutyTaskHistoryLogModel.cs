using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskHistoryLogModel
    {
        [DataMember(Name = "HistoryID")]
        public int? HistoryID { get; set; }

        [DataMember(Name = "TaskID")]
        public int? TaskID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "ActionBy")]
        public string ActionBy { get; set; }

        [DataMember(Name = "ActionDateTime")]
        public DateTime? ActionDateTime { get; set; }
    }
}
