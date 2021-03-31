using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class TripSameDayPeriodModel
    {
        [DataMember(Name = "Requestor")]
        public string Requestor { get; set; }

        [DataMember(Name = "DriverName")]
        public string DriverName { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "CoPassenger")]
        public string CoPassenger { get; set; }
    }
}
