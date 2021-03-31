using RulersCourt.Models.DutyTasks;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    [DataContract]
    public class DutyTaskCountryAndEmiratesModel
    {
        [DataMember(Name = "Country")]
        public List<M_CountryModel> Country { get; set; }

        [DataMember(Name = "Emirates")]
        public List<M_EmiratesModel> Emirates { get; set; }

    }
}