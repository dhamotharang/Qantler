using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_ApproverConfigurationModel
    {
        [DataMember(Name = "ApproverID")]
        public List<int?> ApproverID { get; set; }
    }
}
