using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Legal
{
    public class LegalReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<LegalReport> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
