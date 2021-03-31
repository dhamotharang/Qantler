using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.LeaveRequest
{
    [DataContract]
    public class LeaveSaveResponseModel
    {
        [DataMember(Name = "LeaveID")]
        public int? LeaveID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}
