using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public enum EnumLetter
    {
        [Display(Name = "My Pending Actions Incoming")]
        MyPendingActionsIncoming = 1,

        [Display(Name = "My Pending Actions Outgoing")]
        MyPendingActionsOutgoing = 2,

        [Display(Name = "Outgoing Letters")]
        OutgoingLetters = 3,

        [Display(Name = "Incoming Letters")]
        IncomingLetters = 4,

        [Display(Name = "Draft Letters")]
        DraftMemos = 5,

        [Display(Name = "Historical Letters Incoming")]
        HistoricalLettersIncoming = 6,

        [Display(Name = "Historical Letters Outgoing")]
        HistoricalLettersOutgoing = 7
    }
}
