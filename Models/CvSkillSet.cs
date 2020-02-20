using CvGenerator.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvGenerator.Models
{
    public class CvSkillSet : IToHtml
    {
        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Skills")]
        public string SkillsInAString { get; set; }

        public List<string> Skills
        {
            get
            {
                if (string.IsNullOrEmpty(SkillsInAString))
                    return null;
                return SkillsInAString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }
        }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h3 style=\"margin: 0px; \">").Append(CategoryName).Append(" Skills</h3>\n");
            sb.Append("<ul>");
            foreach (var item in Skills)
            {
                sb.Append("<li>").Append(item).Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
