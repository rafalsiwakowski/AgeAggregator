using Newtonsoft.Json;
using System.Collections.Generic;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;

namespace AgeAggregator.Logic.Serialization
{
    class JsonPeopleDeserializer : PeopleDeserializerBase
    {
        public JsonPeopleDeserializer(IValidator<Person> personValidator) : base(personValidator)
        {
        }

        protected override IEnumerable<Person> DeserializeInternal(string serialized)
        {
            var settings = JsonSerializationHelper.PrepareSettings();
            return JsonConvert.DeserializeObject<IEnumerable<Person>>(serialized, settings);            
        }        
    }
}
