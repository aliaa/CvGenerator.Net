using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CvGenerator.Models
{
    public class CvLanguageSkill
    {
        public enum Proficiency
        {
            Elementary,
            [Display(Name = "Limited Working")]
            LimitedWorking,
            [Display(Name = "Professional Working")]
            ProfessionalWorking,
            [Display(Name = "Full Professional")]
            FullProfessional,
            Native,
        }

        public string Name { get; set; }
        public Proficiency Level { get; set; }
    }
}
