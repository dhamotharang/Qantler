using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Maintenance
{
    [DataContract]
    public class MaintenanceReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<MaintenanceReportModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
