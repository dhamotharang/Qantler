using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    public class LetterReportListModel
    {
        [DataMember(Name = "Collection")]
        public List<LetterReportModel> Collection { get; set; }
    }
}
