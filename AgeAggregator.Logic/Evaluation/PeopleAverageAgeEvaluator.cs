using AgeAggregator.Logic.Models;
using System.Collections.Generic;
using System.Linq;
using AgeAggregator.Logic.Cache;

namespace AgeAggregator.Logic.Evaluation
{
    public interface IPeopleAverageAgeEvaluator
    {
        IEnumerable<AveragePeopleAgePerCountry> ComputePeopleAverageAge(IEnumerable<Person> people);
    }

    class PeopleAverageAgeEvaluator : IPeopleAverageAgeEvaluator
    {
        public IEnumerable<AveragePeopleAgePerCountry> ComputePeopleAverageAge(IEnumerable<Person> people)
        {
            return people.GroupBy(x => x.Country).Select(x => new AveragePeopleAgePerCountry { Country = x.Key, AverageAge = (float)x.Average(p => p.Age) }).OrderBy(x => x.Country);
        }
    }
}
