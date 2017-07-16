using AgeAggregator.Logic.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AgeAggregator.Logic.FileOperations
{
    public interface IDirectoryMonitor
    {
        event EventHandler<FileSystemEventArgs> FileAdded;
        event EventHandler<FileSystemEventArgs> FileRemoved;
        void StartMonitoring(string directoryPath);
        Task StartMonitoringAsync(string directoryPath);
    }

    class DirectoryMonitor : IDirectoryMonitor
    {
        readonly IPathHelper _fileHelper;

        public DirectoryMonitor(IPathHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public event EventHandler<FileSystemEventArgs> FileAdded;
        public event EventHandler<FileSystemEventArgs> FileRemoved;

        public void StartMonitoring(string directoryPath)
        {
            var watcher = new FileSystemWatcher
            {
                Path = directoryPath,
            };
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public async Task StartMonitoringAsync(string directoryPath)
        {
            await Task.Run(() => StartMonitoring(directoryPath));
        }

        void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                WaitUntilFileCreatesCompletelyInVeryUglyWay();
                FileAdded(this, e);
            }
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                FileRemoved(this, e);
            }
        }

        void WaitUntilFileCreatesCompletelyInVeryUglyWay()
        {
            Thread.Sleep(100);
        }
    }
}
