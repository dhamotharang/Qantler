using System;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
	public class CopyrightMaster
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
        public string Media { get; set; }
        public bool Protected { get; set; }
        public bool IsResourceProtect { get; set; }
		public int Weight { get; set; }
	}

    public class ShortCopyright
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public string Media { get; set; }
        public bool Protected { get; set; }
        public bool IsResourceProtect { get; set; }
	}
    public class CreateCopyrightMaster
    {
        [Required(ErrorMessage = "Title Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        [MaxLength(10000, ErrorMessage = "Maximum length 10000")]
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public int CreatedBy { get; set; }
        public string Media { get; set; }
        public bool Protected { get; set; }
        public bool IsResourceProtect { get; set; }
		public int Weight { get; set; }
	}

    public class UpdateCopyrightMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        [MaxLength(10000, ErrorMessage = "Maximum length 10000")]
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
        public string Media { get; set; }
        public bool Protected { get; set; }
        public bool IsResourceProtect { get; set; }
		public int Weight { get; set; }
	}

}
