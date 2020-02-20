using CvGenerator.Logic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvEmployment : IToHtml
    {
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }

        [Display(Name = "End Year")]
        public int? EndYear { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        public string Company { get; set; }

        [Display(Name = "Still working here")]
        public bool StillWorking { get; set; }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StartYear).Append(" - ");
            if (StillWorking)
                sb.Append("Present");
            else
                sb.Append(EndYear);
            sb.Append("<br/>").Append(HttpUtility.HtmlEncode(JobTitle)).Append("<br/>").Append(HttpUtility.HtmlEncode(Company));
            return sb.ToString();
        }
    }
}
