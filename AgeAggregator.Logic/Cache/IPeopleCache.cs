using AgeAggregator.Logic.Models;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Cache
{
    public interface IPeopleCache
    {
        void Initialize(string directoryPath);
        void IncludeFiles(params string[] fileNames);
        void ExcludeFiles(params string[] fileNames);
        IEnumerable<Person> GetPeople();
    }
}
