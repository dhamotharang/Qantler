using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverBlinding
    {
        [DataMember(Name = "DriverID")]
        public List<int?> DriverID { get; set; }
    }
}
