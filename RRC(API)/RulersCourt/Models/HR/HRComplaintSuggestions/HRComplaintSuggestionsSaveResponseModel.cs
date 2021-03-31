using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HRComplaintSuggestions
{
    [DataContract]
    public class HRComplaintSuggestionsSaveResponseModel
    {
        [DataMember(Name = "HRComplaintSuggestionsID")]
        public int? HRComplaintSuggestionsID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}
