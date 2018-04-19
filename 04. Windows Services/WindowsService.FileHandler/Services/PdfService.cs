using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly string _brokenFolderName;

        public PdfService(ISettingsProvider settingsProvider, ILogger logger)
        {
            _logger = logger;

            _retryCount = int.Parse(settingsProvider.GetSetting("RetryCount"));
            _retryDelay = int.Parse(settingsProvider.GetSetting("RetryDelay")) * 1000;
            _brokenFolderName = settingsProvider.GetSetting("BrokenFolderName");
        }

        public PdfDocument CreateDocument()
        {
            _logger.Debug("Creating new pdf document.");
            return new PdfDocument();
        }

        public bool AddImage(string imagePath, PdfDocument document)
        {
            bool success = true;

            OpenFileWithRetries(imagePath, stream =>
            {
                var bytes = new byte[8];
                stream.Read(bytes, 0, 8);

                if (!bytes.IsImage())
                {
                    _logger.Warn($"The file {imagePath} is not an image. Skipping the file.");
                    success = false;
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

            return success;
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

        public async Task CopyBrokenSequenceToFolder(IList<string> sequence, string pdfFilePath, string destinationFolderPath, CancellationToken token)
        {
            await Task.Factory.StartNew(() =>
            {
                token.ThrowIfCancellationRequested();

                var pdfFileName = Path.GetFileNameWithoutExtension(pdfFilePath);
                var brokenFolderPath = Path.Combine(destinationFolderPath, _brokenFolderName, pdfFileName);

                if (!Directory.Exists(brokenFolderPath))
                {
                    Directory.CreateDirectory(brokenFolderPath);
                }

                File.Move(pdfFilePath, Path.Combine(brokenFolderPath, Path.GetFileName(pdfFilePath)));

                for (int i = 0; i < sequence.Count; i++)
                {
                    token.ThrowIfCancellationRequested();

                    var destinationPath = Path.Combine(brokenFolderPath, Path.GetFileName(sequence[i]));

                    _logger.Warn($"Broken sequence: copying {sequence[i]} to {destinationPath}.");

                    CopyFile(sequence[i], destinationPath);
                }
            }, token);
        }

        private void OpenFileWithRetries(string path, Action<Stream> action)
        {
            Stream stream = null;

            _logger.Debug($"Starting opening the file {path}.");

            for (int i = 0; i < _retryCount; i++)
            {
                try
                {
                    _logger.Debug($"{path} - attempt {i}.");
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

        private void CopyFile(string sourcePath, string destinationPath)
        {
            OpenFileWithRetries(sourcePath, source =>
            {
                using (var destination = File.Open(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    source.CopyTo(destination);
                }
            });
        }
    }
}
