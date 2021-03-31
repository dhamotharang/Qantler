using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    public class ReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<Report> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
