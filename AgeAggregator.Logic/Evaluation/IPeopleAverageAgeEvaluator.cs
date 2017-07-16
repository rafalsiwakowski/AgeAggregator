using AgeAggregator.Logic.Models;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Evaluation
{
    public interface IPeopleAverageAgeEvaluator
    {
        IEnumerable<AveragePeopleAgePerCountry> ComputePeopleAverageAge(IEnumerable<Person> people);
    }
}
