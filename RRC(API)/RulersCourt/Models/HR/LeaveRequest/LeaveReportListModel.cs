using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.LeaveRequest
{
    public class LeaveReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<LeaveReport> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
