using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.TripCoPassengers
{
    [DataContract]
    public class TripCoPassengersModel
    {
        [DataMember(Name = "CoPassengerID")]
        public int? CoPassengerID { get; set; }

        [DataMember(Name = "CoPassengerDepartmentID")]
        public int? CoPassengerDepartmentID { get; set; }

        [DataMember(Name = "CoPassengerDepartment")]
        public string CoPassengerDepartment { get; set; }

        [DataMember(Name = "CoPassengerName")]
        public string CoPassengerName { get; set; }

        [DataMember(Name = "OthersCoPassengerName")]
        public string OtherCoPassengerName { get; set; }
    }
}
