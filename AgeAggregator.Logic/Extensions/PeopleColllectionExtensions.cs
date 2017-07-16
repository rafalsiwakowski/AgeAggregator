using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using System.Collections.Generic;
using System.Linq;

namespace AgeAggregator.Logic.Extensions
{
    static class PeopleColllectionExtensions
    {
        public static IEnumerable<Person> GetValidPeople(this IEnumerable<Person> people, IValidator<Person> validator)
        {
            return people.Where(x => validator.IsValid(x));
        }
    }
}
