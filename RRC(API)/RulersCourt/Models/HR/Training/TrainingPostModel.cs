using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Training
{
    [DataContract]
    public class TrainingPostModel
    {
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "TrainingFor")]
        public bool TrainingFor { get; set; }

        [DataMember(Name = "TraineeName")]
        public string TraineeName { get; set; }

        [DataMember(Name = "TrainingName")]
        public string TrainingName { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "ApproverID")]
        public int ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int ApproverDepartmentID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }
    }
}
