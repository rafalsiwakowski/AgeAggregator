using AgeAggregator.Logic.Extensions;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using System;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Serialization
{
    abstract class PeopleDeserializerBase : IDeserializer<Person>
    {
        readonly IValidator<Person> _personValidator;

        public PeopleDeserializerBase(IValidator<Person> personValidator)
        {
            _personValidator = personValidator;
        }

        public IEnumerable<Person> DeserializeArray(string serializedArray)
        {
            try
            {
                var allPeople = DeserializeInternal(serializedArray);
                return allPeople.GetValidPeople(_personValidator);
            }
            catch(Exception ex)
            {
                return default(IEnumerable<Person>);
            }
        }

        protected abstract IEnumerable<Person> DeserializeInternal(string serializedArray);
    }
}
