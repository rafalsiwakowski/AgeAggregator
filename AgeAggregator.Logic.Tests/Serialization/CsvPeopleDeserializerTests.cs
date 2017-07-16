using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using AgeAggregator.Logic.Validation;
using NUnit.Framework;
using System.Linq;

namespace AgeAggregator.Logic.Tests.Serialization
{
    class CsvPeopleDeserializerTests : PeopleDeserializationTestsBase<CsvPeopleDeserializer>
    {
        protected override CsvPeopleDeserializer CreateTestObject(IValidator<Person> personValidator)
        {
            return new CsvPeopleDeserializer(personValidator);
        }

        protected override void TestIgnoringInvalidEntries()
        {
            var inputWithInvalidEntry = @"first_name,last_name,country,age
Ryon,,Belarus,71
Elie,Mardall,Kazakhstan,invalid
Dorothy,Lanigan,American Samoa,70";

            var result = ObjectUnderTest.DeserializeArray(inputWithInvalidEntry);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Dorothy", result.First().FirstName);
            Assert.AreEqual("Lanigan", result.First().LastName);
            Assert.AreEqual("American Samoa", result.First().Country);
            Assert.AreEqual(70, result.First().Age);
        }
    }
}
