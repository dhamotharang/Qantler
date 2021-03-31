using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_News
{
    public class M_NewslistModel
    {
        [DataMember(Name = "Collection")]
        public List<M_NewsModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
