using System;
using System.Collections.Generic;
using System.Linq;

namespace CvGenerator.Models
{
    [Serializable]
    public class CvInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string LinkedinAddress { get; set; }
        public string GithubAddress { get; set; }
        public string PersonalAddress { get; set; }
        public string SkillsInAString { get; set; }
        public IEnumerable<string> Skills => SkillsInAString?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        public List<CvLanguageSkill> Languages { get; set; } = new List<CvLanguageSkill>();
        public List<CvEmployment> Employments { get; set; } = new List<CvEmployment>();
        public List<CvEducation> Educations { get; set; } = new List<CvEducation>();
        public List<CvProject> Projects { get; set; } = new List<CvProject>();
    }
}
