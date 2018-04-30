using System;
using System.Collections.Generic;
using System.Net;
using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using NLog;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class FileInfoMessageHandler : MessageHandler<FileInfoMessage>
    {
        private readonly IDownloadClientFactory _downloadFactory;
        private readonly List<IDownloadClient> _downloadClients;
        private readonly ILogger _logger;

        public FileInfoMessageHandler(IDownloadClientFactory downloadFactory, ILogger logger)
        {
            _downloadFactory = downloadFactory;
            _downloadClients = new List<IDownloadClient>();
            _logger = logger;
        }

        public override void Stop()
        {
            _downloadClients.ForEach(x => x.Cancel());
            _downloadClients.Clear();
        }

        protected override async void HandleMessage(FileInfoMessage message)
        {
            _logger.Debug($"File info message has been received. File path: {message.FilePath}");

            var client = _downloadFactory.GetClient($"file://{message.FilePath}");
            _downloadClients.Add(client);

            try
            {
                await client.Download();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {
                    _logger.Warn($"Downloading the file {message.FilePath} has been canceled.");
                    return;
                }

                _logger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
