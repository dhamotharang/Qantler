using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.AdminManagement
{
    [DataContract]
    public class M_AdminManagementPutModel
    {
        [DataMember(Name = "LookupsID")]
        public int? LookupsID { get; set; }

        [DataMember(Name = "Category")]
        public string Category { get; set; }

        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
