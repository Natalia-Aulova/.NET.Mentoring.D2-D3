using System;
using System.Drawing;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WindowsService.FileHandler.Helpers;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class PdfService : IPdfService
    {
        public PdfDocument CreateDocument()
        {
            return new PdfDocument();
        }

        public void AddImage(string imagePath, PdfDocument document)
        {
            var bytes = new byte[8];

            OpenFileWithRetries(imagePath, stream =>
            {
                stream.Read(bytes, 0, 8);

                if (!ImageTypeHelper.IsImage(bytes))
                {
                    return;
                }

                var page = document.AddPage();

                using (var xGraphics = XGraphics.FromPdfPage(page))
                {
                    using (var image = Image.FromStream(stream))
                    {
                        using (var xImage = XImage.FromGdiPlusImage(image))
                        {
                            xGraphics.DrawImage(xImage, 0, 0);
                        }
                    }
                }
            });
        }

        public void SaveDocument(string destinatonPath, PdfDocument document)
        {
            if (document.PageCount > 0)
            {
                document.Save(destinatonPath);
            }

            document.Close();
        }

        private void OpenFileWithRetries(string path, Action<Stream> action)
        {
            using (var fs = File.OpenRead(path))
            {
                action(fs);
            }
        }
    }
}
