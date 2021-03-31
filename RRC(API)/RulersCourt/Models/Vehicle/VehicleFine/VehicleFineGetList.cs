using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineGetList
    {
        [DataMember(Name = "Time")]
        public DateTime? Time { get; set; }

        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "IssuedAgainstDepartment")]
        public string IssuedAgainstDepartment { get; set; }

        [DataMember(Name = "IssuedAgainstName")]
        public string IssuedAgainstName { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "VehicleFineID")]
        public int? VehicleFineID { get; set; }

        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }

        [DataMember(Name = "DriverMailID")]
        public string DriverMailID { get; set; }
    }
}
