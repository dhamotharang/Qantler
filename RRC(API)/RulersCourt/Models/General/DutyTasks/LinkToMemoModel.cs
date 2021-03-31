using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class LinkToMemoModel
    {
        [DataMember(Name = "MemoReferenceNumber")]
        public string MemoReferenceNumber { get; set; }

        [DataMember(Name = "MemoID")]
        public string MemoID { get; set; }
    }
}