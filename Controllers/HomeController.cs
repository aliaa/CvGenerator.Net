using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CvGenerator.Models;
using CvGenerator.Logic;
using Microsoft.AspNetCore.Hosting;
using EasyMongoNet;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using PuppeteerSharp.Media;

namespace CvGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IDbContext db;
        private readonly QrGenerator qrGenerator;

        private readonly Dictionary<string, Template> templates;
        private readonly HtmlToPdfConverter converter;


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, 
            IDbContext db, HtmlToPdfConverter converter, Dictionary<string, Template> templates, QrGenerator qrGenerator)
        {
            this.logger = logger;
            this.env = env;
            this.db = db;
            this.converter = converter;
            this.templates = templates;
            this.qrGenerator = qrGenerator;
        }

        public IActionResult Index()
        {
            return View(new CvInformation());
        }

        [HttpPost]
        public async Task<IActionResult> Preview(CvInformation cv)
        {
            byte[] pdfContent = await CreatePdf(cv);
            return Ok(Convert.ToBase64String(pdfContent));
        }

        [HttpPost]
        public async Task<IActionResult> Index(CvInformation cv)
        {
            if (!ModelState.IsValid)
                return View(cv);
            if (!cv.AgreePrivacy)
                return Error();
            if (cv.AgreeSave)
                db.Save(cv);
            byte[] pdfContent = await CreatePdf(cv);
            return File(pdfContent, "application/pdf", "cv.pdf");
        }

        private async Task<byte[]> CreatePdf(CvInformation cv)
        {
            CleanupEmptyListItems(cv);
            if (!string.IsNullOrWhiteSpace(cv.QrCodeLink))
            {
                cv.QrCodeImage = qrGenerator.GetPngBase64Encoded(cv.QrCodeLink);
            }
            var selectedTemplate = templates[cv.TemplateName];
            var html = selectedTemplate.Renderer.FillData(cv);
            var paperFormat = cv.PaperSize == "A4" ? PaperFormat.A4 : PaperFormat.Letter;
            return await converter.ConvertToPdf(null, html, paperFormat , cv.Margin, cv.Scale / 100m);
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

        public IActionResult AddEducation() => GetEditorTemplatePartialView(nameof(CvEducation));

        public IActionResult AddEmployment() => GetEditorTemplatePartialView(nameof(CvEmployment));

        public IActionResult AddLanguageSkill() => GetEditorTemplatePartialView(nameof(CvLanguageSkill));

        public IActionResult AddProject() => GetEditorTemplatePartialView(nameof(CvProject));
    }
}
