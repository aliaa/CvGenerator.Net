using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CvGenerator.Models;
using CvGenerator.Logic;
using Microsoft.AspNetCore.Hosting;
using EasyMongoNet;
using System.IO;

namespace CvGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbContext db;
        private readonly string templatesPath;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, IDbContext db)
        {
            this.logger = logger;
            this.env = env;
            this.db = db;
            this.templatesPath = Path.Combine(env.ContentRootPath, "CvTemplates");
        }

        public IActionResult Index()
        {
            return View(new CvInformation());
        }

        [HttpPost]
        public IActionResult Index(CvInformation cv)
        {
            string selectedTemplatePath = Path.Combine(templatesPath, "1");
            HtmlRenderer renderer = new HtmlRenderer(selectedTemplatePath);
            renderer.FillData(cv);
            byte[] pdfContent = renderer.ConvertToPdf();
            return File(pdfContent, "application/pdf", "cv.pdf");
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
