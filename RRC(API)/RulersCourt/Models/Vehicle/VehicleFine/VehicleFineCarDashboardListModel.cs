using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineCarDashboardListModel
    {
        [DataMember(Name = "Collection")]
        public List<VehicleFineCarListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
