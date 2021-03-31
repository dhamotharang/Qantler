using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MasterLookupsGetModel
    {
        [DataMember(Name = "LookupsID")]
        public int? LookupsID { get; set; }

        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "ArDisplayName")]
        public string ArDisplayName { get; set; }

        [DataMember(Name = "CountryID")]
        public int? CountryID { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public int? DisplayOrder { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "EmiratesID")]
        public int? EmiratesID { get; set; }
    }
}
