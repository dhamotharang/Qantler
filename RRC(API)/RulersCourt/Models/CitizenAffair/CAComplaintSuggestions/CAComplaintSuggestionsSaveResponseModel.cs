using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CAComplaintSuggestionsSaveResponseModel
    {
        [DataMember(Name = "CAComplaintSuggestionsID")]
        public int? CAComplaintSuggestionsID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}
