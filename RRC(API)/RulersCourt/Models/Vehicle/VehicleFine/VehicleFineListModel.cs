using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFineListModel
    {
        [DataMember(Name = "Collection")]
        public List<VehicleFineGetList> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "Organizations")]
        public List<OrganizationModel> Organizations { get; set; }
    }
}
