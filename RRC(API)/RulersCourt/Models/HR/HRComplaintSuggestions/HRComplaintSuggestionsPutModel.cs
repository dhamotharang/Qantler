using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HRComplaintSuggestions
{
    [DataContract]
    public class HRComplaintSuggestionsPutModel
    {
        [DataMember(Name = "HRComplaintSuggestionsID")]
        public int? HRComplaintSuggestionsID { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [DataMember(Name = "RequestCreatedBy")]
        public string RequestCreatedBy { get; set; }

        [DataMember(Name = "MailID")]
        public string MailID { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "ActionTaken")]
        public string ActionTaken { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }
    }
}
