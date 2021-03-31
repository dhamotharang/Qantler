using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestDashboardListModel
    {
        [DataMember(Name = "VehicleReqID")]
        public int? VehicleReqID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "StatusCode")]
        public int? StatusCode { get; set; }

        [DataMember(Name = "RequestorName")]
        public string RequestorName { get; set; }

        [DataMember(Name = "Requestor")]
        public int? Requestor { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestorDepartment")]
        public string RequestorDepartment { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "TripDateFrom")]
        public DateTime? TripDateFrom { get; set; }

        [DataMember(Name = "TripDateTo")]
        public DateTime? TripDateTo { get; set; }

        [DataMember(Name = "TripTimeFrom")]
        public string TripTimeFrom { get; set; }

        [DataMember(Name = "TripTimeTo")]
        public string TripTimeTo { get; set; }

        [DataMember(Name = "createdby")]
        public int? Createdby { get; set; }
    }
}
