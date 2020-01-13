using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
