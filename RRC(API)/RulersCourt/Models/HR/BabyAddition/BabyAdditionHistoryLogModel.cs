using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.BabyAddition
{
    [DataContract]
    public class BabyAdditionHistoryLogModel
    {
        [DataMember(Name = "HistoryID")]
        public int? HistoryID { get; set; }

        [DataMember(Name = "BabyAdditionID")]
        public int? BabyAdditionID { get; set; }

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
