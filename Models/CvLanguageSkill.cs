using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvGenerator.Models
{
    public class CvLanguageSkill
    {
        public enum Proficiency
        {
            Elementary,
            LimitedWorking,
            ProfessionalWorking,
            FullProfessional,
            Native,
        }

        public string Name { get; set; }
        public Proficiency Level { get; set; }
    }
}
