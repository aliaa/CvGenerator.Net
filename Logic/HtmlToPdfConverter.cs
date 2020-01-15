using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.IO;
using System.Threading.Tasks;

namespace CvGenerator.Logic
{
    public class HtmlToPdfConverter
    {
        private readonly Browser browser;

        public HtmlToPdfConverter()
        {
            Task.Run(() => new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision)).Wait();
            browser = Task.Run(() => Puppeteer.LaunchAsync(new LaunchOptions { Headless = true })).Result;
        }

        public async Task<byte[]> ConvertToPdf(string resPath, string html, int margin = 32, decimal scale = 1)
        {
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(resPath);
                await page.SetContentAsync(html);

                string marginStr = margin + "px";
                var pdfOptions = new PdfOptions
                {
                    Format = PaperFormat.A4,
                    MarginOptions = new MarginOptions
                    {
                        Left = marginStr,
                        Right = marginStr,
                        Top = marginStr,
                        Bottom = marginStr
                    },
                    Scale = scale,
                    PrintBackground = true
                };

                return await page.PdfDataAsync(pdfOptions);
            }
        }
    }
}
