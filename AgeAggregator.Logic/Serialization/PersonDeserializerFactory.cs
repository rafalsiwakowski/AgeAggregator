using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using System;

namespace AgeAggregator.Logic.Serialization
{
    class PersonDeserializerFactory : IPersonDeserializerFactory
    {
        readonly Func<IValidator<Person>> _personValidatorFunc;

        public PersonDeserializerFactory(Func<IValidator<Person>> personValidatorFunc)
        {
            _personValidatorFunc = personValidatorFunc;
        }

        public IDeserializer<Person> CreateFor(string fileExtension)
        {
            if (string.Equals(".xml", fileExtension, StringComparison.OrdinalIgnoreCase))
                return new XmlPeopleDeserializer(_personValidatorFunc());
            if (string.Equals(".json", fileExtension, StringComparison.OrdinalIgnoreCase))
                return new JsonPeopleDeserializer(_personValidatorFunc());
            if (string.Equals(".csv", fileExtension, StringComparison.OrdinalIgnoreCase))
                return new CsvPeopleDeserializer(_personValidatorFunc());
            throw new ArgumentException($"File extension {fileExtension} not supported");
        }
    }
}
