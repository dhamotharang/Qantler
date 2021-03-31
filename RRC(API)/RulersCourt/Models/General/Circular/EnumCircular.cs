using System.ComponentModel.DataAnnotations;

namespace RulersCourt.Models.Circular
{
    public enum EnumCircular
    {
        [Display(Name = "My Pending Actions")]
        MyPendingActionsIncoming = 1,

        [Display(Name = "Outgoing Circulars")]
        OutgoingLetters = 2,

        [Display(Name = "Incoming Circulars")]
        IncomingLetters = 3,

        [Display(Name = "Draft Circulars")]
        DraftCircular = 4,

        [Display(Name = "Historical Circulars Incoming")]
        HistoricalLettersIncoming = 5,

        [Display(Name = "Historical Circulars Outgoing")]
        HistoricalLettersOutgoing = 6
    }
}
