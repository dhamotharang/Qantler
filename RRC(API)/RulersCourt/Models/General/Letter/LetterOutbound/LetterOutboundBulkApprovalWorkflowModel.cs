using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundBulkApprovalWorkflowModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public List<LetterOutboundReferenceModel> ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "DestinationUserID")]
        public List<LetterOutboundDestinationUsersModel> DestinationUserID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }
    }
}
