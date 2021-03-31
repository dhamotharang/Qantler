using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CAComplaintSuggestionsDashboardListModel
    {
        [DataMember(Name = "CAComplaintSuggestionsID")]
        public int? CAComplaintSuggestionsID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }
    }
}
