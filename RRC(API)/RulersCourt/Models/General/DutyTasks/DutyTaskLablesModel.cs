using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    public class DutyTaskLablesModel
    {
        [DataMember(Name = "labels")]
        public string Labels { get; set; }
    }
}
