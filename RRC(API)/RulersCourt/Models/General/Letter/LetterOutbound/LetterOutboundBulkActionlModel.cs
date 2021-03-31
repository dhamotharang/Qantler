using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundBulkActionlModel
    {
        [DataMember(Name = "LettersID")]
        public List<LetterOutboundIDModel> LettersID { get; set; }

        [DataMember(Name = "ActionBy")]
        public string ActionBy { get; set; }

        [DataMember(Name = "ActionDateTime")]
        public DateTime ActionDateTime { get; set; }
    }
}
