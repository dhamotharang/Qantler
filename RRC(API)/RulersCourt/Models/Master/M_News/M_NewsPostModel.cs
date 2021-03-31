using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_News
{
    [DataContract]
    public class M_NewsPostModel
    {
        [DataMember(Name = "News")]
        public string News { get; set; }

        [DataMember(Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
