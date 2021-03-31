using System.Runtime.Serialization;

namespace RulersCourt.Models.CVBank
{
    [DataContract]
    public class CVBankSaveResponseModel
    {
        [DataMember(Name = "CVBankId")]
        public int? CVBankId { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
