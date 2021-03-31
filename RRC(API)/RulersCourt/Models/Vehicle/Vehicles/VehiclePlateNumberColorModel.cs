using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    public class VehiclePlateNumberColorModel
    {
        [DataMember(Name = "PlateNumber")]
        public List<string> PlateNumber { get; set; }

        [DataMember(Name = "PlateColor")]
        public List<string> PlateColor { get; set; }
    }
}
