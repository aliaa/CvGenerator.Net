using System;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvEmployment
    {
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public bool StillWorking { get; set; }
    }
}
