using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Photos
{
    [DataContract]
    public class M_PhotoListModel
    {
        [DataMember(Name = "Collection")]
        public List<M_PhotoModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
