using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Certificate
{
    [DataContract]
    public class CertificatePutModel
    {
        [DataMember(Name = "CertificateID")]
        public int? CertificateID { get; set; }

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

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }
    }
}
