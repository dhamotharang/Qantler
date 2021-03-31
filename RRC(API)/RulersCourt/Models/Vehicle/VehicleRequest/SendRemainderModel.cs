using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class SendRemainderModel
    {
        [DataMember(Name = "EmailTo")]
        public string EmailTo { get; set; }

        [DataMember(Name = "EmailCCDepartmentID")]
        public int? EmailCCDepartmentID { get; set; }

        [DataMember(Name = "EmailCCUserID")]
        public int? EmailCCUserID { get; set; }

        [DataMember(Name = "VehicleFineID")]
        public int VehicleFineID { get; set; }
    }
}
