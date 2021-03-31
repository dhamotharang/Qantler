using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class ContactSaveResponseModel
    {
        [DataMember(Name = "ContactID")]
        public int? ContactID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
