using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Training
{
    [DataContract]
    public class TrainingListModel
    {
        [DataMember(Name = "Collection")]
        public List<TrainingGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
