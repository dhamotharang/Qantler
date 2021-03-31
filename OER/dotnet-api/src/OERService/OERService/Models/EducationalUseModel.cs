using System;

namespace OERService.Models
{
	public class EducationalUseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
    public class EducationalUseCreate
    {
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string CreatedBy { get; set; }
		public int Weight { get; set; }
	}
    public class EducationalUseUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
}
