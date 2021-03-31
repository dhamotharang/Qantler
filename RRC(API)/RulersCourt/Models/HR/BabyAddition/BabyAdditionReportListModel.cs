using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.BabyAddition
{
    public class BabyAdditionReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<BabyAdditionReportModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
