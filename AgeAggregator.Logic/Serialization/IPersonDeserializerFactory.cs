using AgeAggregator.Logic.Models;

namespace AgeAggregator.Logic.Serialization
{
    public interface IPersonDeserializerFactory
    {
        IDeserializer<Person> CreateFor(string fileExtension);
    }
}
