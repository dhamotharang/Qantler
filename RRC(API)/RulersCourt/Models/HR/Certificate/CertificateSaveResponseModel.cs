using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Certificate
{
    [DataContract]
    public class CertificateSaveResponseModel
    {
        [DataMember(Name = "CertificateId")]
        public int? CertificateId { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
