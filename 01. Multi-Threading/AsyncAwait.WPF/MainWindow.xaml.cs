using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AsyncAwait.WPF.Services;
using AsyncAwait.WPF.Entities;

namespace AsyncAwait.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDownloader _downloader;
        private readonly IList<DownloadTask> _downloadTasks;

        public MainWindow()
        {
            InitializeComponent();

            _downloader = new HttpDownloader();
            _downloadTasks = new List<DownloadTask>();
        }

        private async void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            var url = urlTextBox.Text;

            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var downloadTask = _downloader.Load(url, token);
            var downloadId = AddDownloadTaskToGrid(url);

            _downloadTasks.Add(new DownloadTask(downloadId, downloadTask, tokenSource));

            var statusBlockName = NameHelper.GetStatusTextBlockName(downloadId);
            var statusBlock = FindChild<TextBlock>(downloadingItems, statusBlockName);

            try
            {
                var content = await downloadTask;
                statusBlock.Text = Statuses.Done;
            }
            catch (OperationCanceledException)
            {
                statusBlock.Text = downloadTask.IsCanceled 
                    ? Statuses.Canceled 
                    : Statuses.Faulted;
            }
            catch (Exception)
            {
                statusBlock.Text = Statuses.Faulted;
            }
            finally
            {
                var cancelButtonName = NameHelper.GetCancelButtonName(downloadId);
                var cancelButton = FindChild<Button>(downloadingItems, cancelButtonName);
                downloadingItems.Children.Remove(cancelButton);

                tokenSource.Dispose();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            var downloadTaskId = NameHelper.GetIdFromCancelButtonName(((Button)sender).Name);

            _downloadTasks
                .FirstOrDefault(x => x.Id == downloadTaskId)
                ?.TokenSource
                ?.Cancel();
        }

        private int AddDownloadTaskToGrid(string url)
        {
            TextBlock urlBlock, statusBlock;
            Button cancelButton;

            RowDefinition gridRow;
            var currentNumber = downloadingItems.RowDefinitions.Count;
            
            gridRow = new RowDefinition();
            gridRow.Height = new GridLength(0, GridUnitType.Auto);
            downloadingItems.RowDefinitions.Add(gridRow);

            urlBlock = new TextBlock();
            urlBlock.Text = url;

            statusBlock = new TextBlock();
            statusBlock.Name = NameHelper.GetStatusTextBlockName(currentNumber);
            statusBlock.Text = Statuses.InProgress;

            var converter = new BrushConverter();

            cancelButton = new Button();
            cancelButton.Name = NameHelper.GetCancelButtonName(currentNumber);
            cancelButton.Content = "Cancel";
            cancelButton.Background = (Brush)converter.ConvertFrom("#FFFFFF");
            cancelButton.Foreground = (Brush)converter.ConvertFrom("#000000");
            cancelButton.Click += buttonCancel_Click;

            AddToGrid(urlBlock, currentNumber, 0);
            AddToGrid(statusBlock, currentNumber, 1);
            AddToGrid(cancelButton, currentNumber, 2);
            
            return currentNumber;
        }

        private void AddToGrid(UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            downloadingItems.Children.Add(element);
        }
        
        private T FindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            if (parent == null || string.IsNullOrEmpty(childName))
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else
                {
                    var frameworkElement = child as FrameworkElement;

                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
            }

            return foundChild;
        }
    }
}
