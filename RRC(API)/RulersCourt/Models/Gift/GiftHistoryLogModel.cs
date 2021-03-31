using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class GiftHistoryLogModel
    {
        [DataMember(Name = "HistoryID")]
        public int? HistoryID { get; set; }

        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
