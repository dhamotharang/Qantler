using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MOMSaveResponseModel
    {
        [DataMember(Name = "MOMID")]
        public int? MOMID { get; set; }
    }
}
