using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleHomeDashboardModel
    {
        [DataMember(Name = "Vehicle")]
        public int? Vehicle { get; set; }

        [DataMember(Name = "Fine")]
        public int? Fine { get; set; }

        [DataMember(Name = "Driver")]
        public int? Driver { get; set; }

        [DataMember(Name = "OwnRequest")]
        public int? OwnRequest { get; set; }

        [DataMember(Name = "RentedCar")]
        public int? RentedCar { get; set; }

        [DataMember(Name = "MyProcessedRequest")]
        public int? MyProcessedRequest { get; set; }

        [DataMember(Name = "DriversOnTrip")]
        public int? DriversOnTrip { get; set; }

        [DataMember(Name = "DriversOffTrip")]
        public int? DriversOffTrip { get; set; }

        [DataMember(Name = "VehicleOnTrip")]
        public int? VehicleOnTrip { get; set; }

        [DataMember(Name = "VehicleOffTrip")]
        public int? VehicleOffTrip { get; set; }
    }
}
