using System.ComponentModel.DataAnnotations;

namespace CvGenerator.Models
{
    public class CvProject
    {
        [Required]
        public int Year { get; set; }
        [Required]
        public string Name { get; set; }
        public string Role { get; set; }
        [Required]
        public string Description { get; set; }
        public string Link { get; set; }

        [Display(Name = "Tech Stack")]
        public string TechStack { get; set; }
    }
}
