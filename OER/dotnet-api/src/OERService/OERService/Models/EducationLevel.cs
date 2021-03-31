using System;

namespace OERService.Models
{
	public class EducationLevel
    {
        public int? Id { get; set; }
        public string Level { get; set; }
        public string Level_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
    public class EducationLevelCreate
    {
        public string Level { get; set; }
        public string Level_Ar { get; set; }
        public int CreatedBy { get; set; }
		public int Weight { get; set; }
	}
    public class EducationLevelUpdate
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Level_Ar { get; set; }
        public int UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
}
