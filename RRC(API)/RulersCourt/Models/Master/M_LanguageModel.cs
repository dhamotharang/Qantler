using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_LanguageModel
    {
        [DataMember(Name = "LanguageID")]
        public int? LanguageID { get; set; }

        [DataMember(Name = "LanguageName")]
        public string LanguageName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
