using IronPdf;

namespace CvGenerator.Logic
{
    public class HtmlToPdfConverter
    {
        public HtmlToPdf PdfRenderer { get; private set; }

        public HtmlToPdfConverter()
        {
            var options = new PdfPrintOptions
            {
                PaperSize = PdfPrintOptions.PdfPaperSize.A4,
                MarginTop = 15,
                MarginBottom = 15,
                MarginLeft = 15,
                MarginRight = 15
            };
            this.PdfRenderer = new HtmlToPdf(options);
        }

        public byte[] ConvertToPdf(string html)
        {
            var pdf = PdfRenderer.RenderHtmlAsPdf(html);
            return pdf.BinaryData;
        }
    }
}
