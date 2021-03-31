using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfileWorkExperienceModel
    {
        [DataMember(Name = "WorkExperienceID")]
        public int? WorkExperienceID { get; set; }

        [DataMember(Name = "JobTitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "TimePeriodFrom")]
        public DateTime? TimePeriodFrom { get; set; }

        [DataMember(Name = "TimePeriodTo")]
        public DateTime? TimePeriodTo { get; set; }
    }
}
