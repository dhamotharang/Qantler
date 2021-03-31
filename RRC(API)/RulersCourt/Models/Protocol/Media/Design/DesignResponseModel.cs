using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media.Design
{
    [DataContract]
    public class DesignResponseModel
    {
        [DataMember(Name = "DesignID")]
        public int? DesignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]        public string ReferenceNumber { get; set; }        [DataMember(Name = "status")]        public int Status { get; set; }        [DataMember(Name = "CreatorID")]        public int? CreatorID { get; set; }        [DataMember(Name = "FromID")]        public int? FromID { get; set; }        [DataMember(Name = "ApproverID")]        public int? ApproverID { get; set; }        [DataMember(Name = "ApproverDepartmentID")]        public int? ApproverDepartmentID { get; set; }        [DataMember(Name = "AssigneeID")]        public int? AssigneeID { get; set; }        [DataMember(Name = "MediaHeadUsedID")]        public List<UserModel> MediaHeadUsedID { get; set; }        [DataMember(Name = "Action")]        public string Action { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }
    }
}