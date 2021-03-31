using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenAffairWorkflowModel
    {
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime RequestDate { get; set; }

        [DataMember(Name = "PersonalLocationName")]
        public string PersonalLocationName { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "NotifyUpon")]
        public string NotifyUpon { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }

        [DataMember(Name = "InternalRequestorID")]
        public int? InternalRequestorID { get; set; }

        [DataMember(Name = "ExternalRequestEmailID")]
        public string ExternalRequestEmailID { get; set; }

        [DataMember(Name = "CitizenAffairHeadUserID")]
        public List<UserModel> CitizenAffairHeadUserID { get; set; }
    }
}
