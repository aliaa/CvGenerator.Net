using CvGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using IronPdf;

namespace CvGenerator.Logic
{
    public class HtmlRenderer
    {
        private readonly string html;
        private StringBuilder sb = null;
        private static HtmlToPdf pdfRenderer = new HtmlToPdf(new PdfPrintOptions { PaperSize = PdfPrintOptions.PdfPaperSize.A4 });

        public HtmlRenderer(string templatePath)
        {
            html = File.ReadAllText(Path.Combine(templatePath, "index.html"));
        }

        public void FillData(object data)
        {
            if (sb == null)
                sb = new StringBuilder(html);
            Type type = data.GetType();
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(data);
                if(value != null)
                    sb.Replace("{" + prop.Name + "}", value.ToString());
            }
        }

        public byte[] ConvertToPdf()
        {
            var pdf = pdfRenderer.RenderHtmlAsPdf(sb.ToString());
            return pdf.Stream.ToArray();
        }
    }
}
