using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using AsyncAwait.WPF.Entities;
using Microsoft.Win32;

namespace AsyncAwait.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<DownloadClient> _downloadClients;

        public MainWindow()
        {
            InitializeComponent();

            _downloadClients = new ObservableCollection<DownloadClient>();

            downloadingItems.ItemsSource = _downloadClients;
        }

        private async void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            var filePath = filePathLabel.Text;
            var url = urlTextBox.Text;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show("File path must not be empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("URL must not be empty");
                return;
            }

            var client = new DownloadClient(url, filePath);

            client.DownloadFileCompleted += DownloadFileCompleted;
            client.DownloadProgressChanged += DownloadProgressChanged;
            client.Status = Statuses.InProgress;

            _downloadClients.Add(client);

            try
            {
                await client.Download();
            }
            catch (Exception)
            {
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var downloadClient = button.DataContext as DownloadClient;

            downloadClient?.Cancel();
        }

        private void buttonSaveDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All Files (*.*)|*.*";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = filePathLabel.Text;

            if (saveFileDialog.ShowDialog() == true)
            {
                filePathLabel.Text = saveFileDialog.FileName;
            }
        }
        
        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            (sender as DownloadClient).StatusPercentage = e.ProgressPercentage;
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var client = sender as DownloadClient;

            if (e.Cancelled)
            {
                client.StatusPercentage = 0;
                client.Status = Statuses.Canceled;
                return;
            }

            if (e.Error != null)
            {
                client.StatusPercentage = 0;
                client.Status = Statuses.Faulted;
                return;
            }

            client.Status = Statuses.Done;
        }
    }
}
