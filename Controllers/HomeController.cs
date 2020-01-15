using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CvGenerator.Models;
using CvGenerator.Logic;
using Microsoft.AspNetCore.Hosting;
using EasyMongoNet;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Text;

namespace CvGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbContext db;

        private readonly Dictionary<string, Template> templates;
        private readonly HtmlToPdfConverter converter;


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, IDbContext db, HtmlToPdfConverter converter, Dictionary<string, Template> templates)
        {
            this.logger = logger;
            this.env = env;
            this.db = db;
            this.converter = converter;
            this.templates = templates;
        }

        public IActionResult Index()
        {
            return View(new CvInformation());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CvInformation cv)
        {
            CleanupEmptyListItems(cv);
            var selectedTemplate = templates.Values.First();
            var html = selectedTemplate.Renderer.FillData(cv);
            byte[] pdfContent = await converter.ConvertToPdf(selectedTemplate.DirectoryPath, html, cv.Margin, cv.Scale / 100m);
            return File(pdfContent, "application/pdf", "cv.pdf");
        }

        [HttpPost]
        public IActionResult GetHtmlCv(CvInformation cv)
        {
            CleanupEmptyListItems(cv);
            var selectedTemplate = templates.Values.First();
            var html = selectedTemplate.Renderer.FillData(cv);
            return File(Encoding.UTF8.GetBytes(html), "text/html", "cv.html");
        }

        private void CleanupEmptyListItems(CvInformation cv)
        {
            foreach (var item in cv.Educations.Where(x => x.StartYear == 0 || x.Title == null).ToList())
                cv.Educations.Remove(item);
            foreach (var item in cv.Employments.Where(x => x.StartYear == 0 || x.JobTitle == null).ToList())
                cv.Employments.Remove(item);
            foreach (var item in cv.Languages.Where(x => x.Name == null).ToList())
                cv.Languages.Remove(item);
            foreach (var item in cv.Projects.Where(x => x.Year == 0 || x.Name == null).ToList())
                cv.Projects.Remove(item);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult GetEditorTemplatePartialView(string name) => PartialView("EditorTemplates/" + name);

        [HttpPost]
        public IActionResult AddEducation() => GetEditorTemplatePartialView(nameof(CvEducation));

        [HttpPost]
        public IActionResult AddEmployment() => GetEditorTemplatePartialView(nameof(CvEmployment));

        [HttpPost]
        public IActionResult AddLanguageSkill() => GetEditorTemplatePartialView(nameof(CvLanguageSkill));

        [HttpPost]
        public IActionResult AddProject() => GetEditorTemplatePartialView(nameof(CvProject));
    }
}
