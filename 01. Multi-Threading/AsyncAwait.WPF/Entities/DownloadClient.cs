using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace AsyncAwait.WPF.Entities
{
    public class DownloadClient : INotifyPropertyChanged, IDisposable
    {
        private string _filePath;
        private string _status;
        private int _statusPercentage;
        private WebClient _client;

        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleted;

        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Url { get; }

        public string Status
        {
            get { return _status; }
            set { _status = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status))); }
        }

        public int StatusPercentage
        {
            get { return _statusPercentage; }
            set { _statusPercentage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusPercentage))); }
        }

        public DownloadClient(string url, string filePath)
        {
            Url = url;
            _filePath = filePath;

            _client = new WebClient();
            _client.DownloadFileCompleted += DownloadFileCompletedEventHandler;
            _client.DownloadProgressChanged += DownloadProgressChangedEventHandler;
        }

        public async Task Download()
        {
            await _client.DownloadFileTaskAsync(Url, _filePath);
        }

        public void Cancel()
        {
            _client.CancelAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private void DownloadFileCompletedEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted(this, e);
        }

        private void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged(this, e);
        }
    }
}
