using System;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
	public class QrcMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameAr { get; set; }
    }

    public class CreateQrcMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(150, ErrorMessage = "Maximum length 150")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [MaxLength(2000, ErrorMessage = "Maximum length 2000")]
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CategoryIds { get; set; }
    }

    public class UpdateQrcMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(150, ErrorMessage = "Maximum length 150")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description Required")]
        [MaxLength(2000, ErrorMessage = "Maximum length 2000")]
        public string Description { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
        public string CategoryIds { get; set; }
    }

}
