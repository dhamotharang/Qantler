using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Training
{
    [DataContract]
    public class TrainingSaveResponseModel
    {
        [DataMember(Name = "TrainingID")]
        public int? TrainingID { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}
