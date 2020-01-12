﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvEducation
    {
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }

        [Display(Name = "End Year")]
        public int? EndYear { get; set; }
        public string Title { get; set; }
        public string University { get; set; }

        [Display(Name = "Still Studying Here")]
        public bool StillStudying { get; set; }
    }
}
