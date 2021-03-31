using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehicleLogServicePost
    {
        [DataMember(Name = "CurrentMileage")]
        public int? CurrentMileage { get; set; }

        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "NextMileage")]
        public int? NextMileage { get; set; }

        [DataMember(Name = "LogType")]
        public int? LogType { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }
    }
}
