using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    public class CitizenAffairDashboardListModel
    {
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "personalName")]
        public string PersonalName { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "AssignedTo ")]
        public string AssignedTo { get; set; }

        [DataMember(Name = "Reporter ")]
        public string Reporter { get; set; }
    }
}
