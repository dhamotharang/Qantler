using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverTrips
    {
        [DataMember(Name = "TripTimeFrom")]
        public DateTime? TripTimeFrom { get; set; }

        [DataMember(Name = "TripTimeTo")]
        public DateTime? TripTimeTo { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "Requestor")]
        public string Requestor { get; set; }
    }
}
