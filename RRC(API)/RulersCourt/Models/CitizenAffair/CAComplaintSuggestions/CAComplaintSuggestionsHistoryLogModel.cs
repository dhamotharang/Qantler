using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CAComplaintSuggestionsHistoryLogModel
    {
        [DataMember(Name = "HistoryID")]
        public int? HistoryID { get; set; }

        [DataMember(Name = "CAComplaintSuggestionsID")]
        public int? CAComplaintSuggestionsID { get; set; }

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
