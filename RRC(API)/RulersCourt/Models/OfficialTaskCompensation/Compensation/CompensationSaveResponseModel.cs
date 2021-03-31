using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficalTaskCompensation.Compensation
{
    [DataContract]
    public class CompensationSaveResponseModel
    {
        [DataMember(Name = "CompensationID")]
        public int? CompensationID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}
