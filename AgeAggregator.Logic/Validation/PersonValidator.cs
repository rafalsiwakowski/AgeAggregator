using AgeAggregator.Logic.Models;

namespace AgeAggregator.Logic.Validation
{
    public interface IValidator<T>
    {
        bool IsValid(T instance);
    }

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
