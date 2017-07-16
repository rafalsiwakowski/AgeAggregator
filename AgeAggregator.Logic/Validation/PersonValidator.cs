using AgeAggregator.Logic.Models;

namespace AgeAggregator.Logic.Validation
{
    class PersonValidator : IValidator<Person>
    {
        public bool IsValid(Person instance)
        {
            return instance != null &&
                !string.IsNullOrWhiteSpace(instance.FirstName) &&
                !string.IsNullOrWhiteSpace(instance.LastName) &&
                !string.IsNullOrWhiteSpace(instance.Country) &&
                instance.Age >= 0;
        }
    }
}
