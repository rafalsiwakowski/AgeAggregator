using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AgeAggregator.Logic.Serialization
{
    static class JsonSerializationHelper
    {
        public static JsonSerializerSettings PrepareSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = (serializer, err) => err.ErrorContext.Handled = true,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }
    }
}
