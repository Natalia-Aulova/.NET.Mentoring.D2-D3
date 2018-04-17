using System;
using System.Drawing;
using System.IO;
using System.Threading;
using NLog;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WindowsService.FileHandler.Helpers;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class PdfService : IPdfService
    {
        private readonly ILogger _logger;

        private readonly int _retryCount;
        private readonly int _retryDelay;

        public PdfService(ISettingsProvider settingsProvider, ILogger logger)
        {
            _logger = logger;

            _retryCount = int.Parse(settingsProvider.GetSetting("RetryCount"));
            _retryDelay = int.Parse(settingsProvider.GetSetting("RetryDelay")) * 1000;
        }

        public PdfDocument CreateDocument()
        {
            _logger.Debug("Creating new pdf document.");
            return new PdfDocument();
        }

        public void AddImage(string imagePath, PdfDocument document)
        {
            var bytes = new byte[8];

            OpenFileWithRetries(imagePath, stream =>
            {
                stream.Read(bytes, 0, 8);

                if (!bytes.IsImage())
                {
                    _logger.Warn($"The file {imagePath} is not an image. Skipping the file.");
                    return;
                }

                var page = document.AddPage();

                _logger.Debug($"Starting adding the image {imagePath}.");

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

                _logger.Debug($"The image {imagePath} has been added to the document.");
            });
        }

        public void SaveDocument(string destinatonPath, PdfDocument document)
        {
            if (document.PageCount > 0)
            {
                _logger.Debug($"Saving the document to {destinatonPath}.");
                document.Save(destinatonPath);
            }

            document.Close();
        }

        private void OpenFileWithRetries(string path, Action<Stream> action)
        {
            Stream stream = null;

            _logger.Debug($"Starting opening the file {path}.");

            for (int i = 0; i < _retryCount; i++)
            {
                try
                {
                    _logger.Debug($"Attempt {i}.");
                    stream = File.OpenRead(path);
                    break;
                }
                catch (IOException ex)
                {
                    _logger.Warn(ex, $"Unable to open the file {path}. Waiting for {_retryDelay} milliseconds.");
                    Thread.Sleep(_retryDelay);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    throw;
                }
            }

            if (stream == null)
            {
                var message = $"Unable to open the file {path}.";
                _logger.Error(message);
                throw new IOException(message);
            }

            _logger.Debug($"The file {path} has been opened.");

            action(stream);
            stream?.Dispose();
        }
    }
}
