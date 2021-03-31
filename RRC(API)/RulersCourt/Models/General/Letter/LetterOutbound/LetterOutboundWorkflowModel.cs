using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundWorkflowModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

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

        [DataMember(Name = "DestinationDepartmentID")]
        public List<LetterOutboundDestinationDepartmentModel> DestinationDepartmentID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }

        [DataMember(Name = "DestinationRedirectedBy")]
        public int? DestinationRedirectedBy { get; set; }

        [DataMember(Name = "DestinationDepartmentUserID")]
        public List<UserModel> DestinationDepartmentUserID { get; set; }
    }
}
