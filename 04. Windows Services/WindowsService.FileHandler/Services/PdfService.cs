using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private readonly string _destinationFolderPath;
        private readonly string _brokenFolderName;

        public PdfService(ISettingsProvider settingsProvider, ILogger logger)
        {
            _retryCount = settingsProvider.GetRetryCount();
            _retryDelay = settingsProvider.GetRetryDelay() * 1000;
            _destinationFolderPath = settingsProvider.GetDestinationFolderPath();
            _brokenFolderName = settingsProvider.GetBrokenFolderName();
            _logger = logger;
        }

        public async Task SaveDocument(IEnumerable<string> filePaths, string destinationFileName, CancellationToken token)
        {
            if (filePaths == null || !filePaths.Any())
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                _logger.Debug("Creating new pdf document.");

                using (var document = new PdfDocument())
                {
                    var success = true;

                    foreach (var fileName in filePaths)
                    {
                        if (!AddImage(fileName, document))
                        {
                            success = false;
                            break;
                        }
                    }

                    var destinatonFolderPath = _destinationFolderPath;

                    if (!success)
                    {
                        _logger.Warn($"The sequnce is broken. Saving it to the broken folder.");

                        destinatonFolderPath = Path.Combine(_destinationFolderPath, _brokenFolderName);
                        var brokenSequenceFolder = Path.Combine(destinatonFolderPath, destinationFileName);
                        CopyBrokenSequenceToFolder(filePaths, brokenSequenceFolder, token);
                    }

                    var destinatonPath = Path.Combine(destinatonFolderPath, string.Concat(destinationFileName, ".pdf"));
                    _logger.Debug($"Saving the document to {destinatonPath}.");
                    document.Save(destinatonPath);
                    document.Close();
                }
            }, token);
        }
        
        private bool AddImage(string imagePath, PdfDocument document)
        {
            bool success = true;

            OpenFileWithRetries(imagePath, stream =>
            {
                var bytes = new byte[8];
                stream.Read(bytes, 0, 8);

                if (!bytes.IsImage())
                {
                    _logger.Warn($"The file {imagePath} is not an image.");
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

        private void CopyBrokenSequenceToFolder(IEnumerable<string> filePaths, string brokenSequenceFolder, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!Directory.Exists(brokenSequenceFolder))
            {
                Directory.CreateDirectory(brokenSequenceFolder);
            }

            foreach(var filePath in filePaths)
            {
                token.ThrowIfCancellationRequested();

                var destinationPath = Path.Combine(brokenSequenceFolder, Path.GetFileName(filePath));

                _logger.Warn($"Broken sequence: copying {filePath} to {destinationPath}.");

                CopyFile(filePath, destinationPath);
            }
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
            stream.Dispose();
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
