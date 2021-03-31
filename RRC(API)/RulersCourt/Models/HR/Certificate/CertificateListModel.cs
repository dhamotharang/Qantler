using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Certificate
{
    public class CertificateListModel
    {
        [DataMember(Name = "Collection")]
        public List<CertificateGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
