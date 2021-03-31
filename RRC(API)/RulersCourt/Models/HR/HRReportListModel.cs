using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR
{
    [DataContract]
    public class HRReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<HRReport> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
