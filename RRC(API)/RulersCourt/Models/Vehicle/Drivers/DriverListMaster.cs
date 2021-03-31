using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverListMaster
    {
        [DataMember(Name = "Collection")]
        public List<DriverGetMaster> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
