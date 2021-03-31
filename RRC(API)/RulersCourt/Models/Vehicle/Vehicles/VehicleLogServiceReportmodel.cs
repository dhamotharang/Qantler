using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehicleLogServiceReportModel
    {
        [DisplayName("قراءة العداد الحالية")]
        [DataMember(Name = "CurrentMileage")]
        public int? CurrentMileage { get; set; }

        [DisplayName("الموعد التالي (العداد)")]
        [DataMember(Name = "NextMileage")]
        public int? NextMileage { get; set; }

        [DisplayName("النوع")]
        [DataMember(Name = "ServiceType")]
        public string ServiceType { get; set; }

        [DisplayName("تاريخ")]
        [DataMember(Name = "Date")]
        public string Date { get; set; }
    }
}
