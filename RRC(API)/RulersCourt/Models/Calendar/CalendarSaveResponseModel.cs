using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarSaveResponseModel
    {
        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "ReferenceNumber")]        public string ReferenceNumber { get; set; }        [DataMember(Name = "status")]        public int Status { get; set; }
    }
}
