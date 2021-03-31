using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.CarCompany
{
    [DataContract]
    public class CarCompanyGetModel
    {
        [DataMember(Name = "CarCompanyID")]
        public int? CarCompanyID { get; set; }

        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }
    }
}
