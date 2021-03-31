using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineSaveResponseModel
    {
        [DataMember(Name = "VehicleFineID")]
        public int? VehicleFineID { get; set; }
    }
}
