using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_News
{
    [DataContract]
    public class M_NewsPutModel
    {
        [DataMember(Name = "NewsID")]
        public int? NewsID { get; set; }

        [DataMember(Name = "News")]
        public string News { get; set; }

        [DataMember(Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
