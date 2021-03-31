using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class LegalKeywordsModel
    {
        [DataMember(Name = "Keywords")]
        public string Keywords { get; set; }
    }
}
