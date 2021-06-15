using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CvGenerator.Models
{
    public class CvInformation : MongoEntity
    {
        public string PortraitImage { get; set; }

        [Required(ErrorMessage = "Your first name is required!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Your last name is required!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Your job title is required!")]
        public string JobTitle { get; set; }
        [Required]
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        [Required]
        public string LinkedinLink { get; set; }
        public string GithubLink { get; set; }
        public string PersonalLink { get; set; }
        public string SkypeId { get; set; }
        public string HackerRankLink { get; set; }

        public string QrCodeTitle { get; set; }
        public string QrCodeLink { get; set; }

        [BsonIgnore]
        public string QrCodeImage { get; set; }

        public string Objective { get; set; }

        public int Margin { get; set; } = 32;
        public int Scale { get; set; } = 100;
        public bool AgreePrivacy { get; set; }
        public bool AgreeSave { get; set; }
        public string ThemeColor { get; set; }
        public string PaperSize { get; set; }
        public string TemplateName { get; set; }

        public List<CvSkillSet> SkillSets { get; set; } = new List<CvSkillSet>();
        public List<CvLanguageSkill> Languages { get; set; } = new List<CvLanguageSkill>();
        public List<CvEmployment> Employments { get; set; } = new List<CvEmployment>();
        public List<CvEducation> Educations { get; set; } = new List<CvEducation>();
        public List<CvProject> Projects { get; set; } = new List<CvProject>();
    }
}
