using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehicleDashboardListModel
    {
        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "PlateColour")]
        public string PlateColour { get; set; }

        [DataMember(Name = "DepartmentOffice")]
        public string DepartmentOffice { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "RequestorDepartment")]
        public string RequestorDepartment { get; set; }
    }
}
