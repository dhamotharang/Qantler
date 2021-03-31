using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundDashboardListModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "StatusCode")]
        public string StatusCode { get; set; }

        [DataMember(Name = "LinkedToOtherLetter")]
        public string LinkedToOtherLetter { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "Replied")]
        public string Replied { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "LetterType")]
        public string LetterType { get; set; }

        [DataMember(Name = "SenderName")]
        public string SenderName { get; set; }

        [DataMember(Name = "SenderEntity")]
        public string SenderEntity { get; set; }
    }
}
