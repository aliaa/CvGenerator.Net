using System.ComponentModel.DataAnnotations;

namespace CvGenerator.Models
{
    public class CvProject
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }

        [Display(Name = "Tech Stack")]
        public string TechStack { get; set; }
    }
}
