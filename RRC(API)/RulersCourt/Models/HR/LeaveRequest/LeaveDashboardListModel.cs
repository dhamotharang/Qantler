using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.LeaveRequest
{
    [DataContract]
    public class LeaveDashboardListModel
    {
        [DataMember(Name = "LeaveID")]
        public int? LeaveID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }
    }
}
