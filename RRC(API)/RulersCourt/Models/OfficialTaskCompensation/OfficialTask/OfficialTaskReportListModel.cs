using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficialTaskCompensation.OfficialTask
{
    public class OfficialTaskReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<OfficialTaskReportModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
