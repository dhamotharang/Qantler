using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestReportModel
    {
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "Requestor")]
        public string Requestor { get; set; }

        [DataMember(Name = "TripDateFrom")]
        public DateTime? TripDateFrom { get; set; }

        [DataMember(Name = "TripDateTo")]
        public DateTime? TripDateTo { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "RequestorOfficeDepartment")]
        public string RequestorOfficeDepartment { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public int UserID { get; set; }
    }
}
