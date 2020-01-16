using EasyMongoNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CvGenerator.Models
{
    public class CvInformation : MongoEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string LinkedinLink { get; set; }
        public string GithubLink { get; set; }
        public string PersonalLink { get; set; }
        public string SkillsInAString { get; set; }

        public int Margin { get; set; } = 32;
        public int Scale { get; set; } = 100;
        public bool AgreePrivacy { get; set; }
        public bool AgreeSave { get; set; }

        public List<string> Skills
        {
            get
            {
                if (string.IsNullOrEmpty(SkillsInAString))
                    return null;
                return SkillsInAString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }
        }
        
        public List<CvLanguageSkill> Languages { get; set; } = new List<CvLanguageSkill>();
        public List<CvEmployment> Employments { get; set; } = new List<CvEmployment>();
        public List<CvEducation> Educations { get; set; } = new List<CvEducation>();
        public List<CvProject> Projects { get; set; } = new List<CvProject>();
    }
}
