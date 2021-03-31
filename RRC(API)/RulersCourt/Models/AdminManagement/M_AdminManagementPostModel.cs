using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.AdminManagement
{
    [DataContract]
    public class M_AdminManagementPostModel
    {
        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "Category")]
        public string Category { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
