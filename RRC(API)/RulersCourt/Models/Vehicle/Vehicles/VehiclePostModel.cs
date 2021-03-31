using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehiclePostModel
    {
        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "PlateColor")]
        public string PlateColor { get; set; }

        [DataMember(Name = "VehicleMake")]
        public string VehicleMake { get; set; }

        [DataMember(Name = "VehicleName")]
        public string VehicleName { get; set; }

        [DataMember(Name = "VehicleModel")]
        public string VehicleModel { get; set; }

        [DataMember(Name = "YearofManufacture")]
        public int? YearofManufacture { get; set; }

        [DataMember(Name = "CarCompanyID")]
        public int? CarCompanyID { get; set; }

        [DataMember(Name = "ContractNumber")]
        public string ContractNumber { get; set; }

        [DataMember(Name = "ContractDuration")]
        public int? ContractDuration { get; set; }

        [DataMember(Name = "ContractStartDate")]
        public DateTime? ContractStartDate { get; set; }

        [DataMember(Name = "ContractEndDate")]
        public DateTime? ContractEndDate { get; set; }

        [DataMember(Name = "VehicleRegistrationNumber")]
        public string VehicleRegistrationNumber { get; set; }

        [DataMember(Name = "VehicleRegistrationExpiry")]
        public DateTime? VehicleRegistrationExpiry { get; set; }

        [DataMember(Name = "NextService")]
        public int? NextService { get; set; }

        [DataMember(Name = "TyreChange")]
        public int? TyreChange { get; set; }

        [DataMember(Name = "Notes")]
        public string Notes { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "IsAlternativeVehicle")]
        public bool? IsAlternativeVehicle { get; set; }
    }
}
