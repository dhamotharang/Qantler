using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HRComplaintSuggestions
{
    [DataContract]
    public class HRComplaintSuggestionsWorkflowModel
    {
        [DataMember(Name = "HRComplaintSuggestionsID")]
        public int? HRComplaintSuggestionsID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int? CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int? FromID { get; set; }

        [DataMember(Name = "AssigneeId")]
        public int? AssigneeId { get; set; }

        [DataMember(Name = "HRHeadUsedId")]
        public List<UserModel> HRHeadUsedId { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "IsAnonymous")]
        public bool IsAnonymous { get; set; }
    }
}