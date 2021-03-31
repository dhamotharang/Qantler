using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MailRemainderModel
    {
        [DataMember(Name = "MailRemainder")]
        public string MailRemainder { get; set; }
    }
}
