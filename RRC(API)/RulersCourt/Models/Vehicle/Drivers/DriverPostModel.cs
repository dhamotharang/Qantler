using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverPostModel
    {
        [DataMember(Name = "UserProfileID")]
        public int? UserProfileID { get; set; }

        [DataMember(Name = "LogDate")]
        public DateTime? LogDate { get; set; }

        [DataMember(Name = "ExtraHours")]
        public float ExtraHours { get; set; }

        [DataMember(Name = "CompensateHours")]
        public float CompensateHours { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
