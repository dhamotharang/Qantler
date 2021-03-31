using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    public class CitizenAffairReportListModel
    {
        [DataContract]
        public class MaintenanceReportListModel
        {
            [DataMember(Name = "Collection")]
            public List<CitizenAffairReportModel> Collection { get; set; }

            [DataMember(Name = "Count")]
            public string Count { get; set; }
        }
    }
}
