using System.IO;
using System;

namespace CvGenerator.Logic
{
    public class Template
    {
        public string Name { get; private set; }
        public string DirectoryPath { get; private set; }
        private CachedData<HtmlRenderer> RendererCache;

        public HtmlRenderer Renderer => RendererCache.Data;

        public Template(string directoryPath)
        {
            DirectoryPath = directoryPath;
            Name = Path.GetFileName(directoryPath);
            RendererCache = new CachedData<HtmlRenderer>(
                        RefreshFunc: (x) => new HtmlRenderer(File.ReadAllText(Path.Combine(directoryPath, "index.html"))),
                        ExpireSeconds: 1);
        }
    }
}
