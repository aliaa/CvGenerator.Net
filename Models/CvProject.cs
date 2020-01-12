using System;
using System.Collections.Generic;
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
        public string TechStack { get; set; }
    }
}
