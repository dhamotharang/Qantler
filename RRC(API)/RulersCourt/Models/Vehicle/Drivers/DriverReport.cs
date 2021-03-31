using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverReport
    {
        [DisplayName("تاريخ")]
        [DataMember(Name = "LogDate")]
        public DateTime? LogDate { get; set; }

        [DisplayName("تقرير الساعات الإضافية")]
        [DataMember(Name = "ExtraHours")]
        public float? ExtraHours { get; set; }

        [DisplayName("تعويض")]
        [DataMember(Name = "CompensateHours")]
        public float? CompensateHours { get; set; }

        [DisplayName("رصيد الساعات الإضافية")]
        [DataMember(Name = "BalanceExtraHours")]
        public float? BalanceExtraHours { get; set; }
    }
}
