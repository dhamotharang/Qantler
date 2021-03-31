using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.CarCompany
{
    [DataContract]
    public class CarCompanyReport
    {
        [DisplayName("اسم الشركة")]
        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DisplayName("اسم جهة الاتصال")]
        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DisplayName("رقم الاتصال")]
        [DataMember(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DisplayName("تاريخ الإنشاء")]
        [DataMember(Name = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }
    }
}
