using System;

namespace OERService.Models
{
	public class Announcements
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Text_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? Active { get; set; }
        public DateTime LastLogin { get; set; }
        public Int64 TotalRows { get; set; }
    }
    public class AnnouncementsCreate
    {
        public string Text { get; set; }
        public string Text_Ar { get; set; }
        public int CreatedBy { get; set; }
        public bool? Active { get; set; }
    }
    public class AnnouncementsUpdate
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Text_Ar { get; set; }
        public int UpdatedBy { get; set; }
        public bool? Active { get; set; }
    }
}
