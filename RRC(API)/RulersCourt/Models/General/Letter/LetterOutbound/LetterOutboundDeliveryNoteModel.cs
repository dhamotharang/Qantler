using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Letter.LetterOutbound
{
    [DataContract]
    public class LetterOutboundDeliveryNoteModel
    {
        [DataMember(Name = "LettersID")]
        public List<LetterOutboundIDModel> LettersID { get; set; }
    }
}
