using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestAutoConfirmModel
    {
        [DataMember(Name = "VehicleReqID")]
        public int? VehicleReqID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
