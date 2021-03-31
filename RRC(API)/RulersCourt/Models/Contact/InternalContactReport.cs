using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class InternalContactReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("المكتب / الإدارة")]
        [DataMember(Name = "Department")]
        public string Department { get; set; }

        [DisplayName("اسم المستخدم")]
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DisplayName("المسمى الوظيفي")]
        [DataMember(Name = "Designation")]
        public string Designation { get; set; }

        [DisplayName("البريد الالكتروني")]
        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DisplayName("رقم الهاتف")]
        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
