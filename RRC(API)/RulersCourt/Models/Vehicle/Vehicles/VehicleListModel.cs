using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    public class VehicleListModel
    {
        [DataMember(Name = "Collection")]
        public List<VehicleGetListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
