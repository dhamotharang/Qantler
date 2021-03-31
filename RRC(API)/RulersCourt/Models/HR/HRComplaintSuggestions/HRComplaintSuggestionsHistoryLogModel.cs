using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HRComplaintSuggestions
{
    [DataContract]
    public class HRComplaintSuggestionsHistoryLogModel
    {
        [DataMember(Name = "HistoryID")]
        public int? HistoryID { get; set; }

        [DataMember(Name = "HRComplaintSuggestionsID")]
        public int? HRComplaintSuggestionsID { get; set; }

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
