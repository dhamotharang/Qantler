using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfileEducationModel
    {
        [DataMember(Name = "UserProfileEducationID")]
        public int? UserProfileEducationID { get; set; }

        [DataMember(Name = "Degree")]
        public string Degree { get; set; }

        [DataMember(Name = "SchoolCollege")]
        public string SchoolCollege { get; set; }

        [DataMember(Name = "FieldStudy")]
        public string FieldStudy { get; set; }

        [DataMember(Name = "TimePeriodFrom")]
        public DateTime? TimePeriodFrom { get; set; }

        [DataMember(Name = "TimePeriodTo")]
        public DateTime? TimePeriodTo { get; set; }
    }
}
