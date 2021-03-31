using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Certificate
{
    [DataContract]
    public class CertificateGetModel
    {
        [DataMember(Name = "CertificateID")]
        public int? CertificateID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

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

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "CertificateType")]
        public string CertificateType { get; set; }

        [DataMember(Name = "AssigneeId")]
        public List<CurrentAssigneeModel> AssigneeId { get; set; }

        [DataMember(Name = "HRHeadUsedId")]
        public List<CurrentHRHeadModel> HRHeadUsedId { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<CertificateHistoryLogModel> HistoryLog { get; set; }
    }
}
