using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    public class ContactReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<InternalContactReport> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
