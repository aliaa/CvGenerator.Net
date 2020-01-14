using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.IO;
using System.Threading.Tasks;

namespace CvGenerator.Logic
{
    public class HtmlToPdfConverter
    {
        private readonly Browser browser;
        private readonly PdfOptions pdfOptions;

        public HtmlToPdfConverter()
        {
            Task.Run(() => new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision)).Wait();
            browser = Task.Run(() => Puppeteer.LaunchAsync(new LaunchOptions { Headless = true })).Result;

            pdfOptions = new PdfOptions 
            {
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions 
                {
                    Left = "32px",
                    Right = "32px",
                    Top = "32px",
                    Bottom = "32px"
                },
                PrintBackground = true
            };
        }

        public async Task<byte[]> ConvertToPdf(string resPath, string html)
        {
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(resPath);
                await page.SetContentAsync(html);
                return await page.PdfDataAsync(pdfOptions);
            }
        }
    }
}
