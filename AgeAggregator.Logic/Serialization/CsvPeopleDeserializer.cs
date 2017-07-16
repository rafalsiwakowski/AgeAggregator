using System.IO;
using CsvHelper;
using System.Collections.Generic;
using CsvHelper.Configuration;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using System.Linq;

namespace AgeAggregator.Logic.Serialization
{
    class CsvPeopleDeserializer : PeopleDeserializerBase
    {
        public CsvPeopleDeserializer(IValidator<Person> personValidator) : base(personValidator)
        {
        }

        protected override IEnumerable<Person> DeserializeInternal(string serializedArray)
        {
            using (var reader = new StringReader(serializedArray))
            {
                var configuration = new CsvConfiguration
                {                    
                    IgnoreReadingExceptions = true
                };
                configuration.RegisterClassMap<PersonMap>();
                var csvReader = new CsvReader(reader, configuration);
                return csvReader.GetRecords<Person>().ToList();
            }
        }

        class PersonMap : CsvClassMap<Person>
        {
            public PersonMap()
            {
                Map(m => m.FirstName).Name("first_name");
                Map(m => m.LastName).Name("last_name");
                Map(m => m.Country).Name("country");
                Map(m => m.Age).Name("age").Default(0);
            }
        }
    }
}
