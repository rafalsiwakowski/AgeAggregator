using System.IO;
using AgeAggregator.Logic.FileOperations;
using AgeAggregator.Logic.Utils;
using System.Threading.Tasks;
using AgeAggregator.Logic.Evaluation;
using AgeAggregator.Logic.Models;
using System.Collections.Generic;
using AgeAggregator.Logic.Cache;
using AgeAggregator.Logic.Serialization;

namespace AgeAggregator.App.Services
{
    class AgeAggregatorService
    {
        readonly IDirectoryHelper _directoryPathValidator;
        readonly INotifier _notifier;
        readonly IPathHelper _pathHelper;
        readonly IDirectoryMonitor _directoryMonitor;
        readonly IPeopleCache _peopleCache;
        readonly IPeopleAverageAgeEvaluator _peopleAverageAgeEvaluator;
        readonly ISerializer<AveragePeopleAgePerCountry> _averagePeopleAgePerCountrySerializer;
        readonly IFileHelper _fileHelper;

        public AgeAggregatorService(IDirectoryHelper directoryPathValidator,
            INotifier notifier,
            IPathHelper pathHelper,
            IDirectoryMonitor directoryMonitor,
            IPeopleCache peopleCache,
            IPeopleAverageAgeEvaluator peopleAverageAgeEvaluator,
            ISerializer<AveragePeopleAgePerCountry> averagePeopleAgePerCountrySerializer,
            IFileHelper fileHelper)
        {
            _directoryPathValidator = directoryPathValidator;
            _notifier = notifier;
            _pathHelper = pathHelper;
            _directoryMonitor = directoryMonitor;
            _peopleCache = peopleCache;
            _peopleAverageAgeEvaluator = peopleAverageAgeEvaluator;
            _averagePeopleAgePerCountrySerializer = averagePeopleAgePerCountrySerializer;
            _fileHelper = fileHelper;
        }

        public async Task RunAsync(string directoryPath, string outputFilePath)
        {
            if (!_directoryPathValidator.ValidateDirectoryPath(directoryPath))
            {
                _notifier.NotifyUser($"The directory {directoryPath ?? "<null>"} is invalid or does not exist. Service shuts down.");
                return;
            }
            if (!_pathHelper.CanWrite(outputFilePath))
            {
                _notifier.NotifyUser($"Cannot write on path {outputFilePath ?? "<null>"}. Service shuts down.");
                return;
            }
            var fullDirectoryPath = _pathHelper.GetFullPath(directoryPath);
            var fullOutputFilePath = _pathHelper.GetFullPath(outputFilePath);
            _notifier.NotifyUser($"Service started listening at {fullDirectoryPath}");
            _peopleCache.Initialize(fullDirectoryPath);
            SerializeResults(_peopleAverageAgeEvaluator.ComputePeopleAverageAge(_peopleCache.GetPeople()), fullOutputFilePath);
            await InitializeDirectoryMonitorAsync(fullDirectoryPath, fullOutputFilePath);
        }

        async Task InitializeDirectoryMonitorAsync(string directoryPath, string outputFilePath)
        {
            _directoryMonitor.FileAdded += (sender, args) => OnFileAdded(sender, args, outputFilePath);
            _directoryMonitor.FileRemoved += (sender, args) => OnFileRemoved(sender, args, outputFilePath);
            await _directoryMonitor.StartMonitoringAsync(directoryPath);
        }

        void OnFileAdded(object sender, FileSystemEventArgs e, string outputFilePath)
        {
            _notifier.NotifyUser($"File {e.Name} has been added to the directory. Recalculating results...");
            _peopleCache.IncludeFiles(e.FullPath);
            SerializeResults(_peopleAverageAgeEvaluator.ComputePeopleAverageAge(_peopleCache.GetPeople()), outputFilePath);
        }

        void OnFileRemoved(object sender, FileSystemEventArgs e, string outputFilePath)
        {
            _notifier.NotifyUser($"File {e.Name} has been removed from the directory. Recalculating results...");
            _peopleCache.ExcludeFiles(e.FullPath);
            SerializeResults(_peopleAverageAgeEvaluator.ComputePeopleAverageAge(_peopleCache.GetPeople()), outputFilePath);
        }

        void SerializeResults(IEnumerable<AveragePeopleAgePerCountry> averages, string outputFilePath)
        {
            var serialized = _averagePeopleAgePerCountrySerializer.SerializeArray(averages);
            _fileHelper.SaveFile(outputFilePath, serialized);
            _notifier.NotifyUser($"Results saved at {outputFilePath}");
        }
    }
}
