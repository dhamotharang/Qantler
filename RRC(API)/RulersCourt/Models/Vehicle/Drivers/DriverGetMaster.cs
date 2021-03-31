using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverGetMaster
    {
        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }

        [DataMember(Name = "DriverName")]
        public string DriverName { get; set; }
    }
}
