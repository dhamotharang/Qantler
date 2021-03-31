using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class DocumentListModel
    {
        [DataMember(Name = "Collection")]
        public List<DocumentGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
