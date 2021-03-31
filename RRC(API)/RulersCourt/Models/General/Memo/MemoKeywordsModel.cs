using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    public class MemoKeywordsModel
    {
        [DataMember(Name = "Keywords")]
        public string Keywords { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }
    }
}
