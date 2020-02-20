using CvGenerator.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvEducation : IToHtml
    {
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }

        [Display(Name = "End Year")]
        public int? EndYear { get; set; }
        public string Title { get; set; }
        public string University { get; set; }

        [Display(Name = "Still Studying Here")]
        public bool StillStudying { get; set; }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StartYear).Append(" - ");
            if (StillStudying)
                sb.Append("Present");
            else
                sb.Append(EndYear);
            sb.Append("<br/>").Append(HttpUtility.HtmlEncode(Title)).Append("<br/>").Append(University);
            return sb.ToString();
        }
    }
}
