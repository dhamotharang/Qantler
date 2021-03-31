using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.BabyAddition
{
    [DataContract]
    public class BabyAdditionPostModel
    {
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "BabyName")]
        public string BabyName { get; set; }

        [DataMember(Name = "Gender")]
        public string Gender { get; set; }

        [DataMember(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [DataMember(Name = "HospitalName")]
        public string HospitalName { get; set; }

        [DataMember(Name = "CountryOfBirth")]
        public string CountryOfBirth { get; set; }

        [DataMember(Name = "CityOfBirth")]
        public string CityOfBirth { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Attachments")]
        public List<BabyAdditionAttachmentGetModel> Attachments { get; set; }
    }
}
