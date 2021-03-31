using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class ExternalContactReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("اسم الجهة")]
        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DisplayName("اسم جهة الاتصال")]
        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DisplayName("جهة حكومية")]
        [DataMember(Name = "GoverenmentEntity")]
        public string GoverenmentEntity { get; set; }

        [DisplayName("البريد الالكتروني")]
        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DisplayName("رقم الهاتف")]
        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
