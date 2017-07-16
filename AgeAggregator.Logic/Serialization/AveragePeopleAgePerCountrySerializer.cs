using AgeAggregator.Logic.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Serialization
{
    class AveragePeopleAgePerCountrySerializer : ISerializer<AveragePeopleAgePerCountry>
    {
        public string SerializeArray(IEnumerable<AveragePeopleAgePerCountry> deserializedArray)
        {
            var settings = JsonSerializationHelper.PrepareSettings();            
            return JsonConvert.SerializeObject(deserializedArray, settings);
        }
    }
}
