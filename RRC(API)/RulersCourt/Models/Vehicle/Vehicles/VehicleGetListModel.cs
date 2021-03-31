using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle
{
    public class VehicleGetListModel
    {
        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "PlateColor")]
        public string PlateColor { get; set; }

        [DataMember(Name = "VehicleModel")]
        public string VehicleModel { get; set; }

        [DataMember(Name = "NextService")]
        public int? NextService { get; set; }

        [DataMember(Name = "TyreChange")]
        public int? TyreChange { get; set; }

        [DataMember(Name = "ContractEndDate")]
        public DateTime? ContractEndDate { get; set; }

        [DataMember(Name = "VehicleRegistrationExpiry")]
        public DateTime? VehicleRegistrationExpiry { get; set; }

        [DataMember(Name = "CreatedDepartment")]
        public string CreatedDepartment { get; set; }

        [DataMember(Name = "Nameofuser")]
        public string Nameofuser { get; set; }

        [DataMember(Name = "NameofDepartment")]
        public string NameofDepartment { get; set; }

        [DataMember(Name = "CurrentMileage")]
        public string CurrentMileage { get; set; }
    }
}
