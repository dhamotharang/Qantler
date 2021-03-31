using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Certificate
{
    [DataContract]
    public class CertificatePostModel
    {
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Attention")]
        public string Attention { get; set; }

        [DataMember(Name = "To")]
        public string To { get; set; }

        [DataMember(Name = "SalaryCertificateClassification")]
        public string SalaryCertificateClassification { get; set; }

        [DataMember(Name = "Reason")]
        public string Reason { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "CertificateType")]
        public string CertificateType { get; set; }
    }
}
