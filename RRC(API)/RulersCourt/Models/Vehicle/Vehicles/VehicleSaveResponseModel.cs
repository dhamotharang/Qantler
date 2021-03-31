using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehicleSaveResponseModel
    {
        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
