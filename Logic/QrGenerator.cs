using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvGenerator.Logic
{
    public class QrGenerator
    {
        private readonly QRCodeGenerator generator;

        public QrGenerator()
        {
            generator = new QRCodeGenerator();
        }

        public string GetSvg(string content)
        {
            var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
            var svg = new SvgQRCode(data);
            return svg.GetGraphic(1);
        }

        public byte[] GetPng(string content)
        {
            var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
            var png = new PngByteQRCode(data);
            return png.GetGraphic(20);
        }

        public string GetPngBase64Encoded(string content)
        {
            var base64 = Convert.ToBase64String(GetPng(content));
            return "data:image/png;base64," + base64;
        }
    }
}
