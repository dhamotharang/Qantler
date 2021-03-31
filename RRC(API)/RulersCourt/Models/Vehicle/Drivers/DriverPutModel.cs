using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverPutModel
    {
        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }

        [DataMember(Name = "UserProfileID")]
        public int? UserProfileID { get; set; }

        [DataMember(Name = "LogDate")]
        public DateTime? LogDate { get; set; }

        [DataMember(Name = "ExtraHours")]
        public float? ExtraHours { get; set; }

        [DataMember(Name = "CompensateHours")]
        public float? CompensateHours { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
