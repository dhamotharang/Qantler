using RulersCourt.Models.Vehicle.TripVehicleIssues;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineCarListModel
    {
        [DataMember(Name = "VehicleMake")]
        public string VehicleMake { get; set; }

        [DataMember(Name = "VehicleName")]
        public string VehicleName { get; set; }

        [DataMember(Name = "PlateColour")]
        public string PlateColour { get; set; }

        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "VehicleID")]
        public string VehicleID { get; set; }

        [DataMember(Name = "ModelName")]
        public string ModelName { get; set; }

        [DataMember(Name = "CurrentMileage")]
        public string CurrentMileage { get; set; }

        [DataMember(Name = "VehicleIssues")]
        public List<TripVehicleIssuesPostModel> VehicleIssues { get; set; }
    }
}
