using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using AgeAggregator.Logic.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AgeAggregator.Logic.Cache
{
    public interface IPeopleCache
    {
        void Initialize(string directoryPath);
        void IncludeFiles(params string[] fileNames);
        void ExcludeFiles(params string[] fileNames);
        IEnumerable<Person> GetPeople();
    }

    class PeopleCache : IPeopleCache
    {
        static readonly string[] AllowedExtensions = new[] { ".xml", "json", ".csv" };

        readonly ConcurrentDictionary<string, IEnumerable<Person>> _fileNameBasedPeopleDictionary;
        readonly IPersonDeserializerFactory _personDeserializerFactory;
        readonly IDirectoryHelper _directoryHelper;
        readonly IFileHelper _fileHelper;
        readonly IPathHelper _pathHelper;

        public PeopleCache(IPersonDeserializerFactory personDeserializerFactory,
            IDirectoryHelper directoryHelper,
            IFileHelper fileHelper,
            IPathHelper pathHelper)
        {
            _fileNameBasedPeopleDictionary = new ConcurrentDictionary<string, IEnumerable<Person>>();
            _personDeserializerFactory = personDeserializerFactory;
            _directoryHelper = directoryHelper;
            _fileHelper = fileHelper;
            _pathHelper = pathHelper;
        }

        public IEnumerable<Person> GetPeople()
        {
            return _fileNameBasedPeopleDictionary.SelectMany(x => x.Value);
        }

        public void ExcludeFiles(params string[] fileNames)
        {
            Parallel.ForEach(fileNames, name =>
            {
                IEnumerable<Person> removedPeople;
                _fileNameBasedPeopleDictionary.TryRemove(name, out removedPeople);
            });
        }

        public void IncludeFiles(params string[] fileNames)
        {
            Parallel.ForEach(fileNames, name =>
            {
                var deserialized = _personDeserializerFactory.CreateFor(_pathHelper.GetExtension(name)).DeserializeArray(_fileHelper.ReadFile(name));
                if (deserialized != null)
                {
                    _fileNameBasedPeopleDictionary.TryAdd(name, deserialized);
                }
            });
        }

        public void Initialize(string directoryPath)
        {
            IncludeFiles(_directoryHelper.GetDirectoryFiles(directoryPath, AllowedExtensions));
        }
    }
}
