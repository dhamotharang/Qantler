using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class MaintenanceHomeDashboardListModel
    {
        [DataMember(Name = "MaintenanceID")]
        public int? MaintenanceID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "AttendedBy")]
        public string AttendedBy { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "AssignedTo")]
        public string AssignedTo { get; set; }
    }
}
