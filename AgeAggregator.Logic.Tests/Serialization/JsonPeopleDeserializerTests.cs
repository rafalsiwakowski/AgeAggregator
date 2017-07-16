using AgeAggregator.Logic.Serialization;
using System.Linq;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using NUnit.Framework;

namespace AgeAggregator.Logic.Tests.Serialization
{
    class JsonPeopleDeserializerTests : PeopleDeserializationTestsBase<JsonPeopleDeserializer>
    {
        protected override JsonPeopleDeserializer CreateTestObject(IValidator<Person> personValidator)
        {
            return new JsonPeopleDeserializer(personValidator);
        }

        protected override void TestIgnoringInvalidEntries()
        {
            var inputWithInvalidEntry = @"[{""first_name"":""Pat"",""last_name"":""Silverthorne"",""country"":""France"",""age"":60},
                                             {""invalid"":""Glen"",""last_name"":""Oliff"",""country"":""Bangladesh"",""age"":34},
                                             {""first_name"":""Edvard"",""last_name"":""Ausher"",""country"":""Liberia"",""age"":92}]";

            var result = ObjectUnderTest.DeserializeArray(inputWithInvalidEntry);

            Assert.AreEqual(2, result.Count());
        }
    }
}
