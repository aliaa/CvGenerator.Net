using CvGenerator.Logic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CvGenerator.Models
{
    public class CvLanguageSkill : IToHtml
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

        public string ToHtml()
        {
            return HttpUtility.HtmlEncode(Name) + " : " + EnumHelper<Proficiency>.GetDisplayName(Level.ToString());
        }
    }
}
