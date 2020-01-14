using CvGenerator.Logic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvEmployment : IToHtml
    {
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }

        [Display(Name = "Still working here")]
        public bool StillWorking { get; set; }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StartYear).Append(" - ");
            if (StillWorking)
                sb.Append("Now");
            else
                sb.Append(EndYear);
            sb.Append("<br/>").Append(JobTitle).Append("<br/>").Append(Company);
            return sb.ToString();
        }
    }
}
