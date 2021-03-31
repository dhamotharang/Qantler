using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.CarCompany
{
    [DataContract]
    public class CarCompanyPutModel
    {
        [DataMember(Name = "CarCompanyID")]
        public int? CarCompanyID { get; set; }

        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
