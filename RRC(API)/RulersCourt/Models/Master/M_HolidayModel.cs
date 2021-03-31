using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_HolidayModel
    {
        [DataMember(Name = "HolidayID")]
        public int? HolidayID { get; set; }

        [DataMember(Name = "Holiday")]
        public DateTime? Holiday { get; set; }

        [DataMember(Name = "Message")]
        public string Message { get; set; }
    }
}
