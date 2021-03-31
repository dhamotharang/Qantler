using System.ComponentModel.DataAnnotations;

namespace RulersCourt
{
    /// <summary>
    /// This is the enum class for access control purpose.
    /// </summary>
    public enum AccessControlEnum
    {
        [Display(Name = "Memo")]
        Memo = 1,

        [Display(Name = "InboundLetter")]
        InboundLetter = 2,

        [Display(Name = "OutboundLetter")]
        OutboundLetter = 3,
    }
}
