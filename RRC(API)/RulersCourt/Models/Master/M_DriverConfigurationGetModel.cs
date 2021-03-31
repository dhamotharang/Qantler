using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master
{
    [DataContract]
    public class M_DriverConfigurationGetModel
    {
        [DataMember(Name = "DriverID")]
        public List<int?> DriverID { get; set; }
    }
}
