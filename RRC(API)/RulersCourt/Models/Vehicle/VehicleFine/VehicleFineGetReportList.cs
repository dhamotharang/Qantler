using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineGetReportList
    {
        [DisplayName("رقم اللوحة")]
        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DisplayName("تسجيل مخالفة مرورية على مكتب / إدارة")]
        [DataMember(Name = "IssuedAgainstDepartment")]
        public string IssuedAgainstDepartment { get; set; }

        [DisplayName("مسجلة ضد - اسم الموظف")]
        [DataMember(Name = "IssuedAgainstName")]
        public string IssuedAgainstName { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("الوقت")]
        [DataMember(Name = "Time")]
        public string Time { get; set; }
    }
}
