using System.Collections.Generic;

namespace AgeAggregator.Logic.Serialization
{
    public interface ISerializer<T>
    {
        string SerializeArray(IEnumerable<T> deserializedArray);
    }
}
