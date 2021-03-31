using System.ComponentModel;
using System.Runtime.Serialization;

namespace OERService.Core.Enums
{
    public enum ReportReason
    {
        [EnumMember(Value = "Spam")]
        [Description("Spam")]
        Spam = 1,

        [EnumMember(Value = "Offensive")]
        [Description("Offensive")]
        Offensive = 2,

        [EnumMember(Value = "Misleading")]
        [Description("Misleading")]
        Misleading = 3,

        [EnumMember(Value = "Other")]
        [Description("Other")]
        Other = 4,
    }
}
