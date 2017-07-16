using System.Collections.Generic;

namespace AgeAggregator.Logic.Serialization
{
    public interface IDeserializer<T>
    {
        IEnumerable<T> DeserializeArray(string serializedArray);
    }
}
