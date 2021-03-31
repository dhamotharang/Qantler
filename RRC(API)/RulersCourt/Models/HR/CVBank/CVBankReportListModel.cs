using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    public class CVBankReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<CVBankReport> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
