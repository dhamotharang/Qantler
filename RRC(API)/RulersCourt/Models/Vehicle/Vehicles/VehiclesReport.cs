using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehiclesReport
    {
        [DisplayName("رقم اللوحة")]
        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DisplayName("لون اللوحة")]
        [DataMember(Name = "PlateColor")]
        public string PlateColor { get; set; }

        [DisplayName("موديل المركبة")]
        [DataMember(Name = "VehicleModel")]
        public string VehicleModel { get; set; }

        [DisplayName("تاريخ انتهاء العقد")]
        [DataMember(Name = "ContractEndDate")]
        public string ContractEndDate { get; set; }

        [DisplayName("انتهاء صلاحية ملكية المركبات")]
        [DataMember(Name = "VehicleRegistrationExpiry")]
        public string VehicleRegistrationExpiry { get; set; }

        [DisplayName("موعد خدمة الصيانة التالي")]
        [DataMember(Name = "NextService")]
        public int? NextService { get; set; }

        [DisplayName("تغيير الاطارات")]
        [DataMember(Name = "TyreChange")]
        public int? TyreChange { get; set; }
    }
}
