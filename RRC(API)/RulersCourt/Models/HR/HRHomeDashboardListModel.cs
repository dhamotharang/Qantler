using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class HRHomeDashboardListModel
    {
        [DataMember(Name = "ID")]
        public int? ID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "CreationDate")]
        public DateTime? CreationDate { get; set; }

        [DataMember(Name = "IsCompensationAvailable")]
        public bool? IsCompensationAvailable { get; set; }

        [DataMember(Name = "AssignedTo")]
        public string AssignedTo { get; set; }

        [DataMember(Name = "IsCompensationRequest")]
        public bool? IsCompensationRequest { get; set; }
    }
}
