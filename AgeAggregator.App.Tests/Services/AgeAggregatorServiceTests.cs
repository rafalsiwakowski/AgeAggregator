using AgeAggregator.App.Services;
using AgeAggregator.Logic.Cache;
using AgeAggregator.Logic.Evaluation;
using AgeAggregator.Logic.FileOperations;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using AgeAggregator.Logic.Utils;
using FakeItEasy;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace AgeAggregator.App.Tests.Services
{
    [TestFixture]
    public class AgeAggregatorServiceTests
    {
        AgeAggregatorService _objectUnderTest;

        IDirectoryHelper _directoryPathValidator;
        INotifier _notifier;
        IPathHelper _pathHelper;
        IDirectoryMonitor _directoryMonitor;
        IPeopleCache _peopleCache;
        IPeopleAverageAgeEvaluator _peopleAverageAgeEvaluator;
        ISerializer<AveragePeopleAgePerCountry> _averagePeopleAgePerCountrySerializer;
        IFileHelper _fileHelper;

        [SetUp]
        public void SetUp()
        {
            _directoryPathValidator = A.Fake<IDirectoryHelper>();
            _notifier = A.Fake<INotifier>();
            _pathHelper = A.Fake<IPathHelper>();
            _directoryMonitor = A.Fake<IDirectoryMonitor>();
            _peopleCache = A.Fake<IPeopleCache>();
            _peopleAverageAgeEvaluator = A.Fake<IPeopleAverageAgeEvaluator>();
            _averagePeopleAgePerCountrySerializer = A.Fake<ISerializer<AveragePeopleAgePerCountry>>();
            _fileHelper = A.Fake<IFileHelper>();
            _objectUnderTest = new AgeAggregatorService(_directoryPathValidator, _notifier, _pathHelper, _directoryMonitor, _peopleCache, _peopleAverageAgeEvaluator, _averagePeopleAgePerCountrySerializer, _fileHelper);
        }

        [Test]
        public void Should_NotifyAboutInvalidDirectoryPath_When_InvalidDirectoryPathProvided()
        {
            string directoryPath = "someDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(directoryPath)).Returns(false); 
            
            _objectUnderTest.RunAsync(directoryPath, null);

            A.CallTo(() => _notifier.NotifyUser("The directory someDirectoryPath is invalid or does not exist. Service shuts down.")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_RepresentNullAsTextInNotification_When_NullDirectoryPathProvided()
        {
            string notification = null;
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(null)).Returns(false);
            A.CallTo(() => _notifier.NotifyUser(A<string>.Ignored)).Invokes(call => notification = (string)call.Arguments[0]);

            _objectUnderTest.RunAsync(null, null);

            Assert.AreEqual("The directory <null> is invalid or does not exist. Service shuts down.", notification);
        }

        [Test]
        public void Should_RepresentNullAsTextInNotification_When_NullOutputFilePathProvided()
        {
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(null)).Returns(false);

            _objectUnderTest.RunAsync(null, null);

            A.CallTo(() => _notifier.NotifyUser("Cannot write on path <null>. Service shuts down.")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_NotifyAboutListeningAtDirectory_When_DirectoryIsValid()
        {
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";            
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath); 
            
            _objectUnderTest.RunAsync(existingDirectoryPath, null);

            A.CallTo(() => _notifier.NotifyUser("Service started listening at fullExistingDirectoryPath")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_InitializePeopleCache_When_DirectoryIsValid()
        {
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);

            _objectUnderTest.RunAsync(existingDirectoryPath, null);

            A.CallTo(() => _peopleCache.Initialize(fullExistingDirectoryPath)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_ComputePeopleAverageAge_When_InputDataIsValid()
        {
            IEnumerable<Person> people = Enumerable.Empty<Person>();
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath", outputFilePath = "outputFilePath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(outputFilePath)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            A.CallTo(() => _peopleCache.GetPeople()).Returns(people);

            _objectUnderTest.RunAsync(existingDirectoryPath, outputFilePath);

            A.CallTo(() => _peopleAverageAgeEvaluator.ComputePeopleAverageAge(people)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_SerializeResults_When_OutputIsCalculated()
        {
            IEnumerable<Person> people = Enumerable.Empty<Person>();
            IEnumerable<AveragePeopleAgePerCountry> results = Enumerable.Empty<AveragePeopleAgePerCountry>();
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath", outputFilePath = "outputFilePath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(outputFilePath)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            A.CallTo(() => _peopleCache.GetPeople()).Returns(people);
            A.CallTo(() => _peopleAverageAgeEvaluator.ComputePeopleAverageAge(people)).Returns(results);

            _objectUnderTest.RunAsync(existingDirectoryPath, outputFilePath);

            A.CallTo(() => _averagePeopleAgePerCountrySerializer.SerializeArray(results)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_SaveResults_When_OutputIsSerialized()
        {
            string serializedContent = "serialized";
            IEnumerable<Person> people = Enumerable.Empty<Person>();
            IEnumerable<AveragePeopleAgePerCountry> results = Enumerable.Empty<AveragePeopleAgePerCountry>();
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath", outputFilePath = "outputFilePath", fullOutputFilePath = "fullOutputFilePath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(outputFilePath)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            A.CallTo(() => _pathHelper.GetFullPath(outputFilePath)).Returns(fullOutputFilePath);
            A.CallTo(() => _peopleCache.GetPeople()).Returns(people);
            A.CallTo(() => _peopleAverageAgeEvaluator.ComputePeopleAverageAge(people)).Returns(results);
            A.CallTo(() => _averagePeopleAgePerCountrySerializer.SerializeArray(results)).Returns(serializedContent);

            _objectUnderTest.RunAsync(existingDirectoryPath, outputFilePath);

            A.CallTo(() => _fileHelper.SaveFile(fullOutputFilePath, serializedContent)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_Notify_When_ResultsAreSaved()
        {
            string serializedContent = "serialized";
            IEnumerable<Person> people = Enumerable.Empty<Person>();
            IEnumerable<AveragePeopleAgePerCountry> results = Enumerable.Empty<AveragePeopleAgePerCountry>();
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath", outputFilePath = "outputFilePath", fullOutputFilePath = "fullOutputFilePath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(outputFilePath)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            A.CallTo(() => _pathHelper.GetFullPath(outputFilePath)).Returns(fullOutputFilePath);
            A.CallTo(() => _peopleCache.GetPeople()).Returns(people);
            A.CallTo(() => _peopleAverageAgeEvaluator.ComputePeopleAverageAge(people)).Returns(results);
            A.CallTo(() => _averagePeopleAgePerCountrySerializer.SerializeArray(results)).Returns(serializedContent);

            _objectUnderTest.RunAsync(existingDirectoryPath, outputFilePath);

            A.CallTo(() => _notifier.NotifyUser("Results saved at fullOutputFilePath")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_StartMonitoringDirectory_When_DirectoryIsValid()
        {
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);          
            
            _objectUnderTest.RunAsync(existingDirectoryPath, null);

            A.CallTo(() => _directoryMonitor.StartMonitoringAsync(fullExistingDirectoryPath)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_NotifyUser_When_FileIsAddedToDirectory()
        {
            string newFileName = "newFileName";
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            var testMonitor = new DirectoryMonitorWhichAllowsToFireEventsFromOutside();
            _objectUnderTest = new AgeAggregatorService(_directoryPathValidator, _notifier, _pathHelper, testMonitor, _peopleCache, _peopleAverageAgeEvaluator, _averagePeopleAgePerCountrySerializer, _fileHelper);

            _objectUnderTest.RunAsync(existingDirectoryPath, null);
            Thread.Sleep(1000);
            testMonitor.ForceFileAdded(newFileName);

            A.CallTo(() => _notifier.NotifyUser("File newFileName has been added to the directory. Recalculating results...")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_IncludeNewFileInCache_When_FileIsAddedToDirectory()
        {
            string newFileName = "newFileName";
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            var testMonitor = new DirectoryMonitorWhichAllowsToFireEventsFromOutside();
            _objectUnderTest = new AgeAggregatorService(_directoryPathValidator, _notifier, _pathHelper, testMonitor, _peopleCache, _peopleAverageAgeEvaluator, _averagePeopleAgePerCountrySerializer, _fileHelper);

            _objectUnderTest.RunAsync(existingDirectoryPath, null);
            Thread.Sleep(1000);
            testMonitor.ForceFileAdded(newFileName);

            A.CallTo(() => _peopleCache.IncludeFiles(Path.Combine(fullExistingDirectoryPath, newFileName))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_NotifyUser_When_FileIsRemovedToDirectory()
        {
            string deletedFileName = "deletedFileName";
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            var testMonitor = new DirectoryMonitorWhichAllowsToFireEventsFromOutside();
            _objectUnderTest = new AgeAggregatorService(_directoryPathValidator, _notifier, _pathHelper, testMonitor, _peopleCache, _peopleAverageAgeEvaluator, _averagePeopleAgePerCountrySerializer, _fileHelper);

            _objectUnderTest.RunAsync(existingDirectoryPath, null);
            Thread.Sleep(1000);
            testMonitor.ForceFileRemoved(deletedFileName);

            A.CallTo(() => _notifier.NotifyUser("File deletedFileName has been removed from the directory. Recalculating results...")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Should_DeleteRemovedFileFromCache_When_FileIsRemovedFromDirectory()
        {
            string deletedFileName = "deletedFileName";
            string existingDirectoryPath = "existingFilePath", fullExistingDirectoryPath = "fullExistingDirectoryPath";
            A.CallTo(() => _directoryPathValidator.ValidateDirectoryPath(existingDirectoryPath)).Returns(true);
            A.CallTo(() => _pathHelper.CanWrite(A<string>.Ignored)).Returns(true);
            A.CallTo(() => _pathHelper.GetFullPath(existingDirectoryPath)).Returns(fullExistingDirectoryPath);
            var testMonitor = new DirectoryMonitorWhichAllowsToFireEventsFromOutside();
            _objectUnderTest = new AgeAggregatorService(_directoryPathValidator, _notifier, _pathHelper, testMonitor, _peopleCache, _peopleAverageAgeEvaluator, _averagePeopleAgePerCountrySerializer, _fileHelper);

            _objectUnderTest.RunAsync(existingDirectoryPath, null);
            Thread.Sleep(1000);
            testMonitor.ForceFileRemoved(deletedFileName);

            A.CallTo(() => _peopleCache.ExcludeFiles(Path.Combine(fullExistingDirectoryPath, deletedFileName))).MustHaveHappened(Repeated.Exactly.Once);
        }

        class DirectoryMonitorWhichAllowsToFireEventsFromOutside : IDirectoryMonitor
        {
            public event EventHandler<FileSystemEventArgs> FileAdded;
            public event EventHandler<FileSystemEventArgs> FileRemoved;

            string _directoryPath;

            public void StartMonitoring(string directoryPath)
            {
                _directoryPath = directoryPath;
            }

            public Task StartMonitoringAsync(string directoryPath)
            {
                _directoryPath = directoryPath;
                return new Task(() => StartMonitoring(directoryPath));
            }

            public void ForceFileAdded(string fileName)
            {
                FileAdded(this, new FileSystemEventArgs(WatcherChangeTypes.Created, _directoryPath, fileName));
            }

            public void ForceFileRemoved(string fileName)
            {
                FileRemoved(this, new FileSystemEventArgs(WatcherChangeTypes.Deleted, _directoryPath, fileName));
            }
        }
    }
}
