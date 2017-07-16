using System;
using System.IO;
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
}
