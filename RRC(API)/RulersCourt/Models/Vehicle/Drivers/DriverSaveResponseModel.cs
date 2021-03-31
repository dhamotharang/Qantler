using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverSaveResponseModel
    {
        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }
    }
}
