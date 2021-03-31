using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.CarCompany
{
    [DataContract]
    public class CarCompanyPostModel
    {
        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
