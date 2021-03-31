using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverGetTripDaysModel
    {
        [DataMember(Name = "TripPeriodFrom")]
        public DateTime? TripPeriodFrom { get; set; }

        [DataMember(Name = "TripPeriodTo")]
        public DateTime? TripPeriodTo { get; set; }

        [DataMember(Name = "With")]
        public string With { get; set; }

        [DataMember(Name = "TO")]
        public string TO { get; set; }
    }
}
