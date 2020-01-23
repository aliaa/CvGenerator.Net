using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CvGenerator.Logic
{
    public class HtmlToPdfConverter
    {
        private readonly Browser browser;

        public HtmlToPdfConverter()
        {
            var downloadTask = Task.Run(() => new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision));
            downloadTask.Wait();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var path = Launcher.GetExecutablePath();
                Bash($"chmod 777 {path}");
            }
            browser = Task.Run(() => Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, Args = new string[] { "--no-sandbox" } })).Result;
        }

        private void Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        public async Task<byte[]> ConvertToPdf(string resPath, string html, PaperFormat paperFormat, int margin = 32, decimal scale = 1)
        {
            using (var page = await browser.NewPageAsync())
            {
                if(resPath != null)
                    await page.GoToAsync(resPath);
                await page.SetContentAsync(html);

                string marginStr = margin + "px";
                var pdfOptions = new PdfOptions
                {
                    Format = paperFormat,
                    MarginOptions = new MarginOptions
                    {
                        Left = marginStr,
                        Right = marginStr,
                        Top = marginStr,
                        Bottom = marginStr
                    },
                    Scale = scale,
                    PrintBackground = true,
                };

                return await page.PdfDataAsync(pdfOptions);
            }
        }
    }
}
