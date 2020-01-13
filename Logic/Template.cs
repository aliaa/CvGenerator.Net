using System.IO;
using System;

namespace CvGenerator.Logic
{
    public class Template
    {
        public string Name { get; private set; }
        private CachedData<HtmlRenderer> RendererCache;

        public HtmlRenderer Renderer => RendererCache.Data;

        public Template(string folderPath)
        {
            Name = Path.GetFileName(folderPath);
            RendererCache = new CachedData<HtmlRenderer>(
                        RefreshFunc: (x) => new HtmlRenderer(File.ReadAllText(Path.Combine(folderPath, "index.html"))),
                        ExpireSeconds: 600);
        }
    }
}
