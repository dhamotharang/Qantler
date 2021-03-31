using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class HomeListModel
    {
        [DataMember(Name = "Collection")]
        public List<HomeModel> Collection { get; set; }
    }
}
