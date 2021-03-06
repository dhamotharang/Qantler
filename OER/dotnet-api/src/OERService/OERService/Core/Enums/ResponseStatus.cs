using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum ResponseStatus
    {
        [EnumMember(Value = "OK")]
        [Description("OK")]
        OK = 200,

        [EnumMember(Value = "Bad Request")]
        [Description("Bad Request")]
        BadRequest = 400,

        [EnumMember(Value = "Forbidden")]
        [Description("Forbidden")]
        Forbidden = 403,
        

        [EnumMember(Value = "Server Error")]
        [Description("Internal Server Error")]
        ServerError = 500,
    }
}
